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
            if (exception is not DataInsertionException || exception is not ImageUploadException || exception is not UnitException)
            {
                _logger.LogError(exception.Message);
                return false;
            }           

            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = exception.GetType().Name,
                    Detail = exception.Message ,
                    Status = (int)HttpStatusCode.BadRequest,
                }, cancellationToken);
                _logger.LogError(exception.Message, exception.StackTrace);
                return true;
            }
            
            
        }



       
    }

}
