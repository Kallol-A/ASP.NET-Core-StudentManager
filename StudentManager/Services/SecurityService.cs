using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.DTOs;

namespace StudentManager.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;

        // Constructor
        public SecurityService(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
        }

        public async Task<bool> InsertRoleAsync(Role role)
        {
            try
            {
                // Create a new Student Category object
                var _role = new Role
                {
                    role_name = role.role_name,
                    created_by_user = role.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _securityRepository.InsertRoleAsync(_role);
                var result = await _securityRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync(long? id)
        {
            // Fetch data from the repository based on the presence of id
            var roles = await _securityRepository.GetRolesAsync(id);

            // If an id is provided, return the single record as a list
            if (id.HasValue)
            {
                var role = roles.FirstOrDefault();
                if (role == null)
                {
                    return null;
                }
                return new List<RoleDTO>
                {
                    new RoleDTO
                    {
                        RoleId = role.id_role,
                        RoleName = role.role_name
                    }
                };
            }

            // Otherwise, return all records
            var roleDTOs = roles.Select(u => new RoleDTO
            {
                RoleId = u.id_role,
                RoleName = u.role_name
            });

            return roleDTOs;
        }

        public async Task<bool> UpdateRoleAsync(long idRole, Role role)
        {
            try
            {
                // Create a new Student Category object
                var _role = new Role
                {
                    id_role = idRole,
                    role_name = role.role_name,
                    last_updated_by_user = role.last_updated_by_user,
                    updated_at = DateTime.Now
                };

                // Add Student Category through repository
                _securityRepository.UpdateRoleAsync(_role);
                var result = await _securityRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<bool> InsertPermissionAsync(Permission permission)
        {
            try
            {
                // Create a new Student Category object
                var _permission = new Permission
                {
                    permission_name = permission.permission_name,
                    created_by_user = permission.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _securityRepository.InsertPermissionAsync(_permission);
                var result = await _securityRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<IEnumerable<PermissionDTO>> GetPermissionsAsync(long? id)
        {
            // Fetch data from the repository based on the presence of id
            var permissions = await _securityRepository.GetPermissionsAsync(id);

            // If an id is provided, return the single record as a list
            if (id.HasValue)
            {
                var permission = permissions.FirstOrDefault();
                if (permission == null)
                {
                    return null;
                }
                return new List<PermissionDTO>
                {
                    new PermissionDTO
                    {
                        PermissionId = permission.id_permission,
                        PermissionName = permission.permission_name
                    }
                };
            }

            // Otherwise, return all records
            var permissionDTOs = permissions.Select(u => new PermissionDTO
            {
                PermissionId = u.id_permission,
                PermissionName = u.permission_name
            });

            return permissionDTOs;
        }

        public async Task<bool> UpdatePermissionAsync(long idPermission, Permission permission)
        {
            try
            {
                // Create a new Student Category object
                var _permission = new Permission
                {
                    id_permission = idPermission,
                    permission_name = permission.permission_name,
                    last_updated_by_user = permission.last_updated_by_user,
                    updated_at = DateTime.Now
                };

                // Add Student Category through repository
                _securityRepository.UpdatePermissionAsync(_permission);
                var result = await _securityRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<bool> LinkPermissionRoleAsync(RolePermission rolepermission)
        {
            try
            {
                // Create a new Student Category object
                var _rolepermission = new RolePermission
                {
                    id_permission = rolepermission.id_permission,
                    id_role = rolepermission.id_role,
                    created_by_user = rolepermission.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _securityRepository.LinkPermissionRoleAsync(_rolepermission);
                var result = await _securityRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<IEnumerable<RolePermissionDTO>> GetRolePermissionsAsync()
        {
            // Fetch role permissions from the repository
            var rolePermissions = await _securityRepository.GetRolePermissionsAsync();

            // Map the data to the DTO
            var rolePermissionDTOs = rolePermissions.Select(rp => new RolePermissionDTO
            {
                RoleId = rp.Role.id_role,
                RoleName = rp.Role.role_name,
                PermissionId = rp.Permission.id_permission,
                PermissionName = rp.Permission.permission_name
            });

            return rolePermissionDTOs;
        }
    }
}
