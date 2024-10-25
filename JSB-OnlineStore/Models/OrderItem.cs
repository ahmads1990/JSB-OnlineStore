namespace JSB_OnlineStore.Models;

public class OrderItem
{
    public int OrderItemID { get; set; }
    public int ProductID { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
}
