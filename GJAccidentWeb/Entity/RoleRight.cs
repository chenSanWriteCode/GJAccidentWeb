using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class RoleRight
    {
        public int id { get; set; }
        public string roleId { get; set; }
        public int rightId { get; set; }
        public string rightName { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
    }
}