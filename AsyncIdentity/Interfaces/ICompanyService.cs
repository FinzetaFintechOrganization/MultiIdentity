public interface ICompanyService
{
	Task<Guid> CreateCompanyAsync(CreateCompanyDTO dto);
	Task<bool> UpdateCompanyAsync(UpdateCompanyDTO dto);
	Task<CompanyDTO> GetCompanyByIdAsync(Guid id);
	Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync();
	Task<bool> DeleteCompanyAsync(Guid id);
}
