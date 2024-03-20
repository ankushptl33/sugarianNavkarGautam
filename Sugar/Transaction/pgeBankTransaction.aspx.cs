using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

public partial class Sugar_Transaction_pgeBankTransaction : System.Web.UI.Page
{
    #region data section
    string qry = string.Empty;
    string qry1 = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    string Trans_Date;
    string CGST_Rate;
    string SGST_Rate;
    string IGST_Rate;
    string Dono;
    string State_Code_BillTo;
    string State_Code_Mill;
    int Rowaction = 2;
    int Srno = 3;

    string carporateSale = string.Empty;
    string type = "";
    string AutoID = "";
    string selectedyear = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        Dono = Request.QueryString["dono"];
        AutoID = Request.QueryString["ID"];
        try
        {

            tblPrefix = Session["tblPrefix"].ToString();
            user = Session["user"].ToString();
            tblHead = "BankTranction";
            tblDetails = tblPrefix + "";
            string isAuthenticate = string.Empty;
            string Action = string.Empty;

            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {

                    ViewState["currentTable"] = null;
                    clsButtonNavigation.enableDisable("N");
                    this.makeEmptyForm("N");
                    BindData();
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
                ViewState["currentTable"] = null;

                // txtVehicle_No.Enabled = false;
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

                //  txtVehicle_No.Enabled = true;
                #region set Business logic for save
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

                // txtVehicle_No.Enabled = false;
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

                // txtVehicle_No.Enabled = true;
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
    }

    #endregion



    protected void btnPay_Click(object sender, EventArgs e)
    {
    }

    private void BindData()
    {
        try
        {
            string qrydata = string.Empty;
            if (AutoID != string.Empty)
            {
                qrydata = " SELECT AcCode,Account_Name,Name,AccountNumber,IFSC,Amount,Remark,Narration from  qryBankTranction where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                     " and doc_no=" + Dono + " and Id=" + AutoID + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            }
            else { 
            
            }
              DataSet ds = new DataSet();
              ds = clsDAL.SimpleQuery(qrydata);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtAcCode.Text = dt.Rows[0]["AcCode"].ToString();
                        lblAcName.Text = dt.Rows[0]["Account_Name"].ToString();
                        txtName.Text = dt.Rows[0]["Name"].ToString();
                        txtAccountNumber.Text = dt.Rows[0]["AccountNumber"].ToString();
                        txtIFSCCode.Text = dt.Rows[0]["IFSC"].ToString();
                        txtamount.Text = dt.Rows[0]["Amount"].ToString();
                        txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                        txtnarration.Text = dt.Rows[0]["Narration"].ToString();
                    }
                }
            }
        }
        catch
        {
        }
    }
}