namespace Refuelling.Entities.Models;

public class RefuelRequest
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CnicNumber { get; set; } = string.Empty;     // New: Customer CNIC
    public string CarNumber { get; set; } = string.Empty;      // New: Car / Vehicle Number
    public string Location { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty; // Diesel / Hi-Octane
    public double QuantityLiters { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Assigned, Completed
    public int? AssignedEmployeeId { get; set; }
    public Employee? AssignedEmployee { get; set; }
    public int? AssignedTankId { get; set; }
    public FuelTank? AssignedTank { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
}