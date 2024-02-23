using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace aspnet_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

public async Task<IActionResult> LoginCallback()
    {
        var authResult = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
        if (authResult?.Succeeded != true)
        {
            // Handle failed authentication
            return RedirectToAction("Index");
        }

        // Get the access token and refresh token
        var accessToken = authResult.Properties.GetTokenValue("access_token");
        var refreshToken = authResult.Properties.GetTokenValue("refresh_token");

        // Save the tokens to the user's session or database
        HttpContext.Session.SetString("access_token", accessToken);
        HttpContext.Session.SetString("refresh_token", refreshToken);

        // Redirect the user to the desired page
        return RedirectToAction("Privacy");
    }

}
