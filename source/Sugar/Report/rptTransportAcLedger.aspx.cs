﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptTransportAcLedger : System.Web.UI.Page
{
    string qryCommon = string.Empty;
    string tblPrefix = string.Empty;
    string cityMasterTable = string.Empty;
    string prefix = string.Empty;
    string vtype = string.Empty;
    string f = "../GSReports/TransportBalance_.htm";
    string f_Main = "../Report/TransportBalance";
    string qry = string.Empty;
    string Branch_Code = string.Empty;
    string fromDt = string.Empty;
    string toDt = string.Empty;
    string Transport_Ac = string.Empty;
    DataSet ds;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            tblPrefix = Session["tblPrefix"].ToString();
            qryCommon = "qrycommissionbill";
            cityMasterTable = tblPrefix + "CityMaster";
            fromDt = Request.QueryString["fromDT"];
            toDt = Request.QueryString["toDT"];
            Transport_Ac = Request.QueryString["Transport"];
            Branch_Code = Request.QueryString["Branch_Code"];
            lblCompanyName.Text = Session["Company_Name"].ToString();
            this.bindData();
        }
    }
    private void bindData()
    {
        try
        {
            if (Branch_Code == string.Empty)
            {
                if (Transport_Ac == string.Empty)
                {
                    qry = "select distinct(transport) as TransportCode,transportname from qrydoheaddetail where tran_type='DO' and transport!=0 and doc_date between '" + fromDt + "' and '" + toDt + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by transportname";
                }
                else
                {
                    qry = "select distinct(transport) as TransportCode,transportname from qrydoheaddetail where tran_type='DO' and transport!=0 and doc_date between '" + fromDt + "' and '" + toDt + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and voucher_by=" + Transport_Ac + " order by transportname";
                }

            }
            else
            {
                if (Transport_Ac == string.Empty)
                {
                    qry = "select distinct(transport) as TransportCode,transportname from qrydoheaddetail where tran_type='DO' and transport!=0 and doc_date between '" + fromDt + "' and '" + toDt + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " order by transportname";
                }
                else
                {
                    qry = "select distinct(transport) as TransportCode,transportname from qrydoheaddetail where tran_type='DO' and transport!=0 and doc_date between '" + fromDt + "' and '" + toDt + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Branch_Code=" + Branch_Code + " and voucher_by=" + Transport_Ac + " order by transportname";
                }

            }

            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    lblCompanyName.Text = Session["Company_Name"].ToString();
                    dtlist.DataSource = dt;
                    dtlist.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dtlist_OnitemDatabound(object sender, DataListItemEventArgs e)
    {
        try
        {
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblTransportCode = (Label)e.Item.FindControl("lblTransportCode");
            string transport = lblTransportCode.Text;
            Label lblTotalBalance = (Label)e.Item.FindControl("lblTotalBalance");

            if (Branch_Code == string.Empty)
            {
                if (Transport_Ac == string.Empty)
                {
                    qry = "select doc_no as #,Convert(varchar(5),doc_date,103) as dt,LEFT(a.Ac_Name_E,33) as VoucherBy,millShortName as MillShort,truck_no as lorry,quantal as Qntl,Freight_RateMM as Rate," +
                        " Freight_AmountMM as Freight,Paid_Amount1 as Paid1,Paid_Amount2 as Paid2,Paid_Amount3 as Paid3 from qrydoheaddetail d left outer join " + tblPrefix + "AccountMaster a on d.Ac_Code=a.Ac_Code and d.company_code=a.Company_Code where tran_type='DO' and transport!=0 " +
                        " and d.transport=" + transport + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }
                else
                {
                    qry = "select doc_no as #,Convert(varchar(5),doc_date,103) as dt,LEFT(a.Ac_Name_E,33) as VoucherBy,millShortName as MillShort,truck_no as lorry,quantal as Qntl,Freight_RateMM as Rate," +
                        " Freight_AmountMM as Freight,Paid_Amount1 as Paid1,Paid_Amount2 as Paid2,Paid_Amount3 as Paid3 from qrydoheaddetail d left outer join " + tblPrefix + "AccountMaster a on d.Ac_Code=a.Ac_Code and d.company_code=a.Company_Code where tran_type='DO' and transport!=0 " +
                        " and d.transport=" + transport + " and d.voucher_by=" + Transport_Ac + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                }
            }
            else
            {
                if (Transport_Ac == string.Empty)
                {
                    qry = "select doc_no as #,Convert(varchar(5),doc_date,103) as dt,LEFT(a.Ac_Name_E,33) as VoucherBy,millShortName as MillShort,truck_no as lorry,quantal as Qntl,Freight_RateMM as Rate," +
                                       " Freight_AmountMM as Freight,Paid_Amount1 as Paid1,Paid_Amount2 as Paid2,Paid_Amount3 as Paid3 from qrydoheaddetail d left outer join " + tblPrefix + "AccountMaster a on d.Ac_Code=a.Ac_Code and d.company_code=a.Company_Code where tran_type='DO' and transport!=0 " +
                                       " and d.transport=" + transport + " and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + "";
                }
                else
                {
                    qry = "select doc_no as #,Convert(varchar(5),doc_date,103) as dt,LEFT(a.Ac_Name_E,33) as VoucherBy,millShortName as MillShort,truck_no as lorry,quantal as Qntl,Freight_RateMM as Rate," +
                                      " Freight_AmountMM as Freight,Paid_Amount1 as Paid1,Paid_Amount2 as Paid2,Paid_Amount3 as Paid3 from qrydoheaddetail d left outer join " + tblPrefix + "AccountMaster a on d.Ac_Code=a.Ac_Code and d.company_code=a.Company_Code where tran_type='DO' and transport!=0 " +
                                      " and d.transport=" + transport + " and d.voucher_by=" + Transport_Ac + "  and d.doc_date between '" + fromDt + "' and '" + toDt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and d.Branch_Code=" + Branch_Code + "";
                }
            }
            ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("dt", typeof(string)));
                dt.Columns.Add(new DataColumn("VoucherBy", typeof(string)));
                dt.Columns.Add(new DataColumn("MillShort", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(string)));
                dt.Columns.Add(new DataColumn("Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("Freight", typeof(string)));
                dt.Columns.Add(new DataColumn("Paid1", typeof(string)));
                dt.Columns.Add(new DataColumn("Paid2", typeof(string)));
                dt.Columns.Add(new DataColumn("Paid3", typeof(string)));
                dt.Columns.Add(new DataColumn("balance", typeof(double)));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = ds.Tables[0].Rows[i]["#"].ToString();
                    dr["dt"] = ds.Tables[0].Rows[i]["dt"].ToString();
                    dr["VoucherBy"] = ds.Tables[0].Rows[i]["VoucherBy"].ToString();
                    dr["MillShort"] = ds.Tables[0].Rows[i]["MillShort"].ToString();
                    dr["lorry"] = ds.Tables[0].Rows[i]["lorry"].ToString();
                    dr["Qntl"] = ds.Tables[0].Rows[i]["Qntl"].ToString();
                    dr["Rate"] = ds.Tables[0].Rows[i]["Rate"].ToString();
                    string frtAmt = ds.Tables[0].Rows[i]["Freight"].ToString();
                    double freightAmt = frtAmt != string.Empty ? Convert.ToDouble(frtAmt) : 0.00;
                    dr["Freight"] = freightAmt;
                    string paid1 = ds.Tables[0].Rows[i]["Paid1"].ToString();
                    if (paid1 == "0.00")
                    {
                        dr["Paid1"] = "";
                    }
                    else
                    {
                        dr["Paid1"] = paid1;
                    }
                    string paid2 = ds.Tables[0].Rows[i]["Paid2"].ToString();
                    if (paid2 == "0.00")
                    {
                        dr["Paid2"] = "";
                    }
                    else
                    {
                        dr["Paid2"] = paid2;
                    }
                    string paid3 = ds.Tables[0].Rows[i]["Paid3"].ToString();
                    if (paid3 == "0.00")
                    {
                        dr["Paid3"] = "";
                    }
                    else
                    {
                        dr["Paid3"] = paid3;
                    }
                    double p1 = paid1 != string.Empty ? Convert.ToDouble(paid1) : 0.00;
                    double p2 = paid2 != string.Empty ? Convert.ToDouble(paid2) : 0.00;
                    double p3 = paid3 != string.Empty ? Convert.ToDouble(paid3) : 0.00;

                    double balance = freightAmt - (p1 + p2 + p3);
                    dr["balance"] = balance;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblTotalBalance.Text = Convert.ToString(dt.Compute("SUM(balance)", string.Empty));
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        pnlMain.RenderControl(tw);
        string sim = sw.ToString();
        StrExport.Append(sim);
        StrExport.Append("</div></body></html>");
        string strFile = "report.xls";
        string strcontentType = "application/excel";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.BufferOutput = true;
        Response.ContentType = strcontentType;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        Response.Write(StrExport.ToString());
        Response.Flush();
        Response.Close();
        Response.End();
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {

    }
}