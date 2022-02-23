using System.Collections.Generic;

namespace WebAPI__.NET_Core_JWT_User_Role_Authorization.Models
{
    public class UserInfoList
    {
        public static List<UserInfo> Users = new List<UserInfo>
        {
            new UserInfo() {EmailAddress = "admin@gmail.com", Role="admin",Fullname="Lam Minh Thien",Password="hello_admin" },
            new UserInfo() {EmailAddress= "thien@gmail.com",Role="seller",Fullname="Tran Quoc Thinh",Password="hello_seller"}

        };
    }
}
