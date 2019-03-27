using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models.QueryModels
{
    public class RoleRightQueryModel
    {
        public int id { get; set; }
        public string roleId { get; set; }
        public int rightdata { get; set; }
        /// <summary>
        /// 1 分公司
        /// 2 线路
        /// 3 车辆
        /// </summary>
        public int level { get; set; }
        public string lineId { get; set; }
        public string companyId { get; set; }
    }
}