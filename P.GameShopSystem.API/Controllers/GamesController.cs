using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.GameShopSystem.API.Contracts.Games;
using P.GameShopSystem.DB.Models;
using P.GameShopSystem.Domain.Entities;

namespace P.GameShopSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(GameShopDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetAllAsync([FromQuery] int? categoryId, CancellationToken cancellationToken)
    {
        var query = dbContext.Games.AsNoTracking();
        if (categoryId.HasValue) query = query.Where(g => g.CategoryId == categoryId.Value);

        var games = await query.OrderBy(g => g.Title)
            .Select(g => new GameDto(g.GameId, g.Title, g.CategoryId, g.Price, g.StockQuantity, g.GameCondition, g.IsActive))
            .ToListAsync(cancellationToken);

        return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> CreateAsync(UpsertGameRequestDto request, CancellationToken cancellationToken)
    {
        var existsCategory = await dbContext.Categories.AnyAsync(c => c.CategoryId == request.CategoryId, cancellationToken);
        if (!existsCategory) return BadRequest("Invalid category id.");

        if (!string.IsNullOrWhiteSpace(request.GameCondition) && !GameCondition.All.Contains(request.GameCondition.Trim()))
        {
            return BadRequest("Invalid game condition.");
        }

        var game = new Game
        {
            Title = request.Title.Trim(),
            CategoryId = request.CategoryId,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            GameCondition = request.GameCondition?.Trim(),
            IsActive = request.IsActive
        };

        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new GameDto(game.GameId, game.Title, game.CategoryId, game.Price, game.StockQuantity, game.GameCondition, game.IsActive));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GameDto>> UpdateAsync(int id, UpsertGameRequestDto request, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games.FindAsync([id], cancellationToken);
        if (game is null) return NotFound();

        if (!string.IsNullOrWhiteSpace(request.GameCondition) && !GameCondition.All.Contains(request.GameCondition.Trim()))
        {
            return BadRequest("Invalid game condition.");
        }

        game.Title = request.Title.Trim();
        game.CategoryId = request.CategoryId;
        game.Price = request.Price;
        game.StockQuantity = request.StockQuantity;
        game.GameCondition = request.GameCondition?.Trim();
        game.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new GameDto(game.GameId, game.Title, game.CategoryId, game.Price, game.StockQuantity, game.GameCondition, game.IsActive));
    }
}
