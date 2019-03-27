using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Services
{
    public interface IRoleMenuService
    {
        Task<Result<Pager<List<RoleMenu>>>> search(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition);

        Task<Result<int>> delete(RoleMenuQueryModel condition);
        Task<Result<RoleMenu>> add(RoleMenu model, string userName);
        Task<Result<Pager<List<Menu>>>> searchNotInList(Pager<List<Menu>> pager, RoleMenuQueryModel condition);
    }
}
