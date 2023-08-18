using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class VehicleMasterViewModel
    {
        public string VehicleId { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleCode { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^[A-Z]{2}-\d{2}-[A-Z]{1,2}-\d{4}$", ErrorMessage = "Invalid Gujarat Vehicle Number e.g AA-00-AA-0000 or e.g AA-00-A-0000")]
        public string VehicleRegNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleRegDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleType { get; set; }
        [Required(ErrorMessage = "*")]
        public string VehicleMake { get; set; }
        [Required(ErrorMessage = "*")]
        public string Model { get; set; }
        [Required(ErrorMessage = "*")]
        public string Status { get; set; }
        [Required(ErrorMessage = "*")]
        public string RatePerKm { get; set; }
        [Required(ErrorMessage = "*")]
        public string RateEffectiveDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string InsuranceCommencementDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string InsuranceExpiryDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractStartDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractEndDate { get; set; }
        public string CompanyCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string IsActive { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public string ContractorId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string CenterId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorContactNo { get; set; }
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentList { get; set; }
        public List<BanasCenterMasterRetrieve_Result> CenterList { get; set; }
        public List<BanasContractorMasterRetrieve_Result> ContractorMasterList { get; set; }
        public string Action { get; set; }
    }
}
