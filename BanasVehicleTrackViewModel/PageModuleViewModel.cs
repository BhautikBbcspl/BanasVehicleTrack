using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class PageModuleViewModel
    {
        public string InteId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ModuleId { get; set; }
        [Required(ErrorMessage = "*")]
        public string PageId { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string IsActive { get; set; }
        public string ModuleName { get; set; }
        public string PageName { get; set; }
        public string Action { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasModuleMasterRtr_Result> ModulesList { get; set; }
        public List<BanasPageMasterRtr_Result> PagesList { get; set; }
        public List<BanasIntePageModuleRtr_Result> PageModuleInteList { get; set; }
    }
}
