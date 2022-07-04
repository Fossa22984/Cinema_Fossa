using System;
using System.Linq;
using System.Reflection;

namespace OnlineCinema_Core.Extensions
{
    public static class ObjectExtensions
    {
        public static Tin Copy<Tin, TFrom>(this Tin obj, TFrom fromObj)
        {
            if (obj == null || fromObj == null) return obj;

            Func<PropertyInfo, bool> query = p => p.PropertyType.IsValueType || p.PropertyType.Name == "String";

            var fields1 = obj.GetType().GetProperties().Where(query).ToArray();
            var fields2 = fromObj.GetType().GetProperties().Where(query).ToArray();

            for (int i = 0; i < fields2.Length; i++)
            {
                var value = fields2[i].GetValue(fromObj);
                fields1.FirstOrDefault(x => x.Name == fields2[i].Name)?.SetValue(obj, value);
            }

            return obj;
        }
    }
}
