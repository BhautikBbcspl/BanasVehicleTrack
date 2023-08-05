using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class UserCenterAllocationViewModel
    {
        public string Action { get; set; }
        public string IsActive { get; set; }
        public int UserCenterId { get; set; }
        public int CenterId { get; set; }
        public string EmployeeCode { get; set; }
        public List<BanasUserCenterIntegrationRtr_Result> UserCenterMasterList { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeList { get; set; }
        public List<BanasCenterMasterRetrieve_Result> CenterMasterList { get; set; }

    }
}
