using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Infra.CrossCutting.Helper
{
    public class EnumDescription
    {
        public static string Get(Enum name)
        {
            var type = name.GetType();
            var info = type.GetMember(name.ToString());
            if (info.Length <= 0) return name.ToString();
            var attr = info[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return attr.Length > 0 ? ((DisplayAttribute)attr[0]).Name : name.ToString();
        }
    }
}