using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;


public partial class Sugar_Transaction_pgebankDetail : System.Web.UI.Page
{
    public DataSet ds = null;
    public DataTable dt = null;
    string AC_Update = string.Empty;

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string cs = string.Empty;
    string Action = string.Empty;
    string retValue;
    string fornotsaverecord;
    string str = string.Empty;
    string AccountMasterTable = string.Empty;
    #endregion
    #region updatepart

    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null;
    DataTable dt1 = null;
    DataTable dt2 = null;

    DataTable SalePurcdt = null;


    DataTable Maindt = null;
    DataRow dr = null;
    string msg = string.Empty;
    int Branch_Code = 0;
    Int32 mill_Code;
    Int32 ShipTo;
    Int32 Tender_No = 0;
    Int32 party = 0;
    string DeliveryType = string.Empty;
    Int32 broker = 0;
    int count = 0;
    Int32 subbroker = 0;
    Int32 Doc_No = 0;
    double quntal = 0.00;
    string saudadate = string.Empty;
    double sale_rate = 0.00;
    double commision_rate = 0.00;
    string payment_Date = string.Empty;
    int tenderID = 0;
    int tenderdetailid = 0;
    int shiptoid = 0;
    double TenderQuntl = 0.00;
    int partyid = 0;
    int brokerid = 0;
    int subbrokerid = 0;
    string dtype = string.Empty;
    string Delivery_Type = "";
    string Narration = "";
    double tcs_rate = 0.00;
    double gst_rate = 0.00;
    double tcs_amt = 0.00;
    double gst_amt = 0.00;
    double cashdiff = 0.00;
    #endregion


    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    string Update_Detail = string.Empty;
    string Whatsapp = string.Empty;
    string Number = string.Empty;
    DataTable tempOTP = null;

    string UserIPAddress = string.Empty;
    string ipAddress1 = string.Empty;
    string ipAddress2 = string.Empty;
    string clientipN = string.Empty;
    string User_Security = string.Empty;
    string EmailAdmin = string.Empty;
    string way = "";
    string mobileNumber = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        con = new SqlConnection(cs);
        user = Session["user"].ToString();
        tblHead = "nt_1_accountmaster";
        AccountMasterTable = "qrymstaccountmaster";
        Head_Fields = new StringBuilder();
        Head_Values = new StringBuilder();
        Head_Update = new StringBuilder();
        ModalPopupVerification.Hide();

        txtVerifyAcNo.Attributes.Add("onmousedown", "return noCopyMouse(event);");
        txtVerifyAcNo.Attributes.Add("onkeydown", "return noCopyKey(event);");

        txtVerifyAcNo2.Attributes.Add("onmousedown", "return noCopyMouse(event);");
        txtVerifyAcNo2.Attributes.Add("onkeydown", "return noCopyKey(event);");

        txtVerifyAcNo3.Attributes.Add("onmousedown", "return noCopyMouse(event);");
        txtVerifyAcNo3.Attributes.Add("onkeydown", "return noCopyKey(event);");

