using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Threading;
using System.Data.SqlClient;

public partial class Sugar_BussinessRelated_pgeGroupDispatchUtility : System.Web.UI.Page
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
        dummy.Columns.Add("Doc_no");
        dummy.Columns.Add("doc_dateConverted");
        dummy.Columns.Add("purchaseCode");
        dummy.Columns.Add("purchaseNmber");
        dummy.Columns.Add("MillShort_Name");
        dummy.Columns.Add("SBShort_Name");
        dummy.Columns.Add("quantal");
        dummy.Columns.Add("millRate");
        dummy.Columns.Add("saleRate");
        dummy.Columns.Add("dispatchId");
        dummy.Rows.Add();
        gvCustomers.DataSource = dummy;
        gvCustomers.DataBind();
    }
    [WebMethod]
    public static string GetCustomers(string searchTerm, int pageIndex, int PageSize, int Company_Code, int year)
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
            name += "( qrySalebillTo.Short_Name like '%" + aa + "%' or dbo.GroupDispatch.Doc_no like '%" + aa + "%' or dbo.GroupDispatch.quantal like '%" + aa +
                "%' or qrymill.Short_Name  like '%" + aa + "%' or dbo.GroupDispatch.dispatchId like '%" + aa + "%' ) and";

        }
        name = name.Remove(name.Length - 3);


        string query = "SELECT ROW_NUMBER() OVER ( order by dbo.GroupDispatch.Doc_no desc) AS RowNumber,  dbo.GroupDispatch.Doc_no , dbo.GroupDispatch.Doc_Date, dbo.GroupDispatch.MillCode, dbo.GroupDispatch.purchaseCode, dbo.GroupDispatch.purchaseNmber, " +
                      " dbo.GroupDispatch.saleBillTo, dbo.GroupDispatch.saleBillToId, " +
                      " dbo.GroupDispatch.quantal, dbo.GroupDispatch.millRate, dbo.GroupDispatch.saleRate, dbo.GroupDispatch.Company_Code, dbo.GroupDispatch.Year_Code, dbo.GroupDispatch.dispatchId, " +
                      " dbo.GroupDispatch.tenderdetailid, qrySalebillTo.cityname AS SaleBillToCityName, CONVERT(varchar(10), dbo.GroupDispatch.Doc_Date, 103) AS doc_dateConverted, " +
                      " qrySalebillTo.Short_Name AS SBShort_Name, qrymill.Short_Name AS MillShort_Name " +
                      " FROM         dbo.GroupDispatch LEFT OUTER JOIN " +
                      " dbo.qrymstaccountmaster AS qrySalebillTo ON dbo.GroupDispatch.Company_Code = qrySalebillTo.Company_Code AND dbo.GroupDispatch.saleBillToId = qrySalebillTo.accoid LEFT OUTER JOIN " +
                      " dbo.qrymstaccountmaster AS qrymill ON dbo.GroupDispatch.mc = qrymill.accoid AND dbo.GroupDispatch.Company_Code = qrymill.Company_Code" +
                     " where " + name + " and dbo.GroupDispatch.Company_Code=" + Company_Code + "  order by Doc_no desc ";


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

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "Doc_no desc");
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

    }
}