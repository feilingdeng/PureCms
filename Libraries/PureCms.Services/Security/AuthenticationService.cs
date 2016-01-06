using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace PureCms.Services.Security
{
    public class AuthenticationService
    {
        public static List<string> GetAllActionByAssembly()
        {
            var result = new List<string>();

            var types = Assembly.Load("PureCms.Web").GetTypes().Where(t => typeof(IController).IsAssignableFrom(t)).ToList();

            foreach (var type in types)
            {
                var members = type.GetMethods();
                foreach (var member in members)
                {
                    if (member.ReturnType == typeof(ActionResult) || member.ReturnType.BaseType == typeof(ActionResult))//如果是Action
                    {
                        string s = member.DeclaringType.Name.Substring(0, member.DeclaringType.Name.Length - 10) + "." + member.Name;

                        object[] attrs = member.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        if (attrs.Length > 0)
                            s += (attrs[0] as System.ComponentModel.DescriptionAttribute).Description;

                        result.Add(s);
                    }

                }
            }
            return result;
        }
    }
}
