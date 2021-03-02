using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EducationPortal.BLL.Services;
using EducationPortal.DAL.Interfaces;
using EducationPortal.DAL.Repository;
using EducationPortal.WEB.App_Start;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.WebApi;

namespace EducationPortal.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new EducationPortal.DAL.EF.Initializer.EDUDbInitializer());

            AreaRegistration.RegisterAllAreas();

            // Manually installed WebAPI 5.2.7 after making an MVC project.
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            container.Register<IRepository, InDatabaseRepository>(Lifestyle.Scoped);
            container.Register<AccountService>();
            container.Register<CourseService>();
            container.Register<MaterialService>();
            container.Register<MaterialCourseService>();
            container.Register<SkillService>();
            container.Register<UserCourseService>();

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            // Here your usual Web API configuration stuff.
        }
    }
}
