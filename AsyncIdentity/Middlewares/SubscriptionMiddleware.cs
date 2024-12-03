using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class SubscriptionMiddleware
{
	private readonly RequestDelegate _next;

	public SubscriptionMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
	{
		try
		{
			// Kullanıcının kimlik bilgilerini alın
			var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
			{
				// Kullanıcı ve şirket bilgilerini veritabanından alın
				var user = await dbContext.Users
					.Include(u => u.Company)
					.FirstOrDefaultAsync(u => u.Id == userId);

				// Kullanıcının şirketinin abonelik süresi dolmuş mu kontrol edin
				if (user?.Company?.SubscriptionEndDate != null && user.Company.SubscriptionEndDate <= DateTime.UtcNow)
				{
					context.Response.StatusCode = StatusCodes.Status403Forbidden;
					await context.Response.WriteAsync("Subscription expired for the company.");
					return;
				}
			}

			// Bir sonraki middleware'e geçiş
			await _next(context);
		}
		catch (Exception ex)
		{
			// Hata durumunda bir yanıt döndür
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
		}
	}
}
