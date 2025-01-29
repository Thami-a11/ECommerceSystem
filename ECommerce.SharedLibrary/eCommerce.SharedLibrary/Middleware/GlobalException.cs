using eCommerce.SharedLibrary.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "internal server error";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "too many request";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "not authorised";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Access not allowed";
                    message = "You are not allowed";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex) { 
                LogException.LogExceptions(ex);

                if (ex is TaskCanceledException || ex is TimeoutException) 
                {
                    title = "out of time";
                    message = "Request timedout";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);   
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }),CancellationToken.None);
            return;
        }
    }
}
