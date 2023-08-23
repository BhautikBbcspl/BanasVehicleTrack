//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanasVehicleTrackDbClasses
{
    using System;
    
    public partial class BanasAuditorFinalApproveRetrieve_Result
    {
        public string GatePassId { get; set; }
        public string DepartmentName { get; set; }
        public string UserCode { get; set; }
        public string Username { get; set; }
        public string OtherUser1 { get; set; }
        public string OtherUser2 { get; set; }
        public string OtherUser3 { get; set; }
        public string InformationMode { get; set; }
        public string VisitDateTime { get; set; }
        public string VisitPurpose { get; set; }
        public string Remarks { get; set; }
        public int CenterId { get; set; }
        public string Center { get; set; }
        public string Driver { get; set; }
        public string VehicleCode { get; set; }
        public string VehicleRegNumber { get; set; }
        public Nullable<int> StartOdometer { get; set; }
        public Nullable<decimal> RatePerKm { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> GatePassStatus { get; set; }
        public Nullable<int> CloseOdometer { get; set; }
        public string CloseRemark { get; set; }
        public string CloseDateTime { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public string CloseUser { get; set; }
        public Nullable<int> Netkm { get; set; }
        public Nullable<int> Difference { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public Nullable<int> VisitPurposeId { get; set; }
        public Nullable<int> VehicleDepartmentId { get; set; }
        public string DepartureSecurityName { get; set; }
        public string DepartureSecuritySignature { get; set; }
        public Nullable<int> DepartureSecurityId { get; set; }
        public string DepartureSecurityRemark { get; set; }
        public Nullable<int> StartKm { get; set; }
        public string DepartureDateTime { get; set; }
        public Nullable<int> DepartureVerifyStatus { get; set; }
        public string ArrivalSecurityName { get; set; }
        public string ArrivalSecurityCode { get; set; }
        public string ArrivalSecuritySignature { get; set; }
        public Nullable<int> ArrivalSecurityId { get; set; }
        public string ArrivalSecurityRemark { get; set; }
        public Nullable<int> EndKm { get; set; }
        public string ArrivalDateTime { get; set; }
        public Nullable<int> ArrivalVerifyStatus { get; set; }
        public Nullable<int> DepArrDifference { get; set; }
        public Nullable<int> FinalApprGatepassVisitStatus { get; set; }
        public Nullable<int> FinalApprSecurityVisitStatus { get; set; }
        public Nullable<int> FinalApprAuditVisitStatus { get; set; }
        public Nullable<int> FinalApprDifference { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public Nullable<int> TotalDifference { get; set; }
    }
}
