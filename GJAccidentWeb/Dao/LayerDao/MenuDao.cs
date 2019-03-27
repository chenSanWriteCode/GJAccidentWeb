using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using static GJAccidentWeb.Infrastructure.CommonTool;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class MenuDao : IMenuDao
    {
        public Result<List<Menu>> getMenus(Pager<List<Menu>> pager, MenuLevel menuLevel)
        {
            Result<List<Menu>> result = new Result<List<Menu>>();
            string sql_search = getMenuSql_Search(menuLevel, pager);
            DataTable dt_data = null;
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                dt_data = context.QueryTable(sql_search);
                result.data = TableToList_Menu(dt_data, menuLevel);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        public int getMenusCountByParentId(int parentId, MenuLevel menuLevel)
        {
            string sql_search = getMenuSql_Search(menuLevel);
            sql_search += " where parentMenuId=" + parentId;
            int result = 0;
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
        public int getMenusCount(MenuLevel menuLevel)
        {
            string sql_search = getMenuSql_Search(menuLevel);
            sql_search += " where  web='事故报警系统'";
            int result = 0;
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

        public Result<Menu> getSingleMenuById(int id, MenuLevel menuLevel)
        {
            Result<Menu> result = new Result<Menu>();
            string tableName = getMENUTABLE(menuLevel);
            string sql_search = " select f_id, orderid, title, url, open, html, createdby, createddate, lastupdatedby, lastupdateddate ";
            if (menuLevel != MenuLevel.First_Level)
            {
                sql_search += " ,parentMenuId ";
            }
            sql_search += " from " + tableName + " where f_id=" + id;
            DataTable dt_data = null;
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                dt_data = context.QueryTable(sql_search);
                var fakeList = TableToList_Menu(dt_data, menuLevel);
                result.data = fakeList.Count > 0 ? fakeList[0] : new Menu();
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        public Result<Menu> add(Menu model, MenuLevel menuLevel)
        {
            Result<Menu> result = new Result<Menu>();
            string tableName = getMENUTABLE(menuLevel);
            string sql_search = "";
            if (menuLevel != MenuLevel.First_Level)
            {
                sql_search = $"insert into {tableName} (f_id,orderid, title, url, open, html,  createdby, createddate,parentMenuId,web) values(seq_web_menu_second.nextval,{model.orderId},'{model.title}','{model.url}','{model.open}','{model.htmlStr}','{model.createdBy}',to_date('{model.createdDate}','yyyy/MM/dd HH24:mi:ss'),{model.parentMenuId}),'事故报警系统'";
            }
            else
            {
                sql_search = $"insert into {tableName} (f_id,orderid, title, url, open, html,  createdby, createddate,web) values(seq_web_menu_first.nextval,{model.orderId},'{model.title}','{model.url}','{model.open}','{model.htmlStr}','{model.createdBy}',to_date('{model.createdDate}','yyyy/MM/dd HH24:mi:ss'),'事故报警系统')";
            }
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = context.ExecuteSql(sql_search);
                if (count == 0)
                {
                    result.addError($"未添加成功，请检查sql:{sql_search}");
                }
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        public Result<Menu> update(Menu model, MenuLevel menuLevel)
        {
            Result<Menu> result = new Result<Menu>();
            string tableName = getMENUTABLE(menuLevel);
            string sql_search = $" update {tableName} set orderid={model.orderId}, title='{model.title}', url='{model.url}', open='{model.open}', html='{model.htmlStr}', lastupdatedby='{model.lastUpdatedBy}', lastupdateddate=to_date('{model.lastUpdatedDate}','yyyy/MM/dd HH24:mi:ss')";
            if (menuLevel != MenuLevel.First_Level)
            {
                sql_search += $" ,parentMenuId= {model.parentMenuId}";
            }
            sql_search += $" where f_id= { model.id}";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = context.ExecuteSql(sql_search);
                if (count == 0)
                {
                    result.addError($"未修改成功，请检查sql:{sql_search}");
                }
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        public Result<int> delete(int id, MenuLevel menuLevel)
        {
            Result<int> result = new Result<int>();
            string tableName = getMENUTABLE(menuLevel);
            string sql_search = " delete from " + tableName + " where f_id=" + id;
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                result.data = context.ExecuteSql(sql_search);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        #region tool
        /// <summary>
        /// menu datatable to list
        /// </summary>
        /// <param name="source"></param>
        /// <param name="menuLevel"></param>
        /// <returns></returns>
        private List<Menu> TableToList_Menu(DataTable source, MenuLevel menuLevel)
        {
            List<Menu> dataList = new List<Menu>();
            if (source != null)
            {
                Menu model = null;
                foreach (DataRow item in source.Rows)
                {
                    model = new Menu();
                    if (item["lastUpdatedDate"] != DBNull.Value)
                    {
                        model.lastUpdatedDate = Convert.ToDateTime(item["lastUpdatedDate"]);
                    }
                    if (item["createdDate"] != DBNull.Value)
                    {
                        model.createdDate = Convert.ToDateTime(item["createdDate"]);
                    }
                    model.lastUpdatedBy = item["lastUpdatedBy"]!=DBNull.Value?item["lastUpdatedBy"].ToString():null;
                    model.createdBy = item["createdBy"] != DBNull.Value ? item["createdBy"].ToString() : null;
                    model.url = item["url"] != DBNull.Value ? item["url"].ToString() : null;
                    model.title = item["title"] != DBNull.Value ? item["title"].ToString() : null;
                    model.orderId = Convert.ToInt32(item["orderId"]);
                    model.open = item["open"] != DBNull.Value ? item["open"].ToString() : null;
                    model.htmlStr = item["html"] != DBNull.Value ? item["html"].ToString() : null;
                    model.id = Convert.ToInt32(item["f_id"]);
                    if (menuLevel != MenuLevel.First_Level)
                    {
                        model.parentMenuId = Convert.ToInt32(item["parentMenuId"]);
                    }
                    dataList.Add(model);
                }
            }
            return dataList;
        }
        /// <summary>
        /// menu查询sql
        /// </summary>
        /// <param name="menuLevel"></param>
        /// <returns></returns>
        private string getMenuSql_Search(MenuLevel menuLevel, Pager<List<Menu>> pager = null)
        {
            string tableName = getMENUTABLE(menuLevel);
            StringBuilder sql_search = new StringBuilder();
            if (pager != null)
            {
                sql_search.Append(SQL_HEAD);
                sql_search.Append(" select f_id, orderid, title, url, open, html, createdby, createddate, lastupdatedby, lastupdateddate ");
                if (menuLevel!=MenuLevel.First_Level)
                {
                    sql_search.Append(" ,parentMenuId from " + tableName + "  where  web in('事故报警系统','权限') order by parentMenuId,orderId");
                }
                else
                {
                    sql_search.Append("from " + tableName + " where   web in('事故报警系统','权限')  order by orderId");
                }
                sql_search.Append(getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
            }
            else
            {
                //查询总条目
                sql_search.Append(" select count(1) from " + tableName + " ");
            }
            return sql_search.ToString();
        }

        
        #endregion
    }
}