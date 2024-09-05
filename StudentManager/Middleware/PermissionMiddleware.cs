using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;

using System.IdentityModel.Tokens.Jwt;
using StudentManager.Repositories;

namespace StudentManager.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Exception Block of Permission Middleware
            var bypassPaths = new List<Regex>
            {
                new Regex("^/api/auth/user/register$", RegexOptions.IgnoreCase),
                new Regex("^/api/auth/user/login$", RegexOptions.IgnoreCase)
            };

            if (bypassPaths.Any(regex => regex.IsMatch(context.Request.Path.Value)))
            {
                await _next(context);
                return;
            }
            //End --- Exception Block of Permission Middleware

            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized access: Missing or invalid token");
                return;
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized access: Invalid token");
                return;
            }

            var roleIDClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "roleID");

            if (roleIDClaim != null && long.TryParse(roleIDClaim.Value, out var roleID))
            {
                // Use a scope to resolve the IUserRepository
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _userService = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    if (roleID == 1)
                    {
                        context.Items["FilteredUsers"] = _userService.GetUsersAsync();
                    }
                    else if (roleID == 2)
                    {
                        context.Items["FilteredUsers"] = _userService.GetUsersByRole(2);
                    }
                    else
                    {
                        context.Items["FilteredUsers"] = _userService.GetUsersExceptRole(1);
                    }
                }

                await _next(context);
                return;
            }

            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Unauthorized access: Not Permitted");
        }
    }

    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionMiddleware>();
        }
    }
}
