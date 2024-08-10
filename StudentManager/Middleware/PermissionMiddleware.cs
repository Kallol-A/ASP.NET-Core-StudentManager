using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Middleware
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

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth/user/register") ||
                context.Request.Path.StartsWithSegments("/api/auth/user/login") ||
                context.Request.Path.StartsWithSegments("/api/auth/student/login"))
            {
                await _next(context);
                return;
            }

            var userClaims = context.User.Claims;
            var roleIDClaim = userClaims.FirstOrDefault(c => c.Type == "roleID");
            var studentIDClaim = userClaims.FirstOrDefault(c => c.Type == "studentID");

            if (roleIDClaim != null && long.TryParse(roleIDClaim.Value, out var roleID))
            {
                var permissions = await GetRolePermissionsAsync(roleID);

                if (context.Request.Path.StartsWithSegments("/api/student/detail_by_id") && context.Request.Method == "POST")
                {
                    context.Request.EnableBuffering(); // Enable buffering to read the body multiple times

                    // Read the studentId from the request body
                    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                    context.Request.Body.Position = 0; // Reset the stream position for the next read

                    var studentId = JsonConvert.DeserializeObject<StudentIdDto>(requestBody)?.StudentId;

                    if (studentIDClaim != null && studentId != null && long.TryParse(studentIDClaim.Value, out var studentID) && studentID == studentId)
                    {
                        // If the student is trying to access their own details, allow the request
                        await _next(context);
                        return;
                    }
                }

                // Check if the user has the necessary permission for the current request
                if (CheckPermission(context.Request.Method, permissions))
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Unauthorized access: Not Permitted");
                return;
            }

            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized access: Invalid Token");
        }

        private async Task<string[]> GetRolePermissionsAsync(long roleId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var permissionIds = await dbContext.RolePermissions
                    .Where(rp => rp.id_role == roleId)
                    .Select(rp => rp.id_permission)
                    .ToArrayAsync();

                var permissions = await dbContext.Permissions
                    .Where(p => permissionIds.Contains(p.id_permission))
                    .Select(p => p.permission)
                    .ToArrayAsync();

                return permissions;
            }
        }

        private static bool CheckPermission(string method, string[] permissions)
        {
            // Your logic to check if the user has the necessary permission based on the request method
            // Example logic: Check if the requested method is allowed for the given permissions
            var operation = GetOperationForMethod(method);

            return permissions.Any(p => p.Equals(operation, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetOperationForMethod(string method)
        {
            switch (method.ToUpper())
            {
                case "GET":
                    return "Read";
                case "POST":
                    return "Create";
                case "PUT":
                    return "Update";
                case "DELETE":
                    return "Delete";
                default:
                    return string.Empty;
            }
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
