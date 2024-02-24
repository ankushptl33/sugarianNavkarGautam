﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Sugar_Report_pgePaindingReport : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string searchStr = string.Empty;
    string strTextbox = string.Empty;
    static WebControl objAsp = null;
    string AccountMasterTable = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                txtFromDate.Text = Session["Start_Date"].ToString();
                txtToDate.Text = Session["End_Date"].ToString();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
           string searchString = txtAcCode.Text;
            string Group_Account = string.Empty;
            if (txtAcCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAcCode.Text);
                if (a == false)
                {
                    btnAcCode_Click(this, new EventArgs());
                }
                else
                {
                    Group_Account = clsCommon.getString("select GroupName from GroupCreactionMaster where Doc_No='" + txtAcCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (Group_Account != string.Empty && Group_Account != "0")
                    {
                        hdnfgid.Value = clsCommon.getString("select isnull(autoid,0) as acid from GroupCreactionMaster where Doc_no='" + txtAcCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                        if (Group_Account.Length > 15)
                        {
                            Group_Account.Substring(0, 15);
                        }
                        else if (Group_Account.Length > 10)
                        {
                            Group_Account.Substring(0, 10);
                        }
                        lblAcCodeName.Text = Group_Account;
                        setFocusControl(btnPaindingReport);

                    }
                    else
                    {
                        txtAcCode.Text = string.Empty;
                        lblAcCodeName.Text = string.Empty;
                        setFocusControl(txtAcCode);
                        // AmtCalculation();
                    }
                }
            }
            else
            {
                txtAcCode.Text = string.Empty;
                lblAcCodeName.Text = Group_Account;
            }

        }
        catch
        {
        }
    }

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
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
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
                e.Row.Attributes["onselectstart"] = "javascript:return true;";
            }
        }
        catch
        {
            throw;
        }
    }
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("250px");
            e.Row.Cells[2].Width = new Unit("100px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    protected void imgBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {

        }
    }
    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            string searchtxt = searchStr;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchStr;
            string[] split = null;
            string name = string.Empty;
            if (searchStr != string.Empty && strTextbox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchStr;
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
            if (searchStr != string.Empty && strTextbox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchStr;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtAcCode")
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
        }
        catch
        {

        }
    }
    private void showPopup(string qry)
    {
        try
        {
            this.setFocusControl(txtSearchText);
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

    protected void btnPaindingReport_Click(object sender, EventArgs e)
    {
        string Ac_Code = txtAcCode.Text; 

        if (Ac_Code != string.Empty)
        {
            Ac_Code = txtAcCode.Text;
        }
        else
        {
            Ac_Code = "0";
        }  
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ku", "javascript:pr('" + Ac_Code + "')", true);
        pnlPopup.Style["display"] = "none";
    }

    protected void btnGroupwaiceTender_Click(object sender, EventArgs e)
    {
        string Ac_Code = txtAcCode.Text;

        if (Ac_Code != string.Empty)
        {
            Ac_Code = txtAcCode.Text;
        }
        else
        {
            Ac_Code = "0";
        }
        string FromDt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        string ToDt = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ku", "javascript:GT('" + Ac_Code + "','" + FromDt + "','" + ToDt + "')", true);
        pnlPopup.Style["display"] = "none";
    }

    protected void btnSelfGroupwaice_Click(object sender, EventArgs e)
    {
        string Ac_Code = txtAcCode.Text;

        if (Ac_Code != string.Empty)
        {
            Ac_Code = txtAcCode.Text;
        }
        else
        {
            Ac_Code = "0";
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ku", "javascript:SG('" + Ac_Code + "')", true);
        pnlPopup.Style["display"] = "none";
    }

    protected void btnStockBookMill_Click(object sender, EventArgs e)
    {
         
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ku", "javascript:MW()", true); 
    }
   
}