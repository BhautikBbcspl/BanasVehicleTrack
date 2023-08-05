using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanasVehicleTrackDbClasses;

namespace BanasVehicleTrackViewModel
{
    public class DashboardViewModel
    {
        public string EmployeeCode { get; set; }
        public List<BanasAdminDashboardCountRtr_Result> DashboardCounts { get; set; }
        public List<BanasAdminDashboardListRtr_Result> DashboardList { get; set; }
    }
}
