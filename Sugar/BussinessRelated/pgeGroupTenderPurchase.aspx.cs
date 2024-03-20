using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.Text;
using System.Web.Services;

public partial class Sugar_BussinessRelated_pgeGroupTenderPurchase : System.Web.UI.Page
{

    public DataSet ds = null;
    public DataTable dt = null;
    Hashtable hash = null;
    string tblHeadVoucher = string.Empty;
    string GLedgerTable = string.Empty;
    public string tableName { get; set; }
    public string code { get; set; }
    string strTextBox = string.Empty;
    string cs = string.Empty;
    string Action = string.Empty;
    //int Tender_No = 0;
    int id = 0;
    int flag = 0;
    int count = 0;
    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    LocalVoucher LV = null;
    int voucher_no = 0;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;
    string qry1 = string.Empty;


    DataTable dt1 = null;
    DataTable dt2 = null;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string qryHead = string.Empty;
    string qryDetail = string.Empty;
    string strTextbox = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;
    string Tran_Type = "LV";
    string millShortName = string.Empty;
    string DOShortname = string.Empty;
    string voucherbyshortname = string.Empty;
    string AUTO_VOUCHER = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string retValue;
    string fornotsaverecord;
    #endregion
    #region Head Declaration Field
    string Lifting_Date_Head = String.Empty;
    string Tender_Date = String.Empty;
    Int32 Mill_Code = 0;
    string Sell_Note_No = String.Empty;
    double Mill_Rate = 0.00;
    string Grade = String.Empty;
    double Quantal = 0;
    int Packing = 0;
    int Bags = 0;
    double VOUCHERAMOUNT = 0;
    double DIFF = 0.00;
    bool isNumeric;
    int n;
    double PURCHASE_RATE = 0;
    int Payment_To, Tender_From, Tender_DO, Voucher_By;
    Int32 VoucherNo = 0;
    float m = 0;
    float Excise_Rate = 0;
    Int32 GstRate_Code = 0;
    string Narration = string.Empty;
    string userName = string.Empty;
    float Purc_Rate = 0;
    string type = string.Empty;
    int Branch_Id = 0;
    string Created_By = clsGV.user;
    string Modified_By = clsGV.user;
    string myNarration = string.Empty;
    double Brokrage = 0.00;
    string str = string.Empty;
    double Diff_Amount = 0.00;
    int docno = 0;
    Int32 Tender_No = 0;
    string Year_Code = string.Empty;
    Int32 Company_Code = 0;
    Int32 mc = 0;
    Int32 itemcode = 0;
    string season = string.Empty;
    Int32 pt = 0;
    Int32 tf = 0;
    Int32 vb = 0;
    Int32 bk = 0;
    Int32 ic = 0;
    Int32 td = 0;
    Int32 Rc = 0;
    double CashDiff = 0.00;

    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    #endregion

    #region Detail Declare Field
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;

    Int32 Buyer = 0;
    double Buyer_Quantal = 0.00;
    double Sale_Rate = 0.00;
    double Commission_Rate = 0.00;
    string Narration1 = "";
    // DateTime.Now.ToString("dd/MM/yyyy");
    string dtype = string.Empty;
    int subBroker = 0;
    string Lifting_Date1 = string.Empty;
    int ID = 0;
    double Buyer_Party = 0;

    #endregion
    #region Grid Fields
    int Detail_Id = 2;
    int Member = 3;
    int Name_of_Member = 4;
    int quantal = 5;
    int saleRate = 6;
    int Detail_paid = 7;
    int ItemAmount = 8;
    int Sauda_Narration = 9;
    int trnderdetailid = 10;
    int rowAction = 11;
    int SrNo = 12;
    int memberid = 13;
    int Paid_Date = 14;
    int Profit = 15;
    int otherpaidAc = 16;
    int otherpaidAcName = 17;
    int otherPaid = 18;
    int otherPaidid = 19;
    int IndivisulProfit = 20;
    int MillContribution = 21;
    int Paidorreseive = 22;
    int saudaReverseAc = 23;
    int saudaReverseId = 24;
    int saudaReverseName = 25;
    int RatePerQtl = 26;
    int saudaReverseAmount = 27;
    
 

    #endregion


    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region [tn]
            ViewState["tn"] = Request.QueryString["tn"];
            ViewState["source"] = Request.QueryString["source"];
            hdnfmillpaymentdate.Value = Session["millpaymentdate"].ToString();
            #endregion


            tblHeadVoucher = tblPrefix + "voucher";
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "GroupTenderPurchase";
            tblDetails = "GroupTenderDetails";
            AccountMasterTable = "qrymstaccountmaster";
            qryCommon = "qrytenderheaddetail";
            GLedgerTable = tblPrefix + "GLEDGER";
            qryDetail = "qrytenderdetail";
            qryHead = "qrygrouptenderhead";
            defaultAccountCode = Convert.ToInt32(clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Name_E='Self'"));
            pnlPopup.Style["display"] = "none";
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);


            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();

