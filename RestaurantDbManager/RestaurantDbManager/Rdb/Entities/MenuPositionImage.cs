using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDbManager.Rdb.Entities;

//сущность "фотография позиции меню"
public class MenuPositionImage
{
    public int Id { get; set; }
    public byte[] Image { get; set; } = [];
    
    //внешний ключ
    public int MenuPositionId { get; set; }
    public MenuPosition? MenuPosition { get; set; }

    public MenuPositionImage() { }
}