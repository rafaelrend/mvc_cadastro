using System.Web;
using System.Web.Mvc;

namespace Mvc_Aspnet_Cadastro
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}