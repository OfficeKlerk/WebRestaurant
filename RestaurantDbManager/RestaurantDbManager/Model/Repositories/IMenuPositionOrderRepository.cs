using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с заказами позиций меню
public interface IMenuPositionOrderRepository : IEntityRepository<MenuPositionOrder>
{
    
}