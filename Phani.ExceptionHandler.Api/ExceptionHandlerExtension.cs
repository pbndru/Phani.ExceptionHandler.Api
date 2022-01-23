using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Phani.ExceptionHandler.Api.Models;

namespace Phani.ExceptionHandler.Api
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
        {
            //We can log the errors here
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    var httpException = (HttpException)feature.Error;

                    var errorResponse = new ErrorResponse();
                   
                    errorResponse.StatusCode = httpException.StatusCode;
                    errorResponse.Message = httpException.Message;

                    context.Response.StatusCode = (int)errorResponse.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(errorResponse.ToJsonString());
                });
            });

            return app;
        }
    }
}
