using Microsoft.EntityFrameworkCore;

public class TrialReminderService : BackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public TrialReminderService(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using var scope = _serviceScopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			var companiesToNotify = await dbContext.Companies
				.Where(c => c.IsTrial && c.TrialEndDate <= DateTime.UtcNow.AddDays(3))
				.ToListAsync();

			foreach (var company in companiesToNotify)
			{
				// E-posta gönderimi yapılır
				Console.WriteLine($"Trial for {company.Name} ends on {company.TrialEndDate}");
			}

			await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
		}
	}
}
