using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class CenterMasterViewModel
    {
        public string CenterId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }

        [Required(ErrorMessage = "*")]
        public string CenterCode { get; set; }

        [Required(ErrorMessage = "*")]
        public string CenterName { get; set; }

        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string IsActive { get; set; }
        public string Action { get; set; }
        public List<BanasCenterMasterRetrieve_Result> CenterList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentList { get; set; }
    }
}
