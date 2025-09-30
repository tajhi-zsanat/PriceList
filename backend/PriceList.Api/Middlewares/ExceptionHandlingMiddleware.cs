using Microsoft.AspNetCore.Mvc;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using System.Diagnostics;
using System.Net;

namespace PriceList.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork uow) // ⬅ no ct here
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Try to log to DB (never throw from here)
                try
                {
                    var log = new ErrorLog
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        Path = context.Request.Path,
                        UserName = context.User?.Identity?.Name,
                    };

                    await uow.Errors.AddAsync(log, context.RequestAborted);
                    await uow.SaveChangesAsync(context.RequestAborted);
                }
                catch { /* swallow logging failures */ }

                // If response already started, we can't write a clean ProblemDetails body
                if (context.Response.HasStarted)
                {
                    // best effort: abort
                    context.Abort();
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    Title = "خطای غیرمنتظره‌ای رخ داد.",
                    Detail = "خطا ثبت شد. لطفاً بعداً دوباره تلاش کنید.",
                    Instance = context.Request.Path,
                    Type = "about:blank"
                };

                // Optional: surface trace id to caller for support tickets
                //problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

                await context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
            }
        }
    }
}