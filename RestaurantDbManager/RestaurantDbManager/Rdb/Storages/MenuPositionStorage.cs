using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb.Storages;

//имплементация сервиса для crud-операций с сущностью "позиция из меню"
public class MenuPositionStorage : IMenuPositionRepository
{
    private readonly ApplicationDbContext _db;

    public MenuPositionStorage(ApplicationDbContext db)
    {
        _db = db;
    }

    //получение всех записей позиций меню
    public async Task<List<MenuPosition>> ListAllAsync()
    {
        return await _db.MenuPositions.ToListAsync();
    }
    
    //получение позиции меню по названию
    public async Task<MenuPosition?> GetByNameAsync(string name)
    {
        return await _db.MenuPositions.FirstOrDefaultAsync(x => x.Name == name);
    }
    
    //получение записи позиции меню по id категории меню
    public async Task<List<MenuPosition>> ListByMenuCategoryIdAsync(int id)
    {
        return await _db.MenuPositions
            .Where(x => x.MenuCategoryId == id)
            .ToListAsync();
    }

    //получение записи позиции меню по id
    public async Task<MenuPosition?> GetAsync(int id)
    {
        return await _db.MenuPositions.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //добавление записи позиции меню
    public async Task<MenuPosition> AddAsync(MenuPosition menuPosition)
    {
        await _db.MenuPositions.AddAsync(menuPosition);
        await _db.SaveChangesAsync();
        return await _db.MenuPositions.FirstAsync(x => x.Id == menuPosition.Id);
    }
    
    //удаление записи позиции меню по id
    public async Task RemoveAsync(int id)
    {
        MenuPosition? menuPosition = await GetAsync(id);
        if (menuPosition != null)
        {
            _db.MenuPositions.Remove(menuPosition);
            await _db.SaveChangesAsync();
        }
    }
    
    //редактирование записи позиции меню по id
    public async Task<MenuPosition?> UpdateAsync(MenuPosition newMenuPosition, int id)
    {
        MenuPosition? menuPosition = await GetAsync(id);
        if (menuPosition != null)
        {
            menuPosition.Name = newMenuPosition.Name;
            menuPosition.Price = newMenuPosition.Price;
            menuPosition.Description = newMenuPosition.Description;
            menuPosition.Weight = newMenuPosition.Weight;
            menuPosition.MenuCategoryId = newMenuPosition.MenuCategoryId;
            menuPosition.MenuCategory = newMenuPosition.MenuCategory;
            await _db.SaveChangesAsync();
        }
        return menuPosition;
    }
}