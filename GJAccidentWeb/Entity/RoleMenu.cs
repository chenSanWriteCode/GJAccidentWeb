using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class RoleMenu
    {
        public int id { get; set; }
        public string roleId { get; set; }
        public int menuLevel { get; set; }
        public string menuLevelName { get; set; }
        public int menuId { get; set; }
        public string menuTitle { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }

    }
}