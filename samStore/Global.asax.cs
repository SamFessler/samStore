using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using samStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace samStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (IdentityModels entities = new IdentityModels())
            {

                var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(entities));
                if (!rm.RoleExists("Administrator"))
                {
                    rm.Create(new IdentityRole { Name = "Administrator" });
                }
                if (!rm.RoleExists("ProductAdministrator"))
                {
                    rm.Create(new IdentityRole { Name = "ProductAdministrator" });
                }


            }
        }
    }
}
