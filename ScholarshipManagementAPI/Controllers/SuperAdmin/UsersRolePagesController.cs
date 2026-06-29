using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersRolePage;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/superadmin/users-role-pages")]
    public class UsersRolePagesController : ControllerBase
    {
        private readonly IUsersRolePagesService _service;

        public UsersRolePagesController(IUsersRolePagesService service)
        {
            _service = service;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(UsersRolePageRequestDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "UsersRolePages created successfully"
            });
        }

        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] UsersRolePageRequestDto dto)
        {
            dto.RoleId = id;
            var updated = await _service.UpdateAsync(dto);

            if (!updated)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null,
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "UsersRolePages updated successfully",
                Result = updated,
            });
        }


        // -------- DELETE --------
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "UsersRolePages deleted successfully",
                Result = deleted
            });
        }


        // -------- GET BY ID --------
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Record not found",
                    Result = null
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = data,
                Message = "Record found"
            });
        }


        // -------- FILTER / GET ALL --------
        [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> GetByFilter(UsersRolePageFilterDto filter)
        {
            var result = await _service.GetByFilterAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = result.Items.Count == 0 ? false : true,
                Result = result,
                Message = result.Items.Count == 0
                    ? "Data not found"
                    : "Data fetched successfully"
            });
        }


        [HttpPost("role-permissions")]
        [Authorize]
        public async Task<IActionResult> GetRolePermissions(UsersRolePageFilterDto filter)
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
        public async Task<IActionResult> BulkSave(RolePermissionBulkSaveDto dto)
        {
            if (dto.RoleId <= 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "RoleId is required"
                });
            }

            var createdBy = JwtClaimHelper.UserName(User).ToString();

            await _service.BulkSaveRolePermissionsAsync(dto, createdBy);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Permissions saved successfully"
            });
        }


    }
}
