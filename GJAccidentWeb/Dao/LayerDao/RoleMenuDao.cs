using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models.QueryModels;
using static GJAccidentWeb.Infrastructure.CommonTool;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class RoleMenuDao : IRoleMenuDao
    {
        public Result<RoleMenu> add(RoleMenu model)
        {
            Result<RoleMenu> result = new Result<RoleMenu>();
            string sql = $"insert into web_rolemenu (f_id, roleid, menulevel, menuid, createdby, createddate) values(seq_web_rolemenu.nextval ,'{model.roleId}',{model.menuLevel} ,{model.menuId} ,'{model.createdBy}',to_date('{model.createdDate}','yyyy/MM/dd HH24:mi:ss'))";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<int> delete(RoleMenuQueryModel condition)
        {
            Result<int> result = new Result<int>();
            string sql = "delete from web_rolemenu t where 1=1";
            if (condition.id>0)
            {
                sql += $" and t.f_id={condition.id}";
            }
            if (!string.IsNullOrEmpty(condition.roleId))
            {
                sql += $" and t.roleid='{condition.roleId}'";
            }
            if (condition.menuId>0)
            {
                sql += $" and t.menuid={condition.menuId}";
            }
            if (condition.menuLevel>0)
            {
                sql += $" and t.menulevel={condition.menuLevel} ";
            }
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<List<RoleMenu>> search(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition)
        {
            Result<List<RoleMenu>> result = new Result<List<RoleMenu>>();
            List<RoleMenu> dataList = new List<RoleMenu>();
            StringBuilder sql_search = new StringBuilder(SQL_HEAD);
            sql_search.Append($"select f_id, roleid, menulevel,decode(t.menulevel,1,'一级目录',2,'二级目录') menuLevelName, menuid,decode(t.menulevel,1,(select mf.title from web_menu_first mf where mf.f_id=t.menuid ) ,2,(select mf.title from web_menu_second mf where mf.f_id=t.menuid)) menuTitle, createdby, createddate from web_rolemenu t where t.roleid = '{condition.roleId}' order by menulevel,menuTitle");
            sql_search.Append(getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data!=null)
                {
                    RoleMenu model = null;
                    foreach (DataRow  item in dt_data.Rows)
                    {
                        model = new RoleMenu();
                        model.id = Convert.ToInt32(item["f_id"]);
                        model.menuId = Convert.ToInt32(item["menuid"]);
                        model.menuLevel = Convert.ToInt32(item["menulevel"]);
                        model.roleId = item["roleid"].ToString();
                        model.menuLevelName = item["menuLevelName"].ToString();
                        model.menuTitle = item["menuTitle"].ToString();
                        model.createdBy = item["createdby"].ToString();
                        model.createdDate = Convert.ToDateTime(item["createddate"]);
                        dataList.Add(model);
                    }
                }
                result.data = dataList;
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public int searchCount(RoleMenuQueryModel condition)
        {
            int result = 0;
            string tableName = getMENUTABLE((MenuLevel)condition.menuLevel);
            string sql_search=$"select count(1) from web_rolemenu t where t.roleid = '{condition.roleId}'";
            //if (condition.parentMenuId > 0)
            //{
            //    sql_search+=($" and t.parentmenuid={condition.parentMenuId}");
            //}
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                result = Convert.ToInt32(context.GetSingle(sql_search));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public Result<List<Menu>> searchNotInList(Pager<List<Menu>> pager, RoleMenuQueryModel condition)
        {
            Result<List<Menu>> result = new Result<List<Menu>>();
            List<Menu> dataList = new List<Menu>();
            StringBuilder sql_search = new StringBuilder(SQL_HEAD);
            string tableName = getMENUTABLE((MenuLevel)condition.menuLevel);
            sql_search.Append($"select t.f_id, t.title,nvl(m.f_id,0) isIn  from  {tableName} t,(select rm.f_id,rm.menuid from web_rolemenu rm where rm.roleid = '{condition.roleId}' and rm.menulevel ={condition.menuLevel}) m where t.f_id=m.menuid(+) and t.web in('事故报警系统','权限') ");
            //sql_search.Append($"select  f_id,  title  from {tableName} t where t.f_id not in (select menuid from web_rolemenu rm where rm.roleid='{condition.roleId}' and rm.menulevel={condition.menuLevel})");
            if (condition.parentMenuId>0)
            {
                sql_search.Append($" and t.parentmenuid={condition.parentMenuId}");
            }
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                string sql_role = $"select roleName from web_role where f_id='{condition.roleId}'";
                string roleName = context.GetSingle(sql_role).ToString();
                if (!roleName.Contains("超级管理员"))
                {
                    sql_search.Append(" and t.title not like '%目录%' ");
                }
                sql_search.Append(" order by t.title ");
                sql_search.Append(getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
                var dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data != null)
                {
                    Menu model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new Menu();
                        model.title = item["title"] != DBNull.Value ? item["title"].ToString() : null;
                        model.id = Convert.ToInt32(item["f_id"]);
                        model.isIn = Convert.ToInt32(item["isIn"]);
                        dataList.Add(model);
                    }
                }
                result.data = dataList;
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public int searchNotInListCount(RoleMenuQueryModel condition)
        {
            int result = 0;
            string tableName = getMENUTABLE((MenuLevel)condition.menuLevel);
            string sql_search = $"select  count(1)  from {getMENUTABLE((MenuLevel)condition.menuLevel)} t where and t.web in('事故报警系统','权限')";
            if (condition.parentMenuId > 0)
            {
                sql_search+=($" and t.parentmenuid={condition.parentMenuId}");
            }
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                result = Convert.ToInt32(context.GetSingle(sql_search));
            }
            catch (Exception)
            {
            }
            return result;
        }

        
    }
}