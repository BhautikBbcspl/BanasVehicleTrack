using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class UserDepartmentAllocationViewModel
    {
        public string Action { get; set; }
        public string IsActive { get; set; }
        public int UserDepartmentId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeCode { get; set; }
        public List<BanasUserDepartmentIntegrationRtr_Result> UserDepartmentMasterList { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentList { get; set; }
    }
}
