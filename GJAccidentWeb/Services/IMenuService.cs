using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;

namespace GJAccidentWeb.Services
{
    public interface IMenuService
    {
        Task<Result<Pager<List<Menu>>>> getMenus(Pager<List<Menu>> pager,MenuLevel menuLevel);

        Task<Result<Menu>> getSingleMenuById(int id, MenuLevel menuLevel);

        Task<Result<Menu>> update(Menu model, string userName, MenuLevel menuLevel);

        Task<Result<int>> delete(int id, MenuLevel menuLevel);
        Task<Result<Menu>> add(Menu model, string userName, MenuLevel menuLevel);
        Task<int> getMenusCountByParentId(int parentId, MenuLevel level);
    }
}
