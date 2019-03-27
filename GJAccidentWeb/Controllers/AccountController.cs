using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Services;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class AccountController : Controller
    {
        [Dependency]
        public IUserService service { get; set; }
        //[Dependency]
        //public ICommonToolService commonService { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl=null)
        {
            ViewBag.ReturnUrl = "/Accident/Index";
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string userName, string password, [Required]string returnUrl)
        {
            var result = await service.passwordSignIn(new UserInfo { userName = userName, password = password });
            if (result.success)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", result.message);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}