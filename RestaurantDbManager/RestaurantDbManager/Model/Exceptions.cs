namespace RestaurantDbManager.Model.Scenarios;

//классы исключений модели

//EntityNotFoundException - исключение не найденного объекта сущности
public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(int id, string entity) : base($"{entity} with id '{id}' was not found") { }
    public EntityNotFoundException(string name, string entity) : base($"{entity} with name '{name}' was not found") { }
}

//OrderValidationException - исключение, связанное с некорректным форматом сущности "заказ"
public class OrderValidationException : ApplicationException
{
    public OrderValidationException(string details) : base($"Order is invalid: check the validation of the next string: {details}") { }
}