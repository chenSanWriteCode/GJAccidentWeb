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
    public class RoleRightController : Controller
    {
        [Dependency]
        public IRoleRightService service { get; set; }

        // GET: RoleRight
        public ActionResult Index(Pager<List<RoleRight>> pager, RoleRightQueryModel condition)
        {
            ViewBag.Condition = condition;
            return View(pager);
        }
        public async Task<ActionResult> Search(Pager<List<RoleRight>> pager, RoleRightQueryModel condition)
        {
            var result = await service.search(pager, condition);
            if (result.success)
            {
                ViewBag.Condition = condition;
                return View("Index", pager);
            }
            else
            {
                ViewBag.ReturnUrl = "/RoleRight/Index";
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete([Required]int id, string roleId)
        {
            ViewBag.ReturnUrl = "/RoleRight/Search?roleId=" + roleId;
            if (ModelState.IsValid)
            {
                var result = await service.delete(new RoleRightQueryModel { id = id });
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
        public async Task<ActionResult> Add(RoleRightQueryModel condition)
        {
            // 1 分公司
            // 2 线路
            // 3 车辆
            var level = await service.getRoleRightLevel(condition);
            switch (level)
            {
                case 1:
                    return View("AddCompany", condition);
                case 2:
                    return View("AddLine", condition);
                case 3:
                    return View("AddBus", condition);
                default:
                    ViewBag.ReturnUrl = "/RoleRight/Search?roleId=" + condition.roleId;
                    ViewBag.Msg = "出现了不可预期的异常，请重新操作";
                    return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Add(RoleRight model,RoleRightQueryModel condition)
        {
            ViewBag.ReturnUrl = $"/RoleRight/Add?roleId={condition.roleId}&companyId={condition.companyId}&lineId={condition.lineId}" ;
            if (string.IsNullOrEmpty(model.roleId)|| model.rightId==0)
            {
                ViewBag.Msg = "出现了不可预期的异常，请重新操作";
            }
            else
            {
                string userName = User.Identity.Name;
                var result = await service.add(model, userName);
                if (result.success)
                {
                    return View("Success");
                }
                else
                {
                    ViewBag.Msg = result.message;
                }
            }
            return View("Error");
        }
        //[HttpPost]
        //public async Task<ActionResult> AddLine(RoleRight model)
        //{
        //    ViewBag.ReturnUrl = "/RoleRight/Add?roleId=" + model.roleId;
        //    if (string.IsNullOrEmpty(model.roleId) || model.rightId == 0)
        //    {
        //        ViewBag.Msg = "出现了不可预期的异常，请重新操作";
        //    }
        //    else
        //    {
        //        string userName = User.Identity.Name;
        //        var result = await service.add(model, userName);
        //        if (result.success)
        //        {
        //            return View("Success");
        //        }
        //        else
        //        {
        //            ViewBag.Msg = result.message;
        //        }
        //    }
        //    return View("Error");
        //}
        //[HttpPost]
        //public async Task<ActionResult> AddBus(RoleRight model)
        //{
        //    ViewBag.ReturnUrl = "/RoleRight/Add?roleId=" + model.roleId;
        //    if (string.IsNullOrEmpty(model.roleId) || model.rightId == 0)
        //    {
        //        ViewBag.Msg = "出现了不可预期的异常，请重新操作";
        //    }
        //    else
        //    {
        //        string userName = User.Identity.Name;
        //        var result = await service.add(model, userName);
        //        if (result.success)
        //        {
        //            return View("Success");
        //        }
        //        else
        //        {
        //            ViewBag.Msg = result.message;
        //        }
        //    }
        //    return View("Error");
        //}
    }
}