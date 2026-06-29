using ScholarshipManagementAPI.DTOs.Common.Auth;

namespace ScholarshipManagementAPI.Services.Interface.Common
{
    public interface IJwtService
    {
        string GenerateToken(TokenDto dto);
    }
}
