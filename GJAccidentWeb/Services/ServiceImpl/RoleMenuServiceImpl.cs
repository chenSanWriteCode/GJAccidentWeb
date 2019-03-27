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
    public class RoleMenuServiceImpl : IRoleMenuService
    {
        [Dependency]
        public IRoleMenuDao dao { get; set; }
        public async Task<Result<RoleMenu>> add(RoleMenu model, string userName)
        {
            model.createdBy = userName;
            model.createdDate = DateTime.Now;
            return await Task.Factory.StartNew(() => dao.add(model));
        }

        public async Task<Result<int>> delete(RoleMenuQueryModel condition)
        {
            return await Task.Factory.StartNew(() => dao.delete(condition));
        }

        public async Task<Result<Pager<List<RoleMenu>>>> search(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition)
        {
            Result<Pager<List<RoleMenu>>> result = new Result<Pager<List<RoleMenu>>>();
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

        public async Task<Result<Pager<List<Menu>>>> searchNotInList(Pager<List<Menu>> pager, RoleMenuQueryModel condition)
        {
            Result<Pager<List<Menu>>> result = new Result<Pager<List<Menu>>>();
            var result_data = await Task.Factory.StartNew(() => dao.searchNotInList(pager, condition));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.searchNotInListCount(condition));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }

        
    }
}