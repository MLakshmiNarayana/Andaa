using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrjAndaa.Models;
using System.Data;
using Newtonsoft.Json.Linq;

namespace PrjAndaa.Controllers
{
    public class ConSchemePrefController : Controller
    {
        DAL DALObj = new DAL();
        public ActionResult VwSchemePref()
        {
            return View();
        }


        private List<ClsMasters> GetAccounts(string CompId, string SchemeName)
        {
            DAL.SetLog("GetAccounts");
            string ListString = string.Empty;
            bool Contain;
            List<ClsMasters> AccMstrs = new List<ClsMasters>();
            string error = string.Empty;
            if (SchemeName != "")
            {
                ListString = DALObj.GetMIdList(Convert.ToInt32(CompId), SchemeName, "AccountIds");
            }

            DataSet ds = DALObj.GetData("select m.iMasterId,m.sName,t.iParentId from mCore_Account m ,mCore_AccountTreeDetails t where m.iMasterId=t.iMasterId and m.iStatus<>5  and m.iMasterId<>0 and m.iAccountType IN(5,7)", Convert.ToInt32(CompId), ref error);

            var ids = ListString.Split(',');

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (SchemeName != "")
                            {
                                Contain = ids.Contains(ds.Tables[0].Rows[i]["iMasterId"].ToString());
                            }
                            else
                            {
                                Contain = false;
                            }
                            AccMstrs.Add(new ClsMasters { id = ds.Tables[0].Rows[i]["iMasterId"].ToString(), pid = ds.Tables[0].Rows[i]["iParentId"].ToString(), name = ds.Tables[0].Rows[i]["sName"].ToString(), IsChecked = Contain });
                        }
                    }
                }
            }
            return AccMstrs;
        }

        private List<ClsMasters> GetProducts(string CompId, string SchemeName)
        {
            DAL.SetLog("GetProducts");
            string ListString = string.Empty;
            bool Contain;
            List<ClsMasters> ProdMstrs = new List<ClsMasters>();
            string error = string.Empty;
            if (SchemeName != "")
            {
                ListString = DALObj.GetMIdList(Convert.ToInt32(CompId), SchemeName, "ProductIds");
            }

            DataSet ds = DALObj.GetData("select m.iMasterId,m.sName,t.iParentId from mCore_Product m ,mCore_ProductTreeDetails t where m.iMasterId=t.iMasterId and m.iStatus<>5 and m.iMasterId<>0", Convert.ToInt32(CompId), ref error);

            var ids = ListString.Split(',');
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (SchemeName != "")
                            {
                                Contain = ids.Contains(ds.Tables[0].Rows[i]["iMasterId"].ToString());
                            }
                            else
                            {
                                Contain = false;
                            }
                            ProdMstrs.Add(new ClsMasters { id = ds.Tables[0].Rows[i]["iMasterId"].ToString(), pid = ds.Tables[0].Rows[i]["iParentId"].ToString(), name = ds.Tables[0].Rows[i]["sName"].ToString(), IsChecked = Contain });
                        }
                    }
                }
            }
            //return Json(new { products=ProdMstrs,Accoun});
            return ProdMstrs;
        }


        [HttpPost]
        public ActionResult GetMasters(string CompId, string SchemeName)
        {
            CreateSchema(Convert.ToInt32(CompId));
            return Json(new { SchemeDetails = SchemeHeaderDetails(CompId, SchemeName), Accounts = GetAccounts(CompId, SchemeName), Products = GetProducts(CompId, SchemeName) });
        }


        [HttpPost]
        public ActionResult GetDocNo(string CompId)
        {
            DAL.SetLog("GetDocNo");
            List<ClsGetDocNo> GetDocSeq = new List<ClsGetDocNo>();
            string error = string.Empty;
            DataSet ds = DALObj.GetData("select FORMAT(MAX(ISNULL(DocNo,0))+1,'0000') as DocSeqNo from SchemeTbl", Convert.ToInt32(CompId), ref error);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 0 || Convert.ToString(ds.Tables[0].Rows[0][0]) == "")
                        {
                            GetDocSeq.Add(new ClsGetDocNo { DocNo = "0001" });
                        }
                        else
                        {
                            GetDocSeq.Add(new ClsGetDocNo { DocNo = ds.Tables[0].Rows[0]["DocSeqNo"].ToString() });
                        }
                    }
                }
            }
            return Json(new { GetDocSeq }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveScheme(string CompId, DateTime SchemeDt, string AccountIds, string CreatedBy, string ProductIds, string TargetAmt, string Pct, string SchemeName, string Active, DateTime EffFromDt, DateTime EffToDt)
        {
            DAL.SetLog("SaveScheme");
            string SchemeDate = SchemeDt.ToString("yyyy-MM-dd");
            int EffectiveFromDt = DAL.GetDateToInt(EffFromDt);
            int EffectiveToDt = DAL.GetDateToInt(EffToDt);
            string SaveConf = string.Empty;
            string Err = string.Empty;
            int Flg = DALObj.GetExecute("UPDATE SchemeTbl SET AccountIds='" + AccountIds + "',ProductIds='" + ProductIds + "',TargetAmt=" + TargetAmt + ",Pct=" + Pct + ",Active=" + Active + ",EffFromDt='" + EffectiveFromDt + "',EffToDt='" + EffectiveToDt + "' WHERE SchemeName='" + SchemeName + "' IF @@ROWCOUNT = 0 INSERT INTO SchemeTbl(SchemeDt,AccountIds,CreatedBy,ProductIds,TargetAmt,Pct,SchemeName,Active,EffFromDt,EffToDt) values('" + SchemeDate + "','" + AccountIds + "','" + CreatedBy + "','" + ProductIds + "'," + TargetAmt + "," + Pct + ",'" + SchemeName + "'," + Active + ",'" + EffectiveFromDt + "','" + EffectiveToDt + "')", Convert.ToInt32(CompId), ref Err);

            if (Flg == 1)
            {
                return Json(new { SaveConf = "Scheme Saved Successfully", JsonRequestBehavior.AllowGet });
            }
            else if (Flg == -1)
            {
                return Json(new { SaveConf = "Scheme Name Already Exist", JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { SaveConf = "Internal Error", JsonRequestBehavior.AllowGet });
            }

        }

        private ClsGetSchemeHeader SchemeHeaderDetails(string CompId, string SchemeName)
        {
            string Err = string.Empty;
            var schemaDetails = new ClsGetSchemeHeader();
            //List<ClsGetSchemeHeader> GetSchemeDtls = new List<ClsGetSchemeHeader>();
            if (SchemeName != "")
            {
                DataSet ds = DALObj.GetData("select FORMAT(ISNULL(DocNo,0),'0000') as DocSeqNo,CONVERT(VARCHAR,SchemeDt,105) AS SchemeDt,CreatedBy,TargetAmt,Pct,SchemeName,Active,CONVERT(VARCHAR,dbo.IntToDate(EffFromDt),105) as EffFromDt,CONVERT(VARCHAR, dbo.IntToDate(EffToDt),105) as EffToDt from SchemeTbl WHERE SchemeName='" + SchemeName + "'", Convert.ToInt32(CompId), ref Err);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            schemaDetails = new ClsGetSchemeHeader
                            {
                                DocNo = ds.Tables[0].Rows[0]["DocSeqNo"].ToString(),
                                DocDate = ds.Tables[0].Rows[0]["SchemeDt"].ToString(),
                                CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString(),
                                TargetAmt = ds.Tables[0].Rows[0]["TargetAmt"].ToString(),
                                SchemePct = ds.Tables[0].Rows[0]["Pct"].ToString(),
                                SchemeName = ds.Tables[0].Rows[0]["SchemeName"].ToString(),
                                SchemeActive = ds.Tables[0].Rows[0]["Active"].ToString(),
                                EffFromDt = ds.Tables[0].Rows[0]["EffFromDt"].ToString(),
                                EffToDt = ds.Tables[0].Rows[0]["EffToDt"].ToString()
                            };
                        }
                    }
                }
            }
            return schemaDetails;
        }


        [HttpPost]
        public ActionResult GetSchemeFlag(string CompId,string SchemeName)
        {
            DAL.SetLog("GetSchemeName");
            List<ClsSchemeFlag> GetFlag = new List<ClsSchemeFlag>();
            string error = string.Empty;
            DataSet ds = DALObj.GetData("select 1 as Flag from SchemeTbl where SchemeName='"+ SchemeName + "'", Convert.ToInt32(CompId), ref error);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GetFlag.Add(new ClsSchemeFlag { Flag = ds.Tables[0].Rows[0]["Flag"].ToString() });
                    }
                }
            }
            return Json(new { GetFlag }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetSchemeNames(string CompId)
        {
            DAL.SetLog("GetSchemeNames");
            List<ClsSchemeNames> SchemeNamesList = new List<ClsSchemeNames>();
            string error = string.Empty;
            DataSet ds = DALObj.GetData("select DocNo,SchemeName FROM SchemeTbl", Convert.ToInt32(CompId), ref error);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            SchemeNamesList.Add(new ClsSchemeNames { id = ds.Tables[0].Rows[i]["DocNo"].ToString(), name = ds.Tables[0].Rows[i]["SchemeName"].ToString() });
                        }
                    }
                }
            }
            return Json(SchemeNamesList);
        }


        private void CreateSchema(int CompId)
        {
            string error = string.Empty;
            string StrQry = string.Empty;
            StrQry = "Select * from sysobjects where Name='SchemeTbl' AND type='U'";
            DataSet Ds = DALObj.GetData(StrQry, CompId, ref error);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                StrQry = "CREATE TABLE SchemeTbl (DocNo INT NOT NULL IDENTITY PRIMARY KEY,SchemeDt date NOT NULL,AccountIds varchar(max)NOT NULL,CreatedBy varchar(15) NOT NULL," +
                " ProductIds varchar(max) NOT NULL,TargetAmt int,Pct decimal(5, 2),SchemeName varchar(50),Active bit,EffFromDt INT NOT NULL,EffToDt INT not null,UNIQUE(SchemeName))";
                DALObj.GetExecute(StrQry, CompId, ref error);
            }
            Ds.Clear();
            StrQry = "Select * from sysobjects where Name='ClaimTbl' AND type='U'";
            Ds = DALObj.GetData(StrQry, CompId, ref error);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                StrQry = "CREATE TABLE ClaimTbl (Id int NOT NULL IDENTITY PRIMARY KEY,iBodyId int,ClaimType int,ClaimStatus BIT,VocNo VARCHAR(25))";
                DALObj.GetExecute(StrQry, CompId, ref error);
            }
            Ds.Clear();
            StrQry = "Select * from sysobjects where Name='GetSalesReturn' AND type='FN'";
            Ds = DALObj.GetData(StrQry, CompId, ref error);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                StrQry = "CREATE FUNCTION [dbo].[GetSalesReturn](@CustId INT, @ProdId INT,@FromDt INT,@ToDt INT)" +
                " RETURNS DECIMAL(8, 2) BEGIN" +
                " RETURN(SELECT SUM(mVal7) FROM tCore_Header_0" +
                " INNER JOIN tCore_Data_0 ON tCore_Header_0.iHeaderId = tCore_Data_0.iHeaderId" +
                " INNER JOIN tCore_IndtaBodyScreenData_0 ON tCore_Data_0.iBodyId = tCore_IndtaBodyScreenData_0.iBodyId" +
                " INNER JOIN tCore_HeaderData1792_0 ON tCore_Header_0.iHeaderId = tCore_HeaderData1792_0.iHeaderId" +
                " INNER JOIN tcore_indta_0 ON tCore_Data_0.iBodyId = tcore_indta_0.iBodyId" +
                " WHERE(tCore_Header_0.iVoucherType = 1792) AND(tCore_Data_0.iBookNo = @CustId) AND(tcore_indta_0.iProduct = @ProdId) AND(tCore_Header_0.iDate BETWEEN @FromDt AND @ToDt)); END";
                DALObj.GetExecute(StrQry, CompId, ref error);
            }
            else
            {
                StrQry = "ALTER FUNCTION [dbo].[GetSalesReturn](@CustId INT, @ProdId INT,@FromDt INT,@ToDt INT)"+ 
                " RETURNS DECIMAL(8, 2) BEGIN"+
                " RETURN(SELECT SUM(mVal7) FROM tCore_Header_0"+
                " INNER JOIN tCore_Data_0 ON tCore_Header_0.iHeaderId = tCore_Data_0.iHeaderId"+
                " INNER JOIN tCore_IndtaBodyScreenData_0 ON tCore_Data_0.iBodyId = tCore_IndtaBodyScreenData_0.iBodyId"+
                " INNER JOIN tCore_HeaderData1792_0 ON tCore_Header_0.iHeaderId = tCore_HeaderData1792_0.iHeaderId"+
                " INNER JOIN tcore_indta_0 ON tCore_Data_0.iBodyId = tcore_indta_0.iBodyId"+
                " WHERE(tCore_Header_0.iVoucherType = 1792) AND(tCore_Data_0.iBookNo = @CustId) AND(tcore_indta_0.iProduct = @ProdId) AND(tCore_Header_0.iDate BETWEEN @FromDt AND @ToDt)); END";
                DALObj.GetExecute(StrQry, CompId, ref error);
            }
        }


    }
}