using BanasVehicleTrackViewModel.Master;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrack.GeneralClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace BanasVehicleTrack.Controllers
{
    public class MasterController : GeneralClass
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region==> Master

        #region==> Change Status
        public ActionResult ChangeStatus(string Code, string status, string Type)
        {
            string msg = "";
            try
            {
                if (Type == "DepartmentMaster")
                {
                    msg = db.BanasDepartmentMasterInsUpd(Code, "", "", LoggedUserDetails.CompanyCode, status, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "rights").FirstOrDefault();
                }
                else if (Type == "RoleMaster")
                {
                    msg = db.BanasRoleMasterInsUpd(Code, "", LoggedUserDetails.CompanyCode, status, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "rights").FirstOrDefault();
                }
                else if (Type == "EmployeeMaster")
                {
                    msg = db.BanasEmployeeMasterInsUpd(Code, "", "", "", "", "", "", "", status, LoggedUserDetails.CompanyCode, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "active").FirstOrDefault();
                }
                else if (Type == "SecurityMaster")
                {
                    msg = db.BanasSecurityMasterInsUpd(Code, "", "", LoggedUserDetails.CompanyCode, status, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "rights", "", "").FirstOrDefault();
                }
                else if (Type == "AuditorMaster")
                {
                    msg = db.BanasAuditorMasterInsUpd(Code, "", "", LoggedUserDetails.CompanyCode, status, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "rights", "", "").FirstOrDefault();
                }
                else if (Type == "ModuleMaster")
                {
                    msg = db.BanasModuleMasterInsUpd(Code, "", "", "", status, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "active").FirstOrDefault();
                }
                else if (Type == "PageMaster")
                {
                    msg = db.BanasPageMasterInsUpd(Code, "", "", "", "", status, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "active").FirstOrDefault();
                }
                else if (Type == "IntePageModule")
                {
                    msg = db.BanasIntePageModuleInsUpd(Code, "", "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, status, "", "", "active").FirstOrDefault();
                }
                else if (Type == "InteRoleModule")
                {
                    msg = db.BanasInteRoleModuleInsUpd(Code, "", "", status, "", "", "active", "", "", "", "", "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name).FirstOrDefault();
                }
                else if (Type == "VehicleMaster")
                {
                    msg = db.BanasVehicleMasterInsUpd(Code, "", "", "", "", "", "", "", "", "", "", "", "", "", LoggedUserDetails.CompanyCode, status, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "Active").FirstOrDefault();
                }
                else if (Type == "VisitPurposeMaster")
                {
                    msg = db.BanasVisitPurposeMasterInsUpd(Code, "", status, LoggedUserDetails.CompanyCode, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "Active").FirstOrDefault();
                }
                else
                {
                    msg = "Did not have method";
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region==> Department Master
        public ActionResult ViewDepartmentMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                DepartmentMasterViewModel model = new DepartmentMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("all", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddDepartmentMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                DepartmentMasterViewModel model = new DepartmentMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddDepartmentMaster(DepartmentMasterViewModel dm)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    string msg = db.BanasDepartmentMasterInsUpd(dm.DepartmentId, dm.DepartmentCode, dm.DepartmentName, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "insert").FirstOrDefault();
                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    string msg = db.BanasDepartmentMasterInsUpd(dm.DepartmentId, dm.DepartmentCode, dm.DepartmentName, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "update").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddDepartmentMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditDepartmentMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasDepartmentMasterRetrieve("all", LoggedUserDetails.CompanyCode).Where(c => c.DepartmentId == id).SingleOrDefault();
                DepartmentMasterViewModel sb = new DepartmentMasterViewModel();
                sb.DepartmentId = sm.DepartmentId.ToString();
                sb.DepartmentCode = sm.DepartmentCode;
                sb.DepartmentName = sm.DepartmentName;
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "update";
                ViewBag.action = "update";
                return View("AddDepartmentMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Role Master
        public ActionResult ViewRoleMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                RoleMasterViewModel model = new RoleMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.RoleMasterList = db.BanasRoleMasterRetrieve("all", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddRoleMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                RoleMasterViewModel model = new RoleMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddRoleMaster(RoleMasterViewModel dm)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    string msg = db.BanasRoleMasterInsUpd(dm.RoleId, dm.RoleName, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "insert").FirstOrDefault();
                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    string msg = db.BanasRoleMasterInsUpd(dm.RoleId, dm.RoleName, LoggedUserDetails.CompanyCode, dm.IsActive, "", generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "update").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddRoleMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditRoleMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasRoleMasterRetrieve("all", LoggedUserDetails.CompanyCode).Where(c => c.RoleId == id).SingleOrDefault();
                RoleMasterViewModel sb = new RoleMasterViewModel();
                sb.RoleId = sm.RoleId.ToString();
                sb.RoleName = sm.RoleName;
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "update";
                ViewBag.action = "update";
                return View("AddRoleMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Employee Master
        public ActionResult ViewEmployeeMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                EmployeeMasterViewModel model = new EmployeeMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.EmployeeMasterList = db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "all").ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddEmployeeMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                EmployeeMasterViewModel model = new EmployeeMasterViewModel();
                model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                model.RoleMasterList = db.BanasRoleMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddEmployeeMaster(EmployeeMasterViewModel dm)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasEmployeeMasterInsUpd(dm.EmployeeId, dm.EmployeeCode, dm.EmployeeName, dm.Contact, dm.Gender, dm.Password, dm.RoleId, dm.DepartmentId, dm.IsActive, LoggedUserDetails.CompanyCode, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "", "insert").FirstOrDefault();
                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasEmployeeMasterInsUpd(dm.EmployeeId, dm.EmployeeCode, dm.EmployeeName, dm.Contact, dm.Gender, dm.Password, dm.RoleId, dm.DepartmentId, dm.IsActive, LoggedUserDetails.CompanyCode, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "update").FirstOrDefault();


                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddEmployeeMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditEmployeeMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "all").Where(c => c.EmployeeId == id).SingleOrDefault();
                EmployeeMasterViewModel sb = new EmployeeMasterViewModel();
                sb.EmployeeMasterList = db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "all").ToList();
                sb.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                sb.RoleMasterList = db.BanasRoleMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                sb.EmployeeId = sm.EmployeeId.ToString();
                sb.EmployeeCode = sm.EmployeeCode;
                sb.EmployeeName = sm.EmployeeName;
                sb.Contact = sm.Contact;
                sb.Gender = sm.Gender;
                sb.Password = generalFunctions.Decrypt(sm.Password, true);
                sb.RoleId = sm.RoleId.ToString();
                sb.DepartmentId = sm.DepartmentId.ToString();
                sb.Contact = sm.Contact;
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "Update";
                ViewBag.action = "Update";
                return View("AddEmployeeMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        #endregion

        #region==> Vehicle Master
        public ActionResult ViewVehicleMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VehicleMasterViewModel model = new VehicleMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.VehicleMasterList = db.BanasVehicleMasterRtr("All", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddVehicleMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VehicleMasterViewModel model = new VehicleMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddVehicleMaster(VehicleMasterViewModel dm)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    string msg = db.BanasVehicleMasterInsUpd(dm.VehicleId, dm.VehicleCode, dm.VehicleRegNumber, generalFunctions.dateconvert(dm.VehicleRegDate), dm.VehicleType, dm.VehicleMake, dm.Model, dm.Status, dm.RatePerKm, generalFunctions.dateconvert(dm.RateEffectiveDate), generalFunctions.dateconvert(dm.InsuranceCommencementDate), generalFunctions.dateconvert(dm.InsuranceExpiryDate), generalFunctions.dateconvert(dm.ContractStartDate), generalFunctions.dateconvert(dm.ContractEndDate), LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "", "Insert").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    string msg = db.BanasVehicleMasterInsUpd(dm.VehicleId, dm.VehicleCode, dm.VehicleRegNumber, generalFunctions.dateconvert(dm.VehicleRegDate), dm.VehicleType, dm.VehicleMake, dm.Model, dm.Status, dm.RatePerKm, generalFunctions.dateconvert(dm.RateEffectiveDate), generalFunctions.dateconvert(dm.InsuranceCommencementDate), generalFunctions.dateconvert(dm.InsuranceExpiryDate), generalFunctions.dateconvert(dm.ContractStartDate), generalFunctions.dateconvert(dm.ContractEndDate), LoggedUserDetails.CompanyCode, dm.IsActive, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "Update").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddVehicleMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditVehicleMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasVehicleMasterRtr("All", LoggedUserDetails.CompanyCode).Where(c => c.VehicleId == id).SingleOrDefault();
                VehicleMasterViewModel sb = new VehicleMasterViewModel();
                sb.VehicleId = sm.VehicleId.ToString();
                sb.VehicleCode = sm.VehicleCode;
                sb.VehicleRegNumber = sm.VehicleRegNumber;
                sb.VehicleRegDate = sm.VehRegdate.ToString();
                sb.VehicleType = sm.VehicleType;
                sb.VehicleMake = sm.VehicleMake;
                sb.Model = sm.Model;
                sb.Status = sm.Status;
                sb.RatePerKm = sm.RatePerKm.ToString();
                sb.RateEffectiveDate = sm.RateEffeDate.ToString();
                sb.InsuranceCommencementDate = sm.InsComDate.ToString();
                sb.InsuranceExpiryDate = sm.InsExpDate.ToString();
                sb.ContractStartDate = sm.ConStartDate.ToString();
                sb.ContractEndDate = sm.ConEndDate.ToString();
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "Update";
                ViewBag.action = "Update";
                return View("AddVehicleMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Visit Purpose Master
        public ActionResult ViewVisitPurposeMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VisitPurposeMasterViewModel model = new VisitPurposeMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.VisitPurposeList = db.BanasVisitPurposeMasterRtr("All", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddVisitPurposeMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VisitPurposeMasterViewModel model = new VisitPurposeMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddVisitPurposeMaster(VisitPurposeMasterViewModel dm)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    string msg = db.BanasVisitPurposeMasterInsUpd(dm.VisitId, dm.VisitPurpose, dm.IsActive, LoggedUserDetails.CompanyCode, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "", "Insert").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    string msg = db.BanasVisitPurposeMasterInsUpd(dm.VisitId, dm.VisitPurpose, dm.IsActive, LoggedUserDetails.CompanyCode, "", generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "Update").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddVisitPurposeMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditVisitPurposeMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasVisitPurposeMasterRtr("All", LoggedUserDetails.CompanyCode).Where(c => c.VisitId == id).SingleOrDefault();
                VisitPurposeMasterViewModel sb = new VisitPurposeMasterViewModel();
                sb.VisitId = sm.VisitId.ToString();
                sb.VisitPurpose = sm.VisitPurpose;
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "Update";
                ViewBag.action = "Update";
                return View("AddVisitPurposeMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Gate Pass Security Master
        public ActionResult ViewSecurityMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                SecurityMasterViewModel model = new SecurityMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.SecurityMasterList = db.BanasSecurityMasterRetrieve("all", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddSecurityMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                SecurityMasterViewModel model = new SecurityMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        //[HttpPost]
        //public ActionResult AddSecurityMaster(SecurityMasterViewModel dm, HttpPostedFileBase SignImage)
        //{

        //    try
        //    {

        //        if (dm.Action == "Save")
        //        {
        //            if (SignImage != null)
        //            {
        //                string path = Server.MapPath("~/Uploads/SecuritySign_Img/");
        //                string ImagePath = "Uploads/SecuritySign_Img/";
        //                string extension = Path.GetExtension(SignImage.FileName);

        //                var result = string.Concat(dm.SecurityCode, extension);

        //                SignImage.SaveAs(path + Path.GetFileName(result));
        //                dm.SecuritySignature = string.Concat(ImagePath, result);
        //            }

        //            //int result2 = dm.IsActive.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        //            dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
        //            string msg = db.BanasSecurityMasterInsUpd(dm.SecurityId, dm.SecurityName, dm.SecuritySignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "insert", dm.SecurityCode, dm.Password).FirstOrDefault();
        //            ViewBag.Message = msg;
        //            if (msg.Contains("successfully"))
        //            {
        //                ViewBag.Message = msg.ToUpper();
        //                Success(msg, true);
        //            }
        //            else
        //            {
        //                ViewBag.Message = msg.ToUpper();
        //                Danger(msg, true);
        //            }
        //        }
        //        else
        //        {
        //            if (SignImage != null)
        //            {
        //                string path = Server.MapPath("~/Uploads/SecuritySign_Img/");
        //                string ImagePath = "Uploads/SecuritySign_Img/";
        //                string extension = Path.GetExtension(SignImage.FileName);

        //                var result = string.Concat(dm.SecurityCode, extension);

        //                SignImage.SaveAs(path + Path.GetFileName(result));
        //                dm.SecuritySignature = string.Concat(ImagePath, result);

        //            }
        //            dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
        //            string msg = db.BanasSecurityMasterInsUpd(dm.SecurityId, dm.SecurityName, dm.SecuritySignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "update", dm.SecurityCode, dm.Password).FirstOrDefault();

        //            ViewBag.Message = msg;
        //            if (msg.Contains("successfully"))
        //            {
        //                ViewBag.Message = msg.ToUpper();
        //                Success(msg, true);
        //            }
        //            else
        //            {
        //                ViewBag.Message = msg.ToUpper();
        //                Danger(msg, true);
        //            }
        //        }

        //        return RedirectToAction("AddSecurityMaster");


        //    }
        //    catch (Exception ex)
        //    {
        //        Danger(ex.Message.ToString(), true);
        //        return RedirectToAction("Dashboard", "Home");
        //    }

        //}
        [HttpPost]
        public ActionResult AddSecurityMaster(SecurityMasterViewModel dm, HttpPostedFileBase SecuritySignature, HttpPostedFileBase EditSignature)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    if (SecuritySignature != null)
                    {
                        string path = Server.MapPath("~/Uploads/SecuritySign_Img/");
                        string ImagePath = "Uploads/SecuritySign_Img/";
                        string extension = Path.GetExtension(SecuritySignature.FileName);

                        var result = string.Concat(dm.SecurityCode, extension);

                        SecuritySignature.SaveAs(path + Path.GetFileName(result));
                        dm.SecuritySignature = string.Concat(ImagePath, result);
                    }

                    //int result2 = dm.IsActive.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasSecurityMasterInsUpd(dm.SecurityId, dm.SecurityName, dm.SecuritySignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "insert", dm.SecurityCode, dm.Password).FirstOrDefault();
                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    if (EditSignature != null)
                    {
                        string path = Server.MapPath("~/Uploads/SecuritySign_Img/");
                        string ImagePath = "Uploads/SecuritySign_Img/";
                        string extension = Path.GetExtension(EditSignature.FileName);

                        var result = string.Concat(dm.SecurityCode, extension);

                        EditSignature.SaveAs(path + Path.GetFileName(result));
                        dm.SecuritySignature = string.Concat(ImagePath, result);
                    }
                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasSecurityMasterInsUpd(dm.SecurityId, dm.SecurityName, dm.SecuritySignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "update", dm.SecurityCode, dm.Password).FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddSecurityMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditSecurityMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasSecurityMasterRetrieve("all", LoggedUserDetails.CompanyCode).Where(c => c.SecurityId == id).SingleOrDefault();
                SecurityMasterViewModel sb = new SecurityMasterViewModel();
                sb.SecurityId = sm.SecurityId.ToString();
                sb.SecurityName = sm.SecurityName;

                string ImagePath = sm.SecuritySignature;
                string folderPath = "Uploads/SecuritySign_Img/";

                sb.SignatureName = ImagePath.Substring(folderPath.Length);
                sb.SecuritySignature = ImagePath;
                sb.SecurityCode = sm.SecurityCode;
                sb.Password = generalFunctions.Decrypt(sm.Password, true);
                sb.IsActive = sm.IsActive.ToString();

                sb.Action = "update";
                ViewBag.action = "update";
                return View("AddSecurityMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Gate Pass Auditor Master
        public ActionResult ViewAuditorMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                AuditorMasterViewModel model = new AuditorMasterViewModel();
                string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);
                var re = db.BanasMenuRightsRtr(LoggedUserDetails.RoleId, url).FirstOrDefault();
                if (re != null)
                {
                    ViewBag.ViewRight = re.ViewRight;
                    ViewBag.InsertRight = re.InsertRight;
                    ViewBag.UpdateRight = re.UpdateRight;
                    ViewBag.DeleteRight = re.DeleteRight;
                }
                if (ViewBag.ViewRight == 1)
                {
                    model.AuditorMasterList = db.BanasAuditorMasterRetrieve("all", LoggedUserDetails.CompanyCode).ToList();
                    model.Action = "Save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult AddAuditorMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                AuditorMasterViewModel model = new AuditorMasterViewModel();
                model.Action = "Save";
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddAuditorMaster(AuditorMasterViewModel dm, HttpPostedFileBase AuditorSignature, HttpPostedFileBase EditSignature)
        {
            try
            {
                if (dm.Action == "Save")
                {
                    if (AuditorSignature != null)
                    {
                        string path = Server.MapPath("~/Uploads/AuditorSign_Img/");
                        string ImagePath = "Uploads/AuditorSign_Img/";
                        string extension = Path.GetExtension(AuditorSignature.FileName);

                        var result = string.Concat(dm.AuditorCode, extension);

                        AuditorSignature.SaveAs(path + Path.GetFileName(result));
                        dm.AuditorSignature = string.Concat(ImagePath, result);
                    }

                    //int result2 = dm.IsActive.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasAuditorMasterInsUpd(dm.AuditorId, dm.AuditorName, dm.AuditorSignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "insert", dm.AuditorCode, dm.Password).FirstOrDefault();
                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                else
                {
                    if (EditSignature != null)
                    {
                        string path = Server.MapPath("~/Uploads/AuditorSign_Img/");
                        string ImagePath = "Uploads/AuditorSign_Img/";
                        string extension = Path.GetExtension(EditSignature.FileName);

                        var result = string.Concat(dm.AuditorCode, extension);

                        EditSignature.SaveAs(path + Path.GetFileName(result));
                        dm.AuditorSignature = string.Concat(ImagePath, result);
                    }
                    dm.Password = generalFunctions.Encrypt(dm.Password, true).ToString().Trim();
                    string msg = db.BanasAuditorMasterInsUpd(dm.AuditorId, dm.AuditorName, dm.AuditorSignature, LoggedUserDetails.CompanyCode, dm.IsActive, generalFunctions.getTimeZoneDatetimedb(), "", User.Identity.Name, "update", dm.AuditorCode, dm.Password).FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                    }
                }
                return RedirectToAction("AddAuditorMaster");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditAuditorMaster(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                var sm = db.BanasAuditorMasterRetrieve("all", LoggedUserDetails.CompanyCode).Where(c => c.AuditorId == id).SingleOrDefault();
                AuditorMasterViewModel sb = new AuditorMasterViewModel();
                sb.AuditorId = sm.AuditorId.ToString();
                sb.AuditorName = sm.AuditorName;

                string ImagePath = sm.AuditorSignature;
                string folderPath = "Uploads/AuditorSign_Img/";

                sb.SignatureName = ImagePath.Substring(folderPath.Length);
                sb.AuditorSignature = ImagePath;


                sb.AuditorCode = sm.AuditorCode;
                sb.Password = generalFunctions.Decrypt(sm.Password, true);
                sb.IsActive = sm.IsActive.ToString();
                sb.Action = "update";
                ViewBag.action = "update";
                return View("AddAuditorMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #endregion
    }
}