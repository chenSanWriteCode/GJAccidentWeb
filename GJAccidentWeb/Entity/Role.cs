using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class Role
    {
        public string id { get; set; }
        public string roleName { get; set; }
        /// <summary>
        /// 1 分公司
        /// 2 线路
        /// 3 车辆
        /// 默认线路
        /// </summary>
        public int level { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastUpdatedBy { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
    }
    public class RoleToUser
    {
        public string roleId { get; set; }
        public string userId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
    }
}