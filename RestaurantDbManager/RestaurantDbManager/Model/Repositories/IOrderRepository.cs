using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с заказами
public interface IOrderRepository : IEntityRepository<Order>
{
    
}