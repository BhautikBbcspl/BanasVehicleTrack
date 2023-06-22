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
        public List<BanasVehicleMasterRtr_Result> VehicleMasterList { get; set; }
        public string Action { get; set; }
    }
}
