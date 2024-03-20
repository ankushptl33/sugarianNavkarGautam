using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Net.Http;
using ApiModels.Models;
using Newtonsoft.Json;
using System.Threading;

public partial class Sugar_pgePayments : System.Web.UI.Page
{
    string paymentUrl = "http://localhost:8085/api/payment/makepayment";
    string statusUrl = "http://localhost:8085/api/payment/checkstatus";
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string systemMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string voucherTable = string.Empty;
    string qryVoucherList = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;
    #endregion

    #region head Declare
    Int32 DOC_NO = 0;
    string DOC_DATE = string.Empty;
    int cmpcashac = 0;
    Int32 cashBank = 0;
    string retValue = string.Empty;
    string strRev = string.Empty;
    int Company_Code = 0;
    int Year_Code = 0;
    int year_Code = 0;
    int Branch_Code = 0;
    string userinfo = string.Empty;
    string drpFilterValue = string.Empty;
    string drcr = string.Empty;
    string drcr0 = string.Empty;
    int? mr_no = 0;
    int? bc = 0;
    int? ac = 0;
    #endregion

    #region detail
    int Detail_Id = 0;
    int Bill_No = 0;
    string BillTran_Type = string.Empty;
    string Bill_Date = string.Empty;
    int PartyCode = 0;
    int UnitCode = 0;
    int MillCode = 0;
    double Quintal = 0.00;
    double BillAmt = 0.00;
    double PaidAmt = 0.00;
    double Balance = 0.00;
    double Amount = 0.00;
    double Adj_Amount = 0.00;
    double grdUsedAmt = 0.00;
    double grdadjustamt = 0.00;
    string Narration = string.Empty;
    string EntryYear = string.Empty;
    string EntryCompany = string.Empty;
    int billAuto_id = 0;

    int mrd_no = 0;
    int pc = 0;
    int mc = 0;
    int uc = 0;
    Int32 vocherNo = 0;
    string voucherType = string.Empty;
    Int32 acCode = 0;
    string lorry = string.Empty;
    // Int32 Unit_Code = 0;

    double EnteredAmt = 0.00;
    double finalAmt = 0.00;
    #endregion
    int flag = 0;
    int nn = 0;
    #region
    string Action = string.Empty;

    string concatid = string.Empty;
    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    string Head_Fields = string.Empty;
    string Head_Values = string.Empty;
    string Purchase_Delete = string.Empty;
    string Sale_Delete = string.Empty;
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    int count = 0;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "PaymentHead";
            tblDetails = "PaymentDetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = "qrymultiplereceipthead";
            qryAccountList = "qrymstaccountmaster";
            cityMasterTable = tblPrefix + "CityMaster";
            systemMasterTable = tblPrefix + "SystemMaster";
            voucherTable = tblPrefix + "Voucher";
            qryVoucherList = tblPrefix + "qryVoucherList";
            user = Session["user"].ToString();
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";

