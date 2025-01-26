using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = "ClientCookie";
    config.DefaultSignInScheme = "ClientCookie";
    config.DefaultChallengeScheme = "OurServer";
})
.AddCookie("ClientCookie")
.AddOAuth("OurServer", config =>
{
    config.CallbackPath = "/oauth/callback";
    config.ClientId = "client_id";
    config.ClientSecret = "client_secret";
    config.AuthorizationEndpoint = "http://localhost:5143/oauth/authorize";
    config.TokenEndpoint = "http://localhost:5143/oauth/token";
    config.SaveTokens = true;

    config.Events = new OAuthEvents
    {
        OnCreatingTicket = context =>
        {
            var accessToken = context.AccessToken;
            var payload = accessToken.Split('.')[1];
            var bytes = Convert.FromBase64String(payload);
            var jsonPayload = Encoding.UTF8.GetString(bytes);
            var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

            foreach (var claim in claims)
            {
                context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
