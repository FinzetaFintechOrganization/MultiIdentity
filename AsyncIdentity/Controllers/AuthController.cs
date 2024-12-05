using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Authentication controller for managing user login and registration.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">Service for handling authentication logic.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>A JWT token if authentication is successful.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request);
            return Ok(new ResponseModel<string>(true, "Login successful.", token));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ResponseModel<string>(false, ex.Message, null));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseModel<string>(false, "An unexpected error occurred.", ex.Message));
        }
    }

    /// <summary>
    /// Registers a new user and their associated company.
    /// </summary>
    /// <param name="request">The registration request containing user and company details.</param>
    /// <returns>A success or failure message.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            if (result)
            {
                return Ok(new ResponseModel<string>(true, "User and company registered successfully.", null));
            }

            return BadRequest(new ResponseModel<string>(false, "Registration failed.", null));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseModel<string>(false, ex.Message, null));
        }
    }
}
