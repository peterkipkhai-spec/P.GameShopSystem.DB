namespace P.GameShopSystem.API.Contracts.Orders;

public sealed record CreateOrderItemRequestDto(int GameId, int Quantity);

public sealed record CreateOrderRequestDto(int UserId, List<CreateOrderItemRequestDto> Items);

public sealed record OrderItemDto(int OrderItemId, int GameId, string GameTitle, int Quantity, decimal UnitPrice, decimal LineTotal);

public sealed record OrderDto(int OrderId, int UserId, DateTime OrderDate, string OrderStatus, decimal TotalAmount, List<OrderItemDto> Items);

public sealed record UpdateOrderStatusRequestDto(string OrderStatus);
