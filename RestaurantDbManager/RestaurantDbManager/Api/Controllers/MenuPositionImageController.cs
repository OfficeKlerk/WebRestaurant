using Microsoft.AspNetCore.Mvc;
using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Api.Controllers;

//контроллер с фотографиями позиций меню
[Route("api/menu-position-images")]
[ApiController]
public class MenuPositionImageController : ControllerBase
{
    private readonly MenuPositionImageScenarios _scenarios;

    public MenuPositionImageController(MenuPositionImageScenarios scenarios)
    {  
        _scenarios = scenarios;
    }
    
    //GET api/menu-position-images/by-menu-position-id/{id}
    [HttpGet("by-menu-position-id/{id}")]
    public async Task ReadAllByMenuPositionIdAsync(int id)
    {
        try
        {
            List<MenuPositionImage> menuPositionImages = await _scenarios.ListByMenuPositionIdAsync(id);
            await WriteSuccess(menuPositionImages);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //GET api/menu-position-images/{id}
    [HttpGet("{id}")]
    public async Task ReadAsync(int id)
    {
        try
        {
            MenuPositionImage menuPositionImage = await _scenarios.GetAsync(id);
            await WriteSuccess(menuPositionImage);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //POST api/menu-position-images
    [HttpPost]
    public async Task CreateAsync(MenuPositionImage menuPositionImage)
    {
        MenuPositionImage added = await _scenarios.AddAsync(menuPositionImage);
        await WriteSuccess(added);
    }
    
    //DELETE api/menu-position-images/{id}
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        try
        {
            await _scenarios.RemoveAsync(id);
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            StringMessage message = new StringMessage($"Menu position image with id {id} has been successfully deleted");
            await HttpContext.Response.WriteAsJsonAsync(message);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteError(ex);
        }
    }
    
    //PATCH api/menu-position-images/{id}
    [HttpPatch("{id}")]
    public async Task UpdateAsync(MenuPositionImage menuPositionImage, int id)
    {
        try
        {
            MenuPositionImage updated = await _scenarios.UpdateAsync(menuPositionImage, id);
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
    private async Task WriteSuccess(MenuPositionImage menuPositionImage)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuPositionImage);
    }
    
    //перегрузка ф-и WriteSuccess:
    //ф-я для возвращения успешного сообщения с json-объектами
    private async Task WriteSuccess(List<MenuPositionImage> menuPositionImages)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        await HttpContext.Response.WriteAsJsonAsync(menuPositionImages);
    }
}