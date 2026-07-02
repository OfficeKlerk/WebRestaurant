using Azure.Core.Pipeline;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работы с заказами
public class OrderScenarios : EntityScenarios<Order>
{
    private readonly IOrderRepository _orderRepository;

    public OrderScenarios(IOrderRepository orderRepository) : base(orderRepository)
    {
        _orderRepository = orderRepository;
    }
}