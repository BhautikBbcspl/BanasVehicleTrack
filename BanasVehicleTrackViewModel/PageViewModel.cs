using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class PageViewModel
    {
        public string Action { get; set; }
        public string PageId { get; set; }
        [Required(ErrorMessage = "*")]
        public string PageName { get; set; }
     
        [Required(ErrorMessage = "*")]
        public string PageUrl { get; set; }
        [Required(ErrorMessage = "*")]
        public string PagePriority { get; set; }
        public string IsActive { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<BanasPageMasterRtr_Result> PageMasterList { get; set; }
    }
}
