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

public partial class Sugar_Master_pgeGroupCreactionMaster : System.Web.UI.Page
{
    #region data section
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string qryAccountList = string.Empty;
    string millShortName = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string Action = string.Empty;
    int Doc_No = 0;
    public static SqlConnection conn = null;
    public static SqlCommand cmdm = null;
    int LV_Id = 0;
    string SystemMasterTable = string.Empty;

    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
    string cs = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            tblHead = "GroupCreactionMaster";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMasterTable = tblPrefix + "SystemMaster";
            qryAccountList = "qrymstaccountmaster";
            qryCommon = "qryGroupCreactionMaster";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            //hdnfcommisiontype.Value = Request.QueryString["Tran_Type"];
            //drpVouchertype.SelectedValue = hdnfcommisiontype.Value;
            if (!Page.IsPostBack)
            {
                hdnfyearcode.Value = Session["year"].ToString();
                hdnfcompanycode.Value = Session["Company_Code"].ToString();
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["autoid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                        //setFocusControl(btnEdit);
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
                        setFocusControl(txtGroupName);
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
                string qry = "select * from " + qryCommon + " where Doc_No='" + txtEditDoc_No.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;

            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                obj.code = "Doc_No";

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
                                    txtdoc_no.Text = ds.Tables[0].Rows[0][0].ToString();
                                    txtdoc_no.Enabled = false;
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
                lblMsg.Text = string.Empty;

                #region set Business logic
                lblmember1.Text = string.Empty;
                lblmember2.Text = string.Empty;
                lblmember3.Text = string.Empty;
                lblmember4.Text = string.Empty;
                lblmember5.Text = string.Empty;
                lblmember6.Text = string.Empty;
                lblmember7.Text = string.Empty;
                txtEditDoc_No.Enabled = true;
                btnmember1.Enabled = false;
                btnmember2.Enabled = false;
                btnmember3.Enabled = false;
                btnmember4.Enabled = false;
                btnmember5.Enabled = false;
                btnmember6.Enabled = false;
                btnmember7.Enabled = false; 

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
                lblMsg.Text = "";
                txtEditDoc_No.Enabled = false;
                btntxtDOC_NO.Enabled = false;
                #region set Business logic for save
                lblmember1.Text = string.Empty;
                lblmember2.Text = string.Empty;
                lblmember3.Text = string.Empty;
                lblmember4.Text = string.Empty;
                lblmember5.Text = string.Empty;
                lblmember6.Text = string.Empty;
                lblmember7.Text = string.Empty;
                btnmember1.Enabled = true;
                btnmember2.Enabled = true;
                btnmember3.Enabled = true;
                btnmember4.Enabled = true;
                btnmember5.Enabled = true;
                btnmember6.Enabled = true;
                btnmember7.Enabled = true; 

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

                #region set Business logic for save

                btnmember1.Enabled = false;
                btnmember2.Enabled = false;
                btnmember3.Enabled = false;
                btnmember4.Enabled = false;
                btnmember5.Enabled = false;
                btnmember6.Enabled = false;
                btnmember7.Enabled = false; 
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
                lblMsg.Text = string.Empty;
                #region logic

                btnmember1.Enabled = true;
                btnmember2.Enabled = true;
                btnmember3.Enabled = true;
                btnmember4.Enabled = true;
                btnmember5.Enabled = true;
                btnmember6.Enabled = true;
                btnmember7.Enabled = true; 
                #endregion
                txtEditDoc_No.Enabled = false;
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

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.NextNumber();
        setFocusControl(txtGroupName);
        //txtGSTRateCode.Text = "2";
        string gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        pnlPopupDetails.Style["display"] = "none";

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

    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {

                string tenderheadaccode = string.Empty;
                string concat = string.Empty;
                tenderheadaccode = clsCommon.getString("select top(1) Tender_No from GroupTenderPurchase where Group_Account=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (tenderheadaccode != string.Empty && tenderheadaccode != "0")
                {
                    concat = concat + "Group Tender DocNo: " + tenderheadaccode + "";
                }
                if ( tenderheadaccode == "0")
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "system", "javascript:DeleteConform();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('This Record Use In " + concat + "')", true);
                    return;
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
        hdnf.Value = Request.QueryString["autoid"];

        if (hdnf.Value == "0" || hdnf.Value == "")
        {
            int maxno = Convert.ToInt32(clsCommon.getString("select isnull(max(autoid),0) as autoid from " + tblHead + " "));

            hdnf.Value = Convert.ToString(maxno);
        }
        Response.Redirect("pgeGroupCreactionMaster.aspx?autoid=" + hdnf.Value + "&Action=" + 1);
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
                        string comcode = Session["Company_Code"].ToString();
                        if (hdnfcompanycode.Value != comcode)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records company code & current company code is not same !')", true);
                            return false;
                        }
                         
                        hdnfcompanycode.Value = dt.Rows[0]["Company_Code"].ToString();
                        txtdoc_no.Text = dt.Rows[0]["Doc_No"].ToString();
                        hdnflvdoc.Value = txtdoc_no.Text;
                        hdnflvid.Value = dt.Rows[0]["autoid"].ToString();
                        hdnf.Value = dt.Rows[0]["autoid"].ToString();
                        txtmember1.Text = dt.Rows[0]["member1"].ToString();
                        txtmember2.Text = dt.Rows[0]["member2"].ToString();
                        txtmember3.Text = dt.Rows[0]["member3"].ToString();
                        txtmember4.Text = dt.Rows[0]["member4"].ToString();
                        txtmember5.Text = dt.Rows[0]["member5"].ToString();
                        txtmember6.Text = dt.Rows[0]["member6"].ToString();
                        txtmember7.Text = dt.Rows[0]["member7"].ToString();
                        lblmember1.Text = dt.Rows[0]["MemberName1"].ToString();
                        lblmember2.Text = dt.Rows[0]["MemberName2"].ToString();
                        lblmember3.Text = dt.Rows[0]["MemberName3"].ToString();
                        lblmember4.Text = dt.Rows[0]["MemberName4"].ToString();
                        lblmember5.Text = dt.Rows[0]["MemberName5"].ToString();
                        lblmember6.Text = dt.Rows[0]["MemberName6"].ToString();
                        lblmember7.Text = dt.Rows[0]["MemberName7"].ToString();
                        hdnfmember1.Value = dt.Rows[0]["id1"].ToString();
                        hdnfmember2.Value = dt.Rows[0]["id2"].ToString();
                        hdnfmember3.Value = dt.Rows[0]["id3"].ToString();
                        hdnfmember4.Value = dt.Rows[0]["id4"].ToString();
                        hdnfmember5.Value = dt.Rows[0]["id5"].ToString();
                        hdnfmember6.Value = dt.Rows[0]["id6"].ToString();
                        hdnfmember7.Value = dt.Rows[0]["id7"].ToString();
                        txtGroupName.Text = dt.Rows[0]["GroupName"].ToString(); 
                         
                        hdnf.Value = txtdoc_no.Text;
                        recordExist = true;
                        lblMsg.Text = "";
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtmember1" || v == "txtmember2" || v == "txtmember3" || v == "txtmember4" || v == "txtmember5" || v == "txtmember6" || v == "txtmember7")
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


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where  autoid=" + hdnf.Value + "";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
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



    #region [txtmember1_TextChanged]
    protected void txtmember1_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember1";
        searchString = txtmember1.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember1_Click]
    protected void btnmember1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 

    #region [txtmember2_TextChanged]
    protected void txtmember2_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember2";
        searchString = txtmember2.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember2_Click]
    protected void btnmember2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember2";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 

