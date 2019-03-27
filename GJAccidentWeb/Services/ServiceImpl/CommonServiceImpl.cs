using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GJAccidentWeb.Dao;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;
using Unity;

namespace GJAccidentWeb.Services.ServiceImpl
{
    public class CommonServiceImpl : ICommonService
    {
        [Dependency]
        public ICommonDao dao { get; set; }
        public async Task<List<Car>> carInfo(string userName, string lineName)
        {
            return await Task.Factory.StartNew(() => dao.carInfo(userName, lineName));
        }

        public async Task<List<Depart>> departInfo()
        {
            return await Task.Factory.StartNew(() => dao.departInfo());
        }


        public async Task<List<LineUD>> lineUDInfo(string lineName)
        {
            return await Task.Factory.StartNew(() => dao.lineUDInfo(lineName));
        }

        public async Task<List<Line>> lineInfo(string companyId = null, int database = 0)
        {
            return await Task.Factory.StartNew(() => dao.lineInfo(companyId, database));
        }
        public async Task<List<Station>> getStationInfo(string lineId = null, string upOrDown = null)
        {
            return await Task.Factory.StartNew(() => dao.getStationInfo(lineId, upOrDown));
        }
        public async Task<List<MenuModel>> getMenuInfo(string userName)
        {
            return await Task.Factory.StartNew(() => dao.getMenuInfo(userName));
        }

        public List<string> getMenuHref(string userName)
        {
            var menu = dao.getMenuList(userName);
            List<string> result = new List<string>();
            foreach (var item in menu)
            {
                result.Add(item.url);
            }
            return result;
        }

        public async Task<List<Line>> getLineInfoNotInList(string companyId, string roleId)
        {
            return await Task.Factory.StartNew(() => dao.getLineInfoNotInList(companyId, roleId));
        }
        public async Task<List<Car>> getBusInfoNotInList(string lineId, string roleId)
        {
            return await Task.Factory.StartNew(() => dao.getBusInfoNotInList(lineId, roleId));
        }

        public async Task<Depart> getDepartInfoByLine(Line line)
        {
            return await Task.Factory.StartNew(() => dao.getDepartInfoByLine(line));
        }
    }
}