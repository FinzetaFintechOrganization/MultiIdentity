using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing subscriptions.
/// </summary>
[ApiController]
[Route("api/subscriptions")]
public class SubscriptionController : ControllerBase
{
	private readonly ISubscriptionService _subscriptionService;

	/// <summary>
	/// Initializes a new instance of the <see cref="SubscriptionController"/> class.
	/// </summary>
	/// <param name="subscriptionService">Service for handling subscription-related logic.</param>
	public SubscriptionController(ISubscriptionService subscriptionService)
	{
		_subscriptionService = subscriptionService;
	}

	/// <summary>
	/// Starts a new subscription for a company.
	/// </summary>
	/// <param name="dto">The details of the subscription to start.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the subscription is started successfully.</response>
	/// <response code="400">If the input data is invalid.</response>
	/// <response code="404">If the company is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost("start")]
	public async Task<IActionResult> StartSubscription([FromBody] SubscriptionDTO dto)
	{
		try
		{
			await _subscriptionService.StartSubscriptionAsync(dto);
			return Ok(new { Message = "Subscription started successfully." });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { Message = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new { Message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Extends an existing subscription for a company.
	/// </summary>
	/// <param name="dto">The details of the subscription to extend.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the subscription is extended successfully.</response>
	/// <response code="400">If the input data is invalid.</response>
	/// <response code="404">If the company is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost("extend")]
	public async Task<IActionResult> ExtendSubscription([FromBody] ExtendSubscriptionDTO dto)
	{
		try
		{
			await _subscriptionService.ExtendSubscriptionAsync(dto);
			return Ok(new { Message = "Subscription extended successfully." });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { Message = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new { Message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}
}
