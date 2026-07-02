using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDbManager.Rdb.Entities;

//сущность "позиция меню"
public class MenuPosition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Weight { get; set; }
    
    //внешний ключ
    public int MenuCategoryId { get; set; }
    public MenuCategory? MenuCategory { get; set; }
    
    public MenuPosition() { }
}