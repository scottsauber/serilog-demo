using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace SerilogDemo.Infrastructure
{
    public class UseSerilogLogContextWithAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public UseSerilogLogContextWithAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var unauthenticatedMessage = "Unauthenticated";
            var userName = context.User.Identity != null && context.User.Identity.IsAuthenticated ? context.User.Identity.Name : unauthenticatedMessage;

            // Add extra properties to Serilog
            using (LogContext.PushProperty(SerilogProperties.User, !String.IsNullOrWhiteSpace(userName) ? userName : unauthenticatedMessage))
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class UseSerilogLogContexWithAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseSerilogLogContextWithAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseSerilogLogContextWithAuthMiddleware>();
        }
    }
}