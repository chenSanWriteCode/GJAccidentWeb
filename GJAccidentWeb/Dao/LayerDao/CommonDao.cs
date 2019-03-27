using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models;
using log4net;
using static GJAccidentWeb.Infrastructure.CommonTool;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class CommonDao : ICommonDao
    {
        private ILog log = LogManager.GetLogger("CommonDao");
        /// <summary>
        /// 根据线路查公交车
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="lineName">线路id集合</param>
        /// <returns></returns>
        public List<Car> carInfo(string userName, string lineName)
        {
            string sql_search = "";
            List<Car> result = new List<Car>();
            ORACLEHelper context = null;
            string lineId = lineName.Contains(",") ? "0" : lineName;
            sql_search = $"select c.f_id,c.车牌 from gj_公交车 c where c.线路id in ({lineId}) and c.是否报废=0 order by c.车牌";
            context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        result.Add(new Car { carId= dr[0].ToString(), carNum= dr[1].ToString() });
                    }
                }
            }
            catch (Exception err) { log.Error(err.Message); }
            return result;
        }

        public List<Depart> departInfo()
        {
            List<Depart> result = new List<Depart>();
            string sql_search = "";
            ORACLEHelper context = null;

            sql_search = "select t.f_id,t.f_name from gj_depart t order by t.f_id";
            context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        result.Add(new Depart { departId = dr[0].ToString(), departName = dr[1].ToString() });
                    }
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message);
            }
            return result;
        }

        

        public List<LineUD> lineUDInfo(string lineName)
        {
            List<LineUD> result = new List<LineUD>();
            string sql_search = "";
            ORACLEHelper context = null;
            string lineId = lineName.Contains(",") ? "0" : lineName;
            sql_search = $"select ud.f_id,ud.公交线路,ud.方向 from gj_公交线路上下行表 ud where ud.线路id={lineId} order by ud.updown";
            context = new ORACLEHelper();
            try
            {
                DataTable dt = context.QueryTable(sql_search);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(new LineUD { UDID = Convert.ToInt32(dr[0]), LineDir = dr[1].ToString(),UpDown= dr[2].ToString() });
                    }
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message);
            }
            return result;
        }

        public List<Line> lineInfo(string companyId, int database = 0)
        {
            List<Line> result = new List<Line>();

            string sql_search = "";
            ORACLEHelper context = null;
            sql_search = " select l.f_id,l.线路名称 from gj_depart d,gj_公交线路组 g,gj_公交线路表 l where l.所属线路组=g.f_id and g.单位id=d.f_id";
            if (!string.IsNullOrEmpty(companyId))
            {
                sql_search += $" and d.f_id in ({companyId})";
            }
            sql_search += "  order by l.线路名称";
            context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        result.Add(new Line { lineId = dr[0].ToString(), lineName = dr[1].ToString() });
                    }
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message);
            }
            
            return result;
        }
        public List<Line> getLineInfoNotInList(string companyId, string roleId)
        {
            List<Line> dict_company = new List<Line>();
            string sql_search = "";
            ORACLEHelper context = null;
            sql_search = $" select l.f_id,l.线路名称 from gj_depart@dfdb4 d,gj_公交线路组@dfdb4 g,gj_公交线路表@dfdb4 l where l.所属线路组=g.f_id and g.单位id=d.f_id  and l.f_id not in (select rrd.rightdata from web_rolerightdetail rrd where rrd.roleid='{roleId}')";
            if (!string.IsNullOrEmpty(companyId))
            {
                sql_search += $" and d.f_id in ({companyId})";
            }
            sql_search += "  order by l.线路名称";
            context = new ORACLEHelper(1);
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    Line model;
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        model = new Line();
                        model.lineId = dr[0].ToString();
                        model.lineName = dr[1].ToString();
                        dict_company.Add(model);
                    }
                }
            }
            catch (Exception) { }
            return dict_company;
        }
        public List<Car> getBusInfoNotInList(string lineId, string roleId)
        {
            List<Car> dict = new List<Car>();
            string sql_search = "";
            ORACLEHelper context = null;
            sql_search = $"select c.f_id,c.车牌 from gj_公交车@dfdb4 c where  c.是否报废=0 and c.车牌 like '鲁%' and c.f_id not in (select rrd.rightdata from web_rolerightdetail rrd where rrd.roleid='{roleId}')";
            if (!string.IsNullOrEmpty(lineId))
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
                    Car model;
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        model = new Car();
                        model.carId = dr[0].ToString();
                        model.carNum = dr[1].ToString();
                        dict.Add(model);
                    }
                }
            }
            catch (Exception) { }
            return dict;
        }

        public List<MenuModel> getMenuInfo(string userName)
        {
            List<Menu> menu_first = getLevelMenuInfo(MenuLevel.First_Level, userName);
            List<Menu> menu_second = getLevelMenuInfo(MenuLevel.Second_Level, userName);
            List<MenuModel> menu = new List<MenuModel>();
            MenuModel model = null;
            foreach (var item in menu_first)
            {
                model = new MenuModel();
                model.title = item.title;
                model.url = item.url;
                model.html = item.htmlStr;
                model.open = item.open == "true";
                model.children = menu_second.Where(x => x.parentMenuId == item.id).Select(x => new MenuModel
                {
                    title = x.title,
                    url = x.url,
                    html = x.htmlStr,
                    //open = x.open == "true",
                }).ToList();
                menu.Add(model);
            }
            return menu;
        }
        private List<Menu> getLevelMenuInfo(MenuLevel level, string userName)
        {
            List<Menu> result = new List<Menu>();
            DataTable dt_data = null;
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                string roleId = context.GetSingle($"select f_id from web_role t where t.f_id =(select roleId from web_userrole r where r.userId=(select f_id from web_users u where u.username='{userName}'))").ToString();
                string sql_search = getSql_LevelMenu(level, roleId);
                dt_data = context.QueryTable(sql_search);
                if (dt_data != null)
                {
                    Menu model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new Menu();
                        model.id = Convert.ToInt32(item["f_id"]);
                        model.title = item["title"].ToString();
                        model.url = item["url"].ToString();
                        model.open = item["open"].ToString();
                        model.htmlStr = item["html"].ToString();
                        if (level != MenuLevel.First_Level)
                        {
                            model.parentMenuId = Convert.ToInt32(item["parentMenuId"]);
                        }
                        result.Add(model);
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        private string getSql_LevelMenu(MenuLevel level, string roleId)
        {
            string tableName = getMENUTABLE(level);
            StringBuilder sql_search = new StringBuilder();
            sql_search.Append(" select f_id, title, url, open, html ");
            if (level != MenuLevel.First_Level)
            {
                sql_search.Append(" ,parentMenuId ");
            }
            sql_search.Append($" from {tableName} where f_id in(select menuid from web_rolemenu rm where rm.roleid='{roleId}') order by ");
            if (level != MenuLevel.First_Level)
            {
                sql_search.Append("  parentMenuId,");
            }
            sql_search.Append(" orderId");
            return sql_search.ToString();
        }

        public List<Station> getStationInfo(string lineId, string upOrDown = null)
        {
            List<Station> dict_station = new List<Station>();
            dict_station.Add(new Station() { stationId = 0, stationName = " " });
            string sql_search = "";
            if (!string.IsNullOrEmpty(lineId) && !lineId.Contains(","))
            {
                sql_search = "select t.f_id,t.名称,ls.顺序  from gj_站点 t, gj_线路站点表 ls where t.f_id = ls.站点id  and  t.名称 not like '%拐点%'  and t.名称 not like '%_下行%'  and t.名称 not like '%_上行%' and ls.线路上下行id = (select f_id  from gj_公交线路上下行表 u  where u.线路id in (" + lineId + ")  and u.updown = " + upOrDown + ") order by ls.顺序";
            }
            else
            {
                sql_search = $"select t.f_id,t.名称||'_'||t.地理方向  from gj_站点 t, (select distinct(ls.站点id)  from  gj_线路站点表 ls where ls.线路上下行id in  (select f_id  from gj_公交线路上下行表 u  where u.线路id in  ({lineId}) and u.updown = {upOrDown})) lsu where t.f_id(+) = lsu.站点id   order by t.名称";
            }
            ORACLEHelper context = new ORACLEHelper();
            try
            {
                DataTable dt_line = context.QueryTable(sql_search);
                if (dt_line != null)
                {
                    Station model;
                    foreach (DataRow dr in dt_line.Rows)
                    {
                        model = new Station();
                        model.stationId = Convert.ToInt32(dr[0]);
                        model.stationName = dr[1].ToString();
                        dict_station.Add(model);
                    }
                }
            }
            catch (Exception) { }
            return dict_station;
        }

        public List<MenuModel> getMenuList(string userName)
        {
            List<Menu> menu_first = getLevelMenuInfo(MenuLevel.First_Level, userName);
            List<Menu> menu_second = getLevelMenuInfo(MenuLevel.Second_Level, userName);
            List<MenuModel> menu = new List<MenuModel>();
            MenuModel model = null;
            foreach (var item in menu_first)
            {
                model = new MenuModel();
                model.title = item.title;
                model.url = item.url;
                model.html = item.htmlStr;
                model.open = item.open == "true";
                menu.Add(model);
            }
            foreach (var item in menu_second)
            {
                model = new MenuModel();
                model.title = item.title;
                model.url = item.url;
                model.html = item.htmlStr;
                model.open = item.open == "true";
                menu.Add(model);
            }
            return menu;
        }

        public Depart getDepartInfoByLine(Line line)
        {
            Depart resut = new Depart();
            ORACLEHelper context;
            try
            {
                context = new ORACLEHelper();
                string sql = $"select t.f_id,t.f_name from gj_depart t,gj_公交线路组 g,gj_公交线路表 l where l.所属线路组=g.f_id and g.单位id=t.f_id";
                if (!string.IsNullOrEmpty(line.lineId))
                {
                    sql += $" and l.f_id={line.lineId}";
                }
                if (!string.IsNullOrEmpty(line.lineName))
                {
                    sql += $" and l.线路名称={line.lineName}";
                }
                sql += " order by t.f_name";
                var data = context.QueryTable(sql);
                if (data!=null)
                {
                    resut.departId = data.Rows[0][0].ToString();
                    resut.departName = data.Rows[0][1].ToString();
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message);
            }
            return resut;
        }
        #region 注释 lineInfo(string userName, string companyId)
        //public List<Line> lineInfo(string userName, string companyId)
        //{
        //    List<Line> result = new List<Line>();
        //    string sql_search = "";
        //    ORACLEHelper context = null;

        //    sql_search = $"select l.f_id,l.线路名称 from gj_公交线路表 l where l.所属线路组 in (select f_id from gj_公交线路组 g where g.单位id={companyId}) order by l.线路名称";
        //    context = new ORACLEHelper();
        //    try
        //    {
        //        DataTable dt_line = context.QueryTable(sql_search);
        //        if (dt_line != null)
        //        {
        //            foreach (DataRow dr in dt_line.Rows)
        //            {
        //                result.Add(new Line { lineId= dr[0].ToString(), lineName=dr[1].ToString() });
        //            }
        //        }
        //    }
        //    catch (Exception err) {
        //        log.Error(err.Message);
        //    }
        //    return result;
        //}
        #endregion
    }
}