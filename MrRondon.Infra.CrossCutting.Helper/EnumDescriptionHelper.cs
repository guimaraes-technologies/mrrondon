using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Infra.CrossCutting.Helper
{
    public class EnumDescription
    {
        public static string Get(Enum nome)
        {
            var type = nome.GetType();
            var info = type.GetMember(nome.ToString());
            if (info.Length <= 0) return nome.ToString();
            var attr = info[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return attr.Length > 0 ? ((DisplayAttribute)attr[0]).Name : nome.ToString();
        }
    }
}