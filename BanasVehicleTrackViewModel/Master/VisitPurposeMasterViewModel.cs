using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BanasVehicleTrackViewModel.Master
{
    public class VisitPurposeMasterViewModel
    {
        public string VisitId { get; set; }

        [Required(ErrorMessage = "*")]
        public string VisitPurpose { get; set; }
        public string CompanyCode { get; set; }
        public string IsActive { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasVisitPurposeMasterRtr_Result> VisitPurposeList { get; set; }
        public string Action { get; set; }
    }
}
