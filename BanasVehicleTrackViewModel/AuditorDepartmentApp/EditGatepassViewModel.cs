using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.AuditorDepartmentApp
{
   public  class EditGatepassViewModel
    {
        public string GatePassId { get; set; }
        public string CloseOdometer { get; set; }
        public string Action { get; set; }
        public string NetKM { get; set; }
        public string StartOdometer { get; set; }
        public string Difference { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDate { get; set; }
    }
}
