using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IConfiguration _configuration;
	private readonly ICompanyService _companyService;
	private readonly ApplicationDbContext _dbContext;

	public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ICompanyService companyService, ApplicationDbContext dbContext)
	{
		_userManager = userManager;
		_configuration = configuration;
		_companyService = companyService;
		_dbContext = dbContext;
	}

	public async Task<string> LoginAsync(LoginRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
		{
			throw new UnauthorizedAccessException("Invalid email or password.");
		}

		return GenerateJwtToken(user);
	}

	public async Task<Guid> RegisterCompanyAsync(CreateCompanyDTO companyDto)
	{
		var companyId = await _companyService.CreateCompanyAsync(companyDto);
		return companyId;
	}

	public async Task<bool> RegisterUserAsync(CreateUserDTO userDto)
	{
		var user = new ApplicationUser
		{
			UserName = userDto.UserName,
			Email = userDto.Email,
			CompanyId = userDto.CompanyId
		};

		var result = await _userManager.CreateAsync(user, userDto.Password);
		return result.Succeeded;
	}

	public async Task<bool> RegisterAsync(RegisterRequest request)
	{
		await using var transaction = await _dbContext.Database.BeginTransactionAsync();

		try
		{
			// Check if VKN already exists
			var vknExists = await _dbContext.Set<Company>().AnyAsync(c => c.VKN == request.VKN);
			if (vknExists)
			{
				throw new Exception("A company with this VKN already exists.");
			}

			// Check if Email already exists
			var emailExists = await _userManager.FindByEmailAsync(request.Email) != null;
			if (emailExists)
			{
				throw new Exception("A user with this email address already exists.");
			}

			// Check if PhoneNumber already exists
			var phoneNumberExists = await _dbContext.Set<Company>().AnyAsync(c => c.PhoneNumber == request.PhoneNumber);
			if (phoneNumberExists)
			{
				throw new Exception("A company with this phone number already exists.");
			}

			// Step 1: Create the company
			var companyDto = new CreateCompanyDTO
			{
				Name = request.CompanyName,
				PhoneNumber = request.PhoneNumber,
				VKN = request.VKN,
				IsTrial = true,
				TrialEndDate = DateTime.UtcNow.AddMonths(1)
			};

			var companyId = await RegisterCompanyAsync(companyDto);
			if (companyId == Guid.Empty)
			{
				throw new Exception("Company registration failed.");
			}

			// Step 2: Create the user
			var userDto = new CreateUserDTO
			{
				UserName = request.Email,
				Email = request.Email,
				Password = request.Password,
				CompanyId = companyId
			};

			var userResult = await RegisterUserAsync(userDto);
			if (!userResult)
			{
				await _companyService.DeleteCompanyAsync(companyId);
				throw new Exception("User registration failed.");
			}

			await transaction.CommitAsync();
			return true;
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	private string GenerateJwtToken(ApplicationUser user)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
