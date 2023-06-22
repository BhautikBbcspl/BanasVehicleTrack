using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel
{
    public class ChangePasswordViewModel
    {
        public string EmployeeCode { get; set; }
        public string CompanyCode { get; set; }

        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Required")]
 
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("NewPassword",ErrorMessage ="Confirm Password not matched")]
        public string ConfirmPassword { get; set; }
        public string CreateDate { get; set; }
       
    }
}
