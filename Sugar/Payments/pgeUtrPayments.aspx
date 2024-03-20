<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pgeUtrPayments.aspx.cs" Inherits="Sugar_Payments_pgeUtrPayments" Async="true" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        function PaymentsPage(id, tran_type) {
            var Action = 1;
            window.open('./pgePayments.aspx?tranid=' + id + '&Action=1&tran_type=' + tran_type, "_blank");
        }



        function toggleLoader() {
            debugger
            var loader = document.getElementById('loader');

            loader.classList.remove('hide');
        }
        function removeLoader() {
            var loader = document.getElementById('loader');

            loader.classList.add('hide');
        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js"></script>
    <script type="text/javascript" src="../../Script/jquery-1.4.2.js"></script>
    <style>
        .loader-container {
    position: fixed; /* Full-screen overlay */
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(255, 255, 255, 0.8); /* Semi-transparent background */
    z-index: 9999; /* Ensures it's on top of other elements */
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
}

.spinner {
    border: 4px solid rgba(0, 0, 0, 0.1);
    width: 40px;
    height: 40px;
    border-radius: 50%;
    border-left-color: #09f;
    animation: spin 1s linear infinite;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

.loader-container p {
    margin-top: 10px;
    font-size: 1.2em;
    color: #333;
}

.hide{
    display: none  !important;
}

.checkStatusClass {
    background-color: green;
}

.makePaymentClass{
    background-color: darkorange;
}


    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ajax1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajax1:ToolkitScriptManager>
        <div align="center">
        </div>
        <div id="loader" class="loader-container hide">
    <div class="spinner"></div>
    <p>Please wait while transaction is in process...</p>
</div>

        <div align="center">
            <table width="1400px" align="center" style="font-weight: bold; border-bottom: 1px solid; border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; height: 30px; text-align: center;">
                <tr>
                    <td style="width: 10%;"  align="left">Doc No
                    </td> 
                    <td style="width: 10%;" align="left">Date
                    </td>
                    <td style="width: 25%; text-align: left;">Payment To
                    </td>
                    <td style="width: 10%;text-align: left;">Amount
                    </td>   
                    <td style="width: 20%;text-align: left;">Message Id</td>
                    <td style="width: 10%;text-align: left;">Payment Status</td>
                    <td style="width: 90px;">PAY/STATUS</td>
                </tr>
            </table>
            <asp:DataList ID="dtlAcList" runat="server">
                <ItemTemplate>
                    <table width="1400px" align="center" style="font-size: large; border-bottom: 1px solid; text-align: center; border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; height: 25px;">
                        <tr>
                            <td style="width: 10%;" align="left" >
                                <asp:LinkButton runat="server" ID="lbkTenderNo" Text='<%#Eval("doc_no") %>' 
                                    OnClick="lbkTenderNo_Click">
                                </asp:LinkButton>
                            </td> 
                            <td style="width: 10%;" align="left">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("date") %>'></asp:Label>
                            </td>
                            <td style="width: 25%;font-weight:bold" align="left">
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("payment_to_name") %>'></asp:Label>
                            </td>
                            <td style="width: 10%;"  align="left">
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("amount") %>'></asp:Label></td>
                            <td style="width: 20%;"  align="left">
                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("messageId") %>'></asp:Label></td> 
                             <td style="width: 10%;"  align="left">
                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("paymentStatusDescription") %>'></asp:Label></td>
                            <td style="width: 150px;">
                                <asp:Button Text='<%# (Eval("paymentStatusCode").ToString() == "C" ||  Eval("isPaymentDone").ToString() == "1") ? "Check Status" : 
                                        (new List<string> { "AR", "DF", "CF", "UP", "RD", "PS", "DL", "DC", "CN", "O", "R" }.Contains(Eval("paymentStatusCode").ToString()) ? "Retry Payment" : "Make Payment") %>' 
                                    ID="btnMakePayment" runat="server"
                                    CommandArgument='<%#Eval("doc_no") + ";" + Eval("mill_code")+ ";" + Eval("amount")+ ";" + Eval("isPaymentDone") + ";" + Eval("date") + ";" + Eval("PaymentToId") + ";" + Eval("messageId") + ";" + Eval("bankTransactionId")  +";" + Eval("PaymentType")  +";"%>'
                                    OnClientClick="toggleLoader();" OnClick="btnMakePayment_Click"
                                    
                                    CssClass='<%# (Eval("paymentStatusCode").ToString() == "C" ||  Eval("isPaymentDone").ToString() == "1") ? "checkStatusClass" : "makePaymentClass" %>'
                                    Style='<%# String.Format("padding:10px;border: 1px solid white;color: white;font-weight: bold; border-radius: 5px;width:150px;cursor:pointer;pointer-events: {0};",Eval("paymentStatusCode").ToString() == "U" ? "none" : "all") %>' />
                            </td>
                           <%--  <td style="width: 150px;">
                                <asp:Button Text='<%# (Eval("paymentStatusCode").ToString() == "C" ||  Eval("isPaymentDone").ToString() == "1") ? "Check Status" : 
                                        (new List<string> { "AR", "DF", "CF", "UP", "RD", "PS", "DL", "DC", "CN", "O", "R" }.Contains(Eval("paymentStatusCode").ToString()) ? "Retry Payment" : "Make Payment") %>' 
                                    ID="Button1" runat="server"
                                    CommandArgument='<%#Eval("id") + ";" + Eval("payment_to") + ";" + Eval("net_payable") + ";" + Eval("doc_no")+ ";" + Eval("isPaymentDone") + ";" + Eval("messageId") + ";" + Eval("date") + ";" + Eval("Tran_Type") + ";" + Eval("bankTransactionId")  +";" + Eval("PaymentToId")  +";"%>'
                                    OnClientClick="toggleLoader();" OnClick="btnMakePayment_Click"
                                    
                                    CssClass='<%# (Eval("paymentStatusCode").ToString() == "C" ||  Eval("isPaymentDone").ToString() == "1") ? "checkStatusClass" : "makePaymentClass" %>'
                                    Style='<%# String.Format("padding:10px;border: 1px solid white;color: white;font-weight: bold; border-radius: 5px;width:150px;cursor:pointer;pointer-events: {0};",Eval("paymentStatusCode").ToString() == "U" ? "none" : "all") %>' />
                            </td>--%>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <%--<table>
                <tr>
                    <td>Date:
                         <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                        <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                            PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                        <asp:Button Text="Change Date" runat="server" ID="btnchangedate" OnClick="btnchangedate_Click" />
                    </td>
                </tr>
            </table>--%>
        </div>
    </form>
</body>
</html>
