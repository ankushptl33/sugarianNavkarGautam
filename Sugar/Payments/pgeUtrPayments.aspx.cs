using ApiModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

public partial class Sugar_Payments_pgeUtrPayments : System.Web.UI.Page
{

    string paymentUrl = "https://localhost:8085/api/payments/makepayment";
    string statusUrl = "https://localhost:8085/api/payments/checkstatus";
    string Head_Fields = string.Empty;
    string Head_Values = string.Empty;
    string Head_Update = string.Empty;
    int count = 0;
    int flag = 0;
    string OldmessageId = string.Empty;
    string Bank_Security = string.Empty;
    string UserIPAddress = string.Empty;
    string ipAddress1 = string.Empty;
    string ipAddress2 = string.Empty;
    string clientipN = string.Empty;
    string User_Security = string.Empty;
    string PaymentType = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            string url = "https://icanhazip.com";
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            string str = responseRead;
            UserIPAddress = str;
            ipAddress1 = Session["IPAddress1"].ToString();
            ipAddress2 = Session["IPAddress2"].ToString();
            ipAddress1 = ipAddress1 + '\n';
            Bank_Security = Session["Bank_Security"].ToString();
            if (ipAddress1 == UserIPAddress || ipAddress2 + '\n' == UserIPAddress)
            {

                if (Bank_Security == "N")
                {
                    this.bindData();
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            else {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
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

            qry = "SELECT     dbo.nt_1_utr.doc_no, dbo.nt_1_utr.doc_date, CONVERT(VARCHAR(10), dbo.nt_1_utr.doc_date, 105) AS date, dbo.nt_1_utr.mill_code, dbo.nt_1_utr.mc AS PaymentToId, " +
                     " Mill.Ac_Name_E AS payment_to_name, dbo.nt_1_utr.amount, dbo.nt_1_utr.isPaymentDone, 0 AS id, 0 AS payment_to, dbo.nt_1_utr.messageId, ISNULL(dbo.BankTransactionDetail.StatusCode, 'NC') " +
                     " AS paymentStatusCode, dbo.BankTransactionDetail.StatusDescription AS paymentStatusDescription, ISNULL(dbo.nt_1_utr.bankTransactionId,0) as bankTransactionId,   dbo.nt_1_utr.PaymentType" +
                    " FROM         dbo.nt_1_utr LEFT OUTER JOIN" +
                     " dbo.BankTransactionDetail ON dbo.nt_1_utr.Year_Code = dbo.BankTransactionDetail.Year_Code AND dbo.nt_1_utr.Company_Code = dbo.BankTransactionDetail.Company_Code AND " +
                     " dbo.nt_1_utr.bankTransactionId = dbo.BankTransactionDetail.detailid  LEFT OUTER JOIN" +
                     " dbo.qrymstaccountmaster AS Bank ON dbo.nt_1_utr.ba = Bank.accoid LEFT OUTER JOIN" +
                     " dbo.qrymstaccountmaster AS Mill ON dbo.nt_1_utr.mc = Mill.accoid " +
            " where Processed!='Y' and dbo.nt_1_utr.EntryType='FS' and dbo.nt_1_utr.bank_ac =" + Convert.ToInt32(Session["BankPaymentAc"].ToString()) + " and dbo.nt_1_utr.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and dbo.nt_1_utr.year_code=" + Convert.ToInt32(Session["year"].ToString());
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
    protected void lbkTenderNo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkTenderNo = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkTenderNo.NamingContainer;
            string[] args = lnkTenderNo.CommandArgument.Split(';');
            string id = "";
            string tranType = "";
            if (args.Length >= 3)
            {
                id = args[0];
                tranType = args[1];
            }
            string No = lnkTenderNo.Text;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:PaymentsPage('" + id + "', '" + tranType + "')", true);
            //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tnjhj", "javascript:TN();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
   

    protected async void    btnMakePayment_Click(object sender, EventArgs e)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kyj", "javascript:toggleLoader()", true);
        Button btnMakePayment = (Button)sender;
        string[] args = btnMakePayment.CommandArgument.Split(';');

        // fetch account details
        string DocNo = args[0];
        string paymentTo = args[1];
        string payableAmount = args[2];
       // string doc_no = args[3];
        string isPaymentDone = args[3];
        string paymentDate = args[4];
        string tranType = "UI";
        long PaymentToId = Convert.ToInt64(args[5]);
        string messageId = args[6];
        long bankTransactionId = Convert.ToInt64(args[7]);
        PaymentType = args[8];
        OldmessageId = messageId;
        DateTime date = Convert.ToDateTime(paymentDate);
        string formattedDate = date.ToString("yyyy-MM-dd");
        string DrCrDescription = string.Empty;
        if (isPaymentDone == "1")
        {
            string statusCode = await CheckPaymentStatus(messageId, formattedDate, bankTransactionId, tranType, DocNo, PaymentToId);
            this.bindData();
        }
        else
        {

            string paymentToAcId = "";
            string qry = string.Empty;
           // string qry = "select * from nt_1_accountmaster where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
           
            string benBankAccountNumber = "";
            string benBankIFSC = "";
            string benAccountName = "";
            string SelectedBank = string.Empty;
            SelectedBank = clsCommon.getString("select SelectedBank from qrymstaccountmaster where Ac_Code='" + paymentTo + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            if (SelectedBank == "B2")
            {
                qry = "select payBankAc2 as BankAcNumber,payIfsc2 as BankIfsc,PayBankName2 as BankName,BeneficiaryName2 as BeneficiaryName,accoid from " +
                             " qrymstaccountmaster where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            }
            else if (SelectedBank == "B3")
            {
                qry = "select  payBankAc3 as BankAcNumber,payIfsc3 as BankIfsc,PayBankName3 as BankName,BeneficiaryName3 as BeneficiaryName,accoid from " +
                              " qrymstaccountmaster where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            }
            else
            {
                qry = "select  payBankAc as BankAcNumber,payIfsc as BankIfsc,PayBankName as BankName,BeneficiaryName as BeneficiaryName,accoid from " +
                        " qrymstaccountmaster where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            }
            DataTable dt = new DataTable();
            DataSet ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        benBankAccountNumber = dt.Rows[0]["BankAcNumber"].ToString();
                        benBankIFSC = dt.Rows[0]["BankIfsc"].ToString();
                        benAccountName = dt.Rows[0]["BeneficiaryName"].ToString();
                        paymentToAcId = dt.Rows[0]["accoid"].ToString();
                        DrCrDescription = "" + benAccountName + ", " + benBankAccountNumber + "";
                    }
                }
            }

            bool isAcNoNumeric = benBankAccountNumber.All(char.IsDigit);

            if (!isAcNoNumeric || benBankIFSC == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " asdasd", "alert('Beneficiery details are incorrect!');removeLoader();", true);
                return;
            }
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    PaymentRequestModel paymentRequest = new PaymentRequestModel();

                    PaymentInfoModel paymentInfo = new PaymentInfoModel
                    {
                        AccountNo = Session["accountNo"].ToString(),
                        ClientCode = Session["clintCode"].ToString(),
                        DrDesc = DrCrDescription,
                        MyProdCode = "VPAY",
                        PayMode = PaymentType,
                        TxnAmnt = payableAmount,
                        MessageId = "",
                        MsgSource = "",
                        BatchRefNmbr = "",
                        CompanyId = "",
                        DrRefNmbr = "",
                        PaymentDt = "",
                        InstDt = "",
                        InstRefNo = "",
                        Enrichment = "",
                    };

                    ReceiverBankingDetails receiverBankingDetails = new ReceiverBankingDetails
                    {
                        BeneAcctNo = benBankAccountNumber,
                        RecBrCd = benBankIFSC,
                        BeneName = benAccountName,
                        BeneCode = "",
                        BeneEmail = "",
                        BeneMb = "",
                        BeneAddr1 = "",
                        BeneAddr2 = "",
                        BeneAddr3 = "",
                        BeneAddr4 = "",
                        BeneAddr5 = "",
                        city = "",
                        zip = "",
                        Country = "",
                        State = "",
                        TelephoneNo = "",
                    };

                    paymentRequest.PaymentInfo = paymentInfo;
                    paymentRequest.ReceiverBankingDetails = receiverBankingDetails;

                     
                    var jsonPayload = JsonConvert.SerializeObject(paymentRequest);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make an asynchronous POST request to the API.
                    HttpResponseMessage response = await client.PostAsync(paymentUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful response here.
                        string responseContent = await response.Content.ReadAsStringAsync();
                        PaymentApiFinalResponse paymentApiFinalResponse = JsonConvert.DeserializeObject<PaymentApiFinalResponse>(responseContent);

                        // Now, you can access the properties of apiResponse.
                        string MessageId = paymentApiFinalResponse.MessageId;
                        string PaymentStatusCode = paymentApiFinalResponse.PaymentStatusCode;
                        string PaymentStatusRemark = paymentApiFinalResponse.PaymentStatusRemark;
                        var IsSuccess = paymentApiFinalResponse.IsSuccess;
                        string ErrorMessage = paymentApiFinalResponse.ErrorMessage; 
                        string TransactionDateConvert = DateTime.Now.ToString("yyyy/MM/dd");

                        Head_Fields = Head_Fields + "doc_no,";
                        Head_Values = Head_Values + "'" + DocNo + "',";
                        Head_Fields = Head_Fields + "TranType,";
                        Head_Values = Head_Values + "'" + tranType + "',";
                        Head_Fields = Head_Fields + "AcCode,";
                        Head_Values = Head_Values + "'" + paymentTo + "',";
                        Head_Fields = Head_Fields + "AcId,";
                        Head_Values = Head_Values + "'" + paymentToAcId + "',";
                        Head_Fields = Head_Fields + "AccountNumber,";
                        Head_Values = Head_Values + "'" + benBankAccountNumber + "',";
                        Head_Fields = Head_Fields + "IFSC,";
                        Head_Values = Head_Values + "'" + benBankIFSC + "',";
                        Head_Fields = Head_Fields + "Name,";
                        Head_Values = Head_Values + "'" + benAccountName + "',";
                        Head_Fields = Head_Fields + "Amount,";
                        Head_Values = Head_Values + "'" + payableAmount + "',";
                        Head_Fields = Head_Fields + "Narration,";
                        Head_Values = Head_Values + "'" + DrCrDescription + "',";
                        Head_Fields = Head_Fields + "MessageId,";
                        Head_Values = Head_Values + "'" + MessageId + "',";
                        Head_Fields = Head_Fields + "StatusCode,";
                        Head_Values = Head_Values + "'" + IsSuccess + "',";
                        Head_Fields = Head_Fields + "StatusDescription,";
                        Head_Values = Head_Values + "'" + ErrorMessage + "',";
                        Head_Fields = Head_Fields + "PaymentStatusCode,";
                        Head_Values = Head_Values + "'" + PaymentStatusCode + "',";
                        Head_Fields = Head_Fields + "PaymentStatusRemark,";
                        Head_Values = Head_Values + "'" + PaymentStatusRemark + "',";
                        Head_Fields = Head_Fields + "TranctionDate,";
                        Head_Values = Head_Values + "'" + TransactionDateConvert + "',";
                        Head_Fields = Head_Fields + "Company_Code,";
                        Head_Values = Head_Values + "'" + Company_Code + "',";
                        Head_Fields = Head_Fields + "Year_Code";
                        Head_Values = Head_Values + "'" + Year_Code + "'";

                        qry = "insert into BankTransaction (" + Head_Fields + ") values(" + Head_Values + ");SELECT SCOPE_IDENTITY();";
                        long head_id = clsDAL.ExecuteScalar(qry); 
                        if (IsSuccess)
                        {
                            string updatePaymentHeadQry = "update nt_1_utr set isPaymentDone=1, messageId='" + MessageId + "' where doc_no = " + DocNo + " and company_code = " + Company_Code + " and year_code = " + Year_Code;
                            int rowsAffected = clsDAL.ExecuteNonQuery(updatePaymentHeadQry);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "afdasd", "alert('Payment In Process!');removeLoader();window.reload();", true);
                            // reload page here
                            this.bindData();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ertqew", "alert('Payment Failed!');removeLoader();window.reload();", true);
                        }
                    }
                    else
                    {
                        // Handle API error or non-successful response.
                        // Example: Display an error message to the user.
                        string responseContent = await response.Content.ReadAsStringAsync();
                        PaymentApiFinalResponse paymentApiFinalResponse = JsonConvert.DeserializeObject<PaymentApiFinalResponse>(responseContent);

                        string MessageId = paymentApiFinalResponse.MessageId;
                        string PaymentStatusCode = paymentApiFinalResponse.PaymentStatusCode;
                        string PaymentStatusRemark = paymentApiFinalResponse.PaymentStatusRemark;
                        DateTime TransactionDate = paymentApiFinalResponse.TransactionDate;
                        var IsSuccess = paymentApiFinalResponse.IsSuccess;
                        string ErrorMessage = paymentApiFinalResponse.ErrorMessage; 
                        //string TransactionDateConvert = TransactionDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));

                        string TransactionDateConvert = DateTime.Now.ToString("yyyy/MM/dd");


                        Head_Fields = Head_Fields + "doc_no,";
                        Head_Values = Head_Values + "'" + DocNo + "',";
                        Head_Fields = Head_Fields + "TranType,";
                        Head_Values = Head_Values + "'" + tranType + "',";
                        Head_Fields = Head_Fields + "AcCode,";
                        Head_Values = Head_Values + "'" + paymentTo + "',";
                        Head_Fields = Head_Fields + "AcId,";
                        Head_Values = Head_Values + "'" + paymentToAcId + "',";
                        Head_Fields = Head_Fields + "AccountNumber,";
                        Head_Values = Head_Values + "'" + benBankAccountNumber + "',";
                        Head_Fields = Head_Fields + "IFSC,";
                        Head_Values = Head_Values + "'" + benBankIFSC + "',";
                        Head_Fields = Head_Fields + "Name,";
                        Head_Values = Head_Values + "'" + benAccountName + "',";
                        Head_Fields = Head_Fields + "Amount,";
                        Head_Values = Head_Values + "'" + payableAmount + "',";
                        Head_Fields = Head_Fields + "Narration,";
                        Head_Values = Head_Values + "'" + DrCrDescription + "',";
                        Head_Fields = Head_Fields + "MessageId,";
                        Head_Values = Head_Values + "'" + MessageId + "',";
                        Head_Fields = Head_Fields + "StatusCode,";
                        Head_Values = Head_Values + "'" + IsSuccess + "',";
                        Head_Fields = Head_Fields + "StatusDescription,";
                        Head_Values = Head_Values + "'" + ErrorMessage + "',";
                        Head_Fields = Head_Fields + "PaymentStatusCode,";
                        Head_Values = Head_Values + "'" + PaymentStatusCode + "',";
                        Head_Fields = Head_Fields + "PaymentStatusRemark,";
                        Head_Values = Head_Values + "'" + PaymentStatusRemark + "',";
                        Head_Fields = Head_Fields + "TranctionDate,";
                        Head_Values = Head_Values + "'" + TransactionDateConvert + "',";
                        Head_Fields = Head_Fields + "Company_Code,";
                        Head_Values = Head_Values + "'" + Company_Code + "',";
                        Head_Fields = Head_Fields + "Year_Code,";
                        Head_Values = Head_Values + "'" + Year_Code + "',";

                        qry = "insert into BankTransaction (" + Head_Fields + ") values(" + Head_Values + ");SELECT SCOPE_IDENTITY();";
                        long head_id = clsDAL.ExecuteScalar(qry);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('API Call Failed!');removeLoader();", true);

                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('An error occurred: " + ex.Message + "');removeLoader();", true);
                }
            }
        }
    }

