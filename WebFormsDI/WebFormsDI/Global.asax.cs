using Autofac;
using Autofac.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebFormsDI.Services;

namespace WebFormsDI
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        
        public IContainerProvider ContainerProvider { get => containerProvider; }

        void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            // register dependencies
            builder.RegisterType<SampleService>().As<ISampleService>();
            builder.RegisterType<SampleInnerService>().As<ISampleInnerService>();

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            containerProvider = new ContainerProvider(builder.Build());
        }

        private static IContainerProvider containerProvider;
    }
}