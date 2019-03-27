using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models.QueryModels;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class RoleDao : IRoleDao
    {
        public Result<Role> add(Role t)
        {
            Result<Role> result = new Result<Role>();
            string sql = $"insert into web_role (f_id, roleName, createdby, createddate,web) values('{t.id}','{t.roleName}','{t.createdBy}',to_date('{t.createdDate}','yyyy/MM/dd HH24:mi:ss'),'事故报警系统')";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                int count = Convert.ToInt32(context.GetSingle($"select count(1) from web_role t where t.rolename='{t.roleName}'"));
                if (count > 0)
                {
                    result.addError($"角色名{t.roleName}已存在");
                    return result;
                }
                count = context.ExecuteSql(sql);
                //角色数据级别
                sql =$"insert into web_rolerightlevel values((select f_id from web_role r where r.rolename='{t.roleName}'),{t.level})";
                count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        /// <summary>
        /// 1. 查询用户是否配有角色
        /// 2. 在没有的情况下配置角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<RoleToUser> AddUser(RoleToUser model)
        {
            Result<RoleToUser> result = new Result<RoleToUser>();
            string sql = $"select (select r.rolename from web_role r where r.f_id=ur.roleid) rolename from web_userrole ur where ur.roleid='{model.userId}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_userRole = context.QueryTable(sql);
                if (dt_userRole.Rows.Count > 0)
                {
                    result.addError($"该用户已具备{dt_userRole.Rows[0][0].ToString()}的角色，请先解除本角色");
                    return result;
                }
                sql = $"insert into web_userRole (userid, roleid, createdby, createddate) values('{model.userId}','{model.roleId}','{model.createdBy}',to_date('{model.createdDate}','yyyy/MM/dd HH24:mi:ss'))";
                var count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
        
        public Result<int> delete(string id)
        {
            Result<int> result = new Result<int>();
            //删除数据权限级别
            string sql = $" delete from web_rolerightlevel t where t.roleid='{id}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = context.ExecuteSql(sql);
                sql = $"delete from web_userrole t where t.roleid='{id}' ";
                count = context.ExecuteSql(sql);

                sql = $"delete from web_role t where t.f_id='{id}'";
                count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<int> DeleteUser(RoleToUsersQueryModel condition)
        {
            Result<int> result = new Result<int>();
            string sql = $"delete from web_userrole t where t.userid='{condition.userId}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = Convert.ToInt32(context.GetSingle(sql));
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<List<Role>> search(Pager<List<Role>> pager, string userName)
        {
            Result<List<Role>> result = new Result<List<Role>>();
            StringBuilder sql_search = new StringBuilder(CommonTool.SQL_HEAD);
            sql_search.Append($" select t.f_id, t.rolename,  t.createdby, t.createddate,  t.lastupdatedby,  t.lastupdateddate,  rrl.rightlevelid from web_role t, web_rolerightlevel rrl where t.f_id = rrl.roleId(+) and t.web='事故报警系统' and t.createdby ='{userName}'   order by t.rolename");
            sql_search.Append(CommonTool.getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
            ORACLEHelper context = new ORACLEHelper(1);
            DataTable dt_data = null;
            List<Role> dataList = new List<Role>();
            try
            {
                dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data != null)
                {
                    Role model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new Role();
                        model.id = item["f_id"].ToString();
                        model.roleName = item["roleName"].ToString();
                        model.level = Convert.ToInt32(item["rightlevelid"]);
                        if (item["lastUpdatedDate"] != DBNull.Value)
                        {
                            model.lastUpdatedDate = Convert.ToDateTime(item["lastUpdatedDate"]);
                        }
                        if (item["createdDate"] != DBNull.Value)
                        {
                            model.createdDate = Convert.ToDateTime(item["createdDate"]);
                        }
                        model.lastUpdatedBy = item["lastUpdatedBy"] != DBNull.Value ? item["lastUpdatedBy"].ToString() : null;
                        model.createdBy = item["createdBy"] != DBNull.Value ? item["createdBy"].ToString() : null;
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

        public int searchCount(string userName)
        {
            int result = 0;
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                result = Convert.ToInt32(context.GetSingle($"select count(1) from web_role t where  t.web='事故报警系统' and t.createdby ='{userName}'"));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public Result<Role> searchModelByCondition(Role condition)
        {
            Result<Role> result = new Result<Role>();
            Role model = null;
            StringBuilder sql_search = new StringBuilder(" select f_id, roleName,rrl.rightlevelid from web_role t,web_rolerightlevel rrl where t.f_id=rrl.roleid and  t.web='事故报警系统' ");
            if (!string.IsNullOrEmpty(condition.id))
            {
                sql_search.Append(" and t.f_id='" + condition.id + "'");
            }
            if (!string.IsNullOrEmpty(condition.roleName))
            {
                sql_search.Append(" and t.roleName='" + condition.roleName + "'");
            }
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                var dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data != null)
                {
                    model = new Role();
                    model.id = dt_data.Rows[0]["f_id"].ToString();
                    model.roleName = dt_data.Rows[0]["roleName"].ToString();
                    model.level = Convert.ToInt32(dt_data.Rows[0]["rightlevelid"]);
                }
                result.data = model;
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<List<UserInfo>> searchUsersByRole(Pager<List<UserInfo>> pager, RoleToUsersQueryModel condition)
        {
            Result<List<UserInfo>> result = new Result<List<UserInfo>>();
            string sql = $"select t.userid, (select u.username from web_users u where u.f_id=t.userid) username,  createdby, createddate from web_userrole t where roleId='{condition.roleId}' ";
            List<UserInfo> dataList = new List<UserInfo>();
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dt_data = context.QueryTable(sql);
                if (dt_data != null)
                {
                    UserInfo model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new UserInfo();
                        model.id = item["userid"].ToString();
                        model.userName = item["username"].ToString();
                        model.createdBy = item["createdby"].ToString();
                        if (item["createdDate"] != DBNull.Value)
                        {
                            model.createdDate = Convert.ToDateTime(item["createdDate"]);
                        }
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

        public int searchUsersCount(RoleToUsersQueryModel condition)
        {
            int result = 0;
            string sql = $"select count(1) from web_userrole t where roleId='{condition.roleId}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                result = Convert.ToInt32(context.GetSingle(sql));
            }
            catch (Exception)
            {
            }
            return result;
        }
        /// <summary>
        /// 修改角色
        /// 1. 判断数据级别是否改变
        /// 2. 改变的话，判断角色下是否有数据权限
        /// 3. 有的话，提示先要删除数据权限
        /// 4. 修改角色名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Result<Role> update(Role t)
        {
            Result<Role> result = new Result<Role>();
            string sql = $"select rrl.rightlevelid from web_rolerightlevel rrl  where rrl.roleid='{t.id}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var dataLevel = Convert.ToInt32(context.GetSingle(sql));
                if (dataLevel!=t.level)
                {
                    sql = $"select count(1) from web_rolerightdetail t where t.roleid='{t.id}'";
                    var dataCount = Convert.ToInt32(context.ExecuteSql(sql));
                    if (dataCount>0)
                    {
                        result.addError("角色已具备数据权限，请先删除数据权限后再修改角色数据权限级别");
                        return result;
                    }
                }
                sql = $"select count(1) from web_role t where  t.roleName='{t.roleName}'";
                var count = Convert.ToInt32(context.GetSingle(sql));
                if (count > 0)
                {
                    result.addError($"角色名{t.roleName}已经被用过了");
                    return result;
                }
                sql = $"update web_role t set t.roleName='{t.roleName}',t.lastUpdatedBy='{t.lastUpdatedBy}',t.lastUpdateddate=to_date('{t.lastUpdatedDate}','yyyy/MM/dd HH24:mi:ss') where t.f_id='{t.id}'";
                count = context.ExecuteSql(sql);
                sql = $"update web_rolerightlevel t set t.rightlevelid ={t.level} where t.roleid='{t.id}'";
                count = context.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
    }
}