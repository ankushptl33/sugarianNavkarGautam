﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sugar_Payments_PgePaymentsUtility : System.Web.UI.Page
{
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null;
    string cs = string.Empty;
    string qryCommon = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        qryCommon = "";
        cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        con = new SqlConnection(cs);
        if (!IsPostBack)
        {
            BindDummyRow();
        }
        SetFocus(btnAdd);
    }

    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("doc_no");
        dummy.Columns.Add("tran_type");
        dummy.Columns.Add("doc_date");
        dummy.Columns.Add("Ac_Name_E");
        dummy.Columns.Add("amount");
        dummy.Columns.Add("credit_ac");
        dummy.Columns.Add("Shortcreditname");
        //dummy.Columns.Add("narration");
        dummy.Columns.Add("tranid");

        dummy.Rows.Add();
        gvCustomers.DataSource = dummy;
        gvCustomers.DataBind();
    }

    [WebMethod]
    public static string GetCustomers(string searchTerm, int pageIndex, string Trantype, int PageSize, int Company_Code, int year)
    {
        string searchtxt = "";
        string delimStr = "";
        char[] delimiter = delimStr.ToCharArray();
        string words = "";
        string[] split = null;
        string name = string.Empty;

        searchtxt = searchTerm;
        words = searchTerm;
        split = words.Split(delimiter);
        foreach (var s in split)
        {
            string aa = s.ToString();
            // name += "Doc_No Like '%" + aa + "%'or";
            //name += "( Ac_Name_E like '%" + aa + "%' or narration like '%" + aa + "%' or doc_dateConverted like '%" + aa + "%' or amount like '%" + aa + "%' or tranid like '%" + aa + "%' or doc_dateConverted like '%" + aa + "%' ) and";
            name += "( am1.Ac_Name_E like '%" + aa + "%' or am2.Ac_Name_E like '%" + aa + "%' or ph.date like '%" + aa +
                "%' or ph.amount like '%" + aa + "%' or ph.id like '%" + aa + "%') and";
        }
        name = name.Remove(name.Length - 3);

        // string query = "SELECT ROW_NUMBER() OVER ( order by doc_no ASC) AS RowNumber,doc_no,tran_type,doc_dateConverted as doc_date,"
        //+ "BankName as Ac_Name_E ,amount,credit_ac,Shortcreditname,narration,tranid FROM qrytransheaddetail    where " + name + " and tran_type='" + Trantype + "' and Company_Code=" + Company_Code + " " +
        //" and Year_Code=" + year + " order by doc_no desc,doc_date desc";

        string query = "SELECT " +
                    "ROW_NUMBER() OVER (ORDER BY ph.doc_no DESC, ph.date DESC) AS RowNumber, " +
                    "ph.doc_no, " +
                    "ph.tran_type, " +
                    "CONVERT(varchar(10), ph.date, 103) AS doc_date, " +
                    "am2.Ac_Name_E AS Ac_Name_E, " +
                    "ph.amount, " +
                    "ph.tds_percent, " +
                    "ph.tds_amount, " +
                    "ph.payment_to, " +
                    "am1.Ac_Code as credit_ac, " +
                    "am1.Short_Name AS Shortcreditname, " +
                    "ph.id AS tranid " +
                    "FROM " +
                    "dbo.PaymentHead AS ph " +
                    "LEFT OUTER JOIN dbo.nt_1_accountmaster AS am1 ON am1.Ac_Code = ph.payment_to " +
                    "LEFT OUTER JOIN dbo.nt_1_accountmaster AS am2 ON ph.bank_code = am2.Ac_Code " +
                    "where " + name + " and ph.tran_type='" + Trantype + "' and ph.Company_Code=" + Company_Code + " and ph.Year_Code=" + year +
                    " order by doc_no desc,date desc";



        SqlCommand cmd = new SqlCommand(query);
        cmd.CommandType = CommandType.Text;
        return GetData(cmd, pageIndex, PageSize).GetXml();
    }
    private static DataSet GetData(SqlCommand cmd, int pageIndex, int PageSize)
    {

        string RecordCount = "";
        string cs1 = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;

        using (SqlConnection con = new SqlConnection(cs1))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {

                cmd.Connection = con;
                sda.SelectCommand = cmd;
                DataSet dsreturn = new DataSet();
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    int number = 1;
                    DataTable dtnew = new DataTable();
                    dtnew = ds.Tables[0];
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        dtnew.Rows[i][0] = number;
                        number = number + 1;

                    }
                    string f1 = " RowNumber >=(" + pageIndex + " -1) * (" + PageSize + "+1) and RowNumber<=";
                    string f2 = "(((" + pageIndex + " -1) * " + PageSize + " +1) +" + PageSize + ")-1";

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "doc_no desc");
                    if (results.Count() > 0)
                    {
                        DataTable dt1 = results.CopyToDataTable();
                        dt1.TableName = "Customers";
                        DataTable dt = new DataTable("Pager");
                        dt.Columns.Add("PageIndex");
                        dt.Columns.Add("PageSize");
                        dt.Columns.Add("RecordCount");
                        dt.Rows.Add();
                        RecordCount = ds.Tables[0].Rows.Count.ToString();

                        dt.Rows[0]["PageIndex"] = pageIndex;
                        dt.Rows[0]["PageSize"] = PageSize;
                        dt.Rows[0]["RecordCount"] = RecordCount;

                        dsreturn = new DataSet();
                        dsreturn.Tables.Add(dt1);
                        dsreturn.Tables.Add(dt);
                        return dsreturn;
                    }
                    else
                    {
                        return dsreturn;
                    }

                }
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        // DataSet ds = new DataSet();
        // DataTable dt = new DataTable();
        // string qry = string.Empty;
        // qry = " select doc_no,tran_type,doc_dateConverted as doc_date,"
        //+ "Ac_Name_E ,amount,narration,tranid FROM " + qryCommon + " limit 15";
        // ds = clsDAL.SimpleQuery(qry);
        // if (ds != null)
        // {
        //     dt = ds.Tables[0];
        //     if (dt.Rows.Count > 0)
        //     {
        //         gvCustomers.DataSource = dt;
        //         gvCustomers.DataBind();
        //         ViewState["currentTable"] = dt;
        //     }
        // }
        // else
        // {
        //     gvCustomers.DataSource = null;
        //     gvCustomers.DataBind();
        //     ViewState["currentTable"] = null;
        // }
    }
}