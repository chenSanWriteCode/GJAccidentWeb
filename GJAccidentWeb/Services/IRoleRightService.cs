using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Services
{
    public interface IRoleRightService
    {
        Task<Result<Pager<List<RoleRight>>>> search(Pager<List<RoleRight>> pager, RoleRightQueryModel condition);
        Task<Result<int>> delete(RoleRightQueryModel condition);
        Task<Result<RoleRight>> add(RoleRight t, string userName);
        Task<int> getRoleRightLevel(RoleRightQueryModel condition);
    }
}
