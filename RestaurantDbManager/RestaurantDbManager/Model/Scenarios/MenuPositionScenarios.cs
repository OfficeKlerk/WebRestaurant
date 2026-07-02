using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работы с позициями меню
public class MenuPositionScenarios : EntityScenarios<MenuPosition>
{
    private readonly IMenuPositionRepository _menuPositionRepository;
    private readonly IMenuCategoryRepository _menuCategoryRepository;

    public MenuPositionScenarios(IMenuPositionRepository menuPositionRepository,
        IMenuCategoryRepository menuCategoryRepository) : base(menuPositionRepository)
    {
        _menuPositionRepository = menuPositionRepository;
        _menuCategoryRepository = menuCategoryRepository;
    }
    
    //ListByMenuCategoryIdAsync - метод получения всех записей позиций меню по id категории меню
    //вход: id категории меню
    //выход: список с объектами MenuPosition
    //исключения:
    //1. EntityNotFoundException - по переданному id не была найдена запись категории меню меню
    public async Task<List<MenuPosition>> ListByMenuCategoryIdAsync(int id)
    {
        MenuCategory? menuCategory = await _menuCategoryRepository.GetAsync(id);
        if (menuCategory == null)
        {
            throw new EntityNotFoundException(id, "Menu category");
        }
        return await _menuPositionRepository.ListByMenuCategoryIdAsync(id);
    }
    
    //GetByNameAsync - метод получения позиции меню по ее названию
    //вход: название позиции меню
    //выход: объект MenuPosition
    //исключения:
    //1. EntityNotFoundException - запись позиции меню с введеным именем не найдена
    public async Task<MenuPosition> GetByNameAsync(string name)
    {
        MenuPosition? menuPosition = await _menuPositionRepository.GetByNameAsync(name);
        if (menuPosition == null)
        {
            throw new EntityNotFoundException(name, "Menu position");
        }
        return menuPosition;
    }
}