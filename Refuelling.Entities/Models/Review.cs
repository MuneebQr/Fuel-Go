namespace Refuelling.Entities.Models;

public class Review
{
    public int Id { get; set; }
    public int RefuelRequestId { get; set; }
    public int EmployeeId { get; set; } // Jis employee ko rating mili
    public string CustomerName { get; set; } = string.Empty;
    public int Rating { get; set; } // 1 to 5 stars
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}