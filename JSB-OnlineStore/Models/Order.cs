namespace JSB_OnlineStore.Models;

public class Order
{
    public int OrderID { get; set; }
    // For this demo, I will just store a dummy value
    public int CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public float TotalAmount { get; set; }
    // Navs
    public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
}
