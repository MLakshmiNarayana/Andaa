using Focus.Common.DataStructs;
using Newtonsoft.Json.Linq;
using PrjAndaa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using PrjAndaa.TransactionService;
using System.Globalization;
using System.Web.Configuration;
using System.Collections;
using Newtonsoft.Json;

namespace PrjAndaa.Controllers
{
    public class DefinepaymentController : Controller
    {
        DAL DALObj = new DAL();
        public string ServerIp = "localhost";
        int k = 0;
        string sTmp = "";
        string cell = "";
        string date = "";
        string attchment = "";
        string iVal = "0";
        string duedt = "";
        string fDate = "";
        string tDate = "";
        public int Timeout = 60000;
        string attachment = "";
        string strServer = WebConfigurationManager.AppSettings["IpAddress"];
        // GET: Definepayment
        public class HashData
        {
            public string url { get; set; }
            public List<Hashtable> data { get; set; }
            public int result { get; set; }
            public string message { get; set; }
            public List<Hashtable> Data { get; internal set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Definepayment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetPayRpt(string CompId, string EffFromDt, string EffToDt, int Flag)
        {
            List<ClsReconciled> DefReconciled = new List<ClsReconciled>();
            List<ClsPrepaid> DefPrepaid = new List<ClsPrepaid>();
            List<ClsRecurring> DefRecurring = new List<ClsRecurring>();
            string fDate = DateTime.ParseExact(EffFromDt, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string tDate = DateTime.ParseExact(EffToDt, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (Flag == 0)
            {
                DAL.SetLog("GetPayRpt");
                string Criteria = string.Empty;
                string error = string.Empty;
                string BodyIds = string.Empty;

                string sMySql = @"SELECT ROW_NUMBER() over(order by t.[voucher no] asc) as [Row_No], * from (select distinct acc.sName [Vendor Name], acc2.sName [Expense],   v.sAbbr + ':' + h.sVoucherNo[Voucher No],'' [Last Update], 0 [Last Payment], 'Rent_Term' [Rent Term],  DATEADD(dd, acc.iCreditDays, dbo.IntToDate(h.iDate) ) [Due Date], " +
                                    " ABS(d.mAmount1)[Due Amount], CAST(1 AS BIT)[Select], 100[To Pay], ABS(d.mAmount1)[To Pay Amount],  acc.sBankAccountName  [Benificiary Name], acc.sBankAccountNumber [Beneficiary Account No], acc.BankName[Bank Name], " +
                                    " dbo.fSM_GetDPM_Amount( v.sAbbr + ':' + h.sVoucherNo)DPM_Amount, cur.iCurrencyId, acc2.iMasterId AccountId FROM dbo.tCore_Data_0 d  with(ReadUncommitted)" +
                                    " JOIN dbo.tCore_Header_0 h   with(ReadUncommitted)  ON h.iHeaderId = d.iHeaderId" +
                                    " JOIN dbo.cCore_Vouchers_0 v  with(ReadUncommitted) ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'PPMV'" +
                                    " JOIN dbo.tCore_Data_FX_0 dx  with(ReadUncommitted) ON dx.iBodyId = d.iBodyId" +
                                    " Join dbo.tCore_Data4870_0 ub with(ReadUncommitted) ON  ub.iBodyId = d.IBodyId" +
                                    " JOIN tCore_HeaderData4870_0 uh with(ReadUncommitted) ON uh.iHeaderId = h.iHeaderId" +
                                    " JOIN dbo.vmCore_Account acc  with(ReadUncommitted) ON acc.iMasterId = d.iBookNo" +
                                    " JOIN dbo.vmCore_Account acc2 ON acc2.iMasterId = d.iCode" +
                                    " JOIN dbo.mCore_Currency cur ON cur.iCurrencyId = dx.iCurrencyId" +
                                    " WHERE d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0, 1)" +
                                    " AND h.iDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')" +
                                    " AND v.sAbbr + ':' + h.sVoucherNo NOT IN(SELECT DISTINCT IsNull(uh.Ref_No, '') FROM dbo.tCore_Data_0 d  with(ReadUncommitted) " +
                                    " JOIN dbo.tCore_Header_0 h  with(ReadUncommitted) ON h.iHeaderId = d.iHeaderId " +
                                    " JOIN dbo.cCore_Vouchers_0 v  with(ReadUncommitted) ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'DPM' " +
                                    " JOIN dbo.tCore_HeaderData4866_0  uh  with(ReadUncommitted) ON uh.iHeaderId = h.iHeaderId) " +
                                    " AND d.iSerialNo = 0)t";
                DataSet ds = DALObj.GetData(sMySql, Convert.ToInt32(CompId), ref error);
                if (ds == null)
                {
                    //return
                }
                if (ds.Tables.Count == 0)
                {
                    //return
                }
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClsPrepaid def = new ClsPrepaid();

                        def.Rownumber = Convert.ToInt32(ds.Tables[0].Rows[i]["Row_No"].ToString());
                        def.VendorName = ds.Tables[0].Rows[i]["Vendor Name"].ToString();
                        def.Expense = ds.Tables[0].Rows[i]["Expense"].ToString();
                        def.VoucherNo = (ds.Tables[0].Rows[i]["Voucher No"].ToString());
                        cell = ds.Tables[0].Rows[i]["Last Update"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Lastupdate = sTmp;// ds.Tables[0].Rows[i]["Last Update"].ToString();
                        // def.Stockbalance =Convert.ToInt32(ds.Tables[0].Rows[i]["Stock Balance"].ToString());
                        def.RentTerm = ds.Tables[0].Rows[i]["Rent Term"].ToString();
                        cell = ds.Tables[0].Rows[i]["Due Date"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Duedate = sTmp;// (ds.Tables[0].Rows[i]["Due Date"].ToString());
                        def.Dueamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["Due Amount"].ToString())));
                        def.Select = Convert.ToBoolean(ds.Tables[0].Rows[i]["Select"].ToString());
                        def.Topay = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay"].ToString())));
                        def.Topayamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay Amount"].ToString())));
                        def.BenificeryName = ds.Tables[0].Rows[i]["Benificiary Name"].ToString();
                        def.BenificeryAccount = ds.Tables[0].Rows[i]["Beneficiary Account No"].ToString();
                        def.Bank = ds.Tables[0].Rows[i]["Bank Name"].ToString();
                        //def.iDate=Convert.ToDateTime(ds.Tables[0].Rows[i]["iDate"].ToString());
                        def.DPMamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["DPM_Amount"].ToString())));
                        def.IcurrencyId = Convert.ToInt32(ds.Tables[0].Rows[i]["iCurrencyId"].ToString());
                        def.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());

                        DefPrepaid.Add(def);
                    }


