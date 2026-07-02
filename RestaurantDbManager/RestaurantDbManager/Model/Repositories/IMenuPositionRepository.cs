using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с позициями меню
public interface IMenuPositionRepository : IEntityRepository<MenuPosition>
{
    //получение всех позиций меню по id категории меню
    Task<List<MenuPosition>> ListByMenuCategoryIdAsync(int id);
    
    //получение позиции меню по названию блюда
    Task<MenuPosition?> GetByNameAsync(string name);
}