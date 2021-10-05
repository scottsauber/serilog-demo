using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace SerilogDemo.Infrastructure
{
    public class UseSerilogLogContextNoAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public UseSerilogLogContextNoAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty(SerilogProperties.Url, context.Request.Host + context.Request.Path + context.Request.QueryString))
            using (LogContext.PushProperty(SerilogProperties.IpAddress, context.Connection.RemoteIpAddress?.ToString()))
            {
                await _next.Invoke(context);
            }
        }
    }
    public static class UseSerilogLogContextNoAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseSerilogLogContextNoAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseSerilogLogContextNoAuthMiddleware>();
        }
    }
}