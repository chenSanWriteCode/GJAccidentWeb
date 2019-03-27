using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models
{
    public class UserCreateModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        [Phone(ErrorMessage = "不是有效的电话号码")]
        public string phoneNum { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "密码为6-15个字符")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "密码和确认密码不匹配。")]
        public string confirmPassword { get; set; }
        /// <summary>
        /// 所属分公司
        /// </summary>
        [Required(ErrorMessage = "所属分公司为必填项")]
        public string companyId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        [Required(ErrorMessage = "工号为必填项")]
        public string userNo { get; set; }
    }
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "出现不可预期异常，请重新操作")]
        public string id { get; set; }
        public string userName { get; set; }
        [Phone(ErrorMessage = "不是有效的电话号码")]
        public string phoneNum { get; set; }
        [EmailAddress]
        public string email { get; set; }
        /// <summary>
        /// 所属分公司
        /// </summary>
        [Required(ErrorMessage = "所属分公司为必填项")]
        public string companyId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        [Required(ErrorMessage = "工号为必填项")]
        public string userNo { get; set; }
    }
    public class UserChangePasswordModel
    {
        [Required(ErrorMessage ="出现不可预期异常，请重新操作")]
        public string id { get; set; }
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "密码为6-15个字符")]
        public string oldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "密码为6-15个字符")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "密码和确认密码不匹配。")]
        public string confirmPassword { get; set; }

        
    }
}