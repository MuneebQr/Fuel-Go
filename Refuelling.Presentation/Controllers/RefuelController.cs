#pragma warning disable CS0105
using Microsoft.AspNetCore.Mvc;
using Refuelling.BLL.Services;
using Refuelling.Entities.Models;
#pragma warning restore CS0105

namespace Refuelling.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefuelController : ControllerBase
{
    private readonly RefuellingService _service;

    public RefuelController(RefuellingService service)
    {
        _service = service;
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _service.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("tanks")]
    public async Task<IActionResult> GetTanks()
    {
        var tanks = await _service.GetAllTanksAsync();
        return Ok(tanks);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _service.GetAllRequestsAsync();
        return Ok(orders);
    }

    [HttpGet("employees/{employeeId:int}/dashboard")]
    public async Task<IActionResult> GetEmployeeDashboard(int employeeId)
    {
        var dashboard = await _service.GetEmployeeDashboardAsync(employeeId);
        return dashboard == null
            ? NotFound(new { Message = "Employee portal was not found." })
            : Ok(dashboard);
    }

    [HttpPost("request")]
    public async Task<IActionResult> CreateRequest([FromBody] RefuelRequest request)
    {
        try
        {
            var result = await _service.CreateRefuelRequestAsync(request);
            return Ok(new { Message = "Refuelling Request Sent Successfully!", Data = result });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { Message = exception.Message });
        }
    }

    [HttpPost("shift/start")]
    public async Task<IActionResult> StartShift([FromQuery] int employeeId)
    {
        var shift = await _service.StartShiftAsync(employeeId);
        return Ok(new { Message = "Shift Started Successfully!", Data = shift });
    }

    [HttpPost("delivery/complete")]
    public async Task<IActionResult> CompleteDelivery([FromQuery] int requestId, [FromQuery] int tankId, [FromQuery] int employeeId)
    {
        var success = await _service.CompleteDeliveryAsync(requestId, tankId, employeeId);
        if (!success) return BadRequest(new { Message = "Delivery could not be completed. Check the assigned employee, tank, task status, and fuel stock." });
        return Ok(new { Message = "Delivery completed and stock deducted successfully!" });
    }

    [HttpPost("review")]
    public async Task<IActionResult> SubmitReview([FromBody] ReviewDto dto)
    {
        var success = await _service.SubmitReviewAsync(dto.RequestId, dto.Rating, dto.Comment);
        if (!success) return BadRequest(new { Message = "Failed to submit review." });
        return Ok(new { Message = "Review submitted successfully!" });
    }

    [HttpGet("employee-reviews/{employeeId}")]
    public async Task<IActionResult> GetEmployeeReviews(int employeeId)
    {
        var reviews = await _service.GetEmployeeReviewsAsync(employeeId);
        return Ok(reviews);
    }
}

public class ReviewDto
{
    public string Echo { get; set; } = string.Empty;
    public int RequestId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
[ApiController]
[Route("api/refuel")]
public class RefuelAnalyticsController : ControllerBase
{
    private readonly RefuellingService _refuellingService;

    public RefuelAnalyticsController(RefuellingService refuellingService)
    {
        _refuellingService = refuellingService;
    }

    [HttpGet("legacy/tanks")]
    public async Task<IActionResult> GetTanks()
    {
        var tanks = await _refuellingService.GetAllTanksAsync();
        return Ok(tanks);
    }

    [HttpGet("legacy/employees")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _refuellingService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("admin/employees-analytics")]
    public async Task<IActionResult> GetAdminEmployeesAnalytics()
    {
        var analytics = await _refuellingService.GetAdminEmployeesAnalyticsAsync();
        return Ok(analytics);
    }

    [HttpGet("employee-requests/{employeeId}")]
    public async Task<IActionResult> GetEmployeeRequests(int employeeId)
    {
        var requests = await _refuellingService.GetRequestsByEmployeeAsync(employeeId);
        return Ok(requests);
    }

    [HttpPost("legacy/request")]
    public async Task<IActionResult> CreateRequest([FromBody] RefuelRequest request)
    {
        var result = await _refuellingService.CreateRefuelRequestAsync(request);
        return Ok(new { Message = "Order confirmed successfully!", Data = result });
    }

    [HttpPost("legacy/review")]
    public async Task<IActionResult> SubmitReview([FromBody] LegacyReviewDto dto)
    {
        var success = await _refuellingService.SubmitReviewAsync(dto.RequestId, dto.Rating, dto.Comment);
        if (!success) return BadRequest(new { Message = "Failed to submit review." });
        return Ok(new { Message = "Review submitted successfully!" });
    }

    [HttpGet("legacy/employee-reviews/{employeeId}")]
    public async Task<IActionResult> GetEmployeeReviews(int employeeId)
    {
        var reviews = await _refuellingService.GetEmployeeReviewsAsync(employeeId);
        return Ok(reviews);
    }
}

public class LegacyReviewDto
{
    public int RequestId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}