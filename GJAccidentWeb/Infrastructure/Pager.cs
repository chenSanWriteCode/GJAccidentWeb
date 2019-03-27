using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Entity
{
    public class Pager<T> where T :class,new()
    {
        public Pager()
        {
            data = new T();
        }
        //public Pager(int page)
        //{
        //    this.page = page;
        //    data = new T();
        //}
        public Pager(int page,int recPerPage)
        {
            this.page = page;
            this.recPerPage = recPerPage;
            data = new T();
        }
        public Pager(int recPerPage)
        {
            this.recPerPage = recPerPage;
            data = new T();
        }

        /// <summary>
        /// 初始状态的当前页码
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 总记录数目
        /// </summary>
        public int recTotal { get; set; } = 0;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int recPerPage { get; set; } = 10;
        /// <summary>
        /// 结果集
        /// </summary>
        public T data { get; set; }
    }
}