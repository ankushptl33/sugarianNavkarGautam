using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Services;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using ApiModels.Models;
using System.Threading.Tasks;
public partial class Sugar_Report_rptcheckPaymentStatus : System.Web.UI.Page
{

    string Head_Update = string.Empty;
    int flag = 0;
    string qry = string.Empty;
    string qry1 = string.Empty;
    string Detail = string.Empty;
    int count = 0;
    string cs = string.Empty;
    string returnmsg = string.Empty;
    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    string qryelement = string.Empty;
    string doc_no = string.Empty;
    string Doc_Date = string.Empty;
    string Ac_Code = string.Empty;
    string AcId = string.Empty;
    string AccountNumber = string.Empty;
    string IFSC_Code = string.Empty;
    string Amount = string.Empty;
    string Payment_Type = string.Empty;
    string BankCode = string.Empty;
    string bcId = string.Empty;
    string Company_Code = string.Empty;
    string Year_Code = string.Empty;
    string tranctionid = string.Empty;
    string NARRATION = string.Empty;
    string Branch_Code = string.Empty;
    string UTR = string.Empty;
    string MessageId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        con = new SqlConnection(cs);
        if (!Page.IsPostBack)
        {
            this.bindData();
         
        }
    }
     
    

    private void bindData()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            //lblcityName.Text = clsCommon.getString("select [city_name_e] from " + cityMasterTable + " where [city_code]=" + cityCode + " and [company_code]=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string qry = "";
            qry = "select MessageId,doc_no,detailid,TranctionDateConverted from qryBankTranctionDetail where MessageId !='' and   (UTRNumber is null or UTRNumber='')   and Company_Code="
                + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dtlAcList.DataSource = dt;
                        dtlAcList.DataBind();
                    }
                }
            }


        }
        catch (Exception eec)
        {
            Response.Write(eec.Message);
        }
    }

    protected void dtlAcList_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btncheckStatus = new Button();
            btncheckStatus.Text = "Check Status";
            btncheckStatus.CommandName = "CheckStatus";
            btncheckStatus.CommandArgument = e.Item.ItemIndex.ToString();
            btncheckStatus.Click += new EventHandler(btncheckStatus_Click);
            e.Item.Cells[4].Controls.Add(btncheckStatus);
        }
    }

    protected async void btncheckStatus_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Detail = btn.CommandArgument;
        string messageId = clsCommon.getString("Select MessageId from qryBankTranctionDetail where detailid='" + Detail + "'");
        string PaymentDate = clsCommon.getString("Select TranctionDateConverted from qryBankTranctionDetail where detailid='" + Detail + "'");

        ClientScript.RegisterStartupScript(this.GetType(), "showLoader", "javascript:showLoader();", true);

        var result = await SomeAsyncOperation(messageId, PaymentDate);

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment is " + result + "');", true);

        ClientScript.RegisterStartupScript(this.GetType(), "hideLoader", "javascript:hideLoader();", true);
    }

    private async Task<string> SomeAsyncOperation(string messageId, string PaymentDate)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string statusUrl = "http://localhost:8085/api/payment/checkstatus";
                // Create your payment request payload here
                var payload = new
                {
                    ClientCode = Session["clintCode"].ToString(),
                    MessageId = messageId, // Use the provided messageId
                    TransactionDate = PaymentDate
                };

                // Serialize the payload to JSON.
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Make an asynchronous POST request to the API.
                HttpResponseMessage response = await client.PostAsync(statusUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle the successful response here.
                    string responseContent = await response.Content.ReadAsStringAsync();
                    StatusApiFinalResponse paymentApiFinalResponse = JsonConvert.DeserializeObject<StatusApiFinalResponse>(responseContent);

                    // Now, you can access the properties of apiResponse.
                    var IsSuccess = paymentApiFinalResponse.IsSuccess;
                    MessageId = paymentApiFinalResponse.MessageId;
                    string StatusCode = paymentApiFinalResponse.StatusCode;
                    string StatusDescription = paymentApiFinalResponse.StatusDescription;
                    string InstRefNo = paymentApiFinalResponse.InstRefNo;
                    UTR = paymentApiFinalResponse.UTR;
                    returnmsg = StatusDescription;
                    // Handle success response accordingly

                    Head_Update = Head_Update + "StatusCode=";
                    Head_Update = Head_Update + "'" + StatusCode + "',";
                    Head_Update = Head_Update + "StatusDescription=";
                    Head_Update = Head_Update + "'" + StatusDescription + "',";
                    Head_Update = Head_Update + "UTRNumber=";
                    Head_Update = Head_Update + "'" + UTR + "'";

                    flag = 2;
                    qry = "update BankTranctionDetail set " + Head_Update + " where detailid='" + Detail + "'";
                    Thread thred = new Thread(() => { count = DataStore(qry, flag); });
                    thred.Start();
                    thred.Join();

                    if (StatusCode == "U")
                    {
                        tranctionid = clsCommon.getString("select tranctionid from BankTranctionDetail where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Session["year"].ToString());

                        qryelement = "select * from BankTranction where  id=" + tranctionid + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        DataSet ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qryelement);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                { 
                                    doc_no = dt.Rows[0]["doc_no"].ToString();
                                    Doc_Date = dt.Rows[0]["Doc_date"].ToString();
                                    Ac_Code = dt.Rows[0]["AcCode"].ToString();
                                    AcId = dt.Rows[0]["AcId"].ToString();
                                    AccountNumber = dt.Rows[0]["AccountNumber"].ToString();
                                    IFSC_Code = dt.Rows[0]["IFSC"].ToString();
                                    Amount = dt.Rows[0]["Amount"].ToString();
                                    Payment_Type = dt.Rows[0]["PaymentType"].ToString();
                                    BankCode = dt.Rows[0]["Bank_Code"].ToString();
                                    bcId = dt.Rows[0]["bcid"].ToString();
                                    Company_Code = Session["Company_Code"].ToString();
                                    Year_Code = Session["year"].ToString();
                                    Branch_Code = Session["Branch_Code"].ToString();
                                    NARRATION = "UTR Number:" + UTR + ", MessageId:" + MessageId + "";
                                    qry = "INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO,DOC_DATE,AC_CODE,UNIT_code, NARRATION,AMOUNT,TENDER_ID," + 
                                          "TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc, progid,tranid)" +
                                          " VALUES ('BK','','" + doc_no + "','" + Doc_Date + "','" + Ac_Code + "','0','" + NARRATION + "','" + Amount + "','0','0','0','" + Company_Code + "','" + Year_Code + "','0','D','0','0','0','" + doc_no
                                          + "','" + AcId + "','0','2','0','0')";
                                    //ds = clsDAL.SimpleQuery(qry);
                                    flag = 1;
                                    Thread thred2 = new Thread(() => { count = DataStore(qry, flag); });
                                    thred.Start();
                                    thred.Join();
                                    qry1 = "INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO,DOC_DATE,AC_CODE,UNIT_code, NARRATION,AMOUNT,TENDER_ID," +
                                          "TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc, progid,tranid)" +
                                          " VALUES ('BK','','" + doc_no + "','" + Doc_Date + "','" + BankCode + "','0','" + NARRATION + "','" + Amount + "','0','0','0','" + Company_Code + "','" + Year_Code + "','0','C','0','0','0','" + doc_no
                                          + "','" + bcId + "','0','2','0','0')";
                                    //ds = clsDAL.SimpleQuery(qry1);
                                    flag = 1;
                                    Thread thred3 = new Thread(() => { count = DataStore(qry1, flag); });
                                    thred.Start();
                                    thred.Join();
                                }
                            }
                        }

                    }

                    if (IsSuccess)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment In Sucess!');", true);
                    }

                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('API Call Failed!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('An error occurred: " + ex.Message + "');", true);
            }

            return returnmsg;
        }
    }

    #region DataStore
    private int DataStore(string Query, int flag)
    {
        int count = 0;
        try
        {
            //Connection open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            ///Execution
            myTran = con.BeginTransaction();
            //cmd.CommandText = qry;
            //cmd.Connection = con;
            //cmd.Transaction = myTran;
            if (flag == 1)
            {
                cmd = new SqlCommand(Query, con, myTran);
                cmd.ExecuteNonQuery();
                //cmd = new SqlCommand(Detail_Insert, con, myTran);
                //cmd.ExecuteNonQuery();
                myTran.Commit();
                Thread.Sleep(100);

                count = 1;
            }
            else if (flag == 2)
            {
                if (Query != "")
                {
                    cmd = new SqlCommand(Query, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                myTran.Commit();
                Thread.Sleep(100);
                count = 2;
            }

            return count;
        }
        catch
        {
            if (myTran != null)
            {
                myTran.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('check Entry AND Try Again !')", true);

            }
            return count;

        }
        finally
        {
            con.Close();
        }

    }
    #endregion

}