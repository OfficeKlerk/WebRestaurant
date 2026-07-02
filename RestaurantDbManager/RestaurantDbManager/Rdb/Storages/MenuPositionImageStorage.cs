using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb.Storages;

//имплементация сервиса для crud-операций с сущностью "фотография позиции меню"
public class MenuPositionImageStorage : IMenuPositionImageRepository
{
    private readonly ApplicationDbContext _db;

    public MenuPositionImageStorage(ApplicationDbContext db)
    {
        _db = db;
    }
    
    //получение всех записей фотографий позиций меню 
    public async Task<List<MenuPositionImage>> ListAllAsync()
    {
        return await _db.MenuPositionImages.ToListAsync();
    }


    //получение всех записей фотографий позиций меню по id позиции меню
    public async Task<List<MenuPositionImage>> ListByMenuPositionIdAsync(int id)
    {
        return await _db.MenuPositionImages
            .Where(x => x.MenuPositionId == id)
            .ToListAsync();
    }

    //получение записи изображения позиции меню по id
    public async Task<MenuPositionImage?> GetAsync(int id)
    {
        return await _db.MenuPositionImages.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //добавление записи изображения позиции меню
    public async Task<MenuPositionImage> AddAsync(MenuPositionImage menuPositionImage)
    {
        await _db.MenuPositionImages.AddAsync(menuPositionImage);
        await _db.SaveChangesAsync();
        return await _db.MenuPositionImages.FirstAsync(x => x.Id == menuPositionImage.Id);
    }
    
    //удаление записи изображения позиции меню по id
    public async Task RemoveAsync(int id)
    {
        MenuPositionImage? menuPositionImage = await GetAsync(id);
        if (menuPositionImage != null)
        {
            _db.MenuPositionImages.Remove(menuPositionImage);
            await _db.SaveChangesAsync();
        }
    }
    
    //редактирование записи изображения позиции меню по id
    public async Task<MenuPositionImage?> UpdateAsync(MenuPositionImage newMenuPositionImage, int id)
    {
        MenuPositionImage? menuPositionImage = await GetAsync(id);
        if (menuPositionImage != null)
        {
            menuPositionImage.Image = newMenuPositionImage.Image;
            menuPositionImage.MenuPositionId = newMenuPositionImage.MenuPositionId;
            menuPositionImage.MenuPosition = newMenuPositionImage.MenuPosition;
            await _db.SaveChangesAsync();
        }
        return menuPositionImage;
    }
}