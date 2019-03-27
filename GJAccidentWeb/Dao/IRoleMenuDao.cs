using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Dao
{
    public interface IRoleMenuDao
    {
        Result<List<RoleMenu>> search(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition);
        int searchCount(RoleMenuQueryModel condition);
        Result<int> delete(RoleMenuQueryModel condition);
        Result<RoleMenu> add(RoleMenu model);
        Result<List<Menu>> searchNotInList(Pager<List<Menu>> pager, RoleMenuQueryModel condition);
        int searchNotInListCount(RoleMenuQueryModel condition);
        
    }
}
