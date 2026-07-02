using RestaurantDbManager.Model;
using RestaurantDbManager.Model.Repositories;
using RestaurantDbManager.Model.Scenarios;
using RestaurantDbManager.Rdb;
using RestaurantDbManager.Rdb.Entities;
using RestaurantDbManager.Rdb.Storages;

var builder = WebApplication.CreateBuilder(args);

//политика CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", build =>
    {
        build.AllowAnyOrigin() // Разрешить доступ с любого источника
            .AllowAnyMethod() // Разрешить все HTTP-методы (GET, POST, PUT и т. д.)
            .AllowAnyHeader(); // Разрешить все заголовки
    });
});

builder.Services.AddControllers();

//зависимости
builder.Services.AddTransient<IOrderRepository, OrderStorage>();
builder.Services.AddTransient<IMenuPositionRepository, MenuPositionStorage>();
builder.Services.AddTransient<IMenuPositionOrderRepository, MenuPositionOrderStorage>();
builder.Services.AddTransient<IMenuPositionImageRepository, MenuPositionImageStorage>();
builder.Services.AddTransient<IMenuCategoryRepository, MenuCategoryStorage>();

builder.Services.AddTransient<MenuCategoryScenarios>();
builder.Services.AddTransient<MenuPositionImageScenarios>();
builder.Services.AddTransient<MenuPositionOrderScenarios>();
builder.Services.AddTransient<MenuPositionScenarios>();
builder.Services.AddTransient<OrderScenarios>();

builder.Services.AddDbContext<ApplicationDbContext>();

var app =  builder.Build();

//политика CORS
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();