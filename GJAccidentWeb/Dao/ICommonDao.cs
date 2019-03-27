using System.Collections.Generic;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;

namespace GJAccidentWeb.Dao
{
    public interface ICommonDao
    {
        //List<Line> lineInfo(string userName,string companyId);
        List<Car> carInfo(string userName,string lineName);
        List<LineUD> lineUDInfo(string lineName);
        List<Depart> departInfo();

        /// <summary>
        /// 根据线路、上下行获取包括的站点
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="upOrDown"></param>
        /// <returns></returns>
        List<Station> getStationInfo(string lineId, string upOrDown = null);
        /// <summary>
        /// 根据分公司获取包括的所有线路
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="database">0 默认；1 Money</param>
        /// <returns></returns>
        List<Line> lineInfo(string companyId, int database = 0);
        /// <summary>
        /// 根据分公司获取不在权限内的线路
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Line> getLineInfoNotInList(string companyId, string roleId);
        /// <summary>
        /// 根据线路获取不在权限内的公交车
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Car> getBusInfoNotInList(string lineId, string roleId);
        /// <summary>
        /// layout 的 menu
        /// </summary>
        /// <returns></returns>
        List<MenuModel> getMenuInfo(string userName);
        /// <summary>
        /// 获取目录的列表  没有children 都在一个list中
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<MenuModel> getMenuList(string userName);
        Depart getDepartInfoByLine(Line line);
    }
}
