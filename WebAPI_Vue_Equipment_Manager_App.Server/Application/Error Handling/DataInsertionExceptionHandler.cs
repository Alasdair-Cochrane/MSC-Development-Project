using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

//https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses#default-api-behavior

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling
{
    public class DataInsertionExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DataInsertionExceptionHandler> _logger;

        public DataInsertionExceptionHandler(ILogger<DataInsertionExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DataInsertionException)
            {
                _logger.LogError(exception.Message);
                return false;
            }
            DataInsertionException ex = exception as DataInsertionException;

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Error occured when Adding element to Database",
                Detail = ex.Message ?? "Data Insertion Exception",
                Type = ex.GetType().Name,
                Status = (int) HttpStatusCode.BadRequest,
            }, cancellationToken);
            _logger.LogError(ex.Message, ex.StackTrace);
            return true;
        }
    }

}
