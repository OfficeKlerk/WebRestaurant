namespace RestaurantDbManager.Rdb.Entities;

//сущность "категория меню"
public class MenuCategory
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public byte[] Image { get; set; } = [];
    
    public MenuCategory() { }
}