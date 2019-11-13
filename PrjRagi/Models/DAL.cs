using Focus.Common.DataStructs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace PrjAndaa
{
    public class DAL
    {
        public static DataTable dt = new DataTable();
        public int GetExecute(string strInsertOrUpdateQry, int CompId, ref string error)
        {
            try
            {
                Database obj = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                return (obj.ExecuteNonQuery(CommandType.Text, strInsertOrUpdateQry));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public static void SetLog(string content)
        {
            string strFilename = "RagiLog.txt";

            System.IO.FileStream fs1 = new System.IO.FileStream(string.Format(@"{0}\{1}", System.IO.Path.GetTempPath(), strFilename), System.IO.FileMode.Append, System.IO.FileAccess.Write);
            System.IO.StreamWriter tw = new System.IO.StreamWriter(fs1);
            tw.WriteLine("\r\n");
            tw.WriteLine(DateTime.Now.ToString() + " : " + content);
            tw.Close();
            fs1.Close();
        }

        public DataSet GetData(string strSelQry, int CompId, ref string error)
        {
            try
            {
                Database obj = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                return (obj.ExecuteDataSet(CommandType.Text, strSelQry));
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public static int GetDateToInt(DateTime dt)
        {
            int val;
            val = Convert.ToInt16(dt.Year) * 65536 + Convert.ToInt16(dt.Month) * 256 + Convert.ToInt16(dt.Day);
            return val;
        }
        public static string GetStringDateToint_2(string dDate)
        {
            try
            {
               
                CultureInfo MyCultureInfo = new CultureInfo("en-US");
                DateTime dt2 = DateTime.Parse(dDate, MyCultureInfo);
                return Date.DateToInt(dt2).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
        public int GetCompanyId(string strCompanyCode)
        {
            try
            {
                string strCode = strCompanyCode;
                int iCompId = ((strCode[0] >= 'A' ? strCode[0] - 55 : strCode[0] - 48) * 36 * 36) + ((strCode[1] >= 'A' ? strCode[1] - 55 : strCode[1] - 48) * 36) + (strCode[2] - 48);
                return iCompId;
            }
            catch (Exception ex)
            {
                SetLog("Bad request In GetCompanyId" + ex.Message);
                return 0;
            }

        }

        public static DataSet ExecuteProcedure(int CompId, string Param1, ref string error)
        {
            try
            {
                System.Data.DataSet ds = new DataSet();
                Database obj = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                string conn1 = obj.ConnectionString;
                SqlConnection sqlconn = new SqlConnection();
                sqlconn.ConnectionString = conn1;
                SqlCommand cmdProc = new SqlCommand("PrePaymentDetails", sqlconn);
                cmdProc.CommandType = System.Data.CommandType.StoredProcedure;
                cmdProc.Parameters.AddWithValue("@yyyymmdd", Param1);
                cmdProc.CommandTimeout = 0;
                SqlDataAdapter adapter = new SqlDataAdapter(cmdProc);
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                DAL.SetLog(e.Message);
                error = e.Message;
                return null;
            }
        }

        public int GetMasterId(int CompId, string mastertablename, string mastercode)
        {
            int masterid = 0;
            DataSet dss;
            string error = default(string);
            dss = GetData("select iCurrencyId from  " + mastertablename + " where sCode='" + mastercode + "'", CompId, ref error);
            if (dss.Tables[0].Rows.Count > 0)
            {
                masterid = Convert.ToInt32(dss.Tables[0].Rows[0]["iCurrencyId"]);
            }
            return masterid;
        }

        public string GetMIdList(int CompId, string SchemeName, string FieldName)
        {
            string List = string.Empty;
            DataSet dss;
            string error = default(string);
            dss = GetData("select "+FieldName+" from SchemeTbl where SchemeName='" + SchemeName + "'", CompId, ref error);
            if (dss.Tables[0].Rows.Count > 0)
            {
                List = dss.Tables[0].Rows[0][FieldName].ToString();
            }
            return List;
        }


        public int GetAccId(int CompId, string mastertablename, string mastername)
        {
            int masterid = 0;
            DataSet dss;
            string error = default(string);
            dss = GetData("select iMasterId from  " + mastertablename + " where sName='" + mastername + "'", CompId, ref error);
            if (dss.Tables[0].Rows.Count > 0)
            {
                masterid = Convert.ToInt32(dss.Tables[0].Rows[0]["iMasterId"]);
            }
            return masterid;
        }



        public string GetStringData(string Query, int CompId)
        {
            string ReturnStr = string.Empty;
            string error = default(string);
            DataSet ds = GetData(Query, CompId, ref error);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReturnStr = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            return ReturnStr;
        }

        public IDataReader GetData1(string strSelQry, int CompId, ref string error)
        {
            try
            {
                Database FDatabase = null;
                FDatabase = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                return (FDatabase.ExecuteReader(CommandType.Text, strSelQry));
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }
        internal static DataSet GetData(string que, int myCompanyval, ref object error)
        {
            throw new NotImplementedException();
        }

        public Int64 GetDateTimetoInt(DateTime dt)
        {
            Int64 val;
            val = Convert.ToInt64(dt.Year) * 8589934592 + Convert.ToInt64(dt.Month) * 33554432 + Convert.ToInt64(dt.Day) * 131072 + Convert.ToInt64(dt.Hour) * 4096 + Convert.ToInt64(dt.Minute) * 64 + Convert.ToInt64(dt.Second);
            return val;
        }
        public Date GetIntToDate(int iDate)
        {
            try
            {
                return (new Date(iDate, CalendarType.Gregorean));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long DateTimeToInt(DateTime dt)
        {
            try
            {
                return (new FDateTime(dt, CalendarType.Gregorean).Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DateToInt(DateTime dt)
        {
            int iValue = 0;
            try
            {
                iValue = new Date(dt.Year, dt.Month, dt.Day, CalendarType.Gregorean).Value;
                return (iValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getServiceLink()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string strFileName = "";
            strFileName = System.AppDomain.CurrentDomain.BaseDirectory + "ERPXML\\ServerSettings.xml";

            xmlDoc.Load(strFileName);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/ServSetting/ExternalModule/ServerName");
            string strValue;
            XmlNode node = nodeList[0];
            if (node != null)
                strValue = node.InnerText;
            else
                strValue = "";
            return strValue;
        }
    }
}