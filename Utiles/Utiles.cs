using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace UI_Museum.Utiles
{
    public static class Utiles
    {
        public static List<SelectListItem> ToListSelectListItem<T>()
        {
            var t = typeof(T);

            if (!t.IsEnum) { throw new ApplicationException("Type must be enum"); }

            var members = t.GetFields(BindingFlags.Public | BindingFlags.Static);

            var result = new List<SelectListItem>();

            foreach (var member in members)
            {
                var attributeDescription = member.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute),
                    false);
                var description = member.Name;

                if (attributeDescription.Any())
                {
                    description = ((System.ComponentModel.DescriptionAttribute)attributeDescription[0]).Description;
                }

                var valor = ((int)Enum.Parse(t, member.Name));
                result.Add(new SelectListItem()
                {
                    Text = description,
                    Value = valor.ToString()
                });
            }
            return result;
        }
    }
}
