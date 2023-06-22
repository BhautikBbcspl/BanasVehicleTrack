using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.GatePass
{
    public class VisitManagementViewModel
    {
        public string Action { get; set; }
        public string GatepassId { get; set; }
        public string VisitDateTime { get; set; }
        public string CloseDateTime { get; set; }
        public string VehicleRegNumber { get; set; }


        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleId { get; set; }

        public List<BanasVehicleGatepassRetrieve_Result> visitDateTimeList { get; set; }
        public List<BanasVehicleGatepassRetrieve_Result> GatePassIdList { get; set; }

        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }

        public List<BanasVisitManagementRtr_Result> BanasVisitManagementRtr_Results { get; set; }
    }
}
