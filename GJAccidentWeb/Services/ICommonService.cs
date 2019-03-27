using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;

namespace GJAccidentWeb.Services
{
    public interface ICommonService
    {
        Task<List<Car>> carInfo(string userName,string lineName);
        Task<List<LineUD>> lineUDInfo(string lineName);
        Task<List<Depart>> departInfo();
        Task<Depart> getDepartInfoByLine(Line line);

        /// <summary>
        /// 根据线路、上下行获取包括的站点
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="upOrDown"></param>
        /// <returns></returns>
        Task<List<Station>> getStationInfo(string lineId = null, string upOrDown = null);
        /// <summary>
        /// 根据分公司获取包括的所有线路
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="database">0 默认；1 Money</param>
        /// <returns></returns>
        Task<List<Line>> lineInfo(string companyId = null, int database = 0);
        /// <summary>
        /// 根据分公司获取不在权限内的线路
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<Line>> getLineInfoNotInList(string companyId, string roleId);
        /// <summary>
        /// 根据线路获取不在权限内的公交车
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<Car>> getBusInfoNotInList(string lineId, string roleId);
        Task<List<MenuModel>> getMenuInfo(string userName);
        /// <summary>
        /// 获取用户拥有的目录url集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<string> getMenuHref(string userName);
    }
}