            Detail_Fields = new StringBuilder();
            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();
            Detail_Values = new StringBuilder();
            Detail_Update = new StringBuilder();
            Detail_Delete = new StringBuilder();
            Head_Update = new StringBuilder();
            if (!Page.IsPostBack)
            {
                isAuthenticate = "1";

                //isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {

                        hdnf.Value = Request.QueryString["grouptenderid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        pnlgrdDetail.Enabled = true;
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        //this.DebitCreditDiff();
                        this.showLastRecord();
                        this.enableDisableNavigateButtons();
                        setFocusControl(btnAdd);


                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        #region add self row into grid
                        if (grdDetail.Rows.Count == 0)
                        {
                            this.btnADDBuyerDetails_Click(sender, e);
                        }
                        #endregion
                        setFocusControl(txtGroup);
                    }

                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            //if (objAsp != null)
            //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);

            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
                //if (objAsp != null)
            }
        }
        catch
        {
            //  Response.Redirect("http://localhost:3994/HomePage/pgeloginForm.aspx");
        }
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


                    if (c is System.Web.UI.WebControls.HiddenField)
                    {
                        ((System.Web.UI.WebControls.HiddenField)c).Value = "";
                    }

                }
                txtEditDoc_No.Enabled = true;
                pnlPopup.Style["display"] = "none";
                //Button1.Enabled = false;
                btnSave.Text = "Save";
                btnSave.Enabled = false;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                txtMemberName.Enabled = false;
                btnMember.Enabled = false;
                lblMsg.Text = string.Empty;
                drCalculateStock.Enabled = false;
                drAccounted.Enabled = false;

                txtotherpaidAc.Enabled = false;
                btnotherpaidAc.Enabled = false;

                txtsaudaReverseAc.Enabled = false;
                btnsaudaReverseAc.Enabled = false;

                txtRatePerQtl.Enabled = false;
                txtReverseAmount.Enabled = false;
                lblOtherPaidName.Text = string.Empty;
                txtotherPaid.Enabled = false;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btntxtitem_code.Enabled = false;
                ViewState["currentTable"] = null;
                btnProsess.Enabled = true;
                txtBuyerQuantal.Enabled = false;
                txtBuyerSaleRate.Enabled = false;
                txtDetailpaidDate.Enabled = false;
                txtItemAmount.Enabled = false;
                txtBuyerNarration.Enabled = false;
                btnADDBuyerDetails.Enabled = false;
                btnClose.Enabled = false;
                 
                txtPaid.Enabled = false;
                txtDetailpaidDate.Enabled = false;

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
                    if (c is System.Web.UI.WebControls.HiddenField)
                    {
                        ((System.Web.UI.WebControls.HiddenField)c).Value = "";
                    }
                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }
                }
                txtEditDoc_No.Enabled = false;
                btnSave.Text = "Save";
                btnChangeNo.Text = "Change No";
                btnChangeNo.Enabled = false;
                drCalculateStock.Text = "Y";
                drAccounted.Text = "N";
                #region set Business logic for save
                string start = DateTime.Now.ToShortDateString(); //clsCommon.getString("select Convert(varchar(10),GETDATE(),103) as d");
                DateTime startdate = DateTime.Parse(start);
                txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy"); //startdate.ToShortDateString();
                //  clsCommon.getString("select Convert(varchar(10),DATEADD(day,15,getdate()),103) as d");
                int millpaymentdate = Convert.ToInt32(hdnfmillpaymentdate.Value);
                txtLiftingDate.Text = DateTime.Today.AddDays(millpaymentdate).ToString("dd/MM/yyyy"); //liftingdate.ToString("dd/MM/yyyy");

                // DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
                txtTenderNo.Enabled = false;
                pnlgrdDetail.Enabled = true;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                txtDetailpaidDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            
                //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
                //DateTime DetailLift = DateTime.Parse(Dlift);


                string dd = "";

                dd = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                DateTime Headliftingdate = Convert.ToDateTime(dd);
                string Docdate = Headliftingdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string Dlift = System.DateTime.Now.ToString("yyyy/MM/dd");
                DateTime DetailLift = Convert.ToDateTime(Dlift);


                ViewState["currentTable"] = null;
                btntxtitem_code.Enabled = true;
                #endregion
                txtBuyerQuantal.Enabled = true;
                txtBuyerSaleRate.Enabled = true;
                txtItemAmount.Enabled = true;
                txtBuyerNarration.Enabled = true;
                btnADDBuyerDetails.Enabled = true;
                btnClose.Enabled = true;
                txtPaid.Enabled = true;
                txtDetailpaidDate.Enabled = true;
                txtMemberName.Enabled = true;
                btnMember.Enabled = true;
                drCalculateStock.Enabled = true;
                drAccounted.Enabled = true;


                txtotherpaidAc.Enabled = true;
                btnotherpaidAc.Enabled = true;
                txtotherPaid.Enabled = true;


                txtsaudaReverseAc.Enabled = true;
                btnsaudaReverseAc.Enabled = true;

                txtRatePerQtl.Enabled = true;
                txtReverseAmount.Enabled = true;
                btnProsess.Enabled = false;
                txtitem_code.Text = "1";
                lblitemname.Text = clsCommon.getString("select System_Name_E from nt_1_systemmaster where System_Code=1 and " +
                    "Company_Code=" + Session["Company_Code"].ToString() + " and System_Type='I'");





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

                txtEditDoc_No.Enabled = true;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                txtBuyerQuantal.Enabled = false;
                txtBuyerSaleRate.Enabled = false;
                txtItemAmount.Enabled = false;
                txtBuyerNarration.Enabled = false;
                btnADDBuyerDetails.Enabled = false;
                btnClose.Enabled = false;

                drCalculateStock.Enabled = false;
                drAccounted.Enabled = false;
                btnProsess.Enabled = true;
                txtMemberName.Enabled = false;
                btnMember.Enabled = false;
                btntxtitem_code.Enabled = false;


                txtotherpaidAc.Enabled = false;
                btnotherpaidAc.Enabled = false;
                txtotherPaid.Enabled = false;


                txtsaudaReverseAc.Enabled = false;
                btnsaudaReverseAc.Enabled = false;

                txtRatePerQtl.Enabled = false;
                txtReverseAmount.Enabled = false;

                txtPaid.Enabled = false;
                txtDetailpaidDate.Enabled = false;
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


                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
                //txtTenderNo.Enabled = true;
                btnChangeNo.Text = "Choose No";
                txtDetailpaidDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                btnChangeNo.Enabled = false;
                lblMsg.Text = string.Empty;
                txtEditDoc_No.Enabled = false;
                pnlgrdDetail.Enabled = true;
                txtMemberName.Enabled = true;
                btnMember.Enabled = true;

                txtotherpaidAc.Enabled = true;
                btnotherpaidAc.Enabled = true;
                txtotherPaid.Enabled = true;


                txtsaudaReverseAc.Enabled = true;
                btnsaudaReverseAc.Enabled = true;

                txtRatePerQtl.Enabled = true;
                txtReverseAmount.Enabled = true;
                btntxtitem_code.Enabled = true;
                btnProsess.Enabled = false;

                txtBuyerQuantal.Enabled = true;
                txtBuyerSaleRate.Enabled = true;
                txtItemAmount.Enabled = true;
                txtBuyerNarration.Enabled = true;
                btnADDBuyerDetails.Enabled = true;
                btnClose.Enabled = true;

                drCalculateStock.Enabled = true;
                drAccounted.Enabled = true;

                txtPaid.Enabled = true;
                txtDetailpaidDate.Enabled = true;
            }


            #region common
            if (dAction == "S" || dAction == "N")
            {
                pnlgrdDetail.Enabled = false;

                btnMillCode.Enabled = false;
                btnourTenderNo.Enabled = false;
                btnmillDeffAc.Enabled = false;
                btnGrade.Enabled = false;
                btnGroup.Enabled = false;
                // Button1.Enabled = false;
                calenderExtenderDate.Enabled = false;
                calenderExtenderLiftingdate.Enabled = false;
                calendertxtDetailpaidDate.Enabled = false;
            }
            if (dAction == "A" || dAction == "N")
            {
                lblMillName.Text = string.Empty;
                lblMsg.Text = string.Empty;


                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }

            if (dAction == "A" || dAction == "E")
            {
                btnMillCode.Enabled = true;
                btnourTenderNo.Enabled = true;
                btnmillDeffAc.Enabled = true;
                btnGrade.Enabled = true;
                btnGroup.Enabled = true;
                btnGrade.Enabled = true;

                // Button1.Enabled = true;
                calenderExtenderDate.Enabled = true;
                calenderExtenderLiftingdate.Enabled = true;
                calendertxtDetailpaidDate.Enabled = true;
            }
            #endregion
            // AutoPostBackControl.Dispose();

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
                this.enableDisableNavigateButtons();
                btnAdd.Focus();
            }
            else                     //new code
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

    private string getDisplayQuery()
    {
        try
        {
            //string qryDisplay = "select * from " + tblHead +
            //    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and group_Code=" + hdnf.Value;

            string qryDisplay = "select * from " + qryHead +
                " where grouptenderid =" + hdnf.Value;
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }


    #region [btnClose_Click]
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtMemberName.Text = string.Empty;
        lblMemberName.Text = string.Empty;
        txtBuyerQuantal.Text = string.Empty;
        txtBuyerSaleRate.Text = string.Empty;
        txtItemAmount.Text = string.Empty;
        txtBuyerNarration.Text = string.Empty;
        lblno.Text = string.Empty;
        txtPaid.Text = string.Empty;
        btnADDBuyerDetails.Text = "ADD";
        // pnlPopupTenderDetails.Style["display"] = "none";
        hdnfNextFocus.Value = "";
        btnSave.Focus();
    }
    #endregion

    #region [btnADDBuyerDetails_Click]
    protected void btnADDBuyerDetails_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfmember.Value != "0")
            {
            hdnfNextFocus.Value = "";
            int rowIndex = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt = new DataTable();
            if (ViewState["currentTable"] != null)
            {
                dt = (DataTable)ViewState["currentTable"];

                if (btnADDBuyerDetails.Text == "ADD")
                {
                    
                        dr = dt.NewRow();

                        #region calculate rowindex

                        int maxIndex = 0;

                        int[] index = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            index[i] = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        }
                        if (index.Length > 0)
                        {
                            for (int i = 0; i < index.Length; i++)
                            {
                                if (index[i] > maxIndex)
                                {
                                    maxIndex = index[i];
                                }
                            }
                            rowIndex = maxIndex + 1;
                        }
                        else
                        {
                            rowIndex = maxIndex;          //0
                        }
                        #endregion

                        //rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                   
                }
                else
                {
                    //update row
                    int n = Convert.ToInt32(lblno.Text);
                    rowIndex = Convert.ToInt32(lblID.Text);
                    dr = (DataRow)dt.Rows[n - 1];
                    dr["ID"] = rowIndex;
                    dr["SrNo"] = 0;

                    #region decide whether actual row is updating or virtual [rowAction]
                    string id = clsCommon.getString("select ID from " + tblDetails + " where  trnderdetailid='" + lbltenderdetailid.Text + "'");
                    if (id != string.Empty && id != "0")
                    {
                        dr["rowAction"] = "U";   //actual row
                    }
                    else
                    {
                        id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='" + lblID.Text
                            + "' and IsActive='False' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code="
                            + Convert.ToInt32(Session["year"].ToString()) + "");
                        if (id != string.Empty && id != "0")  //added but deleted by another user
                        {
                            dr["rowAction"] = "N";
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }
                    }

                    #endregion
                }

            }
            else
            {
                rowIndex = 1;
                dr = null;

                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Member", typeof(int)));
                dt.Columns.Add(new DataColumn("Name_of_Member", typeof(string)));

                dt.Columns.Add(new DataColumn("Quantal", typeof(double)));
                dt.Columns.Add(new DataColumn("Sale_Rate", typeof(float)));
                dt.Columns.Add(new DataColumn("Detail_paid", typeof(float)));

                dt.Columns.Add(new DataColumn("ItemAmount", typeof(double)));
                dt.Columns.Add(new DataColumn("Sauda_Narration", typeof(string)));
                dt.Columns.Add(new DataColumn("trnderdetailid", typeof(int)));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));

                dt.Columns.Add(new DataColumn("SrNo", typeof(int)));
                dt.Columns.Add(new DataColumn("memberid", typeof(int)));
                dt.Columns.Add(new DataColumn("Paid_Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Profit", typeof(string)));
                dt.Columns.Add(new DataColumn("otherpaidAc", typeof(string)));
                dt.Columns.Add(new DataColumn("otherpaidAcName", typeof(string)));
                dt.Columns.Add(new DataColumn("otherPaid", typeof(string)));
                dt.Columns.Add(new DataColumn("otherPaidid", typeof(string))); 
                dt.Columns.Add(new DataColumn("IndivisulProfit", typeof(string)));
                dt.Columns.Add(new DataColumn("MillContribution", typeof(string)));
                dt.Columns.Add(new DataColumn("Paidorreseive", typeof(string)));
                dt.Columns.Add(new DataColumn("saudaReverseAc", typeof(string)));
                dt.Columns.Add(new DataColumn("saudaReverseId", typeof(string)));
                dt.Columns.Add(new DataColumn("saudaReverseName", typeof(string)));
                dt.Columns.Add(new DataColumn("RatePerQtl", typeof(string)));
                dt.Columns.Add(new DataColumn("saudaReverseAmount", typeof(string)));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }

            if (rowIndex != 1)
            {
                dr["Member"] = Convert.ToInt32(txtMemberName.Text);
                dr["Name_of_Member"] = lblMemberName.Text;

              
                if (!string.IsNullOrEmpty(txtotherpaidAc.Text))
                {
                    string trimmedText = txtotherpaidAc.Text.Trim();
                    double otherPaid;

                    if (double.TryParse(trimmedText, out otherPaid))
                    {
                        dr["otherpaidAc"] = otherPaid;
                        dr["otherpaidAcName"] = lblOtherPaidName.Text;
                    }
                    else
                    { 
                        dr["otherpaidAc"] = 0;
                    }
                }
                else
                {
                    dr["otherpaidAc"] = 0;
                    dr["otherpaidAcName"] = 0;
                }

                if (!string.IsNullOrEmpty(txtsaudaReverseAc.Text))
                {
                    string trimmedText = txtsaudaReverseAc.Text.Trim();
                    double saudaReverseAc;

                    if (double.TryParse(trimmedText, out saudaReverseAc))
                    {
                        dr["saudaReverseAc"] = saudaReverseAc;
                        dr["saudaReverseName"] = lblsaudaReverseAcName.Text;
                    }
                    else
                    {
                        dr["saudaReverseAc"] = 0;
                    }
                }
                else
                {
                    dr["saudaReverseAc"] = 0;
                    dr["saudaReverseName"] = 0;
                }

                if (!string.IsNullOrEmpty(txtotherPaid.Text))
                {
                    string trimmedText = txtotherPaid.Text.Trim();
                    double otherPaid;

                    if (double.TryParse(trimmedText, out otherPaid))
                    {
                        dr["otherPaid"] = otherPaid;
                    }
                    else
                    { 
                        dr["otherPaid"] = 0.00;
                    }
                }
                else
                {
                    dr["otherPaid"] = 0.00;
                }


                if (!string.IsNullOrEmpty(txtRatePerQtl.Text))
                {
                    string trimmedText = txtRatePerQtl.Text.Trim();
                    double RatePerQtl;

                    if (double.TryParse(trimmedText, out RatePerQtl))
                    {
                        dr["RatePerQtl"] = RatePerQtl;
                    }
                    else
                    {
                        dr["RatePerQtl"] = 0.00;
                    }
                }
                else
                {
                    dr["RatePerQtl"] = 0.00;
                }

                if (!string.IsNullOrEmpty(txtReverseAmount.Text))
                {
                    string trimmedText = txtReverseAmount.Text.Trim();
                    double saudaReverseAmount;

                    if (double.TryParse(trimmedText, out saudaReverseAmount))
                    {
                        dr["saudaReverseAmount"] = saudaReverseAmount;
                    }
                    else
                    {
                        dr["saudaReverseAmount"] = 0.00;
                    }
                }
                else
                {
                    dr["saudaReverseAmount"] = 0.00;
                }


                double Quntal = Convert.ToDouble(txtBuyerQuantal.Text);
                double grdqntal = double.Parse(grdDetail.Rows[0].Cells[5].Text);
                if (grdqntal < Quntal)
                {
                    dr["Quantal"] = txtBuyerQuantal.Text;
                    //setFocusControl(txtBuyerQuantal);
                    //return;
                }
                else
                {
                    dr["Quantal"] = txtBuyerQuantal.Text;
                }

                if (txtBuyerSaleRate.Text != string.Empty)
                {
                    dr["Sale_Rate"] = float.Parse(txtBuyerSaleRate.Text);
                }
                else
                {
                    dr["Sale_Rate"] = 0.00;
                }
                string BillNo = txtPaid.Text;
                BillNo = BillNo.Trim();

                if (BillNo != "")
                {
                    dr["Detail_paid"] = float.Parse(txtPaid.Text);
                }

                else
                {
                    dr["Detail_paid"] = "0";

                }

                if (txtItemAmount.Text != string.Empty)
                {
                    dr["ItemAmount"] = Convert.ToDouble(txtItemAmount.Text);
                }
                else
                {
                    dr["ItemAmount"] = 0.00;
                }
                if (txtDetailpaidDate.Text != string.Empty)
                {
                    dr["Paid_Date"] = txtDetailpaidDate.Text;
                }
                else
                {
                    dr["Paid_Date"] = txtDate.Text;
                }

                if (hdnfProfit.Value != string.Empty)
                {
                    dr["Profit"] = float.Parse(hdnfProfit.Value);
                }
                else
                {
                    dr["Profit"] = 0.00;
                }

                


                //dr["Sauda_Narration"] = txtBuyerNarration.Text;
                dr["Sauda_Narration"] = Server.HtmlDecode(txtBuyerNarration.Text);




                dr["memberid"] = clsCommon.getString("select isnull(accoid,0) as acid from nt_1_accountmaster where Ac_Code=" + dr["Member"].ToString() + " and " +
                    "company_Code=" + Session["Company_Code"].ToString() + "");

                dr["otherPaidid"] = clsCommon.getString("select isnull(accoid,0) as acid from nt_1_accountmaster where Ac_Code=" + dr["otherpaidAc"].ToString() + " and " +
                    "company_Code=" + Session["Company_Code"].ToString() + "");

                dr["saudaReverseId"] = clsCommon.getString("select isnull(accoid,0) as acid from nt_1_accountmaster where Ac_Code=" + dr["saudaReverseAc"].ToString() + " and " +
                "company_Code=" + Session["Company_Code"].ToString() + "");
                dr["IndivisulProfit"] = 0.00;
                dr["MillContribution"] = 0.00;
                dr["Paidorreseive"] = 0.00;


                if (btnADDBuyerDetails.Text == "ADD")
                {
                    dr["trnderdetailid"] = 0;
                    dt.Rows.Add(dr);


                } 
                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='1' and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (id != string.Empty && id != "0")
                {
                    dt.Rows[0]["rowAction"] = "U";
                }
                else
                {
                    dt.Rows[0]["rowAction"] = "A";
                }
            }
            else
            {
                dr["rowAction"] = "A";
                dr["SrNo"] = 1;
                dr["Member"] = "2";
                dr["Name_of_Member"] = "Self";
                lblMemberID.Text = clsCommon.getString("select accoid from nt_1_accountmaster where Ac_Code=2");
                if (txtQuantal.Text != string.Empty)
                {
                    dr["Quantal"] = float.Parse(txtQuantal.Text);
                }
                else
                {
                    dr["Quantal"] = 0;
                }
                if (txtMillRate.Text != string.Empty)
                {
                    dr["Sale_Rate"] = float.Parse(txtMillRate.Text);
                }
                else
                {
                    dr["Sale_Rate"] = 0.00;
                }
                string BillNo = txtPaid.Text;
                BillNo = BillNo.Trim();

                if (BillNo != "")
                {
                    dr["Detail_paid"] = float.Parse(txtPaid.Text);
                }

                else
                {
                    dr["Detail_paid"] = "0";

                }
                dr["ItemAmount"] = 0.00;
                dr["Paid_Date"] = txtDate.Text;
                dr["Profit"] = hdnfProfit.Value; 
                dr["Sauda_Narration"] = string.Empty;
                //dr["trnderdetailid"] = 1;
                dr["memberid"] = Session["SELFID"].ToString();
                dt.Rows.Add(dr);
            }

            #region set sr no
            DataRow drr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drr = (DataRow)dt.Rows[i];
                    drr["SrNo"] = i + 1;
                }
            }
            #endregion

            grdDetail.DataSource = dt;
            grdDetail.DataBind();

            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
            #region set grid view ro colors
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[11].Text == "D" || grdDetail.Rows[i].Cells[11].Text == "R")
                {
                    grdDetail.Rows[i].Style["background-color"] = "#64BB7F";
                    grdDetail.Rows[i].ForeColor = System.Drawing.Color.White;
                    //  grdDetail.Rows[i].BackColor = System.Drawing.Color.Red;
                }
            }

            #endregion

            ViewState["currentTable"] = dt;
            //PopupTenderDetails.Show();

            txtMemberName.Text = string.Empty;
            lblMemberName.Text = string.Empty;
            txtBuyerQuantal.Text = string.Empty;
            lblno.Text = string.Empty;

            txtBuyerSaleRate.Text = string.Empty;
            txtItemAmount.Text = string.Empty;
            txtBuyerNarration.Text = string.Empty;
            lblID.Text = string.Empty;
            txtPaid.Text = string.Empty;
            txtotherpaidAc.Text = string.Empty;
            lblOtherPaidName.Text = string.Empty;
            txtotherPaid.Text = string.Empty;

            txtsaudaReverseAc.Text = string.Empty;
            lblsaudaReverseAcName.Text = string.Empty;


            txtRatePerQtl.Text = string.Empty;
            txtReverseAmount.Text = string.Empty;

            if (btnADDBuyerDetails.Text == "ADD")
            {
                // pnlPopupTenderDetails.Style["display"] = "block";
                txtMemberName.Focus();
            }
            else
            {
                // pnlPopupTenderDetails.Style["display"] = "none";
                // Button1.Focus();
            }


            btnADDBuyerDetails.Text = "ADD";
            //calculate balance self
            this.calculateBalanceSelf();
            this.YourMethod();
            setFocusControl(txtMemberName);
            string dd = "";
            if (txtLiftingDate.Text != string.Empty)
            {

                string liftdate = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                DateTime oDate = DateTime.ParseExact(liftdate, "yyyy/MM/dd", null);
                string d = DateTime.Now.ToString("yyyy/MM/dd");
                DateTime currentDate = DateTime.ParseExact(d, "yyyy/MM/dd", null);



            }
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Check Member!')", true);
                setFocusControl(txtMemberName);

            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            clsButtonNavigation.enableDisable("A");

            ViewState["mode"] = null;
            ViewState["mode"] = "I";
            this.makeEmptyForm("A");
            this.NextNumber();
            setFocusControl(txtGroup);
            #region add self row into grid
            Gridnew.DataSource = null;
            Gridnew.DataBind();
            ViewState["currentTable"] = null;
            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            #endregion

            setFocusControl(txtGroup);
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                //" and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "Tender_No";
                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    txtTenderNo.Text = ds.Tables[0].Rows[0][0].ToString();
                                }
                            }

                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    private void AmtCalculation()
    {
        string Amt = string.Empty;
        double SBBalAmt = 0.00;
        double saleRate = 0.00;
        string docdate = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string SaleTDS = "";
        string PurchaseTDS = "";


        //  hdnfistds.Value = SaleTDS;
        double balancelimit = Convert.ToDouble(Session["BalanceLimit"]);

        DateTime dEnd = DateTime.Parse(docdate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
        DateTime ss = DateTime.Parse("2021/07/01", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));

    }



    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = true; 
            this.YourMethod();
            
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Group", "javascript:pagevalidation();", true);
                 
             
        }
        catch (Exception ex)
        { 
        }
    }


    #endregion

    #region [saveTenderDetails]
    private void saveTenderDetails(string tenderNo, string companyCode)
    {
        try
        {
            string str = "";
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Int32 Buyer = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    double Buyer_Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    double Sale_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[8].Text);
                    double Commission_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    string Narration = "";
                    // DateTime.Now.ToString("dd/MM/yyyy");
                    string dtype = grdDetail.Rows[i].Cells[13].Text.ToString();

                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[12].Text);
                    int ID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    double Buyer_Party = 0;
                    string Paid_Date = grdDetail.Rows[i].Cells[10].Text.ToString();
                    Paid_Date = DateTime.Parse(Paid_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");


                    Buyer_Party = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        if (grdDetail.Rows[i].Cells[14].Text != "N")   //For N do nothing for that row
                        {
                            if (grdDetail.Rows[i].Cells[14].Text == "A")
                            {
                                #region check whether same id is inserted in table already or not (if then insert next no)
                                string id = clsCommon.getString("select AutoID from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + ID + "'");
                                if (id != string.Empty && id != "0")
                                {
                                    //this id is already inserted Get max id
                                    string newId = clsCommon.getString("select max(ID) from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                    ID = Convert.ToInt32(newId) + 1;
                                }
                                #endregion

                                //obj.flag = 1;
                                //obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,Paid_Date,Lifting_Date,Narration,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                //obj.values = "'" + tenderNo + "','" + companyCode + "','" + Buyer + "','" + Buyer_Quantal + "','" + Sale_Rate + "','" + Commission_Rate
                                //    + "','" + Paid_Date + "','" + Lifting_Date + "','" + Narration + "','" + ID + "','" + Buyer_Party + "','True','"
                                //    + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                            }
                            if (grdDetail.Rows[i].Cells[14].Text == "U")
                            {
                                obj.flag = 2;
                                //obj.columnNm = " Buyer='" + Buyer + "',Buyer_Quantal='" + Buyer_Quantal + "',Sale_Rate='" + Sale_Rate + "',Commission_Rate='"
                                //    + Commission_Rate + "',Paid_Date='" + Paid_Date + "',Lifting_Date='" + Lifting_Date + "',Narration='" + Narration + "',Buyer_Party='"
                                //    + Buyer_Party + "',Delivery_Type='" + Delivery_Type + "' where Tender_No='" + tenderNo + "' and Company_Code='"
                                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString())
                                //    + "'  and ID='" + ID + "'";
                                obj.values = "none";
                            }
                            if (grdDetail.Rows[i].Cells[14].Text == "D")
                            {
                                obj.flag = 3;
                                obj.columnNm = "Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and ID='" + ID
                                    + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                obj.values = "none";
                            }
                            obj.tableName = tblDetails;

                            DataSet ds = new DataSet();
                            ds = obj.insertAccountMaster(ref str);
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    #endregion



    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int k = clsIsEdit.Tender_No;
        //if (!clsIsEdit.Tender.Any(a => a == Convert.ToInt32(txtTenderNo.Text)))
        //{
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        //Session["enableSave"] = 0;
        hdnf.Value = lblTender_Id.Text;
        // this.showLastRecord();
        //this.fetchRecord(txtTenderNo.Text);
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtTenderNo.Enabled = false;
        txtEditDoc_No.Text = "";
        //}
        //DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
        //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
        //DateTime DetailLift = DateTime.Parse(Dlift);
        string dd = "";
        dd = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        DateTime Headliftingdate = Convert.ToDateTime(dd);
        string Docdate = Headliftingdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

        string Dlift = System.DateTime.Now.ToString("dd/MM/yyyy");
        string d = DateTime.Parse(Dlift, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        DateTime DetailLift = Convert.ToDateTime(d);



        //int doexist = Convert.ToInt32(clsCommon.getString("select count(*) from   nt_1_deliveryorder where  purc_no=" + txtTenderNo.Text +
        //      " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
        //      " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));

        //int UTRexist = Convert.ToInt32(clsCommon.getString("select count(*) from   NT_1_UTRDetail where  lot_no=" + txtTenderNo.Text +
        //     " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
        //     " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
        //if (doexist >= 1 || UTRexist >= 1)
        //{
        //}
    }

    //private void SaveChanges()
    //{
    //    Session["enableSave"] = 1;
    //}
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string qry2 = string.Empty;
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                qry1 = "update  nt_1_tender set groupTenderNo=0, groupTenderId=0 where groupTenderNo=" + txtTenderNo.Text ;
                ds = clsDAL.SimpleQuery(qry1);
              //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "system", "javascript:DeleteConform();", true);
                qry2 = "update GroupTenderPurchase set   IsDeleted=0 where Tender_No=" + txtTenderNo.Text + " and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry2);


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Sucessfully  Deleted!'); window.location.reload();", true);
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
        hdnf.Value = clsCommon.getString("select max(grouptenderid) from " + tblHead + " where Company_Code=" + Session["Company_Code"].ToString() + " and " +
                " Year_Code=" + Session["year"].ToString() + "");


        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        qry = getDisplayQuery();
        this.fetchRecord(qry);

    }
    #endregion

    #region [fetchRecord]
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
                        //hdnf.Value = txtValue;
                        //txtTenderNo.Text = txtValue;
                        hdnf.Value = dt.Rows[0]["grouptenderid"].ToString();
                        txtDate.Text = dt.Rows[0]["Tender_dateConverted"].ToString();
                        txtLiftingDate.Text = dt.Rows[0]["Lifting_DateConverted"].ToString();
                        lblTender_Id.Text = hdnf.Value;
                        hdnftenderid.Value = hdnf.Value;
                        //txtDate.Text = dt.Rows[0]["Tender_Date"].ToString();
                        //txtLiftingDate.Text = dt.Rows[0]["Lifting_Date"].ToString();
                        //Response.Write("MillCode" + dt.Rows[0]["Mill_Code"].ToS 
                        txtTenderNo.Text = dt.Rows[0]["Tender_No"].ToString();
                        hdnftenderno.Value = txtTenderNo.Text;
                        txtMillCode.Text = dt.Rows[0]["Mill_Code"].ToString();
                        txtitem_code.Text = dt.Rows[0]["itemcode"].ToString();
                        lblitemname.Text = clsCommon.getString("select System_Name_E  from qrymstitem where System_Code=" + txtitem_code.Text + "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                 
                        //lblitemname.Text = dt.Rows[0]["itemname"].ToString(); 

                        txtGrade.Text = dt.Rows[0]["Grade"].ToString();
                        txtQuantal.Text = dt.Rows[0]["Quantal"].ToString();//Convert.ToString(Math.Abs(Convert.ToDouble(dt.Rows[0]["Quantal"].ToString())));
                        txtPacking.Text = dt.Rows[0]["Packing"].ToString();
                        txtBags.Text = dt.Rows[0]["Bags"].ToString();
                        txtMillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                        hdnfmc.Value = dt.Rows[0]["mc"].ToString();
                        lblMillName.Text = dt.Rows[0]["millname"].ToString();
                        hdnfgid.Value = dt.Rows[0]["gid"].ToString();
                        lblTender_Id.Text = dt.Rows[0]["grouptenderid"].ToString();
                        txtGroup.Text = dt.Rows[0]["Group_Account"].ToString();
                        lblGroup_Name.Text = dt.Rows[0]["GroupName"].ToString();
                        txtmillDeff.Text = dt.Rows[0]["MillDeff"].ToString();
                        drCalculateStock.Text = dt.Rows[0]["calculateStock"].ToString();
                        drAccounted.Text = dt.Rows[0]["isAccounted"].ToString();
                        txtourTenderNo.Text = dt.Rows[0]["ourTenderNo"].ToString();
                        lblDate.Text = clsCommon.getString("select Tender_DateConverted  from qrytenderhead  where  Tender_No=" + txtourTenderNo.Text + "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblQuantal.Text = clsCommon.getString("select Quantal  from qrytenderhead  where  Tender_No=" + txtourTenderNo.Text + "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnftenderhelpid.Value = dt.Rows[0]["ourTenderNoid"].ToString();

                        txtmillDeffAc.Text = dt.Rows[0]["MillDeffAc"].ToString();
                        txtNarration.Text = dt.Rows[0]["Narration"].ToString();
                        lblmillDeffAcName.Text = dt.Rows[0]["millDiffAcName"].ToString();
                        hdnfmillDiffacid.Value = dt.Rows[0]["MillDeffacid"].ToString();
                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        string IsDeleted = dt.Rows[0]["IsDeleted"].ToString();
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                        if (lblModified != null)
                        {
                            lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        }

                        recordExist = true;
                        lblMsg.Text = "";

                        #region Tender Details

                        // qry = "select  ID,Buyer as Member,buyerbrokerfullname  as [Name of Member],Buyer_Party as Broker,salepartyfullname as [Name of Broker],Buyer_Quantal as [Quantal],Sale_Rate as [Sale Rate],Commission_Rate as [ItemAmount],Convert(varchar(10),DetailSaudaDate,103) as Paid_Date,Convert(varchar(10),DetailLiftingDate,103) as Lifting_Date,saudanarration as [Sauda Narration],Delivery_Type from " + qryDetail + " where grouptenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and IsActive='True' and TDYearCode='" + Convert.ToInt32(Session["year"].ToString()) + "' order by ID";
                        qry = "select  ID,membercode as Member,name as Name_of_Member ," +
                              " Buyer_Quantal as Quantal,Sale_Rate as Sale_Rate,paid as Detail_paid," +
                              " ItemAmount as ItemAmount,  Narration as Sauda_Narration," +
                              " trnderdetailid,'' as rowAction,'' as SrNo,mid as memberid,  paid_dateConverted AS  Paid_Date, " +
                              " profit as profit, otherpaidAc as otherpaidAc, otherpaidAcName as otherpaidAcName, otherPaid as otherPaid," +
                              " otherPaidid as otherPaidid, indivisulProfit as IndivisulProfit, millContribution as MillContribution," +
                              " paidorreseive as Paidorreseive,saudaReverseAc as saudaReverseAc,saudaReverseId as saudaReverseId ," +
                              " saudaReversename as saudaReverseName , RatePerQtl as RatePerQtl,ReverseAmount as saudaReverseAmount" +
                              " from qrygrouptenderdetail where grouptenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' order by ID";


                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {

                                    // dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                    // dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                                    dt.Rows[0]["SrNo"] = 1;                           //self row
                                    dt.Rows[0]["rowAction"] = "U";                     //self row

                                    for (int i = 1; i < dt.Rows.Count; i++)
                                    {
                                        dt.Rows[i]["rowAction"] = "N";
                                        dt.Rows[i]["SrNo"] = i + 1;
                                    }

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


                        #region Group Details

                        // qry = "select  ID,Buyer as Member,buyerbrokerfullname  as [Name of Member],Buyer_Party as Broker,salepartyfullname as [Name of Broker],Buyer_Quantal as [Quantal],Sale_Rate as [Sale Rate],Commission_Rate as [ItemAmount],Convert(varchar(10),DetailSaudaDate,103) as Paid_Date,Convert(varchar(10),DetailLiftingDate,103) as Lifting_Date,saudanarration as [Sauda Narration],Delivery_Type from " + qryDetail + " where grouptenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and IsActive='True' and TDYearCode='" + Convert.ToInt32(Session["year"].ToString()) + "' order by ID";
                        qry = "select  membercode as Member,name as Name_of_Member , " +
                              " sum(Buyer_Quantal) as Quantal " + 
                              " from qrygrouptenderdetail where grouptenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  group by name,membercode";


                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {

                                    Gridnew.DataSource = dt;
                                    Gridnew.DataBind();
                                    ViewState["currentTable2"] = dt;
                                }
                                else
                                {
                                    Gridnew.DataSource = null;
                                    Gridnew.DataBind();
                                    ViewState["currentTable2"] = null;
                                }
                            }
                            else
                            {
                                Gridnew.DataSource = null;
                                Gridnew.DataBind();
                                ViewState["currentTable2"] = null;
                            }
                        }
                        else
                        {
                            Gridnew.DataSource = null;
                            Gridnew.DataBind();
                            ViewState["currentTable2"] = null;
                        }
                        #endregion
                        this.TCSCalculation();
                        this.TdsCalculation();
                        GridViewRow gr = (GridViewRow)Gridnew.Rows[0];
                        gr.Enabled = false;
                        pnlgrdDetail.Enabled = false;

                        if (IsDeleted == "0")
                        {
                            lblMsg.Text = "Delete";
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                            pnlgrdDetail.Enabled = false; 
                        }
                    }
                }
            }
            this.makeEmptyForm("S");
            //hdnf.Value = txtTenderNo.Text;
            return recordExist;
        }
        catch
        {
            // throw;
            return false;
        }
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        #region [code]
        try
        {
            string query = "";
            query = "select grouptenderid from " + tblHead + " where grouptenderid=(select MIN(grouptenderid) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

            hdnf.Value = clsCommon.getString(query);
            navigateRecord();

        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [grouptenderid] from " + tblHead + " where grouptenderid<" + Convert.ToInt32(hdnf.Value) + " and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())
                    + " ORDER BY grouptenderid DESC  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();

            }

        }
        catch
        {

        }
    }

    #endregion

    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [grouptenderid] from " + tblHead + " where grouptenderid>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY grouptenderid asc  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();

            }

        }
        catch
        {

        }
    }
    #endregion

    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select grouptenderid from " + tblHead + " where grouptenderid=(select MAX(grouptenderid) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();



        }
        catch
        {

        }
    }
    #endregion

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons

        //if (ViewState["mode"].ToString() == "U")
        //{

        int RecordCount = 0;

        string query = "";
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
        //" and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(query);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
        }

        if (RecordCount != 0 && RecordCount == 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
        else if (RecordCount != 0 && RecordCount > 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = true;
            //  btnLast.Focus();
        }

        if (txtTenderNo.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();

                query = "SELECT top 1 [grouptenderid] from " + tblHead +
                    " where grouptenderid>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY grouptenderid asc  ";
                //+ " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY grouptenderid asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnLast.Enabled = true;
                            btnNext.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnLast.Enabled = false;
                            btnNext.Enabled = false;
                        }
                    }
                }

                ds = new DataSet();
                dt = new DataTable();

                query = "SELECT top 1 [grouptenderid] from " + tblHead + " where grouptenderid<" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY grouptenderid asc  ";
                //    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY grouptenderid asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnFirst.Enabled = true;
                            btnPrevious.Enabled = true;
                        }
                        else
                        {
                            btnFirst.Enabled = false;
                            btnPrevious.Enabled = false;
                        }
                    }
                }
                #endregion
            }
        }

        // }

        #endregion
    }
    #endregion

    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                // txtDoc_No.Text = hdnf.Value;
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }
                this.enableDisableNavigateButtons();
                this.makeEmptyForm("S");
            }
            else
            {
                showLastRecord();
            }
        }
        catch
        {

        }
    }

    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            if (e.CommandArgument == "lnk")
            {
                switch (e.CommandName)
                {
                    case "EditRecord":
                        if (grdDetail.Rows[rowindex].Cells[rowAction].Text != "D" && grdDetail.Rows[rowindex].Cells[rowAction].Text != "R")
                        {
                            // pnlPopupTenderDetails.Style["display"] = "block";
                            this.showRecord(grdDetail.Rows[rowindex]);
                            btnADDBuyerDetails.Text = "Update";
                        }
                        break;

                    case "DeleteRecord":
                        string action = "";

                        DataTable dt1 = (DataTable)ViewState["currentTable"];
                        int refid = Convert.ToInt32(dt1.Rows[rowindex]["trnderdetailid"].ToString());
                        string concat = string.Empty;

                        LinkButton lnkDelete = (LinkButton)e.CommandSource;
                        if (lnkDelete.Text == "Delete")
                        {
                            action = "Delete";
                            lnkDelete.Text = "Open";
                        }
                        else
                        {
                            action = "Open";
                            lnkDelete.Text = "Delete";
                        }
                        this.deleteTenderDetailsRecord(grdDetail.Rows[rowindex], action);


                        break;
                }
            }
        }
        catch
        {

        }

    }
    #endregion

    #region [deleteTenderDetailsRecord]
    private void deleteTenderDetailsRecord(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;

            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["trnderdetailid"].ToString());

                string IDExisting = clsCommon.getString("select ID from " + tblDetails + " where trnderdetailid=" + ID + " ");
                if (IDExisting != string.Empty && IDExisting != "0")
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "D";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "N";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                    //  dt.Rows[rowIndex]["rowAction"] = "D";  //isactive false
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;

                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "R";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "A";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }

                    // dt.Rows[rowIndex]["rowAction"] = "N";   //Do nothing
                }
                ViewState["currentTable"] = dt;
                this.calculateBalanceSelf();
                //ViewState["currentTable"] = dt;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["RowNumber"] = i + 1;
            //}
        }
        catch
        {

        }
    }
    #endregion

    #region [showRecord]
    private void showRecord(GridViewRow gridViewRow)
    {
        if (btnADDBuyerDetails.Text == "Update")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Add The Details')", true);

        }
        lblno.Text = Server.HtmlDecode(gridViewRow.Cells[SrNo].Text);
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
        lbltenderdetailid.Text = Server.HtmlDecode(gridViewRow.Cells[trnderdetailid].Text);
        txtMemberName.Text = Server.HtmlDecode(gridViewRow.Cells[Member].Text);
        lblMemberName.Text = Server.HtmlDecode(gridViewRow.Cells[Name_of_Member].Text);


        txtPaid.Text = Server.HtmlDecode(gridViewRow.Cells[Detail_paid].Text);

        txtDetailpaidDate.Text = Server.HtmlDecode(gridViewRow.Cells[Paid_Date].Text);
        double buyerqntl = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[quantal].Text));
        txtBuyerQuantal.Text = Convert.ToString(Math.Abs(buyerqntl));
        double salerate = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[saleRate].Text));
        txtBuyerSaleRate.Text = Convert.ToString(Math.Abs(salerate));
        hdnfProfit.Value = Server.HtmlDecode(gridViewRow.Cells[Profit].Text); 
        // txtBuyerNarration.Text = Server.HtmlDecode(gridViewRow.Cells[Sauda_Narration].Text);
        string BuyerNarration = gridViewRow.Cells[Sauda_Narration].Text;
        if (BuyerNarration == "&nbsp;")
        {
            txtBuyerNarration.Text = "";
        }
        else
        {
            txtBuyerNarration.Text = Server.HtmlDecode(gridViewRow.Cells[Sauda_Narration].Text);
        } 
        txtotherpaidAc.Text = Server.HtmlDecode(gridViewRow.Cells[otherpaidAc].Text);
        lblOtherPaidName.Text = Server.HtmlDecode(gridViewRow.Cells[otherpaidAcName].Text);
        txtotherPaid.Text = Server.HtmlDecode(gridViewRow.Cells[otherPaid].Text);
        //double BuyerotherPaid = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[otherPaid].Text));
        //txtotherPaid.Text = Convert.ToString(Math.Abs(BuyerotherPaid));


        txtsaudaReverseAc.Text = Server.HtmlDecode(gridViewRow.Cells[saudaReverseAc].Text);
        lblsaudaReverseAcName.Text = Server.HtmlDecode(gridViewRow.Cells[saudaReverseName].Text);


        txtRatePerQtl.Text = Server.HtmlDecode(gridViewRow.Cells[RatePerQtl].Text);
        txtReverseAmount.Text = Server.HtmlDecode(gridViewRow.Cells[saudaReverseAmount].Text); 
      

        TCSAmountCalculate();

        if (ViewState["currentTable"] != null)
        {

        }
        setFocusControl(txtMemberName);
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

    #region[btnMillCode_Click]
    protected void btnMillCode_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchText.Text = string.Empty;
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMillCode";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region [txtMillCode_TextChanged]
    protected void txtMillCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            string millName = string.Empty;
            searchString = txtMillCode.Text;
            if (txtMillCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtMillCode.Text);
                if (a == false)
                {
                    btnMillCode_Click(this, new EventArgs());
                }
                else
                {
                    DataSet ds = clsDAL.SimpleQuery("select Ac_Name_E,accoid,Short_Name from qrymstaccountmaster where Ac_Code='" + txtMillCode.Text
                        + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            millName = ds.Tables[0].Rows[0]["Ac_Name_E"].ToString();
                            if (millName != string.Empty && millName != "0")
                            {
                                hdnfmc.Value = ds.Tables[0].Rows[0]["accoid"].ToString();
                                hdnfmillshort.Value = ds.Tables[0].Rows[0]["Short_Name"].ToString();

                                lblMillName.Text = millName;

                                if (btnSave.Text == "Save")
                                {
                                    setFocusControl(txtitem_code);

                                }
                                // AmtCalculation();
                            }
                            else
                            {
                                txtMillCode.Text = string.Empty;
                                lblMillName.Text = string.Empty;
                                setFocusControl(txtMillCode);
                            }
                        }
                        else
                        {
                            txtMillCode.Text = string.Empty;
                            lblMillName.Text = string.Empty;
                            setFocusControl(txtMillCode);
                        }
                    }
                    else
                    {
                        txtMillCode.Text = string.Empty;
                        lblMillName.Text = string.Empty;
                        setFocusControl(txtMillCode);
                    }
                }
            }
            else
            {
                txtMillCode.Text = string.Empty;
                lblMillName.Text = millName;
                setFocusControl(txtMillCode);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnGrade_Click]
    protected void btnGrade_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "GR";
            btnSearch_Click(sender, e);
            setFocusControl(txtSearchText);

        }
        catch
        {

        }
    }
    #endregion

    #region [txtGrade_TextChanged]
    protected void txtGrade_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGrade.Text;
        if (txtGrade.Text != string.Empty)
        {
            bool a = true;
            if (txtGrade.Text.Length < 8)
            {
                a = clsCommon.isStringIsNumeric(txtGrade.Text);
            }
            if (a == false)
            {
                // btnGrade_Click(this, new EventArgs());
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                setFocusControl(txtQuantal);
            }
        }
    }
    #endregion





    #region[btnourTenderNo_Click]
    protected void btnourTenderNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtourTenderNo";
            pnlPopup.ScrollBars = ScrollBars.Both;
            txtSearchText.Text = txtMillRate.Text;
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtourTenderNo_TextChanged]
    protected void txtourTenderNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close"; 
            searchString = txtourTenderNo.Text;
            if (txtourTenderNo.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtourTenderNo.Text);
                if (a == false)
                {
                    btnourTenderNo_Click(this, new EventArgs());
                }
                else
                {
                    DataSet ds = clsDAL.SimpleQuery("select Tender_No,Tender_DateConverted,Grade,Quantal,Mill_Rate,tenderid from qrytenderhead  where  Tender_No='" + txtourTenderNo.Text
                        + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        { 
                                hdnftenderhelpid.Value = ds.Tables[0].Rows[0]["tenderid"].ToString();
                                lblDate.Text = ds.Tables[0].Rows[0]["Tender_DateConverted"].ToString();
                                lblQuantal.Text = ds.Tables[0].Rows[0]["Quantal"].ToString();
                                setFocusControl(txtNarration);
                        }
                        else
                        {
                            txtourTenderNo.Text = string.Empty;
                            lblDate.Text = string.Empty;
                            lblQuantal.Text = string.Empty;
                            setFocusControl(txtMemberName);
                        }
                    }
                    else
                    {
                        txtourTenderNo.Text = string.Empty;
                        lblDate.Text = string.Empty;
                        lblQuantal.Text = string.Empty;
                        setFocusControl(txtourTenderNo);
                    }
                }
            }
            else
            {
                txtourTenderNo.Text = string.Empty;
                lblDate.Text = string.Empty;
                lblQuantal.Text = string.Empty;
                setFocusControl(txtourTenderNo);
            }
        }
        catch
        {
        }  
    }
    #endregion











    protected void btnsubBrker_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "SubBrker";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    protected void txtBrokrage_TextChanged(object sender, EventArgs e)
    {


    }



    #region[btnMember_Click]
    protected void btnMember_Click(object sender, EventArgs e)
    {
        //  pnlPopupTenderDetails.Style["display"] = "block";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "BU";
        btnSearch_Click(sender, e);
    }
    #endregion

    #region[txtMemberName_TextChanged]
    protected void txtMemberName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string Membername = "";
            if (txtMemberName.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtMemberName.Text);
                if (a == false)
                {
                    btnMember_Click(this, new EventArgs());
                }
                else
                {
                    Membername = clsCommon.getString("select name from qryGroupMemberUnion where member=" + txtMemberName.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    hdnfmember.Value = clsCommon.getString("select isnull(acid,0) as acid from qryGroupMemberUnion where member=" + txtMemberName.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                    if (Membername != string.Empty)
                    {

                        lblMemberName.Text = Membername;
                        setFocusControl(txtBuyerQuantal);
                    }
                    else
                    {
                        txtMemberName.Text = "0";
                        lblMemberName.Text = Membername;
                        // setFocusControl(txtGSTRateCode);
                    }
                }
            }
            else
            {
                setFocusControl(txtMemberName);
            }

        }
        catch
        {
        }

    }

    #endregion



    #region[btnotherpaidAc_Click]
    protected void btnotherpaidAc_Click(object sender, EventArgs e)
    {
        try
        {
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "txtotherpaidAc";
        btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region[txtotherpaidAc_TextChanged]
    protected void txtotherpaidAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtotherpaidAc.Text;
        strTextBox = "txtotherpaidAc";
        csCalculations();
    }

    #endregion

     
    #region[txtsaudaReverseAc_TextChanged]
    protected void txtsaudaReverseAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtsaudaReverseAc.Text;
        strTextBox = "txtsaudaReverseAc";
        csCalculations();
    }

    #endregion


    #region[btnsaudaReverseAc_Click]
    protected void btnsaudaReverseAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtsaudaReverseAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region[txtRatePerQtl_TextChanged]
    protected void txtRatePerQtl_TextChanged(object sender, EventArgs e)
    {
        TCSAmountCalculate();
        setFocusControl(txtReverseAmount);
    }
    #endregion

    #region[txtReverseAmount_TextChanged]
    protected void txtReverseAmount_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        { 
             
            if (strTextBox == "txtotherpaidAc")
            {
                string otherpaidname = string.Empty;
                if (txtotherpaidAc.Text != string.Empty)
                {
                    searchString = txtotherpaidAc.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnotherpaidAc_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfotherpaid.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtotherpaidAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtotherpaidAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        otherpaidname = clsCommon.getString(qry);

                        if (otherpaidname != string.Empty)
                        {
                            lblOtherPaidName.Text = otherpaidname;
                            setFocusControl(txtotherPaid);
                        }
                        else
                        {
                            lblOtherPaidName.Text = string.Empty;
                            txtotherpaidAc.Text = string.Empty;
                            setFocusControl(txtotherpaidAc);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtotherpaidAc);
                }
            }


            if (strTextBox == "txtotherpaidAc")
            {
                setFocusControl(txtotherPaid);
            }

            if (strTextBox == "txtsaudaReverseAc")
            {
                string otherpaidname = string.Empty;
                if (txtsaudaReverseAc.Text != string.Empty)
                {
                    searchString = txtsaudaReverseAc.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnsaudaReverseAc_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfReverseAcId.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtsaudaReverseAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtsaudaReverseAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        otherpaidname = clsCommon.getString(qry);

                        if (otherpaidname != string.Empty)
                        {
                            lblsaudaReverseAcName.Text = otherpaidname;
                            setFocusControl(txtRatePerQtl);
                        }
                        else
                        {
                            lblsaudaReverseAcName.Text = string.Empty;
                            txtsaudaReverseAc.Text = string.Empty;
                            setFocusControl(txtsaudaReverseAc);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtsaudaReverseAc);
                }
            }


            if (strTextBox == "txtmillDeffAc")
            {
                string millDeffAcname = string.Empty;
                if (txtmillDeffAc.Text != string.Empty)
                {
                    searchString = txtmillDeffAc.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmillDeffAc_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmillDiffacid.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmillDeffAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtmillDeffAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        millDeffAcname = clsCommon.getString(qry);

                        if (millDeffAcname != string.Empty)
                        {
                            lblmillDeffAcName.Text = millDeffAcname;
                            setFocusControl(txtourTenderNo);
                        }
                        else
                        {
                            lblmillDeffAcName.Text = string.Empty;
                            txtmillDeffAc.Text = string.Empty;
                            setFocusControl(txtmillDeffAc);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmillDeffAc);
                }
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

                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [txtQuantal_TextChanged]
    protected void txtQuantal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "none";
            string pck = "0";
            pck = clsCommon.getString("select KgPerKatta from qrymstitem where System_Type='I' and System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
            txtPacking.Text = pck;
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtPacking);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }

            TCSAmountCalculate();

            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            else
            {
                #region decide whether actual row is updating or virtual [rowAction]
                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text +
                    "' and ID='1' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                //" and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                DataRow dr = null;
                DataTable dt = (DataTable)ViewState["currentTable"];

                dr = dt.Rows[0];
                if (id != string.Empty && id != "0")
                {
                    dr["rowAction"] = "U";   //actual row
                }
                else
                {
                    dr["rowAction"] = "A";    //virtual row
                }


                #endregion
                ViewState["currentTable"] = dt;
                grdDetail.DataSource = dt;
                grdDetail.DataBind();
                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
            }

            this.calculateBalanceSelf();
            this.TCSCalculation();
            this.TdsCalculation();
            this.YourMethod();
           
            setFocusControl(txtPacking);
        }
        catch
        {

        }
        SetFocus(txtPacking);
    }
    #endregion

    #region[txtPacking_TextChanged]
    protected void txtPacking_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [txtDate_TextChanged]
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDate.Text != string.Empty)
            {
                try
                {
                    string scdate = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");

                    DateTime d = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                    int millpaymentdate = Convert.ToInt32(hdnfmillpaymentdate.Value);
                    d = d.AddDays(+(Convert.ToInt32(millpaymentdate)));
                    txtLiftingDate.Text = d.ToString("dd/MM/yyyy");
                    setFocusControl(txtLiftingDate);
                }
                catch
                {
                    txtDate.Text = string.Empty;
                }
            }
            else
            {
                setFocusControl(txtDate);
            }
            //if (txtDate.Text != string.Empty)
            //{
            //    DateTime d = new DateTime();
            //    // d = DateTime.Now;
            //    string date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            //    //  d = DateTime.Parse(date);
            //    // d = d.AddDays(15);
            //    int millpaymentdate = Convert.ToInt32(hdnfmillpaymentdate.Value);
            //    // txtLiftingDate.Text = clsCommon.getString("select adddate('" + date + "', interval 10 DAY) as d");
            //    txtLiftingDate.Text = clsCommon.getString("select DATEADD(day,"+millpaymentdate+",'" + date + "')");
            //    txtLiftingDate.Text = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            //    setFocusControl(txtLiftingDate);
            //}
            //else
            //{
            //    setFocusControl(txtDate);
            //}
        }
        catch
        {
            txtDate.Text = string.Empty;
            setFocusControl(txtDate);
            calenderExtenderDate.Animated = true;
        }
    }
    #endregion

    #region [txtLiftingDate_TextChanged]
    protected void txtLiftingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtLiftingDate.Text != string.Empty)
            {
                // string d = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                //DateTime oDate = DateTime.ParseExact(txtLiftingDate.Text, "yyyy/MM/dd", null);
                string liftdate = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                DateTime oDate = DateTime.ParseExact(liftdate, "yyyy/MM/dd", null);
                string d = DateTime.Now.ToString("yyyy/MM/dd");
                DateTime currentDate = DateTime.ParseExact(d, "yyyy/MM/dd", null);


                setFocusControl(txtMillCode);
            }
        }
        catch
        {
            txtLiftingDate.Text = string.Empty;
            setFocusControl(txtLiftingDate);
        }
    }
    #endregion

     

    #region[txtMillRate_TextChanged]
    protected void txtMillRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMillRate.Text != string.Empty)
            {
                this.TCSCalculation();
                this.TdsCalculation();
            }

            pnlPopup.Style["display"] = "none";
            setFocusControl(txtmillDeff);
        }
        catch
        {

        }
    }
    #endregion


    #region[txtmillDeff_TextChanged]
    protected void txtmillDeff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtmillDeff.Text != string.Empty)
            {
                this.TCSCalculation();
                this.TdsCalculation();
            }

            pnlPopup.Style["display"] = "none";
            setFocusControl(txtMemberName);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtmillDeffAc_TextChanged]
    protected void txtmillDeffAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmillDeffAc.Text;
        strTextBox = "txtmillDeffAc";
        csCalculations();
    }
     #endregion

    #region [btnmillDeffAc_Click]
    protected void btnmillDeffAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmillDeffAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    #endregion


    #region[calculateBalanceSelf]
    /// <summary>
    /// /////////////
    /// </summary>
    protected void calculateBalanceSelf()
    {
        try
        {
            float quantal = 0;
            float balanceSelf = 0;
            float buyerQuantal = 0;
            float quantalTotal = 0;
            float Profit = 0;
            float NetProfit = 0;

            //calculate total of quantals in grid

            for (int i = 1; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[rowAction].Text != "D" && grdDetail.Rows[i].Cells[rowAction].Text != "R")
                {
                    //                   if (grdDetail.Rows[i].RowIndex != 0)
                    //                 {
                    quantalTotal = quantalTotal + float.Parse(grdDetail.Rows[i].Cells[5].Text);
                    Profit = Profit + float.Parse(grdDetail.Rows[i].Cells[15].Text);
                    //               }
                }
            }
            //  quantalTotal = quantalTotal + buyerQuantal;

            if (txtQuantal.Text != string.Empty)
            {
                quantal = float.Parse(txtQuantal.Text);
            }

            if (txtBuyerQuantal.Text != string.Empty)
            {
                buyerQuantal = float.Parse(txtBuyerQuantal.Text);
            }
            balanceSelf = quantal - quantalTotal;


            //set to first row balance self
            grdDetail.Rows[0].Cells[5].Text = balanceSelf.ToString();
            grdDetail.Rows[0].Cells[15].Text = Profit.ToString();
            //  grdDetail.Rows[0].Cells[12].Text = "U";
            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
        }
        catch
        {

        }
    }
    #endregion




    #region [txtTenderNo_TextChanged]
    protected void txtTenderNo_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtTenderNo.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtTenderNo.Text != string.Empty)
                {
                    txtValue = txtTenderNo.Text;

                    string qry = "select * from " + tblHead + " where Tender_No='" + txtValue +
                        "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    //" and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Tender No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        this.getMaxCode();
                                        txtTenderNo.Enabled = false;
                                        //Session["enableSave"] = 1;
                                        btnSave.Enabled = true;   //IMP 
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        bool recordExist = this.fetchRecord(txtValue);
                                        if (recordExist == true)
                                        {
                                            //txtTenderNo.Enabled = true;
                                            pnlgrdDetail.Enabled = true;
                                            setFocusControl(txtGroup);
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(txtGroup);
                                    txtTenderNo.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                    calculateBalanceSelf();
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("A");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtTenderNo.Text = string.Empty;
                                    setFocusControl(txtTenderNo);
                                    calculateBalanceSelf();
                                    //txtTenderNo.Enabled = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    txtTenderNo.Focus();
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Tender No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtTenderNo.Text = string.Empty;
                txtTenderNo.Focus();
            }
        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            pnlPopup.Style["display"] = "block";
            string searchtxt = searchString;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchString;
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty)
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



            if (hdnfClosePopup.Value == "txtMillCode")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " ( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' or Short_Name like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);

                string qry = " Locked=0 and Ac_type='M' and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (" + name + ") order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                if (txtGrade.Text != string.Empty)
                {
                    split = txtGrade.Text.Split(delimiter);
                }
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += "( System_Name_E like '%" + aa + "%' or System_Type like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Group--";
                string qry = "select System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "SubBrker")
            {


                lblPopupHead.Text = "--Select Buyer--";

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%'  or city_name_e like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "BU")
            {
                lblPopupHead.Text = "--Select Member--";
                if (txtMemberName.Text != string.Empty)
                {
                    split = txtMemberName.Text.Split(delimiter);
                }
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += "( member like '%" + aa + "%' or name like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                string qry = "select member,name,acid from qryGroupMemberUnion where member!=0 and Doc_No=" + txtGroup.Text + " and " +
                    " " + name + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }




            if (hdnfClosePopup.Value == "TN" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Tender --";

                qry = "SELECT  Tender_No, Tender_Date,  millname, Quantal,Grade,buyerbrokershortname, Buyer_Quantal,Mill_Rate, Sale_Rate,doname" +
                " FROM  " + qryCommon + " where Buyer=2 and (Tender_No like '%" + txtSearchText.Text + "%' or Tender_Date like '%" + txtSearchText.Text + "%' or millname like '%" + txtSearchText.Text + "%' or millfullname like '%" + txtSearchText.Text + "%') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Tender_No desc";
                this.showPopup(qry);

            }
            if (hdnfClosePopup.Value == "txtitem_code")
            {
                lblPopupHead.Text = "--Select item--";

                qry = "select  System_Code,System_Name_E,Vat_AC as Gst,gstratre from qrymstitem where   (System_Code like '%" + txtSearchText.Text + "%' or System_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "Group")
            {
                lblPopupHead.Text = "--Select Group Name --";
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += "( Doc_No like '%" + aa + "%' or GroupName like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                string qry = "select Doc_No,GroupName,autoid from GroupCreactionMaster where  " +
                    " " + name + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "BPDetail")
            {
                lblPopupHead.Text = "--Select BP Account--";
                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
               + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%'  or city_name_e like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtotherpaidAc")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " ( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' or Short_Name like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (" + name + ") order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }


            if (hdnfClosePopup.Value == "txtsaudaReverseAc")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " ( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' or Short_Name like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (" + name + ") order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmillDeffAc")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " ( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' or Short_Name like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (" + name + ") order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }
           
            if (hdnfClosePopup.Value == "txtourTenderNo")
            {
                if ( txtSearchText.Text == txtMillRate.Text){
                txtSearchText.Text = txtMillRate.Text;
                }
                else{
                
                }
                tdDate.Visible = false;
                if (txtMillCode.Text != string.Empty)
                {
                    lblPopupHead.Text = "--Select No--";
                    //string qry = "select Tender_No,Convert(varchar(10),Tender_Date,103) as Tender_Date,salepartyfullname as Party,buyerbrokerfullname as Party2,Mill_Rate,Grade,Sale_Rate,Buyer_Quantal,despatchqty,balance,doname,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,ID,Delivery_Type as DT from "
                    // + qrypurc_No + " where  Mill_Code=" + txtMillCode.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    // " and (Tender_No like '%" + txtSearchText.Text + "%' or  Tender_Date like '%" + txtSearchText.Text + "%' or salepartyfullname like  '%" + txtSearchText.Text + "%' or buyerbrokerfullname like '%" + txtSearchText.Text + "%' or doname like '%" + txtSearchText.Text + "%') and balance!=0 and Mill_Code=" + txtMillCode.Text + "  order by Tender_No desc";
                    string qry = "";
                    if (btnSave.Text == "Save")
                    {
                        qry = "select Tender_No,Tender_DateConverted,Grade,Quantal,Mill_Rate,millshortname,groupTenderNo from qrytenderhead   where   Mill_Code=" + txtMillCode.Text +
                        " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +

                        " and (Tender_No like '%" + txtSearchText.Text + "%' or  Grade like  '%" +
                        txtSearchText.Text + "%' or Quantal like '%" + txtSearchText.Text + "%' or Mill_Rate like '%" + txtSearchText.Text + "%') " +
                        "  order by Tender_No desc";
                    }
                    else
                    {

                        qry = "select Tender_No,Tender_DateConverted,Grade,Quantal,Mill_Rate,millshortname,groupTenderNo from qrytenderhead  where   Mill_Code=" + txtMillCode.Text +
                      " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +

                        " and (Tender_No like '%" + txtSearchText.Text + "%' or  Grade like  '%" +
                        txtSearchText.Text + "%' or Quantal like '%" + txtSearchText.Text + "%' or Mill_Rate like '%" + txtSearchText.Text + "%') " +
                        "    order by Tender_No desc";

                    }
                    this.showPopup(qry);
                }
                else
                {
                    setFocusControl(txtMillCode);
                    pnlPopup.Style["display"] = "none";
                }
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
            if (hdnfClosePopup.Value == "MM")
            {
                setFocusControl(txtMillCode);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                setFocusControl(txtGrade);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtMemberName);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtMemberName);
            }
            if (hdnfClosePopup.Value == "Group")
            {
                setFocusControl(txtGroup);
            }
            if (hdnfClosePopup.Value == "BPDetail")
            {
            }
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

    #region [btnChangeNo_Click]
    protected void changeNo_click(object sender, EventArgs e)
    {
        try
        {

            //if (hdnfClosePopup.Value =="TN")
            //{

            if (btnChangeNo.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtTenderNo.Text = string.Empty;
                txtTenderNo.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtTenderNo);

            }
            if (btnChangeNo.Text == "Choose No")
            {
                try
                {
                    pnlgrdDetail.Enabled = true;
                    //setFocusControl(txtSearchText);
                    hdnfClosePopup.Value = "TN";
                    pnlPopup.Style["display"] = "block";
                    btnSearch_Click(sender, e);

                }
                catch
                {

                }
                //}

            }
        }
        catch
        {

        }
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[Detail_Id].ControlStyle.Width = new Unit("30px");
            e.Row.Cells[Member].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[Name_of_Member].ControlStyle.Width = new Unit("150px");
            e.Row.Cells[quantal].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[saleRate].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[Detail_paid].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[ItemAmount].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[Sauda_Narration].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[trnderdetailid].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[rowAction].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[SrNo].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[memberid].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[Paid_Date].ControlStyle.Width = new Unit("80px");
            e.Row.Cells[Profit].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[otherpaidAc].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[otherpaidAcName].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[otherPaid].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[otherPaidid].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[IndivisulProfit].ControlStyle.Width = new Unit("100px"); 
            e.Row.Cells[MillContribution].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[Paidorreseive].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[saudaReverseAc].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[saudaReverseId].ControlStyle.Width = new Unit("0px");
            e.Row.Cells[saudaReverseName].ControlStyle.Width = new Unit("150px");
            e.Row.Cells[RatePerQtl].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[saudaReverseAmount].ControlStyle.Width = new Unit("70px");



            e.Row.Cells[trnderdetailid].Visible = true;
            e.Row.Cells[rowAction].Visible = true;
            e.Row.Cells[SrNo].Visible = true;
            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";
            e.Row.Cells[9].Style["overflow"] = "hidden";
            e.Row.Cells[10].Style["overflow"] = "hidden";
            e.Row.Cells[11].Style["overflow"] = "hidden";
            e.Row.Cells[12].Style["overflow"] = "hidden";
            e.Row.Cells[13].Style["overflow"] = "hidden";
            e.Row.Cells[16].Style["overflow"] = "hidden";
            e.Row.Cells[17].Style["overflow"] = "hidden";
            e.Row.Cells[18].Style["overflow"] = "hidden";
            e.Row.Cells[19].Style["overflow"] = "hidden";
            e.Row.Cells[23].Style["overflow"] = "hidden";
            e.Row.Cells[24].Style["overflow"] = "hidden";
            e.Row.Cells[25].Style["overflow"] = "hidden";
            e.Row.Cells[26].Style["overflow"] = "hidden";
            e.Row.Cells[27].Style["overflow"] = "hidden";
            e.Row.Cells[4].ToolTip = e.Row.Cells[4].Text;
            e.Row.Cells[8].ToolTip = e.Row.Cells[8].Text;

            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center; 
                //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;

                //i++;
                //foreach (TableCell cell in e.Row.Cells)
                //{
                //    string s = cell.Text.ToString();
                //    if (cell.Text.Length > 33)
                //    {
                //        cell.Text = cell.Text.Substring(0, 33) + "...";
                //        cell.ToolTip = s;
                //    }
                //}
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [pnlgrdNew_RowDataBound]
    protected void pnlgrdNew_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            e.Row.Cells[0].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("400px");
            e.Row.Cells[3].ControlStyle.Width = new Unit("10px");
        }
        catch
        {

        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (v == "TN")
                {

                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[3].Width = new Unit("80px");
                    e.Row.Cells[4].Width = new Unit("100px");

                }
                if (v == "txtitem_code")
                {
                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("60px");
                    e.Row.Cells[3].Width = new Unit("100px");
                }
                if (v == "txtMillCode")
                {

                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[1].Width = new Unit("300px");
                    e.Row.Cells[2].Width = new Unit("150px");
                    e.Row.Cells[3].Width = new Unit("30px");
                }

                if (v == "txtotherpaidAc")
                {

                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[1].Width = new Unit("300px");
                    e.Row.Cells[2].Width = new Unit("150px");
                    e.Row.Cells[3].Width = new Unit("30px");
                }


                if (v == "GR")
                {
                    e.Row.Cells[0].Width = new Unit("400px");

                }

                if (v == "BU" || v == "BP")
                {
                    //if (e.Row.RowType != DataControlRowType.Pager)
                    //{
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                    // }
                }

            }
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
                strTextbox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBuyerQuantal_TextChanged]
    protected void txtBuyerQuantal_TextChanged(object sender, EventArgs e)
    {

        TCSAmountCalculate();
        if (btnADDBuyerDetails.Text == "ADD")
        {
            txtBuyerSaleRate.Text = "";
        }
        setFocusControl(txtBuyerSaleRate);
    }

    private void TCSAmountCalculate()
    {
        try
        {
            double cashdiffvalue = 0.00;
            double qntl = txtBuyerQuantal.Text != string.Empty ? Convert.ToDouble(txtBuyerQuantal.Text) : 0.00;
            double srate = Convert.ToDouble((txtBuyerSaleRate.Text != string.Empty ? Convert.ToDouble(txtBuyerSaleRate.Text) : 0));
            double millRate = (Convert.ToDouble(txtMillRate.Text != string.Empty ? Convert.ToDouble(txtMillRate.Text) : 0));
            double millDeff = (Convert.ToDouble(txtmillDeff.Text != string.Empty ? Convert.ToDouble(txtmillDeff.Text) : 0));
            double RatePerQtl = Convert.ToDouble((txtRatePerQtl.Text != string.Empty ? Convert.ToDouble(txtRatePerQtl.Text) : 0));

            double ReverseAmount = 0.00;
            double itemAmount = 0.00;
            double profit = 0.00;
            double rate = 0.00;
            itemAmount = qntl * srate;
            txtItemAmount.Text = itemAmount.ToString();

            rate = srate - (millRate);
            profit = qntl * rate;
            hdnfProfit.Value = profit.ToString();
            ReverseAmount = qntl * RatePerQtl;
            txtReverseAmount.Text = ReverseAmount.ToString();

        }
        catch
        {
        }
    }
    #endregion

    #region [txtBuyerSaleRate_TextChanged]
    protected void txtBuyerSaleRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtPaid);
        TCSAmountCalculate();
    }
    #endregion

    #region [txtItemAmount_TextChanged]
    protected void txtItemAmount_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region [txtBuyerNarration_TextChanged]
    protected void txtBuyerNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtotherpaidAc);
    }
    #endregion

    #region [txtNarration_TextChanged]
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtMemberName);
    }
    #endregion



    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    protected void btn_Click(object sender, EventArgs e)
    {
        //string url = "http://www.google.com";
        //StringBuilder sb = new StringBuilder();
        //sb.Append("<script type = 'text/javascript'>");
        //sb.Append("window.open('");
        //sb.Append(url);
        //sb.Append(",'_blank'");
        //sb.Append("');");
        //sb.Append("</script>");
        //ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {

        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);

            if (a == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Enter Only Numbers!')", true);
            }
            else
            {
                int Tender_id = Convert.ToInt32(clsCommon.getString("select grouptenderid from GroupTenderPurchase where Tender_No=" + txtEditDoc_No.Text +
                    " and Company_Code=" + Session["Company_Code"].ToString()));
                //   + " and " +  " Year_Code=" + Session["year"].ToString() + ""));
                hdnf.Value = Tender_id.ToString();
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select isnull(count(Tender_No),0) as Tender_No from " + tblHead +
                " where Company_Code='" + Session["Company_Code"].ToString() + "'"));
            //"' and Year_Code='" + Session["Year"].ToString() + "'"));
            if (counts == 0)
            {
                txtTenderNo.Text = "1";
                Tender_No = 1;
            }
            else
            {
                Tender_No = Convert.ToInt32(clsCommon.getString("SELECT max(Tender_No) as Tender_No from " + tblHead +
                    " where Company_Code='" + Session["Company_Code"].ToString() + "'")) + 1;
                //"' and  Year_Code='" + Session["Year"].ToString() + "'")) + 1;
                txtTenderNo.Text = Tender_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT isnull(count(grouptenderid),0) as grouptenderid from " + tblHead + " "));
            if (counts == 0)
            {
                lblTender_Id.Text = "1";
                id = 1;
            }
            else
            {
                id = Convert.ToInt32(clsCommon.getString("SELECT max(grouptenderid) as grouptenderid from " + tblHead)) + 1;
                lblTender_Id.Text = id.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion




    protected void txtitem_code_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            string itemname = string.Empty;
            searchString = txtMillCode.Text;
            if (txtitem_code.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtitem_code.Text);
                if (a == false)
                {
                    btntxtitem_code_Click(this, new EventArgs());
                }
                else
                {
                    DataSet ds = clsDAL.SimpleQuery("select System_Name_E,Gst_Code,gstratre,systemid,HSN,KgPerKatta from qrymstitem where System_Type='I' and System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
                    string pck = "0";
                    // pck = clsCommon.getString("select KgPerKatta from qrymstitem where System_Type='I' and System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            itemname = ds.Tables[0].Rows[0]["System_Name_E"].ToString();
                            string itemgst = ds.Tables[0].Rows[0]["Gst_Code"].ToString();

                            string rr = ds.Tables[0].Rows[0]["gstratre"].ToString();
                            if (itemname != string.Empty && itemname != "0")
                            {
                                lblitemname.Text = itemname;
                                txtPacking.Text = ds.Tables[0].Rows[0]["KgPerKatta"].ToString();
                                if (txtQuantal.Text != string.Empty)
                                {
                                    int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                                    txtBags.Text = bags.ToString();
                                }
                                if (itemgst == string.Empty)
                                {


                                }
                                else
                                {
                                }


                                setFocusControl(txtGrade);
                            }
                            else
                            {
                                txtitem_code.Text = string.Empty;
                                lblitemname.Text = string.Empty;
                                setFocusControl(txtitem_code);
                            }
                        }
                        else
                        {
                            txtitem_code.Text = string.Empty;
                            lblitemname.Text = string.Empty;
                            setFocusControl(txtitem_code);
                        }
                    }
                    else
                    {
                        txtitem_code.Text = string.Empty;
                        lblitemname.Text = string.Empty;
                        setFocusControl(txtitem_code);
                    }
                }
            }
            else
            {
                txtitem_code.Text = string.Empty;
                lblitemname.Text = string.Empty;
                setFocusControl(txtitem_code);
            }
        }
        catch
        {
        }
    }
    protected void btntxtitem_code_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchText.Text = string.Empty;
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtitem_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region LV POsting
    private void LV_Posting()
    {
        try
        {
            LV = new LocalVoucher();
            #region max
            int counts = 0;
            if (btnSave.Text == "Save")
            {
                counts = Convert.ToInt32(clsCommon.getString("select isnull(count(doc_no),0) as doc_no from commission_bill where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'"));
                if (counts == 0)
                {
                    LV.LV_Doc_No = 1;
                    voucher_no = 1;

                }
                else
                {
                    LV.LV_Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from commission_bill where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                           " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                    voucher_no = LV.LV_Doc_No;
                }
                counts = Convert.ToInt32(clsCommon.getString("SELECT count(commissionid) as commissionid from commission_bill "));
                if (counts == 0)
                {
                    LV.LV_commissionid = 1;

                }
                else
                {
                    LV.LV_commissionid = Convert.ToInt32(clsCommon.getString("SELECT max(commissionid) as commissionid from commission_bill")) + 1;

                }
            }
            else
            {

            }
            #endregion

            string PartyStateCode = clsCommon.getString("select GSTStateCode from qrymstaccountmaster where Ac_Code=" + Voucher_By + " and Company_Code='" + Session["Company_Code"].ToString() + "'");
            string CompanyStateCode = clsCommon.getString("select GSTStateCode from NT_1_CompanyParameters where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'");

            double CGSTRATE = Convert.ToDouble(clsCommon.getString("select CGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double SGSTRATE = Convert.ToDouble(clsCommon.getString("select SGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double IGSTRATE = Convert.ToDouble(clsCommon.getString("select IGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double GSTRATE = Convert.ToDouble(clsCommon.getString("select Rate from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));


            double taxmillamt = Quantal * Diff_Amount;

            double CGSTAmount = 0.00;
            double SGSTAmount = 0.00;
            double IGSTAmount = 0.00;

            if (CompanyStateCode == PartyStateCode)
            {
                CGSTAmount = Math.Round((taxmillamt * CGSTRATE / 100), 2);
                SGSTAmount = Math.Round((taxmillamt * SGSTRATE / 100), 2);
                IGSTAmount = 0.00;
                IGSTRATE = 0.00;
            }
            else
            {
                CGSTAmount = 0.000;
                SGSTAmount = 0.00;
                CGSTRATE = 0.00;
                SGSTRATE = 0.00;
                IGSTAmount = Math.Round((taxmillamt * IGSTRATE / 100), 2);

            }

            double Voucher_Amt = taxmillamt + CGSTAmount + SGSTAmount + IGSTAmount;

            LV.LV_ac = vb;
            LV.LV_bc = bk;
            LV.LV_mc = mc;
            LV.LV_Doc_No = voucher_no;
            LV.LV_Doc_Date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            LV.LV_Link_No = 0;
            LV.LV_link_Type = "";
            LV.LV_Link_id = 0;
            LV.LV_Ac_Code = Convert.ToInt32(Voucher_By);
            LV.LV_Unit_Code = 0;
            LV.LV_Quantal = Quantal;
            LV.LV_PACKING = Packing;
            LV.LV_BAGS = Bags;
            LV.LV_Grade = Grade;
            LV.LV_Transport_Code = 0;
            LV.LV_Mill_Rate = Convert.ToDouble(Mill_Rate);
            LV.LV_Sale_Rate = 0.00;
            LV.LV_FREIGHT = 0.00;
            LV.LV_Narration1 = myNarration;
            LV.LV_Narration2 = string.Empty;
            LV.LV_Narration3 = string.Empty;
            LV.LV_Narration4 = string.Empty;
            LV.LV_Voucher_Amount = Diff_Amount;
            LV.LV_Diff_Amount = Diff_Amount;
            LV.LV_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            LV.LV_Year_Code = Convert.ToInt32(Session["year"].ToString());
            LV.LV_Branch_Code = 0;
            LV.LV_Created_By = "" + Session["user"].ToString() + "";
            LV.LV_Commission_Rate = 0.00;
            LV.LV_Resale_Commisson = 0.00;
            LV.LV_GstRateCode = GstRate_Code;
            LV.LV_CGSTRate = CGSTRATE;
            LV.LV_CGSTAmount = CGSTAmount;
            LV.LV_SGSTRate = SGSTRATE;
            LV.LV_SGSTAmount = SGSTAmount;
            LV.LV_IGSTRate = IGSTRATE;
            LV.LV_IGSTAmount = IGSTAmount;
            LV.LV_TaxableAmount = Diff_Amount;

        }
        catch
        {
        }
    }
    #endregion

    #region[txtGroup_TextChanged]
    protected void txtGroup_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtGroup.Text;
            string Group_Account = string.Empty;
            if (txtGroup.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtGroup.Text);
                if (a == false)
                {
                    btnGroup_Click(this, new EventArgs());
                }
                else
                {
                    Group_Account = clsCommon.getString("select GroupName from GroupCreactionMaster where Doc_No='" + txtGroup.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (Group_Account != string.Empty && Group_Account != "0")
                    {
                        hdnfgid.Value = clsCommon.getString("select isnull(autoid,0) as acid from GroupCreactionMaster where Doc_no='" + txtGroup.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                        if (Group_Account.Length > 15)
                        {
                            Group_Account.Substring(0, 15);
                        }
                        else if (Group_Account.Length > 10)
                        {
                            Group_Account.Substring(0, 10);
                        }
                        lblGroup_Name.Text = Group_Account;
                        setFocusControl(txtMillCode);

                    }
                    else
                    {
                        txtGroup.Text = string.Empty;
                        lblGroup_Name.Text = string.Empty;
                        setFocusControl(txtGroup);
                        // AmtCalculation();
                    }
                }
            }
            else
            {
                txtGroup.Text = string.Empty;
                lblGroup_Name.Text = Group_Account;
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnGroup_Click]
    protected void btnGroup_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "Group";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }

    }
    #endregion





    [WebMethod]
    public static string XMLInsert(string XML, string status)
    {

        string myString = "'1' is an unexpected token. The expected token is  or. Line 1, position 170.";
        string lastWord = myString.Split(' ').Last();
        string msgReturn = "";
        try
        {
            msgReturn = XML.Length.ToString();
            //   XML.Replace("''", """");
            //msgReturn = GetWordAtPosition(XML, 4313);
            XDocument XDoc = XDocument.Parse(XML, LoadOptions.None);
            XML = XDoc.ToString(SaveOptions.DisableFormatting);
            return msgReturn;
        }
        catch
        {
            return "";
        }
    }

    [WebMethod]
    public static string NewInsert(string TenderInsertUpdate, string TenderDetail_Insert, string TenderDetail_Update, string TenderDetail_Delete, string status)
    {
        try
        {
            cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "HeadDetail_CrudOpration";
            cmd.CommandType = CommandType.StoredProcedure;
            //an out parameter


            //an in parameter
            cmd.Parameters.AddWithValue("QryInsertAndUpdate", TenderInsertUpdate);
            cmd.Parameters["QryInsertAndUpdate"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("QryDetail_Insert", TenderDetail_Insert);
            cmd.Parameters["QryDetail_Insert"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("QryDetail_Update", TenderDetail_Update);
            cmd.Parameters["QryDetail_Update"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("QryDetail_Delete", TenderDetail_Delete);
            cmd.Parameters["QryDetail_Delete"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("QryGledger_Delete", "");
            cmd.Parameters["QryGledger_Delete"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("QryGledger_Insert", "");
            cmd.Parameters["QryGledger_Insert"].Direction = ParameterDirection.Input;

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            _adapter.Fill(ds);
            string msgReturn = "";
            if (status == "Save")
            {
                msgReturn = "Record Successfully Added";
            }
            else if (status == "Update")
            {
                msgReturn = "Record Successfully Update";
            }
            return msgReturn;

        }
        catch
        {
            con.Close();
            return "";
        }
        finally
        {
            con.Close();
        }
    }
    protected void TdsCalculation()
    {
        try
        {
            double TDSRate = 0.00;
            double Quantal = 0.00;
            double TdSAmt = 0.00;
            double psamt = 0.00;
            if (txtQuantal.Text != string.Empty)
            {
                Quantal = Convert.ToDouble(txtQuantal.Text);
            }
            if (txtQuantal.Text != string.Empty)



                TdSAmt = Math.Round(((Quantal * psamt) * TDSRate) / 100, 2);
        }
        catch (Exception)
        {
            throw;
        }

    }
    protected void TCSCalculation()
    {
        try
        {
            double Quantal = 0.00;
            double MillDeff = 0.00;
            if (txtQuantal.Text != string.Empty)
            {
                Quantal = Convert.ToDouble(txtQuantal.Text);

            }
            if (txtmillDeff.Text != string.Empty)
            {
                MillDeff = Convert.ToDouble(txtmillDeff.Text);
            }
            double value = 0.00;
            value = Quantal * MillDeff;
            lblValue.Text = value.ToString();


        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtcashdiff_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtPaid_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtDetailpaidDate);
    }

    protected void txtotherPaid_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnADDBuyerDetails);
    }

    protected void txtvtdsrate_TextChanged(object sender, EventArgs e)
    {

    }

    #region [txtDetailpaidDate_TextChanged]
    protected void txtDetailpaidDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDetailpaidDate.Text != string.Empty)
            {
                //string d = DateTime.Parse(txtDetailpaidDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                //txtDetailLiftingDate.Text = clsCommon.getString("SELECT date_format(DATE_ADD('" + d + "', INTERVAL 10 DAY),'%d/%m/%Y') as d");

            }
        }
        catch
        {
            txtDetailpaidDate.Text = string.Empty;
            setFocusControl(txtDetailpaidDate);
        }
    }
    #endregion





    #region [btnProsess_Click]
    protected void btnProsess_Click(object sender, EventArgs e)
    {
        try
        {
            bool allData = YourMethodAll(); // Assuming YourMethodAll() returns a boolean indicating data status

            if (allData)
            {
                double detailPaidValue = 0.00;
                double value = Convert.ToDouble(lblValue.Text.ToString());

                value = Math.Abs(value);

                qry = "select sum(paid) as Detail_paid from GroupTenderDetails where Tender_No=" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                string detailPaid = clsCommon.getString(qry);
                detailPaidValue = Convert.ToDouble(detailPaid.ToString());

                detailPaidValue = Math.Abs(detailPaidValue);

                Double Deff = 0.00;
                Deff = value - detailPaidValue;
                Deff = Math.Abs(Deff);

                if (detailPaidValue == value)
                {
                    DataSet ds2 = new DataSet();
                    string qry2 = "update GroupTenderDetails set indivisulProfit=0,millContribution=0, paidorreseive=0 where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                                   + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtTenderNo.Text + "";
                    ds2 = clsDAL.SimpleQuery(qry2);

                    string qry1 = "select COUNT(*) as a from GroupTenderDetails where ID <> 1 and Tender_No=" + txtTenderNo.Text + " group by membercode ";
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    ds1 = clsDAL.SimpleQuery(qry1);
                    dt1 = ds1.Tables[0];
                    int numberofMember = dt1.Rows.Count;

                    double indivisulProfit = 0.00;
                    double millContribution = 0.00;
                    double memberprofit = 0.00;
                    double NetMemberProfit = 0.00;
                    double pd = 0.00;
                    double paidorreseive = 0.00;
                    int memberNumber = 0;
                    int tenderDetailId = 0;

                    string profit = clsCommon.getString("select profit from GroupTenderDetails where ID = 1 and Tender_No=" + txtTenderNo.Text + "");
                    double paidtomill = Convert.ToDouble(clsCommon.getString("select isnull(SUM(paid), 0) from GroupTenderDetails where Tender_No=" + txtTenderNo.Text + ""));
                    double OtherPaid = Convert.ToDouble(clsCommon.getString("select isnull(SUM(otherPaid), 0)  from GroupTenderDetails where Tender_No=" + txtTenderNo.Text + ""));
                    paidtomill = paidtomill + OtherPaid;
                    qry = "select membercode ,isnull(SUM(profit) ,0) as p , isnull(SUM(paid ),0)+isnull(SUM(otherPaid ),0) as pd,0  as indivisulProfit,0 as millContribution,0 as paidorreseive from GroupTenderDetails where ID <> 1 and Tender_No=" + txtTenderNo.Text + " group by membercode";
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            indivisulProfit = Convert.ToDouble(profit) / numberofMember;
                            millContribution = Convert.ToDouble(paidtomill) / numberofMember;
                            dt.Rows[i]["indivisulProfit"] = indivisulProfit;
                            dt.Rows[i]["millContribution"] = millContribution;

                            memberprofit = Convert.ToDouble(dt.Rows[i]["p"].ToString());
                            pd = Convert.ToDouble(dt.Rows[i]["pd"].ToString());

                            NetMemberProfit = (indivisulProfit - millContribution + pd) - memberprofit;
                            dt.Rows[i]["paidorreseive"] = NetMemberProfit;

                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            memberNumber = Convert.ToInt32(dt.Rows[i]["membercode"].ToString());
                            indivisulProfit = Convert.ToDouble(dt.Rows[i]["indivisulProfit"].ToString());
                            millContribution = Convert.ToDouble(dt.Rows[i]["millContribution"].ToString());
                            paidorreseive = Convert.ToDouble(dt.Rows[i]["paidorreseive"].ToString());
                            tenderDetailId = Convert.ToInt32(clsCommon.getString("select top(1) trnderdetailid from GroupTenderDetails where membercode=" + memberNumber + " and Tender_No=" + txtTenderNo.Text + ""));

                            qry2 = "update GroupTenderDetails set indivisulProfit=" + indivisulProfit + ",millContribution=" + millContribution + ", paidorreseive=" + paidorreseive + " where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and trnderdetailid=" + tenderDetailId + "";
                            ds2 = clsDAL.SimpleQuery(qry2);
                        }

                    }
                    qry2 = "update GroupTenderPurchase set Prosess='Y' where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtTenderNo.Text + "";
                    ds2 = clsDAL.SimpleQuery(qry2);

                    qry2 = "DELETE FROM nt_1_gledger WHERE DOC_NO = " + txtTenderNo.Text + " and TRAN_TYPE='GT' and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    ds2 = clsDAL.SimpleQuery(qry2);


                    qry = "select Tender_No, membercode ,mid , indivisulProfit, millContribution, paidorreseive from GroupTenderDetails where ID <> 1 and Tender_No=" + txtTenderNo.Text + " group by Tender_No, membercode ,mid , indivisulProfit, millContribution, paidorreseive";
                    ds = clsDAL.SimpleQuery(qry);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            paidorreseive = Convert.ToDouble(dt.Rows[i]["paidorreseive"].ToString());
                            memberNumber = Convert.ToInt32(dt.Rows[i]["membercode"].ToString());
                            indivisulProfit = Convert.ToDouble(dt.Rows[i]["indivisulProfit"].ToString());
                            millContribution = Convert.ToDouble(dt.Rows[i]["millContribution"].ToString());
                            Tender_No = Convert.ToInt32(dt.Rows[i]["Tender_No"].ToString());
                            memberid = Convert.ToInt32(dt.Rows[i]["mid"].ToString());

                            //if (paidorreseive < 0)
                            //{
                            //    double absolutePaidorreseive = Math.Abs(paidorreseive);
                            //    qry2 = " INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO, DOC_DATE, AC_CODE, UNIT_Code, NARRATION, AMOUNT, TENDER_ID, TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE, YEAR_CODE, ORDER_CODE, DRCR, DRCR_HEAD, ADJUSTED_AMOUNT, Branch_Code, SORT_TYPE, SORT_NO, ac, vc, progid, tranid,   saleid) " +
                            //           " VALUES ('GT', '', '" + Tender_No + "', '" + txtDate.Text + "', '" + memberNumber + "', '0', '', " + absolutePaidorreseive + ", 0, 0, 0, '" + Convert.ToInt32(Session["Company_Code"].ToString()) + "', '" + Convert.ToInt32(Session["year"].ToString()) + "', '0', 'D', '0', 0, 0, '', 0, " + memberid + ", 0, 0, 0,  0)";
                            //    flag = 1;
                            //    Thread thred = new Thread(() => { count = DataStore(qry2, flag); }); //Calling DataStore Method Using Thread
                            //    thred.Start(); //Thread Operation Start
                            //    thred.Join();

                            //}
                            //else
                            //{

                            //    qry2 = " INSERT INTO nt_1_gledger (TRAN_TYPE, CASHCREDIT, DOC_NO, DOC_DATE, AC_CODE, UNIT_Code, NARRATION, AMOUNT, TENDER_ID, TENDER_ID_DETAIL, VOUCHER_ID, COMPANY_CODE, YEAR_CODE, ORDER_CODE, DRCR, DRCR_HEAD, ADJUSTED_AMOUNT, Branch_Code, SORT_TYPE, SORT_NO, ac, vc, progid, tranid,  saleid) " +
                            //           " VALUES ('GT', '', '" + Tender_No + "', '" + txtDate.Text + "', '" + memberNumber + "', '0', '', " + paidorreseive + ", 0, 0, 0, '" + Convert.ToInt32(Session["Company_Code"].ToString()) + "', '" + Convert.ToInt32(Session["year"].ToString()) + "', '0', 'C', '0', 0, 0, '', 0, " + memberid + ", 0, 0, 0,   0)";
                            //    flag = 1;
                            //    Thread thred = new Thread(() => { count = DataStore(qry2, flag); }); //Calling DataStore Method Using Thread
                            //    thred.Start(); //Thread Operation Start
                            //    thred.Join();
                            //} 
                        }

                    }

                    //Response.Redirect(Request.Url.AbsoluteUri);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Sucessfully  Process!')", true);
                    string groupid = hdnf.Value;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:Open('" + groupid + "')", true);


                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Mill Diff Amount and Detail Paid Amount Are Not Match difference between " + Deff + " ')", true);
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

    #region [btnGroupDetailPrint_Click]
    protected void btnGroupDetailPrint_Click(object sender, EventArgs e)
    {

        string Tender_no = txtTenderNo.Text; 
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:GD('" + Tender_no  + "','Y')", true);
         
    }
    #endregion

    #region [btnGroupSummery_Click]
    protected void btnGroupSummery_Click(object sender, EventArgs e)
    {

        string Tender_no = txtTenderNo.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:GS('" + Tender_no + "')", true);

    }
    #endregion


    protected void YourMethod() // This is an example method to replicate the logic
    {
        if (grdDetail != null)
        {
            foreach (System.Web.UI.WebControls.GridViewRow row in grdDetail.Rows)
            {
                
                    double Quantal;
                    if (double.TryParse(row.Cells[5].Text, out Quantal))
                    {
                        // Use the 'ID' variable as needed...
                        if (Quantal > 0)
                        {
                            drCalculateStock.Text = "Y";

                        }
                        else {
                            drCalculateStock.Text = "N";
                        }
                    }
                    break;
            }
        }
    }

    //protected void YourMethodAll() // This is an example method to replicate the logic
    //{
    //    List<int> parsedQuantals = new List<int>();
    //    if (grdDetail != null)
    //    {
    //        foreach (System.Web.UI.WebControls.GridViewRow row in grdDetail.Rows)
    //        {

    //            int Member;
    //            if (int.TryParse(row.Cells[3].Text, out Member))
    //            {
    //                parsedQuantals.Add(Member);
    //            } 
    //        }
    //        parsedQuantals = parsedQuantals.Distinct().ToList();
    //        parsedQuantals.Sort();
    //        foreach (int parsedQuantal in parsedQuantals)
    //        {
    //            // Perform operations with 'parsedQuantal' as needed
    //        }
    //    }
    //}

    protected bool YourMethodAll()
    {
        bool dataIsTrue = false; 
        List<int> parsedQuantals = new List<int>();
        List<int> membercodeList = new List<int>(); 
        if (grdDetail != null && grdDetail.Rows.Count > 0) 
        {
            foreach (System.Web.UI.WebControls.GridViewRow row in grdDetail.Rows)
            {
                int Member;
                if (int.TryParse(row.Cells[3].Text, out Member))
                {
                    parsedQuantals.Add(Member);
                }
            }

            parsedQuantals = parsedQuantals.Distinct().ToList();
            parsedQuantals.Sort();

            //membercodeList.Add(2);  
            parsedQuantals = parsedQuantals.Distinct().OrderBy(q => q).ToList();

            // Remove the first element if the list has more than one element
            if (parsedQuantals.Count > 1)
            {
                parsedQuantals.RemoveAt(0);
            }

            qry = "select member from qryGroupMemberUnion where member!=0 and Doc_No=" + txtGroup.Text + " and " +
                    " Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            ds = clsDAL.SimpleQuery(qry);
             dt = ds.Tables[0];
             int ac;
            for (int i = 0; i < dt.Rows.Count; i++) {

                ac = Convert.ToInt32(dt.Rows[i]["member"].ToString());
               membercodeList.Add(ac);
                
             }
             membercodeList = membercodeList.Distinct().ToList();
             membercodeList.Sort();

             for (int i = 0; i < membercodeList.Count; i++)
             {
                 int parsed = i < parsedQuantals.Count ? parsedQuantals[i] : 0;
                 int membercode = i < membercodeList.Count ? membercodeList[i] : 0; // Get the current membercode or default to 0 if not available

                 if (membercode == parsed)
                 {
                     dataIsTrue = true; // Set the flag to true if the condition is met
                     Console.WriteLine("Data is true for membercode: " + parsed + ". Do something here...");
                 }
                 else
                 {
                     // For any condition not met, set the flag to false and break the loop if needed
                     dataIsTrue = false;
                     qry = "select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + membercode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    string millDeffAcname = clsCommon.getString(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please add Member In Detail " + millDeffAcname + "')", true);
                     break; // Breaking the loop since the condition isn't met
                 }
             }
        }
        return dataIsTrue;
    }







}