                    return Json(DefPrepaid, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            if (Flag == 1)
            {
                DAL.SetLog("GetPayRpt");
                string Criteria = string.Empty;
                string error = string.Empty;
                string BodyIds = string.Empty;
                string sMySql = @"Select ROW_NUMBER() over(order by [Voucher No] asc) as [Row_No], * from ( Select distinct [Account], [Expense], [Voucher No], [Last Update], [Last Payment], [Bill Date], [Due Date], " +
                 "  [Due Amount],  [Select], [To Pay], [To Pay Amount],  [Beneficiary Name], [Beneficiary Account Number] [Beneficiary Account No], [Bank] [Bank Name], dbo.fSM_GetDPM_Amount([Voucher No])DPM_Amount, iCurrencyId, AccountId  from ( " +
                 " SELECT  acc.sName[Account], acc2.sName[Expense], v.sAbbr + ':' + h.sVoucherNo[Voucher No], ''[Last Update], 0[Last Payment], " +
                 "  dbo.IntToDate(uh.Bill_Date)[Bill Date], DATEADD(dd, acc.iCreditDays, dbo.IntToDate(h.iDate))[Due Date], " +
                 "  ABS(d.mAmount1)[Due Amount], 100[To Pay],ABS(d.mAmount1)[To Pay Amount], CAST(0 AS BIT)[Select], acc.sBankAccountName [Beneficiary Name], acc.sBankAccountNumber[Beneficiary Account Number], acc.BankName[Bank], cur.iCurrencyId, acc2.iMasterId AccountId FROM dbo.tCore_Data_0 d " +
                 "  JOIN dbo.tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                 "  JOIN dbo.cCore_Vouchers_0 v ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'PtyCsh' " +
                 "  JOIN dbo.tCore_Data_FX_0 dx ON dx.iBodyId = d.iBodyId " +
                 "  JOIN tCore_HeaderData5120_0 uh ON uh.iHeaderId = h.iHeaderId " +
                 "  JOIN dbo.vmCore_Account acc ON acc.iMasterId = d.iBookNo " +
                 "  JOIN dbo.vmCore_Account acc2 ON acc2.iMasterId = d.iCode " +
                 "  JOIN dbo.mCore_Currency cur ON cur.iCurrencyId = dx.iCurrencyId " +
                 "  WHERE  d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0, 1) " +
                 " AND h.iDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')" +
                 " AND d.iSerialNo = 0" +
                 " Union All " +
                 " SELECT  acc.sName[Account], acc2.sName[Expense], v.sAbbr + ':' + h.sVoucherNo[Voucher No], ''[Last Update], 0[Last Payment],  " +
                 " ''[Bill Date], DATEADD(dd, acc.iCreditDays, dbo.IntToDate(h.iDate))[Due Date], " +
                 " ABS(d.mAmount1)[Due Amount], 100[To Pay], ABS(d.mAmount1)[To Pay Amount], CAST(0 AS BIT)[Select], acc.sBankAccountName [Beneficiary Name], acc.sBankAccountNumber[Beneficiary Account Number], acc.BankName[Bank], cur.iCurrencyId, acc2.iMasterId AccountId      FROM dbo.tCore_Data_0 d " +
                 " JOIN dbo.tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                 " JOIN dbo.cCore_Vouchers_0 v ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'PMFLA' " +
                 " JOIN dbo.tCore_Data_FX_0 dx ON dx.iBodyId = d.iBodyId " +
                 " JOIN tCore_HeaderData4868_0 uh ON uh.iHeaderId = h.iHeaderId " +
                 " JOIN dbo.vmCore_Account acc ON acc.iMasterId = d.iBookNo " +
                 " JOIN dbo.vmCore_Account acc2 ON acc2.iMasterId = d.iCode " +
                 " JOIN dbo.mCore_Currency cur ON cur.iCurrencyId = dx.iCurrencyId " +
                 " WHERE   uh.Type_Of_Document = 1 and d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0, 1) " + //ADVANCES
                 " AND h.iDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')" +
                 " AND d.iSerialNo = 0 " +
                  " Union All " +
                 " SELECT  acc.sName[Account], acc2.sName[Expense], v.sAbbr + ':' + h.sVoucherNo[Voucher No], ''[Last Update], 0[Last Payment], " +
                 " ''[Bill Date], DATEADD(dd, acc.iCreditDays, dbo.IntToDate(h.iDate))[Due Date], " +
                 " ABS(d.mAmount1)[Due Amount], 100[To Pay], ABS(d.mAmount1)[To Pay Amount],CAST(0 AS BIT)[Select], acc.sBankAccountName [Beneficiary Name], acc.sBankAccountNumber[Beneficiary Account Number], acc.BankName[Bank] , cur.iCurrencyId , acc2.iMasterId AccountId    FROM dbo.tCore_Data_0 d " +
                 " JOIN dbo.tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                 " JOIN dbo.cCore_Vouchers_0 v ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'PMFLA' " +
                 " JOIN dbo.tCore_Data_FX_0 dx ON dx.iBodyId = d.iBodyId " +
                 " JOIN tCore_HeaderData4868_0 uh ON uh.iHeaderId = h.iHeaderId " +
                 " JOIN dbo.vmCore_Account acc ON acc.iMasterId = d.iBookNo " +
                 " JOIN dbo.vmCore_Account acc2 ON acc2.iMasterId = d.iCode " +
                 " JOIN dbo.mCore_Currency cur ON cur.iCurrencyId = dx.iCurrencyId " +
                 " WHERE   uh.Type_Of_Document = 0 and d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0, 1) " + // LOAN
                 " AND h.iDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')" +
                 " AND d.iSerialNo = 0 " +
                   " Union All " +
                 " SELECT  acc.sName[Account], acc2.sName[Expense], v.sAbbr + ':' + h.sVoucherNo[Voucher No], ''[Last Update], 0[Last Payment], " +
                 " ''[Bill Date], DATEADD(dd, acc.iCreditDays, dbo.IntToDate(h.iDate))[Due Date], " +
                 " ABS(d.mAmount1)[Due Amount], 100[To Pay], ABS(d.mAmount1)[To Pay Amount], CAST(0 AS BIT)[Select], acc.sBankAccountName [Beneficiary Name], acc.sBankAccountNumber[Beneficiary Account Number], acc.BankName[Bank] , cur.iCurrencyId , acc2.iMasterId AccountId    FROM dbo.tCore_Data_0 d " +
                 " JOIN dbo.tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                 " JOIN dbo.cCore_Vouchers_0 v ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'CreNTL' " +
                 " JOIN dbo.tCore_Data_FX_0 dx ON dx.iBodyId = d.iBodyId " +
                 " JOIN dbo.vmCore_Account acc ON acc.iMasterId = d.iBookNo " +
                 " JOIN dbo.vmCore_Account acc2 ON acc2.iMasterId = d.iCode " +
                 " JOIN dbo.mCore_Currency cur ON cur.iCurrencyId = dx.iCurrencyId " +
                 " WHERE  d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0, 1) " + // Salary
                 " AND h.iDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')" +
                 " AND d.iSerialNo = 0 ) sm)sm1 where DPM_Amount <> [Due Amount]";
                DataSet ds = DALObj.GetData(sMySql, Convert.ToInt32(CompId), ref error);
                if (ds == null)
                {
                    //return
                }
                if (ds.Tables.Count == 0)
                {
                    //return
                }
                // var data = new List<ClsReconciled>();
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClsRecurring def = new ClsRecurring();

                        def.Rownumber = Convert.ToInt32(ds.Tables[0].Rows[i]["Row_No"].ToString());
                        def.Account = ds.Tables[0].Rows[i]["Account"].ToString();
                        def.Expense = ds.Tables[0].Rows[i]["Expense"].ToString();
                        def.VoucherNo = (ds.Tables[0].Rows[i]["Voucher No"].ToString());
                        cell = ds.Tables[0].Rows[i]["Last Update"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }

                        def.Lastupdate = sTmp;// ds.Tables[0].Rows[i]["Last Update"].ToString();
                        // def.Stockbalance =Convert.ToInt32(ds.Tables[0].Rows[i]["Stock Balance"].ToString());
                        cell = ds.Tables[0].Rows[i]["Bill Date"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.BillDate = sTmp;// ds.Tables[0].Rows[i]["Bill Date"].ToString();
                        cell = ds.Tables[0].Rows[i]["Due Date"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Duedate = sTmp;// ds.Tables[0].Rows[i]["Due Date"].ToString();
                        def.Dueamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["Due Amount"].ToString())));
                        def.Select = Convert.ToBoolean(ds.Tables[0].Rows[i]["Select"].ToString());
                        def.Topay = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay"].ToString())));
                        def.Topayamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay Amount"].ToString())));
                        def.BenificeryName = ds.Tables[0].Rows[i]["Beneficiary Name"].ToString();
                        def.BenificeryAccount = ds.Tables[0].Rows[i]["Beneficiary Account No"].ToString();
                        def.Bank = ds.Tables[0].Rows[i]["Bank Name"].ToString();
                        //def.iDate=Convert.ToDateTime(ds.Tables[0].Rows[i]["iDate"].ToString());
                        def.DPMamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["DPM_Amount"].ToString())));
                        def.IcurrencyId = Convert.ToInt32(ds.Tables[0].Rows[i]["iCurrencyId"].ToString());
                        def.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());

                        DefRecurring.Add(def);
                    }


                    return Json(DefRecurring, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (Flag == 2)
            {
                DAL.SetLog("GetPayRpt");
                string Criteria = string.Empty;
                string error = string.Empty;
                string BodyIds = string.Empty;
                if (Flag == 1) { Criteria = "(C.DefReconciled IS NULL OR C.DefReconciled=1)"; }
                if (Flag == 2) { Criteria = "C.DefReconciled=1"; }
                if (Flag == 3) { Criteria = "C.DefReconciled IS NULL"; }
                // int i = 0;
                string qry = @"Select distinct * from ( SELECT ROW_NUMBER() over(order by [Purchase Invoice] asc) as [Row_No], [Vendor Name], [Purchase Invoice], [Received Date],  [Stock Balance],  [Image / Photo], [Due Date],sum([Due Amount]) [Due Amount],[Select], [Currency],  [Vendor Stat/ Attach], [Last Update], [To Pay],sum([Due Amount]) [To Pay Amount],[Beneficiary Name],  [Beneficiary Account Number] [Beneficiary Account No],   [Bank] [Bank Name] ,  " +
                   "iDate, dbo.fSM_GetDPM_Amount([Purchase Invoice])DPM_Amount, iCurrencyId, AccountId, '' s64_ImagePhoto, '' s64_VendorAttach, [Acc Payment Mode]  FROM (  SELECT  1 [Sno], dbo.IntToDate(h.iDate)vDate, h.iDate, d.iDueDate, acc.sName [Vendor Name], v.sAbbr + ':' + h.sVoucherNo [Purchase Invoice], '' [Received Date], '' [Stock Balance], '' [Image / Photo], " +
                   "dbo.IntToDate(d.iDueDate) [Due Date],   ABS(dx.mFXAmount1) [Due Amount], cur.sCode [Currency], acc.iMasterId AccountId  ,   0 [Vendor Stat/ Attach], dbo.fSM_Get_LastUpdate(acc.iMasterId) [Last Update], CAST( 0 AS BIT) [Select], 100 [To Pay], 'add Listcolumn' [Payment Mode], acc.PaymentMode1 [Acc Payment Mode],  acc.sBankAccountName [Beneficiary Name]," +
                   "acc.sBankAccountNumber [Beneficiary Account Number], acc.BankName [Bank], cur.iCurrencyId  FROM dbo.tCore_Data_0 d  with (ReadUncommitted)  JOIN dbo.tCore_Header_0 h  with (ReadUncommitted) ON h.iHeaderId = d.iHeaderId  JOIN dbo.cCore_Vouchers_0 v  with (ReadUncommitted) ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'LOCPO' " +
                   "JOIN dbo.tCore_Data_FX_0 dx with (ReadUncommitted)  ON dx.iBodyId = d.iBodyId  JOIN tCore_HeaderData2560_0 uh  with (ReadUncommitted) ON uh.iHeaderId = h.iHeaderId  JOIN dbo.vmCore_Account acc  with (ReadUncommitted) ON acc.iMasterId = d.iBookNo  and acc.iTreeId = 0 JOIN dbo.mCore_Currency cur  with (ReadUncommitted) ON cur.iCurrencyId = dx.iCurrencyId " +
                   "WHERE uh.PO_Type = 0 AND d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0,1)    And ABS(dx.mFXAmount1) <> 0   UNION ALL   SELECT  2 [Sno], dbo.IntToDate(h.iDate)vDate, h.iDate, d.iDueDate, acc.sName [Vendor Name], v.sAbbr + ':' + h.sVoucherNo [Purchase Invoice], '' [Received Date], '' [Stock Balance], '' [Image / Photo]," +
                   "dbo.IntToDate(d.iDueDate)  [Due Date],   ABS(dx.mFXAmount1) [Due Amount], cur.sCode [Currency], acc.iMasterId AccountId  ,  0 [Vendor Stat/ Attach], dbo.fSM_Get_LastUpdate(acc.iMasterId) [Last Update], CAST( 0 AS BIT) [Select], 100 [To Pay], 'add Listcolumn' [Payment Mode], acc.PaymentMode1 [Acc Payment Mode],  acc.sBankAccountName [Beneficiary Name], " +
                   "acc.sBankAccountNumber [Beneficiary Account Number], acc.BankName [Bank], cur.iCurrencyId     FROM dbo.tCore_Data_0 d  with (ReadUncommitted)  JOIN dbo.tCore_Header_0 h  with (ReadUncommitted) ON h.iHeaderId = d.iHeaderId  JOIN dbo.cCore_Vouchers_0 v  with (ReadUncommitted) ON v.iVoucherType =h.iVoucherType AND v.sAbbr = 'IMPPO'  " +
                   "JOIN dbo.tCore_Data_FX_0 dx  with (ReadUncommitted) ON dx.iBodyId = d.iBodyId  JOIN tCore_HeaderData2564_0 uh with (ReadUncommitted)   ON uh.iHeaderId = h.iHeaderId  JOIN dbo.vmCore_Account acc  with (ReadUncommitted) ON acc.iMasterId = d.iBookNo  and acc.iTreeId = 0 JOIN dbo.mCore_Currency cur " +
                   "with (ReadUncommitted) ON cur.iCurrencyId = dx.iCurrencyId  WHERE uh.PO_Type = 0 AND d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0,1) And ABS(dx.mFXAmount1) <> 0  UNION ALL " +
                   "SELECT  3 [Sno], dbo.IntToDate(h.iDate)vDate, h.iDate, d.iDueDate, acc.sName [Vendor Name], v.sAbbr + ':' + h.sVoucherNo [Purchase Invoice], " +
                   "dbo.IntToDate(h.iDate) [Received Date], '' [Stock Balance], '' [Image / Photo], dbo.IntToDate(d.iDueDate)  [Due Date],   ABS(dx.mFXAmount1) [Due Amount], cur.sCode [Currency], " +
                   "acc.iMasterId AccountId  ,  0 [Vendor Stat/ Attach], dbo.fSM_Get_LastUpdate(acc.iMasterId) [Last Update], CAST( 0 AS BIT) [Select], 100 [To Pay], 'add Listcolumn' [Payment Mode]," +
                   "acc.PaymentMode1[Acc Payment Mode], acc.sBankAccountName[Beneficiary Name], acc.sBankAccountNumber[Beneficiary Account Number], acc.BankName[Bank], " +
                    "cur.iCurrencyId     FROM dbo.tCore_Data_0 d  with (ReadUncommitted)  JOIN dbo.tCore_Header_0 h  with (ReadUncommitted) ON h.iHeaderId = d.iHeaderId  JOIN dbo.cCore_Vouchers_0 v " +
                    "with (ReadUncommitted) ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'LOCPV'  JOIN dbo.tCore_Data_FX_0 dx with (ReadUncommitted)  ON dx.iBodyId = d.iBodyId  " +
                    "JOIN tCore_HeaderData768_0 uh with (ReadUncommitted)   ON uh.iHeaderId = h.iHeaderId  JOIN dbo.vmCore_Account acc  with (ReadUncommitted) ON acc.iMasterId = d.iBookNo " +
                    "and acc.iTreeId = 0 JOIN dbo.mCore_Currency cur  with (ReadUncommitted) ON cur.iCurrencyId = dx.iCurrencyId  WHERE uh.PO_Type = 1 AND d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 " +
                    "AND d.iAuthStatus IN(0,1)    And ABS(dx.mFXAmount1) <> 0   UNION ALL " +
                    "SELECT  4 [Sno], dbo.IntToDate(h.iDate)vDate, h.iDate, d.iDueDate, acc.sName [Vendor Name], v.sAbbr + ':' + h.sVoucherNo [Purchase Invoice]," +
                    "dbo.IntToDate(h.iDate) [Received Date], '' [Stock Balance], '' [Image / Photo], dbo.IntToDate(d.iDueDate)  [Due Date],   ABS(dx.mFXAmount1) [Due Amount], cur.sCode [Currency], acc.iMasterId AccountId  ,  0 [Vendor Stat/ Attach], " +
                    "dbo.fSM_Get_LastUpdate(acc.iMasterId) [Last Update], CAST( 0 AS BIT) [Select], 100 [To Pay], 'add Listcolumn' [Payment Mode], acc.PaymentMode1 [Acc Payment Mode],  acc.sBankAccountName [Beneficiary Name], acc.sBankAccountNumber [Beneficiary Account Number]," +
                    "acc.BankName [Bank], cur.iCurrencyId     FROM dbo.tCore_Data_0 d  with (ReadUncommitted)  JOIN dbo.tCore_Header_0 h with (ReadUncommitted)  ON h.iHeaderId = d.iHeaderId  JOIN dbo.cCore_Vouchers_0 v  with (ReadUncommitted) ON v.iVoucherType = h.iVoucherType AND v.sAbbr = 'IMPPV' " +
                    "JOIN dbo.tCore_Data_FX_0 dx  with (ReadUncommitted) ON dx.iBodyId = d.iBodyId  JOIN tCore_HeaderData769_0 uh ON uh.iHeaderId = h.iHeaderId  JOIN dbo.vmCore_Account acc  with (ReadUncommitted) ON acc.iMasterId = d.iBookNo and acc.iTreeId = 0  JOIN dbo.mCore_Currency cur " +
                    "with (ReadUncommitted)  ON cur.iCurrencyId = dx.iCurrencyId  WHERE uh.PO_Type = 1 AND d.bSuspendUpdateFA <> 1 AND h.bSuspended = 0 AND d.iAuthStatus IN(0,1)    And ABS(dx.mFXAmount1) <> 0 ) sm " +
                    "where sm.iDueDate BETWEEN dbo.DateToInt('" + fDate + "') AND dbo.DateToInt('" + tDate + "')  group by [Vendor Name], [Purchase Invoice], [Received Date],  [Stock Balance],  [Image / Photo], [Due Date], [Currency],  [Vendor Stat/ Attach], [Last Update],[Select], [To Pay],[Beneficiary Name],  [Beneficiary Account Number] ,   [Bank] ,  iDate, iCurrencyId, AccountId, [Acc Payment Mode]  ) sm1 where DPM_Amount <> [Due Amount] ";

                DataSet ds = DALObj.GetData(qry, Convert.ToInt32(CompId), ref error);
                if (ds == null)
                {
                    //return
                }
                if (ds.Tables.Count == 0)
                {
                    //return
                }
                // var data = new List<ClsReconciled>();
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClsReconciled def = new ClsReconciled();
                        def.Rownumber = Convert.ToInt32(ds.Tables[0].Rows[i]["Row_No"].ToString());
                        def.VendorName = ds.Tables[0].Rows[i]["Vendor Name"].ToString();
                        def.PurchseInv = ds.Tables[0].Rows[i]["Purchase Invoice"].ToString();
                        cell = ds.Tables[0].Rows[i]["Received Date"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Receiveddate = sTmp;// (ds.Tables[0].Rows[i]["Received Date"].ToString());
                        def.Stockbalance = Convert.ToInt32(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Stock Balance"].ToString()) ? "0" : ds.Tables[0].Rows[i]["Stock Balance"].ToString());
                        // def.Stockbalance =Convert.ToInt32(ds.Tables[0].Rows[i]["Stock Balance"].ToString());
                        def.image = ds.Tables[0].Rows[i]["Image / Photo"].ToString();
                        cell = ds.Tables[0].Rows[i]["Due Date"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Duedate = sTmp;// (ds.Tables[0].Rows[i]["Due Date"].ToString());
                        def.Dueamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["Due Amount"].ToString())));
                        def.select = Convert.ToBoolean(ds.Tables[0].Rows[i]["Select"].ToString());
                        def.Currency = ds.Tables[0].Rows[i]["Currency"].ToString();
                        cell = Convert.ToString(ds.Tables[0].Rows[i]["Vendor Stat/ Attach"].ToString());
                        if (cell == "0")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            sTmp = ds.Tables[0].Rows[i]["Vendor Stat/ Attach"].ToString();
                        }
                        def.Vendorattachment = sTmp;
                        cell = ds.Tables[0].Rows[i]["Last Update"].ToString();
                        if (cell == "" || cell == "01-01-1900 00:00:00" || cell == "1900-01-01 00:00:00")
                        {
                            sTmp = "";
                        }
                        else
                        {
                            date = DateTime.ParseExact(cell, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            sTmp = date;
                            sTmp = (cell != null && cell != "") ? sTmp : "";
                        }
                        def.Lastupdate = sTmp;// ds.Tables[0].Rows[i]["Last Update"].ToString();
                        def.Topay = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay"].ToString())));
                        def.Topayamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["To Pay Amount"].ToString())));
                        def.BenificeryName = ds.Tables[0].Rows[i]["Beneficiary Name"].ToString();
                        def.BenificeryAccount = ds.Tables[0].Rows[i]["Beneficiary Account No"].ToString();
                        def.Bank = ds.Tables[0].Rows[i]["Bank Name"].ToString();
                        //def.iDate=Convert.ToDateTime(ds.Tables[0].Rows[i]["iDate"].ToString());
                        def.DPMamount = Convert.ToDecimal(string.Format("{0:##.00}", Convert.ToDecimal(ds.Tables[0].Rows[i]["DPM_Amount"].ToString())));
                        def.IcurrencyId = Convert.ToInt32(ds.Tables[0].Rows[i]["iCurrencyId"].ToString());
                        def.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());
                        def.s64image = ds.Tables[0].Rows[i]["s64_ImagePhoto"].ToString();
                        def.s64vendorattch = ds.Tables[0].Rows[i]["s64_VendorAttach"].ToString();
                        //def.Accpaymode = ds.Tables[0].Rows[i]["Acc Payment Mode"].ToString();
                        def.Accpaymode = ds.Tables[0].Rows[k]["Acc Payment Mode"].ToString();
                        DefReconciled.Add(def);
                    }
                    return Json(DefReconciled, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return Json(DefReconciled, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Accpaymentmodes(string CompId,string ddlitem)
        {
            string error = string.Empty;
            List<ClsReconciled> obj = new List<ClsReconciled>();
            string qry = @"select iMasterId,SName,sCode from mCore_PaymentMode where sName not in('"+ddlitem+"') and iMasterId>0";
            DataSet ds = DALObj.GetData(qry, Convert.ToInt32(CompId), ref error);
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                ClsReconciled def = new ClsReconciled();
                def.AllAccpaymentmodes = ds.Tables[0].Rows[k]["SName"].ToString();
                def.paymentmodeid = Convert.ToInt32(ds.Tables[0].Rows[k]["iMasterId"].ToString());
                obj.Add(def);
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveReconciled(string CompId, string SessionId, List<ClsReconciled> checkeddetailsarray)
        {
            string Msg1 = "";
            DateTime dt = DateTime.Now;
            string date = string.Format("{0:yyyy-MM-dd}", dt);
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //string date = Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
            //DateTime dat = DateTime.ParseExact(date, "yyyy-MM-dd", culture);
            //duedt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int strdate = DAL.GetDateToInt(Convert.ToDateTime(date.ToString()));
            FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + " " + DateTime.Now);
            try
            {
           
                _MTrans objMTrans = new _MTrans();
                Boolean blnPost = false;

                if (checkeddetailsarray == null)
                {
                    return Json(new { status = false, message = "please select atleast one Record" });
                }
                //int strdate = DAL.GetDateToInt(System.DateTime.Now);
                foreach (var items in checkeddetailsarray)
                {
                    cell = items.Duedate;
                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }

                    List<PrjAndaa.TransactionService.IdNameTag> lstHeaderData = new List<PrjAndaa.TransactionService.IdNameTag>();
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "DocNo", Value = "1" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Date", Value = strdate });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "CashBankAC", Value = "4" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payment_Type", Value = "Reconciled" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Recieving_Date", Value = sTmp });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Ref_No", Value = items.PurchseInv });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Currency", Value = items.IcurrencyId });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payament_Status", Value = "paid" });
                    objMTrans.arrHeader = lstHeaderData.ToArray();
                    string[] arrBodyNames = new string[12];
                    arrBodyNames[0] = "Account";
                    arrBodyNames[1] = "Amount";
                    arrBodyNames[2] = "Payment_Mode";
                    arrBodyNames[3] = "Due_Date";
                    arrBodyNames[4] = "To_Pay";
                    arrBodyNames[5] = "Benificiary_Name";
                    arrBodyNames[6] = "Benificiary_Account_No";
                    arrBodyNames[7] = "Bank_Name";
                    arrBodyNames[8] = "Due_Amount";
                    arrBodyNames[9] = "Last_Update";
                    arrBodyNames[10] = "Image___Photo";
                    arrBodyNames[11] = "Vendor_Statement___Quotation";
                    objMTrans.arrBodyNames = arrBodyNames;
                    objMTrans.arrBody = new object[1][];
                    object[] arrBodyValue1 = new object[12];
                    arrBodyValue1 = new object[12];
                    var accid = items.AccountId;
                    var sacTmp = (accid != 0) ? items.AccountId : 19;
                    arrBodyValue1[0] = Convert.ToString(sacTmp);
                    arrBodyValue1[1] = Convert.ToString(items.Topayamount);

                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Accpaymode) : "";
                    if (items.Accpaymode == "Cash")
                    {
                        iVal = "0";
                    }
                    if (items.Accpaymode == "E-Transfer")
                    {
                        iVal = "1";
                    }
                    if (items.Accpaymode == "TT")
                    {
                        iVal = "2";
                    }
                    if (items.Accpaymode == "Cheque")
                    {
                        iVal = "3";
                    }
                    if (items.Accpaymode == "Rawdah BR.10")
                    {
                        iVal = "4";
                    }
                    if (items.Accpaymode == "Sadad")
                    {
                        iVal = "5";
                    }
                    else
                    {
                        iVal = "0";
                    }
                    arrBodyValue1[2] = iVal;
                    cell = items.Duedate;
                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }
                    arrBodyValue1[3] = sTmp;
                    cell = Convert.ToString(items.Topay);
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Topay) : "";
                    arrBodyValue1[4] = sTmp;
                    cell = items.BenificeryName;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryName) : "";
                    arrBodyValue1[5] = sTmp; // 

                    cell = items.BenificeryAccount;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryAccount) : "";
                    arrBodyValue1[6] = sTmp; // [Beneficiary Account No]

                    cell = items.Bank;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Bank) : "";
                    arrBodyValue1[7] = sTmp; // 

                    arrBodyValue1[8] = Convert.ToString(items.Dueamount); // Amount

                    cell = items.Lastupdate;
                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }
                    arrBodyValue1[9] = sTmp;

                    object[] obj = new object[2];


                    cell = items.image;

                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.image) : "";

                    attachment = items.s64image;
                    if (sTmp != "")
                    {
                        obj[0] = cell;
                        obj[1] = attachment;
                        arrBodyValue1[10] = obj;
                    }
                    else { arrBodyValue1[10] = ""; obj = null; }
                    //arrBodyValue1[10] = "";

                    object[] obj1 = new object[2];
                    cell = items.Vendorattachment;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Vendorattachment) : "";

                     attachment = items.s64vendorattch;

                    if (sTmp != "")
                    {
                        obj1[0] = (items.Vendorattachment);
                        obj1[1] = attachment;
                        arrBodyValue1[11] = obj1;
                    }
                    else { arrBodyValue1[11] = ""; obj1 = null; }
                    //arrBodyValue1[11] = "";

                    objMTrans.arrBody[0] = arrBodyValue1;
                    blnPost = true;
                    k = k + 1;
                    if (blnPost)
                    {
                        JObject jsonObj = new JObject();
                        jsonObj = new JObject();
                        jsonObj.Add("objMTrans", JToken.FromObject(objMTrans));//Add parameter
                        jsonObj.Add("iVoucherType", JToken.FromObject(4866));//Add parameter

                        jsonObj.Add("sDocNo", "");
                        //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Json Data " + jsonObj.ToString());

                        jsonObj.Add("bByIds", false);
                        jsonObj.Add("bIsNew", true);
                        //MessageBox.Show("Before Save voucher -1");
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("fsessionid", SessionId); // set the SessionId in Header
                            client.Headers.Add("Content-Type", "application/json");// set the content_Type to header for json format data posting
                            string pathStr = strServer + "/Focus8Library/TransactionService.svc/SaveVoucher2";
                            string arrResponse = client.UploadString(pathStr, jsonObj.ToString());// set the url and data to post
                                                                                                  //MessageBox.Show(jsonObj.ToString()

                            SaveResult.saveresult.RootObject MObjRoot = new SaveResult.saveresult.RootObject();
                            //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Response " + arrResponse);
                            MObjRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveResult.saveresult.RootObject>(arrResponse);
                            blnPost = false;
                            //MessageBox.Show("Before Save voucher -3" + arrResponse);
                            if (MObjRoot.SaveVoucher2Result.arrTransIds != null)
                            {
                                //System.Windows.MessageBox.Show("Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue, "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + "Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue);
                                Msg1 = "Posted Successfully check the Error log for details";
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString(), "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + ": Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString());
                                Msg1 = "Error accured while Posting failed Posting failed Check the Error log";
                            }
                        }
                    }
                }
                //success
            }
            catch (Exception ex)
            {
                Msg1 =ex.Message;
                //fail
            }
            return Json(Msg1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult saveprepaidnewapi(string CompId, string SessionId, List<ClsPrepaid> checkeddetailsarray)
        {
            string msg1 = "";
            HashData objHashResponse = new HashData();
            try
            {
                Boolean blnPost = false;

                if (checkeddetailsarray == null)
                {
                    return Json(new { status = false, message = "please select atleast one Record" });
                }
                foreach (var items in checkeddetailsarray)
                {
                    cell = items.Duedate;

                    string duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    sTmp = (cell != null && cell != "") ? items.Duedate : "";
                    //DateTime date = DateTime.ParseExact(this.sTmp, "yyyy-MM-dd", null);
                    if (sTmp != "")
                        sTmp = DAL.GetStringDateToint_2(duedt);
                    HashData objHashRequest = new HashData();
                    Hashtable objHeader = new Hashtable();
                    objHeader.Add("DocNo", "61");
                    objHeader.Add("Date", sTmp);
                    objHeader.Add("CashBankAC", "4");
                    objHeader.Add("Recieving_Date", sTmp);
                    objHeader.Add("Payment_Type", "Prepaid");
                    objHeader.Add("Ref_No", items.VoucherNo);
                    objHeader.Add("Currency", items.IcurrencyId);
                    List<Hashtable> lstBody = new List<Hashtable>();
                    Hashtable objBody = new Hashtable();
                    var accid = items.AccountId;
                    var sacTmp = (accid != 0) ? items.AccountId : 19;
                    objBody.Add("Account", Convert.ToString(sacTmp));
                    objBody.Add("Amount", Convert.ToDecimal(items.Topayamount));
                    objBody.Add("Payment_Mode", "0");
                    cell = items.Duedate;

                    duedt = DateTime.ParseExact(cell, "dd-MM-yyyy ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd ", CultureInfo.InvariantCulture);
                    sTmp = (cell != null && cell != "") ? items.Duedate : "";
                    //DateTime date = DateTime.ParseExact(this.sTmp, "yyyy-MM-dd", null);
                    if (sTmp != "")
                        sTmp = DAL.GetStringDateToint_2(duedt);
                    objBody.Add("Due_Date", sTmp);
                    cell = items.Lastupdate;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Lastupdate) : "";
                    if (sTmp != "") sTmp = DAL.GetStringDateToint_2(sTmp);
                    objBody.Add("Last_Update", sTmp);
                    objBody.Add("To_Pay", Convert.ToDecimal(items.Topay));
                    cell = items.BenificeryName;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryName) : "";
                    objBody.Add("Benificiary_Name", sTmp);
                    cell = items.BenificeryAccount;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryAccount) : "";
                    objBody.Add("Benificiary_Account_No", sTmp);

                    cell = items.Bank;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Bank) : "";
                    objBody.Add("Bank_Name", sTmp);
                    objBody.Add("Due_Amount", Convert.ToDecimal(items.Dueamount));
                    lstBody.Add(objBody);
                    Hashtable objHash = new Hashtable();
                    objHash.Add("Header", objHeader);
                    objHash.Add("Body", lstBody);
                    List<Hashtable> lstHash = new List<Hashtable>();
                    lstHash.Add(objHash);
                    objHashRequest.Data = lstHash;
                    string sContent = JsonConvert.SerializeObject(objHashRequest);

                    using (var client = new WebClient())
                    {
                        client.Headers.Add("fSessionId", SessionId);
                        client.Headers.Add("Content-Type", "Application/json");
                        string sUrl = strServer+"/8API/Transactions/Define Payment Voucher";
                        string strResponse = client.UploadString(sUrl, sContent);
                        objHashResponse = JsonConvert.DeserializeObject<HashData>(strResponse);
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return Json(objHashResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Saveprepaid(string CompId, string SessionId, List<ClsPrepaid> checkeddetailsarray)
        {
            string Msg1 = "";
              DateTime dt = DateTime.Now;
            string date = string.Format("{0:yyyy-MM-dd}", dt);
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //string date = Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
            //DateTime dat = DateTime.ParseExact(date, "yyyy-MM-dd", culture);
            //duedt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int strdate = DAL.GetDateToInt(Convert.ToDateTime(date.ToString()));
            FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + " " + DateTime.Now);
            try
            {
                _MTrans objMTrans = new _MTrans();
                Boolean blnPost = false;

                if (checkeddetailsarray == null)
                {
                    return Json(new { status = false, message = "please select atleast one Record" });
                }
                //int strdate =DAL.GetDateToInt(System.DateTime.Now);
                foreach (var items in checkeddetailsarray)
                {
                    List<PrjAndaa.TransactionService.IdNameTag> lstHeaderData = new List<PrjAndaa.TransactionService.IdNameTag>();
                    //lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "DocNo", Value = "61" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Date", Value = strdate }); //sTmp
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "CashBankAC", Value = "4" });
                    //lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Branch", Value = "6" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Recieving_Date", Value = strdate });// Rec Date: Posting Default date, as it is null for Prepiad
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payment_Type", Value = "Prepaid" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Ref_No", Value = items.VoucherNo });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Currency", Value = items.IcurrencyId });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payament_Status", Value = "paid" });
                    objMTrans.arrHeader = lstHeaderData.ToArray();
                    string[] arrBodyNames = new string[10];
                    arrBodyNames[0] = "Account";
                    arrBodyNames[1] = "Amount";
                    arrBodyNames[2] = "Payment_Mode";

                    arrBodyNames[3] = "Due_Date";
                    arrBodyNames[4] = "Last_Update";
                    arrBodyNames[5] = "To_Pay";
                    arrBodyNames[6] = "Benificiary_Name";
                    arrBodyNames[7] = "Benificiary_Account_No";
                    arrBodyNames[8] = "Bank_Name";
                    arrBodyNames[9] = "Due_Amount";

                    objMTrans.arrBodyNames = arrBodyNames;
                    objMTrans.arrBody = new object[1][];
                    object[] arrBodyValue1 = new object[10];
                    arrBodyValue1 = new object[10];
                    var accid = items.AccountId;
                    var sacTmp = (accid != 0) ? items.AccountId : 19;
                    arrBodyValue1[0] = Convert.ToString(sacTmp);
                    arrBodyValue1[1] = Convert.ToString(items.Topayamount);
                    arrBodyValue1[2] = "0";
                    cell = items.Duedate;
                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }


                    //DateTime date = DateTime.ParseExact(this.sTmp, "yyyy-MM-dd", null);

                    arrBodyValue1[3] = sTmp;
                    cell = items.Lastupdate;
                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }
                    arrBodyValue1[4] = sTmp;
                    cell = Convert.ToString(items.Topay);
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Topay) : "";
                    arrBodyValue1[5] = sTmp;
                    cell = items.BenificeryName;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryName) : "";
                    arrBodyValue1[6] = sTmp; // 

                    cell = items.BenificeryAccount;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryAccount) : "";
                    arrBodyValue1[7] = sTmp; // [Beneficiary Account No]

                    cell = items.Bank;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Bank) : "";
                    arrBodyValue1[8] = sTmp; // 
                    arrBodyValue1[9] = Convert.ToDecimal(items.Dueamount);
                    objMTrans.arrBody[0] = arrBodyValue1;
                    blnPost = true;
                    if (blnPost)
                    {
                        JObject jsonObj = new JObject();
                        jsonObj = new JObject();
                        jsonObj.Add("objMTrans", JToken.FromObject(objMTrans));//Add parameter
                        jsonObj.Add("iVoucherType", JToken.FromObject(4866));//Add parameter

                        jsonObj.Add("sDocNo", "");
                        //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Json Data " + jsonObj.ToString());

                        jsonObj.Add("bByIds", false);
                        jsonObj.Add("bIsNew", true);
                        //MessageBox.Show("Before Save voucher -1");

                        //  string pathStr = strServer + "/Focus8Library/TransactionService.svc/SaveVoucher2";
                        //HttpWebRequest http = (HttpWebRequest)WebRequest.Create(pathStr);
                        //WebResponse response = http.GetResponse();
                        //MemoryStream stream = response.GetResponseStream();
                        //StreamReader sr = new StreamReader(stream);


                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("fsessionid", SessionId); // set the SessionId in Header
                            client.Headers.Add("Content-Type", "application/json");// set the content_Type to header for json format data posting
                            string pathStr = strServer + "/Focus8Library/TransactionService.svc/SaveVoucher2";
                          //  WebRequest request = (HttpWebRequest)WebRequest.Create(pathStr);
                           // request.Timeout = Timeout;
                            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pathStr);
                            //request.Timeout = 0;
                            //request.ReadWriteTimeout = 10000;
                            //var wresp = (HttpWebResponse)request.GetResponse();

                            string arrResponse = client.UploadString(pathStr, jsonObj.ToString());// set the url and data to post
                                                                                                  //MessageBox.Show(jsonObj.ToString()

                            SaveResult.saveresult.RootObject MObjRoot = new SaveResult.saveresult.RootObject();
                            //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Response " + arrResponse);
                            MObjRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveResult.saveresult.RootObject>(arrResponse);

                            //MessageBox.Show("Before Save voucher -3" + arrResponse);
                            if (MObjRoot.SaveVoucher2Result.arrTransIds != null)
                            {
                                //System.Windows.MessageBox.Show("Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue, "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + "Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue);
                                Msg1 = "Posted Successfully check the Error log for details";
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString(), "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + ": Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString());
                                Msg1 = "Error accured while Posting failed Posting failed Check the Error log";
                            }
                        }

                    }
                }
                //success            
            }
            catch (Exception ex)
            {
                Msg1 = ex.Message;
                //fail
            }
            return Json(Msg1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult saveRecurring(string CompId, string SessionId, List<ClsRecurring> checkeddetailsarray)
        {
            string Msg1 = "";
            DateTime dt = DateTime.Now;
            string date = string.Format("{0:yyyy-MM-dd}", dt);
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //string date = Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
            //DateTime dat = DateTime.ParseExact(date, "yyyy-MM-dd", culture);
            //duedt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int strdate = DAL.GetDateToInt(Convert.ToDateTime(date.ToString()));
            FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + " " + DateTime.Now);
            try
            {
                _MTrans objMTrans = new _MTrans();
                Boolean blnPost = false;

                if (checkeddetailsarray == null)
                {
                    return Json(new { status = false, message = "please select atleast one Record" });
                }
                //int strdate = DAL.GetDateToInt(System.DateTime.Now);
                foreach (var items in checkeddetailsarray)
                {
                    cell = items.Duedate;
                    if (cell != "")
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "")
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (duedt != null && duedt != "") ? duedt : "";
                        //DateTime date = DateTime.ParseExact(this.sTmp, "yyyy-MM-dd", null);
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }
                    List<PrjAndaa.TransactionService.IdNameTag> lstHeaderData = new List<PrjAndaa.TransactionService.IdNameTag>();
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "DocNo", Value = "1" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Date", Value = strdate });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "CashBankAC", Value = "4" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payment_Type", Value = "Recurring" });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Recieving_Date", Value = strdate });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Ref_No", Value = items.VoucherNo });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Currency", Value = items.IcurrencyId });
                    lstHeaderData.Add(new PrjAndaa.TransactionService.IdNameTag { sName = "Payament_Status", Value = "paid" });
                    objMTrans.arrHeader = lstHeaderData.ToArray();
                    string[] arrBodyNames = new string[9];
                    arrBodyNames[0] = "Account";
                    arrBodyNames[1] = "Amount";
                    arrBodyNames[2] = "Payment_Mode";

                    arrBodyNames[3] = "Due_Date";
                    arrBodyNames[4] = "To_Pay";
                    arrBodyNames[5] = "Benificiary_Name";
                    arrBodyNames[6] = "Benificiary_Account_No";
                    arrBodyNames[7] = "Bank_Name";
                    arrBodyNames[8] = "Due_Amount";

                    objMTrans.arrBodyNames = arrBodyNames;
                    objMTrans.arrBody = new object[1][];
                    object[] arrBodyValue1 = new object[9];
                    arrBodyValue1 = new object[9];
                    var accid = items.AccountId;
                    var sacTmp = (accid != 0) ? items.AccountId : 19;
                    arrBodyValue1[0] = sacTmp;
                    arrBodyValue1[1] = items.Topayamount;
                    // Amount
                    //sTmp = (cell != null && cell != "") ? Convert.ToString(items.Accpaymode) : "";
                    //if (items.Accpaymode == "Cash")
                    //{
                    //    iVal = "0";
                    //}
                    //if (items.Accpaymode == "E-Transfer")
                    //{
                    //    iVal = "1";
                    //}
                    //if (items.Accpaymode == "TT")
                    //{
                    //    iVal = "2";
                    //}
                    //if (items.Accpaymode == "Cheque")
                    //{
                    //    iVal = "3";
                    //}
                    //arrBodyValue1[2] = iVal;
                    arrBodyValue1[2] = "0";
                    cell = items.Duedate;

                    if (cell != "" && cell != null)
                    {
                        duedt = Convert.ToDateTime(cell).ToString("yyyy-MM-dd");
                        //duedt = DateTime.ParseExact(cell, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    sTmp = cell;
                    if (cell == "" && cell == null)
                    {
                        sTmp = "";
                    }
                    else
                    {
                        sTmp = (cell != null && cell != "") ? items.Duedate : "";
                        //DateTime date = DateTime.ParseExact(this.sTmp, "yyyy-MM-dd", null);
                        if (sTmp != "")
                            sTmp = DAL.GetStringDateToint_2(duedt);
                    }

                    arrBodyValue1[3] = sTmp;
                    cell = Convert.ToString(items.Topay);
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Topay) : "";
                    arrBodyValue1[4] = sTmp;
                    cell = items.BenificeryName;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryName) : "";
                    arrBodyValue1[5] = sTmp; // 

                    cell = items.BenificeryAccount;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.BenificeryAccount) : "";
                    arrBodyValue1[6] = sTmp; // [Beneficiary Account No]

                    cell = items.Bank;
                    sTmp = (cell != null && cell != "") ? Convert.ToString(items.Bank) : "";
                    arrBodyValue1[7] = sTmp; // 
                    arrBodyValue1[8] = Convert.ToDecimal(items.Dueamount);
                    objMTrans.arrBody[0] = arrBodyValue1;
                    blnPost = true;
                    if (blnPost)
                    {
                        JObject jsonObj = new JObject();
                        jsonObj = new JObject();
                        jsonObj.Add("objMTrans", JToken.FromObject(objMTrans));//Add parameter
                        jsonObj.Add("iVoucherType", JToken.FromObject(4866));//Add parameter

                        jsonObj.Add("sDocNo", "");
                        //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Json Data " + jsonObj.ToString());

                        jsonObj.Add("bByIds", false);
                        jsonObj.Add("bIsNew", true);
                        //MessageBox.Show("Before Save voucher -1");
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("fsessionid", SessionId); // set the SessionId in Header
                            client.Headers.Add("Content-Type", "application/json");// set the content_Type to header for json format data posting
                            string pathStr = strServer + "/Focus8Library/TransactionService.svc/SaveVoucher2";
                            string arrResponse = client.UploadString(pathStr, jsonObj.ToString());// set the url and data to post
                                                                                                  //MessageBox.Show(jsonObj.ToString()

                            SaveResult.saveresult.RootObject MObjRoot = new SaveResult.saveresult.RootObject();
                            //FConvert.LogFile("PostingLDP.log", DateTime.Now.ToString() + " Response " + arrResponse);
                            MObjRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveResult.saveresult.RootObject>(arrResponse);

                            //MessageBox.Show("Before Save voucher -3" + arrResponse);
                            if (MObjRoot.SaveVoucher2Result.arrTransIds != null)
                            {
                                //System.Windows.MessageBox.Show("Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue, "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + "Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue);
                                Msg1 = "Posted Successfully check the Error log for details";
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString(), "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                FConvert.LogFile("AndaaCo.log", DateTime.Now.ToString() + ": Posting Failed" + "\n" + "Reason: " + MObjRoot.SaveVoucher2Result.sValue.ToString());
                                Msg1 = "Error accured while Posting failed Posting failed Check the Error log";
                            }
                        }
                    }
                }
                //success
            }
            catch (Exception ex)
            {
                Msg1 = ex.Message;
                //fail
            }
            return Json(Msg1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getpath(FormCollection formData)
        {

            string base64String = "";
            var files = Request.Files;
                HttpPostedFileBase file = files[0];
                string fname;
                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                // Get the complete folder path and store the file inside it.  
                fname = Path.Combine("C:\\Windows\\TEMP", fname);
                file.SaveAs(fname);

                byte[] imageBytes = System.IO.File.ReadAllBytes(fname);
                base64String = Convert.ToBase64String(imageBytes);
               var result = new { bytes = base64String, filename = fname };
                return Json(result, JsonRequestBehavior.AllowGet);         
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            string filename = System.IO.Path.GetFileName(imgPath);
            string folderName = @"c:\windows\system32";
            string pathString = System.IO.Path.Combine(folderName, "uploadedimages");
            System.IO.Directory.CreateDirectory(pathString);
            pathString = System.IO.Path.Combine(pathString, filename);
            System.IO.FileStream fs = System.IO.File.Create(pathString);
            FileStream fs1 = new FileStream(pathString, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] imageBytes = System.IO.File.ReadAllBytes(pathString);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}