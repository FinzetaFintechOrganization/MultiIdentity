using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing company-related operations.
/// </summary>
[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
	private readonly ICompanyService _companyService;

	/// <summary>
	/// Initializes a new instance of the <see cref="CompanyController"/> class.
	/// </summary>
	/// <param name="companyService">Service for handling company logic.</param>
	public CompanyController(ICompanyService companyService)
	{
		_companyService = companyService;
	}

	/// <summary>
	/// Retrieves all companies.
	/// </summary>
	/// <returns>A list of all companies.</returns>
	/// <response code="200">Returns the list of companies.</response>
	/// <response code="404">If no companies are found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet]
	public async Task<IActionResult> GetAllCompanies()
	{
		try
		{
			var companies = await _companyService.GetAllCompaniesAsync();
			if (companies == null || !companies.Any())
				return NotFound(new { Message = "No companies found." });

			return Ok(companies);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Retrieves a company by ID.
	/// </summary>
	/// <param name="id">The unique identifier of the company.</param>
	/// <returns>The details of the specified company.</returns>
	/// <response code="200">Returns the company details.</response>
	/// <response code="404">If the company is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetCompanyById(Guid id)
	{
		try
		{
			var company = await _companyService.GetCompanyByIdAsync(id);
			if (company == null)
				return NotFound(new { Message = $"Company with ID {id} not found." });

			return Ok(company);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Creates a new company.
	/// </summary>
	/// <param name="dto">The details of the company to be created.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="201">If the company is created successfully.</response>
	/// <response code="400">If the request is invalid.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPost]
	public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDTO dto)
	{
		try
		{
			var companyId = await _companyService.CreateCompanyAsync(dto);
			var createdCompany = await _companyService.GetCompanyByIdAsync(companyId);
			return CreatedAtAction(nameof(GetCompanyById), new { id = companyId }, createdCompany);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}


	/// <summary>
	/// Updates an existing company.
	/// </summary>
	/// <param name="id">The unique identifier of the company.</param>
	/// <param name="dto">The updated details of the company.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the company is updated successfully.</response>
	/// <response code="400">If the request is invalid.</response>
	/// <response code="404">If the company is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDTO dto)
	{
		try
		{
			if (id != dto.Id)
				return BadRequest(new { Message = "Company ID mismatch." });

			var result = await _companyService.UpdateCompanyAsync(dto);
			if (result)
				return Ok(new { Message = "Company updated successfully." });

			return NotFound(new { Message = $"Company with ID {id} not found." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}

	/// <summary>
	/// Deletes a company by ID.
	/// </summary>
	/// <param name="id">The unique identifier of the company to be deleted.</param>
	/// <returns>A success or failure message.</returns>
	/// <response code="200">If the company is deleted successfully.</response>
	/// <response code="404">If the company is not found.</response>
	/// <response code="500">If an unexpected error occurs.</response>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCompany(Guid id)
	{
		try
		{
			var result = await _companyService.DeleteCompanyAsync(id);
			if (result)
				return Ok(new { Message = "Company deleted successfully." });

			return NotFound(new { Message = $"Company with ID {id} not found." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
		}
	}
}
