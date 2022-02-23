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

        [HttpGet("Admins")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminsEndpoint()
        {
            //var currentUser = Ge
            return Ok();
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
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
