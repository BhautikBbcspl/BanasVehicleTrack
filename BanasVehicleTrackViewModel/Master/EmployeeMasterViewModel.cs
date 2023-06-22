using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class EmployeeMasterViewModel
    {
        public string EmployeeId { get; set; }
        [Required(ErrorMessage = "*")]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Contact { get; set; }
        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public string RoleId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string IsActive { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeMasterList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public List<BanasRoleMasterRetrieve_Result> RoleMasterList { get; set; }
        public string Action { get; set; }
    }
}
