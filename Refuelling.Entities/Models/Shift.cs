namespace Refuelling.Entities.Models;

public class Shift
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
    public bool IsOnDuty { get; set; } = true;
    public Employee? Employee { get; set; }
}