using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.AuditorDepartment
{
    public class AuditorFinalApproveViewModel
    {

        public List<BanasAuditorFinalApproveRetrieve_Result> BanasVehicleGatepassRetrieveList { get; set; }
    }
    public class DifferenceData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
