using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

//https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses#default-api-behavior

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DataInsertionException || exception is not ImageUploadException || 
                exception is not UnitException || exception is not UnauthorisedOperationException)
            {
                _logger.LogError(exception.Message);
                return false;
            }           

            else if(exception is UnauthorisedOperationException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = exception.GetType().Name,
                    Detail = exception.Message,
                    Status = (int)HttpStatusCode.Unauthorized,
                }, cancellationToken);
                _logger.LogError(exception.Message, exception.StackTrace);
                return true;
            }
            else if(exception is ImageUploadException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = exception.GetType().Name,
                    Detail = exception.Message,
                    Status = (int)HttpStatusCode.InternalServerError,
                }, cancellationToken);
                _logger.LogError(exception.Message, exception.StackTrace);
                return true;
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
