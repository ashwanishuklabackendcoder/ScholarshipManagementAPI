using Microsoft.IdentityModel.Tokens;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.Helper.Utilities;
using ScholarshipManagementAPI.Services.Interface.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScholarshipManagementAPI.Services.Implementation.Common
{
    public class JwtService : IJwtService
    {

        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }


        public string GenerateToken(TokenDto dto)
        {
            var claims = new[]
            {
                
                // Standard JWT claims -- for the TOKEN itself
                new Claim(JwtRegisteredClaimNames.Sub, dto.LoginId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Standard ASP.NET claims -- for [Authorize]
                new Claim(JwtClaimTypes.UserName, dto.LoginName),
                new Claim(JwtClaimTypes.Role, dto.RoleName), // IMP [Authorize(Roles = "")]

                // Custom application claims -- for our own use Business Logic
                new Claim(JwtClaimTypes.LoginId, dto.LoginId.ToString()),
                new Claim(JwtClaimTypes.RoleId, dto.RoleId.ToString()),
                new Claim(JwtClaimTypes.ModuleId, dto.ModuleId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
