using Microsoft.EntityFrameworkCore;

public class CompanyService : ICompanyService
{
	private readonly ApplicationDbContext _context;

	public CompanyService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Guid> CreateCompanyAsync(CreateCompanyDTO companyDto)
	{
		var company = new Company
		{
			Id = Guid.NewGuid(),
			Name = companyDto.Name,
			PhoneNumber = companyDto.PhoneNumber,
			VKN = companyDto.VKN,
			IsTrial = companyDto.IsTrial,
			TrialEndDate = companyDto.TrialEndDate,
		};

		_context.Companies.Add(company);
		await _context.SaveChangesAsync();

		return company.Id;
	}

	public async Task<bool> UpdateCompanyAsync(UpdateCompanyDTO dto)
	{
		var company = await _context.Companies.FindAsync(dto.Id);
		if (company == null)
			throw new KeyNotFoundException("Company not found.");

		company.Name = dto.Name;
		company.PhoneNumber = dto.PhoneNumber;
		company.VKN = dto.VKN;
		company.SubscriptionEndDate = dto.SubscriptionEndDate;
		company.IsTrial = dto.IsTrial;
		company.TrialEndDate = dto.TrialEndDate;

		_context.Companies.Update(company);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<CompanyDTO> GetCompanyByIdAsync(Guid id)
	{
		var company = await _context.Companies.FindAsync(id);
		if (company == null)
			throw new KeyNotFoundException("Company not found.");

		return new CompanyDTO
		{
			Id = company.Id,
			Name = company.Name,
			PhoneNumber = company.PhoneNumber,
			VKN = company.VKN,
			CreatedDate = company.CreatedDate,
			SubscriptionEndDate = company.SubscriptionEndDate,
			IsTrial = company.IsTrial,
			TrialEndDate = company.TrialEndDate
		};
	}

	public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync()
	{
		return await _context.Companies
			.Select(company => new CompanyDTO
			{
				Id = company.Id,
				Name = company.Name,
				PhoneNumber = company.PhoneNumber,
				VKN = company.VKN,
				CreatedDate = company.CreatedDate,
				SubscriptionEndDate = company.SubscriptionEndDate,
				IsTrial = company.IsTrial,
				TrialEndDate = company.TrialEndDate
			})
			.ToListAsync();
	}

	public async Task<bool> DeleteCompanyAsync(Guid id)
	{
		var company = await _context.Companies.FindAsync(id);
		if (company == null)
			throw new KeyNotFoundException("Company not found.");

		_context.Companies.Remove(company);
		return await _context.SaveChangesAsync() > 0;
	}
}
