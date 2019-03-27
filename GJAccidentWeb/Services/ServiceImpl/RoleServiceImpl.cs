using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GJAccidentWeb.Dao;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;
using GJAccidentWeb.Models.QueryModels;
using Unity;

namespace GJAccidentWeb.Services.ServiceImpl
{
    public class RoleServiceImpl : IRoleService
    {
        [Dependency]
        public IRoleDao dao  { get; set; }
        [Dependency]
        public IRoleMenuDao roleMenuDao { get; set; }
        [Dependency]
        public IRoleRightDao roleRightDao { get; set; }
        public async Task<Result<RoleCreateModel>> add(RoleCreateModel t, string userName)
        {
            Result<RoleCreateModel> result = new Result<RoleCreateModel>();
            Role model = new Role
            {
                createdBy = userName,
                createdDate = DateTime.Now,
                id = Guid.NewGuid().ToString(),
                roleName = t.roleName,
                level=t.level
            };
            var result_back = await Task.Factory.StartNew(() => dao.add(model));
            if (!result_back.success)
            {
                result.addError(result_back.message);
            }
            return result;
        }

        public async Task<Result<RoleToUser>> AddUser(RoleToUser model, string userName)
        {
            model.createdBy = userName;
            model.createdDate = DateTime.Now;
            return await Task.Factory.StartNew(() => dao.AddUser(model));
        }
        /// <summary>
        /// 删除角色
        /// 同时删除角色对应的模块、角色对应的权利
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<int>> delete(string id)
        {
            Result<int> result = new Result<int>();
            var result_roleModel = await Task.Factory.StartNew(() => dao.searchModelByCondition(new Role { id = id }));
            if (result_roleModel.success && !result_roleModel.data.roleName.Contains("超级管理员"))
            {
                var result_roleRight = await Task.Factory.StartNew(() => roleRightDao.delete(new RoleRightQueryModel { roleId = id }));
                var result_roleMenu = await Task.Factory.StartNew(() => roleMenuDao.delete(new RoleMenuQueryModel { roleId = id }));
                return await Task.Factory.StartNew(() => dao.delete(id));
            }
            else
            {
                result.addError(result_roleModel.message?? "您没有权限删除超级管理员");
                return result;
            }
            
        }

        public async Task<Result<int>> DeleteUser(RoleToUsersQueryModel condition)
        {
            return await Task.Factory.StartNew(() => dao.DeleteUser(condition));
        }

        public async Task<Result<Pager<List<Role>>>> search(Pager<List<Role>> pager,string userName)
        {
            Result<Pager<List<Role>>> result = new Result<Pager<List<Role>>>();
            var result_data = await Task.Factory.StartNew(() => dao.search(pager, userName));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.searchCount(userName));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }

        public async Task<Result<Role>> searchModelByCondition(Role condition)
        {
            return await Task.Factory.StartNew(() => dao.searchModelByCondition(condition));
        }

        public async Task<Result<Pager<List<UserInfo>>>> searchUsersByRole(Pager<List<UserInfo>> pager, RoleToUsersQueryModel condition)
        {
            Result<Pager<List<UserInfo>>> result = new Result<Pager<List<UserInfo>>>();
            var result_data = await Task.Factory.StartNew(() => dao.searchUsersByRole(pager, condition));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.searchUsersCount(condition));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }

        public async Task<Result<RoleUpdateModel>> update(RoleUpdateModel t, string userName)
        {
            Result<RoleUpdateModel> result = new Result<RoleUpdateModel>();
            Role model = new Role
            {
                lastUpdatedBy = userName,
                lastUpdatedDate = DateTime.Now,
                id = t.id,
                roleName = t.roleName,
                level = t.level
            };
            var result_back = await Task.Factory.StartNew(() => dao.update(model));
            if (!result_back.success)
            {
                result.addError(result_back.message);
            }
            return result;
        }
    }
}