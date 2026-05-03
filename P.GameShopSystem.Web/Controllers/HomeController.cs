using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using P.GameShopSystem.Web.Models.Catalog;

namespace P.GameShopSystem.Web.Controllers;

public sealed class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("GameShopApi");
        var games = await client.GetFromJsonAsync<List<GameCardViewModel>>("api/games", cancellationToken) ?? [];

        return View(new HomeCatalogViewModel
        {
            Games = games.Where(g => g.IsActive).OrderBy(g => g.Title).ToList()
        });
    }
}
