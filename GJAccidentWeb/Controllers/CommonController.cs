using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Services;
using Unity;

namespace GJAccidentWeb.Controllers
{
    public class CommonController : Controller
    {
        [Dependency]
        public ICommonService service { get; set; }
        // GET: Common
        /// <summary>
        /// 根据分公司获取包括的所有线路
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="database">0 默认；1 Money</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> LineInfo(string companyId = null, int database = 0, ModelType type = ModelType.AllModel)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (type == ModelType.AllModel)
            {
                dict.Add("所有", "0");
            }
            var result = await service.lineInfo(User.Identity.Name,companyId,database);
            result.ForEach(x => dict.Add(x.lineName, x.lineId));
            if (type == ModelType.AllModel)
            {
                dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "");
            }
            return Json(dict);
        }
        [HttpPost]
        public async Task<JsonResult> CarInfo(string lineId, ModelType type = ModelType.AllModel)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (type == ModelType.AllModel)
            {
                dict.Add("所有", "");
            }
            var result = await service.carInfo(User.Identity.Name, lineId);
            result.ForEach(x => dict.Add(x.carNum, x.carNum));
            //if (type == ModelType.AllModel)
            //{
            //    dict["所有"] = (dict.Count > 1 ? string.Join(",", dict.Values.ToList()) : "0");
            //}
            return Json(dict);
        }
        [HttpPost]
        public async Task<JsonResult> LineUDInfo(string lineId, ModelType type = ModelType.AllModel)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (type == ModelType.AllModel)
            {
                dict.Add("所有", "");
            }
            var result = await service.lineUDInfo(lineId);
            result.ForEach(x => dict.Add(x.LineDir, x.LineDir));
            return Json(dict);
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="upOrDown">上下行</param>
        /// <returns></returns>
        public async Task<JsonResult> getStationInfo(string lineId = null, string upOrDown = null)
        {
            var station = await service.getStationInfo(lineId, upOrDown);
            return Json(station);
        }
       
        
        /// <summary>
        /// 获取不在权限内的线路
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<JsonResult> getLineInfoNotInList(string roleId, string companyId = null)
        {
            var line = await service.getLineInfoNotInList(companyId, roleId);
            return Json(line);
        }
        /// <summary>
        /// 获取不在权限内的公交车
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<JsonResult> getBusInfoNotInList(string roleId, string lineId = null)
        {
            var bus = await service.getBusInfoNotInList(lineId, roleId);
            return Json(bus);
        }
        /// <summary>
        /// 左侧菜单
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> getMenuInfo()
        {
            string userName = User.Identity.Name;
            var result = await service.getMenuInfo(userName);
            return Json(result);
        }
    }
}