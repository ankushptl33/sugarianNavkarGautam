using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class Sugar_BussinessRelated_pgeGroupUtility : System.Web.UI.Page
{
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

    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;   
    Int32 mill_Code;
    double quntal = 0.00;
    double SaleRate = 0.00;
    double Item_Amount = 0.00;
    double paid = 0.00;
    string paidDate = string.Empty;
    string Narration = string.Empty;
    Int32 Other_PaidAc;
    Int32 Other_PaidAcId;
    double Other_PaidAmount = 0.00;
    Int32 Sauda_ReverseAc;
    Int32 Sauda_ReverseAcId;
    double Reverse_RatePerQtl = 0.00;
    double ReverseAmount = 0.00; 
    string Head_Insert = string.Empty; 
    StringBuilder Head_Update = null;
    DataTable Maindt = new DataTable();
    DataRow dr = null;
    string Update_Detail = string.Empty; 
    string msg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Head_Fields = new StringBuilder();
        Head_Values = new StringBuilder();
        Head_Update = new StringBuilder();
        tblHead = "GroupTenderDetails";
        Maindt = new DataTable();
        dr = null;
        Maindt.Columns.Add("Querys", typeof(string));

    }

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

    #region[btnMember_Click]
    protected void btnMember_Click(object sender, EventArgs e)
    {
        //  pnlPopupTenderDetails.Style["display"] = "block";
       
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "BU";
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

    #region [txtBuyerQuantal_TextChanged]
    protected void txtBuyerQuantal_TextChanged(object sender, EventArgs e)
    {

        TCSAmountCalculate(); 
        setFocusControl(txtBuyerSaleRate);
    }

    #endregion

    #region [txtBuyerSaleRate_TextChanged]
    protected void txtBuyerSaleRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtPaid);
        TCSAmountCalculate();
    }
    #endregion

    private void TCSAmountCalculate()
    {
        try
        {
            double cashdiffvalue = 0.00;
            double qntl = txtBuyerQuantal.Text != string.Empty ? Convert.ToDouble(txtBuyerQuantal.Text) : 0.00;
            double srate = Convert.ToDouble((txtBuyerSaleRate.Text != string.Empty ? Convert.ToDouble(txtBuyerSaleRate.Text) : 0)); 
            double RatePerQtl = Convert.ToDouble((txtRatePerQtl.Text != string.Empty ? Convert.ToDouble(txtRatePerQtl.Text) : 0));
            double millRate = (Convert.ToDouble(lblmillrate.Text != string.Empty ? Convert.ToDouble(lblmillrate.Text) : 0));
         

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

    #region [txtItemAmount_TextChanged]
    protected void txtItemAmount_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    protected void txtPaid_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtDetailpaidDate);
    }

    #region [txtDetailpaidDate_TextChanged]
    protected void txtDetailpaidDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDetailpaidDate.Text != string.Empty)
            {

            }
        }
        catch
        {
            txtDetailpaidDate.Text = string.Empty;
            setFocusControl(txtDetailpaidDate);
        }
    }
    #endregion

    #region [txtBuyerNarration_TextChanged]
    protected void txtBuyerNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtotherpaidAc);
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

    #region[btnotherpaidAc_Click]
    protected void btnotherpaidAc_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtotherpaidAc";
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

    protected void txtotherPaid_TextChanged(object sender, EventArgs e)
    {
       setFocusControl(txtsaudaReverseAc);
    }


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
            if (hdnfpopup.Value == "")
            {
             pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtsaudaReverseAc";
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

    #region[txtmillcode_TextChanged]
    protected void txtmillcode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmillcode.Text;
        strTextBox = "txtmillcode";
        csCalculations();

    }
    #endregion


    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtmillcode")
            {

                if (txtmillcode.Text != string.Empty)
                {

                    qry = "select Tender_No,Tender_dateConverted as Tender_Date, Lifting_DateConverted as Lifting_Date,  Mill_Code, millName,Grade,Buyer_Quantal,GroupName,Mill_Rate,Group_Account from qryGroupTenderHeadDetail where Mill_Code=" + txtmillcode.Text + " and Tender_No=" + hdnftenderno.Value +
                        " and membercode=2   and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            txtmillcode.Text = ds.Tables[0].Rows[0]["Mill_Code"].ToString();
                            lblmillname.Text = ds.Tables[0].Rows[0]["millName"].ToString();
                            txttender.Text = ds.Tables[0].Rows[0]["Tender_No"].ToString();
                            lblselfqty.Text = ds.Tables[0].Rows[0]["Buyer_Quantal"].ToString();
                            lblgrade.Text = ds.Tables[0].Rows[0]["Grade"].ToString();
                            lblmillrate.Text = ds.Tables[0].Rows[0]["Mill_Rate"].ToString();
                            lbllifting.Text = ds.Tables[0].Rows[0]["Lifting_Date"].ToString();
                            lblGroupName.Text = ds.Tables[0].Rows[0]["GroupName"].ToString();
                            hdnfGroupCode.Value = ds.Tables[0].Rows[0]["Group_Account"].ToString();

                            txtDetailpaidDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                            setFocusControl(txttender);

                        }
                        else
                        {
                            setFocusControl(txtmillcode);
                        }
                    }
                }

            }
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
        }
        catch { }
    }
    #endregion

    #region [btntxtmillcode_Click]
    protected void btntxtmillcode_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtmillcode";
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


            if (hdnfClosePopup.Value == "txtmillcode")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Tender_No like '%" + aa + "%' or millName like '%" + aa + "%' or millshortname like '%" + aa + "%' or Grade like '%" + aa + "%' or Buyer_Quantal like '%" + aa + "%' or GroupName like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Mill code--";
                string qry = "select Tender_No,Tender_dateConverted as Tender_Date, Lifting_DateConverted as Lifting_Date,Mill_Code,millName,Grade,Buyer_Quantal,GroupName,Mill_Rate from qryGroupTenderHeadDetail where membercode=2 and Buyer_Quantal<>0 " +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name + " ";
                // + " and Year_code=" + Session["year"].ToString() + " " +

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
                string qry = "select member,name,acid from qryGroupMemberUnion where member!=0 and Doc_No=" + hdnfGroupCode.Value + " and " +
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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion


    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtmillcode")
            {
                setFocusControl(txtmillcode);
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    string v = hdnfClosePopup.Value;
        //    if (e.Row.RowType != DataControlRowType.Pager)
        //    {

        //        e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
        //        e.Row.Cells[1].ControlStyle.Width = new Unit("100px");
        //        e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
        //        e.Row.Cells[3].ControlStyle.Width = new Unit("200px");
        //        e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
        //        e.Row.Cells[5].ControlStyle.Width = new Unit("50px");
        //        e.Row.Cells[6].ControlStyle.Width = new Unit("50px");
        //        e.Row.Cells[7].ControlStyle.Width = new Unit("40px");
        //        e.Row.Cells[8].ControlStyle.Width = new Unit("50px");
        //        e.Row.Cells[9].ControlStyle.Width = new Unit("40px");
        //    }

        //    if (v == "BU" || v == "BP" || v == "txtShipTo")
        //    {
        //        //if (e.Row.RowType != DataControlRowType.Pager)
        //        //{
        //        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
        //        e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
        //        e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
        //        e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
        //        e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
        //        // }
        //    }
        //    if (v == "txtBP_Account")
        //    {
        //        //if (e.Row.RowType != DataControlRowType.Pager)
        //        //{
        //        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
        //        e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
        //        e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
        //        e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
        //        // e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
        //        // }
        //    }
        //}
        //catch
        //{

        //}
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

     #region [btnUpdate_Click]
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        #region validation
        bool isvalidate = true;
        if (txtmillcode.Text != string.Empty && txtmillcode.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtmillcode.Text = string.Empty;
            setFocusControl(txtmillcode);
            return;
        }
        if (lblMemberName.Text != string.Empty && lblMemberName.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtMemberName.Text = string.Empty;
            setFocusControl(txtMemberName);
            return;
        }


        Int32 Tender_No = txttender.Text != string.Empty ? Convert.ToInt32(txttender.Text) : 0;
        int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
        Int32 Member_Code = txtMemberName.Text != string.Empty ? Convert.ToInt32(txtMemberName.Text) : 0;
        mill_Code = txtmillcode.Text != string.Empty ? Convert.ToInt32(txtmillcode.Text) : 0;
        Int32 mid = hdnfmember.Value != string.Empty ? Convert.ToInt32(hdnfmember.Value) : 0;
        SaleRate = txtBuyerSaleRate.Text != string.Empty ? Convert.ToDouble(txtBuyerSaleRate.Text) : 0.00;
        quntal = txtBuyerQuantal.Text != string.Empty ? Convert.ToDouble(txtBuyerQuantal.Text) : 0.00;
        paid = txtPaid.Text != string.Empty ? Convert.ToDouble(txtPaid.Text) : 0.00;
        paidDate = DateTime.Parse(txtDetailpaidDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        Narration = txtBuyerNarration.Text;
        Int32 grouptenderid;
        Int32 ID;
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        Item_Amount = txtItemAmount.Text != string.Empty ? Convert.ToDouble(txtItemAmount.Text) : 0.00;
        double profit = hdnfProfit.Value != string.Empty ? Convert.ToDouble(hdnfProfit.Value) : 0.00;
        double indivisulProfit = 0.00;
        double millContribution = 0.00;
        double paidorreseive = 0.00;
        Other_PaidAc = txtotherpaidAc.Text != string.Empty ? Convert.ToInt32(txtotherpaidAc.Text) : 0;
        Other_PaidAmount = txtotherPaid.Text != string.Empty ? Convert.ToInt32(txtotherPaid.Text) : 0;
        Other_PaidAcId = hdnfotherpaid.Value != string.Empty ? Convert.ToInt32(hdnfotherpaid.Value) : 0;
       
        Sauda_ReverseAc = txtsaudaReverseAc.Text != string.Empty ? Convert.ToInt32(txtsaudaReverseAc.Text) : 0;
        Sauda_ReverseAcId = hdnfReverseAcId.Value != string.Empty ? Convert.ToInt32(hdnfReverseAcId.Value) : 0;
        Reverse_RatePerQtl = txtRatePerQtl.Text != string.Empty ? Convert.ToDouble(txtRatePerQtl.Text) : 0.00;
        ReverseAmount = txtReverseAmount.Text != string.Empty ? Convert.ToDouble(txtReverseAmount.Text) : 0.00;

        Int32 tenderdetailid = Convert.ToInt32(clsCommon.getString("SELECT trnderdetailid from GroupTenderDetails where Tender_No=" + txttender.Text +
                " and Company_Code='" + Session["Company_Code"].ToString() + "' and membercode='2' "));

        Int32 counts = Convert.ToInt32(clsCommon.getString("SELECT max(ID) as ID from GroupTenderDetails where Company_Code='"
               + Session["Company_Code"].ToString() + "' and Tender_No=" + txttender.Text + " ")) + 1; 
        Int32 count = counts;

        grouptenderid = Convert.ToInt32(clsCommon.getString("SELECT grouptenderid as grouptenderid from GroupTenderPurchase where Company_Code='"
            + Session["Company_Code"].ToString() + "' and Tender_No=" + txttender.Text + " "));

        #endregion

        int flag = 0;
        if (btnUpdate.Text == "Update")
        {
            double TenderQuntl = Convert.ToDouble(clsCommon.getString("SELECT Buyer_Quantal from " + tblHead + " where trnderdetailid="
               + tenderdetailid + " and Company_Code=" + Session["Company_Code"].ToString()));
            //    "' and Year_Code=" + Convert.ToInt32(Session["year"]).ToString()));
            TenderQuntl = TenderQuntl - quntal;

            #region Head Add Fields And Values
            Head_Fields.Append("Tender_No,");
            Head_Values.Append("'" + Tender_No + "',");
            Head_Fields.Append("Company_Code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("membercode,");
            Head_Values.Append("'" + Member_Code + "',");
            Head_Fields.Append("mid,");
            Head_Values.Append("'" + mid + "',");
            Head_Fields.Append("Buyer_Quantal,");
            Head_Values.Append("'" + quntal + "',");
            Head_Fields.Append("Sale_Rate,");
            Head_Values.Append("'" + SaleRate + "',");
            Head_Fields.Append("paid,");
            Head_Values.Append("'" + paid + "',");
            Head_Fields.Append("paidDate,");
            Head_Values.Append("'" + paidDate + "',");
            Head_Fields.Append("Narration,");
            Head_Values.Append("'" + Narration + "',");
            Head_Fields.Append("grouptenderid,");
            Head_Values.Append("'" + grouptenderid + "',");
            Head_Fields.Append("ID,");
            Head_Values.Append("'" + count + "',");
            Head_Fields.Append("year_code,");
            Head_Values.Append("'" + Year_Code + "',");
            Head_Fields.Append("ItemAmount,");
            Head_Values.Append("'" + Item_Amount + "',");
            Head_Fields.Append("profit,");
            Head_Values.Append("'" + profit + "',");
            Head_Fields.Append("indivisulProfit,");
            Head_Values.Append("'" + indivisulProfit + "',");
            Head_Fields.Append("millContribution,");
            Head_Values.Append("'" + millContribution + "',");
            Head_Fields.Append("paidorreseive,");
            Head_Values.Append("'" + paidorreseive + "',");
            Head_Fields.Append("otherpaidAc,");
            Head_Values.Append("'" + Other_PaidAc + "',");
            Head_Fields.Append("otherPaid,");
            Head_Values.Append("'" + Other_PaidAmount + "',");
            Head_Fields.Append("otherPaidid,");
            Head_Values.Append("'" + Other_PaidAcId + "',");
            Head_Fields.Append("saudaReverseAc,");
            Head_Values.Append("'" + Sauda_ReverseAc + "',");
            Head_Fields.Append("saudaReverseId,");
            Head_Values.Append("'" + Sauda_ReverseAcId + "',");
            Head_Fields.Append("RatePerQtl,");
            Head_Values.Append("'" + Reverse_RatePerQtl + "',");
            Head_Fields.Append("ReverseAmount");
            Head_Values.Append("'" + ReverseAmount + "'"); 
            #endregion

            Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
            DataRow dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);

            Update_Detail = "update GroupTenderPurchase set Prosess='N' where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                 + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txttender.Text + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Update_Detail;
            Maindt.Rows.Add(dr);
            flag = 1;

            Update_Detail = "Update " + tblHead + " set Buyer_Quantal='" + TenderQuntl + "' where trnderdetailid ='" + tenderdetailid + "'";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Update_Detail;
            Maindt.Rows.Add(dr);
            flag = 1;
        }
        else
        {


        }

        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        if (msg == "Insert")
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Sucessfully  Add!')", true);
            txtmillcode.Text = string.Empty;
            lblmillname.Text = string.Empty;
            lblGroupName.Text = string.Empty;
            txttender.Text = string.Empty;
            lblselfqty.Text = string.Empty;
            lblgrade.Text = string.Empty;
            lbllifting.Text = string.Empty;
            lblmillrate.Text = string.Empty;
            lblseason.Text = string.Empty;
            lblpurcrate.Text = string.Empty;
            txtMemberName.Text = string.Empty;
            lblMemberName.Text = string.Empty;
            txtBuyerQuantal.Text = string.Empty;
            txtBuyerSaleRate.Text = string.Empty;
            txtItemAmount.Text = string.Empty;
            txtPaid.Text = string.Empty;
            txtDetailpaidDate.Text = string.Empty;
            txtBuyerNarration.Text = string.Empty;
            txtotherpaidAc.Text = string.Empty;
            txtotherPaid.Text = string.Empty;
            txtsaudaReverseAc.Text = string.Empty;
            lblsaudaReverseAcName.Text = string.Empty;
            txtRatePerQtl.Text = string.Empty;
            txtReverseAmount.Text = string.Empty; 
        }

    }
     #endregion
}