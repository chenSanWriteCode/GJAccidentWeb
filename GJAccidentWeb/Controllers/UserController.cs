using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;
using GJAccidentWeb.Services;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class UserController : Controller
    {
        [Dependency]
        public IUserService service { get; set; }
        // GET: User
        public async Task<ActionResult> Index(Pager<List<UserInfo>> pager)
        {
            var result = await service.search(pager,User.Identity.Name);
            if (result.success)
            {
                return View(pager);
            }
            else
            {
                ViewBag.ReturnUrl = "/User/Index";
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }
        [AllowAnonymous]
        public ActionResult Add()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Add(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var result = await service.add(model, userName);
                ViewBag.ReturnUrl = "/Accident/Index";
                if (result.success)
                {
                    return View("Success");
                }
                else
                {
                    ViewBag.Msg = result.message;
                    return View("Error");
                }
            }
            return View();
        }
        public async Task<ActionResult> Update(string id)
        {
            var url = Request.Url;
            Result<UserInfo> result = new Result<UserInfo>();
            if (id!=null)
            {
                UserInfo condition = new UserInfo
                {
                    id = id
                };
                result = await service.searchModelByCondition(condition);
                if (result.success)
                {
                    return View(result.data);
                }
            }
            else
            {
                result.addError("出现了不可预期的异常，请重新操作");
            }
            ViewBag.ReturnUrl = "/User/Update?id="+id;
            ViewBag.Msg = result.message;
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Update(UserUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var result = await service.update(model, userName);
                if (result.success)
                {
                    ViewBag.ReturnUrl = "/User/Update?id=" + model.id;
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                }
            }
            var userModel = await service.searchModelByCondition(new UserInfo { id = model.id });
            return View(userModel.data);
        }
        public ActionResult changePassword(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> changePassword(UserChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var result =await service.changePassword(model, userName);
                if (result.success)
                {
                    ViewBag.ReturnUrl = "/Account/LogOff";
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                }
            }
            ViewBag.Id = model.id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await service.delete(id);
            ViewBag.ReturnUrl = "/Account/Login";
            if (result.success)
            {
                return View("Success");
            }
            else
            {
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }

    }
}