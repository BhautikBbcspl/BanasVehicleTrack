using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanasVehicleTrackDbClasses;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.AuditorDepartmentApp
{
    public class AuditorOperateVisitViewModel
    {
        public int Id { get; set; }
        public string GatePassId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string Odometer { get; set; }
        public string OdometerImage { get; set; }

        public string MidwayDeparture { get; set; }
        public string LastOdometer { get; set; }
        public string LastOdometerImage { get; set; }
        public string UserCode { get; set; }
        public string UserSignature { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string GatePassStatus { get; set; }


    }
}
