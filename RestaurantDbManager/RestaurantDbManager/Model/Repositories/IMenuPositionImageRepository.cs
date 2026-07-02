using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с изображениями позиций из меню
public interface IMenuPositionImageRepository : IEntityRepository<MenuPositionImage>
{
    //получение всех записей сущностей по id позиции меню
    Task<List<MenuPositionImage>> ListByMenuPositionIdAsync(int id);
}