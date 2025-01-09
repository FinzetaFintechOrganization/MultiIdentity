using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AuthorizationMiddleware
{
	private readonly RequestDelegate _next;

	public AuthorizationMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
	{
		var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userId != null)
		{
			var user = await dbContext.Users
				.Include(u => u.Company)
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.ThenInclude(r => r.PermissionRoles)
				.ThenInclude(pr => pr.Permission)
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

			if (user == null)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return;
			}

			var routePath = context.Request.Path.Value.ToLower();
			var method = context.Request.Method.ToUpper();

			var hasPermission = user.UserRoles
				.SelectMany(ur => ur.Role.PermissionRoles)
				.Any(pr => pr.Permission.ModuleName.ToLower() == routePath && pr.Permission.Action.ToUpper() == method);

			if (!hasPermission)
			{
				context.Response.StatusCode = StatusCodes.Status403Forbidden;
				await context.Response.WriteAsync("Access Denied");
				return;
			}
		}

		await _next(context);
	}
}
