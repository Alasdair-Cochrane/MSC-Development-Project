
using Microsoft.AspNetCore.Mvc.Filters;

//https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0
namespace WebAPI_Vue_Equipment_Manager_App.Server.Startup
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Content Type : {context.HttpContext.Request.ContentType} Body: {context.HttpContext.Request.Body}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Content Type : {context.HttpContext.Request.ContentType} Body: {context.HttpContext.Request.Body}");

        }
    }
}
