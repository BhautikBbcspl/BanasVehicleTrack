using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BanasVehicleTrackApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //LOG4NET
            string currentDate = DateTime.Now.ToString("yyyy_MM_dd");
            string logFileName = $"C:\\BanasVehicleApiLog\\BanasVehicleTracker-log-{currentDate}.txt";
            // Create a new file appender
            var fileAppender = new FileAppender
            {
                File = logFileName, // Specify the absolute path for the log file
                AppendToFile = true
            };

            // Configure the layout of the log messages
            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            patternLayout.ActivateOptions();
            fileAppender.Layout = patternLayout;

            // Activate the appender
            fileAppender.ActivateOptions();

            // Add the appender to the root logger
            BasicConfigurator.Configure(fileAppender);

            // Other application startup code
        }
    }
}
