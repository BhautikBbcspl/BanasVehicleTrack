using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class ModuleViewModel
    {
        public string Action { get; set; }
        public string ModuleId { get; set; }
        [StringLength(50, ErrorMessage = "The Module Name cannot exceed 50 characters.")]
        [Required(ErrorMessage = "*")]
        public string ModuleName { get; set; }
        public string FaIcon { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only Greater > 0 number allowed.")]
        [Required(ErrorMessage = "*")]
        public string ModulePriority { get; set; }
        public string IsActive { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public bool SelfPage { get; set; }

        public string UpdateUser { get; set; }
        public List<BanasModuleMasterRtr_Result> ModuleViewList { get; set; }
    }
}
