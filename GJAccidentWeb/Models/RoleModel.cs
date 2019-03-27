using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models
{
    public class RoleCreateModel
    {
        [Required]
        public string roleName { get; set; }
        /// <summary>
        /// 1 分公司
        /// 2 线路
        /// 3 车辆
        /// 默认线路
        /// </summary>
        public int level { get; set; } = 2;
    }
    public class RoleUpdateModel
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string roleName { get; set; }
        /// <summary>
        /// 1 分公司
        /// 2 线路
        /// 3 车辆
        /// 默认线路
        /// </summary>
        public int level { get; set; }
    }
}