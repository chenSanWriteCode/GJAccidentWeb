using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GJAccidentWeb.Infrastructure
{
    public class ExcelHelper
    {
        public IWorkbook export<T>(List<T> resource, ExcelType type) where T : class, new()
        {
            IWorkbook workbook = null;
            switch (type)
            {
                case ExcelType.Excel2003:
                    workbook = new HSSFWorkbook();
                    break;
                case ExcelType.Excel2007:
                    workbook = new XSSFWorkbook();
                    break;
                default:
                    return workbook;
            }
            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);
            DataTable dt_resource = ToDataTable(resource);
            var columns = getClassPropNames(typeof(T));
            for (int i = 0; i < columns.Count(); i++)
            {
                row.CreateCell(i).SetCellValue(columns[i].exportName);
            }

            for (int i = 0; i < resource.Count(); i++)
            {
                IRow tempRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < columns.Count(); j++)
                {
                    tempRow.CreateCell(j).SetCellValue(Convert.ToString(dt_resource.Rows[i][j]));
                }
            }
          
            return workbook;
        }

        private List<ExcelColumn> getClassPropNames(Type type)
        {
            PropertyInfo[] props = type.GetProperties();
            List<ExcelColumn> names = new List<ExcelColumn>();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].IsDefined(typeof(ExcelColumnAttribute), false))
                {
                    var attribute = props[i].GetCustomAttribute<ExcelColumnAttribute>(false);
                    names.Add(new ExcelColumn
                    {
                        exportName = attribute.Name
                    });
                }
            }
            return names;
        }

        private DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
    }
    public enum ExcelType
    {
        Excel2003 = 0,
        Excel2007
    }
    public class ExcelColumn
    {
        public string exportName { get; set; }
        public int columnIndex { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExcelColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public ExcelOperationType OperationType { get; set; } = ExcelOperationType.Export;
    }
    public enum ExcelOperationType
    {
        Export = 0,
        Import
    }
}