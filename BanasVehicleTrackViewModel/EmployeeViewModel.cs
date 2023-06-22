using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class EmployeeViewModel
    {
        //[Required(ErrorMessage = "Please enter username.")]
        //[StringLength(50, ErrorMessage = "The field  must be a string with a maximum length of 50")]
        //public string Username { get; set; }
        //[Required(ErrorMessage = "Please enter password.")]

        //public string UserPassword { get; set; }
        //public string UserCode { get; set; }
        //public string CompanyCode { get; set; }
        //public string LabId { get; set; }
        //public string RoleId { get; set; }

        //public string RoleName { get; set; }
        //public string LabName { get; set; }
        //public string Error { get; set; }

        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "The field  must be a string with a maximum length of 50")]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        public string CompanyCode { get; set; }
        public string IsActive { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string DepartmentId { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}