using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GJAccidentWeb.Dao;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;
using Unity;

namespace GJAccidentWeb.Services.ServiceImpl
{
    public class RoleRightServiceImpl : IRoleRightService
    {
        [Dependency]
        public IRoleRightDao dao { get; set; }

        public async Task<Result<RoleRight>> add(RoleRight t, string userName)
        {
            t.createdBy = userName;
            t.createdDate = DateTime.Now;
            return await Task.Factory.StartNew(() => dao.add(t));
        }

        public async Task<Result<int>> delete(RoleRightQueryModel condition)
        {
            return await Task.Factory.StartNew(() => dao.delete(condition));
        }

        public async Task<int> getRoleRightLevel(RoleRightQueryModel condition)
        {
            return await Task.Factory.StartNew(() => dao.getRoleRightLevel(condition));
        }

        public async Task<Result<Pager<List<RoleRight>>>> search(Pager<List<RoleRight>> pager, RoleRightQueryModel condition)
        {
            Result<Pager<List<RoleRight>>> result = new Result<Pager<List<RoleRight>>>();
            var result_data = await Task.Factory.StartNew(() => dao.search(pager, condition));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.searchCount(condition));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }
    }
}