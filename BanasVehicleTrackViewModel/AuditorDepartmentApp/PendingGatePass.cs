using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanasVehicleTrackDbClasses;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.AuditorDepartmentApp
{
    public class PendingGatePass
    {
        public string GatePassId { get; set; }
        public string UserCode { get; set; }
        public string VisitDateTime { get; set; }
        public string VisitPurpose { get; set; }
        public string Remarks { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string VehicleId { get; set; }
        public string VehicleRegNumber { get; set; }
        public string Action { get; set; }
        public string GatePassStatus { get; set; }

    }
}
