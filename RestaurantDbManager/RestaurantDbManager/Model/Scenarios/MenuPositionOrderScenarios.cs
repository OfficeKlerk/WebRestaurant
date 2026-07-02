using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работ с заказами позиций меню
public class MenuPositionOrderScenarios : EntityScenarios<MenuPositionOrder>
{
    private readonly IMenuPositionOrderRepository _menuPositionOrderRepository;

    public MenuPositionOrderScenarios(IMenuPositionOrderRepository menuPositionOrderRepository) : 
        base(menuPositionOrderRepository)
    {
        _menuPositionOrderRepository = menuPositionOrderRepository;
    }
}