            //Head_Fields = new StringBuilder();
            //Head_Values = new StringBuilder();
            Head_Update = new StringBuilder();
            Detail_Update = new StringBuilder();
            Detail_Delete = new StringBuilder();
            Detail_Fields = new StringBuilder();
            Detail_Values = new StringBuilder();

            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            if (!Page.IsPostBack)
            {
                hdnfyearcode.Value = Session["year"].ToString();
                hdnfcompanycode.Value = Session["Company_Code"].ToString();
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    trntype = Request.QueryString["tran_type"];
                    if (Action == "1")
                    {

                        hdnf.Value = Request.QueryString["tranid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                    }
                    else
                    {
                        drpPaymentFor.SelectedValue = trntype;
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.nextNumber();
                        setFocusControl(txtdoc_date);
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

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "and Tran_Type='" + trntype + "'";
                obj.code = "doc_no";
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
                txtEditDoc_No.Enabled = true;
                grdDetail.DataSource = null; grdDetail.DataBind();
                pnlPopup.Style["display"] = "none";
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnTdsAccount.Enabled = false;
                btnGetvouchers.Enabled = false;
                lblMsg.Text = string.Empty;
                txtBalance.Enabled = false;
                calenderExtenderDate.Enabled = false;
                //btnDelete.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                // drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = false;
                lblCashBank.Text = string.Empty;
                lblTdsAcName.Text = string.Empty;
                txtTotal.Text = string.Empty;
                //btnDelete.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                txtBalance.Enabled = false;
                // making make payment button enable
                pnlgrdDetail.Enabled = true;
                #region set Business logic for Bank
                txtCashBank.Enabled = false;
                btntxtCashBank.Enabled = false;
                txtCashBank.Text = Session["BankPaymentAc"].ToString();
                lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                hdnfbankid.Value = Session["bpid"].ToString();
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
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                txtEditDoc_No.Enabled = false;
                #region set Business logic for save
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                pnlgrdDetail.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
                btntxtCashBank.Enabled = true;
                btntxtACCode.Enabled = true;
                btnTdsAccount.Enabled = true;
                btnGetvouchers.Enabled = true;
                // drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                // trntype = drpTrnType.SelectedValue;
                txtBalance.Enabled = false;
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                lblACName.Text = string.Empty;
                lblTdsAcName.Text = string.Empty;
                //if (trntype == "BP" || trntype == "BR")
                //{
                //    txtCashBank.Enabled = true;
                //    btntxtCashBank.Enabled = true;
                //}
                //else
                //{
                //    txtCashBank.Enabled = false;
                //    btntxtCashBank.Enabled = false;
                //}
                txtdoc_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                #endregion
                #region set Business logic for Bank
                txtCashBank.Enabled = false;
                btntxtCashBank.Enabled = false;
                txtCashBank.Text = Session["BankPaymentAc"].ToString();
                lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                hdnfbankid.Value = Session["bpid"].ToString();
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                calenderExtenderDate.Enabled = false;
                pnlgrdDetail.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnTdsAccount.Enabled = false;
                btnAdd.Enabled = true;
                // drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = false;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnGetvouchers.Enabled = false;
                txtBalance.Enabled = false;
                #region set Business logic for Bank
                txtCashBank.Enabled = false;
                btntxtCashBank.Enabled = false;
                txtCashBank.Text = Session["BankPaymentAc"].ToString();
                lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                hdnfbankid.Value = Session["bpid"].ToString();
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                pnlgrdDetail.Enabled = true;
                txtCashBank.Enabled = true;
                btntxtACCode.Enabled = true;
                btntxtCashBank.Enabled = true;
                btnTdsAccount.Enabled = true;
                txtEditDoc_No.Enabled = false;
                // drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                txtBalance.Enabled = false;
                txtACCode.Enabled = true;
                txtdoc_no.Enabled = false;
                btnGetvouchers.Enabled = false;
                #region set Business logic for Bank
                txtCashBank.Enabled = false;
                btntxtCashBank.Enabled = false;
                txtCashBank.Text = Session["BankPaymentAc"].ToString();
                lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                hdnfbankid.Value = Session["bpid"].ToString();
                #endregion
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
        string max = clsCommon.getString("select isnull(max(id),0) as id from PaymentHead where Company_Code='" + Session["Company_Code"].ToString() + "' " +
            " and Year_Code='" + Session["year"].ToString() + "' and tran_type='AF'");
        hdnf.Value = max;
        trntype = "AF";
        drpPaymentFor.Text = trntype;
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.showLastRecord();
    }
    #endregion


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where id=" + hdnf.Value + " and tran_type='" + trntype + "' ";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region [DeleteDetailsRow]
    private void DeleteDetailsRow(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;
            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["ID"].ToString());
                string IDExisting = clsCommon.getString("select detail_id from " + tblHead + " where detail_id=" + ID + " and Tran_Type='" + trntype + "' and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "N";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "A";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }
                }
                ViewState["currentTable"] = dt;
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
        int i = 0;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        e.Row.Cells[0].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[1].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[2].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[3].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[4].ControlStyle.Width = new Unit("10px");
        e.Row.Cells[5].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[6].ControlStyle.Width = new Unit("250px");
        e.Row.Cells[7].ControlStyle.Width = new Unit("200px");
        e.Row.Cells[8].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[9].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[10].ControlStyle.Width = new Unit("50px");

        e.Row.Cells[11].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[12].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[13].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[14].ControlStyle.Width = new Unit("300px");
        //e.Row.Cells[15].ControlStyle.Width = new Unit("300px");

        //e.Row.Cells[16].ControlStyle.Width = new Unit("130px");
        //e.Row.Cells[17].ControlStyle.Width = new Unit("300px");
        //e.Row.Cells[18].ControlStyle.Width = new Unit("230px");
        //e.Row.Cells[19].ControlStyle.Width = new Unit("130px");
        //e.Row.Cells[20].ControlStyle.Width = new Unit("130px");
        //e.Row.Cells[21].ControlStyle.Width = new Unit("130px");

        //e.Row.Cells[22].ControlStyle.Width = new Unit("15px");
        //e.Row.Cells[23].ControlStyle.Width = new Unit("15px");
        //e.Row.Cells[24].ControlStyle.Width = new Unit("15px");
        //e.Row.Cells[25].ControlStyle.Width = new Unit("15px");
        //e.Row.Cells[18].Visible = false;
        //e.Row.Cells[19].Visible = false;
        //e.Row.Cells[20].Visible = false;
        if (drpPaymentFor.SelectedValue == "TP")
        {
            //e.Row.Cells[6].Visible = false;
            //e.Row.Cells[7].Visible = false;
        }
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
        foreach (TableCell cell in e.Row.Cells)
        {
            string s = cell.Text.ToString();
            if (cell.Text.Length > 22)
            {
                cell.Text = cell.Text.Substring(0, 22) + "..";
                cell.ToolTip = s;
            }
        }

        if (drpPaymentFor.SelectedValue == "MP")
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[7].Visible = false;
        }
        else
        {
            e.Row.Cells[14].Visible = false;
        }

        string v = ViewState["mode"].ToString();
        if (v != "I")
        {
            pnlgrdDetail.Enabled = true;
            e.Row.Cells[14].Enabled = true;
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
                if (v == "txtCashBank")
                {
                    e.Row.Cells[0].Width = new Unit("20px");
                    e.Row.Cells[1].Width = new Unit("500px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtACCode")
                {
                    e.Row.Cells[0].Width = new Unit("20px");
                    e.Row.Cells[1].Width = new Unit("500px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
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



    #region [txtdoc_date_TextChanged]
    protected void txtdoc_date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_date.Text;
        strTextBox = "txtdoc_date";
        csCalculations();
    }
    #endregion

    #region [txtACCode_TextChanged]
    protected void txtACCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtACCode.Text;
        strTextBox = "txtACCode";
        csCalculations();
    }
    #endregion

    #region [btntxtACCode_Click]
    protected void btntxtACCode_Click(object sender, EventArgs e)
    {
        try
        {
            nn = 1;
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtACCode";
            btnSearch_Click(sender, e);

        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtVoucherNo_Click]
    protected void btntxtVoucherNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtVoucherNo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtCashBank_Click]
    protected void btntxtCashBank_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == string.Empty)
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtCashBank";
                btnSearch_Click(sender, e);
            }
            else
            {
                if (nn == 0)
                {
                    hdnfClosePopup.Value = string.Empty;
                    setFocusControl(txtACCode);
                }
                else
                {
                    hdnfClosePopup.Value = string.Empty;
                    setFocusControl(txtAmount);
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCashBank_TextChanged]
    protected void txtCashBank_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCashBank.Text;
        strTextBox = "txtCashBank";
        csCalculations();
    }
    #endregion

    #region [btntxtnarration_Click]
    protected void btntxtnarration_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtnarration";
            btnSearch_Click(sender, e);
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
            if (hdnfClosePopup.Value == "txtACCode")
            {
                setFocusControl(txtACCode);
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

        try
        {
            string tranType = drpPaymentFor.SelectedValue;
            double docNo = Convert.ToInt64(txtdoc_no.Text);
            double bankCode = Convert.ToInt64(txtCashBank.Text);
            double paymentTo = Convert.ToInt64(txtACCode.Text);
            double tdsAccount = Convert.ToInt64(txtTdsAccount.Text);
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            decimal tdsPercent = txtTdsPercent.Text == string.Empty ? 0 : Convert.ToDecimal(txtTdsPercent.Text);
            decimal tdsAmount = txtTdsAmount.Text == string.Empty ? 0 : Convert.ToDecimal(txtTdsAmount.Text);
            decimal netPayable = amount - tdsAmount;
            string companyCode = Session["Company_Code"].ToString();
            string yearCode = Session["year"].ToString();
            DateTime date = Convert.ToDateTime(txtdoc_date.Text);
            string formattedDate = date.ToString("yyyy-MM-dd");
            long head_id = lblmr_no.Text != string.Empty ? Convert.ToInt64(lblmr_no.Text) : 0;

            if (btnSave.Text == "Update")
            {
                string query = string.Format("UPDATE [dbo].[PaymentHead] SET " +
                             "[Tran_Type] = '{0}', " +
                             "[bank_code] = {1}, " +
                             "[payment_to] = {2}, " +
                             "[tds_ac] = {3}, " +
                             "[amount] = {4}, " +
                             "[tds_percent] = {5}, " +
                             "[tds_amount] = {6}, " +
                             "[net_payable] = {7}, " +
                             "[company_code] = {8}, " +
                             "[year_code] = {9}, " +
                             "[date] = '{10}' " +
                             "WHERE [id] = {11};",
                             tranType, bankCode, paymentTo, tdsAccount, amount, tdsPercent, tdsAmount, netPayable, companyCode, yearCode, formattedDate, lblmr_no.Text);

                var ds = clsDAL.SimpleQuery(query);

                var queryBuilder = new StringBuilder();
                var multipleReceiptQueryBuilderUpdate = new StringBuilder();
                var deleteDetailEntries = new StringBuilder();

                for (int k = 0; k < grdDetail.Rows.Count; k++)
                {
                    TextBox txtgrdAmount = (TextBox)grdDetail.Rows[k].Cells[11].FindControl("txtgrdAmount");
                    double amt = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
                    double detailId = Convert.ToDouble(grdDetail.Rows[k].Cells[0].Text);
                    Label lblTenderId = (Label)grdDetail.Rows[k].Cells[13].FindControl("lblTenderId");
                    Int64 tenderid = Convert.ToInt64(lblTenderId.Text);

                    if (amt > 0)
                    {
                        double docNumber = Convert.ToDouble(grdDetail.Rows[k].Cells[1].Text);
                        double doid = Convert.ToDouble(grdDetail.Rows[k].Cells[2].Text);
                        TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[k].Cells[12].FindControl("txtgrdAdjustedAmount");
                        TextBox narration = (TextBox)grdDetail.Rows[k].Cells[13].FindControl("txtNarration");
                        
                        double adj = txtgrdAdjustedAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount.Text) : 0.00;

                        if (detailId == 0)
                        {
                            queryBuilder.AppendLine("INSERT INTO [dbo].[PaymentDetail] " +
                           "([head_id], [doc_no], [doid], [amount], [narration], [adjusted_amount], [company_code], [year_code]) " +
                           "VALUES " +
                           "(" + head_id + ", " + docNumber + ", " + doid + ", " + amt + ", '" + narration.Text + "', " + adj + ", " + companyCode + ", " + yearCode + ");");
                        }
                        else
                        {
                            queryBuilder.AppendLine("UPDATE [dbo].[PaymentDetail] SET " +
                           "[amount]=" + amt + ", [narration]='" + narration.Text + "', [adjusted_amount]=" + adj + " WHERE head_id=" + lblmr_no.Text + " and Detail_Id=" + detailId + "");

                            if (tranType == "AF")
                            {
                                multipleReceiptQueryBuilderUpdate.Append("Update multiple_receipt_detail SET Value= " + amt + ",  Adj_Value=" + adj + ", Narration='" + narration.Text + "' where Tran_Type='AF' and detail_Id=" + detailId + " and mr_no=" + head_id + "");
                            }
                            else
                            {
                                multipleReceiptQueryBuilderUpdate.Append("Update nt_1_utrdetail SET amount= " + amt + ",  Adj_Value=" + adj + " where lot_no=" + docNumber + " and bpid=" + detailId + " and ln=" + tenderid + "");
                            }
                        }
                    }
                    else
                    {
                        if (tranType == "AF")
                        {
                            deleteDetailEntries.Append("delete from PaymentDetail where Tran_Type='AF' and detail_Id=" + detailId + "");
                            deleteDetailEntries.Append("delete from multiple_receipt_detail where Tran_Type='AF' and detail_Id=" + detailId + " and mr_no=" + head_id + "");
                        }
                        else
                        {
                            deleteDetailEntries.Append("delete from PaymentDetail where Tran_Type='MP' and detail_Id=" + detailId + "");
                            deleteDetailEntries.Append("delete from nt_1_utrdetail SET where bpid=" + detailId + " and ln=" + tenderid + "");
                        }
                    }
                }

                string query2 = queryBuilder.ToString();

                var ds3 = clsDAL.SimpleQuery(query2);

                // update in multiple receipt detail table

                string query3 = multipleReceiptQueryBuilderUpdate.ToString();

                clsDAL.SimpleQuery(query3);
                //delete zero balance 
                string query4 = deleteDetailEntries.ToString();
                var ds4 = clsDAL.SimpleQuery(query4);
            }
            else
            {
                string insertHeadQuery = string.Format("INSERT INTO [dbo].[PaymentHead] " +
                        "([Tran_Type], [doc_no], [bank_code], [payment_to], [tds_ac], [amount], [tds_percent], [tds_amount], [net_payable], [company_code], [year_code], [date]) " +
                        "VALUES " +
                        "('" + tranType + "', " + docNo + ", " + bankCode + ", " + paymentTo + ", " + tdsAccount + ", " + amount + ", " + tdsPercent + ", " + tdsAmount + ", " + netPayable + ", " + companyCode + ", " + yearCode + ", '" + formattedDate + "');SELECT SCOPE_IDENTITY();");


                head_id = clsDAL.ExecuteScalar(insertHeadQuery);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "system", "javascript:validation();", true);

                var queryBuilder = new StringBuilder();
                var multipleReceiptQueryBuilder = new StringBuilder();
                string qrytoinsert = "";
                for (int k = 0; k < grdDetail.Rows.Count; k++)
                {
                    TextBox txtgrdAmount = (TextBox)grdDetail.Rows[k].Cells[11].FindControl("txtgrdAmount");
                    double amt = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
                    if (amt > 0)
                    {
                        double docNumber = Convert.ToDouble(grdDetail.Rows[k].Cells[1].Text);
                        double doid = Convert.ToDouble(grdDetail.Rows[k].Cells[2].Text);
                        TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[k].Cells[12].FindControl("txtgrdAdjustedAmount");
                        TextBox narration = (TextBox)grdDetail.Rows[k].Cells[13].FindControl("txtNarration");
                        Label lblTenderId = (Label)grdDetail.Rows[k].Cells[13].FindControl("lblTenderId");
                        Int64 tenderid = Convert.ToInt64(lblTenderId.Text);

                        double adj = txtgrdAdjustedAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount.Text) : 0.00;

                        qrytoinsert = "INSERT INTO [dbo].[PaymentDetail] " +
                           "([head_id], [doc_no], [doid], [amount], [narration], [adjusted_amount], [company_code], [year_code],[tran_type]) " +
                           "VALUES " +
                           "(" + head_id + ", " + docNumber + ", " + doid + ", " + amt + ", '" + narration.Text + "', " + adj + ", " + companyCode + ", " + yearCode + ", '" + tranType + "')";

                        long newDetailId = clsDAL.ExecuteScalar(qrytoinsert + ";SELECT SCOPE_IDENTITY();");

                        if (tranType == "AF")
                        {
                            string paymenttoaccoid = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");

                            multipleReceiptQueryBuilder.Append("insert into multiple_receipt_detail " +
                                " (Tran_Type, Bill_Tran_Type, Bill_Tran_Date, pc, Party_Code, Value, Adj_Value, LorryNo,Bill_Auto_Id, Narration,detail_Id, mr_no, Company_Code,Year_Code) " +
                                " values('" + tranType + "', 'DO', '" + formattedDate + "', " + paymenttoaccoid + ", " + paymentTo + ", " + amt + ", " + adj + ", '', " + doid + ", '" + narration.Text + "'," + newDetailId + "," + head_id + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + "," + Convert.ToInt32(Session["year"].ToString()) + ")");
                        }
                        else
                        {
                            multipleReceiptQueryBuilder.Append("insert into nt_1_utrdetail (doc_no,lot_no,amount,Adjusted_Amt, LTNo,bpid,Company_Code,Year_Code, ln, lotCompany_Code, lotYear_Code) values(" + head_id + "," + docNumber + ", " + amt
                                + ", " + adj + ", 0, " + newDetailId + ", " + Convert.ToInt32(Session["Company_Code"].ToString()) + "," + Convert.ToInt32(Session["year"].ToString()) + ", " + tenderid + ", " + Convert.ToInt32(Session["Company_Code"].ToString()) + "," + Convert.ToInt32(Session["year"].ToString()) + ")");
                        }
                    }
                }

                //string query = queryBuilder.ToString();

                //DataSet ds2 = clsDAL.SimpleQuery(query);
                // insert in multiple receipt detail table
                //if (tranType == "AF")
                //{
                var qryInsertMultipleReceipt = multipleReceiptQueryBuilder.ToString();
                clsDAL.SimpleQuery(qryInsertMultipleReceipt);
                //}
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "EntrySuccess('" + btnSave.Text + "'," + docNo + "," + head_id + ")", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "EntryFailure()", true);
        }
    }

    #endregion

    protected void txtgrdAmount_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtgrdAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtgrdAdjustedAmount_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtgrdAdjustedAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtNarration";
        int index = RowIndex(sender);
        // this.calculation(index);
    }

    protected void calculation(int index)
    {
        double Billamount = Convert.ToDouble(grdDetail.Rows[index].Cells[10].Text);

        double Balance = Convert.ToDouble(grdDetail.Rows[index].Cells[10].Text);
        TextBox txtgrdAmount = (TextBox)grdDetail.Rows[index].Cells[11].FindControl("txtgrdAmount");
        TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[index].Cells[12].FindControl("txtgrdAdjustedAmount");
        //TextBox txtOnAc = (TextBox)grdDetail.Rows[index].Cells[17].FindControl("txtOnAc");
        TextBox txtNarration = (TextBox)grdDetail.Rows[index].Cells[13].FindControl("txtNarration");
        double grdamount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
        if (grdamount > Billamount)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('The amount deposited against this invoice is greater than the invoice !');", true);
        }

        double enterAmt = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0.00;
        if (enterAmt > 0)
        {
            CalculateBalance();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Enter Amount!');", true);
            setFocusControl(txtAmount);
            txtgrdAmount.Text = "";
            return;
        }


        if (strTextBox == "txtgrdAmount")
        {
            setFocusControl(txtgrdAdjustedAmount);
        }

        if (strTextBox == "txtgrdAdjustedAmount")
        {
            //setFocusControl(txtOnAc);
        }
        if (strTextBox == "txtOnAc")
        {
            //setFocusControl(txtOnAc);
        }

        if (strTextBox == "txtNarration")
        {
            if (index < grdDetail.Rows.Count - 1)
            {
                TextBox txtgrdAmount1 = (TextBox)grdDetail.Rows[index + 1].Cells[13].FindControl("txtgrdAmount");
                setFocusControl(txtgrdAmount1);
            }
            else
            {
                setFocusControl(btnSave);
            }
        }
    }

    private void CalculateBalance()
    {
        double amt1 = 0.00;
        double tdsAmount;
        double.TryParse(txtTdsAmount.Text, out tdsAmount);
        double adj1 = 0.00;
        for (int k = 0; k < grdDetail.Rows.Count; k++)
        {
            TextBox txtgrdAmount2 = (TextBox)grdDetail.Rows[k].Cells[11].FindControl("txtgrdAmount");
            double amt2 = txtgrdAmount2.Text != string.Empty ? Convert.ToDouble(txtgrdAmount2.Text) : 0.00;
            amt1 += amt2;

            TextBox txtgrdAdjustedAmount1 = (TextBox)grdDetail.Rows[k].Cells[12].FindControl("txtgrdAdjustedAmount");
            double adj2 = txtgrdAdjustedAmount1.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount1.Text) : 0.00;
            adj1 += adj2;
        }
        //  txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - amt1);
        txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - (amt1 + tdsAmount + adj1));

        double bal = Convert.ToDouble(txtBalance.Text);
        txtBalance.Text = bal.ToString();
    }

    private static int RowIndex(object sender)
    {
        GridViewRow grow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = grow.RowIndex;
        return index;
    }



    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtdoc_no")
            {

            }
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtACCode);
                    }
                    else
                    {
                        txtdoc_date.Text = string.Empty;
                        setFocusControl(txtdoc_date);
                    }
                }
                else
                {
                    setFocusControl(txtdoc_date);
                }
            }
            if (strTextBox == "txtACCode")
            {
                if (txtACCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtACCode.Text);
                    if (a == false)
                    {
                        btntxtACCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = "";
                        nn = 1;
                        if (drpPaymentFor.SelectedValue == "T")
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type='T' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type!='B' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        hdnfacid.Value = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        if (str != string.Empty)
                        {
                            lblACName.Text = str;
                            setFocusControl(txtTdsAccount);
                        }
                        else
                        {
                            lblACName.Text = string.Empty;
                            txtACCode.Text = string.Empty;
                            setFocusControl(txtACCode);

                        }
                    }
                }
                else
                {
                    lblACName.Text = string.Empty;
                    txtACCode.Text = string.Empty;
                    setFocusControl(txtACCode);

                }
            }
            if (strTextBox == "txtTdsAccount")
            {
                if (txtTdsAccount.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtTdsAccount.Text);
                    if (a == false)
                    {
                        btnTdsAccount_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = "";
                        nn = 1;
                        str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtTdsAccount.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //if (drpPaymentFor.SelectedValue == "T")
                        //{
                        //    str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type='T' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //}
                        //else
                        //{
                        //    str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type!='B' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //}
                        hdnfacid.Value = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + txtTdsAccount.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        if (str != string.Empty)
                        {
                            lblTdsAcName.Text = str;
                            setFocusControl(txtAmount);
                        }
                        else
                        {
                            lblTdsAcName.Text = string.Empty;
                            txtTdsAccount.Text = string.Empty;
                            setFocusControl(txtTdsAccount);
                        }
                    }
                }
                else
                {
                    lblTdsAcName.Text = string.Empty;
                    txtTdsAccount.Text = string.Empty;
                    setFocusControl(txtTdsAccount);
                }
            }
            if (strTextBox == "txtCashBank")
            {
                if (txtCashBank.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtCashBank.Text);
                    if (a == false)
                    {
                        btntxtCashBank_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnfbankid.Value = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");

                        if (str != string.Empty)
                        {
                            lblCashBank.Text = str;
                            setFocusControl(txtACCode);
                        }
                        else
                        {
                            lblCashBank.Text = string.Empty;
                            txtCashBank.Text = string.Empty;
                            setFocusControl(txtCashBank);

                        }
                    }
                }
                else
                {
                    lblCashBank.Text = string.Empty;
                    txtCashBank.Text = string.Empty;
                    setFocusControl(txtCashBank);

                }
            }
            #region Calculation Part
            double total = 0.00;
            if (grdDetail.Rows.Count > 0)
            {

                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    double Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                    total = total + Amt;
                }
            }
            txtTotal.Text = Math.Round(total, 2).ToString();
            #endregion

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
            //if (searchString != string.Empty)// && strTextBox == hdnfClosePopup.Value)
            //{
            //    txtSearchText.Text = searchString;
            //}
            //else
            //{
            //    txtSearchText.Text = txtSearchText.Text;
            //}

            string searchtxt = "";
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = "";
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
            if (hdnfClosePopup.Value == "txtdoc_no")
            {
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                else
                {
                    lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select doc_no,doc_date,debitAcName from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
                    " and  (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%') group by doc_no,doc_date,debitAcName order by doc_no";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtACCode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    // name += " Sub_Group_Code Like '%" + aa + "%'or Sub_Group_Name Like '%" + aa + "%'or HSN_Code Like '%" + aa + "%'or Main_Group_Code Like '%" + aa + "%'or Remark Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select AC Code--";
                string qry = "";
                if (drpPaymentFor.SelectedValue == "TP")
                {
                    //qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    //    " " + name + "";
                    qry = " Locked=0 and Ac_type='T' and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (" + name + ") ";


                }
                else
                {
                    //qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    //    " " + name + "";
                    qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                     + " and (" + name + ") ";
                }
                //this.showPopup(qry);
                this.showPopupAccountMaster(qry);
            }
            if (hdnfClosePopup.Value == "txtTdsAccount")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    // name += " Sub_Group_Code Like '%" + aa + "%'or Sub_Group_Name Like '%" + aa + "%'or HSN_Code Like '%" + aa + "%'or Main_Group_Code Like '%" + aa + "%'or Remark Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select AC Code--";
                string qry = "";
                //if (drpPaymentFor.SelectedValue == "TP")
                //{
                //    //qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                //    //    " " + name + "";
                //    qry = " Locked=0 and Ac_type='T' and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                //     + " and (" + name + ") ";


                //}
                //else
                //{
                //    //qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                //    //    " " + name + "";
                //    qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                //     + " and (" + name + ") ";
                //}
                qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                    + " and (" + name + ") ";
                //this.showPopup(qry);
                this.showPopupAccountMaster(qry);
            }
            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtACCode.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (iscarporate == "Y")
                    {
                        lblMsg.Text = "";
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select Unit_name,UnitName,unitCity from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtACCode.Text +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Unit_name like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or unitCity like '%" + txtSearchText.Text + "%') order by UnitName";
                        this.showPopup(qry);
                    }
                    else
                    {
                        lblPopupHead.Text = "--Select Unit Code--";
                        string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                        this.showPopup(qry);
                    }
                }
                else
                {
                    setFocusControl(txtACCode);
                }
            }
            if (hdnfClosePopup.Value == "txtCashBank")
            {
                string qry = "";
                lblPopupHead.Text = "--Select Cash/Bank--";

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    // name += " Sub_Group_Code Like '%" + aa + "%'or Sub_Group_Name Like '%" + aa + "%'or HSN_Code Like '%" + aa + "%'or Main_Group_Code Like '%" + aa + "%'or Remark Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or city_name_e like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                //if (drpTrnType.SelectedValue == "CP")
                //{
                //qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                //" " + name + "";
                //}
                //else
                //{
                //    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                //       " " + name + "";
                //}
                //this.showPopup(qry);
                qry = " Locked=0  and  dbo.nt_1_accountmaster.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                   + " and (" + name + ") ";

                this.showPopupAccountMaster(qry);
            }
            if (hdnfClosePopup.Value == "txtnarration")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + systemMasterTable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void drpTrnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Focus();
        //this.showLastRecord();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void grdPopup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdPopup.SelectedIndex = -1;
            grdPopup.DataBind();
        }
        catch { }
    }
    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        trntype = drpPaymentFor.SelectedValue;
        this.nextNumber();
    }
    #endregion

    protected void txtUnit_Code_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtUnit_Code.Text;
        strTextBox = "txtUnit_Code";
        csCalculations();
    }
    protected void btntxtUnitcode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUnit_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void btnGetvouchers_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtACCode.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtACCode);
                return;
            }

            if (drpPaymentFor.SelectedValue == "AF")
            {
                qry = "select 0 as Detail_Id, 0 as amount, 0 as adjusted_amount, '' as narration , doc_no as doc_no,0 as tenderid, convert(varchar(10),doc_date,103) as Doc_date,tran_type,transportname as PartyName,quantal,Memo_Advance as amounttopay,paid as received,'0' as adjusted,Balance as balance,doid, " +
             " Year_Code as EntryYearCode,millshortname,'' as adjacamt,truck_no as LorryNo from qrydofreightbalance where transport='" + txtACCode.Text +
             "' and Balance!=0 AND Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'";
            }
            else if (drpPaymentFor.SelectedValue == "MP")
            {
                qry = "select '' as LorryNo, '' as narration,0 as amount, 0 as adjusted_amount, 0 as Detail_Id,Tender_No as doc_no, ISNULL(tenderid,0) as tenderid,Tender_DateConverted as Doc_date,'MP' as tran_type, Quantal as quantal" +
                         ", millamt1 as amounttopay,paidamount as received,payableamount as balance,'0' as adjusted,Year_Code as EntryYearCode,Grade,millshortname, 0 as doid from qrymillpaymentbalance"
                           + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                           " and payableamount !=0 and Payment_To=" + txtACCode.Text;
            }

            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (btnSave.Text == "Update")
                    {
                        if (ViewState["currentTable"] != null)
                        {
                            DataTable dt1;
                            dt1 = (DataTable)ViewState["currentTable"];
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (dt.Rows[j]["tran_type"].ToString() == dt1.Rows[i]["tran_type"].ToString() && dt.Rows[j]["doc_no"].ToString() == dt1.Rows[i]["doc_no"].ToString())
                                    {
                                        dt.Rows[j].Delete();
                                        // dt1.Rows[i].Remo
                                        dt.AcceptChanges();
                                    }
                                }
                            }
                            //DataView view = new DataView(dt);
                            //view.RowFilter = ("Name<>'Ram'");
                            //dt = view.ToTable();

                            dt1.Merge(dt, true, MissingSchemaAction.Ignore);
                            // dt1 = dt1.DefaultView.ToTable(true, "tran_type", "doc_no");
                            grdDetail.DataSource = dt1;
                            grdDetail.DataBind();
                        }
                    }
                    else
                    {
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No Record Found This Ac Code " + txtACCode.Text + " ')", true);
                }
            }
            else
            {
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }
            setFocusControl(btnGetvouchers);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void drpPaymentFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string paymentFor = drpPaymentFor.SelectedValue;
        string max = clsCommon.getString("select max(doc_no) as id from " + tblHead + " where Tran_Type='" + paymentFor + "' " +
            " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
        hdnf.Value = max;
        trntype = paymentFor;
        clsButtonNavigation.enableDisable("N");
        this.makeEmptyForm("A"); // change this to N later
        ViewState["mode"] = "I";
        this.showLastRecord();

    }
    #region NextNumber
    private void nextNumber()
    {
        try
        {
            DOC_NO = Convert.ToInt32(clsCommon.getString("select isnull(max(DOC_NO),0) as docno from PaymentHead where Company_Code='" + Session["Company_Code"].ToString() + "' and " +
            " Year_Code=" + Session["year"].ToString() + " and tran_type='" + trntype + "'")) + 1;
            if (DOC_NO == 0)
            {
                DOC_NO = 1;
                txtdoc_no.Text = "1";
            }
            else
            {
                txtdoc_no.Text = DOC_NO.ToString();
            }
            //mr_no = Convert.ToInt32(clsCommon.getString("select isnull(max(mr_no),0) as mr_no from multiple_receipt_head ")) + 1;
            //if (mr_no == 0)
            //{
            //    mr_no = 1;
            //    lblmr_no.Text = "1";
            //}
            //else
            //{
            //    lblmr_no.Text = mr_no.ToString();
            //}
        }
        catch
        {

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

                        hdnfyearcode.Value = dt.Rows[0]["year_code"].ToString();
                        hdnfcompanycode.Value = dt.Rows[0]["company_code"].ToString();

                        //Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        //Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        //if (lblCreated != null)
                        //{
                        //    //lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        //}
                        //if (lblModified != null)
                        //{
                        //    //lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        //}
                        //Label lblCreatedDate = (Label)Master.FindControl("MasterlblCreatedDate");
                        //Label lblModifiedDate = (Label)Master.FindControl("MasterlblModifiedDate");
                        //if (lblCreatedDate != null)
                        //{
                        //    //if (dt.Rows[0]["Created_Date"].ToString() == string.Empty)
                        //    //{
                        //    //    lblCreatedDate.Text = "";
                        //    //}
                        //    //else
                        //    //{
                        //    //    lblCreatedDate.Text = "Created Date" + dt.Rows[0]["Created_Date"].ToString();
                        //    //}
                        //}
                        //if (lblModifiedDate != null)
                        //{
                        //    //if (dt.Rows[0]["Modified_Date"].ToString() == string.Empty)
                        //    //{
                        //    //    lblModifiedDate.Text = "";
                        //    //}
                        //    //else
                        //    //{
                        //    //    lblModifiedDate.Text = "Modified Date" + dt.Rows[0]["Modified_Date"].ToString();
                        //    //}
                        //}

                        txtdoc_no.Text = dt.Rows[0]["doc_no"].ToString();
                        hdnfdoc.Value = txtdoc_no.Text;
                        lblmr_no.Text = dt.Rows[0]["id"].ToString();
                        hdnfid.Value = dt.Rows[0]["id"].ToString();
                        drpPaymentFor.Text = dt.Rows[0]["Tran_Type"].ToString();
                        txtdoc_date.Text = Convert.ToDateTime(dt.Rows[0]["date"].ToString()).ToString("dd-MM-yyyy");
                        string paymentTo = dt.Rows[0]["payment_to"].ToString();
                        txtACCode.Text = paymentTo;
                        lblACName.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + paymentTo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        string cashBank = dt.Rows[0]["bank_code"].ToString();
                        txtCashBank.Text = cashBank;
                        lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + cashBank + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        txtAmount.Text = dt.Rows[0]["amount"].ToString();
                        string tdsAccount = dt.Rows[0]["tds_ac"].ToString();
                        txtTdsAccount.Text = tdsAccount;
                        lblTdsAcName.Text = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + tdsAccount + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        txtTdsPercent.Text = dt.Rows[0]["tds_percent"].ToString();
                        txtTdsAmount.Text = dt.Rows[0]["tds_amount"].ToString();
                        //txtnarrationLedger.Text = dt.Rows[0]["narration"].ToString();
                        hdnfacid.Value = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
                        hdnfbankid.Value = clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");

                        if (drpPaymentFor.SelectedValue == "AF")
                        {
                            qry = "select pd.Detail_Id as Detail_Id, pd.amount as amount, pd.adjusted_amount as adjusted_amount,0 as tenderid, pd.narration , qf.doc_no as doc_no,convert(varchar(10),qf.doc_date,103) as Doc_date,qf.tran_type,qf.transportname as PartyName,qf.quantal,qf.Memo_Advance as amounttopay,qf.paid as received,'0' as adjusted,qf.Balance as balance,qf.doid, " +
                         " qf.Year_Code as EntryYearCode,qf.millshortname,'' as adjacamt,qf.truck_no as LorryNo from qrydofreightbalance qf inner join PaymentDetail pd ON pd.doc_no=qf.doc_no  where pd.head_id='" + lblmr_no.Text +
                         "' and qf.Balance!=0 AND qf.Company_Code='" + Session["Company_Code"].ToString() + "' and qf.Year_Code='" + Session["year"].ToString() + "'";
                        }
                        //else if (drpPaymentFor.SelectedValue == "MP")
                        //{
                        //    qry = "select '' as LorryNo, pd.Detail_Id as Detail_Id, pd.amount as amount, pd.adjusted_amount as adjusted_amount, pd.narration ,qf.Tender_No as doc_no, ISNULL(qf.tenderid,0) as tenderid,qf.Tender_DateConverted as Doc_date,'MP' as tran_type, qf.Quantal as quantal" +
                        //             ", qf.millamt1 as amounttopay,qf.paidamount as received,qf.payableamount as balance,'0' as adjusted,qf.Year_Code as EntryYearCode,qf.Grade,qf.millshortname, 0 as doid from qrymillpaymentbalance qf" +
                        //             " inner join PaymentDetail pd ON pd.doc_no=qf.Tender_No  "
                        //               + " where qf.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        //               " and qf.payableamount !=0 and pd.head_id=" + lblmr_no.Text;
                        //}
                        else if (drpPaymentFor.SelectedValue == "MP")
                        {
                            qry = "select '' as LorryNo, pd.Detail_Id as Detail_Id, pd.amount as amount, pd.adjusted_amount as adjusted_amount, pd.narration ,qf.Tender_No as doc_no, ISNULL(qf.tenderid,0) as tenderid,qf.Tender_DateConverted as Doc_date,'MP' as tran_type, qf.Quantal as quantal" +
                                     ", qf.millamt1 as amounttopay,qf.paidamount as received,qf.payableamount as balance,'0' as adjusted,qf.Year_Code as EntryYearCode,qf.Grade,qf.millshortname, 0 as doid from qrymillpaymentbalance qf" +
                                     " inner join PaymentDetail pd ON pd.doc_no=qf.Tender_No  "
                                       + " where  qf.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                       "  and pd.head_id=" + lblmr_no.Text;
                        }


                        DataSet ds1 = clsDAL.SimpleQuery(qry);
                        if (ds1 != null)
                        {
                            DataTable dt1 = ds1.Tables[0];
                            if (dt1.Rows.Count > 0)
                            {
                                double balanceAmt = 0.00;
                                double aamt = 0.00;
                                double aamtadj = 0.00;
                                double aamtonac = 0.00;
                                //for (int i = 0; i < dt1.Rows.Count; i++)
                                //{

                                //    balanceAmt = balanceAmt + Convert.ToDouble(dt1.Rows[i]["amount"].ToString());
                                //    aamtadj = aamtadj + Convert.ToDouble(dt1.Rows[i]["adjusted_amount"].ToString());
                                //    aamtonac = aamtonac + Convert.ToDouble(dt1.Rows[i]["OnAc"].ToString());
                                //    aamt = aamt + Convert.ToDouble(dt1.Rows[i]["Bill_Amount"].ToString());

                                //}
                                //txtTotal.Text = balanceAmt.ToString();
                                //balanceAmt = Convert.ToDouble(txtAmount.Text) - (balanceAmt + aamtadj + aamtonac);

                                //txtBalance.Text = balanceAmt.ToString();
                                grdDetail.DataSource = dt1;
                                grdDetail.DataBind();
                                //csCalculations();
                                CalculateBalance();
                                ViewState["currentTable"] = dt1;
                            }
                        }


                    }
                }
            }
            pnlgrdDetail.Enabled = false;
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string id = lblmr_no.Text;
        string isPaymentDone = clsDAL.GetString("select isPaymentDone from PaymentHead where id=" + Convert.ToInt64(id) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (isPaymentDone == "1")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('You can not edit this entry as Payment is done for this record.')", true);
            return;
        }
        clsButtonNavigation.enableDisable("E");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("E");
        btnSave.Text = "Update";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
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
                btnEdit.Focus();
            }
            else                     //new code
            {
                btnEdit.Enabled = false;

            }

        }
        catch
        {
        }
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                //Company_Code = Session["Company_Code"].ToString();
                //Year_Code = Session["year"].ToString();
                string id = lblmr_no.Text;
                string isPaymentDone = clsDAL.GetString("select isPaymentDone from PaymentHead where id=" + Convert.ToInt64(id) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (isPaymentDone == "1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('You can not Delete this entry as Payment is done for this record.')", true);
                    return;
                }
                else
                {
                    var qryDeleteBuilder = new StringBuilder();
                    string qryDeletePaymentHead = "delete from PaymentHead where id=" + id;
                    string qryDeletePaymentDetail = "delete from PaymentDetail where head_id=" + id;
                    string qryDeleteMultipleReceipt = string.Empty;

                    if (drpPaymentFor.SelectedValue == "AF")
                    {
                        qryDeleteMultipleReceipt = "delete from multiple_receipt_detail where tran_type='" + drpPaymentFor.SelectedValue + "' and mr_no=" + id;
                    }
                    else
                    {
                        string detailIds = clsDAL.GetString("SELECT STRING_AGG(CAST(Detail_Id AS VARCHAR), ', ') AS concatenated_detail_ids FROM PaymentDetail WHERE head_id = " + id + "");
                        qryDeleteMultipleReceipt = "delete from nt_1_utr_detail where doc_no=" + id + " and bpid in (" + detailIds + ")";
                    }

                    qryDeleteBuilder.Append(qryDeletePaymentHead);
                    qryDeleteBuilder.Append(qryDeletePaymentDetail);
                    qryDeleteBuilder.Append(qryDeleteMultipleReceipt);

                    string query2 = qryDeleteBuilder.ToString();
                    var ds3 = clsDAL.SimpleQuery(query2);

                    long lastId = clsDAL.ExecuteScalar("select top(1) id from PaymentHead order by id desc");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asdasdc", "DeleteSuccess('Delete'," + lastId + "," + lastId + ")", true);
                }

                // delete ledger entry

                string deleteLedgerQuery = "delete from nt_1_gledger where TRAN_TYPE='BK' and DOC_NO="+ id+ " and COMPANY_CODE="+ Company_Code + " and YEAR_CODE="+EntryYear+"";
                var ds4 = clsDAL.SimpleQuery(deleteLedgerQuery);
            }
        }
        catch
        {

        }
    }
    #endregion

    protected void txtOnAc_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtOnAc";
        int index = RowIndex(sender);
        this.calculation(index);
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
                trntype = drpPaymentFor.SelectedValue;
                string docno = clsCommon.getString("select id from PaymentHead where doc_no =" + txtEditDoc_No.Text + " and Company_Code='"
                + Session["Company_Code"].ToString() + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + drpPaymentFor.SelectedValue + "'");
                hdnf.Value = docno;
                //hdnf.Value = txtEditDoc_No.Text;
                string qry1 = getDisplayQuery();
                fetchRecord(qry1);
                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtTdsAccount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTdsAccount.Text;
        strTextBox = "txtTdsAccount";
        csCalculations();
    }

    protected void btnTdsAccount_Click(object sender, EventArgs e)
    {
        try
        {
            nn = 1;
            searchString = txtTdsAccount.Text;
            strTextBox = "txtTdsAccount";
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTdsAccount";
            btnSearch_Click(sender, e);

        }
        catch
        {
        }
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        CalculateTdsBasedOnAmount("amount");
        CalculateBalance();
        setFocusControl(txtTdsPercent);
    }

    protected void txtTdsPercent_TextChanged(object sender, EventArgs e)
    {
        CalculateTdsBasedOnAmount("percent");
        CalculateBalance();
        setFocusControl(txtTdsAmount);
    }

    protected void txtTdsAmount_TextChanged(object sender, EventArgs e)
    {
        CalculateTdsBasedOnAmount("tdsamount");
        CalculateBalance();
        setFocusControl(btnGetvouchers);
    }

    private void CalculateTdsBasedOnAmount(string changedField)
    {
        double percent, amount, tdsAmount;

        // Safely parsing values, defaults to 0 if invalid or empty
        double.TryParse(txtTdsPercent.Text, out percent);
        double.TryParse(txtAmount.Text, out amount);
        double.TryParse(txtTdsAmount.Text, out tdsAmount);

        // Calculate TDS Amount if percent text changed or Calculate Percent if TDS Amount text changed
        if (changedField == "percent" && amount > 0)
        {
            tdsAmount = CalculateAmount(percent, amount);
            txtTdsAmount.Text = tdsAmount.ToString("N2"); // Formatting to 2 decimal places
        }
        else if (changedField == "amount" && amount > 0)
        {
            if (percent > 0)
            {
                tdsAmount = CalculateAmount(percent, amount);
                txtTdsAmount.Text = tdsAmount.ToString("N2"); // Formatting to 2 decimal places
            }
            else if (tdsAmount > 0)
            {
                percent = CalculatePercentage(tdsAmount, amount);
                txtTdsPercent.Text = percent.ToString("N2"); // Formatting to 2 decimal places
            }
        }
        else if (changedField == "tdsamount" && amount > 0)
        {
            if (tdsAmount > 0)
            {
                percent = CalculatePercentage(tdsAmount, amount);
                txtTdsPercent.Text = percent.ToString("N2"); // Formatting to 2 decimal places
            }
        }
    }

    private double CalculatePercentage(double part, double whole)
    {
        if (whole == 0) return 0; // Prevent division by zero
        return (part / whole) * 100;
    }

    private double CalculateAmount(double percentage, double totalAmount)
    {
        return (percentage / 100) * totalAmount;
    }


    protected async void btnMakePayment_Click(object sender, EventArgs e)
    {
        // fetch account details
        string paymentTo = txtACCode.Text;
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
                    benBankAccountNumber = dt.Rows[0]["Bank_Ac_No"].ToString();
                    benBankIFSC = dt.Rows[0]["IFSC"].ToString();
                    benAccountName = dt.Rows[0]["Ac_Name_E"].ToString();
                }
            }
        }

        bool isAcNoNumeric = benBankAccountNumber.All(char.IsDigit);

        if (!isAcNoNumeric || benBankIFSC == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Beneficiery details are incorrect!')", true);
            return;
        }

        using (HttpClient client = new HttpClient())
        {
            try
            {
                PaymentRequestModel paymentRequest = new PaymentRequestModel();

                PaymentInfoModel paymentInfo = new PaymentInfoModel
                {
                    AccountNo = Session["accountNo"].ToString(),
                    ClientCode = Session["clintCode"].ToString(),
                    DrDesc = "",
                    MyProdCode = "VPAY",
                    PayMode = "NEFT",
                    //TxnAmnt = txtamount.Text,
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

                    int Year_Code = Convert.ToInt32(Session["year"].ToString());
                    int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());

                    Head_Fields = Head_Fields + "doc_no,";
                    Head_Values = Head_Values + "'" + txtdoc_no.Text + "',";
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

                    // update isPayment done field of PaymentDetails table  
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
                    Head_Values = Head_Values + "'" + txtdoc_no.Text + "',";
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

    #region DataStore

    public static SqlConnection con = null;
    public static SqlCommand cmd = null;
    public static SqlTransaction myTran = null;
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