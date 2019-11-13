using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrjAndaa.Models
{
    public class GetterSetter
    {
    }

    public class ClsMasters
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ClsClaimTypes
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ClsSchemeNames
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ClsGetDocNo
    {
        public string DocNo { get; set; }
    }

    public class ClsSchemeFlag
    {
        public string Flag { get; set; }
    }

    public class ClsGetSalesData
    {
        public string BodyId { get; set; }
        public string CustomerId { get; set; }
        public string SchemeName { get; set; }
        public string CustName { get; set; }
        public string TargetAmt { get; set; }
        public string TaxableSL { get; set; }
        public string TaxableSR { get; set; }
        public string NetSales { get; set; }
        //public string AppStatus { get; set; }
        public string ClaimPct { get; set; }
        public string ClaimAmt { get; set; }
        public string ClaimType { get; set; }
        public int ClaimStatus { get; set; }
        public int Flag { get; set; }
    }

    public class ClsGetSchemeHeader
    {
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string CreatedBy { get; set; }
        public string TargetAmt { get; set; }
        public string SchemePct { get; set; }
        public string SchemeName { get; set; }
        public string SchemeActive { get; set; }
        public string EffFromDt { get; set; }
        public string EffToDt { get; set; }
    }
    public class ClsReconciled
    {
        public int Index { get; set; }
        public Int32 CompanyId { get; set; }
        public string SessionId { get; set; }
        public string Userid { get; set; }
        public int Rownumber { get; set; }
        public string VendorName { get; set; }
        public string PurchseInv { get; set; }
        public string Receiveddate { get; set; }
        public int Stockbalance { get; set; }
        public string image { get; set; }
        public string Duedate { get; set; }
        public decimal Dueamount { get; set; }
        public bool select { get; set; }
        public string Currency { get; set; }
        public string Vendorattachment { get; set; }
        public string Lastupdate { get; set; }
        public decimal Topay { get; set; }
        public decimal Topayamount { get; set; }
        public string BenificeryName { get; set; }
        public string BenificeryAccount { get; set; }
        public string Bank { get; set; }
        public DateTime iDate { get; set; }
        public decimal DPMamount { get; set; }
        public int IcurrencyId { get; set; }
        public int AccountId { get; set; }
        public string s64image { get; set; }
        public string s64vendorattch { get; set; }
        public string Accpaymode { get; set; }
        public int paymentmodeid { get; set; }
        public string AllAccpaymentmodes { get; set; }
        public int Flag { get; set; }
    }
    public class ClsPrepaid
    {
        public Int32 CompanyId { get; set; }
        public string SessionId { get; set; }
        public string Userid { get; set; }
        public int Rownumber { get; set; }
        public string VendorName { get; set; }
        public string Expense { get; set; }
        public string VoucherNo { get; set; }
        public string Lastupdate { get; set; }
        public decimal Lastpayment { get; set; }
        public string RentTerm { get; set; }
        public string Duedate { get; set; }
        public decimal Dueamount { get; set; }
        public bool Select { get; set; }
        public decimal Topay { get; set; }
        public decimal Topayamount { get; set; }
        public string BenificeryName { get; set; }
        public string BenificeryAccount { get; set; }
        public string Bank { get; set; }
        public decimal DPMamount { get; set; }
        public int IcurrencyId { get; set; }
        public int AccountId { get; set; }
    }
    public class ClsRecurring
    {
        public Int32 CompanyId { get; set; }
        public string SessionId { get; set; }
        public string Userid { get; set; }
        public int Rownumber { get; set; }
        public string Account { get; set; }
        public string Expense { get; set; }
        public string VoucherNo { get; set; }
        public string Lastupdate { get; set; }
        public decimal Lastpayment { get; set; }
        public string BillDate { get; set; }
        public string Duedate { get; set; }
        public decimal Dueamount { get; set; }
        public bool Select { get; set; }
        public decimal Topay { get; set; }
        public decimal Topayamount { get; set; }
        public string BenificeryName { get; set; }
        public string BenificeryAccount { get; set; }
        public string Bank { get; set; }
        public decimal DPMamount { get; set; }
        public int IcurrencyId { get; set; }
        public int AccountId { get; set; }
    }

}