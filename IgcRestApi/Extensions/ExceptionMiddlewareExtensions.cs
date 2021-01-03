using IgcRestApi.DataConversion;
using IgcRestApi.Exceptions;
using IgcRestApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace IgcRestApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            var dataConverter = AutoMapperDataConverter.GetDataConverter();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var coreApiExceptionModel = dataConverter.Convert<CoreApiExceptionModel>(contextFeature.Error);

                        if (contextFeature.Error is CoreApiException coreApiException)
                        {
                            context.Response.StatusCode = (int)coreApiException.StatusCode;

                        }

                        await context.Response.WriteAsync(coreApiExceptionModel.ToString());

                    }
                });
            });
        }
    }
}
