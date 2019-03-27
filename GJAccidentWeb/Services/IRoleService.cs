using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Services
{
    public interface IRoleService
    {
        Task<Result<Pager<List<Role>>>> search(Pager<List<Role>> pager, string userName);
        Task<Result<Role>> searchModelByCondition(Role condition);
        Task<Result<RoleUpdateModel>> update(RoleUpdateModel t, string userName);
        Task<Result<int>> delete(string id);
        Task<Result<RoleCreateModel>> add(RoleCreateModel t, string userName);

        Task<Result<Pager<List<UserInfo>>>> searchUsersByRole(Pager<List<UserInfo>> pager, RoleToUsersQueryModel condition);

        Task<Result<int>> DeleteUser(RoleToUsersQueryModel condition);

        Task<Result<RoleToUser>> AddUser(RoleToUser model, string userName);
    }
}
