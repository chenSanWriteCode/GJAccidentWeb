using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class UserDao : IUserDao
    {
        public Result<UserInfo> add(UserInfo t)
        {
            Result<UserInfo> result = new Result<UserInfo>();
            string sql = $"insert into web_users (f_id, username, phonenumber, email, password, createdby, createddate,web,companyId,userNo) values(seq_web_user.nextval,'{t.userName}','{t.phoneNum}','{t.email}','{t.password}','{t.createdBy}',to_date('{t.createdDate}','yyyy/MM/dd HH24:mi:ss'),'事故报警系统',{t.companyId},'{t.userNo}')";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                int count = Convert.ToInt32(context.GetSingle($"select count(1) from web_users t where t.username='{t.userName}'"));
                if (count>0)
                {
                    result.addError($"用户名{t.userName}已存在");
                    return result;
                }
                count = context.ExecuteSql(sql);
                //if (count==0)
                //{
                //    result.addError("增减用户失败，请联系管理员");
                //}
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<UserInfo> changePassword(UserInfo t, string newPassword)
        {
            Result<UserInfo> result = new Result<UserInfo>();
            string sql = $"select count(1) from web_users t where t.f_id='{t.id}' and t.password='{t.password}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = Convert.ToInt32(context.GetSingle(sql));
                if (count==0)
                {
                    result.addError("原密码不正确");
                    return result;
                }
                sql = $"update web_users t set t.password='{newPassword}',t.lastUpdatedBy='{t.lastUpdatedBy}',t.lastUpdateddate=to_date('{t.lastUpdatedDate}','yyyy/MM/dd HH24:mi:ss') where t.f_id='{t.id}'";
                count = context.ExecuteSql(sql);
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
            string sql = $"delete from web_users t where t.f_id='{id}'";
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

        public Result<List<UserInfo>> search(Pager<List<UserInfo>> pager, string userName)
        {
            Result<List<UserInfo>> result = new Result<List<UserInfo>>();
            StringBuilder sql_search = new StringBuilder(CommonTool.SQL_HEAD);
            sql_search.Append($" select f_id, username, createdby, createddate,  lastupdatedby,  lastupdateddate from web_users t   where t.web='事故报警系统'  and (t.createdby ='{userName}' or t.username ='{userName}') order by username");
            sql_search.Append(CommonTool.getSQL_TAIL((pager.page - 1) * pager.recPerPage, pager.page * pager.recPerPage));
            ORACLEHelper context = new ORACLEHelper(1);
            DataTable dt_data = null;
            List<UserInfo> dataList = new List<UserInfo>();
            try
            {
                dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data!=null)
                {
                    UserInfo model = null;
                    foreach (DataRow item in dt_data.Rows)
                    {
                        model = new UserInfo();
                        model.id = item["f_id"].ToString();
                        model.userName = item["userName"].ToString();
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

        public Result<UserInfo> searchModelByCondition(UserInfo condition)
        {
            Result<UserInfo> result = new Result<UserInfo>();
            UserInfo model = null;
            StringBuilder sql_search = new StringBuilder(" select f_id, username, phonenumber, email from web_users t where t.web='事故报警系统'  ");
            if (!string.IsNullOrEmpty(condition.id))
            {
                sql_search.Append(" and t.f_id='"+ condition.id + "'");
            }
            if (!string.IsNullOrEmpty(condition.userName))
            {
                sql_search.Append(" and t.username='" + condition.userName + "'");
            }
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                var dt_data = context.QueryTable(sql_search.ToString());
                if (dt_data!=null)
                {
                    model = new UserInfo();
                    model.id = dt_data.Rows[0]["f_id"].ToString();
                    model.userName=dt_data.Rows[0]["userName"].ToString();
                    model.phoneNum = dt_data.Rows[0]["phonenumber"]!=DBNull.Value? dt_data.Rows[0]["phonenumber"].ToString():null;
                    model.email = dt_data.Rows[0]["email"] != DBNull.Value ? dt_data.Rows[0]["email"].ToString() : null;
                }
                result.data = model;
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public int searchCount(string userName)
        {
            int result =0;
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                result = Convert.ToInt32(context.GetSingle($"select count(1) from web_users t where t.web='事故报警系统'  and (t.createdby ='{userName}' or t.username ='{userName}')"));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public Result<UserInfo> update(UserInfo t)
        {
            Result<UserInfo> result = new Result<UserInfo>();
            string sql = $"select count(1) from web_users t where t.f_id<>'{t.id}' and t.username='{t.userName}'";
            try
            {
                ORACLEHelper context = new ORACLEHelper(1);
                var count = Convert.ToInt32(context.GetSingle(sql));
                if (count>0)
                {
                    result.addError($"用户名{t.userName}已经被用过了");
                    return result;
                }
                sql = $"update web_users t set t.username='{t.userName}' , t.phonenumber='{t.phoneNum}' , t.email='{t.email}',t.lastUpdatedBy='{t.lastUpdatedBy}',t.lastUpdateddate=to_date('{t.lastUpdatedDate}','yyyy/MM/dd HH24:mi:ss'),t.companyId={t.companyId},t.userNo='{t.userNo}' where t.f_id='{t.id}'";
                count = context.ExecuteSql(sql);
                //if (count==0)
                //{
                //    result.addError("修改失败，请联系管理员");
                //}
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }

        public Result<int> passwordSignIn(UserInfo user)
        {
            Result<int> result = new Result<int>();
            ORACLEHelper context = new ORACLEHelper(1);
            try
            {
                result.data = Convert.ToInt32(context.GetSingle($"select count(1) from web_users where username='{user.userName}' and password='{user.password}'"));
                if (result.data==0)
                {
                    result.addError("用户名或密码不正确");
                }
            }
            catch (Exception err)
            {
                result.addError(err.Message);
            }
            return result;
        }
    }
}