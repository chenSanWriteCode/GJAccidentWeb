using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models;
using log4net;

namespace GJAccidentWeb.Dao.LayerDao
{
    public class AccidentDao : IAccidentDao
    {
        private ILog log = LogManager.GetLogger("AccidentDao");
        public Result<AccidentModel> add(AccidentModel model)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            Result<AccidentModel> result = new Result<AccidentModel>();
            string sql = "";
            try
            {
                sql = $"insert into gj_info_shigutongji (f_id, 时间, 线路, 车号, en, 地点, 天气, 路况地形, 车型, 时速, 线路走向, 事故责任, 事故经过, 原因分析, 损失情况, 是否私了, 姓名, 性别, 年龄, 公交驾龄, 当日工作时长, 连续工作时间, 饭后时长, 体检情况, 婚姻情况, 家庭住址, optime, opid, dwid, dwname) values(seq_gj_info_shigutongji.nextval,to_date('{model.accidentTime.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss'),'{model.lineName}','{model.carNum}','{model.en}','{model.area}','{model.weather}','{model.roadCondition}','{model.carType}',{model.speed},'{model.lineDir}','{model.accidentDuty}','{model.accidentPass}','{model.accidentReason}','{model.accidentLost}','{model.isUnder}','{model.driverName}','{model.driverSex}','{model.driverAge}','{model.drivingYears}','{model.workingHours}','{model.continueHours}','{model.eatedHours}','{model.healthyCondition}','{model.marriageCondition}','{model.driverAddress}',to_date('{model.optTime.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss'),{model.optId},{model.dwId},'{model.dwName}')";
                var count = context.ExecuteSql(sql);

            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
                result.addError(err.Message);
            }
            result.data = model;
            return result;
        }

