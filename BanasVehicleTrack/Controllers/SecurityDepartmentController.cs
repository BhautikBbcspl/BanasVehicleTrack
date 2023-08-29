using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel.SecurityDepartment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanasVehicleTrack.Controllers
{
    public class SecurityDepartmentController : GeneralClass
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region==> Security Department Visit Management

        //Security DASHBOARD
        public ActionResult SecurityDashboard()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                SecurityVisitMgmtViewModel model = new SecurityVisitMgmtViewModel();
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
                    model.Action = "Save";
                    model.GatePassList = db.BanasSecurityVehicleGatepassRetrieve("all", "").ToList();
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
        //
        public ActionResult SecurityVisitMgmtDashboard()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                SecurityVisitMgmtViewModel model = new SecurityVisitMgmtViewModel();
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
                    model.Action = "Save";
                    model.GatePassList = db.BanasSecurityVehicleGatepassRetrieve("all", "").ToList();
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

        public ActionResult ViewSecurityGatePass(string id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                SecurityVisitMgmtViewModel model = new SecurityVisitMgmtViewModel();
                model.Action = "details";
                model.GatePassId = id;
                model.GatePassList = db.BanasSecurityVehicleGatepassRetrieve(model.Action, id).ToList();

                model.DepartureVerifyStatus = Convert.ToBoolean(model.GatePassList.FirstOrDefault().DepartureVerifyStatus);
                model.ArrivalVerifyStatus = Convert.ToBoolean(model.GatePassList.FirstOrDefault().ArrivalVerifyStatus);
                model.SecurityMasterList = db.BanasSecurityMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddSecurityGatePass(SecurityVisitMgmtViewModel dm)
        {
            try
            {
                if (dm.DepartureVerifyStatus == true && dm.ArrivalVerifyStatus == false)
                {
                    DateTime DepartureDateTime = DateTime.ParseExact(dm.DepartureDateTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, "", "", "","", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "DepartureInsert", DepartureDateTime, dm.StartKm, dm.DepartureVerifyStatus ? "1" : "0", dm.DepartureSecurityId, dm.DepartureSecurityRemark, null, null, "", "", "").FirstOrDefault();
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
                else if (dm.DepartureVerifyStatus == true && dm.ArrivalVerifyStatus == true)
                {
                    DateTime ArrivalDateTime = DateTime.ParseExact(dm.ArrivalDateTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "ArrivalInsert", null, null, "", "", "", ArrivalDateTime, dm.EndKm, dm.ArrivalVerifyStatus ? "1" : "0", dm.ArrivalSecurityId, dm.ArrivalSecurityRemark).FirstOrDefault();

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
                return RedirectToAction("SecurityVisitMgmtDashboard");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult SecuritySignatureJson(int SecurityId)
        {
            try
            {
                var selectedSecurity = db.BanasSecurityMasterRetrieve("active", LoggedUserDetails.CompanyCode).FirstOrDefault(s => s.SecurityId == SecurityId);
                if (selectedSecurity != null)
                {
                    var signatureUrl = selectedSecurity.SecuritySignature;

                    return Json(new { signatureUrl }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { signatureUrl = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
//test