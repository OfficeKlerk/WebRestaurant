using Microsoft.AspNetCore.Mvc;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Api.Controllers;

//контроллер с категориями меню
[Route("api/menu-categories")]
[ApiController]
public class MenuCategoryController : ControllerBase
{
    private readonly MenuCategoryScenarios _scenarios;

    public MenuCategoryController(MenuCategoryScenarios scenarios)
    {  
        _scenarios = scenarios;
    }
    
    //GET api/menu-categories
    [HttpGet]
    public async Task<List<MenuCategory>> ReadAllAsync()
    {
        return await _scenarios.ListAllAsync();
    }
    
    //GET api/menu-categories/by-name?name={name}
    [HttpGet("by-name")]
    public async Task ReadByNameAsync(string name)
    {
        try
        {
            MenuCategory menuCategory = await _scenarios.GetByNameAsync(name);
            await WriteSuccess(menuCategory);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //GET api/menu-categories/{id}
    [HttpGet("{id:int}")]
    public async Task ReadAsync(int id)
    {
        try
        {
            MenuCategory menuCategory = await _scenarios.GetAsync(id);
            await WriteSuccess(menuCategory);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //POST api/menu-categories
    [HttpPost]
    public async Task CreateAsync(MenuCategory menuCategory)
    {
        MenuCategory added = await _scenarios.AddAsync(menuCategory);
        await WriteSuccess(added);
    }
    
    //DELETE api/menu-categories/{id}
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        try
        {
            await _scenarios.RemoveAsync(id);
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            StringMessage message = new StringMessage($"Menu category with id {id} has been successfully deleted");
            await HttpContext.Response.WriteAsJsonAsync(message);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //PATCH api/menu-categories/{id}
    [HttpPatch("{id}")]
    public async Task UpdateAsync(MenuCategory menuCategory, int id)
    {
        try
        {
            MenuCategory updated = await _scenarios.UpdateAsync(menuCategory, id);
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
    private async Task WriteSuccess(MenuCategory menuCategory)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuCategory);
    }
}