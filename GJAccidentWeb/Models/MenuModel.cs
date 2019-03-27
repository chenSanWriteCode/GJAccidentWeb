using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GJAccidentWeb.Models
{
    public class MenuModel
    {
        public string title { get; set; }
        public string url { get; set; }
        public string html { get; set; }
        public bool open { get; set; }
        public List<MenuModel> children { get; set; }
    }
}