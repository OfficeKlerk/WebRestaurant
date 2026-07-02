using System.Text.RegularExpressions;
using RestaurantDbManager.Rdb.Entities;
using RestaurantDbManager.Model.Repositories;
namespace RestaurantDbManager.Model.Scenarios;

//сценарии для работы с сущностями
public class EntityScenarios<T> where T: class
{
    private readonly IEntityRepository<T> _repository;

    public EntityScenarios(IEntityRepository<T> repository)
    {
        _repository = repository;
    }

    //ListAllAsync - метод получения всех записей сущностей в виде списка
    //вход: -
    //выход: список с объектами сущностей
    //исключения: -
    public async Task<List<T>> ListAllAsync()
    {
        return await _repository.ListAllAsync();
    }
    
    
    //GetAsync - метод получения записи сущности по id
    //вход: id записи
    //выход: объект сущности
    //исключения:
    //1. EntityNotFoundException - по переданному id не была найдена запись
    public async Task<T> GetAsync(int id)
    {
        T? entity = await _repository.GetAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException(id, GetEntityClassName(entity));
        }
        return entity;
    }
    
    //AddAsync - метод добавления записи сущности
    //вход: объект сущности
    //выход: добавленный объект сущности
    //исключения: -
    public async Task<T> AddAsync(T entity)
    {
        ValidationTest(entity);
        return await _repository.AddAsync(entity);
    }
    
    //RemoveAsync - метод удаления записи сущности по id
    //вход: id записи 
    //выход: -
    //исключения: 
    //1. EntityNotFoundException - по переданному id не была найдена запись
    public async Task RemoveAsync(int id)
    {
        await GetAsync(id);
        await _repository.RemoveAsync(id);
    }
    
    //UpdateAsync - метод редактирования записи сущности
    //вход: id записи, новый объект сущности
    //выход: отредактированный объект сущности
    //исключения: 
    //1. EntityNotFoundException - по переданному id не была найдена запись
    public async Task<T> UpdateAsync(T entity, int id)
    {
        await GetAsync(id);
        ValidationTest(entity);
        return await _repository.UpdateAsync(entity, id);
    }
    
    //вспомогательный метод для получения названия класса сущности
    private string GetEntityClassName(T? entity)
    {
        string message = typeof(T) switch
        {
            Type t when t == typeof(Order) => "Order",
            Type t when t == typeof(MenuPositionOrder) => "Menu position order",
            Type t when t == typeof(MenuPositionImage) => "Menu position image",
            Type t when t == typeof(MenuPosition) => "Menu position",
            Type t when t == typeof(MenuCategory) => "Menu category",
            _ => "Entity not found"
        };
        return message;
    }
    
    //вспомогательный метод для проверки валидации объекта заказа
    private void ValidationTest(T entity)
    {
        //если передался заказ, проверяем его на валидность
        if(typeof(T) == typeof(Order))
        {
            Order? order = entity as Order;
            if (order != null)
            {
                //проверяем телефон на соответствие формату
                if (!Regex.IsMatch(order.UserPhoneNumber, @"^7-\d{3}-\d{3}-\d{2}-\d{2}$"))
                {
                    throw new OrderValidationException(order.UserPhoneNumber);
                }
            }
        }
    }
}