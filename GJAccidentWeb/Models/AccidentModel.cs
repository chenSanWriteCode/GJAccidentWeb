using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models
{
    public class AccidentModel
    {
        public decimal id { get; set; }
        [Required(ErrorMessage = "事故时间不可为空！")]
        /// <summary>
        /// 事故发生时间
        /// </summary>
        public DateTime accidentTime { get; set; } = DateTime.Now.Date;
        [Required(ErrorMessage = "线路不可为空！")]
        public string lineId { get; set; }
        public string lineName { get; set; }
        [Required(ErrorMessage = "车牌号不可为空！")]
        public string carNum { get; set; }
        /// <summary>
        /// 是否有用
        /// </summary>
        public decimal en { get; set; } = 1;
        [Required(ErrorMessage = "地点不可为空！")]
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
        [Required(ErrorMessage = "车型不可为空！")]
        /// <summary>
        /// 车型
        /// </summary>
        public string carType { get; set; }
        [Required(ErrorMessage = "时速不可为空！")]
        public float speed { get; set; }
        [Required(ErrorMessage = "线路走向不可为空！")]
        /// <summary>
        /// 线路走向
        /// </summary>
        public string lineDir { get; set; }
        [Required(ErrorMessage = "事故责任不可为空！")]
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
        /// <summary>
        /// 是否私了
        /// </summary>
        public string isUnder { get; set; } = "否";
        [Required(ErrorMessage = "驾驶员姓名不可为空！")]
        public string driverName { get; set; }
        public string driverSex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public decimal? driverAge { get; set; }
        /// <summary>
        /// 公交驾龄
        /// </summary>
        public string drivingYears { get; set; }
        /// <summary>
        /// 当日工作时长
        /// </summary>
        public string workingHours { get; set; }
        /// <summary>
        /// 连续工作时间
        /// </summary>
        public string continueHours { get; set; }
        /// <summary>
        /// 饭后时长
        /// </summary>
        public string eatedHours { get; set; }
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
        public DateTime optTime { get; set; } = DateTime.Now;
        public string optId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public decimal dwId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string dwName { get; set; }
    }
}