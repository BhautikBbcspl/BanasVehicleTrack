using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.SecurityDepartment
{
    public class SecurityVisitMgmtViewModel
    {
        public string Action { get; set; }
        public string GatePassId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartureDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal StartKm { get; set; }
        public bool DepartureVerifySTS { get; set; }
        public bool DepartureVerifyStatus { get; set; }
        public string DepartureSecurityId { get; set; }
        public string DepartureSecurityRemark { get; set; }
        [Required(ErrorMessage = "*")]
        public string ArrivalDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal EndKm { get; set; }
        public bool ArrivalVerifySTS { get; set; }
        public bool ArrivalVerifyStatus { get; set; }
        public string ArrivalSecurityId { get; set; }
        public string ArrivalSecurityRemark { get; set; }
        public List<BanasSecurityVehicleGatepassRetrieve_Result> GatePassList { get; set; }
        public List<BanasSecurityMasterRetrieve_Result> SecurityMasterList { get; set; }
    
    }
}
