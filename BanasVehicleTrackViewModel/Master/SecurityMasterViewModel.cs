using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.Master
{
    public class SecurityMasterViewModel
    {
        public string SecurityId { get; set; }

        [Required(ErrorMessage = "*")]
        public string SecurityName { get; set; }

        [Required(ErrorMessage = "*")]
        public string SecuritySignature { get; set; }
        public string SignatureName { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string IsActive { get; set; }
        public List<BanasSecurityMasterRetrieve_Result> SecurityMasterList { get; set; }
        public string Action { get; set; }
        [Required(ErrorMessage = "*")]
        public string SecurityCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }
}
