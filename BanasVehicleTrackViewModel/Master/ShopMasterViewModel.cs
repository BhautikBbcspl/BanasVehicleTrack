using BanasVehicleTrackDbClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanasVehicleTrackViewModel.Master
{
    public class ShopMasterViewModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ShopMasterId { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessage = "*")]
        public string ShopCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string ShopName { get; set; }
        [Required(ErrorMessage = "*")]
        public string ShopContactNo { get; set; }
        public string CompanyCode { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string IsActive { get; set; }
        public string Action { get; set; }
        public List<BanasShopMasterRetrieve_Result> ShopList { get; set; }
        public List<BanasDepartmentMasterRetrieve_Result> DepartmentList { get; set; }
    }
}
