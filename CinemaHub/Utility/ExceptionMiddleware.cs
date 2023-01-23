using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using Microsoft.AspNetCore.Http;

namespace CinemaHub.Utility
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = new ValidationFailedResponse(exception.ValidationErrors);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
