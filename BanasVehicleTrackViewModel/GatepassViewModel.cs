using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class GatepassViewModel
    {
        public string GatePassId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string UserCode { get; set; }
        [StringLength(200, ErrorMessage = "The OtherUser1 cannot exceed 200 characters.")]
        public string OtherUser1 { get; set; }
        [StringLength(200, ErrorMessage = "The OtherUser2 cannot exceed 200 characters.")]
        public string OtherUser2 { get; set; }
        [StringLength(200, ErrorMessage = "The OtherUser3 cannot exceed 200 characters.")]
        public string OtherUser3 { get; set; }
        [Required(ErrorMessage = "*")]
        public string InformationMode { get; set; }
        [Required(ErrorMessage = "*")]
        public string VisitDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string VisitPurpose { get; set; }
        [StringLength(150, ErrorMessage = "The Module Name cannot exceed 150 characters.")]
        public string Remarks { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleDepartment { get; set; }
        [StringLength(50, ErrorMessage = "The Module Name cannot exceed 50 characters.")]
        [Required(ErrorMessage = "*")]
        public string Driver { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleId { get; set; }
         [Required(ErrorMessage = "*")]
        public string StartOdometer { get; set; }
        public string CloseOdometer { get; set; }
        public string CloseDateTime { get; set; }
        public string CloseRemark { get; set; }
        public string Netkm { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string CloseUser { get; set; }
        public string CloseDate { get; set; }
        public string EditUser { get; set; }
        public string EditDate { get; set; }
        public string action { get; set; }
     public string difference { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentList { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeList { get; set; }
        public List<BanasVisitPurposeMasterRtr_Result> VisitPurposeList { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleList { get; set; }
    }
}
