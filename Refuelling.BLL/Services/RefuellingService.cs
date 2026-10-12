using Microsoft.EntityFrameworkCore;
using Refuelling.DAL.Data;
using Refuelling.Entities.Models;

namespace Refuelling.BLL.Services;

public class RefuellingService
{
    private readonly AppDbContext _context;

    public RefuellingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.OrderBy(e => e.Id).ToListAsync();
    }

    public async Task<List<FuelTank>> GetAllTanksAsync()
    {
        return await _context.FuelTanks.OrderBy(t => t.Id).ToListAsync();
    }

    public async Task<List<RefuelRequest>> GetAllRequestsAsync()
    {
        return await _context.RefuelRequests
            .Include(r => r.AssignedEmployee)
            .Include(r => r.AssignedTank)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<List<RefuelRequest>> GetRequestsByEmployeeAsync(int employeeId)
    {
        return await _context.RefuelRequests
            .Where(request => request.AssignedEmployeeId == employeeId)
            .Include(request => request.AssignedTank)
            .OrderByDescending(request => request.RequestedAt)
            .ToListAsync();
    }

    public async Task<List<object>> GetAdminEmployeesAnalyticsAsync()
    {
        var employees = await _context.Employees
            .Where(employee => employee.Role == "Supplier")
            .OrderBy(employee => employee.Id)
            .ToListAsync();
        var requests = await GetAllRequestsAsync();
        var tanks = await GetAllTanksAsync();
        var analytics = new List<object>();

        foreach (var employee in employees)
        {
            var employeeRequests = requests
                .Where(request => request.AssignedEmployeeId == employee.Id)
                .ToList();
            var reviews = await GetEmployeeReviewsAsync(employee.Id);
            var activeRequest = employeeRequests.FirstOrDefault(request => request.Status != "Completed");
            var assignedTank = activeRequest?.AssignedTank
                ?? tanks.FirstOrDefault(tank => employee.FuelTypeManaged == "Diesel"
                    ? tank.FuelType == "Diesel" || tank.FuelType == "Hybrid"
                    : tank.FuelType == "Hybrid");

            analytics.Add(new
            {
                employee.Name,
                employee.EmployeeCode,
                employee.Role,
                employee.FuelTypeManaged,
                employee.PhoneNumber,
                DutyStatus = activeRequest == null ? "Free" : "In Task / Working",
                AssignedTankNumber = assignedTank?.TankNumber ?? "—",
                TankStockLiters = employee.FuelTypeManaged == "Diesel"
                    ? assignedTank?.DieselStockLiters ?? 0
                    : assignedTank?.HiOctaneStockLiters ?? 0,
                TotalEarningsRs = employeeRequests
                    .Where(request => request.Status == "Completed")
                    .Sum(request => request.QuantityLiters * (request.FuelType == "Diesel" ? 396.24 : 410.00) + 500),
                TotalTasksCompleted = employeeRequests.Count(request => request.Status == "Completed"),
                TotalTasksAssigned = employeeRequests.Count,
                AverageRating = reviews.Count == 0 ? 0 : reviews.Average(review => review.Rating),
                TotalReviewsCount = reviews.Count,
                Reviews = reviews
            });
        }

        return analytics;
    }

    public async Task<object?> GetEmployeeDashboardAsync(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null || employee.Role != "Supplier") return null;

        var today = DateTime.Today;
        var requests = await _context.RefuelRequests
            .Where(r => r.AssignedEmployeeId == employeeId)
            .Include(r => r.AssignedTank)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        var tanks = await _context.FuelTanks
            .Where(t => employee.FuelTypeManaged == "Diesel"
                ? t.FuelType == "Diesel" || t.FuelType == "Hybrid"
                : t.FuelType == "Hybrid")
            .OrderBy(t => t.Id)
            .ToListAsync();

        return new
        {
            Employee = employee,
            DailyCompletedTasks = requests.Count(r => r.Status == "Completed" && r.CompletedAt?.Date == today),
            PendingTasks = requests.Where(r => r.Status is not ("Completed" or "Cancelled")).ToList(),
            CompletedTasks = requests.Where(r => r.Status == "Completed").ToList(),
            Tanks = tanks
        };
    }

    public async Task<object> CreateRefuelRequestAsync(RefuelRequest request)
    {
        if (request.FuelType is not ("Diesel" or "Hi-Octane"))
        {
            throw new InvalidOperationException("Only Diesel and Hi-Octane orders are currently available.");
        }
        if (request.QuantityLiters <= 0 || double.IsNaN(request.QuantityLiters) || double.IsInfinity(request.QuantityLiters))
        {
            throw new InvalidOperationException("Fuel quantity must be greater than zero.");
        }

        request.Status = "Assigned";
        request.RequestedAt = DateTime.Now;

        var managedFuel = request.FuelType;
        var employees = await _context.Employees
            .Where(e => e.Role == "Supplier" && e.IsActive && e.FuelTypeManaged == managedFuel)
            .OrderBy(e => e.Id)
            .ToListAsync();

        var assignedCounts = await _context.RefuelRequests
            .Where(r => r.Status != "Completed" && r.Status != "Cancelled" && r.AssignedEmployeeId != null)
            .GroupBy(r => r.AssignedEmployeeId!.Value)
            .Select(group => new { EmployeeId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(group => group.EmployeeId, group => group.Count);

        var employee = employees
            .OrderBy(e => assignedCounts.GetValueOrDefault(e.Id))
            .ThenBy(e => e.Id)
            .FirstOrDefault();

        if (employee == null)
        {
            throw new InvalidOperationException("No active employee is available for this fuel type.");
        }

        request.AssignedEmployeeId = employee.Id;

        // Tank assignment and stock deduction logic
        var tank = await _context.FuelTanks
            .Where(t => request.FuelType == "Diesel"
                ? t.FuelType == "Diesel" || t.FuelType == "Hybrid"
                : t.FuelType == "Hybrid")
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync();

        if (tank != null)
        {
            request.AssignedTankId = tank.Id;
            tank.CurrentStockLiters -= request.QuantityLiters;
        }

        _context.RefuelRequests.Add(request);
        await _context.SaveChangesAsync();

        return new
        {
            Message = "Request created and assigned successfully.",
            Request = request,
            AssignedEmployee = new { employee.Name, employee.PhoneNumber },
            AssignedTank = tank?.TankNumber ?? "Tank-General"
        };
    }

    public async Task<Shift> StartShiftAsync(int employeeId)
    {
        var shift = new Shift
        {
            EmployeeId = employeeId,
            StartTime = DateTime.Now,
            IsOnDuty = true
        };

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<bool> CompleteDeliveryAsync(int requestId, int tankId, int employeeId)
    {
        var request = await _context.RefuelRequests.FindAsync(requestId);
        var tank = await _context.FuelTanks.FindAsync(tankId);
        if (request == null || tank == null || request.Status == "Completed"
            || request.AssignedEmployeeId != employeeId || request.AssignedTankId != tankId)
        {
            return false;
        }

        var stock = request.FuelType == "Diesel" ? tank.DieselStockLiters : tank.HiOctaneStockLiters;
        if (stock < request.QuantityLiters) return false;

        if (request.FuelType == "Diesel")
        {
            tank.DieselStockLiters -= request.QuantityLiters;
        }
        else
        {
            tank.HiOctaneStockLiters -= request.QuantityLiters;
        }

        tank.CurrentStockLiters -= request.QuantityLiters;
        request.Status = "Completed";
        request.CompletedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitReviewAsync(int requestId, int rating, string comment)
    {
        var request = await _context.RefuelRequests.FindAsync(requestId);
        if (request == null || !request.AssignedEmployeeId.HasValue || rating is < 1 or > 5) return false;

        _context.Reviews.Add(new Review
        {
            RefuelRequestId = requestId,
            EmployeeId = request.AssignedEmployeeId.Value,
            CustomerName = request.CustomerName,
            Rating = rating,
            Comment = comment
        });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Review>> GetEmployeeReviewsAsync(int employeeId)
    {
        return await _context.Reviews
            .Where(review => review.EmployeeId == employeeId)
            .OrderByDescending(review => review.CreatedAt)
            .ToListAsync();
    }
}