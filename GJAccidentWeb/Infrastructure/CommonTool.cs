using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace GJAccidentWeb.Infrastructure
{
    public static class CommonTool
    {
        /// <summary>
        /// 一级目录表名
        /// </summary>
        public const string FIRST_LEVEL_TABLE = "web_menu_first";
        /// <summary>
        /// 二级目录表名
        /// </summary>
        public const string SECOND_LEVEL_TABLE = "web_menu_second";

        /// <summary>
        /// sql前缀 分页
        /// </summary>
        public const string SQL_HEAD = "select * from (select tt.*,rownum num from (";
        /// <summary>
        /// 用户拥有的模块href列表
        /// </summary>
        public static List<string> MODULELIST { get; set; }
        /// <summary>
        /// sql后缀 分页
        /// </summary>
        /// <param name="start">开始index</param>
        /// <param name="end">结束index</param>
        /// <returns></returns>
        public static string getSQL_TAIL(int start, int end)
        {
            return ") tt where rownum<=" + end + ") where num>" + start;
        }
        /// <summary>
        /// 获取目录表名
        /// </summary>
        /// <param name="level">目录级别</param>
        /// <returns></returns>
        public static string getMENUTABLE(MenuLevel level)
        {
            string tableName = "";
            switch (level)
            {
                case MenuLevel.First_Level:
                    tableName = FIRST_LEVEL_TABLE;
                    break;
                case MenuLevel.Second_Level:
                    tableName = SECOND_LEVEL_TABLE;
                    break;
                default:
                    break;
            }
            return tableName;
        }
    }
    public enum MenuLevel
    {
        First_Level = 1,
        Second_Level
    }
    /// <summary>
    /// 数据权限等级
    /// </summary>
    public enum DataLevel
    {
        /// <summary>
        /// 异常
        /// </summary>
        ExceptLevel = 0,
        /// <summary>
        /// 分公司
        /// </summary>
        Company,
        /// <summary>
        /// 线路
        /// </summary>
        Line,
        /// <summary>
        /// 公交车
        /// </summary>
        Bus
    }
}