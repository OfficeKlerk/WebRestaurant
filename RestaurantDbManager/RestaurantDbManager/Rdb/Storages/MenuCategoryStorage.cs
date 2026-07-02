using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb.Storages;

//имплементация сервиса для crud-операций с сущностью "категория меню"
public class MenuCategoryStorage: IMenuCategoryRepository
{
    private readonly ApplicationDbContext _db;

    public MenuCategoryStorage(ApplicationDbContext db)
    {
        _db = db;
    }

    //получение всех записей категорий меню
    public async Task<List<MenuCategory>> ListAllAsync()
    {
        return await _db.MenuCategories.ToListAsync();
    }
    
    //получение категории меню по названию
    public async Task<MenuCategory?> GetByNameAsync(string name)
    {
        return await _db.MenuCategories.FirstOrDefaultAsync(x => x.CategoryName == name);
    }

    //получение записи категории меню по id
    public async Task<MenuCategory?> GetAsync(int id)
    {
        return await _db.MenuCategories.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //добавление записи категории меню
    public async Task<MenuCategory> AddAsync(MenuCategory menuCategory)
    {
        await _db.MenuCategories.AddAsync(menuCategory);
        await _db.SaveChangesAsync();
        return await _db.MenuCategories.FirstAsync(x => x.Id == menuCategory.Id);
    }
    
    //удаление записи категории меню по id
    public async Task RemoveAsync(int id)
    {
        MenuCategory? menuCategory = await GetAsync(id);
        if (menuCategory != null)
        {
            _db.MenuCategories.Remove(menuCategory);
            await _db.SaveChangesAsync();
        }
    }
    
    //редактирование записи категории меню по id
    public async Task<MenuCategory?> UpdateAsync(MenuCategory newMenuCategory, int id)
    {
        MenuCategory? menuCategory = await GetAsync(id);
        if (menuCategory != null)
        {
            menuCategory.CategoryName = newMenuCategory.CategoryName;
            menuCategory.Image = newMenuCategory.Image;
            await _db.SaveChangesAsync();
        }
        return menuCategory;
    }
}