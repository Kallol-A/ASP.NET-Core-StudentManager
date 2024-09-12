using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using StudentManager.Data; // Assuming you have a DbContext for accessing the database

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
            // Exception Block of Permission Middleware
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
            // End --- Exception Block of Permission Middleware

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

            // Extract user ID and role ID from JWT token
            var userIDClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userID");
            var roleIDClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "roleID");

            if (userIDClaim == null || roleIDClaim == null || !long.TryParse(userIDClaim.Value, out var userID) || !int.TryParse(roleIDClaim.Value, out var roleID))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized access: Invalid user or role ID");
                return;
            }

            // Determine the permission based on HTTP method
            var method = context.Request.Method.ToUpper();
            long permissionId = 0;
            switch (method)
            {
                case "POST":
                    permissionId = 1;
                    break;
                case "GET":
                    permissionId = 2;
                    break;
                case "PUT":
                    permissionId = 3;
                    break;
                case "DELETE":
                    permissionId = 4;
                    break;
                case "PATCH":
                    permissionId = 5;
                    break;
                default:
                    permissionId = 0;
                    break;
            }


            if (permissionId == 0)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Unauthorized access: Invalid HTTP method");
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Your DbContext

                // Check if the role has the required permission
                var hasPermission = await dbContext.RolePermissions
                    .Where(rp => rp.id_role == roleID && rp.id_permission == permissionId && rp.deleted_at == null)
                    .Include(rp => rp.Role)
                    .Include(rp => rp.Permission)
                    .AnyAsync();

                if (!hasPermission)
                {
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("Unauthorized access: Not Permitted");
                    return;
                }
            }

            // If everything is okay, pass on to the next flow
            await _next(context);
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
