namespace RestaurantDbManager.Rdb.Entities;

//сущность "заказ"
public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string UserPhoneNumber { get; set; } = string.Empty;

    public Order() { }

}