using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class SecretController : Controller
{
    [Authorize]
    public string Index()
    {
        return "secret message";
    }
}