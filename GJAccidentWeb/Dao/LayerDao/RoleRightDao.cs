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
    public class RoleRightDao : IRoleRightDao
    {
        public Result<RoleRight> add(RoleRight model)
        {
            Result<RoleRight> result = new Result<RoleRight>();
            string sql = $"insert into web_rolerightdetail (f_id, roleid, rightdata, createdby, createddate) values(seq_web_rolerightdetail.nextval ,'{model.roleId}',{model.rightId} ,'{model.createdBy}',to_date('{model.createdDate}','yyyy/MM/dd HH24:mi:ss'))";
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

        public Result<int> delete(RoleRightQueryModel condition)
        {
            Result<int> result = new Result<int>();
            string sql = "delete from web_rolerightdetail t where 1=1";
            if (condition.id > 0)
            {
                sql += $" and t.f_id={condition.id}";
            }
            if (!string.IsNullOrEmpty(condition.roleId))
            {
                sql += $" and t.roleid='{condition.roleId}'";
            }
            if (condition.rightdata > 0)
            {
                sql += $" and t.rightdata={condition.rightdata}";
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

        public Result<List<RoleRight>> search(Pager<List<RoleRight>> pager, RoleRightQueryModel condition)
        {
            Result<List<RoleRight>> result = new Result<List<RoleRight>>();
            List<RoleRight> dataList = new List<RoleRight>();
            StringBuilder sql_search = new StringBuilder(SQL_HEAD);
            sql_search.Append($"select t.f_id, t.roleid,  decode((select rrl.rightlevelid  from web_rolerightlevel rrl where rrl.roleid='{condition.roleId}'), 1,  (select f_name from gj_depart@dfdb4 d where d.f_id = t.rightdata),2,(select l.线路名称  from gj_公交线路表@dfdb4 l  where l.f_id = t.rightdata), 3, (select c.车牌 from gj_公交车@dfdb4 c where c.f_id = t.rightdata)) rightName,t.createdBy,t.createddate  from web_rolerightdetail t where t.roleid = '{condition.roleId}' order by rightName ");
            sql_search.Append(getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data != null)
                {
                    RoleRight model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new RoleRight();
                        model.id = Convert.ToInt32(item["f_id"]);
                        model.roleId = item["roleId"].ToString();
                        model.rightName = item["rightName"].ToString();
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

        public int getRoleRightLevel(RoleRightQueryModel condition)
        {
            int result = 2;
            string sql_search = $"select rrl.rightlevelid  from web_rolerightlevel rrl where rrl.roleid='{condition.roleId}'";
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
        public int searchCount(RoleRightQueryModel condition)
        {
            int result = 0;
            string sql_search = $"select count(1)  from web_rolerightdetail t where t.roleid = '{condition.roleId}'";
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