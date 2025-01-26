using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

public class OAuthController : Controller
{
    [HttpGet]
    public IActionResult Authorize(
        string response_type,
        string client_id,
        string redirect_uri,
        string scope,
        string state)
    {
        var query = new QueryBuilder
        {
            { "redirectUri", redirect_uri },
            { "state", state }
        };

        return View(model: query.ToString());
    }

    [HttpPost]
    public IActionResult Authorize(
        string username,
        string redirectUri,
        string state)
    {
        const string code = "sample_code_123";
        var query = new QueryBuilder
        {
            { "code", code },
            { "state", state }
        };

        return Redirect($"{redirectUri}{query.ToString()}");
    }

    [HttpPost]
    public async Task<IActionResult> Token(
        string grant_type,
        string code,
        string redirect_uri,
        string client_id)
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

        var access_token = new JwtSecurityTokenHandler().WriteToken(token);

        var responseObject = new
        {
            access_token,
            token_type = "Bearer"
        };

        var responseJson = JsonConvert.SerializeObject(responseObject);
        var responseBytes = Encoding.UTF8.GetBytes(responseJson);

        Response.ContentType = "application/json";
        await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

        return new EmptyResult();
    }
}
