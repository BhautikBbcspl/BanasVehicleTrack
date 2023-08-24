using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanasVehicleTrackDbClasses;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.GatePass
{
    public class ViewStartGatePassViewModel
    {
        public string GatePassId { get; set; }
        public string UserCode { get; set; }
        public string UserCode1 { get; set; }
        public string UserCode2 { get; set; }
        public string OtherUser1 { get; set; }
        public string OtherUser2 { get; set; }
        public string OtherUser3 { get; set; }
        public string InformationMode { get; set; }

        public string VisitDateTime { get; set; }
        public string VisitPurpose { get; set; }
        public string Remarks { get; set; }
        public string VehicleDepartment { get; set; }
        public string Driver { get; set; }


        public string VehicleCode { get; set; }
        public string VehicleRegNumber { get; set; }
        public string StartOdometer { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string GatePassStatus { get; set; }
        [Required(ErrorMessage = "*")]
        public string CloseOdometer { get; set; }
        public string CloseRemark { get; set; }


        public string Difference { get; set; }

        [Required(ErrorMessage = "*")]
        public string CloseDateTime { get; set; }
        public string CloseDate { get; set; }
        public string CloseUser { get; set; }

        [Required(ErrorMessage = "*")]
        public string Netkm { get; set; }

        public string DepartmentId { get; set; }

        public string Action { get; set; }
        public string VehicleId { get; set; }
        public string EditUser { get; set; }
        public string EditDate { get; set; }


        public List<BanasVehicleGatepassRetrieve_Result> BanasVehicleGatepassRetrieveList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
    }

}