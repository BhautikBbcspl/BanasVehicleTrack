using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BanasVehicleTrack.Controllers
{
    public class UserManagementController : GeneralClass
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region ==> ModuleMaster
        public ActionResult ViewModuleMaster()
        {
            try
            {
                ModuleViewModel mv = new ModuleViewModel();
                mv.Action = "All";
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
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
                    mv.ModuleViewList = db.BanasModuleMasterRtr(mv.Action, mv.ModuleId).ToList();
                return View(mv);
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
        public ActionResult AddModuleMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                ModuleViewModel sb = new ModuleViewModel();
                sb.ModuleId = "0";
                sb.Action = "Save";
                ViewBag.action = "Add";
                return View(sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddModuleMaster(ModuleViewModel sm)
        {
            try
            {
                if (sm.ModuleId == "0")
                {
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.Action = "Save";
                    sm.CreateUser = User.Identity.Name;
                    string msg = db.BanasModuleMasterInsUpd(sm.ModuleId, sm.ModuleName.Trim(), sm.ModulePriority,sm.FaIcon, sm.IsActive, sm.CreateDate, sm.UpdateDate, sm.CreateUser, sm.UpdateUser, sm.Action).FirstOrDefault();
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                        return RedirectToAction("ViewModuleMaster", "UserManagement");

                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        ViewBag.action = "Add";
                        Danger(msg, true);
                        return View(sm);
                    }
                }
                else
                {
                    sm.Action = "Update";
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.CreateUser = User.Identity.Name;
                    sm.UpdateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.UpdateUser = User.Identity.Name;
                    string msg = db.BanasModuleMasterInsUpd(sm.ModuleId, sm.ModuleName.Trim(), sm.ModulePriority, sm.FaIcon, sm.IsActive, sm.CreateDate, sm.UpdateDate, sm.CreateUser, sm.UpdateUser, sm.Action).FirstOrDefault();
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                        return RedirectToAction("ViewModuleMaster", "UserManagement");

                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        ViewBag.action = "Update";
                        Danger(msg, true);
                        return View(sm);
                    }
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult EditModuleMaster(int ID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                ModuleViewModel sb = new ModuleViewModel();
                var mm = db.BanasModuleMasterRtr("GetModule", ID.ToString()).FirstOrDefault();
                sb.ModuleId = mm.ModuleId.ToString();
                sb.ModuleName = mm.ModuleName;
                sb.ModulePriority = mm.ModulePriority.ToString();
                sb.FaIcon = mm.FaIcon;
                sb.IsActive = mm.IsActive.ToString();
                ViewBag.action = "Update";
                sb.Action = "Update";
                return View("AddModuleMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region ==> PageMaster
        public ActionResult ViewPageMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                PageViewModel mv = new PageViewModel();
                mv.Action = "All";
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
                    mv.PageMasterList = db.BanasPageMasterRtr(mv.Action, mv.PageId).ToList();
                    return View(mv);
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
        public ActionResult AddPageMaster()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                PageViewModel sb = new PageViewModel();
                sb.PageId = "0";
                ViewBag.action = "Add";
                sb.Action = "Save";
                return View(sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddPageMaster(PageViewModel sm)
        {
            try
            {
                if (sm.PageId == "0")
                {
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.Action = "Save";
                    sm.CreateUser = User.Identity.Name;
                    string msg = db.BanasPageMasterInsUpd(sm.PageId, sm.PageName.Trim(), sm.PageUrl, sm.CreateDate, sm.CreateUser, sm.IsActive, sm.PagePriority, sm.UpdateDate, sm.UpdateUser, sm.Action).FirstOrDefault();
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                        return RedirectToAction("ViewPageMaster", "UserManagement");
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                        return View(sm);
                    }
                }
                else
                {
                    sm.Action = "Update";
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.CreateUser = User.Identity.Name;
                    sm.UpdateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.UpdateUser = User.Identity.Name;
                    string msg = db.BanasPageMasterInsUpd(sm.PageId, sm.PageName.Trim(), sm.PageUrl, sm.CreateDate, sm.CreateUser, sm.IsActive, sm.PagePriority, sm.UpdateDate, sm.UpdateUser, sm.Action).FirstOrDefault();
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                        return RedirectToAction("ViewPageMaster", "UserManagement");
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                        return View(sm);
                    }
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult EditPageMaster(int ID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                PageViewModel sb = new PageViewModel();
                var mm = db.BanasPageMasterRtr("GetPage", ID.ToString()).FirstOrDefault();
                sb.PageId = mm.PageId.ToString();
                sb.PageName = mm.PageName;
                sb.PageUrl = mm.PageUrl;
                sb.PagePriority = mm.PagePriority.ToString();
                sb.IsActive = mm.IsActive.ToString();
                ViewBag.action = "Update";
                sb.Action = "Update";
                return View("AddPageMaster", sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region==> Page Module Integration
        public ActionResult ViewIntePageModule()
        {
            try
            {
                PageModuleViewModel pmv = new PageModuleViewModel();
                pmv.Action = "all";
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
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
                    pmv.PageModuleInteList = db.BanasIntePageModuleRtr(pmv.Action, pmv.ModuleId).ToList();
                    return View(pmv);
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
        public ActionResult AddIntePageModule()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                PageModuleViewModel sb = new PageModuleViewModel();
                sb.Action = "insert";
                sb.ModulesList = db.BanasModuleMasterRtr("active", sb.ModuleId).ToList();
                sb.PagesList = db.BanasPageMasterRtr("All", sb.PageId).ToList();
                return View(sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddIntePageModule(PageModuleViewModel sm)
        {
            try
            {
               
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.Action = "insert";
                    sm.CreateUser = User.Identity.Name;
                    string msg = db.BanasIntePageModuleInsUpd(sm.InteId, sm.ModuleId, sm.PageId, sm.CreateDate, sm.CreateUser, sm.IsActive, sm.ModuleName, sm.PageName, sm.Action).FirstOrDefault();
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
              
                return RedirectToAction("ViewIntePageModule", "UserManagement");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region ==> Role Module Integration
        public ActionResult ViewInteRoleModule()
        {
            try
            {
                RoleModuleViewModel rmv = new RoleModuleViewModel();
                rmv.Action = "all";
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
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
                    rmv.RoleModuleInteList = db.BanasInteRoleModuleRtr().ToList();
                    return View(rmv);
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
        public ActionResult AddInteRoleModule()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                RoleModuleViewModel sb = new RoleModuleViewModel();
                sb.Action = "insert";
                sb.RoleList = db.BanasRoleMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                sb.ModuleList = db.BanasModuleMasterRtr("active", sb.ModuleId).ToList();
                sb.PageList = db.BanasPageMasterRtr("active", sb.PageId).ToList();
                return View(sb);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddInteRoleModule(RoleModuleViewModel sm)
        {
            try
            {
                if (sm.Action == "insert")
                {
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.CreateUser = User.Identity.Name;
                    string msg = db.BanasInteRoleModuleInsUpd(sm.InteId, sm.RoleId, sm.ModuleId, sm.IsActive, sm.CreateDate, sm.CreateUser, sm.Action, sm.MPInteId, sm.ViewRight == true ? "1" : "0", sm.InsertRight == true ? "1" : "0", sm.UpdateRight == true ? "1" : "0", sm.DeleteRight == true ? "1" : "0", sm.UpdateDate, sm.UpdateUser).FirstOrDefault();
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                        Success("Page to module integration saved successfully.", true);
                    }
                }
                else
                {
                    sm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.CreateUser = User.Identity.Name;
                    sm.UpdateDate = generalFunctions.getTimeZoneDatetimedb();
                    sm.UpdateUser = User.Identity.Name;
                    string msg = db.BanasInteRoleModuleInsUpd(sm.InteId, sm.RoleId, sm.ModuleId, sm.IsActive, sm.CreateDate, sm.CreateUser, sm.Action, sm.MPInteId, sm.ViewRight == true ? "1" : "0", sm.InsertRight == true ? "1" : "0", sm.UpdateRight == true ? "1" : "0", sm.DeleteRight == true ? "1" : "0", sm.UpdateDate, sm.UpdateUser).ToString();
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
                return RedirectToAction("ViewInteRoleModule", "UserManagement");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult SelectPageJson(int Moduleid)
        {
            try
            {
                var PageList = db.BanasIntePageModuleRtr("active", Moduleid.ToString()).Select(c => new PageViewModel() { PageId = c.InteId.ToString(), PageName = c.PageName }).ToList();
                var obj = new { PageList };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /*Test*/
    }
}