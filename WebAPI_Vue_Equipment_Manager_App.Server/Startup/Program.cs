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

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

//Configure

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseHttpLogging();

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
