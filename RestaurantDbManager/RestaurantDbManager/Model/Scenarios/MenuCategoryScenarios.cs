using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работы с категориями меню
public class MenuCategoryScenarios : EntityScenarios<MenuCategory>
{
    private readonly IMenuCategoryRepository _menuCategoryRepository;

    public MenuCategoryScenarios(IMenuCategoryRepository menuCategoryRepository) : 
        base(menuCategoryRepository)
    {
        _menuCategoryRepository = menuCategoryRepository;
    }
    
    //GetByNameAsync - метод получения категории меню по ее названию
    //вход: название категории меню
    //выход: объект MenuCategory
    //исключения:
    //1. EntityNotFoundException - запись категории меню с введеным именем не найдена
    public async Task<MenuCategory> GetByNameAsync(string name)
    {
        MenuCategory? menuCategory = await _menuCategoryRepository.GetByNameAsync(name);
        if (menuCategory == null)
        {
            throw new EntityNotFoundException(name, "Menu category");
        }
        return menuCategory;
    }
}