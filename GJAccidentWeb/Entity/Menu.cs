using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class Menu
    {
        public int id { get; set; }
        /// <summary>
        /// 排列顺序
        /// </summary>
        public int orderId { get; set; }
        public int parentMenuId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        public string url { get; set; }
        public string open { get; set; }
        public string htmlStr { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastUpdatedBy { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<Menu> children { get; set; }
        /// <summary>
        /// 是否在角色内
        /// 0 未包含
        /// 》0 包含
        /// </summary>
        public int isIn { get; set; }

    }
}