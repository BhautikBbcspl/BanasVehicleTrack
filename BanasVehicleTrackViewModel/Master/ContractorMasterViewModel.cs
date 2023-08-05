using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class ContractorMasterViewModel
    {
        public string ContractorId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractorCompanyName { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractorName { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractorCode { get; set; }
        public string Contactno { get; set; }
        public string IsActive { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasContractorMasterRetrieve_Result> ConMasterList { get; set; }
        public string Action { get; set; }
        public string PageName { get; set; }
    }
}
