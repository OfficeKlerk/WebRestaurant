using Microsoft.AspNetCore.Mvc;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Api.Controllers;

//контроллер с позициями меню
[Route("api/menu-positions")]
[ApiController]
public class MenuPositionController : ControllerBase
{
    private readonly MenuPositionScenarios _scenarios;

    public MenuPositionController(MenuPositionScenarios scenarios)
    {  
        _scenarios = scenarios;
    }
    
    //GET api/menu-positions
    [HttpGet]
    public async Task<List<MenuPosition>> ReadAllAsync()
    {
        return await _scenarios.ListAllAsync();
    }
    
    //GET api/menu-positions/by-menu-category-id/{id}
    [HttpGet("by-menu-category-id/{id}")]
    public async Task ReadByMenuCategoryIdAsync(int id)
    {
        try
        {
            List<MenuPosition> menuPositions = await _scenarios.ListByMenuCategoryIdAsync(id);
            await WriteSuccess(menuPositions);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //GET api/menu-positions/by-name?name={name}
    [HttpGet("by-name")]
    public async Task ReadByNameAsync(string name)
    {
        try
        {
            MenuPosition menuPosition = await _scenarios.GetByNameAsync(name);
            await WriteSuccess(menuPosition);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }

    //GET api/menu-positions/{id}
    [HttpGet("{id}")]
    public async Task ReadAsync(int id)
    {
        try
        {
            MenuPosition menuPosition = await _scenarios.GetAsync(id);
            await WriteSuccess(menuPosition);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //POST api/menu-positions
    [HttpPost]
    public async Task CreateAsync(MenuPosition menuPosition)
    {
        MenuPosition added = await _scenarios.AddAsync(menuPosition);
        await WriteSuccess(added);
    }
    
    //DELETE api/menu-positions/{id}
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        try
        {
            await _scenarios.RemoveAsync(id);
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            StringMessage message = new StringMessage($"Menu position with id {id} has been successfully deleted");
            await HttpContext.Response.WriteAsJsonAsync(message);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //PATCH api/menu-positions/{id}
    [HttpPatch("{id}")]
    public async Task UpdateAsync(MenuPosition menuPosition, int id)
    {
        try
        {
            MenuPosition updated = await _scenarios.UpdateAsync(menuPosition, id);
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
    private async Task WriteSuccess(MenuPosition menuPosition)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuPosition);
    }
    
    //перегрузка ф-и WriteSuccess:
    //ф-я для возвращения успешного сообщения с json-объектами
    private async Task WriteSuccess(List<MenuPosition> menuPositions)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuPositions);
    }
}