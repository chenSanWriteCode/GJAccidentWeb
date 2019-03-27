using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using GJAccidentWeb.Entity;

namespace GJAccidentWeb.Infrastructure
{
    public class BeanHelper
    {
        public static void CopyBean<Aim, Source>(ref Aim aim, Source source) where Aim : class, new() where Source : class, new()
        {
            try
            {
                var Types = source.GetType();//获得类型  
                var Typea = typeof(Aim);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo ap in Typea.GetProperties())
                    {
                        if (ap.Name == sp.Name && ap.PropertyType.Name == sp.PropertyType.Name)//判断属性名是否相同  
                        {
                            ap.SetValue(aim, sp.GetValue(source, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception)
            {
                aim = new Aim();
            }
        }
        /// <summary>
        /// 整合结果
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static Result<object> combineResult(params Result<object>[] results)
        {
            Result<object> result = new Result<object>();
            if (results != null)
            {
                foreach (var item in results)
                {
                    if (!item.success)
                    {
                        result.addError(item.message);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string getFistCharUpper(string str)
        {
            string tempStr = "";
            char c = str.ToArray()[0];
            int asc = (int)c;
            if (asc >= 33 && asc <= 126)
            {
                if ((asc > 64 && asc < 91) || (asc > 96 && asc < 123))
                {
                    tempStr += c.ToString().ToUpper();
                }
                else
                {
                    tempStr += "*";
                }
            }
            else
            {//累加拼音声母   
                tempStr += GetPYChar(c.ToString()).ToUpper();
            }
            return tempStr;
        }
        ///    
        /// 取单个字符的拼音声母   
        ///    
        /// 要转换的单个汉字   
        /// 拼音声母   
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "j";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "*";
        }



    }
}
