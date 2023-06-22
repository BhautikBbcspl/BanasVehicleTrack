using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class DepartmentMasterViewModel
    {
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentName { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string IsActive { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
        public string Action { get; set; }
    }
}
