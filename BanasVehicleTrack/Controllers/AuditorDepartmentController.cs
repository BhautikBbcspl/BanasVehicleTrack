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
                    //model.Action = "Save";
                    model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(model.VisitDateTime, model.CloseDateTime, model.DepartmentId, model.VehicleId, model.GatepassId, "dashboard").ToList();
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
                    model.visitDateTimeList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.GatePassIdList = db.BanasVehicleGatepassRetrieve("all", "", "", "", "", "").ToList();
                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(model.VisitDateTime, model.CloseDateTime, model.DepartmentId, model.VehicleId, model.GatepassId, "all").ToList();
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
        public ActionResult AuditorMgmtDashboard(AuditorMgmtViewModel model)
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


                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorDepartmentDashboardRetrieve(visitDateTime, closeDateTime, model.DepartmentId, model.VehicleId, model.GatepassId, "all").ToList();
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
        [HttpPost]
        public ActionResult ExportReport(string DepartmentId, string VehicleId, string VisitDateTime, string CloseDateTime, string GatePassId)
        {

            string visitDateTime = VisitDateTime != "" ? generalFunctions.dateconvert(VisitDateTime) : null;

            string closeDateTime = CloseDateTime != "" ? generalFunctions.dateconvert(CloseDateTime) : null;

            var re1 = db.BanasAuditorDepartmentDashboardRetrieve(visitDateTime, closeDateTime, DepartmentId, VehicleId, string.IsNullOrEmpty(GatePassId) ? null : GatePassId, "all").ToList();

            //string filePath = Server.MapPath("~/Uploads/AuditorDashboardReport/AuditorDashboardReport.xlsx");
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AuditorReportTable");

                // Add title
                string title = "Auditor Report Dashboard";
                worksheet.Range("A1:V1").Merge().Value = title;

                string[] TitlecellNames = { "A1" };

                foreach (string TitlecellName in TitlecellNames)
                {
                    worksheet.Cell(TitlecellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(TitlecellName).Style.Font.Bold = true;
                    worksheet.Cell(TitlecellName).Style.Font.FontSize = 20;
                    worksheet.Cell(TitlecellName).Style.Fill.BackgroundColor = XLColor.FromHtml("#dce6f1");
                }

                // Add header row
                worksheet.Cell("A2").Value = "Trip Code";
                worksheet.Cell("B2").Value = "GatePass Id";
                worksheet.Cell("C2").Value = "Department Name";
                worksheet.Cell("D2").Value = "UserCode";
                worksheet.Cell("E2").Value = "OtherUser1";
                worksheet.Cell("F2").Value = "OtherUser2";
                worksheet.Cell("G2").Value = "OtherUser3";
                worksheet.Cell("H2").Value = "Information Mode";
                worksheet.Cell("I2").Value = "Visit DateTime";
                worksheet.Cell("J2").Value = "Visit Purpose";
                worksheet.Cell("K2").Value = "Remarks";
                worksheet.Cell("L2").Value = "Vehicle Department";
                worksheet.Cell("M2").Value = "Driver";
                worksheet.Cell("N2").Value = "Vehicle Code";
                worksheet.Cell("O2").Value = "Vehicle Reg Number";
                worksheet.Cell("P2").Value = "Rate/km";
                worksheet.Cell("Q2").Value = "Departure Security Name";
                worksheet.Cell("R2").Value = "Departure Security Sign";
                worksheet.Cell("S2").Value = "Arrival Security Name";
                worksheet.Cell("T2").Value = "Arrival Security Sign";
                worksheet.Cell("U2").Value = "Final Difference";
                worksheet.Cell("V2").Value = "Final Amount";

                string[] cellNames = { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2", "J2", "K2", "L2", "M2", "N2", "O2", "P2","Q2","R2","S2","T2","U2","V2" };

                foreach (string cellName in cellNames)
                {
                    worksheet.Cell(cellName).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(cellName).Style.Font.Bold = true;
                    worksheet.Cell(cellName).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                }

                string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P","Q","R","S","T","U","V" };

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
                    worksheet.Cell("B" + rowIndex).Value = item.GatePassId;
                    worksheet.Cell("C" + rowIndex).Value = item.DepartmentName;
                    worksheet.Cell("D" + rowIndex).Value = item.UserCode;
                    worksheet.Cell("E" + rowIndex).Value = item.OtherUser1;
                    worksheet.Cell("F" + rowIndex).Value = item.OtherUser2;
                    worksheet.Cell("G" + rowIndex).Value = item.OtherUser3;
                    worksheet.Cell("H" + rowIndex).Value = item.InformationMode;
                    worksheet.Cell("I" + rowIndex).Value = item.VisitDateTime;
                    worksheet.Cell("J" + rowIndex).Value = item.VisitPurpose;
                    worksheet.Cell("K" + rowIndex).Value = item.Remarks;
                    worksheet.Cell("L" + rowIndex).Value = item.VehicleDepartment;
                    worksheet.Cell("M" + rowIndex).Value = item.Driver;
                    worksheet.Cell("N" + rowIndex).Value = item.VehicleCode;
                    worksheet.Cell("O" + rowIndex).Value = item.VehicleRegNumber;
                    worksheet.Cell("P" + rowIndex).Value = item.RatePerKm;
                    worksheet.Cell("Q" + rowIndex).Value = item.DepartureSecurityName;
                    worksheet.Cell("S" + rowIndex).Value = item.ArrivalSecurityName;
                    worksheet.Cell("U" + rowIndex).Value = item.FinalApprDifference;
                    worksheet.Cell("V" + rowIndex).Value = item.FinalAmount;

                    if (!string.IsNullOrEmpty(item.DepartureSecuritySignature))
                    {
                        string imagePath = Server.MapPath("~/" + item.DepartureSecuritySignature);

                        using (Image image = Image.FromFile(imagePath))
                        {
                            string tempImagePath = Path.GetTempFileName();
                            image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);

                            var picture = worksheet.AddPicture(tempImagePath)
                                .MoveTo(worksheet.Cell("R" + rowIndex).AsRange().FirstCell().Address, 5, 5)
                                .WithSize(200, 55);


                            // Resize column width to fit the image
                            worksheet.Column("R").Width = 35;
                            worksheet.Row(rowIndex).Height = 45;
                            System.IO.File.Delete(tempImagePath);
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ArrivalSecuritySignature))
                    {
                        string imagePath = Server.MapPath("~/" + item.ArrivalSecuritySignature);

                        using (Image image = Image.FromFile(imagePath))
                        {
                            string tempImagePath = Path.GetTempFileName();
                            image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);

                            var picture = worksheet.AddPicture(tempImagePath)
                                .MoveTo(worksheet.Cell("T" + rowIndex).AsRange().FirstCell().Address, 5, 5)
                                .WithSize(200, 55);


                            // Resize column width to fit the image
                            worksheet.Column("T").Width = 35;
                            worksheet.Row(rowIndex).Height = 45;
                            System.IO.File.Delete(tempImagePath);
                        }
                    }

                    worksheet.Row(rowIndex).Height = 45;
                    worksheet.Row(rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    //totalDifference += Convert.ToDecimal(item.Difference);
                    //totalValueperColumn = Convert.ToDecimal(item.RatePerKm) * Convert.ToDecimal(item.Difference);
                    //totalValue += totalValueperColumn;

                    totalDifference += Convert.ToDecimal(item.FinalApprDifference);
                    totalValue += Convert.ToDecimal(item.FinalAmount);
                    rowIndex++;
                }
                worksheet.Cell("U" + rowIndex).Value = "TOTAL Value";
                worksheet.Cell("U" + rowIndex).Style.Font.Bold = true;
                worksheet.Cell("U" + rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Cell("V" + rowIndex).Value = totalValue;
                worksheet.Cell("V" + rowIndex).Style.Font.Bold = true;
                worksheet.Cell("V" + rowIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // worksheet.RowHeight = 45;


                // Set the range for the table
                var tableRange = worksheet.Range("A3:V" + (rowIndex - 1));

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
                    model.BanasVehicleGatepassRetrieveList = db.BanasAuditorFinalApproveRetrieve("NotApproved").ToList();
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

                    msg=db.BanasAuditorFinalApproveUpd(id,value,type,LoggedUserDetails.EmployeeCode, generalFunctions.getTimeZoneDatetimedb(),User.Identity.Name, "Approve").FirstOrDefault();
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
    }
}