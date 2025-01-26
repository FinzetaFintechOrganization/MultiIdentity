using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Secret()
    {
        return View();
    }

    public IActionResult Authenticate()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "user_id"),
            new Claim("username", "user_name")
        };

        var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
        var key = new SymmetricSecurityKey(secretBytes);
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials
        );

        var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { access_token = tokenJson });
    }
}
