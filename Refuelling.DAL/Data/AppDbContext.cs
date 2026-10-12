using Microsoft.EntityFrameworkCore;
using Refuelling.Entities.Models;

namespace Refuelling.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<FuelTank> FuelTanks => Set<FuelTank>();
    public DbSet<RefuelRequest> RefuelRequests => Set<RefuelRequest>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();

        modelBuilder.Entity<RefuelRequest>()
            .HasOne(r => r.AssignedTank)
            .WithMany()
            .HasForeignKey(r => r.AssignedTankId)
            .OnDelete(DeleteBehavior.SetNull);

        // Employees with Unique Passwords
        var employees = new[]
        {
            new Employee { Id = 1, EmployeeCode = "AD-001", Name = "Muneeb", PhoneNumber = "0300-1111111", Role = "Admin", FuelTypeManaged = "All", Password = "Admin123" },
            new Employee { Id = 2, EmployeeCode = "EM-001", Name = "Ali", PhoneNumber = "0300-222222", Role = "Supplier", FuelTypeManaged = "Diesel", Password = "ali@123" },
            new Employee { Id = 3, EmployeeCode = "EM-002", Name = "Ahmad", PhoneNumber = "0300-222223", Role = "Supplier", FuelTypeManaged = "Diesel", Password = "ahmad@123" },
            new Employee { Id = 4, EmployeeCode = "EM-003", Name = "Mustafa", PhoneNumber = "0300-222224", Role = "Supplier", FuelTypeManaged = "Diesel", Password = "mustafa@123" },
            new Employee { Id = 5, EmployeeCode = "EM-004", Name = "Fahad", PhoneNumber = "0300-222225", Role = "Supplier", FuelTypeManaged = "Diesel", Password = "Fahad@123" },
            new Employee { Id = 6, EmployeeCode = "EM-005", Name = "Umair", PhoneNumber = "0300-222226", Role = "Supplier", FuelTypeManaged = "Diesel", Password = "Umair@123" },
            new Employee { Id = 7, EmployeeCode = "EM-006", Name = "Zubair", PhoneNumber = "0300-333337", Role = "Supplier", FuelTypeManaged = "Hi-Octane", Password = "Zubair@123" },
            new Employee { Id = 8, EmployeeCode = "EM-007", Name = "Touqeer", PhoneNumber = "0300-333338", Role = "Supplier", FuelTypeManaged = "Hi-Octane", Password = "Touqeer@123" },
            new Employee { Id = 9, EmployeeCode = "EM-008", Name = "Jawad", PhoneNumber = "0300-333339", Role = "Supplier", FuelTypeManaged = "Hi-Octane", Password = "Jawad@123" },
            new Employee { Id = 10, EmployeeCode = "EM-009", Name = "Zuhaib", PhoneNumber = "0300-3333310", Role = "Supplier", FuelTypeManaged = "Hi-Octane", Password = "Zuhaib@123" },
            new Employee { Id = 11, EmployeeCode = "EM-010", Name = "Shoaib", PhoneNumber = "0300-3333311", Role = "Supplier", FuelTypeManaged = "Hi-Octane", Password = "Shoaib@123" }
        };
        modelBuilder.Entity<Employee>().HasData(employees);

        var tanks = new List<FuelTank>();
        for (var i = 1; i <= 5; i++)
        {
            tanks.Add(new FuelTank
            {
                Id = i,
                TankNumber = $"Tank-D-{i:00}",
                FuelType = "Diesel",
                CapacityLiters = 10000,
                CurrentStockLiters = 10000,
                DieselCapacityLiters = 10000,
                DieselStockLiters = 10000
            });
        }

        for (var i = 1; i <= 5; i++)
        {
            tanks.Add(new FuelTank
            {
                Id = i + 5,
                TankNumber = $"Tank-H-{i:00}",
                FuelType = "Hybrid",
                CapacityLiters = 10000,
                CurrentStockLiters = 10000,
                DieselCapacityLiters = 5000,
                DieselStockLiters = 5000,
                HiOctaneCapacityLiters = 5000,
                HiOctaneStockLiters = 5000
            });
        }

        modelBuilder.Entity<FuelTank>().HasData(tanks);
    }
}