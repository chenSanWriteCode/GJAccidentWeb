using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Dao
{
    public interface IRoleDao : IDao<Role>
    {
        Result<List<Role>> search(Pager<List<Role>> pager,string userName);
        int searchCount(string userName);
        Result<Role> searchModelByCondition(Role condition);

        Result<List<UserInfo>> searchUsersByRole(Pager<List<UserInfo>> pager, RoleToUsersQueryModel condition);
        int searchUsersCount(RoleToUsersQueryModel condition);
        /// <summary>
        /// 给角色删除用户
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Result<int> DeleteUser(RoleToUsersQueryModel condition);
        /// <summary>
        /// 给角色增加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<RoleToUser> AddUser(RoleToUser model);
    }
}
