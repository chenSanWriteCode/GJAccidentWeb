using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models.QueryModels
{
    public class RoleMenuQueryModel
    {
        public int id { get; set; }
        public string roleId { get; set; }
        public int menuLevel { get; set; }
        public int menuId { get; set; }
        public int parentMenuId { get; set; }
    }
}