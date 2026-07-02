namespace RestaurantDbManager.Rdb.Entities;

//сущность "заказ позиции меню"
public class MenuPositionOrder
{
    public int Id { get; set; }
    public int Count { get; set; }
    
    //внешние ключи
    public int MenuPositionId { get; set; }
    public MenuPosition? MenuPosition { get; set; }
    
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    
    public MenuPositionOrder() { }
}