using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class Car
    {
        public string carId { get; set; }
        public string carNum { get; set; }
    }
    /// <summary>
    /// 上下行
    /// </summary>
    public class LineUD
    {
        public int UDID { get; set; }
        /// <summary>
        /// 线路走向
        /// </summary>
        public string LineDir { get; set; }
        /// <summary>
        /// 上下行
        /// </summary>
        public string UpDown { get; set; }
    }
    public class Line
    {
        public string lineId { get; set; }
        public string lineName { get; set; }
    }
    /// <summary>
    /// 部门（分公司）
    /// </summary>
    public class Depart
    {
        public string departId { get; set; }
        public string departName { get; set; }

    }
   
    public struct Station
    {
        public int stationId { get; set; }
        public string stationName { get; set; }
    }
    /// <summary>
    /// chart图标数据
    /// </summary>
    public struct ChartInfo
    {
        public List<string> lables { get; set; }
        public Dictionary<string, List<int>> datas { get; set; }
    }

}