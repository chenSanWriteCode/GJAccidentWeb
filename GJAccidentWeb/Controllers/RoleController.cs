using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;
using GJAccidentWeb.Models.QueryModels;
using GJAccidentWeb.Services;
using Newtonsoft.Json;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class RoleController : Controller
    {
        [Dependency]
        public IRoleService service { get; set; }
        // GET: Role
        public async Task<ActionResult> Index(Pager<List<Role>> pager)
        {
            var result = await service.search(pager,User.Identity.Name);
            if (result.success)
            {
                return View(pager);
            }
            else
            {
                ViewBag.ReturnUrl = "/Role/Index";
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var result = await service.add(model, userName);
                ViewBag.ReturnUrl = "/Role/Index";
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
            Result<Role> result = new Result<Role>();
            if (id != null)
            {
                Role condition = new Role
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
            ViewBag.ReturnUrl = "/Role/Index";
            ViewBag.Msg = result.message;
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Update(RoleUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var result = await service.update(model, userName);
                if (result.success)
                {
                    ViewBag.ReturnUrl = "/Role/Index";
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("", result.message);
                }
            }
            var userModel = await service.searchModelByCondition(new Role { id = model.id });
            return View(userModel.data);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await service.delete(id);
            ViewBag.ReturnUrl = "/Role/Index";
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
        public async Task<ActionResult> ManageUsers(Pager<List<UserInfo>> pager, string roleId, string roleName)
        {
            if (roleId != null)
            {
                var result = await service.searchUsersByRole(pager, new RoleToUsersQueryModel { roleId = roleId });
                if (result.success)
                {
                    ViewBag.RoleId = roleId;
                    ViewBag.RoleName = roleName;
                    return View(pager);
                }
                else
                {
                    ViewBag.Msg = result.message;
                }
            }
            else
            {
                ViewBag.Msg = "出现不可预期的异常，请重新操作";
            }
            ViewBag.ReturnUrl = "/Role/Index";
            return View("Error");
        }
        public async Task<ActionResult> DeleteUser(string userId, string roleId, string roleName)
        {
            var result = await service.DeleteUser(new RoleToUsersQueryModel { userId = userId });
            ViewBag.ReturnUrl = "/Role/ManageUsers?roleId=" + roleId + "&roleName=" + roleName;
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
        public ActionResult AddUser([Required] string roleId)
        {
            ViewBag.RoleId = roleId;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(RoleToUser model)
        {
            string userName = User.Identity.Name;
            var result = await service.AddUser(model, userName);
            ViewBag.ReturnUrl = "/Role/Index";
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