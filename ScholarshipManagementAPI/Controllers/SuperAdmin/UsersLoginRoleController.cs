using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScholarshipManagementAPI.DTOs.Common;
using ScholarshipManagementAPI.DTOs.Common.Response;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLogin;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersLoginRole;
using ScholarshipManagementAPI.DTOs.SuperAdmin.UsersMenu;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.SuperAdmin;

namespace ScholarshipManagementAPI.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/superadmin/users-login-role")]
    public class UsersLoginRoleController : ControllerBase
    {
        private readonly IUsersLoginRoleService _service;

        public UsersLoginRoleController(IUsersLoginRoleService service)
        {
            _service = service;
        }


        // -------- CREATE --------
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(UsersLoginRoleRequestDto dto)
        {
            dto.CreatedDate = DateTime.UtcNow;                            // always server-side
            dto.CreatedBy = JwtClaimHelper.LoginId(User).ToString();      // or from claims

            var id = await _service.CreateAsync(dto);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = id,
                Message = "UsersLoginRole created successfully"
            });
        }


        // -------- UPDATE --------
        [HttpPut("update/{id:long}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, [FromBody] UsersLoginRoleRequestDto dto)
        {
            dto.UserLoginRoleId = id;
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
                Message = "UsersLoginRole updated successfully",
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
                Message = "UsersLoginRole deleted successfully",
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
        public async Task<IActionResult> GetByFilter(UsersLoginRoleFilterDto filter)
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


        [HttpPost("login-roles")]
        [Authorize]
        public async Task<IActionResult> GetRolesByLogin(UsersLoginRoleFilterDto filter)
        {
            if (filter.LoginId == null || filter.LoginId <= 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "LoginId is required"
                });
            }

            var result = await _service.GetRolesByLoginAsync(filter);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Result = result,
                Message = result.Items.Count > 0
                    ? "Data fetched successfully"
                    : "No permissions found"
            });
        }



        [HttpPost("bulk-save")]
        [Authorize]
        public async Task<IActionResult> BulkSave(LoginRoleBulkSaveDto dto)
        {
            if (dto.LoginId <= 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "LoginId is required"
                });
            }

            var createdBy = JwtClaimHelper.UserName(User).ToString();

            await _service.BulkSaveRolesAsync(dto, createdBy);

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Roles saved successfully"
            });
        }


    }
}
