public interface IAuthService
{
	Task<string> LoginAsync(LoginRequest request);
	Task<Guid> RegisterCompanyAsync(CreateCompanyDTO companyDto);
	Task<bool> RegisterUserAsync(CreateUserDTO userDto);
	Task<bool> RegisterAsync(RegisterRequest request);
}
