using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.GatePass
{
    public class VisitCenterReportViewModel
    {
        public string VisitDateTime { get; set; }
        public string CloseDateTime { get; set; }
        [Required(ErrorMessage = "*")]
        public string DepartmentId { get; set; }
        public string Center { get; set; }

        [Required(ErrorMessage = "*")]
        public string EmployeeCode { get; set; }
        public List<BanasAuditorOperateVisitMasterRtr_Result> AuditorOperateVisitList { get; set; }
        public List<BanasCenterMasterRetrieve_Result> CenterList { get; set; }
        public List<BanasVisitCenterReportRtr_Result> VisitCenterReportList { get; set; }
        public List<BanasEmployeeMasterRtr_Result> EmployeeMasterList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentMasterList { get; set; }
    }
}
