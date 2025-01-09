using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing permissions and role-based assignments.
/// </summary>
[ApiController]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
	private readonly IPermissionService _permissionService;

	/// <summary>
	/// Initializes a new instance of the <see cref="PermissionController"/> class.
	/// </summary>
	/// <param name="permissionService">Service for handling permission-related logic.</param>
	public PermissionController(IPermissionService permissionService)
	{
		_permissionService = permissionService;
	}

	/// <summary>
	/// Retrieves all permissions in the system.
	/// </summary>
	/// <returns>A list of all permissions.</returns>
	/// <response code="200">Returns the list of permissions.</response>
	/// <response code="404">If no permissions are found.</response>
	[HttpGet]
	public async Task<IActionResult> GetAllPermissions()
	{
		var permissions = await _permissionService.GetAllPermissionsAsync();
		if (permissions == null || !permissions.Any())
			return NotFound("No permissions found.");

		return Ok(permissions);
	}

	/// <summary>
	/// Retrieves a specific permission by its ID.
	/// </summary>
	/// <param name="id">The ID of the permission.</param>
	/// <returns>The requested permission.</returns>
	/// <response code="200">Returns the requested permission.</response>
	/// <response code="400">If the ID is invalid.</response>
	/// <response code="404">If the permission is not found.</response>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetPermissionById([FromRoute] Guid id)
	{
		if (id == Guid.Empty)
			return BadRequest("Invalid permission ID.");

		try
		{
			var permission = await _permissionService.GetPermissionByIdAsync(id);
			return Ok(permission);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	/// <summary>
	/// Creates a new permission.
	/// </summary>
	/// <param name="dto">The details of the permission to create.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="201">If the permission is created successfully.</response>
	/// <response code="400">If the input is invalid.</response>
	/// <response code="500">If an error occurs while creating the permission.</response>
	[HttpPost]
	public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDTO dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		try
		{
			var result = await _permissionService.CreatePermissionAsync(dto);
			if (result)
				return CreatedAtAction(nameof(GetPermissionById), new { id = dto.ModuleName }, "Permission created successfully.");
			else
				return BadRequest("Failed to create permission.");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"An error occurred while creating the permission: {ex.Message}");
		}
	}

	/// <summary>
	/// Updates an existing permission.
	/// </summary>
	/// <param name="id">The ID of the permission to update.</param>
	/// <param name="dto">The updated details of the permission.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the permission is updated successfully.</response>
	/// <response code="400">If the ID or input is invalid.</response>
	/// <response code="404">If the permission is not found.</response>
	/// <response code="500">If an error occurs while updating the permission.</response>
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePermission([FromRoute] Guid id, [FromBody] UpdatePermissionDTO dto)
	{
		if (id == Guid.Empty || dto.Id != id)
			return BadRequest("Permission ID mismatch.");

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		try
		{
			var result = await _permissionService.UpdatePermissionAsync(dto);
			if (result)
				return Ok("Permission updated successfully.");
			else
				return BadRequest("Failed to update permission.");
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"An error occurred while updating the permission: {ex.Message}");
		}
	}

	/// <summary>
	/// Deletes a specific permission.
	/// </summary>
	/// <param name="id">The ID of the permission to delete.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the permission is deleted successfully.</response>
	/// <response code="400">If the ID is invalid.</response>
	/// <response code="404">If the permission is not found.</response>
	/// <response code="500">If an error occurs while deleting the permission.</response>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeletePermission([FromRoute] Guid id)
	{
		if (id == Guid.Empty)
			return BadRequest("Invalid permission ID.");

		try
		{
			var result = await _permissionService.DeletePermissionAsync(id);
			if (result)
				return Ok("Permission deleted successfully.");
			else
				return BadRequest("Failed to delete permission.");
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"An error occurred while deleting the permission: {ex.Message}");
		}
	}

	/// <summary>
	/// Assigns a permission to a role.
	/// </summary>
	/// <param name="dto">The details of the permission assignment.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the permission is assigned successfully.</response>
	/// <response code="400">If the input is invalid.</response>
	/// <response code="404">If the role or permission is not found.</response>
	/// <response code="409">If the permission is already assigned.</response>
	/// <response code="500">If an error occurs while assigning the permission.</response>
	[HttpPost("assign")]
	public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionDTO dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		try
		{
			var result = await _permissionService.AssignPermissionToRoleAsync(dto);
			if (result)
				return Ok("Permission assigned to role successfully.");
			else
				return BadRequest("Failed to assign permission to role.");
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(ex.Message);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"An error occurred while assigning permission: {ex.Message}");
		}
	}

	/// <summary>
	/// Retrieves all permissions assigned to a specific role.
	/// </summary>
	/// <param name="roleId">The ID of the role.</param>
	/// <returns>A list of permissions assigned to the role.</returns>
	/// <response code="200">Returns the list of permissions.</response>
	/// <response code="400">If the role ID is invalid.</response>
	/// <response code="404">If no permissions are found for the role.</response>
	/// <response code="500">If an error occurs while retrieving the permissions.</response>
	[HttpGet("role/{roleId}")]
	public async Task<IActionResult> GetPermissionsByRole([FromRoute] Guid roleId)
	{
		if (roleId == Guid.Empty)
			return BadRequest("Invalid role ID.");

		try
		{
			var permissions = await _permissionService.GetPermissionsByRoleAsync(roleId);
			if (permissions == null || !permissions.Any())
				return NotFound($"No permissions found for role ID {roleId}.");

			return Ok(permissions);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"An error occurred while retrieving permissions: {ex.Message}");
		}
	}
}
