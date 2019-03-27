using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;

namespace GJAccidentWeb.Services
{
    public interface IUserService
    {
        Task<Result<UserUpdateModel>> update(UserUpdateModel t, string userName);
        Task<Result<int>> delete(string id);
        Task<Result<UserCreateModel>> add(UserCreateModel t, string userName);
        Task<Result<Pager<List<UserInfo>>>> search(Pager<List<UserInfo>> pager,string userName);
        Task<Result<UserInfo>> searchModelByCondition(UserInfo condition);
        Task<Result<UserChangePasswordModel>> changePassword(UserChangePasswordModel model, string userName);
        /// <summary>
        /// 用户密码登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Result<int>> passwordSignIn(UserInfo user);
    }
}
