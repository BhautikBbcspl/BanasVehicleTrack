using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.AuditorDepartment
{
    public class AuditorFinalApproveViewModel
    {
        [Required(ErrorMessage = "*")]
        public string VisitDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string CloseDateTime { get; set; }
        public string DepartmentId { get; set; }
        public string VehicleId { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
        public List<BanasAuditorFinalApproveRetrieve_Result> BanasVehicleGatepassRetrieveList { get; set; }
    }
    public class DifferenceData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
