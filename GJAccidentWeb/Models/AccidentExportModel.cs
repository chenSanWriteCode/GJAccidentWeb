using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GJAccidentWeb.Infrastructure;

namespace GJAccidentWeb.Models
{
    public class AccidentExportModel
    {
        /// <summary>
        /// 事故发生时间
        /// </summary>
        [ExcelColumn(Name = "事故发生时间")]
        [Required]
        public DateTime accidentTime { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [ExcelColumn(Name = "部门")]
        public string dwName { get; set; }
        [ExcelColumn(Name = "线路")]
        public string lineName { get; set; }
        [ExcelColumn(Name = "车牌号")]
        public string carNum { get; set; }
        [ExcelColumn(Name = "地点")]
        /// <summary>
        /// 地点
        /// </summary>
        public string area { get; set; }
        [ExcelColumn(Name = "天气")]
        /// <summary>
        /// 天气
        /// </summary>
        public string weather { get; set; }
        [ExcelColumn(Name = "路况地形")]
        /// <summary>
        /// 路况地形
        /// </summary>
        public string roadCondition { get; set; }
        [ExcelColumn(Name = "车型")]
        /// <summary>
        /// 车型
        /// </summary>
        public string carType { get; set; }
        [ExcelColumn(Name = "时速")]
        public float speed { get; set; }
        [ExcelColumn(Name = "线路走向")]
        /// <summary>
        /// 线路走向
        /// </summary>
        public string lineDir { get; set; }
        [ExcelColumn(Name = "事故责任")]
        /// <summary>
        /// 事故责任
        /// </summary>
        public string accidentDuty { get; set; }
        [ExcelColumn(Name = "事故经过")]
        /// <summary>
        /// 事故经过
        /// </summary>
        public string accidentPass { get; set; }
        [ExcelColumn(Name = "原因分析")]
        /// <summary>
        /// 原因分析
        /// </summary>
        public string accidentReason { get; set; }
        [ExcelColumn(Name = "损失情况")]
        /// <summary>
        /// 损失情况
        /// </summary>
        public string accidentLost { get; set; }
        [ExcelColumn(Name = "是否私了")]
        /// <summary>
        /// 是否私了
        /// </summary>
        public string isUnder { get; set; }
        [ExcelColumn(Name = "驾驶员")]
        public string driverName { get; set; }
        [ExcelColumn(Name = "性别")]
        public string driverSex { get; set; }
        [ExcelColumn(Name = "年龄")]
        /// <summary>
        /// 年龄
        /// </summary>
        public decimal driverAge { get; set; }
        [ExcelColumn(Name = "公交驾龄")]
        /// <summary>
        /// 公交驾龄
        /// </summary>
        public string drivingYears { get; set; }
        [ExcelColumn(Name = "当日工作时长")]
        /// <summary>
        /// 当日工作时长
        /// </summary>
        public string workingHours { get; set; }
        [ExcelColumn(Name = "连续工作时间")]
        /// <summary>
        /// 连续工作时间
        /// </summary>
        public string continueHours { get; set; }
        [ExcelColumn(Name = "饭后时长")]
        /// <summary>
        /// 饭后时长
        /// </summary>
        public string eatedHours { get; set; }
        [ExcelColumn(Name = "体检情况")]
        /// <summary>
        /// 体检情况
        /// </summary>
        public string healthyCondition { get; set; }
        [ExcelColumn(Name = "婚姻情况")]
        /// <summary>
        /// 婚姻情况
        /// </summary>
        public string marriageCondition { get; set; }
        [ExcelColumn(Name = "家庭住址")]
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string driverAddress { get; set; }
        
    }
}