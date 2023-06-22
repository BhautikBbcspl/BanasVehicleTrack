using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class RoleMasterViewModel
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "*")]
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasRoleMasterRetrieve_Result> RoleMasterList { get; set; }
        public string Action { get; set; }
        public string PageName { get; set; }
    }
}
