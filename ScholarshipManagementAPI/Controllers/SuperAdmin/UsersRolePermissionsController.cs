using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePermission;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/superadmin/users-role-permission")]
    public class UsersRolePermissionsController : ControllerBase
    {
        private readonly IUsersRolePermissionService _service;

        public UsersRolePermissionsController(IUsersRolePermissionService service)
        {
            _service = service;
        }


        [HttpPost("role-permissions")]
        [Authorize]
        public async Task<IActionResult> GetRolePermissions(UsersRolePermissionFilterDto filter)
        {
            if (filter.RoleId == null || filter.RoleId <= 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "RoleId is required"
                });
            }

            var result = await _service.GetRolePermissionsAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = result.Items.Count > 0
                    ? "Data fetched successfully"
                    : "No permissions found"
            });
        }


        [HttpPost("role-permissions/bulk-save")]
        [Authorize]
        public async Task<IActionResult> BulkSave(UsersRolePermissionBulkSaveDto dto)
        {
            if (dto.RoleId <= 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "RoleId is required"
                });
            }

            var createdBy = JwtClaimHelper.LoginId(User);

            await _service.BulkSaveRolePermissionsAsync(dto, createdBy);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Permissions saved successfully"
            });
        }


    }
}
