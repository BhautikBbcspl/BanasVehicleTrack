using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class EmailTemplateMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public string Path { get; set; }
        public string To { get; set; }
        public string CC { get; set; }

        public string BCC { get; set; }
        public string SubjectName { get; set; }
        public List<BanasEmailTemplateMasterRtr_Result> EmailTemplateList { get; set; }
    }
}
