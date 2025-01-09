public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}