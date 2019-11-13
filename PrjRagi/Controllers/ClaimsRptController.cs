using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrjAndaa.Models;
using System.Data;
using PrjAndaa.TransactionService;
using Newtonsoft.Json.Linq;
using System.Net;
using static PrjAndaa.SaveResult;
using System.Web.Configuration;



namespace PrjAndaa.Controllers
{
    public class ClaimsRptController : Controller
    {
        // GET: ClaimsRpt
        DAL DALObj = new DAL();
        public ActionResult VwClaimsRpt()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetClaimsType(string CompId)
        {
            DAL.SetLog("GetClaimsType");
            List<ClsClaimTypes> ClaimsType = new List<ClsClaimTypes>();
            string error = string.Empty;
            DataSet ds = DALObj.GetData("select iMasterId,sName from mCore_ClaimType where bGroup=0 and iStatus=0 and iMasterId!=0", Convert.ToInt32(CompId), ref error);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ClaimsType.Add(new ClsClaimTypes { id = ds.Tables[0].Rows[i]["iMasterId"].ToString(), name = ds.Tables[0].Rows[i]["sName"].ToString() });
            }
            return Json(ClaimsType);
        }


        [HttpPost]
        public ActionResult GetClaimsRpt(string CompId, string EffFromDt, string EffToDt, int Flag)
        {
            DAL.SetLog("GetClaimsRpt");
            string Criteria = string.Empty;
            string error = string.Empty;
            //int PrevId = 0, CurrId = 0;
           // Decimal TaxableSL = 0, TargetAmt = 0, NetSales = 0, ClaimAmt = 0;
            string BodyIds = string.Empty;
            DateTime FDate = Convert.ToDateTime(EffFromDt);
            DateTime TDate = Convert.ToDateTime(EffToDt);

            List<ClsGetSalesData> SalesData = new List<ClsGetSalesData>();

            if (Flag == 1) { Criteria = "(C.ClaimStatus IS NULL OR C.ClaimStatus=1)"; }
            if (Flag == 2) { Criteria = "C.ClaimStatus=1"; }
            if (Flag == 3) { Criteria = "C.ClaimStatus IS NULL"; }
            int i = 0;
            string Str = "SELECT tCore_Data_0.iBodyId,tCore_Data_0.iBookNo,CustName=(select sName from mCore_Account where iMasterId=tCore_Data_0.iBookNo)," +
            " ISNULL(CAST(tCore_IndtaBodyScreenData_0.mVal7 AS DECIMAL(10,2)),0) as TaxableSL, S.Pct, S.SchemeName, S.TargetAmt, " +
            " ISNULL(dbo.GetSalesReturn(tCore_Data_0.iBookNo, tcore_indta_0.iProduct, S.EffFromDt, S.EffToDt), 0) as TaxableSR," +
            //" ISNULL(CAST(tCore_IndtaBodyScreenData_0.mVal7 - ISNULL(dbo.GetSalesReturn(tCore_Data_0.iBookNo, tcore_indta_0.iProduct, S.EffFromDt, S.EffToDt), 0) AS DECIMAL(10,2)),0) as NetSales, " +
           // " IIF(tCore_IndtaBodyScreenData_0.mVal7 - ISNULL(dbo.GetSalesReturn(tCore_Data_0.iBookNo, tcore_indta_0.iProduct, S.EffFromDt, S.EffToDt), 0) >= S.TargetAmt, 'Yes', 'No') as AppStatus," +
            " ISNULL(C.ClaimType,-1) as ClaimType,ClaimStatus=ISNULL((SELECT DISTINCT 1 FROM tCore_Header_0 WHERE iVoucherType=4098 AND sVoucherNo=C.VocNo),0) FROM tCore_Header_0" +
            " INNER JOIN tCore_Data_0 ON tCore_Header_0.iHeaderId = tCore_Data_0.iHeaderId" +
            " INNER JOIN tCore_Indta_0 ON tCore_Data_0.iBodyId = tCore_Indta_0.iBodyId" +
            " INNER JOIN tCore_IndtaBodyScreenData_0 ON tCore_Data_0.iBodyId = tCore_IndtaBodyScreenData_0.iBodyId" +
            " JOIN SchemeTbl S on tCore_Data_0.iBookNo in (select * from dbo.fCore_fnDStringToTable(S.AccountIds, ',')) AND" +
            " tCore_Indta_0.iProduct in (select * from dbo.fCore_fnDStringToTable(S.ProductIds,','))" +
            " left JOIN ClaimTbl C on C.iBodyId = tCore_Data_0.iBodyId" +
            " WHERE tCore_Header_0.iVoucherType & 0xff00 = 3328 AND" +
            " tCore_Data_0.iMainBodyId = 0 AND tCore_Header_0.iDate BETWEEN " + DAL.GetDateToInt(FDate) + " AND " + DAL.GetDateToInt(TDate) + " AND " + Criteria + " ORDER BY S.SchemeName ASC";
            //DAL.SetLog(Str);
            DataSet ds = DALObj.GetData(Str, Convert.ToInt32(CompId), ref error);
            if (ds == null)
            {
                //return
            }
            if (ds.Tables.Count == 0)
            {
                //return
            }
            var data = new List<ClsGetSalesData>();
            try
            {
                data = ds.Tables[0].AsEnumerable().Select(c => new ClsGetSalesData
                {
                    BodyId = c.Field<object>("iBodyId").ToString(),
                    CustomerId = c.Field<object>("iBookNo").ToString(),
                    SchemeName = c.Field<string>("SchemeName"),
                    CustName = c.Field<string>("CustName"),
                    TargetAmt = c.Field<object>("TargetAmt").ToString(),
                    TaxableSL = c.Field<object>("TaxableSL").ToString(),
                    TaxableSR = c.Field<object>("TaxableSR").ToString(),
                    NetSales = (Convert.ToDecimal(c.Field<object>("TaxableSL"))-Convert.ToDecimal(c.Field<object>("TaxableSR"))).ToString(),
                    //AppStatus = c.Field<string>("AppStatus"),
                    ClaimPct = c.Field<object>("Pct").ToString(),
                    ClaimAmt =(Convert.ToDecimal(c.Field<object>("Pct"))* (Convert.ToDecimal(c.Field<object>("TaxableSL")) - Convert.ToDecimal(c.Field<object>("TaxableSR")))/100).ToString(),
                    ClaimType = c.Field<object>("ClaimType").ToString(),
                    ClaimStatus = Convert.ToInt32(c.Field<object>("ClaimStatus").ToString()),
                    Flag=0,
                }).ToList();
            }
            catch (Exception e)
            {
                throw;
            }

            var customerGroups = data.GroupBy(c => c.CustomerId);

            var claims = customerGroups.Select(g => new ClsGetSalesData
            {
                BodyId = string.Join(",", g.Select(c => c.BodyId).ToArray()),
                CustomerId = g.Key,
                CustName = g.FirstOrDefault().CustName,
                SchemeName = g.FirstOrDefault().SchemeName,
                TargetAmt = g.FirstOrDefault().TargetAmt,
                ClaimPct = g.FirstOrDefault().ClaimPct,
                //AppStatus = g.FirstOrDefault().AppStatus,
                ClaimType = g.FirstOrDefault().ClaimType,
                ClaimStatus = g.FirstOrDefault().ClaimStatus,
                TaxableSR = g.FirstOrDefault().TaxableSR,
                TaxableSL = g.Sum(v => Convert.ToDecimal(v.TaxableSL)).ToString(),
                //(parseFloat(SalesData[k].ClaimPct)*Net)/100;
                ClaimAmt = (Convert.ToDecimal(g.FirstOrDefault().ClaimPct) * (Convert.ToDecimal(g.Sum(v => Convert.ToDecimal(v.TaxableSL)) - Convert.ToDecimal(g.FirstOrDefault().TaxableSR))) / 100).ToString(),
                NetSales = Convert.ToDecimal(g.Sum(v => Convert.ToDecimal(v.TaxableSL)) - Convert.ToDecimal(g.FirstOrDefault().TaxableSR)).ToString(),
                Flag =0,
            }).ToList();
            return Json(new { claims }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PostVoucher(string CompId, string SessionId, List<ClsGetSalesData> model)
        {
            DAL.SetLog("PostVoucher");
            int Flg = 0;
            try
            {
                string error = string.Empty;
                int iBodyCnt = 0;
                DateTime today = DateTime.Today;

                int CurrDate = DALObj.DateToInt(today);
                string PostedDt = string.Empty;
                for (int i = 0; i < model.Count; i++)
                {
                    int AccHeader = DALObj.GetAccId(Convert.ToInt32(CompId), "mCore_Account", "Sales Account");
                    int AccBody = DALObj.GetAccId(Convert.ToInt32(CompId), "mCore_Account", model[i].CustName);
                    _MTrans objMTrans1 = new _MTrans();
                    JObject jsonObj1 = new JObject();
                    List<TransactionService.IdNameTag> lstHeaderData = new List<TransactionService.IdNameTag>();
                    string strServer = WebConfigurationManager.AppSettings["IpAddress"];
                    lstHeaderData.Add(new TransactionService.IdNameTag { sName = "Date", Value = CurrDate });
                    lstHeaderData.Add(new TransactionService.IdNameTag { sName = "Account", Value = AccHeader });
                    objMTrans1.arrHeader = lstHeaderData.ToArray();
                    string[] arrBodyNames = new string[6];
                    arrBodyNames[0] = "Account";
                    arrBodyNames[1] = "Net_Sales";
                    arrBodyNames[2] = "Claim_Percent";
                    arrBodyNames[3] = "Amount";
                    arrBodyNames[4] = "Scheme";
                    arrBodyNames[5] = "Claim_Type";
                    objMTrans1.arrBodyNames = arrBodyNames;
                    iBodyCnt = model.Count;
                    objMTrans1.arrBody = new object[1][];
                    object[] arrBodyValue1 = new object[6];
                    arrBodyValue1 = new object[6];
                    arrBodyValue1[0] = AccBody;
                    arrBodyValue1[1] = model[i].NetSales;
                    arrBodyValue1[2] = model[i].ClaimPct;
                    arrBodyValue1[3] = model[i].ClaimAmt;
                    arrBodyValue1[4] = model[i].SchemeName;
                    arrBodyValue1[5] = model[i].ClaimType;
                    objMTrans1.arrBody[0] = arrBodyValue1;
                    jsonObj1 = new JObject();
                    jsonObj1.Add("objMTrans", JToken.FromObject(objMTrans1));
                    jsonObj1.Add("iVoucherType", "4098");//Schemes to be Claimed
                    jsonObj1.Add("sDocNo", "");
                    jsonObj1.Add("bByIds", false);
                    jsonObj1.Add("bIsNew", true);
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                        client.Headers.Add("fsessionid", SessionId);
                        string arrResponse = client.UploadString(strServer + "/Focus8Library/TransactionService.svc/SaveVoucher2", jsonObj1.ToString());// set the url and data to post
                        saveresult.RootObject MObjRoot = new saveresult.RootObject();
                        MObjRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<saveresult.RootObject>(arrResponse);
                        DAL.SetLog(jsonObj1.ToString());
                        DAL.SetLog(arrResponse);
                        if (MObjRoot.SaveVoucher2Result.arrTransIds != null)
                        {
                            //DAL.SetLog("Prepayments Code: " + ds.Tables[0].Rows[i]["sCode"].ToString() + "  Posted Successfully to Voucher No: " + MObjRoot.SaveVoucher2Result.sValue);\

                            var Ids = model[i].BodyId.Split(',');
                            foreach (var Id in Ids)
                            {
                                DALObj.GetExecute("DELETE FROM ClaimTbl WHERE iBodyId=" + Id + "", Convert.ToInt32(CompId), ref error);
                                DALObj.GetExecute("INSERT INTO ClaimTbl(iBodyId,ClaimType,ClaimStatus,VocNo) VALUES(" + Id + "," + model[i].ClaimType + ",1,'" + MObjRoot.SaveVoucher2Result.sValue + "')", Convert.ToInt32(CompId), ref error);
                            }

                            Flg = 1;
                        }
                        else
                        {
                            Flg = 0;
                            //DAL.SetLog("Prepayments Code: " + ds.Tables[0].Rows[i]["sCode"].ToString() + " Voucher Posting Failed - " + arrResponse);
                        }
                    }
                }
                if (Flg == 1)
                {
                    return Json(new { Msg = true, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Msg = false, JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                DAL.SetLog("Voucher Posting Failed - " + ex.Message);
                return Json(new
                {
                    Msg = "Internal Error",
                    JsonRequestBehavior.AllowGet
                });
            }
        }


    }
}