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


public partial class Sugar_BussinessRelated_pgeGroupDispatch : System.Web.UI.Page
{
    #region data section
    string temp = "0";
    string doauth = "";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMastertable = string.Empty;
    string qryCommon = string.Empty;
    string qryHead = string.Empty;
    string qryDetail = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qrycarporateSalebalance = string.Empty;
    string qryUTRBalance = string.Empty;
    string qrypurc_No = string.Empty;
    string qryAccountList = string.Empty;
    string millShortName = string.Empty;
    int defaultAccountCode = 0;
    string trnType = "dispatchId";
    string AUTO_VOUCHER = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    public static int an = 0;
    double LvAmnt = 0.00;
    string Action = string.Empty;
    string cs = string.Empty;
    int DOC_NO = 0;
    int doid = 0;
    double OldGSTAmt = 0.00;
    double OldSaleAmt = 0.00;
    double OldCommission = 0.00;
    double OldBillAmt;
    double OldQty = 0.00;

    string salebillname = string.Empty;
    string salebilltocityname = string.Empty;
    #endregion


    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    DataTable dt1 = null;
    DataTable dt2 = null;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    DataSet DS = null;
    int counts = 0;
    int Doc_No = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            user = Session["user"].ToString();
            doauth = Session["DOPages"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "GroupDispatch";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMastertable = tblPrefix + "SystemMaster";
            qryCommon = "qryGroupDispatch";
            qryHead = "qrydohead";
            qryDetail = "qrydodetail";
            pnlPopup.Style["display"] = "none";
            GLedgerTable = tblPrefix + "GLEDGER";
            qrycarporateSalebalance = tblPrefix + "qryCarporatesellbalance";
            qryUTRBalance = tblPrefix + "qryUTRBalance";
            qryAccountList = "qrymstaccountmaster";
            qrypurc_No = "qrysugarBalancestock";

            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            Maindt = new DataTable();
            dr = null;

            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            DS = new DataSet();
            if (!Page.IsPostBack)
            {
                hdnfyearcode.Value = Session["year"].ToString();
                hdnfcompanycode.Value = Session["Company_Code"].ToString();
                //txtSearchText.Attributes.Add("onkeypress", "abc(event);");
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    if (doauth == "1")
                    {
                        Action = Request.QueryString["Action"];
                        if (Action == "1")
                        {
                            hdnf.Value = Request.QueryString["dispatchId"];
                            pnlPopup.Style["display"] = "none";
                            ViewState["currentTable"] = null;
                            clsButtonNavigation.enableDisable("N");

                            this.makeEmptyForm("N");
                            ViewState["mode"] = "I";
                            this.showLastRecord();
                            this.enableDisableNavigateButtons();

                            setFocusControl(btnEdit);
                        }
                        else
                        {
                            string docno = string.Empty;
                            clsButtonNavigation.enableDisable("A");
                            ViewState["mode"] = null;
                            ViewState["mode"] = "I";
                            this.makeEmptyForm("A");
                            this.NextNumber();
                            btntxtDOC_NO.Enabled = false;
                        }
                        txtFromDate.Text = Session["Start_Date"].ToString();
                        txtToDate.Text = Session["End_Date"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                    }
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
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

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {

            string mystring = "select * from " + qryCommon + " where dispatchId=" + hdnf.Value + "";


            return mystring;
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
                        hdnfyearcode.Value = dt.Rows[0]["Year_Code"].ToString();
                        hdnf.Value = dt.Rows[0]["dispatchId"].ToString();
                        txtdoc_no.Text = dt.Rows[0]["Doc_no"].ToString();
                        hdnflvdoc.Value = dt.Rows[0]["Doc_no"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtMILL_CODE.Text = dt.Rows[0]["MillCode"].ToString();
                        hdnfmc.Value = dt.Rows[0]["mc"].ToString();
                        LBLMILL_NAME.Text = dt.Rows[0]["MillName"].ToString();
                        txtPurcNo.Text = dt.Rows[0]["purchaseCode"].ToString();
                        txtPurcOrder.Text = dt.Rows[0]["purchaseNmber"].ToString();
                        txtGETPASS_CODE.Text = dt.Rows[0]["getpass"].ToString();
                        hdnfgp.Value = dt.Rows[0]["gid"].ToString();
                        LBLGETPASS_NAME.Text = dt.Rows[0]["GetPassName"].ToString() + " , " + dt.Rows[0]["GetpassCity"].ToString() ;
                        txtvoucher_by.Text = dt.Rows[0]["shipto"].ToString();
                        hdnfst.Value = dt.Rows[0]["shiptoid"].ToString();
                        lblvoucherbyname.Text = dt.Rows[0]["ShipToName"].ToString() + " , " + dt.Rows[0]["ShiptoCityName"].ToString();
                        txtSaleBillTo.Text = dt.Rows[0]["saleBillTo"].ToString();
                        hdnfsb.Value = dt.Rows[0]["saleBillToId"].ToString();
                      
                        salebillname = dt.Rows[0]["SaleBillToName"].ToString();
                        salebilltocityname = dt.Rows[0]["SaleBillToCityName"].ToString();
                        txtNARRATION4.Text = salebillname + ", " + salebilltocityname;
                        txtGRADE.Text = dt.Rows[0]["grade"].ToString();
                        txtquantal.Text = dt.Rows[0]["quantal"].ToString();
                        txtPACKING.Text = dt.Rows[0]["packing"].ToString();
                        txtBAGS.Text = dt.Rows[0]["bags"].ToString();
                        txtmillRate.Text = dt.Rows[0]["millRate"].ToString();
                        txtSALE_RATE.Text = dt.Rows[0]["saleRate"].ToString();
                        txtpurchaserate.Text = dt.Rows[0]["purchaseRate"].ToString();
                        recordExist = true;
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

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        //ViewState["mode"] = null;
        //ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.NextNumber();
        //setFocusControl(txtBank); 
        //string gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //pnlPopupDetails.Style["display"] = "none";

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
        setFocusControl(txtMILL_CODE);

    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "system", "javascript:DeleteConform();", true);
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
        hdnf.Value = Request.QueryString["dispatchId"];

        if (hdnf.Value == "0" || hdnf.Value == "")
        {
            int maxno = Convert.ToInt32(clsCommon.getString("select isnull(max(dispatchId),0) as dispatchId from " + tblHead + " "));

            hdnf.Value = Convert.ToString(maxno);
        }
        Response.Redirect("pgeGroupDispatch.aspx?dispatchId=" + hdnf.Value + "&Action=" + 1);
    }
    #endregion

    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (hdnfyearcode.Value != Session["year"].ToString())
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records year code & current year code is not same!')", true);
            return;
            // Response.Redirect("~/Sugar/pgeHome.aspx", false);
        }
        if (hdnfcompanycode.Value != Session["Company_Code"].ToString())
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records company code & current company code is not same!')", true);
            return;

        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "system", "javascript:pagevalidation();", true);

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
                txtEditDoc_No.Enabled = true;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;

                pnlPopup.Style["display"] = "none";

                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                lblMsg.Text = string.Empty;


                #region Logic
                calenderExtenderDate.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtvoucher_by.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btntxtPurcNo.Enabled = false;
                ViewState["currentTable"] = null;
                lblVoucherLedgerByBalance.Text = string.Empty;
                lblSaleBillToLedgerByBalance.Text = string.Empty;
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


                hdnfmc.Value = "0";
                hdnfgp.Value = "0";
                hdnfst.Value = "0";
                hdnfsb.Value = "0";
                //txtSaleBillTo.Enabled = false;

                txtNARRATION4.Enabled = false;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Change No";
                btntxtDOC_NO.Enabled = true;

                //drpDeliveryType.Enabled = true; 
                #region set Business logic for save

                // btnTransLetter.Enabled = false;

                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;

                btntxtGRADE.Enabled = true;

                btntxtNARRATION4.Enabled = true;

                txtdoc_no.Enabled = false;
                btntxtPurcNo.Enabled = true;
                txtGETPASS_CODE.Enabled = false;
                LBLMILL_NAME.Text = string.Empty;
                LBLGETPASS_NAME.Text = string.Empty;
                lblvoucherbyname.Text = string.Empty;


                lblMsg.Text = "";
                ViewState["currentTable"] = null;


                txtDOC_DATE.Text = DateTime.Now.ToString("dd/MM/yyyy");

                setFocusControl(txtDOC_DATE);

                ViewState["GstRateAutoId"] = null;


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

                txtEditDoc_No.Enabled = true;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;

                #region Logic
                calenderExtenderDate.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;

                btntxtvoucher_by.Enabled = false;
                btntxtGRADE.Enabled = false;

                btntxtNARRATION4.Enabled = false;

                btntxtPurcNo.Enabled = false;


                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;

                        if (((System.Web.UI.WebControls.TextBox)c).Text == "0.00")
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Text = string.Empty;
                        }
                    }
                }
                txtEditDoc_No.Enabled = false;


                txtNARRATION4.Enabled = false;
                txtGETPASS_CODE.Enabled = false;


                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = true;
                lblMsg.Text = string.Empty;

                #region set Business logic for edit

                calenderExtenderDate.Enabled = true;

                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;
                btntxtGRADE.Enabled = true;

                btntxtNARRATION4.Enabled = true;

                btntxtPurcNo.Enabled = true;



                #endregion



            }

            txtPurcNo.Enabled = false;
            txtPurcOrder.Enabled = false;


        }
        catch
        {
        }
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {

            counts = Convert.ToInt32(clsCommon.getString("select count(Doc_no) as Doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(Doc_no) as Doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);

        }
    }
    #endregion


    #region txtEditDoc_No_TextChanged
    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);

        //    if (a == false)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Enter Only Numbers!')", true);
        //    }
        //    else
        //    {
        //        int do_id = Convert.ToInt32(clsCommon.getString("select doid from nt_1_deliveryorder where Doc_no=" + txtEditDoc_No.Text +
        //            " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" +
        //            " " + Session["year"].ToString() + ""));

        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:DoOPen('" + do_id + "')", true);

        //    }
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
    }
    #endregion

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
        //csCalculations();
    }
    #endregion

    #region [btntxtdoc_no_Click]
    protected void btntxtdoc_no_Click(object sender, EventArgs e)
    {
        try
        {
            //pnlPopup.Style["display"] = "block";
            //hdnfClosePopup.Value = "txtEditDoc_No";
            //btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtDOC_DATE.Text;
        //strTextBox = "txtDOC_DATE";
        //csCalculations();
    }
    #endregion


    #region [txtMILL_CODE_TextChanged]
    protected void txtMILL_CODE_TextChanged(object sender, EventArgs e)
    {

        searchString = txtMILL_CODE.Text;
        strTextBox = "txtMILL_CODE";
        csCalculations();
        if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
        {
            if (ViewState["mode"].ToString() == "I")
            {
                // calculation();
            }
        }
    }
    #endregion

    #region [btntxtMILL_CODE_Click]
    protected void btntxtMILL_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMILL_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtPurcNo_TextChanged]
    protected void txtPurcNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtPurcNo.Text;
            strTextBox = "txtPurcNo";
            txtPurcOrder.Enabled = false;
            txtPurcNo.Enabled = false;
            if (strTextBox == "txtPurcNo")
            {
                setFocusControl(txtvoucher_by);
                int i = 0;
                i++;
                string a = txtPurcOrder.Text;
                if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
                {
                    string qry = "select membercode,membername,Grade,Buyer_Quantal,Packing,Bags," +
                       " Mill_Rate,Sale_Rate," +
                      " itemcode,trnderdetailid from  qryGroupTenderHeadDetail" +
                       "  where Tender_No=" + txtPurcNo.Text + " and ID=" + txtPurcOrder.Text +
                       " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());





                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    string PP = "";
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                txtvoucher_by.Text = dt.Rows[0]["membercode"].ToString();
                                lblvoucherbyname.Text = dt.Rows[0]["membername"].ToString();
                                txtSaleBillTo.Text = dt.Rows[0]["membercode"].ToString();
                                txtNARRATION4.Text = dt.Rows[0]["membername"].ToString();
                                hdnfTenderDetailid.Value = dt.Rows[0]["trnderdetailid"].ToString();
                                txtGRADE.Text = dt.Rows[0]["Grade"].ToString();
                                txtPACKING.Text = dt.Rows[0]["Packing"].ToString();
                                txtBAGS.Text = dt.Rows[0]["Bags"].ToString();
                                txtmillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                                txtSALE_RATE.Text = dt.Rows[0]["Sale_Rate"].ToString();
                                double SR = Convert.ToDouble(dt.Rows[0]["Sale_Rate"].ToString());
                                hdnfSaleRate.Value = Convert.ToString(SR);

                                txtPurcNo.Enabled = false;
                                string distance = clsCommon.getString("Select Distance from " + qryAccountList + " where Ac_Code=" + txtvoucher_by.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                txtGETPASS_CODE.Text = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code='" + Session["year"].ToString() + "'");
                                string selfac_name = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtGETPASS_CODE.Text
                                       + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                string selftacc_city = clsCommon.getString("select CityName from " + qryAccountList + "  where Ac_Code=" + txtGETPASS_CODE.Text
                                    + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                LBLGETPASS_NAME.Text = selfac_name + "," + selftacc_city;
                                txtpurchaserate.Text = dt.Rows[0]["Mill_Rate"].ToString();

                            }

                            #region Assign
                            if (txtMILL_CODE.Text != string.Empty)
                            {
                                hdnfmc.Value = clsCommon.getString("select isnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtMILL_CODE.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                                // tenderdetailid = Convert.ToInt32(clsCommon.getString("select tenderdetailid from qrytenderdobalanceview where Mill_Code=" + MILL_CODE + " and Company_code='" + Company_Code + "' and Year_Code='" + Year_Code + "'"));
                            }


                            if (txtGETPASS_CODE.Text != string.Empty)
                            {
                                hdnfgp.Value = clsCommon.getString("select  isnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtGETPASS_CODE.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                            }

                            if (txtvoucher_by.Text != string.Empty)
                            {
                                hdnfst.Value = clsCommon.getString("select  isnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtvoucher_by.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                            }

                            if (txtSaleBillTo.Text != string.Empty)
                            {
                                try
                                {
                                    hdnfsb.Value = clsCommon.getString("select  isnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtSaleBillTo.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                                }
                                catch { }
                            }

                            #endregion

                        }
                    }
                }
            }






        }
        catch
        {
        }
        //setFocusControl(txtvoucher_by);

    }

    #endregion

    #region [btntxtPurcNo_Click]
    protected void btntxtPurcNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurcNo";
            pnlPopup.ScrollBars = ScrollBars.Both;
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #endregion

    #region [txtGETPASS_CODE_TextChanged]
    protected void txtGETPASS_CODE_TextChanged(object sender, EventArgs e)
    {

        searchString = txtGETPASS_CODE.Text;
        strTextBox = "txtGETPASS_CODE";
        string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (txtGETPASS_CODE.Text == selfac)
        {
            txtNARRATION4.Enabled = false;
            txtSaleBillTo.Enabled = true;
        }
        else
        {
            //txtNARRATION4.Enabled = true;
            //txtSaleBillTo.Enabled = true;
            //txtSaleBillTo.Text = "";
            //txtNARRATION4.Text = "";
            //txtSalebilltoGstStateCode.Text = "";
            // lbltxtSalebilltoGstStateName.Text = "";
        }
        csCalculations();
        //setFocusControl(txtitem_Code);
    }
    #endregion

    #region [btntxtGETPASS_CODE_Click]
    protected void btntxtGETPASS_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGETPASS_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtvoucher_by_TextChanged]
    protected void txtvoucher_by_TextChanged(object sender, EventArgs e)
    {

        searchString = txtvoucher_by.Text;
        strTextBox = "txtvoucher_by";
        csCalculations();
    }
    #endregion

    #region [btntxtvoucher_by_Click]
    protected void btntxtvoucher_by_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtvoucher_by";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSaleBillTo_TextChanged]
    protected void txtSaleBillTo_TextChanged(object sender, EventArgs e)
    {

        searchString = txtSaleBillTo.Text;
        strTextBox = "txtSaleBillTo";
        csCalculations();
    }
    #endregion

    #region [txtNARRATION4_TextChanged]
    protected void txtNARRATION4_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION4.Text;
        strTextBox = "txtNARRATION4";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION4_Click]
    protected void btntxtNARRATION4_Click(object sender, EventArgs e)
    {
        try
        {

            searchString = txtSaleBillTo.Text;
            strTextBox = "txtSaleBillTo";
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION4";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtGRADE_TextChanged]
    protected void txtGRADE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGRADE.Text;
        if (txtGRADE.Text != string.Empty)
        {
            bool a = true;
            if (txtGRADE.Text.Length < 8)
            {
                a = clsCommon.isStringIsNumeric(txtGRADE.Text);
            }
            if (a == false)
            {
                btntxtGRADE_Click(this, new EventArgs());
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                setFocusControl(txtquantal);
            }
        }
    }
    #endregion

    #region [btntxtGRADE_Click]
    protected void btntxtGRADE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGRADE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtquantal_TextChanged]
    protected void txtquantal_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    if (txtquantal.Text != string.Empty && txtPACKING.Text != string.Empty && txtquantal.Text != "0" && txtPACKING.Text != "0")
        //    {
        //        int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
        //        txtBAGS.Text = bags.ToString();
        //        if (hdnfpacking.Value != "1")
        //        {
        //            //  setFocusControl(txtPACKING);
        //            setFocusControl(txtSALE_RATE);
        //        }
        //        else
        //        {
        //            setFocusControl(txtvoucher_by);
        //            hdnfpacking.Value = "2";
        //        }
        //    }
        //    else if ((txtPACKING.Text == string.Empty || txtPACKING.Text == "0") && txtquantal.Text != string.Empty && txtquantal.Text != "0")
        //    {
        //        txtPACKING.Text = "50";
        //        int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
        //        txtBAGS.Text = bags.ToString();
        //        setFocusControl(txtPACKING);
        //    }
        //    else
        //    {
        //        txtquantal.Text = string.Empty;
        //        setFocusControl(txtquantal);
        //        txtBAGS.Text = "0";
        //    } 
        //    strTextBox = "txtquantal";
        //    calculation();
        //    MemoadvanceCalculation(); 

        //    hdnfTenderQty.Value = Convert.ToDouble(Convert.ToDouble(hdnfQTY.Value != string.Empty ? hdnfQTY.Value : "0") - Convert.ToDouble(txtquantal.Text)).ToString();
        //}
        //catch { }
    }
    #endregion

    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        //strTextBox = "txtPACKING";
        //csCalculations();
    }
    #endregion

    #region [txtBAGS_TextChanged]
    protected void txtBAGS_TextChanged(object sender, EventArgs e)
    {

        //strTextBox = "txtBAGS";
        //csCalculations();
    }
    #endregion


    #region [txtmillRate_TextChanged]
    protected void txtmillRate_TextChanged(object sender, EventArgs e)
    {
        //strTextBox = "txtmillRate";
        //csCalculations();
        //calculation();
        //GSTCalculations();
        //setFocusControl(txtSALE_RATE);
    }

    #endregion

    #region [txtSALE_RATE_TextChanged]
    protected void txtSALE_RATE_TextChanged(object sender, EventArgs e)
    {

        //strTextBox = "txtSALE_RATE";
        //csCalculations();
        //calculation();
        //GSTCalculations();
    }
    #endregion

    #region [txtpurchaserate_TextChanged]
    protected void txtpurchaserate_TextChanged(object sender, EventArgs e)
    {
        //strTextBox = "txtpurchaserate";
        //csCalculations();
        //calculation();
        //GSTCalculations();
        //setFocusControl(txtCashDiff);
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                setFocusControl(txtGETPASS_CODE);
            }
            if (hdnfClosePopup.Value == "txtvoucher_by")
            {
                setFocusControl(txtvoucher_by);
            }

            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
            setFocusControl(btnSave);
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

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string searchtxt = searchString;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchString;
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtdoc_no" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtDOC_NO.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtDOC_NO.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select dispatchId--";
                    tdDate.Visible = true;
                    string fromdt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    string todt = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

                    string qry = "select  Doc_no as No,convert(varchar(10),doc_date,103) as Date,ISNULL(LEFT(millName,15),millShortName) as Mill,GetPassName as Getpass," +
                        "VoucherByname As Voucher_By,voucherby_cityname,ISNULL(LEFT(narration4,15),millShortName) as SalebillTo,getpasscity,quantal as Qntl,mill_rate as [M.R]," +
                        "sale_rate as [S.R] ,SB_No as SaleBill_no,voucher_no as Purchase_No,truck_no, " +
                        "FreightPerQtl as Frieght,vasuli_rate1 as Vasuli,ISNULL(LEFT(TransportName,15),millShortName)AS TransportShortName,memo_no from NT_1_qryDeliveryOrderList" +
                        " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                        + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='dispatchId' and doc_date between '" + fromdt + "' and '" + todt + "'" +
                        "and(Doc_no like '%" + txtSearchText.Text + "%' or truck_no like '%"
                        + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text
                        + "%' or truck_no like '%" + txtSearchText.Text + "%' or VoucherByname like '%" + txtSearchText.Text + "%' or voucherby_cityname like '%" + txtSearchText.Text + "%') order by Doc_no desc";

                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Mill--";
                string qry = " Locked=0  and  Ac_type='M' and dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Address_E like '%" + txtSearchText.Text + "%' or city_name_e like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";
                this.showPopupAccountMaster(qry);

            }
            if (hdnfClosePopup.Value == "txtBill_To")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from qrymstaccountmaster where Locked=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select GetpassCode--";
                string qry = " Locked=0  and Ac_type!='C' and Ac_type!='B' and dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Address_E like '%" + txtSearchText.Text + "%' or city_name_e like '%" + txtSearchText.Text + "%') order by Ac_Name_E ";
                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtvoucher_by")
            {

                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Voucher--";
                string qry = " Locked=0  and Ac_type!='C' and Ac_type!='B' and dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Address_E like '%" + txtSearchText.Text + "%' or city_name_e like '%" + txtSearchText.Text + "%') order by Ac_Name_E ";
                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtGRADE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtNARRATION4" || hdnfClosePopup.Value == "txtSaleBillTo")
            {
                tdDate.Visible = false;
                txtSearchText.Text = searchString;
                hdnfClosePopup.Value = "txtparty";
                lblPopupHead.Text = "--Select Party--";

                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Locked=0 and Company_Code=" + Session["Company_Code"].ToString() + " " +
              "  and Ac_Code!=2   and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);

            }



            if (hdnfClosePopup.Value == "txtPurcNo")
            {
                tdDate.Visible = false;
                if (txtMILL_CODE.Text != string.Empty)
                {
                    lblPopupHead.Text = "--Select No--";
                    string qry = "";
                    if (btnSave.Text == "Save")
                    {
                        qry = "select Tender_No,Tender_DateConverted as Tender_Date,membername as MemberName,Mill_Rate,Grade,Sale_Rate,Buyer_Quantal,DESPATCH,BALANCE," +
                           "ID,trnderdetailid as tenderdetailid,grouptenderid from " +
                        " qrygroupdobalance where BALANCE!=0 and calculateStock='N' and Mill_Code=" + txtMILL_CODE.Text +
                        " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +

                        " and (Tender_No like '%" + txtSearchText.Text + "%' or  membername like  '%" + txtSearchText.Text + "%' or Buyer_Quantal like '%" + txtSearchText.Text + "%') " +
                        "  and balance!=0   order by Tender_No desc";
                    }
                    else
                    {

                        qry = "select Tender_No,Tender_DateConverted as Tender_Date,membername as MemberName,Mill_Rate,Grade,Sale_Rate,Buyer_Quantal,DESPATCH,BALANCE," +
                         "ID,trnderdetailid as tenderdetailid,grouptenderid from " +
                      " qrygroupdobalance where and calculateStock='N'  Mill_Code=" + txtMILL_CODE.Text +
                      " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +

                        " and (Tender_No like '%" + txtSearchText.Text + "%' or  membername like  '%" + txtSearchText.Text + "%' or Buyer_Quantal like '%" + txtSearchText.Text + "%') " +
                        "  and balance!=0   order by Tender_No desc";

                    }
                    this.showPopup(qry);
                }
                else
                {
                    setFocusControl(txtMILL_CODE);
                    pnlPopup.Style["display"] = "none";
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
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "kj", "javascript:SelectRow(0, {0});", true);
                        //grdPopup.Rows[0].Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});",grdPopup.Rows[0].RowIndex);
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {

                if (v == "txtGRADE")
                {
                    e.Row.Cells[0].Width = new Unit("400px");
                }
                if (v == "txtGETPASS_CODE" || v == "txtvoucher_by")
                {

                    e.Row.Cells[0].Width = new Unit("90px");
                    e.Row.Cells[1].Width = new Unit("400px");
                    e.Row.Cells[2].Width = new Unit("150px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                }


                if (v == "txtMILL_CODE")
                {
                    e.Row.Cells[0].Width = new Unit("90px");
                    e.Row.Cells[1].Width = new Unit("400px");
                    e.Row.Cells[2].Width = new Unit("200px");
                    e.Row.Cells[3].Width = new Unit("150px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                }
                if (v == "txtdoc_no" || v == "txtEditDoc_No")
                {

                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtdoc_no" || v == "txtEditDoc_No")
                {

                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(25);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(5);

                    i++;

                }


                if (v == "txtPurcNo")
                {


                    e.Row.Cells[0].ControlStyle.Width = new Unit("50px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("60px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("120px");

                    e.Row.Cells[6].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[7].ControlStyle.Width = new Unit("100px");
                    e.Row.Cells[8].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[9].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[10].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[11].ControlStyle.Width = new Unit("90px");

                    e.Row.Cells[12].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[13].ControlStyle.Width = new Unit("40px");
                    e.Row.Cells[14].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[15].ControlStyle.Width = new Unit("30px");
                    e.Row.Cells[16].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[16].ControlStyle.Width = new Unit("30px");


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
            int pgCount = 0;
            pgCount = grdPopup.PageCount;
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex, pgCount);
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

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
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
        }
        if (txtdoc_no.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [Doc_no] from " + tblHead +
                    " where dispatchId>" + Convert.ToInt32(hdnf.Value) +
                    " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnNext.Enabled = true;
                            btnLast.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnNext.Enabled = false;
                            btnLast.Enabled = false;
                        }
                    }
                }
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [Doc_no] from " + tblHead + " where dispatchId<" + Convert.ToInt32(hdnf.Value) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY Doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnPrevious.Enabled = true;
                            btnFirst.Enabled = true;
                        }
                        else
                        {
                            btnPrevious.Enabled = false;
                            btnFirst.Enabled = false;
                        }
                    }
                }
                #endregion
            }
        }

        #endregion
    }
    #endregion


    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select dispatchId from " + tblHead + "  where dispatchId=(select MIN(dispatchId) from " + tblHead + "  where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [dispatchId] from " + tblHead + "  where dispatchId<" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + 
                    " ORDER BY dispatchId DESC  ";
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
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [dispatchId] from " + tblHead + "  where dispatchId>" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + 
                    " ORDER BY dispatchId asc  ";
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
            query = "select dispatchId from " + tblHead + "  where dispatchId=(select MAX(dispatchId) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";


            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U"; 
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                this.enableDisableNavigateButtons();
                this.makeEmptyForm("S");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }

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
    #endregion

    #region grdPopup_PageIndexChanging
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {

        if (strTextBox == "txtMILL_CODE")
        {
            string millName = "";
            if (txtMILL_CODE.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtMILL_CODE.Text);
                if (a == false)
                {
                    btntxtMILL_CODE_Click(this, new EventArgs());
                }
                else
                {
                    string millshortname1 = "";
                    string gststatecode1 = "";
                    DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "  where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (ds != null)
                    {
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            millName = dt.Rows[0]["Ac_Name_E"].ToString();
                            millshortname1 = dt.Rows[0]["Short_Name"].ToString();
                            gststatecode1 = dt.Rows[0]["GSTStateCode"].ToString();
                            hdnfmc.Value = dt.Rows[0]["accoid"].ToString();
                        }
                    }

                    if (millName != string.Empty && millName != "0")
                    {
                        hdnfmillshortname.Value = millshortname1;

                        LBLMILL_NAME.Text = millName;

                        setFocusControl(btntxtPurcNo);
                    }
                    else
                    {
                        txtMILL_CODE.Text = string.Empty;
                        LBLMILL_NAME.Text = millName;
                        setFocusControl(txtMILL_CODE);
                    }
                }
            }
            else
            {
                LBLMILL_NAME.Text = "";
                setFocusControl(txtMILL_CODE);
            }
            // return;
        }

        if (strTextBox == "txtGETPASS_CODE")
        {
            string getPassName = "";
            if (txtGETPASS_CODE.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtGETPASS_CODE.Text);
                if (a == false)
                {
                    btntxtGETPASS_CODE_Click(this, new EventArgs());
                }
                else
                {
                    string aa = txtGETPASS_CODE.Text;
                    char[] ch = aa.ToCharArray();
                    for (int i = 0; i < aa.Length; i++)
                    {
                        i = 0;
                        string sub = ch[i].ToString();
                        if (sub == "0")
                        {
                            aa = aa.Remove(i, 1);
                            ch = aa.ToCharArray();
                        }
                        else
                        {
                            break;
                        }
                    }
                    txtGETPASS_CODE.Text = aa;
                    getPassName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (getPassName != string.Empty && getPassName != "0")
                    {
                        string getpasscodecitycode = clsCommon.getString("Select City_Code from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string getpasscity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster   where city_code=" + getpasscodecitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        LBLGETPASS_NAME.Text = getPassName + ", " + getpasscity;

                        hdnfgp.Value = clsCommon.getString("select isnull(accoid,0) as id from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (txtvoucher_by.Text == "2")
                        {
                            //txtGETPASS_CODE.Text = txtGETPASS_CODE.Text.Substring(1);
                            txtvoucher_by.Text = txtGETPASS_CODE.Text;
                            lblvoucherbyname.Text = LBLGETPASS_NAME.Text + ", " + getpasscity; ;
                            if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString())
                            {
                                txtSALE_RATE.Text = txtmillRate.Text;
                            }

                        }

                    }
                    else
                    {
                        txtGETPASS_CODE.Text = string.Empty;
                        LBLGETPASS_NAME.Text = getPassName;
                        setFocusControl(txtGETPASS_CODE);
                    }
                }
            }
            else
            {
                LBLGETPASS_NAME.Text = "";
                setFocusControl(txtGETPASS_CODE);
            }
        }

        if (strTextBox == "txtvoucher_by")
        {
            string vByName = "";
            string voucherbycitycode = "";
            string voucherbycity = "";
            string distance = "";
            if (txtvoucher_by.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtvoucher_by.Text);
                if (a == false)
                {
                    btntxtvoucher_by_Click(this, new EventArgs());
                }
                else
                {
                    string aa = txtvoucher_by.Text;
                    char[] ch = aa.ToCharArray();
                    for (int i = 0; i < aa.Length; i++)
                    {
                        i = 0;
                        string sub = ch[i].ToString();
                        if (sub == "0")
                        {
                            aa = aa.Remove(i, 1);
                            ch = aa.ToCharArray();
                        }
                        else
                        {
                            break;
                        }
                    }
                    txtvoucher_by.Text = aa;

                    DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (ds != null)
                    {
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            vByName = dt.Rows[0]["Ac_Name_E"].ToString();
                            voucherbycitycode = dt.Rows[0]["City_Code"].ToString();
                            voucherbycity = dt.Rows[0]["cityname"].ToString();
                            distance = dt.Rows[0]["Distance"].ToString();
                            hdnfst.Value = dt.Rows[0]["accoid"].ToString();
                        }
                    }

                    if (vByName != string.Empty && vByName != "0")
                    {
                        lblvoucherbyname.Text = vByName + ", " + voucherbycity;



                        setFocusControl(txtSaleBillTo);



                        if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString() && txtvoucher_by.Text == Session["SELF_AC"].ToString())
                        {
                            txtSALE_RATE.Text = txtmillRate.Text;
                        }
                        string id = clsCommon.getString("select accoid from " + qryAccountList + "  where Ac_Code=" + txtSaleBillTo.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnfsb.Value = id;
                        setFocusControl(txtSaleBillTo);



                    }
                    else
                    {
                        txtvoucher_by.Text = string.Empty;
                        lblvoucherbyname.Text = vByName;
                        setFocusControl(txtvoucher_by);
                    }
                }
            }
            else
            {
                lblvoucherbyname.Text = "";
                setFocusControl(txtGETPASS_CODE);
            }
        }

        if (strTextBox == "txtSaleBillTo")
        {

            if (txtSaleBillTo.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtSaleBillTo.Text);
                if (a == false)
                {
                    btntxtNARRATION4_Click(this, new EventArgs());
                }
                else
                {
                    string aa = txtSaleBillTo.Text;
                    char[] ch = aa.ToCharArray();
                    for (int i = 0; i < aa.Length; i++)
                    {
                        i = 0;
                        string sub = ch[i].ToString();
                        if (sub == "0")
                        {
                            aa = aa.Remove(i, 1);
                            ch = aa.ToCharArray();
                        }
                        else
                        {
                            break;
                        }
                    }
                    txtSaleBillTo.Text = aa;

                    string salebilltoshortname = "";
                    string gststatecode1 = string.Empty;
                    string stateName1 = "";
                     salebilltocityname = clsCommon.getString("select cityname from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtSaleBillTo.Text + "");


                    DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "   where Ac_Code="
                        + txtSaleBillTo.Text + " and Ac_type!='C' and Ac_type!='B' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (ds != null)
                    {
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            salebillname = dt.Rows[0]["Ac_Name_E"].ToString();
                            salebilltoshortname = dt.Rows[0]["Short_Name"].ToString();
                            gststatecode1 = dt.Rows[0]["GSTStateCode"].ToString();
                            stateName1 = dt.Rows[0]["State_Name"].ToString();
                            hdnfsb.Value = dt.Rows[0]["accoid"].ToString();
                            salebilltocityname = dt.Rows[0]["cityname"].ToString();

                        }
                    }

                    if (salebillname != string.Empty && salebillname != "0")
                    {

                        txtNARRATION4.Text = salebillname + ", " + salebilltocityname;

                    }
                    else
                    {
                        txtSaleBillTo.Text = string.Empty;
                        txtNARRATION4.Text = salebillname + "," + salebilltocityname;
                        setFocusControl(txtGRADE);
                    }


                }
            }
            else
            {
                txtNARRATION4.Text = "";
                setFocusControl(txtGRADE);
            }
            setFocusControl(btnSave);
            //return;
        }
    }
    #endregion
}