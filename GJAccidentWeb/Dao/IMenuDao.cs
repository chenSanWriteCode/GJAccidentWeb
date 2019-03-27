using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;

namespace GJAccidentWeb.Dao
{
    public interface IMenuDao
    {
        Result<List<Menu>> getMenus(Pager<List<Menu>> pager, MenuLevel menuLevel);
        int getMenusCount(MenuLevel menuLevel);
        Result<Menu> getSingleMenuById(int id, MenuLevel menuLevel);

        Result<Menu> update(Menu model, MenuLevel menuLevel);

        Result<int> delete(int id, MenuLevel menuLevel);
        Result<Menu> add(Menu model, MenuLevel menuLevel);

        int getMenusCountByParentId(int parentId, MenuLevel menuLevel);
    }
}