    private async Task<string> CheckPaymentStatus(string messageId, string PaymentDate, long bankTransactionId, string tranType, string DocNo, long paymentToId)
    {
        string qryelement = string.Empty;
        string doc_no = string.Empty;
        string Doc_Date = string.Empty;
        string Ac_Code = string.Empty;
        long AcId = 0;
        string AccountNumber = string.Empty;
        string IFSC_Code = string.Empty;
        string Amount = string.Empty;
        string Payment_Type = string.Empty;
        string BankCode = string.Empty;
        string bcId = string.Empty;
        string Year_Code = Session["year"].ToString();
        string Company_Code = Session["Company_Code"].ToString();
        string NARRATION = string.Empty;
        string Branch_Code = string.Empty;
        string UTR = string.Empty;
        string MessageId = string.Empty;
        string returnMessage = string.Empty;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Create your payment request payload here
                var payload = new
                {
                    ClientCode = Session["clintCode"].ToString(),
                    MessageId = messageId,
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
                    StatusApiFinalResponse statusApiFinalResponse = JsonConvert.DeserializeObject<StatusApiFinalResponse>(responseContent);

                    // Now, you can access the properties of apiResponse.
                    var IsSuccess = statusApiFinalResponse.IsSuccess;
                    MessageId = statusApiFinalResponse.MessageId;
                    string StatusCode = statusApiFinalResponse.StatusCode;
                    string StatusDescription = statusApiFinalResponse.StatusDescription;
                    string InstRefNo = statusApiFinalResponse.InstRefNo;
                    UTR = statusApiFinalResponse.UTR;
                    var returnmsg = StatusDescription;
                    // Handle success response accordingly
                    Head_Fields = string.Empty;
                    Head_Values = string.Empty;
                    Head_Fields = Head_Fields + "tranctionid,";
                    Head_Values = Head_Values + "'" + bankTransactionId + "',";
                    Head_Fields = Head_Fields + "StatusCode,";
                    Head_Values = Head_Values + "'" + StatusCode + "',";
                    Head_Fields = Head_Fields + "StatusDescription,";
                    Head_Values = Head_Values + "'" + StatusDescription + "',";
                    Head_Fields = Head_Fields + "UTRNumber,";
                    Head_Values = Head_Values + "'" + UTR + "',";
                    Head_Fields = Head_Fields + "Company_Code,";
                    Head_Values = Head_Values + "'" + Company_Code + "',";
                    Head_Fields = Head_Fields + "Year_Code,";
                    Head_Values = Head_Values + "'" + Year_Code + "',";
                    Head_Fields = Head_Fields + "MessageId,";
                    Head_Values = Head_Values + "'" + MessageId + "',";
                    Head_Fields = Head_Fields + "UtrDocNo";
                    Head_Values = Head_Values + "'" + DocNo + "'";

                    string qry = "insert into BankTransactionDetail (" + Head_Fields + ") values(" + Head_Values + ");SELECT SCOPE_IDENTITY();"; // update status if status already there(get detail id databind query)
                    long detailId = clsDAL.ExecuteScalar(qry);
                      
                    if (OldmessageId == messageId) 
                    {
                        string updatePaymentHeadQry = "update nt_1_utr set  bankTransactionId=" + detailId + ", messageId='" + MessageId + "' where doc_no = " + DocNo + " and company_code = " + Company_Code + " and year_code = " + Year_Code;
                        int rowsAffected = clsDAL.ExecuteNonQuery(updatePaymentHeadQry);
                    }
                    else
                    {

                        string updatePaymentHeadQry = "update nt_1_utr set  bankTransactionId=" + detailId + ", messageId='" + MessageId + "' where doc_no = " + DocNo + " and company_code = " + Company_Code + " and year_code = " + Year_Code;
                        int rowsAffected = clsDAL.ExecuteNonQuery(updatePaymentHeadQry);
                    }
                    returnMessage = StatusCode;
                    if (StatusCode == "U")
                    {
                        string updatePaymentHeadQry = "update nt_1_utr set bankTransactionId=" + detailId + ", Processed='Y',utr_no='" + UTR + "', messageId='" + MessageId + "' where doc_no = " + DocNo + " and company_code = " + Company_Code + " and year_code = " + Year_Code;
                        int rowsAffected = clsDAL.ExecuteNonQuery(updatePaymentHeadQry);
                    }

                    if (IsSuccess)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment In Success!');removeLoader(); window.reload();", true);
                    }

                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('API Call Failed!Please Try Again Later.');removeLoader(); window.reload();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('An error occurred: " + ex.Message + "');removeLoader(); window.reload();", true);
            }
            return returnMessage;
        }
    }

    #region DataStore

    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    private int DataStore(string Query, int flag)
    {
        var cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        con = new SqlConnection(cs);
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