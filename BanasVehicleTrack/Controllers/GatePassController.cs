using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using BanasVehicleTrackViewModel.AuditorDepartmentApp;
using BanasVehicleTrackViewModel.GatePass;
using BanasVehicleTrackViewModel.Master;
using ClosedXML.Excel;

namespace BanasVehicleTrack.Controllers
{
    public class GatePassController : GeneralClass
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();
        public ActionResult ViewGatepass()
        {
            string filterValue = Request.QueryString["filterValue"];

            string url = generalFunctions.getCommon(Request.Url.AbsoluteUri);

            int questionMarkIndex = url.IndexOf('?');
            if (questionMarkIndex >= 0)
            {
                url = url.Substring(0, questionMarkIndex);

            }
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
                if (filterValue != null)
                {
                    var re1 = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "", LoggedUserDetails.EmployeeCode).Where(x => x.GatePassStatus == Convert.ToInt32(filterValue));
                    return View(re1);
                }
                else
                {
                    var re1 = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "", LoggedUserDetails.EmployeeCode);
                    return View(re1);
                }
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult ViewStartGatePass(string id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                ViewStartGatePassViewModel model = new ViewStartGatePassViewModel();
                model.Action = "details";
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id, LoggedUserDetails.EmployeeCode).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult ViewTrip(string id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                ViewStartGatePassViewModel model = new ViewStartGatePassViewModel();
                model.Action = "details";
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id, LoggedUserDetails.EmployeeCode).ToList();


                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        #region ==> Close GatePass
        public ActionResult ClosedGatePass(string id, string type)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                ViewBag.Type = type;
                ViewStartGatePassViewModel model = new ViewStartGatePassViewModel();
                model.Action = "details";
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id, LoggedUserDetails.EmployeeCode).ToList();
                model.StartOdometer = model.BanasVehicleGatepassRetrieveList.FirstOrDefault().StartOdometer.ToString();

                model.GatePassId = id;

                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }

        [HttpPost]
        public ActionResult ClosedGatePass(ViewStartGatePassViewModel vsgp)
        {
            try

            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }

                HttpCookie reqCookies = Request.Cookies["EmployeeMaster"];
                vsgp.Action = "close";

                string closeDateTime = vsgp.CloseDateTime != null ? generalFunctions.dateconvert(vsgp.CloseDateTime) : null;

                vsgp.CloseDate = generalFunctions.getDate();
                vsgp.CloseUser = reqCookies["EmployeeCode"].ToString();
                //vsgp.GatePassStatus = "0";
                string msg = db.BanasVehicleGatepassInsUpd(vsgp.GatePassId, vsgp.DepartmentId, vsgp.UserCode, vsgp.OtherUser1, vsgp.OtherUser2, vsgp.OtherUser3, vsgp.InformationMode, vsgp.VisitDateTime, vsgp.VisitPurpose, vsgp.Remarks, vsgp.VehicleDepartment, vsgp.Driver, vsgp.VehicleId, vsgp.StartOdometer, vsgp.CloseOdometer, closeDateTime, vsgp.CloseRemark, vsgp.Netkm, vsgp.CreateUser, vsgp.CreateDate, vsgp.CloseUser, vsgp.CloseDate, vsgp.EditUser, vsgp.EditDate, vsgp.Difference, vsgp.Action, null, null, "", "", "", null, null, "", "", "").FirstOrDefault();

                if (msg == "VehicleGatepass " + vsgp.GatePassId + " closed successfully")
                {
                    ViewBag.Message = msg.ToUpper();
                    Success(msg, true);
                    return RedirectToAction("ViewGatepass", "GatePass");
                }
                else
                {
                    ViewBag.Message = msg.ToUpper();
                    Danger(msg, true);
                    return RedirectToAction("ViewGatepass", "GatePass");
                }
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        public ActionResult AddGatepass()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HttpCookie reqCookies = Request.Cookies["EmployeeMaster"];
                if (reqCookies == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                GatepassViewModel gvm = new GatepassViewModel();
                gvm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                gvm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
                //gvm.VehicleList = db.BanasVehicleMasterRtr("active", "BDL01").ToList();
                gvm.VehicleList = db.BanasVehicleMasterRtr("open", LoggedUserDetails.CompanyCode).ToList();

                //gvm.AuditorList = db.BanasEmployeeMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
                gvm.VisitDateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                gvm.StartOdometer = db.BanasFetchLastCosingOdometer(gvm.VehicleId).SingleOrDefault().ToString();

                gvm.EmployeeList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                .ToList();

                gvm.EmployeeList.ForEach(e =>
                {
                    e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                });
                gvm.DepartmentId = LoggedUserDetails.DepartmentId;
                gvm.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
          ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();
                //gvm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                //gvm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
                //gvm.VehicleList = db.BanasVehicleMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
                //gvm.EmployeeList = db.BanasEmployeeMasterRtr( LoggedUserDetails.CompanyCode, "Active").ToList();
                gvm.action = "Add";
                return View(gvm);
            }
        }
        [HttpPost]
        public ActionResult AddGatepass(GatepassViewModel dm)
        {
            try
            {
                if (dm.action == "Add")
                {
                    //dm.GatePassId = DateTime.Now.ToString("yyyyMMddHHmmss");
                    dm.CreateUser = User.Identity.Name;
                    dm.CreateDate = generalFunctions.getTimeZoneDatetimedb();
                    string visitdate = generalFunctions.dateconvert(dm.VisitDateTime.Substring(0, 10));
                    string visittime = dm.VisitDateTime.Substring(11, 5);
                    dm.VisitDateTime = visitdate + " " + visittime;

                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, dm.DepartmentId, dm.UserCode, dm.OtherUser1, dm.OtherUser2, dm.OtherUser3, dm.InformationMode, dm.VisitDateTime, dm.VisitPurpose, dm.Remarks, dm.Center, dm.Driver, dm.VehicleId, dm.StartOdometer, dm.CloseOdometer, dm.CloseDateTime, dm.CloseRemark, dm.Netkm, dm.CreateUser, dm.CreateDate, dm.CloseUser, dm.CloseDate, dm.EditUser, dm.EditDate, dm.difference, "open", null, null, null, null, null, null, null, null, null, null).FirstOrDefault();

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
                    dm.EditUser = User.Identity.Name;
                    dm.EditDate = generalFunctions.getTimeZoneDatetimedb();
                    string visitdate = generalFunctions.dateconvert(dm.VisitDateTime.Substring(0, 10));
                    string visittime = dm.VisitDateTime.Substring(11, 5);
                    dm.VisitDateTime = visitdate + " " + visittime;
                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, dm.DepartmentId, dm.UserCode, dm.OtherUser1, dm.OtherUser2, dm.OtherUser3, dm.InformationMode, dm.VisitDateTime, dm.VisitPurpose, dm.Remarks, dm.Center, dm.Driver, dm.VehicleId, dm.StartOdometer, dm.CloseOdometer, dm.CloseDateTime, dm.CloseRemark, dm.Netkm, dm.CreateUser, dm.CreateDate, dm.CloseUser, dm.CloseDate, dm.EditUser, dm.EditDate, dm.difference, "edit", null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
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
                return RedirectToAction("ViewGatepass");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EditGatepass(string id)
        {
            try
            {
                //if (!User.Identity.IsAuthenticated)
                //{
                //    FormsAuthentication.RedirectToLoginPage();
                //}
                GatepassViewModel dm = new GatepassViewModel();
                var mm = db.BanasVehicleGatepassRetrieve("details", null, null, "", "", id, LoggedUserDetails.EmployeeCode).FirstOrDefault();
                dm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                dm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
                dm.VehicleList = db.BanasVehicleMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();

                dm.EmployeeList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                .ToList();

                dm.EmployeeList.ForEach(e =>
                {
                    e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                });
                dm.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
          ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

                dm.action = "Edit";
                dm.GatePassId = id;
                dm.DepartmentId = LoggedUserDetails.DepartmentId;
                dm.Center = mm.CenterId.ToString();
                dm.UserCode = mm.UserCode;
                dm.OtherUser1 = mm.OtherUser1;
                dm.OtherUser2 = mm.OtherUser2;
                dm.OtherUser3 = mm.OtherUser3;
                dm.InformationMode = mm.InformationMode;
                dm.VisitDateTime = mm.VisitDateTime;
                dm.VisitPurpose = mm.VisitPurposeId.ToString();
                dm.Remarks = mm.Remarks;
                dm.Driver = mm.Driver;
                dm.VehicleId = mm.VehicleId.ToString();
                dm.StartOdometer = decimal.Parse(mm.StartOdometer.ToString()).ToString("#.0");
                return View("AddGatepass", dm);
            }
            catch (Exception)
            {
                //Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult SelectDepartmentWiseUser_CenterJson(int ddlDepartmentId)
        {
            var CenterList = db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId == ddlDepartmentId).Select(c => new CenterMasterViewModel() { CenterId = c.CenterId.ToString(), CenterName = c.CenterName }).ToList();

            var EmployeeList = db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                       .Where(x => x.DepartmentId == ddlDepartmentId).Select(c => new EmployeeMasterViewModel() { EmployeeCode = c.EmployeeCode, EmployeeName = c.EmployeeName }).ToList();

            EmployeeList.ForEach(e =>
            {
                e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
            });

            var obj = new { CenterList, EmployeeList };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCenterWiseVehicleJson(int ddlCenter)
        {
            var VehicleList = db.BanasVehicleMasterRtr("open", LoggedUserDetails.CompanyCode).Where(x => x.CenterId == ddlCenter).Select(c => new VehicleMasterViewModel() { VehicleId = c.VehicleId.ToString(), VehicleRegNumber = c.VehicleRegNumber }).ToList();

            var obj = new { VehicleList };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOdometer(string Vehicleid)
        {
            try
            {
                string value = db.BanasFetchLastCosingOdometer(Vehicleid).SingleOrDefault().ToString();
                string odometer = decimal.Parse(value).ToString("#.0");
                var obj = new { odometer };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region==> Visit Management (Gate Pass Report)  
        public ActionResult VisitMgmt()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VisitManagementViewModel model = new VisitManagementViewModel();
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
                    model.VisitDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    model.CloseDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    //model.Action = "Save";
                    //model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.BanasVisitManagementRtr_Results = db.BanasVisitManagementRtr("all", generalFunctions.dateconvert(model.VisitDateTime), generalFunctions.dateconvert(model.CloseDateTime), model.DepartmentId, model.VehicleId, model.Center, model.UserCode, LoggedUserDetails.EmployeeCode).ToList();

                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();

                    model.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
             ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

                    model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();

                    //model.EmployeeList = (LoggedUserDetails.EmployeeCode == "Admin") ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList() : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).Select(x=>x.EmployeeName=String.Format("{0} {1}",x.EmployeeCode,x.EmployeeName)).ToList();
                    model.EmployeeList = (LoggedUserDetails.EmployeeCode == "Admin")
                    ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                    : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                        .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                        .ToList();

                    model.EmployeeList.ForEach(e =>
                    {
                        e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                    });

                    model.DepartmentId = LoggedUserDetails.DepartmentId;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult VisitMgmt(VisitManagementViewModel model)
        {

            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "", LoggedUserDetails.EmployeeCode).ToList();
                //model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();

                // Check if VisitDateTime is null before converting
                string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

                // Check if CloseDateTime is null before converting
                string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

                model.BanasVisitManagementRtr_Results = db.BanasVisitManagementRtr("all", generalFunctions.dateconvert(model.VisitDateTime), generalFunctions.dateconvert(model.CloseDateTime), model.DepartmentId, model.VehicleId, model.Center, model.UserCode, LoggedUserDetails.EmployeeCode).ToList();


                model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                model.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

                model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();

                model.EmployeeList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                .ToList();

                model.EmployeeList.ForEach(e =>
                {
                    e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                });

                model.DepartmentId = LoggedUserDetails.DepartmentId;
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }

        }
        public ActionResult ExportReport(string DepartmentId, string VehicleId, string VisitDateTime, string CloseDateTime, string GatePassId, string UserCode)
        {

            string visitDateTime = VisitDateTime != "" ? generalFunctions.dateconvert(VisitDateTime) : null;

            string closeDateTime = CloseDateTime != "" ? generalFunctions.dateconvert(CloseDateTime) : null;

            var re1 = db.BanasVisitManagementRtr("all", visitDateTime, closeDateTime, string.IsNullOrEmpty(DepartmentId) ? null : DepartmentId, string.IsNullOrEmpty(VehicleId) ? null : VehicleId, string.IsNullOrEmpty(GatePassId) ? null : GatePassId, string.IsNullOrEmpty(UserCode) ? null : UserCode, LoggedUserDetails.EmployeeCode).ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("GatePassReport");

                // Add title
                string title = "Gate Pass Report";

                string LogoPath = Server.MapPath("~/Content/AdminTemplateAssets/images/LogoForExcel.png");

                Image image = null;
                try
                {
                    image = Image.FromFile(LogoPath);
                }
                catch (FileNotFoundException)
                {
                    // Handle the case when the image file doesn't exist
                    Console.WriteLine("Image file not found: " + LogoPath);
                }

                if (image != null)
                {
                    // Your existing code to process the image
                    using (image)
                    {
                        string tempImagePath = Path.GetTempFileName();
                        image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);


                        var picture = worksheet.AddPicture(tempImagePath)
                            .MoveTo(worksheet.Range("A1:B1").Merge().FirstCell().Address, 5, 5)
                            .WithSize(350, 55);

                        System.IO.File.Delete(tempImagePath);
                    }
                }
                else
                {
                    worksheet.Range("A1:B1").Merge();
                    worksheet.Cell("A1").Value = "Logo Not Found!!";
                }

                worksheet.Range("C1:V1").Merge().Value = title;

                worksheet.Row(1).Height = 45;
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                string[] TitlecellNames = { "A1", "B1", "C1" };

                foreach (string TitlecellName in TitlecellNames)
                {
                    worksheet.Cell(TitlecellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Font.Bold = true;
                    worksheet.Cell(TitlecellName).Style.Font.FontSize = 20;
                    worksheet.Cell(TitlecellName).Style.Fill.BackgroundColor = XLColor.FromHtml("#dce6f1");
                }

                // Add header row
                worksheet.Cell("A2").Value = "GatePass Id";
                worksheet.Cell("B2").Value = "User Code";
                worksheet.Cell("C2").Value = "User Name";
                worksheet.Cell("D2").Value = "Department Name";
                //worksheet.Cell("D2").Value = "OtherUser1";
                //worksheet.Cell("E2").Value = "OtherUser2";
                //worksheet.Cell("F2").Value = "OtherUser3";
                worksheet.Cell("E2").Value = "Center";
                worksheet.Cell("F2").Value = "Vehicle Number";
                worksheet.Cell("G2").Value = "Information Mode";
                worksheet.Cell("H2").Value = "Visit Purpose";
                worksheet.Cell("I2").Value = "Remarks";
                worksheet.Cell("J2").Value = "Driver Name";

                worksheet.Cell("K2").Value = "GatePass Generated Date & Time";
                worksheet.Cell("L2").Value = "Gatepass Closing Date & Time";
                worksheet.Cell("M2").Value = "Start Odometer";
                worksheet.Cell("N2").Value = "End Odometer";
                worksheet.Cell("O2").Value = "Difference";

                worksheet.Cell("P2").Value = "Security Name Departure";
                worksheet.Cell("Q2").Value = "Security Verification Date & time";
                worksheet.Cell("R2").Value = "Security Name Arrival ";
                worksheet.Cell("S2").Value = "Security Verification Date & time Arrival";
                worksheet.Cell("T2").Value = "Security StartOdeometer";
                worksheet.Cell("U2").Value = "Security EndOdeometer";
                worksheet.Cell("V2").Value = "Security Odeometer Difference";


                string[] cellNames = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2", "K2", "L2", "M2", "N2", "O2", "P2", "Q2", "R2", "S2", "T2", "U2", "V2" };

                foreach (string cellName in cellNames)
                {
                    worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(cellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(cellName).Style.Font.Bold = true;
                    worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                    worksheet.Cell(cellName).Style.Alignment.WrapText = true;
                }

                string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };

                foreach (string columnName in columnNames)
                {
                    worksheet.Column(columnName).Width = 25;
                }

                // Populate data rows
                int rowIndex = 3;
                foreach (var item in re1)
                {
                    worksheet.Cell("A" + rowIndex).Value = item.GatePassId;
                    worksheet.Cell("B" + rowIndex).Value = item.UserCode;
                    worksheet.Cell("C" + rowIndex).Value = item.EmployeeName;
                    worksheet.Cell("D" + rowIndex).Value = item.DepartmentName;
                    //worksheet.Cell("D" + rowIndex).Value = item.OtherUser1;
                    //worksheet.Cell("E" + rowIndex).Value = item.OtherUser2;
                    //worksheet.Cell("F" + rowIndex).Value = item.OtherUser3;
                    worksheet.Cell("E" + rowIndex).Value = item.Center;
                    worksheet.Cell("F" + rowIndex).Value = item.VehicleRegNumber;
                    worksheet.Cell("G" + rowIndex).Value = item.InformationMode;
                    worksheet.Cell("H" + rowIndex).Value = item.VisitPurpose;
                    worksheet.Cell("I" + rowIndex).Value = item.Remarks;
                    worksheet.Cell("J" + rowIndex).Value = item.Driver;
                    worksheet.Cell("K" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("L" + rowIndex).Value = item.CloseDateTime;
                    worksheet.Cell("M" + rowIndex).Value = item.StartOdometer;
                    worksheet.Cell("N" + rowIndex).Value = item.CloseOdometer;
                    worksheet.Cell("O" + rowIndex).Value = item.Difference;
                    worksheet.Cell("P" + rowIndex).Value = item.DepartureSecurityName;
                    worksheet.Cell("Q" + rowIndex).Value = item.DepartureDateTime;
                    worksheet.Cell("R" + rowIndex).Value = item.ArrivalSecurityName;
                    worksheet.Cell("S" + rowIndex).Value = item.ArrivalDateTime;
                    worksheet.Cell("T" + rowIndex).Value = item.StartKm;
                    worksheet.Cell("U" + rowIndex).Value = item.EndKm;
                    worksheet.Cell("V" + rowIndex).Value = item.DepArrDifference;
                    rowIndex++;
                }

                var tableRange = worksheet.Range("A3:D" + (rowIndex - 1));

                // Add filters to the header row
                //tableRange.SetAutoFilter();

                //workbook.SaveAs(filePath);

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    ms.Position = 0;

                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(ms.ToArray(), contentType, "GatePassReport.xlsx");

                }
            }
        }
        #endregion

        #region==> GatePass Center Visit Report  
        public ActionResult VisitCenterReport()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                VisitCenterReportViewModel model = new VisitCenterReportViewModel();
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
                    model.VisitDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    model.CloseDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    model.EmployeeMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                    ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                    : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                    .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                    .ToList();

                    model.EmployeeMasterList.ForEach(e =>
                    {
                        e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                    });
                    model.DepartmentId = LoggedUserDetails.DepartmentId;
                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                    model.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
                     ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();
                    model.VisitCenterReportList = db.BanasVisitCenterReportRtr("all", model.VisitDateTime, model.CloseDateTime, "", "", "", LoggedUserDetails.EmployeeCode).Where(x => (Convert.ToInt32(x.GatePassStatus) == 0)).ToList();
                    model.AuditorOperateVisitList = db.BanasAuditorOperateVisitMasterRtr("", "all").ToList();

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
        [HttpPost]
        public ActionResult VisitCenterReport(VisitCenterReportViewModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

                string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

                model.VisitCenterReportList = db.BanasVisitCenterReportRtr("report", visitDateTime, closeDateTime, model.DepartmentId, model.Center, model.EmployeeCode, LoggedUserDetails.EmployeeCode).Where(x => (Convert.ToInt32(x.GatePassStatus) == 0)).ToList();

                model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.DepartmentId).ToList();
                model.CenterList = (LoggedUserDetails.EmployeeCode == "Admin")
            ? db.BanasCenterMasterRetrieve("active").ToList() : db.BanasCenterMasterRetrieve("active").Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();
                model.EmployeeMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active").ToList()
                : db.BanasEmployeeMasterRtr(LoggedUserDetails.CompanyCode, "Active")
                .Where(x => x.DepartmentId.ToString() == LoggedUserDetails.DepartmentId)
                .ToList();

                model.EmployeeMasterList.ForEach(e =>
                {
                    e.EmployeeName = $"{e.EmployeeCode} - {e.EmployeeName}";
                });
                model.DepartmentId = LoggedUserDetails.DepartmentId;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SelectGatepassIdwiseDetailsJson(string gatepassId)
        {
            VisitCenterReportViewModel model = new VisitCenterReportViewModel();
            model.AuditorOperateVisitList = db.BanasAuditorOperateVisitMasterRtr("", "All").Where(x => x.GatePassId == gatepassId).ToList();

            var obj = new { model.AuditorOperateVisitList };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExportVisitCenterReport(string DepartmentId, string Center, string EmployeeCode, string VisitDateTime, string CloseDateTime)
        {

            string visitDateTime = VisitDateTime != "" ? generalFunctions.dateconvert(VisitDateTime) : null;

            string closeDateTime = CloseDateTime != "" ? generalFunctions.dateconvert(CloseDateTime) : null;

            var re1 = db.BanasVisitCenterReportRtr("report", visitDateTime, closeDateTime, string.IsNullOrEmpty(DepartmentId) ? null : DepartmentId, string.IsNullOrEmpty(Center) ? null : Center, string.IsNullOrEmpty(EmployeeCode) ? null : EmployeeCode, LoggedUserDetails.EmployeeCode).Where(x => (Convert.ToInt32(x.GatePassStatus) == 0)).ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CenterVisitReportTable");

                string title = "Center Visit Report";

                string LogoPath = Server.MapPath("~/Content/AdminTemplateAssets/images/LogoForExcel.png");

                Image image = null;
                try
                {
                    image = Image.FromFile(LogoPath);
                }
                catch (FileNotFoundException)
                {
                    // Handle the case when the image file doesn't exist
                    Console.WriteLine("Image file not found: " + LogoPath);
                }

                if (image != null)
                {
                    // Your existing code to process the image
                    using (image)
                    {
                        string tempImagePath = Path.GetTempFileName();
                        image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);


                        var picture = worksheet.AddPicture(tempImagePath)
                            .MoveTo(worksheet.Range("A1:B1").Merge().FirstCell().Address, 5, 5)
                            .WithSize(350, 55);

                        System.IO.File.Delete(tempImagePath);
                    }
                }
                else
                {
                    worksheet.Range("A1:B1").Merge();
                    worksheet.Cell("A1").Value = "Logo Not Found!!";
                }

                worksheet.Range("C1:J1").Merge().Value = title;

                worksheet.Row(1).Height = 45;
                worksheet.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                string[] TitlecellNames = { "A1", "B1", "C1" };

                foreach (string TitlecellName in TitlecellNames)
                {
                    worksheet.Cell(TitlecellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Font.Bold = true;
                    worksheet.Cell(TitlecellName).Style.Font.FontSize = 20;
                    worksheet.Cell(TitlecellName).Style.Fill.BackgroundColor = XLColor.FromHtml("#dce6f1");
                }

                // Add header row
                worksheet.Cell("A2").Value = "GatePass Id";
                worksheet.Cell("B2").Value = "Usercode";
                worksheet.Cell("C2").Value = "Username";
                worksheet.Range("D2:E2").Merge();
                worksheet.Cell("D2").Value = "Department";
                worksheet.Range("F2:G2").Merge();
                worksheet.Cell("F2").Value = "Center";
                worksheet.Cell("H2").Value = "Visit Date-Time";
                worksheet.Cell("I2").Value = "Close Date-Time";
                worksheet.Cell("J2").Value = "Total km";

                string[] cellNames = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2" };

                foreach (string cellName in cellNames)
                {
                    worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(cellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(cellName).Style.Font.Bold = true;
                    worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                }

                string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

                foreach (string columnName in columnNames)
                {
                    worksheet.Column(columnName).Style.Alignment.WrapText = true;
                    worksheet.Column(columnName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Column(columnName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Column(columnName).Width = 25;
                }

                // Populate data rows
                int rowIndex = 3;
                foreach (var item in re1)
                {
                    worksheet.Range("A" + rowIndex).Merge();
                    worksheet.Cell("A" + rowIndex).Value = item.GatePassId;
                    worksheet.Cell("B" + rowIndex).Value = item.UserCode;
                    worksheet.Cell("C" + rowIndex).Value = item.Username;
                    worksheet.Range("D" + rowIndex + ":E" + rowIndex).Merge();
                    worksheet.Cell("D" + rowIndex).Value = item.DepartmentName;

                    worksheet.Range("F" + rowIndex + ":G" + rowIndex).Merge();
                    worksheet.Cell("F" + rowIndex).Value = item.CenterName;


                    worksheet.Cell("H" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("I" + rowIndex).Value = item.CloseDateTime;
                    worksheet.Cell("J" + rowIndex).Value = item.TotKm;

                    var re2 = db.BanasAuditorOperateVisitMasterRtr("", "All").Where(x => x.GatePassId == item.GatePassId).ToList();
                    if (re2.Count > 0)
                    {
                        rowIndex++;
                        worksheet.Cell("B" + rowIndex).Value = "Gatepass " + item.GatePassId + " Center Visit Details";
                        worksheet.Range("B" + rowIndex + ":J" + rowIndex).Merge().Style.Font.Bold = true;
                        worksheet.Range("B" + rowIndex + ":J" + rowIndex).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("B" + rowIndex + ":J" + rowIndex).Merge().Style.Font.FontSize = 14;
                        worksheet.Range("B" + rowIndex + ":J" + rowIndex).Style.Fill.BackgroundColor = XLColor.Aqua;

                        rowIndex++;

                        // Add child table header row
                        worksheet.Cell("B" + rowIndex).Value = "Gatepass ID";
                        worksheet.Cell("C" + rowIndex).Value = "Centername";
                        worksheet.Cell("D" + rowIndex).Value = "Google Location";
                        worksheet.Cell("E" + rowIndex).Value = "User Location";
                        worksheet.Cell("F" + rowIndex).Value = "Location";
                        worksheet.Cell("G" + rowIndex).Value = "Odometer";
                        worksheet.Cell("H" + rowIndex).Value = "Odometer Image";
                        worksheet.Cell("I" + rowIndex).Value = "CreateDate";
                        worksheet.Cell("J" + rowIndex).Value = "CreateUser";

                        // Apply formatting to child table header row
                        string[] childHeaderCellNames = { "B" + rowIndex, "C" + rowIndex, "D" + rowIndex, "E" + rowIndex, "F" + rowIndex, "G" + rowIndex, "H" + rowIndex, "I" + rowIndex, "J" + rowIndex };
                        foreach (string cellName in childHeaderCellNames)
                        {
                            worksheet.Cell(cellName).Style.Alignment.WrapText = true;
                            worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(cellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            worksheet.Cell(cellName).Style.Font.Bold = true;
                            worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        rowIndex++;
                        foreach (var item2 in re2)
                        {
                            worksheet.Cell("B" + rowIndex).Value = item2.GatePassId;
                            worksheet.Cell("C" + rowIndex).Value = item2.centername;
                            worksheet.Cell("D" + rowIndex).Value = item2.LocationName;
                            worksheet.Cell("E" + rowIndex).Value = item2.UserGivenLocation;
                            string hyperlinkUrl = "https://www.google.com/maps?q=" + item2.Latitude + "," + item2.Longitude;
                            worksheet.Cell("F" + rowIndex).Value = "View Location";
                            worksheet.Cell("F" + rowIndex).Hyperlink = new XLHyperlink(hyperlinkUrl);

                            worksheet.Cell("G" + rowIndex).Value = item2.Odometer;
                            worksheet.Cell("I" + rowIndex).Value = item2.CreateDate;
                            worksheet.Cell("J" + rowIndex).Value = item2.CreateUser;

                            if (!string.IsNullOrEmpty(item2.OdometerImage))
                            {
                                string imagePath = Server.MapPath("~/" + item2.OdometerImage);
                                Image image1 = null;
                                try
                                {
                                    image1 = Image.FromFile(imagePath);
                                }
                                catch (FileNotFoundException)
                                {
                                    // Handle the case when the image file doesn't exist
                                    Console.WriteLine("Image file not found: " + imagePath);
                                }

                                if (image1 != null)
                                {
                                    // Your existing code to process the image
                                    using (image1)
                                    {
                                        string tempImagePath = Path.GetTempFileName();
                                        image1.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);

                                        var picture = worksheet.AddPicture(tempImagePath)
                                            .MoveTo(worksheet.Cell("H" + rowIndex).AsRange().FirstCell().Address, 5, 5)
                                            .WithSize(200, 55);

                                        worksheet.Column("H").Width = 35;
                                        worksheet.Row(rowIndex).Height = 45;
                                        System.IO.File.Delete(tempImagePath);
                                    }
                                }
                                else
                                {
                                    worksheet.Cell("H" + rowIndex).Value = "Image Not Found!!";
                                }
                            }
                            rowIndex++;
                        }
                    }
                    rowIndex++;
                }

                var tableRange = worksheet.Range("A3:J" + (rowIndex - 1));

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    ms.Position = 0;

                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(ms.ToArray(), contentType, "CenterVisitReport.xlsx");

                }
            }

        }

        //[HttpPost]
        //public ActionResult VisitMgmt(VisitManagementViewModel model)
        //{

        //    try
        //    {
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return RedirectToAction("Login", "Home");
        //        }
        //        model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
        //        //model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();

        //        // Check if VisitDateTime is null before converting
        //        string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

        //        // Check if CloseDateTime is null before converting
        //        string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

        //        model.BanasVisitManagementRtr_Results = db.BanasVisitManagementRtr("all", visitDateTime, closeDateTime, model.DepartmentId, model.VehicleId, model.GatepassId).ToList();
        //        model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
        //        model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Danger(ex.Message.ToString(), true);
        //        return RedirectToAction("Dashboard", "Home");
        //    }

        //}


        #endregion
    }
}