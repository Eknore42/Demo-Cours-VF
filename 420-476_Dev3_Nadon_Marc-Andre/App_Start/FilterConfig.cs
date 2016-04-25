using System.Web;
using System.Web.Mvc;

namespace _420_476_Dev3_Nadon_Marc_Andre
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
