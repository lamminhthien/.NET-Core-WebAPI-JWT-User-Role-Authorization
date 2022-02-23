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

            return Ok();

        }
        //Kiểm tra dữ liệu nhập vào so sánh với Database simulator (UserList)
        private UserInfo Authenticate(UserAuthorize userAuthorize)
        {
            // Kiểm tra với database simulator (UserInfo)
            var currentUser = UserList.Users.FirstOrDefault(o => o.Username.ToLower() == userAuthorize.Username.ToLower() && o.Password == userAuthorize.Password);
            
            //Trả về thông tin của user hiện tại khi đăng nhập thành công
            if (currentUser != null)
            {
                return currentUser;
            }

            //Trả về null khi đăng nhập thất bại
            return null;
            
        }
    }
}
