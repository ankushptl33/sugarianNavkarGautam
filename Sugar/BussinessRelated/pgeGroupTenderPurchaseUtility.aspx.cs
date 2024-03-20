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

public partial class Sugar_BussinessRelated_pgeGroupTenderPurchaseUtility : System.Web.UI.Page
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
        dummy.Columns.Add("Tender_No");
        dummy.Columns.Add("Tender_dateConverted");
        dummy.Columns.Add("GroupName");
        dummy.Columns.Add("millShortName");
        dummy.Columns.Add("Quantal");
        dummy.Columns.Add("Mill_Rate");
        dummy.Columns.Add("Grade");
        dummy.Columns.Add("ourTenderNo");
        dummy.Columns.Add("Buyer_Quantal");
        dummy.Columns.Add("isAccounted");
        dummy.Columns.Add("grouptenderid");
        dummy.Columns.Add("IsDeleted");
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
            name += "( millShortName like '%" + aa + "%' or Tender_No like '%" + aa + "%' or Grade like '%" + aa +
                "%' or  GroupName  like '%" + aa + "%' or grouptenderid like '%" + aa + "%' or Buyer_Quantal like '%" + aa + "%' or Mill_Rate like '%" + aa + "%' or ourTenderNo like '%" + aa + "%' ) and";

        }
        name = name.Remove(name.Length - 3);


        //string query = "SELECT ROW_NUMBER() OVER ( order by dbo.GroupTenderPurchase.Tender_No desc) AS RowNumber,     dbo.GroupTenderPurchase.Tender_No, CONVERT(varchar(10), dbo.GroupTenderPurchase.Tender_Date, 103) AS Tender_dateConverted, mill.Short_Name AS millName, " +
        //             " dbo.GroupTenderPurchase.Grade, dbo.GroupTenderPurchase.Quantal, dbo.GroupTenderPurchase.grouptenderid, dbo.GroupCreactionMaster.GroupName " +
        //              " FROM         dbo.GroupTenderPurchase LEFT OUTER JOIN " +
        //              " dbo.GroupCreactionMaster ON dbo.GroupTenderPurchase.gid = dbo.GroupCreactionMaster.autoid LEFT OUTER JOIN " +
        //              " dbo.qrymstaccountmaster AS mill ON dbo.GroupTenderPurchase.mc = mill.accoid AND dbo.GroupTenderPurchase.Company_Code = mill.Company_Code " +
        //             " where " + name + " and dbo.GroupTenderPurchase.Company_Code=" + Company_Code + "  order by Tender_no desc ";

        string query = " select  ROW_NUMBER() OVER ( order by Tender_No desc) AS RowNumber,  Tender_No,Tender_dateConverted,GroupName,millShortName,Quantal,Mill_Rate,Grade,isAccounted,Buyer_Quantal,grouptenderid,ourTenderNo,IsDeleted " +
                       " from qryGroupTenderHeadDetail " +
                        " where " + name + " and membercode=2 and Company_Code=" + Company_Code + "  order by Tender_no desc ";
                       

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

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "Tender_No desc");
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