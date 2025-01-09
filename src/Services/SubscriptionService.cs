public class SubscriptionService : ISubscriptionService
{
	private readonly ApplicationDbContext _context;

	public SubscriptionService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task StartSubscriptionAsync(SubscriptionDTO dto)
	{
		// Şirketin mevcut olup olmadığını kontrol edin
		var company = await _context.Companies.FindAsync(dto.CompanyId);
		if (company == null)
		{
			throw new KeyNotFoundException($"Company with ID {dto.CompanyId} not found.");
		}

		// Yeni abonelik başlat
		company.SubscriptionEndDate = dto.EndDate;
		company.IsTrial = dto.IsTrial;

		// Abonelik geçmişine kaydet
		var subscriptionHistory = new SubscriptionHistory
		{
			Id = Guid.NewGuid(),
			CompanyId = dto.CompanyId,
			StartDate = DateTime.UtcNow,
			EndDate = dto.EndDate,
			Price = dto.Price,
			IsTrial = dto.IsTrial
		};

		_context.SubscriptionHistories.Add(subscriptionHistory);
		await _context.SaveChangesAsync();
	}

	public async Task ExtendSubscriptionAsync(ExtendSubscriptionDTO dto)
	{
		// Şirketin mevcut olup olmadığını kontrol edin
		var company = await _context.Companies.FindAsync(dto.CompanyId);
		if (company == null)
		{
			throw new KeyNotFoundException($"Company with ID {dto.CompanyId} not found.");
		}

		// Abonelik tarihini uzat
		company.SubscriptionEndDate = dto.NewEndDate;

		// Abonelik geçmişine kaydet
		var subscriptionHistory = new SubscriptionHistory
		{
			Id = Guid.NewGuid(),
			CompanyId = dto.CompanyId,
			StartDate = DateTime.UtcNow,
			EndDate = dto.NewEndDate,
			Price = 0, // Uzatmada fiyat sıfır olabilir veya DTO'dan alınabilir
			IsTrial = company.IsTrial
		};

		_context.SubscriptionHistories.Add(subscriptionHistory);
		await _context.SaveChangesAsync();
	}
}
