using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Services;
using GJAccidentWeb.Services.ServiceImpl;
using static GJAccidentWeb.Infrastructure.CommonTool;

namespace GJAccidentWeb.Infrastructure
{
    public enum ModelType
    {
        /// <summary>
        /// 包含所有选项
        /// </summary>
        AllModel = 0,
        /// <summary>
        /// 不包含所有选项
        /// </summary>
        DefaultModel
    }
    public static class BaseDataHelper
    {
        #region 分公司
        /// <summary>
        /// 获取所有分公司
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<string, string> companyInfo_All(this HtmlHelper html, ModelType Type = ModelType.AllModel)
        {
            Dictionary<string, string> dict_company = new Dictionary<string, string>();
            if (Type==ModelType.AllModel)
            {
                dict_company.Add("所有", "0");
            }
            string sql_search = "";
            ORACLEHelper context = null;

            sql_search = $" select d.f_id,d.f_name from gj_depart d  order by d.f_name";
            context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict_company.Add(dr[1].ToString(), dr[0].ToString());
                    }
                }
            }
            catch (Exception) { }
            if (Type == ModelType.AllModel)
            {
                dict_company["所有"] = (dict_company.Count > 1 ? string.Join(",", dict_company.Values.ToList()) : "0");
            }
            return dict_company;
        }
        public static Dictionary<string, string> companyInfo(this HtmlHelper html, string userName, ModelType Type = ModelType.AllModel)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (Type == ModelType.AllModel)
            {
                dict.Add("所有", "0");
            }
            ORACLEHelper context = null;
            var level = getUserDataLevel(userName);
            //用户没有分公司权限
            if (level != (int)DataLevel.Company)
            {
                return dict;
            }
            else
            {
                string sql_search = $"select rrl.rightdata,d.f_name from web_rolerightdetail rrl,gj_depart@dfdb4 d where rrl.rightdata=d.f_id and rrl.roleid=({getRoleId(userName)})";
                context = new ORACLEHelper(1);
                try
                {
                    DataTable dt_line = context.QueryTable(sql_search);
                    if (dt_line != null)
                    {
                        foreach (DataRow dr in dt_line.Rows)
                        {
                            dict.Add(dr[1].ToString(), dr[0].ToString());
                        }
                    }
                }
                catch (Exception) { }
                if (Type == ModelType.AllModel)
                {
                    dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "0");
                }
                return dict;
            }

        }

        /// <summary>
        /// 根据用户获取分公司信息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="database">0 默认；1 money</param>
        /// <returns></returns>
        //public static Dictionary<string, string> companyInfo(this HtmlHelper html, string userName, int database = 0)
        //{
        //    Dictionary<string, string> dict = new Dictionary<string, string>();
        //    dict.Add("所有", "0");
        //    try
        //    {
        //        ORACLEHelper context = new ORACLEHelper();
        //        var level = getUserDataLevel(userName);
        //        //用户没有分公司权限
        //        if (level != (int)DataLevel.Company)
        //        {
        //            return dict;
        //        }
        //        else
        //        {
        //            string sql = $"select rrl.rightdata,d.f_name from web_rolerightdetail rrl,gj_depart d where rrl.rightdata=d.f_id and rrl.roleid=({getRoleId(userName)})";
        //            var dt_data = context.QueryTable(sql);
        //            if (dt_data != null)
        //            {
        //                foreach (DataRow item in dt_data.Rows)
        //                {
        //                    dict.Add(item[1].ToString(), item[0].ToString());
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return dict;
        //    }
        //    dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "0");
        //    return dict;
        //}
        /// <summary>
        /// 获取不在角色内的分公司
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<int, string> getCompanyNotInList(this HtmlHelper html, string roleId)
        {
            Dictionary<int, string> dict_company = new Dictionary<int, string>();
            string sql_search = "";
            ORACLEHelper context = null;

            sql_search = $" select d.f_id, d.f_name  from gj_depart@dfdb4 d where d.f_id not in (select rrd.rightdata from web_rolerightdetail rrd where rrd.roleId = '{roleId}') order by d.f_name";
            context = new ORACLEHelper(1);
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict_company.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
            }
            catch (Exception) { }
            return dict_company;
        }
        #endregion

        #region 线路
        /// <summary>
        /// 获取所有线路信息dict
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<string, string> lineInfo_All(this HtmlHelper html, int companyId = 0)
        {
            Dictionary<string, string> dict_line = new Dictionary<string, string>();
            dict_line.Add("所有", "0");
            string sql_search = "";
            ORACLEHelper context = null;
            sql_search = $"select t.f_id lineId,t.线路名称 lineName from gj_公交线路表 t ";
            if (companyId > 0)
            {
                sql_search += $" where l.所属线路组 in (select f_id from gj_公交线路组 lg where lg.单位id in (select d.f_id from gj_depart d where d.f_id={companyId}))";
            }
            sql_search += " order by t.线路名称";
            context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict_line.Add(dr[1].ToString(), dr[0].ToString());
                    }
                }
            }
            catch (Exception) { }
            dict_line["所有"] = (dict_line.Count > 1 ? string.Join(",", dict_line.Values.ToList()) : "0");
            return dict_line;
        }
        /// <summary>
        /// 获取用户线路信息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="database">0 默认；1 money</param>
        /// <returns></returns>
        public static Dictionary<string, string> lineInfo(this HtmlHelper html, string userName, ModelType type = ModelType.AllModel, int database = 0)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (type == ModelType.AllModel)
            {
                dict.Add("所有", "0");
            }
            var dataLevel = (DataLevel)getUserDataLevel(userName);
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                string sql = "";
                switch (dataLevel)
                {
                    case DataLevel.Company:
                        sql = $"select l.f_id,l.线路名称 from gj_公交线路表@dfdb4 l ,gj_公交线路组@dfdb4 g ,gj_depart@dfdb4 d where l.所属线路组=g.f_id and g.单位id=d.f_id and d.f_id in(select rrl.rightdata from web_rolerightdetail rrl where rrl.roleid=({getRoleId(userName)})) order by l.线路名称";
                        break;
                    case DataLevel.Line:
                        sql = $"select rrl.rightdata,l.线路名称 from web_rolerightdetail rrl,gj_公交线路表@dfdb4 l where rrl.rightdata=l.f_id and rrl.roleid=({getRoleId(userName)}) order by l.线路名称";
                        break;
                    default:
                        return dict;
                }
                var dt_data = context.QueryTable(sql);
                if (dt_data != null)
                {
                    foreach (DataRow item in dt_data.Rows)
                    {
                        dict.Add(item[1].ToString(), item[0].ToString());
                    }
                }
            }
            catch (Exception)
            {
                return dict;
            }
            if (type == ModelType.AllModel)
            {
                dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "0");
            }
            return dict;
        }
        /// <summary>
        /// 获取不在角色权限内的线路
        /// </summary>
        /// <param name="html"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Dictionary<int, string> getLineNotInList(this HtmlHelper html, string roleId, string companyId = null)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            string sql_search = "";
            ORACLEHelper context = null;

            sql_search = $" select l.f_id,l.线路名称  from gj_公交线路表 l@dfdb4 where l.f_id not in  (select rrd.rightdata from web_rolerightdetail rrd where rrd.roleId = '{roleId}')";
            if (!string.IsNullOrEmpty(companyId))
            {
                sql_search += $" and  l.所属线路组 in (select f_id from gj_公交线路组@dfdb4 g where g.单位id in(select f_id from gj_depart@dfdb4 d where d.f_id in({companyId})))";
            }
            sql_search += " order by l.线路名称";
            context = new ORACLEHelper(1);
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
            }
            catch (Exception) { }
            return dict;
        }
        #endregion

        #region 公交车
        /// <summary>
        /// 获取不在权限内的公交车
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Dictionary<int, string> getBusInfoNotInList(this HtmlHelper html, string roleId, string lineId = null)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            string sql_search = "";
            ORACLEHelper context = null;
            sql_search = $"select c.f_id,c.车牌 from gj_公交车@dfdb4 c where  c.是否报废=0 and c.车牌 like '鲁%'  and c.f_id not in (select rrd.rightdata from web_rolerightdetail rrd where rrd.roleid='{roleId}')";
            if (!string.IsNullOrEmpty(lineId) && lineId != "0")
            {
                sql_search += $" and c.运行线路id in ({lineId})";
            }
            sql_search += " order by c.车牌";
            context = new ORACLEHelper(1);
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
            }
            catch (Exception) { }
            return dict;
        }
        /// <summary>
        /// 获取用户公交车信息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="userName"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> busInfo(this HtmlHelper html, string userName, string lineId = null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("所有", "0");
            var dataLevel = (DataLevel)getUserDataLevel(userName);
            try
            {
                ORACLEHelper context = new ORACLEHelper();
                string sql = "";
                switch (dataLevel)
                {
                    case DataLevel.Company:
                        sql = $"select c.f_id,c.车牌  from gj_公交车@dfdb4 c where c.是否报废 = 0   and c.车牌 like '鲁%'   and c.运行线路id in (select l.f_id from gj_公交线路表@dfdb4 l ,gj_公交线路组@dfdb4 g ,gj_depart@dfdb4 d where l.所属线路组=g.f_id and g.单位id=d.f_id and d.f_id in(select rrl.rightdata from web_rolerightdetail rrl where rrl.roleid=({getRoleId(userName)}))) order by c.车牌";
                        break;
                    case DataLevel.Line:
                        sql = $"select c.f_id,c.车牌  from gj_公交车@dfdb4 c where c.是否报废 = 0   and c.车牌 like '鲁%'   and c.运行线路id in (select rrl.rightdata from web_rolerightdetail rrl where rrl.roleid=({getRoleId(userName)}))  order by c.车牌";
                        break;
                    case DataLevel.Bus:
                        sql = $"select rrl.rightdata,c.车牌 from web_rolerightdetail rrl,gj_公交车@dfdb4 c where rrl.rightdata=c.f_id and rrl.roleid=({getRoleId(userName)})  order by c.车牌";
                        break;
                    default:
                        return dict;
                }
                var dt_data = context.QueryTable(sql);
                if (dt_data != null)
                {
                    foreach (DataRow item in dt_data.Rows)
                    {
                        dict.Add(item[1].ToString(), item[0].ToString());
                    }
                }
            }
            catch (Exception)
            {
                return dict;
            }
            dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "0");
            return dict;
        }
        #endregion

        #region 站点
        /// <summary>
        /// 根据用户权限获取站点信息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="userName"></param>
        /// <param name="lineId"></param>
        /// <param name="upOrDown"></param>
        /// <returns></returns>
        public static Dictionary<int, string> stationInfo(this HtmlHelper html, string userName, string lineId = null, string upOrDown = null)
        {
            Dictionary<int, string> dict_station = new Dictionary<int, string>();
            dict_station.Add(0, " ");
            string sql_search = "";
            if (!string.IsNullOrEmpty(lineId) && !lineId.Contains(","))
            {
                sql_search = "select t.f_id,t.名称||'_'||t.地理方向,ls.顺序  from gj_站点 t, gj_线路站点表 ls where t.f_id = ls.站点id  and ls.线路上下行id = (select f_id  from gj_公交线路上下行表 u  where u.线路id in (" + lineId + ")  and u.updown = " + upOrDown + ") order by ls.顺序";
            }
            else
            {
                sql_search = $"select t.f_id,t.名称||'_'||t.地理方向  from gj_站点 t, (select distinct(ls.站点id)  from  gj_线路站点表 ls where ls.线路上下行id in       (select f_id  from gj_公交线路上下行表 u  where u.线路id in  ({html.lineInfo(userName)["所有"]}) and u.updown = 2)) lsu where t.f_id(+) = lsu.站点id   order by t.名称";
            }

            try
            {
                ORACLEHelper context = new ORACLEHelper();
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        dict_station.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                }
            }
            catch (Exception err)
            {
            }
            return dict_station;
        }
        #endregion

        #region 目录
        /// <summary>
        /// 获取上级目录
        /// </summary>
        /// <param name="html"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Dictionary<int, string> parentMenuInfo(this HtmlHelper html, MenuLevel level)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            string sql_search = $"select t.f_id,t.title from {getMENUTABLE(level)} t order by t.orderId";
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                var dt_data = context.QueryTable(sql_search);
                if (dt_data != null)
                {
                    foreach (DataRow item in dt_data.Rows)
                    {
                        dict.Add(Convert.ToInt32(item[0]), item[1].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return dict;

        }
        #endregion

        #region 权限
        /// <summary>
        /// 获取所有未分配角色的用户
        /// 住：一个用户只能配一个角色
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<string, string> userNotInRole(this HtmlHelper html,string userName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = $"select t.f_id,t.username from web_users t where t.f_id not in (select userid from web_userrole) and t.web='事故报警系统' and t.createdby='{userName}' order by t.username";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_data = context.QueryTable(sql);
                if (dt_data != null)
                {
                    foreach (DataRow item in dt_data.Rows)
                    {
                        dict.Add(item[0].ToString(), item[1].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return dict;
        }
        /// <summary>
        /// 获取所有角色字典
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<string, string> roleInfo(this HtmlHelper html)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = "select t.f_id,t.rolename from web_role t where t.web='事故报警系统' order by t.rolename";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_data = context.QueryTable(sql);
                if (dt_data != null)
                {
                    foreach (DataRow item in dt_data.Rows)
                    {
                        dict.Add(item[0].ToString(), item[1].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return dict;
        }
        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="html"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string getUserIdByName(this HtmlHelper html, string userName)
        {
            string sql = $"select t.f_id from web_users t where t.username='{userName}'";
            string userId = null;
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                userId = context.GetSingle(sql).ToString();
            }
            catch (Exception)
            {
            }
            return userId;
        }
        /// <summary>
        /// 获取数据权限的级别
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static Dictionary<int, string> getRightLevel(this HtmlHelper html)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var data = context.QueryTable("select f_id,rightLevel from web_rightlevel");
                if (data != null)
                {
                    foreach (DataRow item in data.Rows)
                    {
                        dict.Add(Convert.ToInt32(item[0]), item[1].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return dict;
        }

        /// <summary>
        /// 用户所属角色
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string getRoleId(string userName)
        {
            return $"select ur.roleid from web_userrole ur where ur.userid =(select u.f_id from web_users u where u.username='{userName}')";
        }
        /// <summary>
        /// 用户数据权限等级
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int getUserDataLevel(string userName)
        {
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                string sql = $"select rrl.rightlevelid from web_rolerightlevel rrl where rrl.roleid=({getRoleId(userName)})";
                return Convert.ToInt32(context.GetSingle(sql));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

    }
}