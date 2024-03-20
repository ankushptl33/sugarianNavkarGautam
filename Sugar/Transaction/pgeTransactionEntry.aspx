<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeTransactionEntry.aspx.cs" Inherits="Sugar_Transaction_pgeTransactionEntry" Async="true" %>


<%@ MasterType VirtualPath="~/MasterPage2.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        // Show the loader when an AJAX request starts
        $(document).ajaxStart(function () {
            $('#loader').show();
        });

        // Hide the loader when an AJAX request completes
        $(document).ajaxStop(function () {
            $('#loader').hide();
        });
    });
</script>

    <script type="text/javascript" language="javascript">
        debugger;
        document.addEventListener('keyup', function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var key = event.key || event.keyCode;

            if (key === 'Escape' || key === 'Esc' || key === 27) {
                //                doWhateverYouWantNowThatYourKeyWasHit();
                debugger;
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                if (hdnfClosePopupValue == "txtAcCode") {
                    document.getElementById("<%=txtAcCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBankCode") {
                    document.getElementById("<%=txtBankCode.ClientID %>").focus();
                }
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }
        });
    </script>

    <script type="text/javascript" language="javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            }
            else if (KeyCode == 38) {
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            }
            else if (KeyCode == 13) {
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                var grid = document.getElementById("<%= grdPopup.ClientID %>");
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";

                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;


                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }

                if (hdnfClosePopupValue == "txtAcCode") {
                    document.getElementById("<%=txtAcCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblAcName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtAcCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtBankCode") {
                    document.getElementById("<%=txtBankCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBankName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBankCode.ClientID %>").focus();
                }
            }
}
function SelectRow(CurrentRow, RowIndex) {
    UpperBound = parseInt('<%= this.grdPopup.Rows.Count %>') - 1;
    LowerBound = 0;
    if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }
    if (CurrentRow != null) {
        CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
        CurrentRow.originalForeColor = CurrentRow.style.color;
        CurrentRow.style.backgroundColor = '#DCFC5C';
        CurrentRow.style.color = 'Black';
    }
    SelectedRow = CurrentRow;
    SelectedRowIndex = RowIndex;
    setTimeout("SelectedRow.focus();", 0);
}
    </script>

    <script type="text/javascript">

        function AcCode(e) {

            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=hdnfpopup.ClientID%>").val("0");
                $("#<%=btnAcCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtAcCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtAcCode.ClientID %>").val(unit);
                __doPostBack("txtAcCode", "TextChanged");

            }
        }
        function BankCode(e) {

            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=hdnfpopup.ClientID%>").val("0");
                $("#<%=btnBankCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBankCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBankCode.ClientID %>").val(unit);
                __doPostBack("txtBankCode", "TextChanged");

            }
        }
    </script>


    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
               }
               else {
                   confirm_value.value = "No";
                   document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }



        function UP(DocNo) {
            var No = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            window.open('../Report/rptUtrPrint.aspx?Doc_No=' + No);
        }


        function Back() {
            window.open('../Transaction/TransactionEntryUtility.aspx', '_self');
        }

        function Pay() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
               }
               else {
                   confirm_value.value = "No";
                   document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
               }
               document.forms[0].appendChild(confirm_value);
           }

           function PendingTransaction() {

               window.open('../Report/rptcheckPendingTransaction.aspx');
               // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
           }
           function PaymentStatus() {

               window.open('../Report/rptcheckPaymentStatus.aspx');
               // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
           }
           

    </script> 
    <style>
        .loader {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: none;
    justify-content: center;
    align-items: center;
    z-index: 9999;
    color: white;
    font-size: 24px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div id="loader" class="loader">
    Loading...
</div>
    <div class="content">
        <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
            <legend style="text-align: center;">
                <asp:Label ID="label1" runat="server" Text="   Bank Tranction   " Font-Names="verdana"
                    ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
        </fieldset>

        <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hdnfClosePopup" runat="server" />
                <asp:HiddenField ID="hdnfpopup" runat="server" />
                <asp:HiddenField ID="hdnfyearcode" runat="server" />
                <asp:HiddenField ID="hdnfcompanycode" runat="server" />
                <asp:HiddenField ID="hdnf" runat="server" />
                <asp:HiddenField ID="hdnfacid" runat="server" />
                <asp:HiddenField ID="hdHelpPageCount" runat="server" />
                <asp:HiddenField ID="hdconfirm" runat="server" />
                <asp:HiddenField ID="hdnfbcid" runat="server" />

                <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                    <table width="100%" align="left">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                    ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" TabIndex="12" />
                                &nbsp;
                                <asp:Button ID="btnSave" runat="server" OnClientClick="if (!Validate()) return false;"
                                    Text="Save" CssClass="btnHelp" Width="90px" TabIndex="13" Height="24px" ValidationGroup="add"
                                    OnClick="btnSave_Click" />
                                &nbsp;
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                                    TabIndex="14" Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" />
                                &nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                                    Height="24px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()"
                                    TabIndex="15" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                                    TabIndex="16" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                                    ValidationGroup="save" Height="24px" OnClientClick="Back();" TabIndex="17" />
                                &nbsp;&nbsp;
                                  <asp:Button ID="btnPay" runat="server"
                                      Text="Pay" CssClass="btnHelp" Width="90px" TabIndex="18" Height="24px" ValidationGroup="add"
                                      OnClick="btnPay_Click" OnClientClick="Pay();" />
                                &nbsp;&nbsp;
                                  <asp:Button ID="btnPendingTransaction" runat="server"
                                      Text="Pending Transaction" CssClass="btnHelp" Width="150px" TabIndex="18" Height="24px" ValidationGroup="add"
                                      OnClientClick="PendingTransaction();" />
                                    &nbsp;&nbsp;
                                  <asp:Button ID="btnPaymentStatus" runat="server"
                                      Text="Payment Status" CssClass="btnHelp" Width="150px" TabIndex="18" Height="24px" ValidationGroup="add"
                                    OnClientClick="PaymentStatus();" />
                            </td>
                        </tr>
                    </table>
                    <table width="80%" align="left">
                        <tr>
                            <td align="center" colspan="4">&nbsp;&nbsp; 
                            </td>
                            <td align="center" colspan="4">&nbsp;&nbsp; 
                            </td>
                            <td align="center" colspan="4">&nbsp;&nbsp; 
                            </td>
                            <td align="center" colspan="4">&nbsp;&nbsp; 
                            </td>
                            <td align="center" colspan="4"></td>
                        </tr>
                    </table>
                    <table style="width: 100%;" align="left" cellpadding="2" cellspacing="5">
                        <tr>
                            <td align="right" style="width: 30%;">Change No:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                    TabIndex="1" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                    TargetControlID="txtEditDoc_No">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                    Font-Size="Small" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Entry No:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                    Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtdoc_no_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtDOC_NO_Click"
                                    CssClass="btnHelp" Height="24px" />
                            </td>
                            <%--<td align="left">
                        </td>--%>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Date:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="2" Width="90px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                                Payment Date: 
                                <asp:TextBox ID="txtPayment_DATE" runat="Server" CssClass="txt" TabIndex="3" Width="90px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPayment_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender2" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderPaymentDate" runat="server" TargetControlID="txtPayment_DATE"
                                    PopupButtonID="imgcalender2" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Ac Code:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtAcCode" runat="Server" CssClass="txt" TabIndex="4" Width="90px"
                                    onkeydown="AcCode(event);" Style="text-align: right;" AutoPostBack="true" Height="24px"
                                    OnTextChanged="txtAcCode_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnAcCode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                    Width="20px" OnClick="btnAcCode_Click" />
                                <asp:Label ID="lblAcName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Bank Code:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtBankCode" runat="Server" CssClass="txt" TabIndex="5" Width="90px"
                                    onkeydown="BankCode(event);" Style="text-align: right;" AutoPostBack="true" Height="24px"
                                    OnTextChanged="txtBankCode_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnBankCode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                    Width="20px" OnClick="btnBankCode_Click" />
                                <asp:Label ID="lblBankName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Name:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtName" runat="Server" CssClass="txt" TabIndex="6" Width="160px"
                                    Style="text-align: left;" AutoPostBack="True" Height="24px" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Account Number:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtAccountNumber" runat="Server" CssClass="txt" TabIndex="7" Width="160px"
                                    Style="text-align: left;" AutoPostBack="false" Height="24px" OnTextChanged="txtAccountNumber_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">IFSC Code:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtIFSCCode" runat="Server" CssClass="txt" TabIndex="8" Width="160px"
                                    Style="text-align: left;" AutoPostBack="false" Height="24px" OnTextChanged="txtIFSCCode_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="right" style="width: 30%;">Mobile Number:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtMobileNumber" runat="Server" CssClass="txt" TabIndex="8" Width="160px"
                                    Style="text-align: left;" AutoPostBack="false" Height="24px"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="right" style="width: 20%;">Payment Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpPaymentType" runat="server" CssClass="ddl" Width="140px"
                                    TabIndex="2" AutoPostBack="true" Height="26px" OnSelectedIndexChanged="drpPaymentType_SelectedIndexChanged">
                                    <asp:ListItem Text="NEFT" Value="NEFT" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="IMPS" Value="IMPS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Amount:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtamount" runat="Server" CssClass="txt" TabIndex="9" Width="160px"
                                    Style="text-align: right;" AutoPostBack="false" Height="24px" OnTextChanged="txtamount_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Remark:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtRemark" runat="Server" CssClass="txt" TabIndex="10" Width="260px"
                                    Style="text-align: left;" AutoPostBack="false" TextMode="MultiLine" Height="50px" OnTextChanged="txtRemark_TextChanged"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" style="width: 30%;">Narration:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtnarration" runat="Server" CssClass="txt" TabIndex="11" Width="260px"
                                    Style="text-align: left;" TextMode="MultiLine" AutoPostBack="false" Height="50px" OnTextChanged="txtnarration_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <table style="width: 100%;" align="left" cellpadding="2" cellspacing="5">
                        <tr>
                            <td style="width: 15%;"></td>
                            <td align="left" style="width: 60%;">
                                <div style="width: 100%; position: relative; margin-top: 0px;">
                                    <asp:UpdatePanel ID="upGrid" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlgrdDetail" runat="server" align="left" ScrollBars="Both" Height="150px"
                                                Width="1050px" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true"
                                                Font-Names="Verdana" Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                                                <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                                    HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="120%"
                                                    OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound"
                                                    Style="table-layout: fixed; float: left">
                                                    <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                <asp:Panel ID="pnlPopup" runat="server" Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%; background-color: #FFFFE4;">


                    <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                        Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                        ToolTip="Close" />
                    <table width="95%">
                        <tr>
                            <td align="center" style="background-color: #F5B540; width: 100%;">
                                <asp:Label ID="lblPopupHead" runat="server" Font-Size="Medium" Font-Names="verdana"
                                    Font-Bold="true" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Search Text:
                                <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                    Width="250px" Height="20px" AutoPostBack="true" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                                    CssClass="btnSubmit" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlInner" runat="server" ScrollBars="Auto" autosize="true" Width="100%"
                                    Direction="LeftToRight" BackColor="#FFFFE4" Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                    <asp:GridView ID="grdPopup" CssClass="select" runat="server" AutoGenerateColumns="true"
                                        AllowPaging="true" PageSize="25" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                        HeaderStyle-ForeColor="White" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                        OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                                        <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                        <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                                        <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                                        <PagerSettings Position="TopAndBottom" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                    BorderColor="Teal" BorderWidth="1px" Height="300px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                    <table width="80%" align="center">
                        <tr>
                            <td colspan="4" align="center" style="background-color: lightslategrey; color: White;">
                                <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                    Text="Tender Details"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

