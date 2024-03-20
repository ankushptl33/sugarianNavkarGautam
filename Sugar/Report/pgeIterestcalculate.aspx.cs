using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sugar_Report_pgeIterestcalculate : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string AccountMasterTable = string.Empty;
    string searchStr = "";
    string strTextbox = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            // BindContrydropdown();
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                //txtFromDt.Text = clsGV.Start_Date;
                //txtToDt.Text = clsGV.To_date;
                txtFromDt.Text = Session["Start_Date"].ToString();
                txtToDt.Text = Session["End_Date"].ToString();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnAcCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfnew.Value != "N")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtAcCode";
                btnSearch_Click(sender, e);
            }
            else
            {
                hdnfnew.Value = "";
            }
        }
        catch
        {
        }
    }
    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtAcCode.Text != string.Empty)
            {
                searchStr = txtAcCode.Text;
                hdnfAc.Value = "0";
                strTextbox = "txtAcCode";

                bool a = clsCommon.isStringIsNumeric(txtAcCode.Text);
                if (a == false)
                {
                    btnAcCode_Click(this, new EventArgs());
                }
                else
                {
                    string str = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAcCode.Text);
                    if (str != string.Empty)
                    {
                        lblAcCodeName.Text = str;
                        hdnfAc.Value = clsCommon.getString("select accoid from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and accoid=" + txtAcCode.Text);

                        // setFocusControl(txtUnitCode);
                        pnlPopup.Style["display"] = "none";
                    }
                    else
                    {

                        txtAcCode.Text = string.Empty;
                        lblAcCodeName.Text = string.Empty;
                    }
                }
            }
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
    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
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
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account--";


                string qry = "SELECT a.Ac_Code, a.Ac_Name_E,dbo.nt_1_citymaster.city_name_e as City_Name FROM nt_1_bsgroupmaster g RIGHT OUTER JOIN  " +
                    " nt_1_accountmaster a ON g.bsid = a.bsid LEFT  OUTER JOIN nt_1_gledger l ON a.accoid = l.ac LEFT OUTER JOIN dbo.nt_1_citymaster ON a.cityid = dbo.nt_1_citymaster.cityid where (a.Ac_Code like'%" + txtSearchText.Text + "%' or a.Ac_Name_E like '%" + txtSearchText.Text + "%' or dbo.nt_1_citymaster.city_name_e like '%" + txtSearchText.Text + "%')  and l.Company_Code='"
                     + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and a.Locked=0  group by  a.Ac_Code, a.Ac_Name_E,dbo.nt_1_citymaster.city_name_e";
                this.showPopup(qry);

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
            //    hdnfClosePopup.Value = "Close";
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

                searchStr = txtSearchText.Text;
                strTextbox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
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
                e.Row.Attributes["onselectstart"] = "javascript:return true;";

                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

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

                if (v == "txtAcCode")
                {
                    e.Row.Cells[0].Width = new Unit("150px");
                    e.Row.Cells[1].Width = new Unit("550px");
                    e.Row.Cells[2].Width = new Unit("250px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                }

            }
        }
        catch
        {

        }
    }
    protected void btnInterestData_Click(object sender, EventArgs e)
    {
        //double InterestRate =0.00;
        string accode = txtAcCode.Text;
        DataSet ds;
        if (accode != string.Empty && accode != "0")
        {
            ds = clsDAL.SimpleQuery("SELECT * FROM  nt_1_accountmaster WHERE company_code="
               + Session["Company_Code"].ToString() + " and AC_CODE=" + accode);

        }
        else
        {
            ds = clsDAL.SimpleQuery("SELECT * FROM  nt_1_accountmaster WHERE Ac_type='I' AND company_code="
               + Session["Company_Code"].ToString());
        }
        string lastdate = DateTime.Parse(Session["End_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string startdate = DateTime.Parse(Session["Start_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string fromdate = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string todate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        DataTable dtreturn = new DataTable();
        dtreturn.Columns.Add("accode", typeof(string));
        dtreturn.Columns.Add("acname", typeof(string));
        dtreturn.Columns.Add("ClosingBalance", typeof(double));
        dtreturn.Columns.Add("InterestRate", typeof(double));
        dtreturn.Columns.Add("InterestAmtDr", typeof(double));
        dtreturn.Columns.Add("InterestAmtCr", typeof(double));
        dtreturn.Columns.Add("TDSRate", typeof(double));
        dtreturn.Columns.Add("TdsAmount", typeof(double));
        DataRow dr;
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double tdsrate = 0.00;

                dr = dtreturn.NewRow();
                string TDSApplicable = Convert.ToString(dt.Rows[i]["TDSApplicable"]);
                accode = Convert.ToString(dt.Rows[i]["AC_CODE"]);

                qry = "select SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance" +
                                " from qrygledger where AC_CODE=" + accode + " and DOC_DATE<'" + startdate + "' and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString())
                                + "  group by AC_CODE,Ac_Name_E,CityName,Mobile_No having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) !=0 order by Ac_Name_E";
                double opbalance = Convert.ToDouble(clsCommon.getString(qry));
                double Balance = 0.00;
                string lastlegdate = "";
                Balance = opbalance;
                if (TDSApplicable == "Y")
                {

                    tdsrate = Convert.ToDouble(Session["InterestRate"]);
                }
                else
                {
                    tdsrate = 0.00;
                }
                double InterestAmtDr = 0.00;
                double InterestAmtCr = 0.00;
                double tdsamt = 0.00;
                double InterestRate = 0.00;
                InterestRate = Convert.ToDouble(dt.Rows[i]["Ac_rate"]);
                if (accode != string.Empty )
                {
                    qry = "select *  from qrygledger where AC_CODE=" + accode + " and DOC_DATE between '" + startdate + "' and '" + lastdate + "' and COMPANY_CODE="
                                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE";
                }
                else
                {
                    qry = "select *  from qrygledger where  DOC_DATE between '" + startdate + "' and '" + lastdate + "' and COMPANY_CODE="
                                   + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE";
                }
                DataSet dsgleger = clsDAL.SimpleQuery(qry);

                DataTable dtleg = new DataTable();
                dtleg = dsgleger.Tables[0];
                double interest = 0.00;
                //if (dtleg.Rows.Count == 0 && opbalance!=0)
                //{
                //    qry = "select *  from qrygledger where  AC_CODE=" + accode + " and DOC_DATE <'" + startdate + "' and COMPANY_CODE="
                //                  + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by DOC_DATE";
                //    DataSet dsgleger1 = clsDAL.SimpleQuery(qry);
                //    dtleg = dsgleger1.Tables[0];
                //    dtleg.Rows[0]["DOC_DATE"] = startdate;
                //    Balance = 0;
                //}

                if (dtleg.Rows.Count > 0)
                {
                    DateTime ddt = Convert.ToDateTime(fromdate);


                    for (int j = 0; j < dtleg.Rows.Count; j++)
                    {

                        if (j != 0)
                        {
                            ddt = Convert.ToDateTime(dtleg.Rows[j - 1]["DOC_DATE"].ToString());
                            lastlegdate = dtleg.Rows[j]["DOC_DATE"].ToString();
                        }
                        string doc_date = dtleg.Rows[j]["DOC_DATE"].ToString();
                        if (dtleg.Rows.Count ==1)
                        {
                            lastlegdate = dtleg.Rows[j]["DOC_DATE"].ToString();
                        }
                        string drcr = dtleg.Rows[j]["drcr"].ToString();
                        double amount = Convert.ToDouble(dtleg.Rows[j]["amount"].ToString());
                        double cramt = 0.00;
                        double dramt = 0.00;

                        if (drcr == "C")
                        {
                            cramt = amount;

                            dramt = 0;

                        }
                        else
                        {
                            dramt = amount;

                            cramt = 0;
                        }

                        DateTime dtdoc_date = Convert.ToDateTime(doc_date);
                        TimeSpan days1 = dtdoc_date - ddt;
                        double days = Convert.ToDouble(days1.TotalDays.ToString());


                        interest = interest + (days * (Balance * InterestRate) / 365) / 100;

                        Balance = Balance + dramt - cramt;

                        //Balance = opbalance;


                        if (interest > 0)
                        {
                            InterestAmtDr = interest;
                            InterestAmtCr = 0.00;
                        }
                        else
                        {
                            InterestAmtCr = interest;
                            InterestAmtDr = 0.00;
                        }
                    }

                }
                if (tdsrate > 0)
                {
                    if (InterestAmtDr != 0)
                    {
                        tdsamt = (InterestAmtDr * tdsrate) / 100;
                    }
                    else
                    {
                        tdsamt = (InterestAmtCr * tdsrate) / 100;
                    }

                }

                if (dtleg.Rows.Count == 0)
                {
                    lastlegdate = startdate;
                }

                DateTime dtdoc_date1 = Convert.ToDateTime(todate);
                TimeSpan days11 = dtdoc_date1 - Convert.ToDateTime(lastlegdate);
                double days12 = Convert.ToDouble(days11.TotalDays.ToString());


                interest = interest + ((days12 + 1) * (Balance * InterestRate) / 365) / 100;

                if (interest > 0)
                {
                    InterestAmtDr = interest;
                    InterestAmtCr = 0.00;
                }
                else
                {
                    InterestAmtCr = interest;
                    InterestAmtDr = 0.00;
                }
                //  interest = interest + (days * (Balance * InterestRate) / 365) / 100;
                dr[0] = dt.Rows[i]["Ac_Code"];
                dr[1] = dt.Rows[i]["Ac_Name_E"];
                dr[2] = Balance;
                dr[3] = InterestRate;
                dr[4] = InterestAmtDr;
                dr[5] = InterestAmtCr;
                dr[6] = tdsrate;
                dr[7] = tdsamt;
                dtreturn.Rows.Add(dr);

            }
        }
        grddepreciation.DataSource = dtreturn;
        grddepreciation.DataBind();
        btnInterestpost.Visible = true;
    }
    protected void btnInterestpost_Click(object sender, EventArgs e)
    {
        string lastdate = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        string asondate = DateTime.Parse(lastdate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        string qry = clsCommon.getString("delete from nt_1_gledger where DOC_NO=9999 and TRAN_TYPE='IP' and COMPANY_CODE="
            + Session["Company_code"] + " and YEAR_CODE=" + Session["year"]);
        //string DepreciationAC = Session["DepreciationAC"];
        //string DepreciationAC
        if (grddepreciation.Rows.Count > 0)
        {
            string insertqry = "insert into nt_1_gledger (TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,SORT_TYPE,SORT_NO,ac) values ";
            string insertvalues = "";
            for (int i = 0; i < grddepreciation.Rows.Count; i++)
            {
                string accode = grddepreciation.Rows[i].Cells[0].Text;
                string accoid = clsCommon.getString("SELECT accoid FROM  nt_1_accountmaster WHERE AC_CODE=" + accode + " AND company_code=" + Session["Company_Code"].ToString());

                string acname = grddepreciation.Rows[i].Cells[1].Text;
                double interestamtcr = Convert.ToDouble(grddepreciation.Rows[i].Cells[5].Text);
                double interestamtdr = Convert.ToDouble(grddepreciation.Rows[i].Cells[4].Text);

                double tdsamt = Convert.ToDouble(grddepreciation.Rows[i].Cells[7].Text);
                //double befoer = Convert.ToDouble(grddepreciation.Rows[i].Cells[3].Text);
                //string narrattion = acname + ",On Amount:" + (op + befoer) + ",(DepRate:" + deprate + "),After Amount:" + after + "(" + deprate / 2 + ")";
                if (interestamtdr != 0)
                {
                    if (interestamtdr > 0)
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(interestamtdr, 0) + "," + Session["Company_Code"] +
                            "," + Session["year"] + ",1,'D','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["INTEREST_AC"].ToString() + ",''," + Math.Round(interestamtdr, 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",2,'C','IP',9999," + Session["INTEREST_ACid"].ToString() + "),";

                    }
                    else
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(Math.Abs(interestamtcr), 0) + "," + Session["Company_Code"] +
                           "," + Session["year"] + ",1,'C','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["INTEREST_AC"].ToString() + ",''," + Math.Round(Math.Abs(interestamtcr), 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",2,'D','IP',9999," + Session["INTEREST_ACid"].ToString() + "),";

                    }
                }
                if (interestamtcr != 0)
                {
                    if (interestamtdr > 0)
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(interestamtdr, 0) + "," + Session["Company_Code"] +
                            "," + Session["year"] + ",1,'D','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["INTEREST_AC"].ToString() + ",''," + Math.Round(interestamtdr, 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",2,'C','IP',9999," + Session["INTEREST_ACid"].ToString() + "),";

                    }
                    else
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(Math.Abs(interestamtcr), 0) + "," + Session["Company_Code"] +
                           "," + Session["year"] + ",1,'C','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["INTEREST_AC"].ToString() + ",''," + Math.Round(Math.Abs(interestamtcr), 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",2,'D','IP',9999," + Session["INTEREST_ACid"].ToString() + "),";

                    }
                }
                if (tdsamt != 0)
                {
                    if (tdsamt > 0)
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(tdsamt, 0) + "," + Session["Company_Code"] +
                            "," + Session["year"] + ",3,'C','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["INTEREST_AC"].ToString() + ",''," + Math.Round(interestamtdr, 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",4,'D','IP',9999," + Session["INTEREST_ACid"].ToString() + "),";

                    }
                    else
                    {
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + accode + ",''," + Math.Round(Math.Abs(tdsamt), 0) + "," + Session["Company_Code"] +
                           "," + Session["year"] + ",3,'D','IP',9999," + accoid + "),";
                        insertvalues = insertvalues + "('IP'," + "9999,'" + asondate + "'," + Session["InterestTDSAc"].ToString() + ",''," + Math.Round(Math.Abs(tdsamt), 0) + "," + Session["Company_Code"] +
                          "," + Session["year"] + ",4,'C','IP',9999," + Session["InterestTDSAcid"].ToString() + "),";

                    }
                }
            }

            string a = insertqry + insertvalues;
            a = a.Remove(a.Length - 1);

            string run = clsCommon.getString(a);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);

        }
    }
    protected void btnInterestdelete_Click(object sender, EventArgs e)
    {
        string qry = clsCommon.getString("delete from nt_1_gledger where DOC_NO=9999 and TRAN_TYPE='IP' and COMPANY_CODE="
                  + Session["Company_code"] + " and YEAR_CODE=" + Session["year"]);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Deleted !')", true);

    }
}