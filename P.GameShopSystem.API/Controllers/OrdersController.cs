using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.GameShopSystem.API.Contracts.Orders;
using P.GameShopSystem.DB.Models;
using P.GameShopSystem.Domain.Entities;

namespace P.GameShopSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(GameShopDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders.AsNoTracking()
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Game)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);

        return Ok(orders.Select(ToDto).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateAsync(CreateOrderRequestDto request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0) return BadRequest("Order must contain at least one item.");

        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == request.UserId, cancellationToken);
        if (!userExists) return BadRequest("Invalid user id.");

        var gameIds = request.Items.Select(i => i.GameId).Distinct().ToList();
        var games = await dbContext.Games.Where(g => gameIds.Contains(g.GameId) && g.IsActive)
            .ToDictionaryAsync(g => g.GameId, cancellationToken);

        if (games.Count != gameIds.Count) return BadRequest("One or more games are invalid or inactive.");

        var order = new Order
        {
            UserId = request.UserId,
            OrderStatus = OrderStatus.Pending,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in request.Items)
        {
            var game = games[item.GameId];
            if (item.Quantity <= 0 || game.StockQuantity < item.Quantity) return BadRequest($"Invalid quantity for game {game.Title}.");

            game.StockQuantity -= item.Quantity;
            order.OrderItems.Add(new OrderItem { GameId = game.GameId, Quantity = item.Quantity, UnitPrice = game.Price });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        await dbContext.Entry(order).Collection(o => o.OrderItems).LoadAsync(cancellationToken);
        foreach (var item in order.OrderItems)
        {
            await dbContext.Entry(item).Reference(i => i.Game).LoadAsync(cancellationToken);
        }

        return Ok(ToDto(order));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusRequestDto request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(o => o.OrderId == id, cancellationToken);
        if (order is null) return NotFound();

        var status = request.OrderStatus.Trim();
        if (!OrderStatus.All.Contains(status)) return BadRequest("Invalid order status.");

        order.OrderStatus = status;
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(ToDto(order));
    }

    private static OrderDto ToDto(Order order)
    {
        var items = order.OrderItems.Select(i =>
            new OrderItemDto(i.OrderItemId, i.GameId, i.Game?.Title ?? string.Empty, i.Quantity, i.UnitPrice, i.Quantity * i.UnitPrice)).ToList();

        return new OrderDto(order.OrderId, order.UserId, order.OrderDate, order.OrderStatus ?? OrderStatus.Pending, order.TotalAmount, items);
    }
}
