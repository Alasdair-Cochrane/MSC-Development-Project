using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var key = Encoding.ASCII.GetBytes("TEST KEY DO NOT USE");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;}
    ).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //source of token must be confirmed against approved issuer
            ValidateAudience = true, //recipient of token must be confirmed against approved recipients
            ValidateLifetime = true, // checks that the token has not expired
            ValidateIssuerSigningKey = true, // checks the signature is valid - against the private key
            ValidIssuer = "ISSUER",
            ValidAudience = "AUDIENCE",
            IssuerSigningKey = new SymmetricSecurityKey(key)        
        };
    });

builder.Services.AddIdentityApiEndpoints<User>().
    AddEntityFrameworkStores<MainDbContext>();
//Connect Database

builder.Services.AddDbContext<MainDbContext>(
    options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresRun")); 
        });

//Register Dependencies

builder.Services.AddScoped<IEquipmentModelRepository, EquipmentModelRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped(typeof(ICategoryRepository<>), typeof(GenericCategoryRepository<>));
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();


builder.Services.AddScoped<IEquipmentModelService, EquipmentModelsService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();

builder.Services.AddSingleton<IEntityCache<EquipmentModelCategory>, InMemoryEntityCache<EquipmentModelCategory>>();
builder.Services.AddSingleton<IEntityCache<MaintenanceCategory>, InMemoryEntityCache<MaintenanceCategory>>();
builder.Services.AddSingleton<IEntityCache<ItemStatusCategory>, InMemoryEntityCache<ItemStatusCategory>>();


//Configure


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapIdentityApi<User>();
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
