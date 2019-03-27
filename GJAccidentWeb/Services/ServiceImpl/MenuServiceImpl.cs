using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GJAccidentWeb.Dao;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using Unity;

namespace GJAccidentWeb.Services.ServiceImpl
{
    public class MenuServiceImpl : IMenuService
    {
        [Dependency]
        public IMenuDao  dao { get; set; }

        public async Task<Result<Pager<List<Menu>>>> getMenus(Pager<List<Menu>> pager, MenuLevel menuLevel)
        {
            Result<Pager<List<Menu>>> result = new Result<Pager<List<Menu>>>();
            var result_data = await Task.Factory.StartNew(() => dao.getMenus(pager, menuLevel));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.getMenusCount(menuLevel));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }
        public async Task<int> getMenusCountByParentId(int parentId, MenuLevel level)
        {
            return await Task.Factory.StartNew(() => dao.getMenusCountByParentId(parentId, level));
        }
        public async Task<Result<Menu>> getSingleMenuById(int id, MenuLevel menuLevel)
        {
            return await Task.Factory.StartNew(() => dao.getSingleMenuById(id, menuLevel));
        }

        public async Task<Result<Menu>> update(Menu model, string userName, MenuLevel menuLevel)
        {
            model.lastUpdatedBy = userName;
            model.lastUpdatedDate = DateTime.Now;
            return await Task.Factory.StartNew(() => dao.update(model, menuLevel));
        }
        public async Task<Result<int>> delete(int id, MenuLevel menuLevel)
        {
            return await Task.Factory.StartNew(() => dao.delete(id, menuLevel));
        }

        public async Task<Result<Menu>> add(Menu model, string userName, MenuLevel menuLevel)
        {
            model.createdBy = userName;
            model.createdDate = DateTime.Now;
            return await Task.Factory.StartNew(() => dao.add(model, menuLevel));
        }
    }
}