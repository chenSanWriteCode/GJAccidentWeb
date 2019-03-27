using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;

namespace GJAccidentWeb.Dao
{
    public interface IUserDao:IDao<UserInfo>
    {
        Result<List<UserInfo>> search(Pager<List<UserInfo>> pager,string userName);
        int searchCount(string userName);
        Result<UserInfo> searchModelByCondition(UserInfo condition);
        /// <summary>
        /// 修改密码
        /// 1. 判断原密码是否能对的上
        /// 2. 修改密码
        /// </summary>
        /// <param name="t"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Result<UserInfo> changePassword(UserInfo t,string newPassword);
        Result<int> passwordSignIn(UserInfo user);
    }
}