        public Result<Accident> delete(string id)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            Result<Accident> result = new Result<Accident>();
            string sql = "";
            try
            {
                sql = $"update gj_info_shigutongji set en=0 where f_id={id}";
                var count = context.ExecuteSql(sql);

            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
                result.addError(err.Message);
            }
            return result;
        }

        public Result<Accident> findById(string id)
        {
            Result<Accident> result = new Result<Accident>();
            ORACLEHelper context = new ORACLEHelper(1);
            string sql = $"select f_id, 时间, 线路, 车号, en, 地点, 天气, 路况地形, 车型, 时速, 线路走向, 事故责任, 事故经过, 原因分析, 损失情况, 是否私了, 姓名, 性别, 年龄, 公交驾龄, 当日工作时长, 连续工作时间, 饭后时长, 体检情况, 婚姻情况, 家庭住址, optime, opid, dwid, dwname from gj_info_shigutongji t where f_id={id} ";
            try
            {
                var data = context.QueryTable(sql);
                var dataList = datatableToList(data);
                if (dataList.Count==1)
                {
                    result.data = dataList.First();
                }
                else
                {
                    result.addError("记录已被删除");
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
                result.addError(err.Message);
            }
            return result;
        }

        public List<Accident> getListByCondition(AccidentQueryModel condition)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            List<Accident> dataList= new List<Accident>();
            string sql = $"select  f_id, 时间, 线路, 车号, en, 地点, 天气, 路况地形, 车型, 时速, 线路走向, 事故责任, 事故经过, 原因分析, 损失情况, 是否私了, 姓名, 性别, 年龄, 公交驾龄, 当日工作时长, 连续工作时间, 饭后时长, 体检情况, 婚姻情况, 家庭住址, optime, opid, dwid, dwname from gj_info_shigutongji t where en=1 and t.时间>=to_date('{condition.accidentTimeStart.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss') and t.时间<=to_date('{condition.accidentTimeEnd.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss')";
            sql += combineCondition(condition);
            sql += $"  order by 时间";
            try
            {
                var data = context.QueryTable(sql);
                dataList = datatableToList(data);
            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
            }
            return dataList;
        }

        public Result<List<Accident>> search(Pager<List<Accident>> pager,AccidentQueryModel condition)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            Result<List<Accident>> result = new Result<List<Accident>>();
            List<Accident> dataList;
            int start = (pager.page - 1) * pager.recPerPage;
            int end = pager.page * pager.recPerPage;
            string sql = $"select * from (select rownum num, f_id, 时间, 线路, 车号, en, 地点, 天气, 路况地形, 车型, 时速, 线路走向, 事故责任, 事故经过, 原因分析, 损失情况, 是否私了, 姓名, 性别, 年龄, 公交驾龄, 当日工作时长, 连续工作时间, 饭后时长, 体检情况, 婚姻情况, 家庭住址, optime, opid, dwid, dwname from gj_info_shigutongji t where en=1 and t.时间>=to_date('{condition.accidentTimeStart.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss') and t.时间<=to_date('{condition.accidentTimeEnd.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss')";
            sql += combineCondition(condition);
            sql += $" and rownum<={end} order by 时间) where num>{start}";
            try
            {
                var data = context.QueryTable(sql);
                dataList = datatableToList(data);
                result.data = dataList;
            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
                result.addError(err.Message);
            }
            return result;
        }

       

        public int searchCount(AccidentQueryModel condition)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            int result = 0;
            string sql = $"select count(1) from gj_info_shigutongji t where t.时间>=to_date('{condition.accidentTimeStart}','yyyy-MM-dd HH24:mi:ss') and t.时间<=to_date('{condition.accidentTimeEnd}','yyyy-MM-dd HH24:mi:ss')";
            sql += combineCondition(condition);
            try
            {
                result = Convert.ToInt32(context.GetSingle(sql));
            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
            }
            return result;
        }

        public Result<AccidentModel> update(AccidentModel model)
        {
            ORACLEHelper context = new ORACLEHelper(1);
            Result<AccidentModel> result = new Result<AccidentModel>();
            string sql = "";
            try
            {
                sql = $"update gj_info_shigutongji set 时间=to_date('{model.accidentTime.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss'), 线路='{model.lineName}', 车号='{model.carNum}', en='{model.en}', 地点='{model.area}', 天气='{model.weather}', 路况地形='{model.roadCondition}', 车型='{model.carType}', 时速={model.speed}, 线路走向='{model.lineDir}', 事故责任='{model.accidentDuty}', 事故经过='{model.accidentPass}', 原因分析='{model.accidentReason}', 损失情况='{model.accidentLost}', 是否私了='{model.isUnder}', 姓名='{model.driverName}', 性别='{model.driverSex}', 年龄='{model.driverAge}', 公交驾龄='{model.drivingYears}', 当日工作时长='{model.workingHours}', 连续工作时间='{model.continueHours}', 饭后时长='{model.eatedHours}', 体检情况='{model.healthyCondition}', 婚姻情况='{model.marriageCondition}', 家庭住址='{model.driverAddress}', optime=to_date('{model.optTime.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-MM-dd HH24:mi:ss'), opid={model.optId}, dwid={model.dwId}, dwname='{model.dwName}' where f_id={model.id}";
                var count = context.ExecuteSql(sql);

            }
            catch (Exception err)
            {
                log.Error(err.Message + " :" + sql);
                result.addError(err.Message);
            }
            result.data = model;
            return result;
        }
        private string combineCondition(AccidentQueryModel condition)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(condition.carNum))
            {
                sql += $" and t.车号 like '%{condition.carNum}%' ";
            }
            else if (!string.IsNullOrEmpty(condition.lineName))
            {
                sql += $" and t.线路 in({condition.lineName}) ";
            }
            else if (string.IsNullOrEmpty(condition.dwId))
            {
                sql += $" and t.dwid in({condition.dwId})";
            }
            if (!string.IsNullOrEmpty(condition.area))
            {
                sql += $" and t.地点 like '%{condition.area}%'";
            }
            if (!string.IsNullOrEmpty(condition.weather))
            {
                sql += $" and t.天气='{condition.weather}'";
            }
            if (!string.IsNullOrEmpty(condition.roadCondition))
            {
                sql += $" and t.路况地形 like'%{condition.roadCondition}%'";
            }
            if (!string.IsNullOrEmpty(condition.carType))
            {
                sql += $" and t.车型='{condition.carType}'";
            }
            if (!string.IsNullOrEmpty(condition.lineDir))
            {
                sql += $" and t.线路走向 like '%{condition.lineDir}%'";
            }
            if (!string.IsNullOrEmpty(condition.accidentDuty))
            {
                sql += $" and t.事故责任='{condition.accidentDuty}'";
            }
            if (!string.IsNullOrEmpty(condition.isUnder))
            {
                sql += $" and t.是否私了='{condition.isUnder}'";
            }
            
            if (condition.speedMin > 0)
            {
                sql += $" and t.时速>={condition.speedMin}";
            }
            if (condition.speedMax > 0)
            {
                sql += $" and t.时速<={condition.speedMax}";
            }
            if (!string.IsNullOrEmpty(condition.accidentPass))
            {
                sql += $"  and t.事故经过 like '%{condition.accidentPass}%'";
            }
            if (!string.IsNullOrEmpty(condition.accidentReason))
            {
                sql += $"  and t.原因分析 like '%{condition.accidentReason}%'";
            }
            if (!string.IsNullOrEmpty(condition.driverName))
            {
                sql += $" and t.姓名='{condition.driverName}'";
            }
            if (!string.IsNullOrEmpty(condition.driverSex))
            {
                sql += $" and t.性别='{condition.driverSex}'";
            }
            if (condition.driverAgeMin > 0)
            {
                sql += $" and t.年龄>={condition.driverAgeMin}";
            }
            if (condition.driverAgeMax > 0)
            {
                sql += $" and t.年龄<={condition.driverAgeMax}";
            }
            if (condition.drivingYearsMin > 0)
            {
                sql += $" and t.公交驾龄>={condition.drivingYearsMin}";
            }
            if (condition.drivingYearsMax > 0)
            {
                sql += $" and t.公交驾龄<={condition.drivingYearsMax}";
            }

            if (condition.workingHoursMin > 0)
            {
                sql += $" and t.当日工作时长>={condition.workingHoursMin}";
            }
            if (condition.workingHoursMax > 0)
            {
                sql += $" and t.当日工作时长<={condition.workingHoursMax}";
            }
            if (condition.continueHoursMin > 0)
            {
                sql += $" and t.连续工作时间>={condition.continueHoursMin}";
            }
            if (condition.continueHoursMax > 0)
            {
                sql += $" and t.连续工作时间<={condition.continueHoursMax}";
            }
            if (condition.eatedHoursMin > 0)
            {
                sql += $" and t.饭后时长>={condition.eatedHoursMin}";
            }
            if (condition.eatedHoursMax > 0)
            {
                sql += $" and t.饭后时长<={condition.eatedHoursMax}";
            }
            if (!string.IsNullOrEmpty(condition.healthyCondition))
            {
                sql += $" and t.体检情况='{condition.healthyCondition}'";
            }
            if (!string.IsNullOrEmpty(condition.marriageCondition))
            {
                sql += $" and t.婚姻情况='{condition.marriageCondition}'";
            }
            if (!string.IsNullOrEmpty(condition.driverAddress))
            {
                sql += $" and t.家庭住址='{condition.driverAddress}'";
            }
            return sql;
        }
        private List<Accident> datatableToList(DataTable data)
        {
            List<Accident> result = new List<Accident>();
            if (data != null)
            {
                Accident model;
                foreach (DataRow item in data.Rows)
                {
                    model = new Accident();
                    model.id = Convert.ToDecimal(item["f_id"]);
                    model.accidentDuty = item["事故责任"]?.ToString();
                    model.accidentTime = Convert.ToDateTime(item["时间"]);
                    model.lineName = item["线路"]?.ToString();
                    model.carNum = item["车号"]?.ToString();
                    model.en = Convert.ToDecimal(item["en"]);
                    model.area = item["地点"]?.ToString();
                    model.weather = item["天气"]?.ToString();
                    model.roadCondition = item["路况地形"]?.ToString();
                    model.carType = item["车型"]?.ToString();
                    model.speed = float.Parse(item["时速"]?.ToString());
                    model.lineDir = item["线路走向"]?.ToString();
                    model.accidentPass = item["事故经过"]?.ToString();
                    model.accidentReason = item["原因分析"]?.ToString();
                    model.accidentLost = item["损失情况"]?.ToString();
                    model.isUnder = item["是否私了"]?.ToString();
                    model.driverName = item["姓名"]?.ToString();
                    model.driverSex = item["性别"]?.ToString();
                    if (item["年龄"]!=DBNull.Value)
                    {
                        model.driverAge = Convert.ToDecimal(item["年龄"]);
                    }
                    model.drivingYears = item["公交驾龄"]?.ToString();
                    model.workingHours = item["当日工作时长"]?.ToString();
                    model.continueHours = item["连续工作时间"]?.ToString();
                    model.eatedHours = item["饭后时长"]?.ToString();
                    model.healthyCondition = item["体检情况"]?.ToString();
                    model.marriageCondition = item["婚姻情况"]?.ToString();
                    model.driverAddress = item["家庭住址"]?.ToString();
                    model.optTime = Convert.ToDateTime(item["optime"]);
                    model.optId = Convert.ToDecimal(item["opid"]);
                    model.dwId = Convert.ToDecimal(item["dwid"]);
                    model.dwName = item["dwname"]?.ToString();
                    result.Add(model);
                }
            }

            return result;
        }
    }
}