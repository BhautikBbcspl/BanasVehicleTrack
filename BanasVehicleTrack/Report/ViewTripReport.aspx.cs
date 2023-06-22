using BanasVehicleTrackDbClasses;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanasVehicleTrack.Report
{
    public partial class ViewTripReport : System.Web.UI.Page
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                Uri myUri = new Uri(Request.Url.AbsoluteUri);

                string gatePassId = HttpUtility.ParseQueryString(myUri.Query).Get("gatePassId");

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/rdlc/ViewTripReport.rdlc");


                var re1 = db.BanasVehicleGatepassRetrieve("details", "", "", "", "", gatePassId).ToList();
                ReportDataSource datasource = new ReportDataSource("BanasVehicleTrackDataSet", re1);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                byte[] pdfBytes = ReportViewer1.LocalReport.Render("PDF", null);

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline; filename=ViewTripReport.pdf");

                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
        }
    }
}