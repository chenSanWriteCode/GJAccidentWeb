using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Services;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class Menu_SecondController : Controller
    {
        [Dependency]
        public IMenuService service { get; set; }
        // GET: Menu_Second
        public async Task<ActionResult> Index(Pager<List<Menu>> pager)
        {
            var result = await service.getMenus(pager, MenuLevel.Second_Level);
            if (result.success)
            {
                return View(pager);
            }
            else
            {
                ViewBag.ReturnUrl = "/Menu_Second/Index";
                ViewBag.Msg = result.message;
                return View("Error");
            }
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(Menu model)
        {
            string userName = User.Identity.Name; 
            var result = await service.add(model, userName, MenuLevel.Second_Level);
            ViewBag.ReturnUrl = "/Menu_Second/Index";
            if (result.success)
            {
                ViewBag.Msg = "增加成功！";
                return View("Success");
            }
            else
            {
                ViewBag.Msg = $"增加失败：{result.message}";
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await service.delete(id, MenuLevel.Second_Level);
            ViewBag.ReturnUrl = "/Menu_Second/Index";
            if (result.success)
            {
                return View("Success");
            }
            else
            {
                ViewBag.Msg = $"删除失败：{result.message}";
                return View("Error");
            }
        }

        public async Task<ActionResult> Update(int id)
        {
            var result = await service.getSingleMenuById(id, MenuLevel.Second_Level);
            if (result.success)
            {
                if (result.data.id != 0)
                {
                    return View(result.data);
                }
            }
            ViewBag.ReturnUrl = "/Menu_Second/Index";
            ViewBag.Msg = result.message == null ? "记录不存在" : result.message;
            return View(result.data);
        }
        [HttpPost]
        public async Task<ActionResult> Update(Menu model)
        {
            string userName = User.Identity.Name;
            var result = await service.update(model, userName, MenuLevel.Second_Level);
            ViewBag.ReturnUrl = "/Menu_Second/Index";
            if (result.success)
            {
                ViewBag.Msg = "修改成功！";
                return View("Success");
            }
            else
            {
                ViewBag.Msg = $"修改失败：{result.message}";
                return View("Error");
            }
        }
    }
}