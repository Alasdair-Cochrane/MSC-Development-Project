using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

using System.Text;
using WebAPI_Vue_Equipment_Manager_App.Server.Startup;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Google.Protobuf.WellKnownTypes;
using Microsoft.OpenApi.Models;
using System.Xml.Linq;
using Microsoft.Net.Http.Headers;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//https://www.infoworld.com/article/2334527/implement-authorization-for-swagger-in-aspnet-core.html
//To allow authorisation access when debuggin in the Swagger UI
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
        Scheme = "Bearer"
    });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }});
    });

builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Logging.AddConsole();
builder.Services.AddHttpLogging(o => { });

//Configure Authentication/Authorisation

builder.Services.AddAuthentication();

builder.Services.AddIdentityApiEndpoints<User>().
    AddEntityFrameworkStores<MainDbContext>();

//Connect Database

builder.Services.AddDbContext<MainDbContext>(
    options => {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Remote"));
       
        });

//Register Dependencies

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

//Configure
builder.Services.AddHttpClient();

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
    app.MapSwagger().RequireAuthorization();
}


app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();


app.MapFallbackToFile("/index.html");

app.Run();
