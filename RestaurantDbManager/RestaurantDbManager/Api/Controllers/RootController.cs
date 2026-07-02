using Microsoft.AspNetCore.Mvc;

namespace RestaurantDbManager.Api.Controllers;

//корневой контроллер
[Route("api/root")]
[ApiController]
public class RootController : ControllerBase
{
    //индекс
    [HttpGet]
    public StringMessage Index()
    {
        return new StringMessage($"Server is running on {HttpContext.Request.Host}...");
    }

    //пинг
    [HttpGet("ping")]
    public StringMessage Ping()
    {
        return new StringMessage("Pong");
    }
}