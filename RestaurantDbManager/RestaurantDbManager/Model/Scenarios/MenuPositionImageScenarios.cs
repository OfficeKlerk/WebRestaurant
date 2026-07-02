using System.Text.RegularExpressions;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работы с изображениями позиций меню
public class MenuPositionImageScenarios : EntityScenarios<MenuPositionImage>
{
    private readonly IMenuPositionImageRepository _menuPositionImageRepository;
    private readonly IMenuPositionRepository _menuPositionRepository;

    public MenuPositionImageScenarios(IMenuPositionImageRepository menuPositionImageRepository, 
           IMenuPositionRepository menuPositionRepository) : base(menuPositionImageRepository)
    {
        _menuPositionImageRepository = menuPositionImageRepository;
        _menuPositionRepository = menuPositionRepository;
    }

    //ListByMenuPositionIdAsync - метод получения всех записей фотографий позиций меню по id позиции из меню
    //вход: id позиции меню
    //выход: список с объектами MenuPositionImage
    //исключения:
    //1. EntityNotFoundException - по переданному id не была найдена запись позиции меню
    public async Task<List<MenuPositionImage>> ListByMenuPositionIdAsync(int id)
    {
        MenuPosition? menuPosition = await _menuPositionRepository.GetAsync(id);
        if (menuPosition == null)
        {
            throw new EntityNotFoundException(id, "Menu position");
        }
        return await _menuPositionImageRepository.ListByMenuPositionIdAsync(id);
    }
}