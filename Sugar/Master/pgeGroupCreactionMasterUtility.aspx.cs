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
using System.Globalization;


public partial class Sugar_Master_pgeGroupCreactionMasterUtility : System.Web.UI.Page
{
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null;
    string cs = string.Empty;
    string qryCommon = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        qryCommon = "qryPaymentNote";
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
        dummy.Columns.Add("Doc_No");
        dummy.Columns.Add("GroupName");
        dummy.Columns.Add("MemberName1");
        dummy.Columns.Add("MemberName2");
        dummy.Columns.Add("MemberName3");
        dummy.Columns.Add("MemberName4");
        dummy.Columns.Add("MemberName5");
        dummy.Columns.Add("MemberName6");
        dummy.Columns.Add("MemberName7");
        dummy.Columns.Add("autoid");

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
            name += "( dbo.GroupCreactionMaster.Doc_No like '%" + aa + "%' or dbo.GroupCreactionMaster.GroupName like '%" + aa + "%' or dbo.GroupCreactionMaster.autoid like '%" + aa + "%') and";


        }
        name = name.Remove(name.Length - 3);

        string query = "SELECT ROW_NUMBER() OVER ( order by dbo.GroupCreactionMaster.Doc_No ASC) AS RowNumber, dbo.GroupCreactionMaster.Doc_No, dbo.GroupCreactionMaster.GroupName, " +
                       " dbo.GroupCreactionMaster.autoid, member1.Short_Name AS MemberName1, member2.Short_Name AS MemberName2, member3.Short_Name AS MemberName3," +
                      " member4.Short_Name AS MemberName4, member5.Short_Name AS MemberName5, member6.Short_Name AS MemberName6, member7.Short_Name AS MemberName7 " +
                      " FROM         dbo.GroupCreactionMaster LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member7 ON dbo.GroupCreactionMaster.id7 = member7.accoid AND dbo.GroupCreactionMaster.Company_Code = member7.Company_Code LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member6 ON dbo.GroupCreactionMaster.Company_Code = member6.Company_Code AND dbo.GroupCreactionMaster.id6 = member6.accoid LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member5 ON dbo.GroupCreactionMaster.id5 = member5.accoid AND dbo.GroupCreactionMaster.Company_Code = member5.Company_Code LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member4 ON dbo.GroupCreactionMaster.id4 = member4.accoid AND dbo.GroupCreactionMaster.Company_Code = member4.Company_Code LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member3 ON dbo.GroupCreactionMaster.id3 = member3.accoid AND dbo.GroupCreactionMaster.Company_Code = member3.Company_Code LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member2 ON dbo.GroupCreactionMaster.id2 = member2.accoid AND dbo.GroupCreactionMaster.Company_Code = member2.Company_Code LEFT OUTER JOIN" +
                      " dbo.qrymstaccountmaster AS member1 ON dbo.GroupCreactionMaster.Company_Code = member1.Company_Code AND dbo.GroupCreactionMaster.id1 = member1.accoid" +
       " where " + name + " and dbo.GroupCreactionMaster.Company_Code=" + Company_Code + " " +
       "  order by dbo.GroupCreactionMaster.Doc_No desc";

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

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "Doc_No desc");
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