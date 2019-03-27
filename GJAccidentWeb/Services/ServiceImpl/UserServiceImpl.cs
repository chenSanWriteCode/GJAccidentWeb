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
    public class UserServiceImpl : IUserService
    {
        [Dependency]
        public IUserDao dao { get; set; }
        [Dependency]
        public IRoleDao roleDao { get; set; }
        public async Task<Result<UserCreateModel>> add(UserCreateModel t, string userName)
        {
            Result<UserCreateModel> result = new Result<UserCreateModel>();
            UserInfo model = new UserInfo
            {
                createdBy = userName,
                createdDate = DateTime.Now,
                //id = Guid.NewGuid().ToString(),
                userName = t.userName,
                phoneNum = t.phoneNum,
                email = t.email,
                password = t.password,
                companyId = t.companyId,
                userNo = t.userNo
            };
            var result_back= await Task.Factory.StartNew(() => dao.add(model));
            if (!result_back.success)
            {
                result.addError(result_back.message);
            }
            return result;
        }
        /// <summary>
        ///修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Result<UserChangePasswordModel>> changePassword(UserChangePasswordModel model, string userName)
        {
            Result<UserChangePasswordModel> result = new Result<UserChangePasswordModel>();
            UserInfo old = new UserInfo
            {
                id = model.id,
                password = model.oldPassword,
                lastUpdatedBy = userName,
                lastUpdatedDate = DateTime.Now
            };
            var result_back = await Task.Factory.StartNew(() => dao.changePassword(old, model.confirmPassword));
            if (!result_back.success)
            {
                result.addError(result_back.message);
            }
            return result;
        }

        /// <summary>
        /// 删除用户
        /// 1. 判断用户是否有对应的权角色，如果有，删除角色关系
        /// 2. 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<int>> delete(string id)
        {
            Result<int> result = new Result<int>();
            var result_role = await Task.Factory.StartNew(() => roleDao.DeleteUser(new RoleToUsersQueryModel { userId = id }));
            if (result_role.success)
            {
                var result_user = await Task.Factory.StartNew(() => dao.delete(id));
                if (!result_user.success)
                {
                    result.addError(result_user.message);
                }
            }
            else
            {
                result.addError(result_role.message);
            }
            return result;
        }

        public async Task<Result<int>> passwordSignIn(UserInfo user)
        {
            return await Task.Factory.StartNew(() => dao.passwordSignIn(user));
        }

        public async Task<Result<Pager<List<UserInfo>>>> search(Pager<List<UserInfo>> pager, string userName)
        {
            Result<Pager<List<UserInfo>>> result = new Result<Pager<List<UserInfo>>>();
            var result_data = await Task.Factory.StartNew(() => dao.search(pager,userName));
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

        public async Task<Result<UserInfo>> searchModelByCondition(UserInfo condition)
        {
            return await Task.Factory.StartNew(()=> dao.searchModelByCondition(condition));
        }

        public async Task<Result<UserUpdateModel>> update(UserUpdateModel t, string userName)
        {
            Result<UserUpdateModel> result = new Result<UserUpdateModel>();
            UserInfo model = new UserInfo
            {
                lastUpdatedBy = userName,
                lastUpdatedDate = DateTime.Now,
                id = t.id,
                userName = t.userName,
                phoneNum = t.phoneNum,
                email = t.email,
                companyId=t.companyId,
                userNo=t.userNo
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