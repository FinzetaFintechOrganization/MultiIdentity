using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing roles.
/// </summary>
[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
	private readonly IRoleService _roleService;

	/// <summary>
	/// Initializes a new instance of the <see cref="RoleController"/> class.
	/// </summary>
	/// <param name="roleService">Service for handling role-related logic.</param>
	public RoleController(IRoleService roleService)
	{
		_roleService = roleService;
	}

	/// <summary>
	/// Retrieves all roles.
	/// </summary>
	/// <returns>A list of all roles.</returns>
	/// <response code="200">Returns the list of roles.</response>
	/// <response code="404">If no roles are found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet]
	public async Task<IActionResult> GetAllRoles()
	{
		try
		{
			var roles = await _roleService.GetAllRolesAsync();
			if (roles == null || !roles.Any())
				return NotFound(new { Message = "No roles found." });

			return Ok(roles);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Retrieves a specific role by its ID.
	/// </summary>
	/// <param name="id">The ID of the role to retrieve.</param>
	/// <returns>The requested role.</returns>
	/// <response code="200">Returns the requested role.</response>
	/// <response code="404">If the role is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetRoleById(Guid id)
	{
		try
		{
			var role = await _roleService.GetRoleByIdAsync(id);
			if (role == null)
				return NotFound(new { Message = $"Role with ID {id} not found." });

			return Ok(role);
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
	/// Creates a new role.
	/// </summary>
	/// <param name="roleDto">The details of the role to create.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the role is created successfully.</response>
	/// <response code="400">If the role creation fails due to validation errors or conflicts.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost]
	public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO roleDto)
	{
		try
		{
			var result = await _roleService.CreateRoleAsync(roleDto);

			if (result)
				return Ok(new { Message = "Role created successfully." });

			return BadRequest(new { Message = "Role creation failed." });
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
	/// Deletes a specific role.
	/// </summary>
	/// <param name="id">The ID of the role to delete.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the role is deleted successfully.</response>
	/// <response code="400">If the role deletion fails due to conflicts.</response>
	/// <response code="404">If the role is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteRole(Guid id)
	{
		try
		{
			var result = await _roleService.DeleteRoleAsync(id);

			if (result)
				return Ok(new { Message = "Role deleted successfully." });

			return BadRequest(new { Message = "Role deletion failed." });
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
