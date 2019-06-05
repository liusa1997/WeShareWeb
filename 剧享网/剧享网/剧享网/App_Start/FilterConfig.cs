using System.Web;
using System.Web.Mvc;

namespace 剧享网
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //远程服务器获取异常要用
            //filters.Add(new UI.Models.MyExceptionAttribute());
        }
    }
}
