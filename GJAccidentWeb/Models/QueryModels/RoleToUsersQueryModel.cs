using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models.QueryModels
{
    public class RoleToUsersQueryModel
    {
        public string roleId { get; set; }
        public string userId { get; set; }

        public string createdBy { get; set; }
    }
}