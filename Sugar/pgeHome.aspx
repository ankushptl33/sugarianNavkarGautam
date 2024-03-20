<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeHome.aspx.cs" Inherits="pgeHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
 
    <style type="text/css">
        .bagroundPopup {
            opacity: 0.6;
            background-color: Black;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://jsonip.appspot.com/?callback=DisplayIP";
            document.getElementsByTagName("head")[0].appendChild(script);
        };
        function DisplayIP(response) {
            document.getElementById("ipaddress").innerHTML = "Your IP Address is " + response.ip;
        }
    </script>
    <script type="text/javascript">
        //function Validate() {
        //    var RB1 = document.getElementById("");
         //   var radio = RB1.getElementsByTagName("input");
           // var isChecked = false;
            //for (var i = 0; i < radio.length; i++) {
              //  if (radio[i].checked) {
                //    isChecked = true;
                  //  break;
                //}
            //}
            //if (!isChecked) {
              //  alert("Please select atleast one option");
           // }
            //return isChecked;
        //}
    </script>
 
    <script type="text/javascript">
        function dispSummary() {
            var d = new Date().toISOString().slice(0, 10);
            window.open('../Report/rptDispSummarySmall.aspx?fromDT=' + d + '&toDT=' + d + '&Branch_Code=');
        }
        function bwlpa(FromDT, ToDt, Broker_Code) {
            window.open('../Report/rptBrokerWiseLatePayAll.aspx?FromDT=' + FromDT + '&ToDt=' + ToDt + '&Broker_Code=' + Broker_Code);
        }

    </script>
    <script type="text/javascript">
        window.onload = function () {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://www.telize.com/jsonip?callback=DisplayIP";
            document.getElementsByTagName("head")[0].appendChild(script);
        };
        function DisplayIP(response) {

        }
    </script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
    <script type="text/javascript">
        //var checkStatus;

        //var element = new Image();
        //element.__defineGetter__('id', function () {
        //    checkStatus = 'on';
        //});

        //setInterval(function () {
        //    checkStatus = 'off';
        //    console.log(element);
        //    console.clear();
        //    document.querySelector('#status').innerHTML = checkStatus;
        //}, 1000)
    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js" type="text/javascript">
    </script>

    
    <script type="text/javascript">
        function DO() {
            //   window.open('../Sugar/pgeDeliveryOrderForGST.aspx');
            var a = '<%= Session["DOPages"] %>';
            if (a == 1) {
                window.open('../Sugar/BussinessRelated/pgeDeliveryOrderForGSTxmlNew.aspx?DO=0&Action=2');
            }
            else {

                window.open('../Sugar/BussinessRelated/pgeDeliveryOrderMultipleItem.aspx?DO=0&Action=2');
            }
        }
        function TP() {
            window.open('../Sugar/BussinessRelated/PgeTenderHeadUtility.aspx');
        }
        function GL() {
            window.open('../Sugar/Report/pgeGLedgerReport.aspx');
        }
        function MP(FromDT, ToDt, Millcode) {
            window.open('../Sugar/Report/rptMillPaymentForGST.aspx?fromDT=' + FromDT + '&ToDt=' + ToDt + '&Mill_Code=0')
        }
        //function Ac(FromDT, ToDt) {
        //    window.open('../Sugar/Report/rptUtrOnlyBalance.aspx?accode=&utr_no=&AcType=M&FromDT=' + FromDT + '&ToDt=' + ToDt);
        //}
        function DD(FromDT, ToDt, Branch_Code) {
            window.open('../Sugar/Report/rptDoWiseDispatch.aspx?FromDT=' + FromDT + '&ToDt=' + ToDt + '&Branch_Code=' + Branch_Code);
        }
        function report(FromDT, ToDt) {
            window.open('../Sugar/Report/rptPartyWiseSugarStock.aspx?FromDT=' + FromDT + '&ToDt=' + ToDt);
        }
        function dispSummary() {
            var d = new Date().toISOString().slice(0, 10);
            window.open('../Sugar/Report/rptDispSummarySmall.aspx?fromDT=' + d + '&toDT=' + d + '&Branch_Code=');
        }
        function bwlpa(FromDT, ToDt, Broker_Code) {
            window.open('../Sugar/Report/rptBrokerWiseLatePayAll.aspx?FromDT=' + FromDT + '&ToDt=' + ToDt + '&Broker_Code=' + Broker_Code);
        }
        function Account() {
            window.open('../Sugar/Master/pgeAccountUtility.aspx');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnfway" runat="server" />
            <asp:HiddenField ID="hdnfconfirm" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <div class="leftPane" style="width: 1300px; height: 500px; margin: 15px 0 0 10px;">
                    <table width="100%" align="left">
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink9" runat="server"  Style="font-size: large; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/BussinessRelated/pgeRegisters.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Register</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink5" runat="server" Style="font-size: large; color: white; width: 170px; height: 50px" Target="_blank"
                                    class="button" onclick="DO();"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Delivery Order</p>
                                         </span></asp:HyperLink>


                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink1" runat="server" Style="font-size: large; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Transaction/PgeUTRHeadUtility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Utr Entry</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink2" runat="server" Style="font-size: medium; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/BussinessRelated/PgeTenderHeadUtility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Tender Purchase</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btndatabasebackup" Text="Database Backup" Style="font-size: large; text-decoration: none; color: White; width: 190px; height: 50px;"
                                    class="button"
                                    OnClick="btndatabasebackup_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink3" runat="server" Style="font-size: large; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/BussinessRelated/PgeCarporatesaleUtility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Carporate Sale</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink4" runat="server" Style="font-size: medium; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Transaction/pgeMultipleReceipt_utility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Multiple Receipt</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink6" runat="server" Style="font-size: medium; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Transaction/PgeReceiptPaymentUtility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Receipt Payment</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink7" runat="server" Style="font-size: small; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Report/pgeTrialBalance.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Trial Balance Screen</p>
                                         </span></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink10" runat="server" Style="font-size: small; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Report/pgestockbook.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                           Sugar Balance Stock</p>
                                         </span></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <a onclick="javascript:dispSummary();" style="font-size: large; text-decoration: none; color: White;"
                                    class="button">
                                    <p style="margin: 20px auto;">
                                        Dispatch Summary
                                    </p>
                                </a>
                            </td>
                            <td>
                                <a href="../Sugar/pgeTransportSMS.aspx" target="_blank" style="font-size: large; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                        Transport SMS
                                    </p>
                                </a>
                            </td>
                            <td>
                                <a href="../Sugar/Report/rptSugarBalanceStocks.aspx" target="_blank" style="font-size: large; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                        Stock Book
                                    </p>
                                </a>
                            </td>
                            <td>
                                <a href="../Sugar/Report/rptSugarBalanceStockSummary.aspx" target="_blank" style="font-size: large; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                        Stock Summary
                                    </p>
                                </a>
                                <asp:Label ID="lblIPAdd" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="Label1" runat="server" CssClass="lblName" ForeColor="Red"></asp:Label>
                            </td>
                             <td>
                                <a href="../Sugar/Inword/pgeGrainPurchaseBillUtility.aspx" target="_blank" style="font-size: small; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                       Grain Purchase Bill
                                    </p>
                                </a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnAll" Text="Daily Report" Style="font-size: large; text-decoration: none; color: White; width: 190px; height: 50px;"
                                    class="button"
                                    OnClick="btnAll_Click" />
                            </td>


                            <td>
                                <a href="../Sugar/Report/pgeGLedgerReport.aspx" target="_blank" style="font-size: large; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                        Ledger
                                    </p>
                                </a>
                            </td>
                            <td>
                                <a href="../Sugar/BussinessRelated/pgeCarporateRegister.aspx" target="_blank" style="font-size: medium; text-decoration: none; color: White; width: 170px; height: 50px;"
                                    class="button">
                                    <p style="margin: 20px auto; margin-left: 10px;">
                                        Carporate Register
                                    </p>
                                </a>
                            </td>
                            <td>
                                <asp:HyperLink ID="HyperLink8" runat="server" Style="font-size: large; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Report/pgeBrokerReport.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Broker Report</p>
                                         </span></asp:HyperLink>
                            </td>
                             <td>
                                <asp:HyperLink ID="HyperLink11" runat="server" Style="font-size: large; color: white; width: 170px; height: 50px;" Target="_blank" class="button" NavigateUrl="~/Sugar/Outword/pgeGrainSaleBillUtility.aspx"><span>
                         <p style="margin-top: 20px; margin-left: 20px;">
                            Grain Sale Bill</p>
                                         </span></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table width="100%" align="right">
                        <tr>
                            <td>
                                <a target="_blank" href="https://services.gst.gov.in/services/login">
                                    <img src="../Images/gst.png" height="60px" width="60px" />
                                </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://onlineservices.tin.egov-nsdl.com/etaxnew/tdsnontds.jsp">
                        <img src="../Images/e-payments.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://ewaybillgst.gov.in/login.aspx">
                        <img src="../Images/e way.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.google.com/">
                        <img src="../Images/google.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://accounts.google.com/signin/v2/identifier?passive=1209600&continue=https%3A%2F%2Faccounts.google.com%2Fb%2F0%2FAddMailService&followup=https%3A%2F%2Faccounts.google.com%2Fb%2F0%2FAddMailService&flowName=GlifWebSignIn&flowEntry=ServiceLogin">
                        <img src="../Images/gmail.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.google.com/drive/">
                        <img src="../Images/Google Drive.png" height="60px" width="60px" />
                    </a>

                                &emsp;&emsp;
                    <a target="_blank" href="https://accounts.google.com/signin/v2/identifier?service=analytics&passive=1209600&continue=https%3A%2F%2Fanalytics.google.com%2Fanalytics%2Fweb%2F%23&followup=https%3A%2F%2Fanalytics.google.com%2Fanalytics%2Fweb%2F&flowName=GlifWebSignIn&flowEntry=ServiceLogin">
                        <img src="../Images/Google Analytics.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://outlook.live.com/owa/">
                        <img src="../Images/outlook.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.moneycontrol.com/">
                        <img src="../Images/money control.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.bseindia.com/markets/Commodity/commodity.html">
                        <img src="../Images/bse-commodity.png" height="60px" width="60px" />
                    </a>

                                &emsp;&emsp;
                    <a target="_blank" href="./Transaction/pgeTransactionUtility.aspx">
                        <img src="../Images/Skypeimage.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.investing.com/">
                        <img src="../Images/investing.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://cibnext.icicibank.com/corp/AuthenticationController?FORMSGROUP_ID__=AuthenticationFG&__START_TRAN_FLAG__=Y&FG_BUTTONS__=LOAD&ACTION.LOAD=Y&AuthenticationFG.LOGIN_FLAG=1&BANK_ID=ICI&ITM=nli_corp_primer_login_btn_desk">
                        <img src="../Images/icici.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://netbanking.hdfcbank.com/netbanking/">
                        <img src="../Images/hdfc.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                                <a target="_blank" href="https://corp.onlinesbi.com/corpuser/login.htm">
                                    <img src="../Images/sbi.png" height="60px" width="60px" />
                                </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://corp.idbibank.co.in/corp/BANKAWAY?Action.CorpUser.Init.001=y&AppSignonBankId=IBKL&AppType=corporate">
                        <img src="../Images/idbi.png" height="60px" width="60px" />
                    </a>

                                &emsp;&emsp;
                    <a target="_blank" href="https://corporate.axisbank.co.in/wps/portal/cBanking/axiscbanking/AxisCorporateLogin/!ut/p/a1/04_Sj9CPykssy0xPLMnMz0vMAfGjzOKNAzxMjIwNjLwszI0cDRw9PU3dw3xcjP19TIAKIoEKDHAARwNC-sP1o_AqMTWFKsBjRUFuhEGmo6IiACFl0ps!/dl5/d5/L2dBISEvZ0FBIS9nQSEh/?_ga=2.11015732.1610748224.1570105808-2061852097.1570105808">
                        <img src="../Images/axis.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.facebook.com/">
                        <img src="../Images/facebook.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.linkedin.com/">
                        <img src="../Images/linked in.png" height="60px" width="60px" />
                    </a>

                                &emsp;&emsp;
                    <a target="_blank" href="https://twitter.com/">
                        <img src="../Images/twitter.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.youtube.com/">
                        <img src="../Images/youtube.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.aawaz.com/">
                        <img src="../Images/aawaz.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                                <a target="_blank" href="https://www.makemytrip.com/">
                                    <img src="../Images/mmt.png" height="60px" width="60px" />
                                </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.booking.com/index.en-gb.html?label=gen173rf-1BCAEoggI46AdIM1gDaGyIAQGYAQm4ARfIAQzYAQHoAQGIAgGiAg5jaGluaW1hbmRpLmNvbagCA7gCqMKK_AXAAgHSAiQ2NjUyZDk5NS0wMmNlLTRmYTEtYThhYS00N2JjMzUyNDQ0NmHYAgXgAgE;sid=3c6547307fe97f11e6e7b8ebbcb9ee27;keep_landing=1&sb_price_type=total&">
                        <img src="../Images/booking.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.redbus.in/">
                        <img src="../Images/redbus.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://www.irctc.co.in/nget/train-search">
                        <img src="../Images/IRCTC.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://hangouts.google.com/">
                        <img src="../Images/Google hangout.png" height="60px" width="60px" />
                    </a>
                                &emsp;&emsp;
                    <a target="_blank" href="https://web.whatsapp.com/">
                        <img src="../Images/Whatsapp.png" height="60px" width="60px" />
                    </a>

                            </td>
                        </tr>
                    </table>
                </div>

            

            </asp:Panel>
             
            <asp:Button ID="btn4" runat="server" Style="display: none;" />
            <div class="otp-popup" id="divOtp" style="display: none;">
                 
            </div>
         
        
         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
