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

public partial class Sugar_Transaction_pgeTransactionEntry : System.Web.UI.Page
{
    #region data section
    string Head_Update = string.Empty;
    string Name = string.Empty;
    static WebControl objAsp = null;
    Int64 AccountNumber = 0;
    string IFSC = string.Empty;
    Int64 MobileNo = 0;
    string Remark = string.Empty;
    double Amount = 0;
    string Narration = string.Empty;
    string qry = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryDetail = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string user = string.Empty;
    string SystemMasterTable = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    string cs = string.Empty;
    int Doc_No = 0;
    int flag = 0;
    int TranctionDateT = 0;
    string MessageId = string.Empty;
    Int64 StatusCode = 0;
    Int64 PaymentStatusCode = 0;
    string PaymentStatusRemark = string.Empty;
    string Head_Fields = string.Empty;
    string Head_Values = string.Empty;
    string TranctionDate = string.Empty;
    string StatusDescription = string.Empty;
    string UTRNumber = string.Empty;
    string doc_Date = string.Empty;
    string PaymentType = string.Empty;
    Int32 AcCode = 0;
    Int32 AcId = 0;
    Int32 Bank_Code = 0;
    Int32 bcid = 0;
    int count = 0;

    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            tblHead = "BankTranction";
            tblDetails = "BankTranctionDetail";
            qryDetail = "qryBankTranctionDetail";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMasterTable = tblPrefix + "SystemMaster";
            qryAccountList = "qrymstaccountmaster";
            qryCommon = "qryBankTranction";

            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            string isAuthenticate = string.Empty;
            string Action = string.Empty;
            //hdnfcommisiontype.Value = Request.QueryString["Tran_Type"];
            //drpVouchertype.SelectedValue = hdnfcommisiontype.Value;
            if (!Page.IsPostBack)
            {
                hdnfyearcode.Value = Session["year"].ToString();
                hdnfcompanycode.Value = Session["Company_Code"].ToString();
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");

                string Bank_Security = clsCommon.getString("Select Bank_Security from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["id"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                        // setFocusControl(btnEdit);
                        //  setFocusControl(txtAcCode);
                        if (Bank_Security == "Y")
                        {
                            btnPay.Visible = false;
                            btnPaymentStatus.Visible = false;
                        }
                    }  
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        setFocusControl(txtAcCode);
                    }

                }
                else
                {
                    //Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["id"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                        btnPay.Visible = false;
                        btnPendingTransaction.Visible = false;
                        // setFocusControl(btnEdit);
                        //  setFocusControl(txtAcCode);
                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        btnPay.Visible = false;
                        btnPendingTransaction.Visible = false;
                        this.NextNumber();
                        setFocusControl(txtAcCode);
                    }
                }
            }
            if (objAsp != null)
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
            }

        }
        catch
        {
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.NextNumber();
        setFocusControl(txtAcCode);
        //txtGSTRateCode.Text = "2";
        string gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        pnlPopupDetails.Style["display"] = "none";
        Response.Redirect("pgeTransactionEntry.aspx");

    }
    #endregion




    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region [Validation]
            bool isvalidate = true;
            //Doc_No = Convert.ToInt32(txtdoc_no.Text);

            if (string.IsNullOrEmpty(txtdoc_no.Text) || txtdoc_no.Text == "0")
            {
                isvalidate = false;
                txtdoc_no.Text = string.Empty;
                setFocusControl(txtdoc_no);
            }
            else
            {
                Doc_No = Convert.ToInt32(txtdoc_no.Text);

            }


            // Add validation logic for AcCode
            if (string.IsNullOrEmpty(txtAcCode.Text) || txtAcCode.Text == "0")
            {
                isvalidate = false;
                txtAcCode.Text = string.Empty;
                setFocusControl(txtAcCode);
            }
            else
            {
                AcCode = Convert.ToInt32(txtAcCode.Text);
                AcId = Convert.ToInt32(hdnfacid.Value);
            }

            if (string.IsNullOrEmpty(txtBankCode.Text) || txtBankCode.Text == "0")
            {
                isvalidate = false;
                txtBankCode.Text = string.Empty;
                setFocusControl(txtBankCode);
            }
            else
            {
                Bank_Code = Convert.ToInt32(txtBankCode.Text);
                bcid = Convert.ToInt32(hdnfbcid.Value);
            }

            // Add validation logic for Name
            if (string.IsNullOrEmpty(txtName.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                txtName.Text = string.Empty;
                setFocusControl(txtName);
            }
            else
            {
                Name = txtName.Text;
            }

            // Add validation logic for AccountNumber
            if (string.IsNullOrEmpty(txtAccountNumber.Text) || txtAccountNumber.Text == "0")
            {
                isvalidate = false;
                txtAccountNumber.Text = string.Empty;
                setFocusControl(txtAccountNumber);
            }
            else
            {
                AccountNumber = Convert.ToInt64(txtAccountNumber.Text);
            }

            // Add validation logic for IFSC
            if (string.IsNullOrEmpty(txtIFSCCode.Text) || txtIFSCCode.Text == "0")
            {
                isvalidate = false;
                txtIFSCCode.Text = string.Empty;
                setFocusControl(txtIFSCCode);
            }
            else
            {
                IFSC = txtIFSCCode.Text;
            }

            // Add validation logic for Amount
            //if (string.IsNullOrEmpty(txtamount.Text) || txtamount.Text == "0")
            //{
            //    isvalidate = false;
            //    txtamount.Text = string.Empty;
            //    setFocusControl(txtamount);
            //}
            //else
            //{
            //    Amount = Convert.ToDouble(txtamount.Text);
            //}
            if (string.IsNullOrEmpty(txtamount.Text) || txtamount.Text == "0")
            {
                isvalidate = false;
                txtamount.Text = string.Empty;
                setFocusControl(txtamount);
            }
            else
            {
                double enteredAmount = Convert.ToDouble(txtamount.Text);
                if (enteredAmount <= 20000)
                {
                    Amount = enteredAmount;
                }
                else
                {
                    isvalidate = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Amount Limit Is 20000!')", true); 
                    setFocusControl(txtamount);
                }
            }


            // Add validation logic for Remark
            if (string.IsNullOrEmpty(txtRemark.Text) || txtRemark.Text == "0")
            {
                isvalidate = false;
                txtRemark.Text = string.Empty;
                setFocusControl(txtRemark);
            }
            else
            {
                Remark = txtRemark.Text;
            }

            // Add validation logic for Narration
            if (string.IsNullOrEmpty(txtnarration.Text) || txtnarration.Text == "0")
            {
                isvalidate = false;
                txtnarration.Text = string.Empty;
                setFocusControl(txtnarration);
            }
            else
            {
                Narration = txtnarration.Text;
            }

            // Check if any validation failed
            if (!isvalidate)
            {
                // Display an error message and do not proceed with saving
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validationError", "alert('Please fill in all required fields.')", true);
                return; // Exit the method
            }

            PaymentType = drpPaymentType.Text;
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            MessageId = "0";
            StatusCode = 0;
            StatusDescription = "0";
            UTRNumber = "0";
            PaymentStatusCode = 0;
            PaymentStatusRemark = "0";
            doc_Date = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            //TranctionDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            TranctionDate = "";

            #endregion

            #region Create  Query
            Head_Fields = Head_Fields + "doc_no,";
            Head_Values = Head_Values + "'" + Doc_No + "',";
            Head_Fields = Head_Fields + "Doc_date,";
            Head_Values = Head_Values + "'" + doc_Date + "',";
            Head_Fields = Head_Fields + "Company_Code,";
            Head_Values = Head_Values + "'" + Company_Code + "',";
            Head_Fields = Head_Fields + "Year_Code,";
            Head_Values = Head_Values + "'" + Year_Code + "',";
            Head_Fields = Head_Fields + "AcCode,";
            Head_Values = Head_Values + "'" + AcCode + "',";
            Head_Fields = Head_Fields + "Name,";
            Head_Values = Head_Values + "'" + Name + "',";
            Head_Fields = Head_Fields + "AccountNumber,";
            Head_Values = Head_Values + "'" + AccountNumber + "',";
            Head_Fields = Head_Fields + "IFSC,";
            Head_Values = Head_Values + "'" + IFSC + "',";
            Head_Fields = Head_Fields + "Remark,";
            Head_Values = Head_Values + "'" + Remark + "',";
            Head_Fields = Head_Fields + "Amount,";
            Head_Values = Head_Values + "'" + Amount + "',";
            Head_Fields = Head_Fields + "Narration,";
            Head_Values = Head_Values + "'" + Narration + "',";
            Head_Fields = Head_Fields + "MessageId,";
            Head_Values = Head_Values + "'" + MessageId + "',";
            Head_Fields = Head_Fields + "StatusCode,";
            Head_Values = Head_Values + "'" + StatusCode + "',";
            Head_Fields = Head_Fields + "TranctionDate,";
            Head_Values = Head_Values + "'" + TranctionDate + "',";
            Head_Fields = Head_Fields + "StatusDescription,";
            Head_Values = Head_Values + "'" + StatusDescription + "',";
            Head_Fields = Head_Fields + "UTRNumber,";
            Head_Values = Head_Values + "'" + UTRNumber + "',";
            Head_Fields = Head_Fields + "AcId,";
            Head_Values = Head_Values + "'" + AcId + "',";
            Head_Fields = Head_Fields + "PaymentStatusCode,";
            Head_Values = Head_Values + "'" + PaymentStatusCode + "',";
            Head_Fields = Head_Fields + "PaymentStatusRemark,";
            Head_Values = Head_Values + "'" + PaymentStatusRemark + "',";
            Head_Fields = Head_Fields + "Bank_Code,";
            Head_Values = Head_Values + "'" + Bank_Code + "',";
            Head_Fields = Head_Fields + "bcid,";
            Head_Values = Head_Values + "'" + bcid + "',";
            Head_Fields = Head_Fields + "PaymentType";
            Head_Values = Head_Values + "'" + PaymentType + "'";
            #endregion

            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    flag = 1;
                    qry = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";

                    Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                    thred.Start(); //Thread Operation Start
                    thred.Join();

                    if (count == 1)
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Save !')", true);
                        clsButtonNavigation.enableDisable("S");
                        this.makeEmptyForm("S");
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                    }
                }
                else
                {

                    #region Create Update Query
                    Head_Update = Head_Update + "doc_no=";
                    Head_Update = Head_Update + "'" + Doc_No + "',";
                    Head_Update = Head_Update + "Doc_date=";
                    Head_Update = Head_Update + "'" + doc_Date + "',";
                    Head_Update = Head_Update + "Company_Code=";
                    Head_Update = Head_Update + "'" + Company_Code + "',";
                    Head_Update = Head_Update + "Year_Code=";
                    Head_Update = Head_Update + "'" + Year_Code + "',";
                    Head_Update = Head_Update + "AcCode=";
                    Head_Update = Head_Update + "'" + AcCode + "',";
                    Head_Update = Head_Update + "Name=";
                    Head_Update = Head_Update + "'" + Name + "',";
                    Head_Update = Head_Update + "AccountNumber=";
                    Head_Update = Head_Update + "'" + AccountNumber + "',";
                    Head_Update = Head_Update + "IFSC=";
                    Head_Update = Head_Update + "'" + IFSC + "',";
                    Head_Update = Head_Update + "Remark=";
                    Head_Update = Head_Update + "'" + Remark + "',";
                    Head_Update = Head_Update + "Amount=";
                    Head_Update = Head_Update + "'" + Amount + "',";
                    Head_Update = Head_Update + "Narration=";
                    Head_Update = Head_Update + "'" + Narration + "',";
                    Head_Update = Head_Update + "MessageId=";
                    Head_Update = Head_Update + "'" + MessageId + "',";
                    Head_Update = Head_Update + "StatusCode=";
                    Head_Update = Head_Update + "'" + StatusCode + "',";
                    Head_Update = Head_Update + "TranctionDate=";
                    Head_Update = Head_Update + "'" + TranctionDate + "',";
                    Head_Update = Head_Update + "StatusDescription=";
                    Head_Update = Head_Update + "'" + StatusDescription + "',";
                    Head_Update = Head_Update + "UTRNumber=";
                    Head_Update = Head_Update + "'" + UTRNumber + "',";
                    Head_Update = Head_Update + "AcId=";
                    Head_Update = Head_Update + "'" + AcId + "',";
                    Head_Update = Head_Update + "PaymentStatusCode=";
                    Head_Update = Head_Update + "'" + PaymentStatusCode + "',";
                    Head_Update = Head_Update + "PaymentStatusRemark=";
                    Head_Update = Head_Update + "'" + PaymentStatusRemark + "',";
                    Head_Update = Head_Update + "Bank_Code=";
                    Head_Update = Head_Update + "'" + Bank_Code + "',";
                    Head_Update = Head_Update + "bcid=";
                    Head_Update = Head_Update + "'" + bcid + "',";
                    Head_Update = Head_Update + "PaymentType=";
                    Head_Update = Head_Update + "'" + PaymentType + "'";
                    #endregion

                    flag = 2;
                    qry = "update " + tblHead + " set " + Head_Update + " where Company_Code='" + Company_Code + "' and Year_Code='" + Year_Code + "' and Id='" + hdnf.Value + "'";
                    Thread thred = new Thread(() => { count = DataStore(qry, flag); });
                    thred.Start();
                    thred.Join();
                    qry = "";
                    if (count == 2)
                    {
                        //hdnf.Value = lblbsid.Text;
                        //clsButtonNavigation.enableDisable("S");
                        //this.makeEmptyForm("S");
                        //qry = getDisplayQuery();
                        //this.fetchRecord(qry);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
                        clsButtonNavigation.enableDisable("S");
                        this.makeEmptyForm("S");
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception if needed
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "errorAlert", "alert('An error occurred: " + ex.Message + "')", true);
        }
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        this.makeEmptyForm("E");
        txtdoc_no.Enabled = false;
        setFocusControl(txtAcCode);

    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {

        try
        {
            if (hdconfirm.Value == "Yes")
            {
                DataSet ds = new DataSet();
                string currentDoc_No = txtdoc_no.Text;


                string qry = "";
                qry = "delete from " + tblHead + " where  doc_no=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Records are found, indicating the record was not deleted
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "deleteError", "alert('Record not deleted!')", true);
                }
                else
                {
                    // No records found, indicating the record was deleted successfully
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "deleteSuccess", "alert('Record deleted successfully!')", true);
                    Response.Redirect("TransactionEntryUtility.aspx?id=" + hdnf.Value + "&Action=" + 1);


                }



            }


        }

        catch
        {
        }

    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hdnf.Value = Request.QueryString["id"];

        if (hdnf.Value == "0" || hdnf.Value == "")
        {
            int maxno = Convert.ToInt32(clsCommon.getString("select isnull(max(id),0) as id from  BankTranction "));

            hdnf.Value = Convert.ToString(maxno);
        }
        Response.Redirect("pgeTransactionEntry.aspx?id=" + hdnf.Value + "&Action=" + 1);
    }
    #endregion

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            }

            //counts = Convert.ToInt32(clsCommon.getString("SELECT count(pid) as pid from " + tblHead + " "));
            //if (counts == 0)
            //{
            //    lblLV_Id.Text = "1";
            //    LV_Id = 1;
            //}
            //else
            //{
            //    LV_Id = Convert.ToInt32(clsCommon.getString("SELECT max(pid) as pid from " + tblHead)) + 1;
            //    lblLV_Id.Text = LV_Id.ToString();
            //}
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);

        }
    }
    #endregion

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);

            if (a == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Only Numbers Allow!')", true);
            }
            else
            {
                //string qry = "select * from " + qryHead + " where doc_no='" + txtEditDoc_No.Text + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code="+Session["year"].ToString()+"";
                //this.fetchRecord(qry);
                //setFocusControl(txtEditDoc_No);
                string qry = "select * from " + qryCommon + " where doc_no='" + txtEditDoc_No.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Session["year"].ToString() + "";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
        csCalculations();
    }
    #endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_DATE.Text;
        strTextBox = "txtDOC_DATE";
        csCalculations();
    }
    #endregion

    #region [txtPayment_DATE_TextChanged]
    protected void txtPayment_DATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPayment_DATE.Text;
        strTextBox = "txtDOC_DATE";
        csCalculations();
    }
    #endregion

    #region [btntxtDOC_NO_Click]
    protected void btntxtDOC_NO_Click(object sender, EventArgs e)
    {
        try
        {
            if (btntxtDOC_NO.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtdoc_no.Text = string.Empty;
                txtdoc_no.Enabled = true;
                btnSave.Enabled = false;
                setFocusControl(txtdoc_no);
            }
            if (btntxtDOC_NO.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtdoc_no";
                btnSearch_Click(sender, e);

            }
        }
        catch
        {
        }
    }
    #endregion

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where  id=" + hdnf.Value + "";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region [fetchrecord]
    private bool fetchRecord(string qry)
    {
        try
        {
            bool recordExist = false;
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
                        string yearcode = Session["year"].ToString();
                        if (hdnfyearcode.Value != yearcode)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records year code & current year code is not same !')", true);
                            return false;
                        }
                        string comcode = Session["Company_Code"].ToString();
                        if (hdnfcompanycode.Value != comcode)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records company code & current company code is not same !')", true);
                            return false;
                        }

                        hdnfyearcode.Value = dt.Rows[0]["Year_Code"].ToString();
                        hdnfcompanycode.Value = dt.Rows[0]["Company_Code"].ToString();
                        txtdoc_no.Text = dt.Rows[0]["doc_no"].ToString();
                        hdnf.Value = dt.Rows[0]["Id"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtPayment_DATE.Text = dt.Rows[0]["TranctionDateConverted"].ToString();
                        txtAcCode.Text = dt.Rows[0]["AcCode"].ToString();
                        lblAcName.Text = dt.Rows[0]["Account_Name"].ToString();
                        txtBankCode.Text = dt.Rows[0]["Bank_Code"].ToString();
                        lblBankName.Text = dt.Rows[0]["BankName"].ToString();
                        txtName.Text = dt.Rows[0]["Name"].ToString();
                        txtAccountNumber.Text = dt.Rows[0]["AccountNumber"].ToString();
                        txtIFSCCode.Text = dt.Rows[0]["IFSC"].ToString();
                        txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                        txtamount.Text = dt.Rows[0]["Amount"].ToString();
                        txtnarration.Text = dt.Rows[0]["Narration"].ToString();

                        drpPaymentType.SelectedValue = dt.Rows[0]["PaymentType"].ToString();
                        hdnfacid.Value = dt.Rows[0]["AcId"].ToString();
                        hdnfbcid.Value = dt.Rows[0]["bcid"].ToString();

                         
                        //hdnf.Value = txtdoc_no.Text;
                        recordExist = true;


                        #region  Details
                        qry = "select TranctionDateConverted as TranctionDate,MessageId,StatusCode,StatusDescription,UTRNumber,PaymentStatusCode,PaymentStatusRemark from " + qryDetail + " where tranctionid=" + hdnf.Value;
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    //dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("SrNo", typeof(int)));
                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{
                                    //    dt.Rows[i]["rowAction"] = "N";
                                    //    dt.Rows[i]["SrNo"] = i + 1;
                                    //}
                                    grdDetail.DataSource = dt;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = dt;
                                }
                                else
                                {
                                    grdDetail.DataSource = null;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = null;
                                }
                            }
                            else
                            {
                                grdDetail.DataSource = null;
                                grdDetail.DataBind();
                                ViewState["currentTable"] = null;
                            }
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion
                    }
                }
            }
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion


    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtAcCode")
            {
                string partyName = string.Empty;
                if (txtAcCode.Text != string.Empty)
                {
                    searchString = txtAcCode.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnAcCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfacid.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);
                        qry = "select Bank_Ac_No from " + qryAccountList + " where Ac_Code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        txtAccountNumber.Text = clsCommon.getString(qry);
                        qry = "select IFSC from " + qryAccountList + " where Ac_Code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        txtIFSCCode.Text = clsCommon.getString(qry);
                        qry = "select Mobile_No from " + qryAccountList + " where Ac_Code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        //txtMobileNumber.Text = clsCommon.getString(qry);
                        qry = "select Address_E from " + qryAccountList + " where Ac_Code=" + txtAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        txtRemark.Text = clsCommon.getString(qry);
                        if (partyName != string.Empty)
                        {
                            lblAcName.Text = partyName;
                            txtName.Text = partyName;
                            setFocusControl(txtBankCode);
                        }
                        else
                        {
                            lblAcName.Text = string.Empty;
                            txtAcCode.Text = string.Empty;
                            setFocusControl(txtAcCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtAcCode);
                }
            }


            if (strTextBox == "txtBankCode")
            {
                string BankName = string.Empty;
                if (txtBankCode.Text != string.Empty)
                {
                    searchString = txtBankCode.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnBankCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfbcid.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtBankCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtBankCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        BankName = clsCommon.getString(qry);
                        if (BankName != string.Empty)
                        {
                            lblBankName.Text = BankName;
                            setFocusControl(txtName);
                        }
                        else
                        {
                            lblBankName.Text = string.Empty;
                            txtBankCode.Text = string.Empty;
                            setFocusControl(txtBankCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBankCode);
                }
            }

            if (strTextBox == "txtAcCode")
            {
                setFocusControl(txtBankCode);
            }
            if (strTextBox == "txtBankCode")
            {
                setFocusControl(txtName);
            }


            if (strTextBox == "txtName")
            {

                setFocusControl(txtAccountNumber);
            }
            if (strTextBox == "txtAccountNumber")
            {

                setFocusControl(txtIFSCCode);
            }
            //if (strTextBox == "txtIFSCCode")
            //{

            //    //setFocusControl(txtMobileNumber);
            //}
            //if (strTextBox == "txtMobileNumber")
            //{

            //    setFocusControl(txtRemark);
            //}

            if (strTextBox == "txtRemark")
            {

                setFocusControl(txtamount);
            }

            if (strTextBox == "txtamount")
            {

                setFocusControl(txtnarration);
            }
            if (strTextBox == "txtnarration")
            {

                setFocusControl(btnPay);
            }

        }
        catch
        {
        }
    }
    #endregion
    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAcCode.Text;
        strTextBox = "txtAcCode";
        csCalculations();
    }

    protected void txtBankCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBankCode.Text;
        strTextBox = "txtBankCode";
        csCalculations();
    }

    protected void txtMessageId_TextChanged(object sender, EventArgs e)
    {

    }

    #region [txtName_TextChanged]
    protected void txtName_TextChanged(object sender, EventArgs e)
    {
        searchString = txtName.Text;
        strTextBox = "txtName";
        csCalculations();
    }
    #endregion


    #region [txtAccountNumber_TextChanged]
    protected void txtAccountNumber_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAccountNumber.Text;
        strTextBox = "txtAccountNumber";
        csCalculations();
    }
    #endregion


    #region [txtIFSCCode_TextChanged]
    protected void txtIFSCCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIFSCCode.Text;
        strTextBox = "txtIFSCCode";
        csCalculations();
    }
    #endregion

    #region [txtamount_TextChanged]
    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtamount.Text;
        strTextBox = "txtamount";
        csCalculations();
    }
    #endregion

    #region [txtRemark_TextChanged]
    protected void txtRemark_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRemark.Text;
        strTextBox = "txtRemark";
        csCalculations();
    }
    #endregion


    #region [txtnarration_TextChanged]
    protected void txtnarration_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration.Text;
        strTextBox = "txtnarration";
        csCalculations();
    }
    #endregion





    #region [makeEmptyForm]
    private void makeEmptyForm(string dAction)
    {
        try
        {
            if (dAction == "N")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }
                }
                pnlPopup.Style["display"] = "none";

                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;

                #region set Business logic
                lblAcName.Text = string.Empty;
                txtEditDoc_No.Enabled = true;
                btnAcCode.Enabled = false;
                btnBankCode.Enabled = false;
                calenderExtenderDate.Enabled = false;
                calenderPaymentDate.Enabled = false;
                drpPaymentType.Enabled = false;
                setFocusControl(btnAdd);

                #endregion
            }
            if (dAction == "A")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Change No";
                btntxtDOC_NO.Enabled = true;
                txtdoc_no.Enabled = false;
                txtEditDoc_No.Enabled = false;
                btntxtDOC_NO.Enabled = false;
                btnPay.Enabled = false;
                btnPendingTransaction.Enabled = false;
                #region set Business logic for save
                lblAcName.Text = string.Empty;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                btnAcCode.Enabled = true;
                btnBankCode.Enabled = true;
                calenderExtenderDate.Enabled = true;
                calenderPaymentDate.Enabled = false;
                txtPayment_DATE.Enabled = false;
                drpPaymentType.Enabled = true;

                #endregion

            }
            if (dAction == "S")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                txtEditDoc_No.Enabled = true;
                btnPay.Enabled = true;
                btnPendingTransaction.Enabled = true;
                drpPaymentType.Enabled = false;
                #region set Business logic for save

                btnAcCode.Enabled = false;
                btnBankCode.Enabled = false;
                calenderExtenderDate.Enabled = false;
                calenderPaymentDate.Enabled = false;


                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = true;
                txtPayment_DATE.Enabled = false;
                btnPay.Enabled = false;
                btnPendingTransaction.Enabled = false;
                drpPaymentType.Enabled = true;
                #region logic

                lblAcName.Enabled = true;
                calenderExtenderDate.Enabled = true;
                calenderPaymentDate.Enabled = true;

                #endregion
                txtEditDoc_No.Enabled = false;
                btnAcCode.Enabled = true;
                btnBankCode.Enabled = true;
            }
            #region Always check this
            #endregion
        }
        catch
        {
        }
    }
    #endregion

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = getDisplayQuery();
            bool recordExist = this.fetchRecord(qry);
            if (recordExist == true)
            {
                //btnEdit.Focus();
                btnAdd.Focus();
            }
            else                            //new code
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }


        }

        catch
        {
        }
    }

    #endregion

    protected void btnAcCode_Click(object sender, EventArgs e)
    {

        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAcCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    protected void btnBankCode_Click(object sender, EventArgs e)
    {

        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBankCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }

    protected async void btnPay_Click(object sender, EventArgs e)
    {
        if (hdconfirm.Value == "Yes")
        {

            #region [Validation]
            bool isvalidate = true;
            //Doc_No = Convert.ToInt32(txtdoc_no.Text);

            if (string.IsNullOrEmpty(txtdoc_no.Text) || txtdoc_no.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check Entry  No')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }


            if (string.IsNullOrEmpty(txtName.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check Name')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }


            if (string.IsNullOrEmpty(txtAccountNumber.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check AccountNumber ')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }

            if (string.IsNullOrEmpty(txtIFSCCode.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check IFSCCode')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }

            if (string.IsNullOrEmpty(txtamount.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check Amount')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }

            if (string.IsNullOrEmpty(txtRemark.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check Remark')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }

            if (string.IsNullOrEmpty(txtnarration.Text) || txtName.Text == "0")
            {
                isvalidate = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Check Narration')", true);
                return;
            }
            else
            {
                isvalidate = true;

            }
            Doc_No = Convert.ToInt32(txtdoc_no.Text);
            #endregion


            using (HttpClient client = new HttpClient())
            {
                try
                {

                    string paymentUrl = "http://localhost:8085/api/payment/makepayment";
                    //string statusUrl = "http://localhost:8085/api/payment/checkstatus";

                    PaymentRequestModel paymentRequest = new PaymentRequestModel();

                    PaymentInfoModel paymentInfo = new PaymentInfoModel
                    {
                        //AccountNo = "7111565336",
                        //ClientCode = "NAVKAR007",
                        //DrDesc = "Test Debit Transaction",
                        //MyProdCode = "VPAY",
                        //PayMode = "IMPS",

                        AccountNo = Session["accountNo"].ToString(),
                        ClientCode = Session["clintCode"].ToString(),
                        DrDesc = txtnarration.Text,
                        MyProdCode = "VPAY",
                        PayMode = drpPaymentType.Text,
                        TxnAmnt = txtamount.Text,
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
                        //BeneAcctNo = "50100230364765",
                        //RecBrCd = "HDFC0004887",
                        //BeneName = "Ankush Patil"
                        BeneAcctNo = txtAccountNumber.Text,
                        RecBrCd = txtIFSCCode.Text,
                        BeneName = txtName.Text,
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

                        int Year_Code = Convert.ToInt32(Session["year"].ToString());
                        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
 
                        Head_Fields = Head_Fields + "doc_no,";
                        Head_Values = Head_Values + "'" + Doc_No + "',";
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
                        Head_Fields = Head_Fields + "tranctionid";
                        Head_Values = Head_Values + "'" + hdnf.Value + "'";

                        qry = "insert into BankTranctionDetail (" + Head_Fields + ") values(" + Head_Values + ")";
                        flag = 1;
                        Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                        thred.Start(); //Thread Operation Start
                        thred.Join();

                        if (IsSuccess)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment In Process!');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Payment Failed!');", true);
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

                        int Year_Code = Convert.ToInt32(Session["year"].ToString());
                        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());

                        Head_Fields = Head_Fields + "doc_no,";
                        Head_Values = Head_Values + "'" + Doc_No + "',";
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
                        Head_Fields = Head_Fields + "tranctionid";
                        Head_Values = Head_Values + "'" + hdnf.Value + "'";

                        qry = "insert into BankTranctionDetail (" + Head_Fields + ") values(" + Head_Values + ")";
                        flag = 1;
                        Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                        thred.Start(); //Thread Operation Start
                        thred.Join();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('API Call Failed!');", true);

                    }
                }
                catch (Exception ex)
                { 
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('An error occurred: " + ex.Message + "');", true);
                }
                 
            }
        }
    }


    protected async void btnPaymentStatus_Click(object sender, EventArgs e)
    {
    }

    private void setFocusControl(WebControl wc)
    {
        ScriptManager.GetCurrent(this).SetFocus(wc);
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
                searchString = txtSearchText.Text;
            }

            if (hdnfClosePopup.Value == "txtAcCode")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtAcCode.Text;
                // string qry = "select Ac_Code,Ac_Name_E from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " and Locked=0 and " +
                //" (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E desc";
                // this.showPopup(qry);

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }


            if (hdnfClosePopup.Value == "txtBankCode")
            {

                lblPopupHead.Text = "--Select Bank--";
                txtSearchText.Text = txtBankCode.Text;
                // string qry = "select Ac_Code,Ac_Name_E from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " and Locked=0 and " +
                //" (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E desc";
                // this.showPopup(qry);

                string qry = " Locked=0  and dbo.nt_1_accountmaster.ac_type='B' and dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
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
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [grdPopup_PageIndexC]
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtAcCode" || v == "txtBankCode")
            {
                e.Row.Cells[0].Width = new Unit("100px");
                e.Row.Cells[1].Width = new Unit("400px");
            }
            else
            {
                e.Row.Cells[0].Width = new Unit("100px");
                e.Row.Cells[1].Width = new Unit("400px");
                e.Row.Cells[2].Width = new Unit("200px");
            }

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
                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [Account Master Popup Button Code]
    protected void showPopupAccountMaster(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.myaccountmaster(qry);

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
                        pnlPopup.Style["display"] = "block";
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                        pnlPopup.Style["display"] = "block";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion


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

    protected void drpPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;

        }
        catch
        {
        }
        csCalculations();
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            // { 
            //e.Row.Cells[TranctionDateT].ControlStyle.Width = new Unit("100px");
            //e.Row.Cells[10].Visible = true;
            //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[3].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
            e.Row.Cells[5].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[6].ControlStyle.Width = new Unit("250px");
            int i = 1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 20)
                    {
                        cell.Text = cell.Text.Substring(0, 31) + "..";
                        cell.ToolTip = s;
                    }
                }

            }
            //}
        }
        catch
        {
        }
    }
    #endregion



}