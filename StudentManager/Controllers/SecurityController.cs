using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using StudentManager.DTOs;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        // Constructor
        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        /////////////////////////////////////// -- Role -- ///////////////////////////////////////////
        ///

        // GET api/security/role or api/security/role/{id}
        [HttpGet("role/{id?}")]
        public async Task<ActionResult<IEnumerable<SecurityDTO>>> GetRolesAsync(long? id)
        {
            var roles = await _securityService.GetRolesAsync(id);

            if (id.HasValue && roles == null)
            {
                return NotFound("No Records Found"); // Return 404 if the specific category is not found
            }

            return Ok(roles);
        }

        // POST api/security/role
        [HttpPost("role")]
        public async Task<IActionResult> InsertRoleAsync([FromBody] Role inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _securityService.InsertRoleAsync(inputModel);

            if (result)
            {
                return Ok("New role added successfully");
            }
            else
            {
                return BadRequest("Failed to add new role");
            }
        }

        // PUT api/security/role/{id}
        [HttpPut("role/{id}")]
        public async Task<IActionResult> UpdateRoleAsync(long id, [FromBody] Role inputModel)
        {
            // Call the Update method in the service
            var result = await _securityService.UpdateRoleAsync(id, inputModel);

            if (!result)
            {
                return NotFound("Failed to update role");
            }

            return Ok("Role updated successfully");
        }

        /////////////////////////////////////// -- Permission -- ///////////////////////////////////////////
        ///

        // GET api/security/permission or api/security/permission/{id}
        [HttpGet("permission/{id?}")]
        public async Task<ActionResult<IEnumerable<SecurityDTO>>> GetPermissionsAsync(long? id)
        {
            var permissions = await _securityService.GetPermissionsAsync(id);

            if (id.HasValue && permissions == null)
            {
                return NotFound("No Records Found"); // Return 404 if the specific category is not found
            }

            return Ok(permissions);
        }

        // POST api/security/permission
        [HttpPost("permission")]
        public async Task<IActionResult> InsertPermissionAsync([FromBody] Permission inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _securityService.InsertPermissionAsync(inputModel);

            if (result)
            {
                return Ok("New permission added successfully");
            }
            else
            {
                return BadRequest("Failed to add new permission");
            }
        }

        // PUT api/security/permission/{id}
        [HttpPut("permission/{id}")]
        public async Task<IActionResult> UpdatePermissionAsync(long id, [FromBody] Permission inputModel)
        {
            // Call the Update method in the service
            var result = await _securityService.UpdatePermissionAsync(id, inputModel);

            if (!result)
            {
                return NotFound("Failed to update permission");
            }

            return Ok("permission updated successfully");
        }

        /////////////////////////////////////// -- Link Role-Permssion -- ///////////////////////////////////////////
        ///

        // POST api/security/permission/link-role
        [HttpPost("permission/link-role")]
        public async Task<IActionResult> LinkPermissionRoleAsync([FromBody] RolePermission inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _securityService.LinkPermissionRoleAsync(inputModel);

            if (result)
            {
                return Ok("Permission & Role linked successfully");
            }
            else
            {
                return BadRequest("Failed to link permission with role");
            }
        }

        // GET api/security/permission/link-role
        [HttpGet("permission/link-role")]
        public async Task<ActionResult<IEnumerable<RolePermissionDTO>>> GetRolePermissionsAsync()
        {
            var rolePermissions = await _securityService.GetRolePermissionsAsync();

            if (rolePermissions == null || !rolePermissions.Any())
            {
                return NotFound("No role permissions found.");
            }

            return Ok(rolePermissions);
        }
    }
}
