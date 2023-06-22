using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanasVehicleTrack.GeneralClasses
{
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";
        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success background-success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger background-danger";
    }
}