using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.AuditorDepartment
{
    public class AuditorMgmtViewModel
    {
        public string Action { get; set; }
        public string GatepassId { get; set; }

        [Required(ErrorMessage = "*")]
        public string VisitDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string CloseDateTime { get; set; }
        public string VehicleRegNumber { get; set; }


        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleId { get; set; }
        
        public List<BanasVehicleGatepassRetrieve_Result> visitDateTimeList { get; set; }
        public List<BanasVehicleGatepassRetrieve_Result> GatePassIdList { get; set; }
        public List<BanasAuditorDepartmentDashboardRetrieve_Result> BanasVehicleGatepassRetrieveList { get; set; }

        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractorId { get; set; }
        public List<BanasContractorMasterRetrieve_Result> ContractorList { get; set; }
        public List<BanasContractorAmountRpt_Result> BanasContractorAmountRptList { get; set; }
        public List<BanasAdminDashboardCountRtr_Result> DashboardCounts { get; set; }
        public List<BanasContractorTotalAmountMonthwiseRtr_Result> ContractorTotalAmountMonthwiseList { get; set; }

    }
}
