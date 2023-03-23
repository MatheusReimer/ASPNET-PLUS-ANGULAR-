

using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }
            catch(Exception e){
                _logger.LogError(e,  e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response=  _env.IsDevelopment()
                    ? new ApiExceptions(context.Response.StatusCode,e.Message,e.StackTrace?.ToString())
                    : new ApiExceptions(context.Response.StatusCode,e.Message,"Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            }
        }   
        
    }
}