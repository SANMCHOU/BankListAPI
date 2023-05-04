using BankListAPI.VsCode.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace BankListAPI.VsCode.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) { 
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            
            var errorDetails = new ErrorDetails
            {
                ErrorType = "failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                case NotFoundExceptions notFoundExceptions:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not found";
                    break;
                default:
                    break;
            }
            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }
    }
}
