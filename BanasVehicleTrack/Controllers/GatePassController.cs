using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using BanasVehicleTrackViewModel.GatePass;
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
                    var re1 = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").Where(x => x.GatePassStatus == Convert.ToInt32(filterValue));
                    return View(re1);
                }
                else
                {
                    var re1 = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "");
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
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id).ToList();

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
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id).ToList();

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
                model.BanasVehicleGatepassRetrieveList = db.BanasVehicleGatepassRetrieve(model.Action, "", "", "", "", id).ToList();
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
                string msg = db.BanasVehicleGatepassInsUpd(vsgp.GatePassId, vsgp.DepartmentId, vsgp.UserCode, vsgp.OtherUser1, vsgp.OtherUser2, vsgp.OtherUser3, vsgp.InformationMode, vsgp.VisitDateTime, vsgp.VisitPurpose, vsgp.Remarks, vsgp.VehicleDepartment, vsgp.Driver, vsgp.VehicleId, vsgp.StartOdometer, vsgp.CloseOdometer, closeDateTime, vsgp.CloseRemark, vsgp.Netkm, vsgp.CreateUser, vsgp.CreateDate, vsgp.CloseUser, vsgp.CloseDate, vsgp.EditUser, vsgp.EditDate, vsgp.Difference, vsgp.Action,null, null, "", "", "", null, null, "", "", "").FirstOrDefault();

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
            GatepassViewModel gvm = new GatepassViewModel();
            gvm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", "BDL01").ToList();
            gvm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", "BDL01").ToList();
            gvm.VehicleList = db.BanasVehicleMasterRtr("active", "BDL01").ToList();
            gvm.EmployeeList = db.BanasEmployeeMasterRtr("BDL01", "Active").ToList();
            gvm.VisitDateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            gvm.StartOdometer = db.BanasFetchLastCosingOdometer(gvm.VehicleId).SingleOrDefault().ToString();
            //gvm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
            //gvm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
            //gvm.VehicleList = db.BanasVehicleMasterRtr("active", LoggedUserDetails.CompanyCode).ToList();
            //gvm.EmployeeList = db.BanasEmployeeMasterRtr( LoggedUserDetails.CompanyCode, "Active").ToList();
            gvm.action = "Add";
            return View(gvm);
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
                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, dm.DepartmentId, dm.UserCode, dm.OtherUser1, dm.OtherUser2, dm.OtherUser3, dm.InformationMode, dm.VisitDateTime, dm.VisitPurpose, dm.Remarks, dm.VehicleDepartment, dm.Driver, dm.VehicleId, dm.StartOdometer, dm.CloseOdometer, dm.CloseDateTime, dm.CloseRemark, dm.Netkm, dm.CreateUser, dm.CreateDate, dm.CloseUser, dm.CloseDate, dm.EditUser, dm.EditDate, dm.difference, "open", null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
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
                    string msg = db.BanasVehicleGatepassInsUpd(dm.GatePassId, dm.DepartmentId, dm.UserCode, dm.OtherUser1, dm.OtherUser2, dm.OtherUser3, dm.InformationMode, dm.VisitDateTime, dm.VisitPurpose, dm.Remarks, dm.VehicleDepartment, dm.Driver, dm.VehicleId, dm.StartOdometer, dm.CloseOdometer, dm.CloseDateTime, dm.CloseRemark, dm.Netkm, dm.CreateUser, dm.CreateDate, dm.CloseUser, dm.CloseDate, dm.EditUser, dm.EditDate, dm.difference, "edit", null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
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
                var mm = db.BanasVehicleGatepassRetrieve("details", null, null, "", "", id).FirstOrDefault();
                dm.DepartmentList = db.BanasDepartmentMasterRetrieve("active", "BDL01").ToList();
                dm.VisitPurposeList = db.BanasVisitPurposeMasterRtr("active", "BDL01").ToList();
                dm.VehicleList = db.BanasVehicleMasterRtr("active", "BDL01").ToList();
                dm.EmployeeList = db.BanasEmployeeMasterRtr("BDL01", "Active").ToList();
                dm.action = "Edit";
                dm.GatePassId = id;
                dm.DepartmentId = mm.DepartmentId.ToString();
                dm.UserCode = mm.UserCode;
                dm.OtherUser1 = mm.OtherUser1;
                dm.OtherUser2 = mm.OtherUser2;
                dm.OtherUser3 = mm.OtherUser3;
                dm.InformationMode = mm.InformationMode;
                dm.VisitDateTime = mm.VisitDateTime;
                dm.VisitPurpose = mm.VisitPurposeId.ToString();
                dm.Remarks = mm.Remarks;
                dm.VehicleDepartment = mm.VehicleDepartmentId.ToString();
                dm.Driver = mm.Driver;
                dm.VehicleId = mm.VehicleId.ToString();
                dm.StartOdometer = mm.StartOdometer.ToString();
                return View("AddGatepass", dm);
            }
            catch (Exception ex)
            {
                //Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult GetOdometer(int Vehicleid)
        {
            try
            {
                var odometer = db.BanasFetchLastCosingOdometer(Vehicleid.ToString()).SingleOrDefault().ToString();
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
                    //model.Action = "Save";
                    model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.BanasVisitManagementRtr_Results = db.BanasVisitManagementRtr("all", model.VisitDateTime, model.CloseDateTime, model.DepartmentId, model.VehicleId, model.GatepassId).ToList();
                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                    model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();
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
        public ActionResult VisitMgmt(VisitManagementViewModel model)
        {
           
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login", "Home");
                    }
                    model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();

                    // Check if VisitDateTime is null before converting
                    string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

                    // Check if CloseDateTime is null before converting
                    string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

                    model.BanasVisitManagementRtr_Results = db.BanasVisitManagementRtr("all", visitDateTime, closeDateTime, model.DepartmentId, model.VehicleId, model.GatepassId).ToList();
                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                    model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    Danger(ex.Message.ToString(), true);
                    return RedirectToAction("Dashboard", "Home");
                }

        }


        public ActionResult ExportReport(string DepartmentId, string VehicleId, string VisitDateTime, string CloseDateTime, string GatePassId)
        {

            string visitDateTime = VisitDateTime != "" ? generalFunctions.dateconvert(VisitDateTime) : null;

            string closeDateTime = CloseDateTime != "" ? generalFunctions.dateconvert(CloseDateTime) : null;

            var re1 = db.BanasAuditorDepartmentDashboardRetrieve(visitDateTime, closeDateTime, DepartmentId, VehicleId, string.IsNullOrEmpty(GatePassId) ? null : GatePassId, "all").ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("GatePassReport");

                // Add title
                string title = "Gate Pass Report";
                worksheet.Range("A1:U1").Merge().Value = title;

                string[] TitlecellNames = { "A1" };

                foreach (string TitlecellName in TitlecellNames)
                {
                    worksheet.Cell(TitlecellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Font.Bold = true;
                    worksheet.Cell(TitlecellName).Style.Font.FontSize = 20;
                    worksheet.Cell(TitlecellName).Style.Fill.BackgroundColor = XLColor.FromHtml("#dce6f1");
                }

                // Add header row
                worksheet.Cell("A2").Value = "GatePass Id";
                worksheet.Cell("B2").Value = "Department Name";
                worksheet.Cell("C2").Value = "User";
                worksheet.Cell("D2").Value = "OtherUser1";
                worksheet.Cell("E2").Value = "OtherUser2";
                worksheet.Cell("F2").Value = "OtherUser3";
                worksheet.Cell("G2").Value = "Information Mode";
                worksheet.Cell("H2").Value = "Visit DateTime";
                worksheet.Cell("I2").Value = "Visit Purpose";
                worksheet.Cell("J2").Value = "Remarks";
                worksheet.Cell("K2").Value = "Visit Department";
                worksheet.Cell("L2").Value = "Vehicle Number";
                worksheet.Cell("M2").Value = "Driver Name";
                worksheet.Cell("N2").Value = "Start Odometer";
                worksheet.Cell("O2").Value = "GatePass Generated Date & Time";
                worksheet.Cell("P2").Value = "Security Name Departure";
                worksheet.Cell("Q2").Value = "Security Verification Date & time";
                worksheet.Cell("R2").Value = "Security Name Arrival ";
                worksheet.Cell("S2").Value = "Security Verification Date & time Arrival";
                worksheet.Cell("T2").Value = "End Odometer";
                worksheet.Cell("U2").Value = "Gatepass Closing Date & Time";

                string[] cellNames = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2", "K2", "L2", "M2", "N2", "O2", "P2", "Q2", "R2", "S2", "T2", "U2" };

                foreach (string cellName in cellNames)
                {
                    worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(cellName).Style.Font.Bold = true;
                    worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                }

                string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U" };

                foreach (string columnName in columnNames)
                {
                    worksheet.Column(columnName).Width = 25;
                }

                // Populate data rows
                int rowIndex = 3;
                foreach (var item in re1)
                {
                    worksheet.Cell("A" + rowIndex).Value = item.GatePassId;
                    worksheet.Cell("B" + rowIndex).Value = item.DepartmentName;
                    worksheet.Cell("C" + rowIndex).Value = item.UserCode;
                    worksheet.Cell("D" + rowIndex).Value = item.OtherUser1;
                    worksheet.Cell("E" + rowIndex).Value = item.OtherUser2;
                    worksheet.Cell("F" + rowIndex).Value = item.OtherUser3;
                    worksheet.Cell("G" + rowIndex).Value = item.InformationMode;
                    worksheet.Cell("H" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("I" + rowIndex).Value = item.VisitPurpose;
                    worksheet.Cell("J" + rowIndex).Value = item.Remarks;
                    worksheet.Cell("K" + rowIndex).Value = item.VehicleDepartment;
                    worksheet.Cell("L" + rowIndex).Value = item.VehicleRegNumber;
                    worksheet.Cell("M" + rowIndex).Value = item.Driver;
                    worksheet.Cell("N" + rowIndex).Value = item.StartOdometer;
                    worksheet.Cell("O" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("P" + rowIndex).Value = item.DepartureSecurityName;
                    worksheet.Cell("Q" + rowIndex).Value = item.DepartureDateTime;
                    worksheet.Cell("R" + rowIndex).Value = item.ArrivalSecurityName;
                    worksheet.Cell("S" + rowIndex).Value = item.ArrivalDateTime;
                    worksheet.Cell("T" + rowIndex).Value = item.CloseOdometer;
                    worksheet.Cell("U" + rowIndex).Value = item.CloseDateTime;
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
    }
}