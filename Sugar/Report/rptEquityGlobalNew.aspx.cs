using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using System.Drawing.Printing;

using System.Net;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using iTextSharp.tool.xml;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Threading;


public partial class Sugar_Report_rptEquityGlobalNew : System.Web.UI.Page
{
    string fromdt = string.Empty;
    string todt = string.Empty;
    string mail = string.Empty;
    int company_code;
    int year_code;
    string unitno = string.Empty;
    string Accode = string.Empty;
    string company_name = string.Empty;
    string Email = string.Empty;
    string State = string.Empty;
    string Address = string.Empty;
    ReportDocument rprt1 = new ReportDocument();
    string datefrom = "";
    string dateto = "";
    string pagehead = string.Empty;

    string tblPrefix = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    int count = 0;
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null; 
    string cs = string.Empty;
    int Script_Code = 0;
    int Qty = 0;
    int Bonus = 0;
    Double PNL = 0.00; 
    Double Value = 0.00;
    DataTable dt = null;
    DataSet ds = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            company_code = Convert.ToInt32(Session["Company_Code"].ToString());
            year_code = Convert.ToInt32(Session["year"].ToString());
            fromdt = Request.QueryString["fromdt"];
            todt = Request.QueryString["todt"];

            datefrom = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            dateto = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            String qry = "update Script_Master set OpQty=0, OpBonus=0,OpValue=0,PurcQty=0,PurcBonus=0," +
                             " PurcValue=0,SaleQty=0,SaleBonus=0,SaleValue=0,Profit=0," +
                             " Loss=0,ClosingQty=0,ClosingBonus=0,ClosingVale=0,ClosingRate=0,Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +"";
            Thread thred = new Thread(() => { count = DataStore(qry); });
            thred.Start();
            thred.Join();
            // Add Purchase Records
            String qry1 = "select SUM(Qty) as Q,SUM(Bonus) as B , SUM(Value) as V, Script_Code from EquityPurchaseSale where Tran_Type='P' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                          " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and [Date]>= '" + datefrom + "' group by Script_Code";
            ds = clsDAL.SimpleQuery(qry1);
            dt = new DataTable();
            dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Script_Code = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());
                Qty = Convert.ToInt32(dt.Rows[i]["Q"].ToString());
                Bonus = Convert.ToInt32(dt.Rows[i]["B"].ToString());
                Value = Convert.ToDouble(dt.Rows[i]["V"].ToString());
                qry1 = "Update Script_Master set PurcQty= " + Qty + " , PurcBonus=" + Bonus + ", PurcValue=" + Value + " where FNO_Script_No=" + Script_Code;
                Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                thred2.Start();
                thred2.Join();
            }

            // Add Sale Records
            qry1 = "select SUM(Qty) as Q,SUM(Bonus) as B , SUM(Value) as V, Script_Code from EquityPurchaseSale where Tran_Type='S' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                         " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and [Date]>= '" + datefrom + "' group by Script_Code";
            ds = clsDAL.SimpleQuery(qry1);
            dt = new DataTable();
            dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Script_Code = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());
                Qty = Convert.ToInt32(dt.Rows[i]["Q"].ToString());
                Bonus = Convert.ToInt32(dt.Rows[i]["B"].ToString());
                Value = Convert.ToDouble(dt.Rows[i]["V"].ToString());
                qry1 = "Update Script_Master set SaleQty= " + Qty + " , SaleBonus=" + Bonus + ", SaleValue=" + Value + " where FNO_Script_No=" + Script_Code;
                Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                thred2.Start();
                thred2.Join();
            }
            // Update Profit And Loss
            int BREAK1 = 0;
            qry1 = "SELECT  dbo.qryEquitysale.Year_Code, dbo.qryEquitysale.Doc_No, dbo.qryEquitysale.Date, dbo.qryEquitysale.Script_Code, dbo.qryEquitysale.Qty, dbo.qryEquitysale.Bonus, " +
                    " dbo.qryEquitysale.Rate, dbo.qryEquitysale.Value, dbo.qryEquityPurchase.Rate AS PurcRate, dbo.qryEquitysale.Qty * dbo.qryEquityPurchase.Rate AS PurcValue, " +
                    " dbo.qryEquitysale.Value - dbo.qryEquitysale.Qty * dbo.qryEquityPurchase.Rate AS PROFIT" +
                    " FROM         dbo.qryEquitysale INNER JOIN" +
                    " dbo.qryEquityPurchase ON dbo.qryEquitysale.Purc_No = dbo.qryEquityPurchase.Doc_No AND dbo.qryEquitysale.Purc_Company_code = dbo.qryEquityPurchase.Company_Code AND " +
                    " dbo.qryEquitysale.Purc_Year_Code = dbo.qryEquityPurchase.Year_Code" +
                    " WHERE dbo.qryEquitysale.Year_Code = " + Convert.ToInt32(Session["year"].ToString()) + " and dbo.qryEquitysale.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "   ORDER BY dbo.qryEquitysale.Script_Code";
            ds = clsDAL.SimpleQuery(qry1);
            dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count != 0)
            {

                BREAK1 = Convert.ToInt32(dt.Rows[0]["Script_Code"].ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (BREAK1 == Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString()))
                    {

                        PNL = PNL + Convert.ToDouble(dt.Rows[i]["PROFIT"].ToString());
                        BREAK1 = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());

                    }
                    else
                    {
                        if (PNL > 0)
                        {
                            qry1 = "Update Script_Master set Profit= " + PNL + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[i - 1]["Script_Code"].ToString());
                            Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                            thred2.Start();
                            thred2.Join();
                            PNL = 0;
                            PNL = PNL + Convert.ToDouble(dt.Rows[i]["PROFIT"].ToString());
                            BREAK1 = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());
                        }
                        else
                        {

                            qry1 = "Update Script_Master set Loss= " + Math.Abs(PNL) + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[i - 1]["Script_Code"].ToString());
                            Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                            thred2.Start();
                            thred2.Join();
                            PNL = 0;
                            PNL = PNL + Convert.ToDouble(dt.Rows[i]["PROFIT"].ToString());
                            BREAK1 = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());
                        }
                    }
                }


                //Saving Last record
                if (PNL > 0)
                {
                    qry1 = "Update Script_Master set Profit= " + PNL + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Script_Code"].ToString());
                    Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                    thred2.Start();
                    thred2.Join();
                }
                else
                {

                    qry1 = "Update Script_Master set Loss= " + Math.Abs(PNL) + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Script_Code"].ToString());
                    Thread thred2 = new Thread(() => { count = DataStore(qry1); });
                    thred2.Start();
                    thred2.Join();

                }

            }


            //creating opening records
            qry1 = " Delete From EquityPurchaseSaleDummy";
            Thread thred3 = new Thread(() => { count = DataStore(qry1); });
            thred3.Start();
            thred3.Join();
            qry1 = " insert into   EquityPurchaseSaleDummy select * from EquityPurchaseSale where Tran_Type='S' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code <" + Convert.ToInt32(Session["year"].ToString());
            Thread thred4 = new Thread(() => { count = DataStore(qry1); });
            thred4.Start();
            thred4.Join();


            int myYear_Code = Convert.ToInt32(Session["year"].ToString()) - 1;

            qry1 = "select * from qryEquityBalanceDummy where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code =" + myYear_Code + " order by script_code";
            ds = clsDAL.SimpleQuery(qry1);
            dt = new DataTable();
            dt = ds.Tables[0];
            BREAK1 = 0;
            Value = 0;
            Qty = 0;
            Bonus = 0;
            BREAK1 = Convert.ToInt32(dt.Rows[0]["Script_Code"].ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (BREAK1 == Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString()))
                {

                    Qty = Qty + Convert.ToInt32(dt.Rows[i]["Balance"].ToString());
                    Bonus = Bonus + Convert.ToInt32(dt.Rows[i]["BalanceBonus"].ToString());
                    Value = Value + (Convert.ToInt32(dt.Rows[i]["Balance"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()));
                    BREAK1 = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());

                }
                else
                {

                    qry1 = "Update Script_Master set OpQty= " + Qty + " ,OpBonus= " + Bonus + ",OpValue= " + Value + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[i - 1]["Script_Code"].ToString());
                    Thread thred5 = new Thread(() => { count = DataStore(qry1); });
                    thred5.Start();
                    thred5.Join();
                    Qty = 0;
                    Bonus = 0;
                    Value = 0;
                    Qty = Convert.ToInt32(dt.Rows[i]["Balance"].ToString());
                    Value = Value + (Convert.ToInt32(dt.Rows[i]["Balance"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()));
                    BREAK1 = Convert.ToInt32(dt.Rows[i]["Script_Code"].ToString());
                }
            }
            //Saving Last record

            qry1 = "Update Script_Master set OpQty= " + Qty + " ,OpBonus= " + Bonus + ",OpValue= " + Value + " where FNO_Script_No=" + Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Script_Code"].ToString());
            Thread thred6 = new Thread(() => { count = DataStore(qry1); });
            thred6.Start();
            thred6.Join();

            dt = GetData(fromdt, todt);
            SqlDataAdapter da = new SqlDataAdapter();

            rprt1.Load(Server.MapPath("CryEquityGlobalNew.rpt"));
            rprt1.SetDataSource(dt);
            CryEquityGlobalNew.ReportSource = rprt1;

            company_name = Session["Company_Name"].ToString();
            // State = Session["state"].ToString();
            //Address = Session["address"].ToString();
            rprt1.DataDefinition.FormulaFields["company_name"].Text = "\"" + company_name + "\"";
            rprt1.DataDefinition.FormulaFields["pagehead"].Text = "\"" + pagehead + "\"";
        }
        catch(Exception err){

            //clsDAL.MessageBox(this, err.Message);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('"+ err.Message +"')", true);
            Response.Write("<script>alert('" + err.Message + "')</script>");
        }
       
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath=@"D:\pdffiles\cryChequePrinting.pdf";
            string filepath = "C:\\PDFFiles";

            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("C:\\PDFFiles");
            }
            string filename = filepath + "\\SupplierPurchaseOrder" + company_code + "_" + year_code + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";
            rprt1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filename);

            //open PDF File

            //System.Diagnostics.Process.Start(filename);
            WebClient User = new WebClient();

            Byte[] FileBuffer = User.DownloadData(filename);

            if (FileBuffer != null)
            {

                Response.ContentType = "application/pdf";

                Response.AddHeader("content-length", FileBuffer.Length.ToString());

                Response.BinaryWrite(FileBuffer);

            }
        }
        catch (Exception e1)
        {
            Response.Write("PDF err:" + e1);
            return;
        }
        //   Response.Write("<script>alert('PDF successfully Generated');</script>");

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath = @"D:\ashwini\bhavani10012019\accowebBhavaniNew\PAN\cryChequePrinting.pdf";
            //string filepath = @"E:\Lata Software Backup\accowebnavkar\PAN\Saudapending.pdf";
            string filepath = "C:\\PDFFiles";

            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("C:\\PDFFiles");
            }
            string filename = filepath + "\\SupplierPurchaseOrder_" + company_code + "_" + year_code + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";
            rprt1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filename);
            //rprt1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filepath);

            if (txtEmail.Text != string.Empty)
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txtEmail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "SupplierPurchaseOrder";
                // Attachment attachment = new Attachment(Server.MapPath(filename), contentType);
                Attachment attachment = new Attachment(filename);
                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                EncryptPass enc = new EncryptPass();
                emailPassword = enc.Decrypt(emailPassword);
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "SupplierPurchaseOrder";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "No:";
                //msg.IsBodyHtml = true;
                if (smtpPort != string.Empty)
                {
                    SmtpServer.Port = Convert.ToInt32(smtpPort);
                }
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                SmtpServer.Send(msg);
                attachment.Dispose();
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                Response.Write("<script>alert('Mail Send successfully');</script>");
            }


        }
        catch (Exception e1)
        {
            Response.Write("Mail err:" + e1);
            return;
        }


    }

    #region DataStore
    private int DataStore(string Query)
    {
        int count = 0;
        try
        {
            //Connection open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            ///Execution
            myTran = con.BeginTransaction();
            //cmd.CommandText = qry;
            //cmd.Connection = con;
            //cmd.Transaction = myTran;

                if (Query != "")
                {
                    cmd = new SqlCommand(Query, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                myTran.Commit();
                Thread.Sleep(100);
                count = 2;


            return count;
        }
        catch
        {
            if (myTran != null)
            {
                myTran.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('check Entry AND Try Again !')", true);

            }
            return count;

        }
        finally
        {
            con.Close();
        }

    }
    #endregion

    private DataTable GetData(string fromdt, string todt)
    {
        DataTable dt = new DataTable();
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strcon))
        {

            SqlCommand cmd = new SqlCommand("select * from Script_Master where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " and Year_Code<=" + Convert.ToInt32(Session["year"].ToString()) + "", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
        }
        return dt;
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        //rprt1.Close();
        //rprt1.Clone();
        //rprt1.Dispose();
        //GC.Collect();
        this.CryEquityGlobalNew.ReportSource = null;

        CryEquityGlobalNew.Dispose();

        if (rprt1 != null)
        {

            rprt1.Close();

            rprt1.Dispose();

            rprt1 = null;

        }

        GC.Collect();

        GC.WaitForPendingFinalizers();
    }

}