using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.Master
{
    public class AuditorMasterViewModel
    {
        public string AuditorId { get; set; }
        [Required(ErrorMessage = "*")]
        public string AuditorName { get; set; }
        [Required(ErrorMessage = "*")]
        public string AuditorSignature { get; set; }
        public string SignatureName { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string IsActive { get; set; }
        //public List<BanasAuditorMasterRetrieve_Result> AuditorMasterList { get; set; }
        public string Action { get; set; }
        [Required(ErrorMessage = "*")]
        public string AuditorCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }
}
