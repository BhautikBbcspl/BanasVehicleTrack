using System;
using BanasVehicleTrackDbClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class RoleModuleViewModel
    {
        public string InteId { get; set; }

        [Required(ErrorMessage = "*")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "*")]
        public string ModuleId { get; set; }
        public string IsActive { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string Action { get; set; }

        [Required(ErrorMessage = "*")]
        public string MPInteId { get; set; }
        public string PageId { get; set; }
        public bool ViewRight { get; set; }
        public bool InsertRight { get; set; }
        public bool UpdateRight { get; set; }
        public bool DeleteRight { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasRoleMasterRetrieve_Result> RoleList { get; set; }
        public List<BanasModuleMasterRtr_Result> ModuleList { get; set; }
        public List<BanasPageMasterRtr_Result> PageList { get; set; }
        public List<BanasInteRoleModuleRtr_Result> RoleModuleInteList { get; set; }
    }
}
