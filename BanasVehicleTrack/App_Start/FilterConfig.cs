using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanasVehicleTrack
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
        }
    }
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    InnerException = filterContext.Exception?.InnerException?.ToString(),
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    ActionName = filterContext.RouteData.Values["action"].ToString(),
                    LineNumber = "Line Number: " + new StackTrace(filterContext.Exception, true).GetFrame(0).GetFileLineNumber(),
                    LogTime = GeneralClass.generalFunctions.getTimeZoneDatetimedb()
                };

                string msg = db.BanasExceptionLoggerIns(logger.ExceptionMessage, logger.ControllerName, logger.ActionName, logger.LineNumber, logger.ExceptionStackTrace, logger.InnerException, logger.LogTime,  "insert").FirstOrDefault();

                if (msg.Contains("successfully"))
                {
                    #region send Email                

                    HttpUnhandledException httpUnhandledException =
                                new HttpUnhandledException(filterContext.Exception.Message, filterContext.Exception);

                    var htmlError = httpUnhandledException.GetHtmlErrorMessage();

                    var emailTemplateMaster = db.BanasEmailTemplateMasterRtr("Active").Where(x => x.TemplateName == "ExceptionSummary").FirstOrDefault();

                    if (emailTemplateMaster != null)
                    {
                        string mailBody = htmlError;
                        string Subject = emailTemplateMaster.SubjectName;
                        string To = emailTemplateMaster.EmailTo;
                        string cc = emailTemplateMaster.EmailCC;
                        string bcc = emailTemplateMaster.EmailBCC;

                        GeneralClass.SendMail_Message(Subject, mailBody, To, cc, bcc);
                    }
                    #endregion HTML BODY

                    filterContext.ExceptionHandled = true;
                    filterContext.Result = new RedirectResult("~/ErrorLogs/Index", false);
                }
            }
        }
    }
}
