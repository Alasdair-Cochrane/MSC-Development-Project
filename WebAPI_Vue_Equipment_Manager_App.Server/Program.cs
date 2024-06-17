using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Connect Database

builder.Services.AddDbContext<PostgresDbContext>(
    options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresRun")); 
        });

builder.Services.AddTransient<MainDbContext, PostgresDbContext>();

//Register Dependencies

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(ICategoryRepository<>), typeof(GenericCategoryRepository<>));
builder.Services.AddScoped<IModelService, ModelsService>();

builder.Services.AddSingleton<IEntityCache<EquipmentModelCategory>, InMemoryEntityCache<EquipmentModelCategory>>();
builder.Services.AddSingleton<IEntityCache<MaintenanceCategory>, InMemoryEntityCache<MaintenanceCategory>>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
