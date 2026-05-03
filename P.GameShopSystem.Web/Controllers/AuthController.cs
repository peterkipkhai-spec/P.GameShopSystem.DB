using System.Net.Http.Json;
using P.GameShopSystem.Web.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace P.GameShopSystem.Web.Controllers;

public sealed class AuthController(IHttpClientFactory httpClientFactory) : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var client = httpClientFactory.CreateClient("GameShopApi");
        var response = await client.PostAsJsonAsync("api/auth/login", model, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseViewModel>(cancellationToken: cancellationToken);
        TempData["AuthMessage"] = $"Welcome back, {auth?.FullName}!";

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var client = httpClientFactory.CreateClient("GameShopApi");
        var response = await client.PostAsJsonAsync("api/auth/register", model, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Registration failed. Phone may already exist.");
            return View(model);
        }

        TempData["AuthMessage"] = "Registration successful. Please login.";
        return RedirectToAction(nameof(Login));
    }
}
