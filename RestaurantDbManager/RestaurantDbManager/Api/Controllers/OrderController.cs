using Microsoft.AspNetCore.Mvc;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Api.Controllers;

//контроллер с заказами
[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderScenarios _scenarios;

    public OrderController(OrderScenarios scenarios)
    {  
        _scenarios = scenarios;
    }
    
    //GET api/orders
    [HttpGet]
    public async Task<List<Order>> ReadAllAsync()
    {
        return await _scenarios.ListAllAsync();
    }
    
    //GET api/orders/{id}
    [HttpGet("{id}")]
    public async Task ReadAsync(int id)
    {
        try
        {
            Order order = await _scenarios.GetAsync(id);
            await WriteSuccess(order);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //POST api/orders
    [HttpPost]
    public async Task CreateAsync(Order order)
    {
        try
        {
            Order added = await _scenarios.AddAsync(order);
            await WriteSuccess(added);
        }
        catch (OrderValidationException ex)
        {
            await WriteError(ex);
        }
    }
    
    //DELETE api/orders/{id}
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        try
        {
            await _scenarios.RemoveAsync(id);
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            StringMessage message = new StringMessage($"Order with id {id} has been successfully deleted");
            await HttpContext.Response.WriteAsJsonAsync(message);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //PATCH api/orders/{id}
    [HttpPatch("{id}")]
    public async Task UpdateAsync(Order order, int id)
    {
        try
        {
            Order updated = await _scenarios.UpdateAsync(order, id);
            await WriteSuccess(updated);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
        catch (OrderValidationException ex)
        {
            await WriteError(ex);
        }
    }
    
    //вспомогательная ф-я для возвращения сообщения об ошибке
    private async Task WriteError(ApplicationException ex)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
        await HttpContext.Response.WriteAsJsonAsync(error);
    }
    
    //вспомогательная ф-я для возвращения успешного сообщения с json-объектом
    private async Task WriteSuccess(Order order)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(order);
    }
}