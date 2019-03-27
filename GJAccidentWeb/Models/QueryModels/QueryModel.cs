using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models.QueryModels
{
    public class QueryModel
    {
        /// <summary>
        /// 分公司id 用于统一权限控制
        /// </summary>
        public string companyId { get; set; }
        /// <summary>
        /// 线路 用于统一权限控制
        /// </summary>
        public string lineId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime timeBegin { get; set; } = DateTime.Now;
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime timeEnd { get; set; } = DateTime.Now;
    }
}