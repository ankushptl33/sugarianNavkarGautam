using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Device.Location;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

public partial class pgeHome : System.Web.UI.Page
{
    string mobileNumber = string.Empty;
    string user = string.Empty;
    string User_Type = string.Empty;
    string pcbiossrno = string.Empty;
    string pcusername = string.Empty;
    string pcmac = string.Empty;
    string dbbiossrno = string.Empty;
    string dbusername = string.Empty;
    string dbmac = string.Empty;
    DataTable tempOTP = null;
    DataRow dr;
    string way = "";
    string throughWa = string.Empty;
    string EmailAdmin = string.Empty;

    string clientipN = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        user = Session["user"].ToString();
        clsMAC mac = new clsMAC();
        pcmac = mac.GetMACAddress();
        pcusername = mac.HostName();
        pcbiossrno = HardwareInfo.GetBIOSserNo();
      

       
    }

    private void Companyname()
    {
        #region set company name
        try
        {
            string companyCode = Session["Company_Code"].ToString();

            string qry = "";


            string Company_Name_E = clsCommon.getString("select Company_Name_E from Company where Company_Code='" + companyCode + "'");

            Label lbl = (Label)Master.FindControl("lblCompanyName");
            lbl.Text = Company_Name_E;
        #endregion

            #region generate company wise table prefix

            string companyPrefix = string.Empty;

            if (Company_Name_E != string.Empty)
            {
                string[] initials = Company_Name_E.Split(' ');
                for (int i = 0; i < initials.Length; i++)
                {
                    companyPrefix = companyPrefix + initials[i].Substring(0, 1);
                }
                companyPrefix = companyPrefix + "_" + companyCode + "_";

                Session["tblPrefix"] = companyPrefix;
                Session["tblPrefix"] = "NT_1_";
                //generate company wise view
                clsCommon.generateView(companyCode);
            }
            #endregion
        }
        catch
        {
            //company not selected  Go to startup page to view the list of all companies
        }
    }
    protected string HideMobileNumber()
    {
        mobileNumber = clsCommon.getString("select Mobile_NoWa from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        if (!string.IsNullOrEmpty(mobileNumber) && mobileNumber.Length >= 2)
        {
            string lastTwoDigits = mobileNumber.Substring(mobileNumber.Length - 2);
            string hiddenDigits = new string('*', mobileNumber.Length - 2);
             mobileNumber = hiddenDigits + lastTwoDigits;
            return mobileNumber;
        }
        return mobileNumber;
     

    }

    protected string HideEmailId()
    {
        EmailAdmin = clsCommon.getString("Select EmailId from tblUser where User_Name='" + user.Trim() + "'");

        //if (!string.IsNullOrEmpty(EmailAdmin) && EmailAdmin.Length >= 2)
        //{
        //    string lastTwoDigits = EmailAdmin.Substring(EmailAdmin.Length - 2);
        //    string hiddenDigits = new string('*', EmailAdmin.Length - 2); 
        //    EmailAdmin =  hiddenDigits + lastTwoDigits;
        //    return EmailAdmin;
        //}

        int atIndex = EmailAdmin.IndexOf('@');
        if (atIndex <= 1)
        {
            return EmailAdmin;
        }

        string maskedPart = new string('*', atIndex - 1);
        string domain = EmailAdmin.Substring(atIndex);
        string newdomain = EmailAdmin.Substring(atIndex - 2);
        EmailAdmin = EmailAdmin[atIndex - 16] + maskedPart + newdomain;
          
        return EmailAdmin;
    }

     
    protected void btnAll_Click(object sender, EventArgs e)
    {
        string Datefrm = DateTime.Parse(clsGV.To_date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        string fromDT = Session["Start_Date"].ToString();
        string toDT = Session["End_Date"].ToString();
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "D", "javascript:DO()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "E", "javascript:TP()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "F", "javascript:GL()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "G", "javascript:MP('" + Datefrm + "','" + Datefrm + "','')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "B", "javascript:Ac('" + fromDT + "','" + toDT + "','')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "H", "javascript:DD('" + Datefrm + "','" + Datefrm + "','" + Branch_Code + "')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "A", "javascript:report('" + fromDT + "','" + toDT + "','')", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "C", "javascript:Account()", true);

    }
    protected void rblist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception)
        {
            throw;
        }
    }
      

    private string IpAddress()
    {
        string strIpAddress;
        strIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (strIpAddress == null)
            strIpAddress = Request.ServerVariables["REMOTE_ADDR"];
        return strIpAddress;
    }

    protected void btnMB_Click(object sender, EventArgs e)
    {
        MB.Show("Ankush");
    }

    protected void btnBWSPA_Click(object sender, EventArgs e)
    {
        try
        {
            string fromDT = Session["Start_Date"].ToString();
            string toDT = Session["End_Date"].ToString();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ku", "javascript:bwlpa('" + fromDT + "','" + toDT + "','')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btndatabasebackup_Click(object sender, EventArgs e)
    {
        #region
        //string conn = ConfigurationManager.ConnectionStrings["sqlconnection"].ToString();
        //SqlConnection sqlconn = new SqlConnection(conn);
        //SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter da = new SqlDataAdapter();
        //DataTable dt = new DataTable();



        //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conn);

        //string database = builder.InitialCatalog;
        //// Backup destination
        //string backupDestination = "D:\\SQLBackUpFolder";
        //// check if backup folder exist, otherwise create it.
        //if (!System.IO.Directory.Exists(backupDestination))
        //{
        //    System.IO.Directory.CreateDirectory("D:\\SQLBackUpFolder");
        //}
        //try
        //{
        //    sqlconn.Open();
        //    sqlcmd = new SqlCommand("backup database " + database + " to disk='" + backupDestination + "\\" + "Sugarian" + DateTime.Now.ToString("ddMMyyyy_HHmmss_") + user + ".Bak'", sqlconn);
        //    sqlcmd.ExecuteNonQuery();
        //    //Close connection
        //    sqlconn.Close();
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Successfully Backup Database');", true);
        //}
        //catch (Exception ex)
        //{
        //    Response.Write(ex.Message);
        //}
        #endregion
        string conn = ConfigurationManager.ConnectionStrings["sqlconnection"].ToString();
        SqlConnection sqlconn = new SqlConnection(conn);
        SqlCommand sqlcmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();



        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conn);

        string database = builder.InitialCatalog;
        // Backup destination
        string backupDrive = clsCommon.getString("select dbbackup from tblvoucherheadaddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ");
        //string backupDestination = backupDrive+":\\SQLBackUpFolder";
        string backupDestination = backupDrive;
        // check if backup folder exist, otherwise create it.
        if (!System.IO.Directory.Exists(backupDestination))
        {
            System.IO.Directory.CreateDirectory(backupDrive);
        }
        try
        {
            sqlconn.Open();
            sqlcmd = new SqlCommand("backup database " + database + " to disk='" + backupDestination + "\\" + "Sugarian" + DateTime.Now.ToString("ddMMyyyy_HHmmss_") + user + ".Bak'", sqlconn);
            sqlcmd.ExecuteNonQuery();
            //Close connection
            sqlconn.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Successfully Backup Database');", true);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

    }

      
}