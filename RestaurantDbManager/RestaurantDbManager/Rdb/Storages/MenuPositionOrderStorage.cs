using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb.Storages;

//имплементация сервиса для crud-операций с сущностью "заказ позиции меню"
public class MenuPositionOrderStorage : IMenuPositionOrderRepository
{
    private readonly ApplicationDbContext _db;

    public MenuPositionOrderStorage(ApplicationDbContext db)
    {
        _db = db;
    }

    //получение всех записей заказов позиций меню
    public async Task<List<MenuPositionOrder>> ListAllAsync()
    {
        return await _db.MenuPositionOrders.ToListAsync();
    }

    //получение записи заказа позиции меню по id
    public async Task<MenuPositionOrder?> GetAsync(int id)
    {
        return await _db.MenuPositionOrders.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //добавление записи заказа позиции меню
    public async Task<MenuPositionOrder> AddAsync(MenuPositionOrder menuPositionOrder)
    {
        await _db.MenuPositionOrders.AddAsync(menuPositionOrder);
        await _db.SaveChangesAsync();
        return await _db.MenuPositionOrders.FirstAsync(x => x.Id == menuPositionOrder.Id);
    }
    
    //удаление записи заказа позиции меню по id
    public async Task RemoveAsync(int id)
    {
        MenuPositionOrder? menuPositionOrder = await GetAsync(id);
        if (menuPositionOrder != null)
        {
            _db.MenuPositionOrders.Remove(menuPositionOrder);
            await _db.SaveChangesAsync();
        }
    }
    
    //редактирование записи заказа позиции меню по id
    public async Task<MenuPositionOrder?> UpdateAsync(MenuPositionOrder newMenuPositionOrder, int id)
    {
        MenuPositionOrder? menuPositionOrder = await GetAsync(id);
        if (menuPositionOrder != null)
        {
            menuPositionOrder.Count = newMenuPositionOrder.Count;
            menuPositionOrder.MenuPositionId = newMenuPositionOrder.MenuPositionId;
            menuPositionOrder.MenuPosition = newMenuPositionOrder.MenuPosition;
            menuPositionOrder.OrderId = newMenuPositionOrder.OrderId;
            menuPositionOrder.Order = newMenuPositionOrder.Order;
            await _db.SaveChangesAsync();
        }
        return menuPositionOrder;
    }
}