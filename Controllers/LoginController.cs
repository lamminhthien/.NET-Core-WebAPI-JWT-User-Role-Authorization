using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI__.NET_Core_JWT_User_Role_Authorization.Models;

namespace WebAPI__.NET_Core_JWT_User_Role_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Đọc appsettings.json
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        //Không cần kiểm tra xác thực khi thực hiện đăng nhập
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserAuthorize userAuthorize)
        {
            //Thực hiện bước kiểm tra xác thực
            var user = Authenticate(userAuthorize);
            //Khi user có tồn tại, tiến hành tạo token
            if (user != null)
            {
                var token = GenerateJWT_Token(user);
                    return Ok(token);
            }
            return NotFound("User not found");

        }
        //Kiểm tra dữ liệu nhập vào so sánh với Database simulator (UserList)
        private UserInfo Authenticate(UserAuthorize userAuthorize)
        {
            // Kiểm tra với database simulator (UserInfo)
            var currentUser = UserInfoList.Users.FirstOrDefault(o => o.Username.ToLower() == userAuthorize.Username.ToLower() && o.Password == userAuthorize.Password);
            
            //Trả về thông tin của user hiện tại khi đăng nhập thành công
            if (currentUser != null)
            {
                return currentUser;
            }

            //Trả về null khi đăng nhập thất bại
            return null;

        }

        //Generate JWT chứa tất cả thông tin của người dùng
        //từ table UserInfo
        private string GenerateJWT_Token(UserInfo userInfo)
        {
            //Mã hoá secret key từ file appsetting.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            //Tạo chứng chỉ chứng thực từ secretkey trên với mã hoá Sha256 (Header của JWT)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Tạo  Claim để mã hoá các thông tin trong database (Payload của JWT)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userInfo.Username),
                new Claim (ClaimTypes.Email,userInfo.EmailAddress),
                new Claim (ClaimTypes.Name,userInfo.Fullname),
                new Claim (ClaimTypes.Role,userInfo.Role)
            };

            // Tạo tokken cùng với các thông tin của Key "Jwt" trong appsetting.json
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            // Serialize từ JWT sang JWE hoặc JWS (Chữ ký xác thực JSON)
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
        
    }
}
