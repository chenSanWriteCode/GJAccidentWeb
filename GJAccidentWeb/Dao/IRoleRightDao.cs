using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Dao
{
    public interface IRoleRightDao
    {
        Result<List<RoleRight>> search(Pager<List<RoleRight>> pager, RoleRightQueryModel condition);
        int searchCount(RoleRightQueryModel condition);
        Result<int> delete(RoleRightQueryModel condition);
        Result<RoleRight> add(RoleRight model);
        /// <summary>
        /// 获取角色数据权限级别
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int getRoleRightLevel(RoleRightQueryModel condition);
    }
}
