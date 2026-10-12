namespace Refuelling.Entities.Models;

public class Employee
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty; // New: Employee Contact Number
    public string Role { get; set; } = "Supplier"; // "Admin" or "Supplier"
    public string FuelTypeManaged { get; set; } = string.Empty;
    public string Password { get; set; } = "emp123";
    public bool IsActive { get; set; } = true;
}