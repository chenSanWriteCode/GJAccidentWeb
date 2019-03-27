using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models
{
    public class AccidentQueryModel
    {
        public int id { get; set; }
        /// <summary>
        /// 事故发生时间
        /// </summary>
        public DateTime accidentTime { get; set; }
        public DateTime accidentTimeStart { get; set; } = DateTime.Now.Date;
        public DateTime accidentTimeEnd { get; set; } = DateTime.Now.AddDays(1);
        public string lineId { get; set; }
        public string lineName { get; set; }
        public string carNum { get; set; }
        /// <summary>
        /// 是否有用
        /// </summary>
        public int en { get; set; } = 1;
        /// <summary>
        /// 地点
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string weather { get; set; }
        /// <summary>
        /// 路况地形
        /// </summary>
        public string roadCondition { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        public string carType { get; set; }
        public float? speed { get; set; }
        public float? speedMin { get; set; }
        public float? speedMax { get; set; }
        /// <summary>
        /// 线路走向
        /// </summary>
        public string lineDir { get; set; }
        /// <summary>
        /// 事故责任
        /// </summary>
        public string accidentDuty { get; set; }
        /// <summary>
        /// 事故经过
        /// </summary>
        public string accidentPass { get; set; }
        /// <summary>
        /// 原因分析
        /// </summary>
        public string accidentReason { get; set; }
        /// <summary>
        /// 损失情况
        /// </summary>
        public string accidentLost { get; set; }
        //public float? AccidentLostMin { get; set; }

        /// <summary>
        /// 是否私了
        /// </summary>
        public string isUnder { get; set; } = "否";
        public string driverName { get; set; }
        public string driverSex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? driverAge { get; set; }
        public int? driverAgeMin { get; set; }
        public int? driverAgeMax { get; set; }
        /// <summary>
        /// 公交驾龄
        /// </summary>
        public string drivingYears { get; set; }
        public float? drivingYearsMin { get; set; }
        public float? drivingYearsMax { get; set; }
        /// <summary>
        /// 当日工作时长
        /// </summary>
        public float? workingHours { get; set; }
        public float? workingHoursMin { get; set; }
        public float? workingHoursMax { get; set; }

        /// <summary>
        /// 连续工作时间
        /// </summary>
        public float? continueHours { get; set; }
        public float? continueHoursMin { get; set; }
        public float? continueHoursMax { get; set; }
        /// <summary>
        /// 饭后时长
        /// </summary>
        public float? eatedHours { get; set; }
        public float? eatedHoursMin { get; set; }
        public float? eatedHoursMax { get; set; }
        /// <summary>
        /// 体检情况
        /// </summary>
        public string healthyCondition { get; set; }
        /// <summary>
        /// 婚姻情况
        /// </summary>
        public string marriageCondition { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string driverAddress { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime optTime { get; set; }
        public int optId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string dwId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string dwName { get; set; }
    }
}