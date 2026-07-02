using Microsoft.AspNetCore.Mvc;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Api.Controllers;

//контроллер с заказами позиций меню
[Route("api/menu-position-orders")]
[ApiController]
public class MenuPositionOrderController : ControllerBase
{
    private readonly MenuPositionOrderScenarios _scenarios;

    public MenuPositionOrderController(MenuPositionOrderScenarios scenarios)
    {  
        _scenarios = scenarios;
    }
    
    //GET api/menu-position-orders
    [HttpGet]
    public async Task<List<MenuPositionOrder>> ReadAllAsync()
    {
        return await _scenarios.ListAllAsync();
    }
    
    //GET api/menu-position-orders/{id}
    [HttpGet("{id}")]
    public async Task ReadAsync(int id)
    {
        try
        {
            MenuPositionOrder menuPositionOrder = await _scenarios.GetAsync(id);
            await WriteSuccess(menuPositionOrder);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //POST api/menu-position-orders
    [HttpPost]
    public async Task CreateAsync(MenuPositionOrder menuPositionOrder)
    {
        MenuPositionOrder added = await _scenarios.AddAsync(menuPositionOrder);
        await WriteSuccess(added);
    }
    
    //DELETE api/menu-position-orders/{id}
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        try
        {
            await _scenarios.RemoveAsync(id);
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            StringMessage message = new StringMessage($"Menu position order with id {id} has been successfully deleted");
            await HttpContext.Response.WriteAsJsonAsync(message);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //PATCH api/menu-position-orders/{id}
    [HttpPatch("{id}")]
    public async Task UpdateAsync(MenuPositionOrder menuPositionOrder, int id)
    {
        try
        {
            MenuPositionOrder updated = await _scenarios.UpdateAsync(menuPositionOrder, id);
            await WriteSuccess(updated);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //вспомогательная ф-я для возвращения сообщения об ошибке
    private async Task WriteError(EntityNotFoundException ex)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
        await HttpContext.Response.WriteAsJsonAsync(error);
    }
    
    //вспомогательная ф-я для возвращения успешного сообщения с json-объектом
    private async Task WriteSuccess(MenuPositionOrder menuPositionOrder)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuPositionOrder);
    }
}