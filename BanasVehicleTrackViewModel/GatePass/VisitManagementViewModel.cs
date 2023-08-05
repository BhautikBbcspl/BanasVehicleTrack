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
        [Required(ErrorMessage = "*")]
        public string VisitDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string CloseDateTime { get; set; }
        public string VehicleRegNumber { get; set; }
        public string DepartmentId { get; set; }
        public string Center { get; set; }
        public string VehicleId { get; set; }
        public string UserCode { get; set; }
        public List<BanasCenterMasterRetrieve_Result> CenterList { get; set; }
        public List<BanasVehicleGatepassRetrieve_Result> visitDateTimeList { get; set; }
        public List<BanasVehicleGatepassRetrieve_Result> GatePassIdList { get; set; }

        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeList { get; set; }

        public List<BanasVisitManagementRtr_Result> BanasVisitManagementRtr_Results { get; set; }
    }
}
