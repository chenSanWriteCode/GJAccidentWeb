using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models;
using GJAccidentWeb.Services;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class AccidentController : Controller
    {
        [Dependency]
        public IAccidentService service { get; set; }
        [Dependency]
        public ICommonService commonService { get; set; }
        [Dependency]
        public IUserService userService { get; set; }
        // GET: Accident
        public ActionResult Index(Pager<List<Accident>> pager, AccidentQueryModel condition)
        {
            ViewBag.Condition = condition;
            return View(pager);
        }
        public async Task<ActionResult> Search(Pager<List<Accident>> pager, AccidentQueryModel condition)
        {
            var lines = await commonService.lineInfo();

            if (condition.lineId.Contains(","))
            {
                List<string> lineName = new List<string>();
                foreach (var item in condition.lineId.Split(','))
                {
                    lineName.Add(lines.FirstOrDefault(x => x.lineId == item)?.lineName);
                }
                condition.lineName = "'" + string.Join("','", lineName.ToArray()) + "'";
            }
            else
            {
                condition.lineName = "'" + lines.FirstOrDefault(x => x.lineId == condition.lineId)?.lineName + "'";
            }

            var result = await service.search(pager, condition);
            if (result.success)
            {
                ViewBag.Condition = condition;
                return View("Index", pager);
            }
            ViewBag.Msg = result.message;
            ViewBag.ReturnUrl = "/Accident/Index";
            return View("Error");
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View(new AccidentModel());
        }
        [HttpPost]
        public async Task<ActionResult> Add(AccidentModel model)
        {
            if (ModelState.IsValid)
            {
                var lines = await commonService.lineInfo();
                model.lineName = lines.First(x => x.lineId == model.lineId).lineName;
                var userModel = await userService.searchModelByCondition(new UserInfo { userNo = User.Identity.Name });
                if (userModel.success)
                {
                    model.optId = userModel.data.id;
                }
                //model.optId = User.Identity.Name;
                var depart = await commonService.getDepartInfoByLine(new Line { lineId = model.lineId });
                model.dwName = depart.departName;
                model.dwId = Convert.ToInt32(depart.departId);
                var result = await service.add(model);
                if (result.success)
                {
                    ModelState.AddModelError("", "添加成功");
                }
                else
                {
                    ModelState.AddModelError(" ", "添加失败 " + result.message);
                }

            }

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Update(string id)
        {
            var result = await service.findById(id);
            if (result.success)
            {
                return View(result.data);
            }
            else
            {
                ViewBag.Msg = result.message;
                ViewBag.ReturnUrl = "/Accident/Index";
                return View("Error");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Update(AccidentModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = await userService.searchModelByCondition(new UserInfo { userNo = User.Identity.Name });
                if (userModel.success)
                {
                    model.optId = userModel.data.id;
                }
                //model.optId = User.Identity.Name;
                var lines = await commonService.lineInfo();
                model.lineName = lines.First(x => x.lineId == model.lineId).lineName;
                var depart = await commonService.getDepartInfoByLine(new Line { lineId = model.lineId });
                model.dwName = depart.departName;
                model.dwId = Convert.ToInt32(depart.departId);
                var result = await service.update(model);
                if (result.success)
                {
                    ModelState.AddModelError("", "修改成功");
                }
                else
                {
                    ModelState.AddModelError("", "修改失败" + result.message);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await service.delete(id);
            ViewBag.ReturnUrl = "/Accident/Index";
            if (result.success)
            {
                ViewBag.Msg = "删除成功";
                return View("Success");
            }
            else
            {
                ViewBag.Msg = result.message;
                return View("Error");
            }

        }
        [HttpGet]
        public async Task<JsonResult> Get(string id)
        {
            var result = await service.findById(id);
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        [HttpPost]
        public async Task<FileResult> Export(AccidentQueryModel condition)
        {
            var lines = await commonService.lineInfo();

            if (condition.lineId.Contains(","))
            {
                List<string> lineName = new List<string>();
                foreach (var item in condition.lineId.Split(','))
                {
                    lineName.Add(lines.FirstOrDefault(x => x.lineId == item)?.lineName);
                }
                condition.lineName = "'" + string.Join("','", lineName.ToArray()) + "'";
            }
            else
            {
                condition.lineName = "'" + lines.FirstOrDefault(x => x.lineId == condition.lineId).lineName + "'";
            }
            List<AccidentExportModel> dataList = await service.getListByCondition(condition);
            ExcelHelper excelHelper = new ExcelHelper();
            IWorkbook workbook = excelHelper.export(dataList, ExcelType.Excel2007);
            //MemoryStream ms = new MemoryStream();
            byte[] bytes;
            try
            {
                using (MemoryStream ms = new MemoryStream()) {
                    workbook.Write(ms);
                    string fileName = DateTime.Now.ToShortDateString();
                    bytes = ms.GetBuffer();
                    return File(bytes, "application/vnd.ms-excel",$"{fileName}.xlsx");
                }
                
            }
            catch (Exception err)
            {
                ILog log = LogManager.GetLogger("AccidentController-Export");
                log.Error(err.Message);
            }
            return File("", "application/vnd.ms-excel");

        }

    }
}