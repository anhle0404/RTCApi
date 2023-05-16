using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
using ConnectSQL19.Repo.GenericRepo;
using StockManagementCSPart.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSQL19.Repo
{
    public class UserRepo:GenericRepo<Users>
    {
        public UserDTO Login(LoginInfo info)
        {
            UserDTO user = new UserDTO();
            DataTable dt = TextUtils.GetDataTableSP("spLogin", 
                            new string[] { "@LoginName", "@Password" }, 
                            new object[] { info.LoginName, MaHoaMD5.EncryptPassword(info.Password) });
            if (dt.Rows.Count > 0)
            {
                user.id = TextUtils.ToInt(dt.Rows[0]["ID"]);
                user.code = TextUtils.ToString(dt.Rows[0]["Code"]);
                user.fullName = TextUtils.ToString(dt.Rows[0]["FullName"]);
                user.roleUse = TextUtils.ToInt(dt.Rows[0]["RoleUse"]);
                user.isAdmin = TextUtils.ToBoolean(dt.Rows[0]["IsAdmin"]);
            }

            return user;
        }
    }
}
