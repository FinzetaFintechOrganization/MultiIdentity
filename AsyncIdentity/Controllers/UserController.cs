using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing users.
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserController"/> class.
	/// </summary>
	/// <param name="userService">Service for handling user-related logic.</param>
	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	/// <summary>
	/// Retrieves all users.
	/// </summary>
	/// <returns>A list of all users.</returns>
	/// <response code="200">Returns the list of users.</response>
	/// <response code="404">If no users are found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet]
	public async Task<IActionResult> GetAllUsers()
	{
		try
		{
			var users = await _userService.GetAllUsersAsync();
			if (users == null || !users.Any())
				return NotFound(new { Message = "No users found." });

			return Ok(users);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Retrieves a specific user by ID.
	/// </summary>
	/// <param name="id">The ID of the user to retrieve.</param>
	/// <returns>The requested user.</returns>
	/// <response code="200">Returns the requested user.</response>
	/// <response code="404">If the user is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(Guid id)
	{
		try
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
				return NotFound(new { Message = $"User with ID {id} not found." });

			return Ok(user);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { Message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Creates a new user.
	/// </summary>
	/// <param name="userDto">The details of the user to create.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the user is created successfully.</response>
	/// <response code="400">If the input data is invalid.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost]
	public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDto)
	{
		try
		{
			var result = await _userService.CreateUserAsync(userDto);
			if (result)
				return Ok(new { Message = "User created successfully." });

			return BadRequest(new { Message = "User creation failed." });
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
	/// Deletes a specific user by ID.
	/// </summary>
	/// <param name="id">The ID of the user to delete.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the user is deleted successfully.</response>
	/// <response code="404">If the user is not found.</response>
	/// <response code="400">If the deletion fails.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		try
		{
			var result = await _userService.DeleteUserAsync(id);
			if (result)
				return Ok(new { Message = "User deleted successfully." });

			return BadRequest(new { Message = "User deletion failed." });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { Message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}
}
