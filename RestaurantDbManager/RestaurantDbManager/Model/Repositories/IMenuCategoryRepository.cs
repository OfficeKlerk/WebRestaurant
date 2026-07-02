using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с категорями меню
public interface IMenuCategoryRepository : IEntityRepository<MenuCategory>
{
    //получение категории меню по названию
    Task<MenuCategory?> GetByNameAsync(string name);
}