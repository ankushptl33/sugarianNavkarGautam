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

public partial class Sugar_Payments_rptPendingPayments : System.Web.UI.Page
{

    string paymentUrl = "https://localhost:8085/api/payments/makepayment";
    string statusUrl = "https://localhost:8085/api/payments/checkstatus";
    string Head_Fields = string.Empty;
    string Head_Values = string.Empty;
    string Head_Update = string.Empty;
    int count = 0;
    int flag = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string clientIpAddress = IpAddressHelper.GetClientIpAddress(Request);

        if (!IpAddressHelper.AllowedIpAddress.Contains(clientIpAddress))
        {
            Response.Clear();
            Response.Write("<h1>You do not have access to view this page.</h1>");
            Response.End();
        }
        else
        {
            if (!Page.IsPostBack)
            {
                this.bindData();
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
            qry = "SELECT pd.id, pd.doc_no, pd.Tran_Type, pd.bank_code, am.Ac_Name_E as bank_name, pd.payment_to,am2.accoid as PaymentToId, am2.Ac_Name_E as payment_to_name, pd.amount, pd.tds_percent, pd.tds_amount, pd.net_payable, pd.company_code, pd.year_code, CONVERT(VARCHAR(10), pd.date, 105) as date, ISNULL(pd.isPaymentDone, 0) as isPaymentDone, ISNULL(pd.paymentStatus,0) as paymentStatus, pd.messageId, ISNULL(pd.bankTransactionId,0) as bankTransactionId,  " +
                " ISNULL(btd.StatusCode, 'NC') as paymentStatusCode, btd.StatusDescription paymentStatusDescription, btd.UTRNumber as UTRNumber FROM PaymentHead pd " + 
                " left outer join nt_1_accountmaster am on pd.bank_code = am.Ac_Code and pd.company_code = am.company_code left outer join nt_1_accountmaster am2 on pd.payment_to = am2.Ac_Code and pd.company_code = am2.company_code " +
                " left outer join BankTransactionDetail btd on btd.tranctionid = pd.bankTransactionId and btd.Company_Code =pd.company_code and btd.Year_Code=pd.year_code " +
                " where pd.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and pd.year_code=" + Convert.ToInt32(Session["year"].ToString());
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:PaymentsPage('" + id + "', '"+tranType+"')", true);
            //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tnjhj", "javascript:TN();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //protected void btnchangedate_Click(object sender, EventArgs e)
    //{
    //    string changedate = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd"); ;
    //    string qry = "SELECT doid as DO_NO,doc_date,Purchase_Date,voucher_no,saleidtable,doc_no FROM qrydohead where  (SB_NO is null or SB_No=0)  and desp_type!='DO' and purc_no!=0 and SaleBillTo!=0 and SaleBillTo!=2 and SaleBillTo!=GETPASSCODE and Company_Code="
    //              + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
    //    DataSet ds = clsDAL.SimpleQuery(qry);
    //    DataTable dt = new DataTable();
    //    DataRow dr;
    //    DataTable Maindt = new DataTable();
    //    Maindt.Columns.Add("Querys", typeof(string));
    //    dr = Maindt.NewRow();
    //    if (ds != null)
    //    {
    //        dt = ds.Tables[0];
    //        string dono = "";
    //        string doc_no = "";
    //        string psno = "";
    //        string saleid = "";
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            dono = dono + dt.Rows[i]["DO_NO"].ToString() + ",";
    //            doc_no = doc_no + dt.Rows[i]["doc_no"].ToString() + ",";
    //            psno = psno + dt.Rows[i]["voucher_no"].ToString() + ",";
    //            saleid = saleid + dt.Rows[i]["saleidtable"].ToString() + ",";

    //        }
    //        dono = dono.Remove(dono.Length - 1);
    //        doc_no = doc_no.Remove(doc_no.Length - 1);
    //        psno = psno.Remove(psno.Length - 1);
    //        saleid = saleid.Remove(saleid.Length - 1);

    //        string qry1 = "update nt_1_deliveryorder set doc_date='" + changedate + "',Purchase_Date='" + changedate + "' where doid in(" + dono + ")";
    //        string qry2 = "update nt_1_sugarsale set doc_date='" + changedate + "' where saleid in(" + saleid + ")";
    //        string qry3 = "update nt_1_sugarpurchase set doc_date='" + changedate + "' where doc_no in(" + psno +
    //                ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

    //        string qry4 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='PS' and doc_no in(" + psno +
    //               ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
    //        string qry5 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='SB' and saleid in(" + saleid +
    //            ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
    //        string qry6 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='DO' and doc_no in(" + doc_no +
    //             ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry1;
    //        Maindt.Rows.Add(dr);

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry2;
    //        Maindt.Rows.Add(dr);

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry3;
    //        Maindt.Rows.Add(dr);

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry4;
    //        Maindt.Rows.Add(dr);

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry5;
    //        Maindt.Rows.Add(dr);

    //        dr = Maindt.NewRow();
    //        dr["Querys"] = qry6;
    //        Maindt.Rows.Add(dr);

    //        string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(2, Maindt);


    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
    //        Response.Redirect("rptcheckpendingsalebill.aspx");
    //    }


    //}

    protected async void btnMakePayment_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kyj", "javascript:toggleLoader()", true);
        Button btnMakePayment = (Button)sender;
        string[] args = btnMakePayment.CommandArgument.Split(';');

        // fetch account details
        string headId = args[0];
        string paymentTo = args[1];
        string payableAmount = args[2];
        string doc_no = args[3];
        string isPaymentDone = args[4];
        string messageId = args[5];
        string paymentDate = args[6];
        string tranType = args[7];
        long bankTransactionId = Convert.ToInt64(args[8]);
        long PaymentToId = Convert.ToInt64(args[9]);

        DateTime date = Convert.ToDateTime(paymentDate);
        string formattedDate = date.ToString("yyyy-MM-dd");
        string DrCrDescription = string.Empty;
        if (isPaymentDone == "1")
        {
            string statusCode = await CheckPaymentStatus(messageId, formattedDate, bankTransactionId, tranType, headId, PaymentToId);
            this.bindData();
        }
        else
        {

        string paymentToAcId = "";
        string qry = "select * from nt_1_accountmaster where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
        DataTable dt = new DataTable();
        DataSet ds = clsDAL.SimpleQuery(qry);
        string benBankAccountNumber = "";
        string benBankIFSC = "";
        string benAccountName = "";
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    benBankAccountNumber = dt.Rows[0]["payBankAc"].ToString();
                    benBankIFSC = dt.Rows[0]["payIfsc"].ToString();
                    benAccountName = dt.Rows[0]["Ac_Name_E"].ToString();
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
                    PayMode = "NEFT",
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


                //var payload = new
                //{
                //    ClientCode = "NAVKAR007",
                //    MessageId = "BHQMJU20231012077881",
                //    TransactionDate = "2023-10-12"
                //};

                // Serialize the payload to JSON.
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

                    //DateTime TransactionDate = paymentApiFinalResponse.TransactionDate; 
                    //string TransactionDateConvert = TransactionDate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));

                    string TransactionDateConvert = DateTime.Now.ToString("yyyy/MM/dd");

                    Head_Fields = Head_Fields + "doc_no,";
                    Head_Values = Head_Values + "'" + headId + "',";
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
                    //flag = 1;
                    //Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                    //thred.Start(); //Thread Operation Start
                    //thred.Join();

                    // update isPayment done field of PaymentDetails table  
                    if (IsSuccess)
                    {
                        string updatePaymentHeadQry = "update PaymentHead set isPaymentDone=1, bankTransactionId=" + head_id + ", messageId='" + MessageId + "' where id = " + headId + " and company_code = " + Company_Code + " and year_code = " + Year_Code;
                        int rowsAffected = clsDAL.ExecuteNonQuery(updatePaymentHeadQry);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "afdasd", "alert('Payment In Process!');removeLoader();window.reload()", true);
                            // reload page here
                            this.bindData();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ertqew", "alert('Payment Failed!');removeLoader();", true);
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
                    Head_Values = Head_Values + "'" + headId + "',";
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

    private async Task<string> CheckPaymentStatus(string messageId, string PaymentDate, long bankTransactionId, string tranType, string paymentHeadId, long paymentToId)
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
                    Head_Fields = Head_Fields + "Year_Code";
                    Head_Values = Head_Values + "'" + Year_Code + "'";

                    string qry = "insert into BankTransactionDetail (" + Head_Fields + ") values(" + Head_Values + ");SELECT SCOPE_IDENTITY();"; // update status if status already there(get detail id databind query)
                    long detailId = clsDAL.ExecuteScalar(qry);
                    returnMessage = StatusCode;
                    if (StatusCode == "U")
                    {
                        qryelement = "select * from PaymentHead where id=" + paymentHeadId + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        DataSet ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qryelement);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    doc_no = dt.Rows[0]["id"].ToString();
                                    Doc_Date = dt.Rows[0]["date"].ToString();
                                    DateTime date = Convert.ToDateTime(Doc_Date);
                                    string formattedDate = date.ToString("yyyy-MM-dd");
                                    Ac_Code = dt.Rows[0]["payment_to"].ToString();
                                    AcId = paymentToId;
                                    Amount = dt.Rows[0]["Amount"].ToString();
                                    Payment_Type = dt.Rows[0]["Tran_Type"].ToString();
                                    BankCode = Session["BankPaymentAc"].ToString();
                                    bcId = Session["bpid"].ToString();
                                    Company_Code = Session["Company_Code"].ToString();
                                    Year_Code = Session["year"].ToString();
                                    Branch_Code = Session["Branch_Code"].ToString();
                                    NARRATION = "UTR Number:" + UTR + ", MessageId:" + MessageId + "";
                                    qry = "INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO,DOC_DATE,AC_CODE,UNIT_code, NARRATION,AMOUNT,TENDER_ID," +
                                          "TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc, progid,tranid)" +
                                          " VALUES ('BK','','" + doc_no + "','" + formattedDate + "','" + Ac_Code + "','0','" + NARRATION + "','" + Amount + "','0','0','0','" + Company_Code + "','" + Year_Code + "','0','D','0','0','0','" + doc_no
                                          + "','" + AcId + "','0','2','0','0');";
                                    clsDAL.SimpleQuery(qry);
                                   
                                    string qry1 = "INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO,DOC_DATE,AC_CODE,UNIT_code, NARRATION,AMOUNT,TENDER_ID," +
                                          "TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc, progid,tranid)" +
                                          " VALUES ('BK','','" + doc_no + "','" + formattedDate + "','" + BankCode + "','0','" + NARRATION + "','" + Amount + "','0','0','0','" + Company_Code + "','" + Year_Code + "','0','C','0','0','0','" + doc_no
                                          + "','" + bcId + "','0','2','0','0');";
                                    clsDAL.SimpleQuery(qry1);
                                }
                            }
                        }
                    }

                    if (IsSuccess)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment In Success!');removeLoader();", true);
                    }

                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('API Call Failed!Please Try Again Later.');removeLoader();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('An error occurred: " + ex.Message + "');removeLoader();", true);
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