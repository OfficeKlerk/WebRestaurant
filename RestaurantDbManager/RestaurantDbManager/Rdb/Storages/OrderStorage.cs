using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb.Storages;

//имплементация сервиса для crud-операций с сущностью "заказ"
public class OrderStorage : IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderStorage(ApplicationDbContext db)
    {
        _db = db;
    }
    
    //получение всех записей заказов по id
    public async Task<List<Order>> ListAllAsync()
    {
        return await _db.Orders.ToListAsync();
    }
    
    //получение записи заказа по id
    public async Task<Order?> GetAsync(int id)
    {
        return await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    //добавление записи заказа
    public async Task<Order> AddAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        return await _db.Orders.FirstAsync(x => x.Id == order.Id);
    }
    
    //удаление записи заказа по id
    public async Task RemoveAsync(int id)
    {
        Order? order = await GetAsync(id);
        if (order != null)
        {
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
        }
    }
    
    //редактирование записи заказа по id
    public async Task<Order?> UpdateAsync(Order newOrder, int id)
    {
        Order? order = await GetAsync(id);
        if (order != null)
        {
            order.Date = newOrder.Date;
            order.UserPhoneNumber = newOrder.UserPhoneNumber;
            await _db.SaveChangesAsync();
        }
        return order;
    }
}