using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class UserInfo
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string phoneNum { get; set; }
        public string email { get; set; }
        /// <summary>
        /// 所属分公司
        /// </summary>
        public string companyId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string userNo { get; set; }
        public string password { get; set; }
        public int activityFlag { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastUpdatedBy { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
    }
}