        Maindt = new DataTable();
        dr = null;
        Maindt.Columns.Add("Querys", typeof(string));
        dr = Maindt.NewRow();
        if (!Page.IsPostBack)
        {
            pnlPopup.Style["display"] = "none";
            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
            }
            setFocusControl(drpTrnType);
            btnUpdate.Visible = true;
            btnOtp.Visible = false;
        }
    }

    private void SendMSG()
    {

    }

    private void ReLoad()
    {

    }

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtAC_Code")
            {
                string BP_Account = string.Empty;
                if (txtAC_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtAC_Code.Text);
                    if (a == false)
                    {
                        btnAC_Code_Click(this, new EventArgs());
                    }
                    else
                    {
                        if (drpTrnType.SelectedValue == "AF")
                        {
                            BP_Account = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_type='T' and  Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        }
                        else {
                            BP_Account = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where  Ac_type in('P','M','S') and Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        }
                        if (BP_Account != string.Empty && BP_Account != "0")
                        {
                            qry = "select * from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            lblACName.Text = BP_Account;
                            DataTable dt = new DataTable();
                            DataSet ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        txtBankAcNo.Text = dt.Rows[0]["payBankAc"].ToString();
                                        txtIFSC.Text = dt.Rows[0]["payIfsc"].ToString();
                                        txtbankname.Text = dt.Rows[0]["PayBankName"].ToString();
                                        txtbeneficiary.Text = dt.Rows[0]["BeneficiaryName"].ToString();
                                        txtVerifyAcNo.Text = dt.Rows[0]["VerifyAcNo"].ToString();
                                        txtBankAcNo2.Text = dt.Rows[0]["payBankAc2"].ToString();
                                        txtIFSC2.Text = dt.Rows[0]["payIfsc2"].ToString();
                                        txtbankname2.Text = dt.Rows[0]["PayBankName2"].ToString();
                                        txtbeneficiary2.Text = dt.Rows[0]["BeneficiaryName2"].ToString();
                                        txtVerifyAcNo2.Text = dt.Rows[0]["VerifyAcNo2"].ToString();
                                        txtBankAcNo3.Text = dt.Rows[0]["payBankAc3"].ToString();
                                        txtIFSC3.Text = dt.Rows[0]["payIfsc3"].ToString();
                                        txtbankname3.Text = dt.Rows[0]["PayBankName3"].ToString();
                                        txtbeneficiary3.Text = dt.Rows[0]["BeneficiaryName3"].ToString();
                                        txtVerifyAcNo3.Text = dt.Rows[0]["VerifyAcNo3"].ToString(); 
                                    }
                                }
                            }
                            hdnfacid.Value = clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtBankAcNo.Text = clsCommon.getString("select payBankAc from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtIFSC.Text = clsCommon.getString("select payIfsc from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbankname.Text = clsCommon.getString("select PayBankName from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbeneficiary.Text = clsCommon.getString("select BeneficiaryName from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //String SelectedValue = string.Empty;
                            ////SelectedValue = clsCommon.getString("select FrieghtOrMill from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            ////if (SelectedValue == "0")
                            ////{
                            ////    drpTrnType.SelectedValue = "AF";
                            ////}
                            ////else { 
                            //// drpTrnType.SelectedValue = SelectedValue;
                            ////}

                            //txtBankAcNo2.Text = clsCommon.getString("select payBankAc2 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtIFSC2.Text = clsCommon.getString("select payIfsc2 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbankname2.Text = clsCommon.getString("select PayBankName2 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbeneficiary2.Text = clsCommon.getString("select BeneficiaryName2 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            //txtBankAcNo3.Text = clsCommon.getString("select payBankAc3 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtIFSC3.Text = clsCommon.getString("select payIfsc3 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbankname3.Text = clsCommon.getString("select PayBankName3 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            //txtbeneficiary3.Text = clsCommon.getString("select BeneficiaryName3 from qrymstaccountmaster where Ac_Code='" + txtAC_Code.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            setFocusControl(txtBankAcNo);
                        }
                        else
                        {
                            txtAC_Code.Text = string.Empty;
                            lblACName.Text = string.Empty;
                            if (drpTrnType.SelectedValue == "AF")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('Only Transport Frieght Account!');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('Only Mill And Party Payment Account!');", true);
                            }
                           
                            setFocusControl(txtAC_Code);
                        }
                    }
                }
                else
                {
                    txtAC_Code.Text = string.Empty;
                    lblACName.Text = BP_Account;
                    setFocusControl(txtBankAcNo);
                }
            }

            if (strTextBox == "txtBankAcNo")
            {
                setFocusControl(txtIFSC);
            }


        }
        catch
        {
        }
    }
    #endregion
    #region[txtAC_Code_TextChanged]
    protected void txtAC_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_Code.Text;
        strTextBox = "txtAC_Code";
        csCalculations();
        enableddisabled();
    }
    #endregion


    #region[btnAC_Code_Click]
    protected void btnAC_Code_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                hdnfClosePopup.Value = "txtAC_Code";
                pnlPopup.Style["display"] = "block";
                btnSearch_Click(sender, e);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                hdnfpopup.Value = null;

            }

        }
        catch
        {
        }

    }
    #endregion

    #region [drpTrnType_SelectedIndexChanged]
    protected void drpTrnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAC_Code.Text = string.Empty;
        lblACName.Text = string.Empty;
        txtBankAcNo.Text = string.Empty;
        txtIFSC.Text = string.Empty;
        txtbankname.Text = string.Empty;
        txtbeneficiary.Text = string.Empty;
        txtBankAcNo2.Text = string.Empty;
        txtIFSC2.Text = string.Empty;
        txtbankname2.Text = string.Empty;
        txtbeneficiary2.Text = string.Empty;
        txtBankAcNo3.Text = string.Empty;
        txtIFSC3.Text = string.Empty;
        txtbankname3.Text = string.Empty;
        txtbeneficiary3.Text = string.Empty;
        setFocusControl(drpSelectedBank);
        enableddisabled();
    }
    #endregion

    #region [drpSelectedBank_SelectedIndexChanged]
    protected void drpSelectedBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtAC_Code);
    }
    #endregion

    #region [txtBankAcNo_TextChanged]
    protected void txtBankAcNo_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtVerifyAcNo);
        pnlPopup.Style["display"] = "none";
    }
    #endregion

    #region [txtVerifyAcNo_TextChanged]
    protected void txtVerifyAcNo_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtIFSC);
        pnlPopup.Style["display"] = "none";
    }
    #endregion

    #region [txtIFSC_TextChanged]
    protected void txtIFSC_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbankname);
    }
    #endregion

    #region [txtbankname_TextChanged]
    protected void txtbankname_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbeneficiary);
    }
    #endregion

    #region [txtbeneficiary_TextChanged]
    protected void txtbeneficiary_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtBankAcNo2);
    }
    #endregion

    #region [txtBankAcNo2_TextChanged]
    protected void txtBankAcNo2_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtVerifyAcNo2);
    }
    #endregion

    #region [txtVerifyAcNo2_TextChanged]
    protected void txtVerifyAcNo2_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtIFSC2);
    }
    #endregion

    #region [txtIFSC2_TextChanged]
    protected void txtIFSC2_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbankname2);
    }
    #endregion

    #region [txtbankname2_TextChanged]
    protected void txtbankname2_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbeneficiary2);
    }
    #endregion

    #region [txtbeneficiary2_TextChanged]
    protected void txtbeneficiary2_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtBankAcNo3);
    }
    #endregion



    #region [txtBankAcNo3_TextChanged]
    protected void txtBankAcNo3_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtVerifyAcNo3);
    }
    #endregion

    #region [txtVerifyAcNo3_TextChanged]
    protected void txtVerifyAcNo3_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtIFSC3);
    }
    #endregion

    #region [txtIFSC3_TextChanged]
    protected void txtIFSC3_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbankname3);
    }
    #endregion

    #region [txtbankname3_TextChanged]
    protected void txtbankname3_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        setFocusControl(txtbeneficiary3);
    }
    #endregion

    #region [txtbeneficiary3_TextChanged]
    protected void txtbeneficiary3_TextChanged(object sender, EventArgs e)
    {
        enableddisabled();
        if (drpTrnType.SelectedValue == "AF")
        {
            setFocusControl(btnUpdate);
        }
        else {
            setFocusControl(btnOtp);
        }
       
    }
    #endregion


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {

            string Pin = txtOtpVerification.Text;
            tempOTP = (DataTable)ViewState["tempOTP"];

            string dtUserName = tempOTP.Rows[0]["User_Name"].ToString();
            string dtCompany_Code = tempOTP.Rows[0]["Company_Code"].ToString();
            string dtOtp = tempOTP.Rows[0]["OTP"].ToString();

            if (dtOtp == Pin)
            {
                if (dtUserName == user)
                {
                    if (dtCompany_Code == Convert.ToString(Convert.ToInt32(Session["Company_Code"].ToString())))
                    {


                        ViewState["tempOTP"] = null;
                        tempOTP = null;
                        btnUpdate.Visible = true;
                        btnOtp.Visible = false;
                    }
                    else
                    {

                        ModalPopupVerification.Show();
                        lblWrongOtp.Text = "Wrong OTP Code.Please Enter Correct OTP!";
                        lblResendOtp.Text = "";
                    }
                }
                else
                {

                    ModalPopupVerification.Show();
                    lblWrongOtp.Text = "Wrong OTP Code.Please Enter Correct OTP!";
                    lblResendOtp.Text = "";
                }
            }
            else
            {

                ModalPopupVerification.Show();
                lblWrongOtp.Text = "Wrong OTP Code.Please Enter Correct OTP!";
                lblResendOtp.Text = "";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void resendlnk_Click(object sender, EventArgs e)
    {
    }


    protected void btnOtp_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtAC_Code.Text.TrimStart('0')) && txtAC_Code.Text.TrimStart('0') != "2")
            {
                tempOTP = new DataTable();
                tempOTP.Columns.Add(new DataColumn("User_Name", typeof(string)));
                tempOTP.Columns.Add(new DataColumn("Company_Code", typeof(string)));
                tempOTP.Columns.Add(new DataColumn("OTP", typeof(string)));
                dr = tempOTP.NewRow();
                dr["User_Name"] = user;
                dr["Company_Code"] = Convert.ToInt32(Session["Company_Code"].ToString());
                tempOTP.Rows.Add(dr);
                ViewState["tempOTP"] = tempOTP;
                HideMobileNumber();
                lblMo_Number.Text = mobileNumber;
                HideEmailId();
                lblEmailId.Text = EmailAdmin;
                ModalPopupOTP.Show();
                //   ModalPopupMsg.Hide();

            }
            else
            {

                setFocusControl(txtAC_Code);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected string HideEmailId()
    {
        EmailAdmin = clsCommon.getString("Select EmailId from tblUser where User_Name='" + user.Trim() + "'");


        int atIndex = EmailAdmin.IndexOf('@');
        if (atIndex <= 1)
        {
            return EmailAdmin;
        }

        string maskedPart = new string('*', atIndex - 1);
        string domain = EmailAdmin.Substring(atIndex);
        string newdomain = EmailAdmin.Substring(atIndex - 2);
        EmailAdmin = EmailAdmin[atIndex - 16] + maskedPart + newdomain;

        return EmailAdmin;
    }

    protected string HideMobileNumber()
    {
        mobileNumber = clsCommon.getString("select Mobile_NoWa from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        if (!string.IsNullOrEmpty(mobileNumber) && mobileNumber.Length >= 2)
        {
            string lastTwoDigits = mobileNumber.Substring(mobileNumber.Length - 2);
            string hiddenDigits = new string('*', mobileNumber.Length - 2);
            mobileNumber = hiddenDigits + lastTwoDigits;
            return mobileNumber;
        }
        return mobileNumber;


    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            ModalPopupOTP.Hide();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        string Admin_Mail = clsCommon.getString("Select EmailId from tblUser where User_Name='" + user + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
        EmailAdmin = Session["EmailId"].ToString();
        way = hdnfway.Value;
        OTP otp = new OTP();
        string PIN = otp.OTPassword();
        tempOTP = (DataTable)ViewState["tempOTP"];
        DataRow dr = tempOTP.Rows[0];
        dr["OTP"] = PIN;
        try
        {
            string mailFrom = Session["OtpEmail"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["OtpPassword"].ToString();
            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Host = clsGV.Email_Address;
            msg.From = new MailAddress(mailFrom);
            msg.To.Add(EmailAdmin);
            msg.Body = "Your Customer: " + lblACName.Text + " Bank Details Are Update User:" + user + " OTP: " + "<b>" + PIN + "</b>";
            msg.IsBodyHtml = true;
            msg.Subject = "One Time Password";
            msg.IsBodyHtml = true;
            if (smtpPort != string.Empty)
            {
                SmtpServer.Port = Convert.ToInt32(smtpPort);
            }
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            SmtpServer.Send(msg);

            string instanceid = clsCommon.getString("select Instance_Id from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string accesstoken = clsCommon.getString("select Access_token from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            string respString = string.Empty;
            string Moblie_Number = Session["Mobile_NoWa"].ToString();
            string msgwa = "Your Customer: " + lblACName.Text + " Bank Details Are Update User:" + user + " OTP: ";
            string Url = "https://wawatext.com/api/send.php?number=91" + Moblie_Number + "&type=text&message=" + msgwa + PIN + "&instance_id=" + instanceid + "&access_token=" + accesstoken + "";
            //string Url = "https://wawatext.com/api/send.php?number=91";
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
            StreamReader reder = new StreamReader(resp.GetResponseStream());
            respString = reder.ReadToEnd();
            reder.Close();
            resp.Close();

            string str = respString;
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace(":", "");
            str = str.Replace(",", "");
            str = str.Replace("\"", "");
            string sub2 = "success";
            bool b = str.Contains(sub2);

            string sub4 = "error";
            bool s = str.Contains(sub4);

            if (b)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Successfully Sent!');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Cloud Not Sent!');", true);
            }
            ModalPopupOTP.Hide();
            ModalPopupVerification.Show();
        }
        catch (Exception e1)
        {
            Response.Write("<script>alert('Error sending OTP');</script>");
            return;
        }
    }


    protected void btnCancelOtp_Click(object sender, EventArgs e)
    {
        try
        {
            //  Response.Redirect("~/login1.aspx", false);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string BankAcNo = string.Empty;
        int flag = 0;
        string IFSC = string.Empty;
        string BankName = string.Empty;
        string BeneficiaryName = string.Empty;
        string BankAcNo2 = string.Empty;
        string IFSC2 = string.Empty;
        string Bankname2 = string.Empty;
        string BeneficiaryName2 = string.Empty;
        string BankAcNo3 = string.Empty;
        string IFSC3 = string.Empty;
        string Bankname3 = string.Empty;
        string BeneficiaryName3 = string.Empty;
        string SelectedBank = string.Empty;
        string VerifyAcNo = string.Empty;
        string VerifyAcNo2 = string.Empty;
        string VerifyAcNo3 = string.Empty;
        if (txtAC_Code.Text == "" || txtAC_Code.Text == "2")
        {
            setFocusControl(txtAC_Code);
            return;

        }

        if (txtBankAcNo.Text != txtVerifyAcNo.Text) {
            setFocusControl(txtVerifyAcNo);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Bank A/c No & Verify A/c No Are Not a Same!');", true); 
            return;
        }

        if (txtBankAcNo2.Text != txtVerifyAcNo2.Text)
        {
            setFocusControl(txtVerifyAcNo2);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Bank A/c No 2 & Verify A/c No 2 Are Not a Same!');", true);
            return;
        }

        if (txtBankAcNo3.Text != txtVerifyAcNo3.Text)
        {
            setFocusControl(txtVerifyAcNo3);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Bank A/c No 3 & Verify A/c No 3 Are Not a Same!');", true);
            return;
        }

        Int32 accode = txtAC_Code.Text != string.Empty ? Convert.ToInt32(txtAC_Code.Text) : 0;
        Int32 acid = hdnfacid.Value != string.Empty ? Convert.ToInt32(hdnfacid.Value) : 0;
        BankAcNo = txtBankAcNo.Text != string.Empty ? Convert.ToString(txtBankAcNo.Text) : "0";
        IFSC = txtIFSC.Text != string.Empty ? Convert.ToString(txtIFSC.Text) : "0";
        BankName = txtbankname.Text != string.Empty ? Convert.ToString(txtbankname.Text) : "0";
        BeneficiaryName = txtbeneficiary.Text != string.Empty ? Convert.ToString(txtbeneficiary.Text) : "0";
        BankAcNo2 = txtBankAcNo2.Text != string.Empty ? Convert.ToString(txtBankAcNo2.Text) : "0";
        IFSC2 = txtIFSC2.Text != string.Empty ? Convert.ToString(txtIFSC2.Text) : "0";
        Bankname2 = txtbankname2.Text != string.Empty ? Convert.ToString(txtbankname2.Text) : "0";
        BeneficiaryName2 = txtbeneficiary2.Text != string.Empty ? Convert.ToString(txtbeneficiary2.Text) : "0";

        BankAcNo3 = txtBankAcNo3.Text != string.Empty ? Convert.ToString(txtBankAcNo3.Text) : "0";
        IFSC3 = txtIFSC3.Text != string.Empty ? Convert.ToString(txtIFSC3.Text) : "0";
        Bankname3 = txtbankname3.Text != string.Empty ? Convert.ToString(txtbankname3.Text) : "0";
        BeneficiaryName3 = txtbeneficiary3.Text != string.Empty ? Convert.ToString(txtbeneficiary3.Text) : "0";
        SelectedBank = drpSelectedBank.SelectedValue != string.Empty ? Convert.ToString(drpSelectedBank.SelectedValue) : "0";

        VerifyAcNo = txtVerifyAcNo.Text != string.Empty ? Convert.ToString(txtVerifyAcNo.Text) : "0";
        VerifyAcNo2 = txtVerifyAcNo2.Text != string.Empty ? Convert.ToString(txtVerifyAcNo2.Text) : "0";
        VerifyAcNo3 = txtVerifyAcNo3.Text != string.Empty ? Convert.ToString(txtVerifyAcNo3.Text) : "0";
        // BeneficiaryName  =   ValidateAndTrimField(txtbeneficiary.Text, 50);

        if (BeneficiaryName.Length > 24)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('BeneficiaryName Maximum Length 24 Characters!');", true);
            setFocusControl(txtbeneficiary);
            return;
        }
        if (BeneficiaryName2.Length > 24)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('BeneficiaryName2 Maximum Length 24 Characters!');", true);
            setFocusControl(txtbeneficiary2);
            return;
        }

        if (BeneficiaryName3.Length > 24)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('BeneficiaryName3 Maximum Length 24 Characters!');", true);
            setFocusControl(txtbeneficiary3);
            return;
        }

        if (IFSC.Length != 11)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('IFSC Maximum Length 11 Characters!');", true);
            setFocusControl(txtIFSC);
            return;
        }

        if (IFSC2.Length != 11)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('IFSC2 Maximum Length 11 Characters!');", true);
            setFocusControl(txtIFSC2);
            return;
        }

        if (IFSC3.Length != 11)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('IFSC3 Maximum Length 11 Characters!');", true);
            setFocusControl(txtIFSC3);
            return;
        }

        #region Create Update Query
        AC_Update = AC_Update + "payBankAc=";
        AC_Update = AC_Update + "'" + BankAcNo + "',";
        AC_Update = AC_Update + "payIfsc=";
        AC_Update = AC_Update + "'" + IFSC + "',";
        AC_Update = AC_Update + "PayBankName=";
        AC_Update = AC_Update + "'" + BankName + "',";
        AC_Update = AC_Update + "BeneficiaryName=";
        AC_Update = AC_Update + "'" + BeneficiaryName + "',";
        AC_Update = AC_Update + "payBankAc2=";
        AC_Update = AC_Update + "'" + BankAcNo2 + "',";
        AC_Update = AC_Update + "payIfsc2=";
        AC_Update = AC_Update + "'" + IFSC2 + "',";
        AC_Update = AC_Update + "PayBankName2=";
        AC_Update = AC_Update + "'" + Bankname2 + "',";
        AC_Update = AC_Update + "BeneficiaryName2=";
        AC_Update = AC_Update + "'" + BeneficiaryName2 + "',";
        AC_Update = AC_Update + "payBankAc3=";
        AC_Update = AC_Update + "'" + BankAcNo3 + "',";
        AC_Update = AC_Update + "payIfsc3=";
        AC_Update = AC_Update + "'" + IFSC3 + "',";
        AC_Update = AC_Update + "PayBankName3=";
        AC_Update = AC_Update + "'" + Bankname3 + "',"; 
        AC_Update = AC_Update + "BeneficiaryName3=";
        AC_Update = AC_Update + "'" + BeneficiaryName3 + "',";
        AC_Update = AC_Update + "SelectedBank=";
        AC_Update = AC_Update + "'" + SelectedBank + "',";
        AC_Update = AC_Update + "VerifyAcNo=";
        AC_Update = AC_Update + "'" + VerifyAcNo + "',";
        AC_Update = AC_Update + "VerifyAcNo2=";
        AC_Update = AC_Update + "'" + VerifyAcNo2 + "',";
        AC_Update = AC_Update + "VerifyAcNo3=";
        AC_Update = AC_Update + "'" + VerifyAcNo3 + "'";
        #endregion

        flag = 2;
        qry = "update " + tblHead + " set " + AC_Update + " where accoid=" + hdnfacid.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
        Thread thred = new Thread(() => { count = DataStore(qry, flag); });
        thred.Start();
        thred.Join();


        if (count == 2)
        {
            txtAC_Code.Text = string.Empty;
            lblACName.Text = string.Empty;
            txtBankAcNo.Text = string.Empty;
            txtVerifyAcNo.Text = string.Empty;
            txtbankname.Text = string.Empty;
            txtIFSC.Text = string.Empty;
            txtbeneficiary.Text = string.Empty;

            txtBankAcNo2.Text = string.Empty;
            txtbankname2.Text = string.Empty;
            txtIFSC2.Text = string.Empty;
            txtbeneficiary2.Text = string.Empty;
            txtVerifyAcNo2.Text = string.Empty;

            txtBankAcNo3.Text = string.Empty;
            txtbankname3.Text = string.Empty;
            txtIFSC3.Text = string.Empty;
            txtbeneficiary3.Text = string.Empty;
            txtVerifyAcNo3.Text = string.Empty;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuccessMessage", "alert('Record Successfully Updated!'); window.reload();", true);
        }
    }


    private string ValidateAndTrimField(string input, int maxLength)
    {
        if (!string.IsNullOrEmpty(input) && input.Length > maxLength)
        {
            // Truncate the string to the maximum allowed length
            input = input.Substring(0, maxLength);
            // You can also display an error message here if needed
        }
        return input;
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



    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            //if (hdnfClosePopup.Value == "txtmillcode")
            //{
            //    setFocusControl(txtmillcode);
            //}


            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "Close")
            {
                txtSearchText.Text = string.Empty;
                pnlPopup.Style["display"] = "none";
                grdPopup.DataSource = null;
                grdPopup.DataBind();
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            }
            else
            {
                pnlPopup.Style["display"] = "block";

                searchString = txtSearchText.Text;
                strTextBox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType != DataControlRowType.Pager)
            {

                e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("100px");
                e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
                e.Row.Cells[3].ControlStyle.Width = new Unit("200px");
                e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
                e.Row.Cells[5].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[6].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[7].ControlStyle.Width = new Unit("40px");
                e.Row.Cells[8].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[9].ControlStyle.Width = new Unit("40px");
            }

            if (v == "txtAC_Code")
            {
                //if (e.Row.RowType != DataControlRowType.Pager)
                //{
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
           e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //searchString = "";
            string searchtxt = searchString;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchString;
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }


            if (hdnfClosePopup.Value == "txtAC_Code")
            {
                //txtSearchText.Text = txtBroker.Text;
                lblPopupHead.Text = "--Select BP Account--";
                if (drpTrnType.SelectedValue == "AF")
                {
                    string qry = "select Ac_Code , Ac_Name_E ,cityname  from " + AccountMasterTable + " where Ac_type='T' and   Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                    this.showPopup(qry);
                }
                else
                {
                    string qry = "select Ac_Code , Ac_Name_E ,cityname  from " + AccountMasterTable + " where Ac_type in('P','M','S') and   Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                    this.showPopup(qry);
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Popup Button Code]
    protected void showPopup(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdPopup.DataSource = dt;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                        //pnlPopup.Style["display"] = "block";

                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                        // pnlPopup.Style["display"] = "block";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region enableddisabled
    private void enableddisabled()
    {
        if (btnUpdate.Visible == true)
        {
            if (drpTrnType.SelectedValue == "AF")
            {

                btnUpdate.Visible = true;
                btnOtp.Visible = false;
            }
            else
            {
                btnUpdate.Visible = false;
                btnOtp.Visible = true;
            }
        }
        else if (btnOtp.Visible == true)
        {
            if (drpTrnType.SelectedValue == "AF")
            {

                btnUpdate.Visible = true;
                btnOtp.Visible = false;
            }
            else
            {
                btnUpdate.Visible = false;
                btnOtp.Visible = true;
            }
        }
    }
    #endregion
}