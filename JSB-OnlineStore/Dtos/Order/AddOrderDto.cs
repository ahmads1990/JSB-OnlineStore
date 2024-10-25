using JSB_OnlineStore.Dtos.OrderItem;

namespace JSB_OnlineStore.Dtos.Order;

public class AddOrderDto
{
    public int CustomerID { get; set; }
    public IEnumerable<AddOrderItemDto> Items { get; set; } = new List<AddOrderItemDto>();
}
