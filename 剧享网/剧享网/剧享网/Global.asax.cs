using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace 剧享网
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //开启一个线程，用于扫描异常信息队列
            string filepath = Server.MapPath("~/Log/");
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    if (UI.Models.MyExceptionAttribute.exceptionQueue.Count() > 0)
                    {
                        Exception ex = UI.Models.MyExceptionAttribute.exceptionQueue.Dequeue();

                        if (ex != null)
                        {
                            //将异常信息写到日志文件中
                            string filename = DateTime.Now.ToString("yyyy-MM-dd");
                            File.AppendAllText(filepath + filename + ".txt", ex.ToString(), System.Text.Encoding.UTF8);
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            }, filepath);
        }
    }
}

