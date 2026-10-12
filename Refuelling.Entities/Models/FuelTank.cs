namespace Refuelling.Entities.Models;

public class FuelTank
{
    public int Id { get; set; }
    public string TankNumber { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty;
    public double CapacityLiters { get; set; } = 10000;
    public double CurrentStockLiters { get; set; } = 10000;
    public double DieselCapacityLiters { get; set; }
    public double DieselStockLiters { get; set; }
    public double HiOctaneCapacityLiters { get; set; }
    public double HiOctaneStockLiters { get; set; }
    public int? AssignedEmployeeId { get; set; }
}