    #region [txtmember3_TextChanged]
    protected void txtmember3_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember3";
        searchString = txtmember3.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember3_Click]
    protected void btnmember3_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember3";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 

    #region [txtmember4_TextChanged]
    protected void txtmember4_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember4";
        searchString = txtmember4.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember4_Click]
    protected void btnmember4_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember4";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 

    #region [txtmember5_TextChanged]
    protected void txtmember5_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember5";
        searchString = txtmember5.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember5_Click]
    protected void btnmember5_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember5";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 

    #region [txtmember6_TextChanged]
    protected void txtmember6_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember6";
        searchString = txtmember6.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember6_Click]
    protected void btnmember6_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember6";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion 


    #region [txtmember7_TextChanged]
    protected void txtmember7_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtmember7";
        searchString = txtmember7.Text;
        csCalculations();
    }
    #endregion

    #region [btnmember7_Click]
    protected void btnmember7_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmember7";
            btnSearch_Click(sender, e);
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
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
                searchString = txtSearchText.Text;
            }

            if (hdnfClosePopup.Value == "txtmember1")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember1.Text; 

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember2")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember2.Text; 

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember3")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember3.Text;

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember4")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember4.Text;

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember5")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember5.Text;

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember6")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember6.Text;

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
            }

            if (hdnfClosePopup.Value == "txtmember7")
            {

                lblPopupHead.Text = "--Select Party--";
                txtSearchText.Text = txtmember7.Text;

                string qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E desc";

                this.showPopupAccountMaster(qry);
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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
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

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtmember1")
            {
                string partyName = string.Empty;
                if (txtmember1.Text != string.Empty)
                {
                    searchString = txtmember1.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember1_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember1.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember1.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember1.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember1.Text = partyName;
                            setFocusControl(txtmember2);
                        }
                        else
                        {
                            lblmember1.Text = string.Empty;
                            txtmember1.Text = string.Empty;
                            setFocusControl(txtmember1);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember2);
                }
            }

            if (strTextBox == "txtmember2")
            {
                string partyName = string.Empty;
                if (txtmember2.Text != string.Empty)
                {
                    searchString = txtmember2.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember2_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember2.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember2.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember2.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember2.Text = partyName;
                            setFocusControl(txtmember3);
                        }
                        else
                        {
                            lblmember2.Text = string.Empty;
                            txtmember2.Text = string.Empty;
                            setFocusControl(txtmember2);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember3);
                }
            }

            if (strTextBox == "txtmember3")
            {
                string partyName = string.Empty;
                if (txtmember3.Text != string.Empty)
                {
                    searchString = txtmember3.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember3_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember3.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember3.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember3.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember3.Text = partyName;
                            setFocusControl(txtmember4);
                        }
                        else
                        {
                            lblmember3.Text = string.Empty;
                            txtmember3.Text = string.Empty;
                            setFocusControl(txtmember3);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember4);
                }
            }

            if (strTextBox == "txtmember4")
            {
                string partyName = string.Empty;
                if (txtmember4.Text != string.Empty)
                {
                    searchString = txtmember4.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember4_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember4.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember4.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember4.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember4.Text = partyName;
                            setFocusControl(txtmember5);
                        }
                        else
                        {
                            lblmember4.Text = string.Empty;
                            txtmember4.Text = string.Empty;
                            setFocusControl(txtmember4);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember5);
                }
            }

            if (strTextBox == "txtmember5")
            {
                string partyName = string.Empty;
                if (txtmember5.Text != string.Empty)
                {
                    searchString = txtmember5.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember5_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember5.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember5.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember5.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember5.Text = partyName;
                            setFocusControl(txtmember6);
                        }
                        else
                        {
                            lblmember5.Text = string.Empty;
                            txtmember5.Text = string.Empty;
                            setFocusControl(txtmember5);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember6);
                }
            }


            if (strTextBox == "txtmember6")
            {
                string partyName = string.Empty;
                if (txtmember6.Text != string.Empty)
                {
                    searchString = txtmember6.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember6_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember6.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember6.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember6.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember6.Text = partyName;
                            setFocusControl(txtmember7);
                        }
                        else
                        {
                            lblmember6.Text = string.Empty;
                            txtmember6.Text = string.Empty;
                            setFocusControl(txtmember6);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtmember7);
                }
            }


            if (strTextBox == "txtmember7")
            {
                string partyName = string.Empty;
                if (txtmember7.Text != string.Empty)
                {
                    searchString = txtmember7.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btnmember7_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        hdnfmember7.Value = clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtmember7.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        qry = "select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtmember7.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblmember7.Text = partyName;
                            setFocusControl(btnSave);
                        }
                        else
                        {
                            lblmember7.Text = string.Empty;
                            txtmember7.Text = string.Empty;
                            setFocusControl(txtmember7);
                        }
                    }
                }
                else
                {
                    setFocusControl(btnSave);
                }
            }

            #region [Calculation Part]
             
        }
        catch
        {
        }
    }
            #endregion
    #endregion
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    } 

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(Doc_No) as Doc_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(Doc_No) as Doc_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            } 
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);

        }
    }
    #endregion
}