using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models.QueryModels;
using GJAccidentWeb.Services;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class RoleMenuController : Controller
    {
        [Dependency]
        public IRoleMenuService service { get; set; }
        // GET: RoleMenu
        public ActionResult Index(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition)
        {
            ViewBag.Condition = condition;
            return View(pager);
        }
        public async Task<ActionResult> Search(Pager<List<RoleMenu>> pager, RoleMenuQueryModel condition)
        {
            var result = await service.search(pager, condition);
            if (result.success)
            {
                ViewBag.Condition = condition;
                return View("Index", pager);
            }
            else
            {
                ViewBag.ReturnUrl = "/RoleMenu/Index";
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete([Required]int id,string roleId)
        {
            ViewBag.ReturnUrl = "/RoleMenu/Search?roleId="+roleId;
            if (ModelState.IsValid)
            {
                var result = await service.delete(new RoleMenuQueryModel { id=id });
                if (result.success)
                {
                    return View("Success");
                }
                else
                {
                    ViewBag.Msg = $"删除失败：{result.message}";
                }
            }
            else
            {
                ViewBag.Msg = "出现了不可预期的异常，请重新操作";
            }
            return View("Error");
        }
        private async Task<ActionResult> Add(RoleMenu model,string returnUrl)
        {
            string userName = User.Identity.Name;
            var result = await service.add(model, userName);
            ViewBag.ReturnUrl = returnUrl;
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
        public async Task<ActionResult> AddFirst(Pager<List<Menu>> pager, RoleMenuQueryModel condition)
        {
            condition.menuLevel = 1;
            var result = await service.searchNotInList(pager, condition);
            ViewBag.Menu = pager;
            return View(condition);
        }
        [HttpPost]
        public async Task<ActionResult> AddFirst(RoleMenu model, RoleMenuQueryModel condition)
        {
            return await Add(model, $"/RoleMenu/AddFirst?roleId={condition.roleId}");
        }
        public async Task<ActionResult> AddSecond(Pager<List<Menu>> pager, RoleMenuQueryModel condition)
        {
            condition.menuLevel = 2;
            var result_second = await service.searchNotInList(pager, condition);
            ViewBag.Menu = pager;
            return View(condition);
        }
        [HttpPost]
        public async Task<ActionResult> AddSecond(RoleMenu model, RoleMenuQueryModel condition)
        {
            return await Add(model, $"/RoleMenu/AddSecond?roleId={model.roleId}&parentMenuId={condition.parentMenuId}");
        }
    }
}