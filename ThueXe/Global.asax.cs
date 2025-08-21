using ThueXe.DAL;
using ThueXe.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web;


namespace ThueXe
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataEntities  , Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var unitofWork = new UnitOfWork())
            {
                Application["ConfigSite"] = unitofWork.ConfigSiteRepository.GetQuery().FirstOrDefault();
            }
        }
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Response.Clear();

            var httpException = exception as HttpException;
            if (httpException != null)
            {
                var code = httpException.GetHttpCode();

                if (code == 404)
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                }
                else
                {
                    Server.ClearError();
                    Response.Redirect("/general");
                }
            }
            else
            {
                Server.ClearError();
                Response.Redirect("/not-found");
            }
        }
    }
}
