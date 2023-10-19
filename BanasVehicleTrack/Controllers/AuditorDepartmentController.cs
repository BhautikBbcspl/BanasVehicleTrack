using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel.AuditorDepartment;
using BanasVehicleTrackViewModel.SecurityDepartment;
using ClosedXML.Excel;
using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel.Drawings;
using BanasVehicleTrackViewModel.Master;

namespace BanasVehicleTrack.Controllers
{
    public class AuditorDepartmentController : GeneralClass
    {
        // GET: AuditorDepartment
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region==> Auditor Department Visit Management

        //Auditor DASHBOARD
        public ActionResult AuditorDashboard()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                AuditorMgmtViewModel model = new AuditorMgmtViewModel();
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
                    string CurrentYear = DateTime.Now.Year.ToString();
                    string CurrentMonth = DateTime.Now.Month.ToString();

                    model.DashboardCounts = db.BanasAdminDashboardCountRtr("Count", LoggedUserDetails.EmployeeCode).ToList();
                    model.ContractorTotalAmountMonthwiseList = db.BanasContractorTotalAmountMonthwiseRtr(CurrentMonth, CurrentYear, "List").ToList();
                    //model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    //model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    //model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(model.VisitDateTime, model.CloseDateTime, model.DepartmentId, model.VehicleId, model.GatepassId, "dashboard").ToList();
                    //model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode).ToList();
                    //model.VehicleMasterList = db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList();
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
        public ActionResult AuditorMgmtDashboard()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                AuditorMgmtViewModel model = new AuditorMgmtViewModel();
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
                    model.VisitDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    model.CloseDateTime = generalFunctions.dateconvert(generalFunctions.getDate());
                    model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "", LoggedUserDetails.EmployeeCode).ToList();
                    //model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(generalFunctions.getDate(), generalFunctions.getDate(), model.DepartmentId, model.VehicleId, model.GatepassId, "all", LoggedUserDetails.EmployeeCode).ToList();
                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                    model.DepartmentId = LoggedUserDetails.DepartmentId;
                    model.VehicleMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                 ? db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList() : db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).Where(c => c.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();
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
        public ActionResult AuditorMgmtDashboard(AuditorMgmtViewModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "", LoggedUserDetails.EmployeeCode).ToList();

                // Check if VisitDateTime is null before converting
                string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

                // Check if CloseDateTime is null before converting
                string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;


                model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(visitDateTime, closeDateTime, model.DepartmentId, model.VehicleId, model.GatepassId, "all", LoggedUserDetails.EmployeeCode).ToList();
                model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();

                model.VehicleMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                    ? db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList() : db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).Where(c => c.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

                model.DepartmentId = LoggedUserDetails.DepartmentId;
                return View(model);
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }

        }
        public ActionResult SelectDepartmentWiseVehicleJson(int ddlDepartment)
        {
            var VehicleList = db.BanasVehicleMasterRtr("open", LoggedUserDetails.CompanyCode).Where(x => x.DepartmentId == ddlDepartment).Select(c => new VehicleMasterViewModel() { VehicleId = c.VehicleId.ToString(), VehicleRegNumber = c.VehicleRegNumber }).ToList();

            var obj = new { VehicleList };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExportReport(string DepartmentId, string VehicleId, string VisitDateTime, string CloseDateTime, string GatePassId)
        {

            string visitDateTime = VisitDateTime != "" ? generalFunctions.dateconvert(VisitDateTime) : null;

            string closeDateTime = CloseDateTime != "" ? generalFunctions.dateconvert(CloseDateTime) : null;

            var re1 = db.BanasAuditorDepartmentDashboardRetrieve(visitDateTime, closeDateTime, string.IsNullOrEmpty(DepartmentId) ? null : DepartmentId, string.IsNullOrEmpty(VehicleId) ? null : VehicleId, string.IsNullOrEmpty(GatePassId) ? null : GatePassId, "all", LoggedUserDetails.EmployeeCode).ToList();

            //string filePath = Server.MapPath("~/Uploads/AuditorDashboardReport/AuditorDashboardReport.xlsx");
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AuditorReportTable");

                // Add title
                string title = "Auditor Report Dashboard";

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

                worksheet.Range("C1:T1").Merge().Value = title;

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
                //worksheet.Cell("E2").Value = "OtherUser1";
                //worksheet.Cell("F2").Value = "OtherUser2";
                //worksheet.Cell("G2").Value = "OtherUser3";
                worksheet.Cell("A2").Value = "GatePass Id";
                worksheet.Cell("B2").Value = "User Code";
                worksheet.Cell("C2").Value = "User Name";
                worksheet.Cell("D2").Value = "Department";
                worksheet.Cell("E2").Value = "Center";
                worksheet.Cell("F2").Value = "Vehicle Code";
                worksheet.Cell("G2").Value = "Vehicle Reg Number";
                worksheet.Cell("H2").Value = "Information Mode";
                worksheet.Cell("I2").Value = "Visit Purpose";
                worksheet.Cell("J2").Value = "Remarks";
                worksheet.Cell("K2").Value = "Driver";
                worksheet.Cell("L2").Value = "Rate/km";
                worksheet.Cell("M2").Value = "GatePass Generated Date & Time";
                worksheet.Cell("N2").Value = "GatePass Closing Date & Time";

                worksheet.Cell("O2").Value = "Departure Security Name";
                worksheet.Cell("P2").Value = "Departure Security Sign";
                worksheet.Cell("Q2").Value = "Arrival Security Name";
                worksheet.Cell("R2").Value = "Arrival Security Sign";
                worksheet.Cell("S2").Value = "Final Difference";
                worksheet.Cell("T2").Value = "Final Amount";

                string[] cellNames = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2", "K2", "L2", "M2", "N2", "O2", "P2", "Q2", "R2", "S2", "T2" };

                foreach (string cellName in cellNames)
                {
                    worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(cellName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(cellName).Style.Font.Bold = true;
                    worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                }

                string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" };

                foreach (string columnName in columnNames)
                {
                    worksheet.Column(columnName).Width = 25;
                }

                // Populate data rows
                int rowIndex = 3;
                decimal totalDifference = 0;
                //decimal totalValueperColumn = 0;
                decimal totalValue = 0;
                foreach (var item in re1)
                {
                    worksheet.Cell("A" + rowIndex).Value = item.GatePassId;
                    worksheet.Cell("B" + rowIndex).Value = item.UserCode;
                    worksheet.Cell("C" + rowIndex).Value = item.UserName;
                    worksheet.Cell("D" + rowIndex).Value = item.DepartmentName;
                    worksheet.Cell("E" + rowIndex).Value = item.Center;
                    //worksheet.Cell("E" + rowIndex).Value = item.OtherUser1;
                    //worksheet.Cell("F" + rowIndex).Value = item.OtherUser2;
                    //worksheet.Cell("G" + rowIndex).Value = item.OtherUser3;

                    worksheet.Cell("F" + rowIndex).Value = item.VehicleCode;
                    worksheet.Cell("G" + rowIndex).Value = item.VehicleRegNumber;
                    worksheet.Cell("H" + rowIndex).Value = item.InformationMode;
                    worksheet.Cell("I" + rowIndex).Value = item.VisitPurpose;
                    worksheet.Cell("J" + rowIndex).Value = item.Remarks;
                    worksheet.Cell("K" + rowIndex).Value = item.Driver;
                    worksheet.Cell("L" + rowIndex).Value = item.RatePerKm;
                    worksheet.Cell("M" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("N" + rowIndex).Value = item.CloseDateTime;

                    worksheet.Cell("O" + rowIndex).Value = item.DepartureSecurityName;

                    worksheet.Cell("Q" + rowIndex).Value = item.ArrivalSecurityName;
                    worksheet.Cell("S" + rowIndex).Value = item.FinalApprDifference;
                    worksheet.Cell("T" + rowIndex).Value = item.FinalAmount;

                    if (!string.IsNullOrEmpty(item.DepartureSecuritySignature))
                    {
                        string imagePath = Server.MapPath("~/" + item.DepartureSecuritySignature);

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
                                    .MoveTo(worksheet.Cell("P" + rowIndex).AsRange().FirstCell().Address, 5, 5)
                                    .WithSize(200, 55);

                                worksheet.Column("P").Width = 35;
                                worksheet.Row(rowIndex).Height = 45;
                                System.IO.File.Delete(tempImagePath);
                            }
                        }
                        else
                        {
                            worksheet.Cell("P" + rowIndex).Value = "Image Not Found";
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ArrivalSecuritySignature))
                    {
                        string imagePath1 = Server.MapPath("~/" + item.ArrivalSecuritySignature);

                        Image image2 = null;
                        try
                        {
                            image2 = Image.FromFile(imagePath1);
                        }
                        catch (FileNotFoundException)
                        {
                            // Handle the case when the image file doesn't exist
                            Console.WriteLine("Image file not found: " + imagePath1);
                        }

                        if (image2 != null)
                        {
                            // Your existing code to process the image
                            using (image2)
                            {
                                string tempImagePath = Path.GetTempFileName();
                                image2.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);

                                var picture = worksheet.AddPicture(tempImagePath)
                                    .MoveTo(worksheet.Cell("R" + rowIndex).AsRange().FirstCell().Address, 5, 5)
                                    .WithSize(200, 55);

                                worksheet.Column("R").Width = 35;
                                worksheet.Row(rowIndex).Height = 45;
                                System.IO.File.Delete(tempImagePath);
                            }
                        }
                        else
                        {
                            worksheet.Cell("R" + rowIndex).Value = "Image Not Found";
                        }
                    }

                    worksheet.Row(rowIndex).Height = 45;
                    worksheet.Row(rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Row(rowIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    //totalDifference += Convert.ToDecimal(item.Difference);
                    //totalValueperColumn = Convert.ToDecimal(item.RatePerKm) * Convert.ToDecimal(item.Difference);
                    //totalValue += totalValueperColumn;

                    totalDifference += Convert.ToDecimal(item.FinalApprDifference);
                    totalValue += Convert.ToDecimal(item.FinalAmount);
                    rowIndex++;
                }
                worksheet.Cell("S" + rowIndex).Value = "TOTAL Value";
                worksheet.Cell("S" + rowIndex).Style.Font.Bold = true;
                worksheet.Cell("S" + rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("S" + rowIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell("T" + rowIndex).Value = totalValue;
                worksheet.Cell("T" + rowIndex).Style.Font.Bold = true;
                worksheet.Cell("T" + rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("T" + rowIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // worksheet.RowHeight = 45;


                // Set the range for the table
                var tableRange = worksheet.Range("A3:S" + (rowIndex - 1));

                //// Add filters to the header row
                //tableRange.SetAutoFilter();
                //workbook.SaveAs(filePath);


                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    ms.Position = 0;

                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(ms.ToArray(), contentType, "AuditorDashboardReport.xlsx");

                }
            }

        }
        public ActionResult AuditorFinalApprove()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                AuditorFinalApproveViewModel model = new AuditorFinalApproveViewModel();
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

                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorFinalApproveRetrieve(generalFunctions.getDate(), generalFunctions.getDate(), model.DepartmentId, model.VehicleId, "NotApproved", LoggedUserDetails.EmployeeCode).ToList();

                    model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                    model.DepartmentId = LoggedUserDetails.DepartmentId;
                    model.VehicleMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                 ? db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList() : db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).Where(c => c.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

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
        public ActionResult AuditorFinalApprove(AuditorFinalApproveViewModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Home");
                }
                string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;

                // Check if CloseDateTime is null before converting
                string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

                model.BanasVehicleGatepassRetrieveList = db.BanasAuditorFinalApproveRetrieve(visitDateTime, closeDateTime, model.DepartmentId, model.VehicleId, "NotApproved", LoggedUserDetails.EmployeeCode).ToList();

                model.DepartmentMasterList = db.BanasDepartmentMasterRetrieve("active", LoggedUserDetails.CompanyCode, LoggedUserDetails.EmployeeCode).ToList();
                model.DepartmentId = LoggedUserDetails.DepartmentId;
                model.VehicleMasterList = (LoggedUserDetails.EmployeeCode == "Admin")
                ? db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).ToList() : db.BanasVehicleMasterRtr("Active", LoggedUserDetails.CompanyCode).Where(c => c.DepartmentId.ToString() == LoggedUserDetails.DepartmentId).ToList();

                return View(model);

            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        [HttpPost]
        public ActionResult ApproveDifferenceSave(List<DifferenceData> dataToSend)
        {
            try
            {
                var msg = "";
                foreach (DifferenceData item in dataToSend)
                {
                    string id = item.Id;
                    string value = item.Value;
                    string type = item.Type;

                    msg = db.BanasAuditorFinalApproveUpd(id, value, type, LoggedUserDetails.EmployeeCode, generalFunctions.getTimeZoneDatetimedb(), User.Identity.Name, "Approve").FirstOrDefault();
                }
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
                return RedirectToAction("AuditorFinalApprove");
            }
            catch (Exception ex)
            {
                Danger(ex.Message.ToString(), true);
                return RedirectToAction("Dashboard", "Home");
            }
        }
        #endregion

        #region Contractor Payment
        public ActionResult ContractorPayment()
        {
            AuditorMgmtViewModel model = new AuditorMgmtViewModel();
            model.ContractorList = db.BanasContractorMasterRetrieve("All", LoggedUserDetails.CompanyCode).ToList();
            model.BanasContractorAmountRptList = db.BanasContractorAmountRpt(model.ContractorId, model.VisitDateTime, model.CloseDateTime).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ContractorPayment(AuditorMgmtViewModel model)
        {
            string visitDateTime = model.VisitDateTime != null ? generalFunctions.dateconvert(model.VisitDateTime) : null;
            string closeDateTime = model.CloseDateTime != null ? generalFunctions.dateconvert(model.CloseDateTime) : null;

            model.ContractorList = db.BanasContractorMasterRetrieve("All", LoggedUserDetails.CompanyCode).ToList();
            model.BanasContractorAmountRptList = db.BanasContractorAmountRpt(model.ContractorId, visitDateTime, closeDateTime).ToList();
            return View(model);


        }
        #endregion
    }
}