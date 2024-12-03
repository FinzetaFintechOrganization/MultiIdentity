using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Authentication controller for managing user login and registration.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly DbContext _dbContext;
	private readonly ICompanyService _companyService;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuthController"/> class.
	/// </summary>
	/// <param name="authService">Service for handling authentication logic.</param>
	/// <param name="dbContext">The database context for transactions.</param>
	/// <param name="companyService">Service for handling company-specific operations.</param>
	public AuthController(IAuthService authService, ApplicationDbContext dbContext, ICompanyService companyService)
	{
		_authService = authService;
		_dbContext = dbContext;
		_companyService = companyService;
	}

	/// <summary>
	/// Authenticates a user and returns a JWT token.
	/// </summary>
	/// <param name="request">The login request containing email and password.</param>
	/// <returns>A JWT token if authentication is successful.</returns>
	/// <response code="200">Returns the JWT token.</response>
	/// <response code="401">If the email or password is incorrect.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		try
		{
			// Attempt to authenticate the user
			var token = await _authService.LoginAsync(request);

			// Return the generated JWT token
			return Ok(new { Token = token });
		}
		catch (UnauthorizedAccessException ex)
		{
			// Return 401 Unauthorized if login credentials are invalid
			return Unauthorized(new { Message = ex.Message });
		}
		catch (Exception ex)
		{
			// Return 500 Internal Server Error for unexpected exceptions
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Registers a new user and their associated company.
	/// </summary>
	/// <param name="request">The registration request containing user and company details.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the user and company are registered successfully.</response>
	/// <response code="400">If either the user or company registration fails.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		try
		{
			var result = await _authService.RegisterAsync(request);
			if (result)
			{
				return Ok(new { Message = "User and company registered successfully." });
			}

			return BadRequest(new { Message = "Registration failed." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Message = ex.Message });
		}
	}

}
