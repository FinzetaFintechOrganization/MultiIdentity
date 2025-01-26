using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAuthentication("OAuth").AddJwtBearer("OAuth", cfg =>
{
    var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
    var key = new SymmetricSecurityKey(secretBytes);

    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = Constants.Issuer,
        ValidAudience = Constants.Audience,
        IssuerSigningKey = key
    };

    cfg.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Query.ContainsKey("access_token"))
            {
                context.Token = context.Request.Query["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

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
