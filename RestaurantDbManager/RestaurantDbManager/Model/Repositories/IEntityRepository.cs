namespace RestaurantDbManager.Model.Repositories;

//интерфейс сервиса для crud-операций с сущностями
public interface IEntityRepository<T> where T : class
{
    //получение всех записей сущностей
    Task<List<T>> ListAllAsync();
    
    //получение записи сущности по id
    Task<T?> GetAsync(int id);
    
    //добавление записи сущности
    Task<T> AddAsync(T entity);
    
    //удаление записи сущности по id
    Task RemoveAsync(int id);
    
    //редактирование записи сущности по id
    Task<T?> UpdateAsync(T entity, int id);
}