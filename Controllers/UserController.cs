using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI__.NET_Core_JWT_User_Role_Authorization.Models;

namespace WebAPI__.NET_Core_JWT_User_Role_Authorization.Controllers
{
    // Đặt route cho controller này "api/usercontroller"
    [Route("api/[controller]")]
    [ApiController]
    // Dùng base Controller với MVC nhưng không cần hỗ trợ View
    public class UserController : ControllerBase
    {

        //Tạo route có kèm xác thực và phân quyền (admin)
        [HttpGet("Admins")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUserFromJWT_Client();
            return Ok($"Hi {currentUser.Fullname}, you are an {currentUser.Role}");
        }


        // Hàm đọc Token JWT vừa được tạo ra và lưu bên LocalStorage của web browser client
        public UserInfo GetCurrentUserFromJWT_Client()
        {
            //Đọc tokken string từ dữ liệu truyền qua giao thức http
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // Trích xuất thông tin từ tokken ra
            if (identity != null)
            {
                //Lấy mảng thông tin định danh người dùng
                var userClaims = identity.Claims;
                
                // Lọc ra các dữ liệu, lưu vào UserInfo Object
                return new UserInfo
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Fullname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value
                };
            }
            return null;
        }
    }
}
