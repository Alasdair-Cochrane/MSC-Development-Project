using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

using System.Text;
using WebAPI_Vue_Equipment_Manager_App.Server.Startup;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<DataInsertionExceptionHandler>();
builder.Logging.AddConsole();
builder.Services.AddHttpLogging(o => { });
builder.Services.AddScoped<LoggingActionFilter>();

//Configure Authentication/Authorisation

builder.Services.AddAuthentication();

builder.Services.AddIdentityApiEndpoints<User>().
    AddEntityFrameworkStores<MainDbContext>();

//Connect Database

builder.Services.AddDbContext<MainDbContext>(
    options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresRun"));
       
        });

//Register Dependencies

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

//Configure

var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseHttpLogging();

app.MapGroup("/api").MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//https://weblog.west-wind.com/posts/2020/Mar/13/Back-to-Basics-Rewriting-a-URL-in-ASPNET-Core
//need to rewrite login requests because of proxy routing 

app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();


app.MapFallbackToFile("/index.html");

app.Run();
