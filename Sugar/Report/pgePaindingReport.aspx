<%@ Page Title="Group Painding Reports" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgePaindingReport.aspx.cs" Inherits="Sugar_Report_pgePaindingReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function pr(Ac_Code) {
            window.open('rptGroupPaindingBanance.aspx?Ac_Code=' + Ac_Code);     
        }
        function GT(Group_Code, FromDt, ToDt, Prosess, Accounted, GroupOrAccount, Ac_no) {
            window.open('rptGroupTenderGroupWice.aspx?Group_Code=' + Group_Code + '&FromDt=' + FromDt + '&ToDt=' + ToDt + '&Prosess=' + Prosess + '&Accounted=' + Accounted + '&GroupOrAccount=' + GroupOrAccount + '&Ac_no=' + Ac_no);
        }
        function SG(Ac_Code, FromDt, ToDt) {
            window.open('rptSelfBalanceGroupWice.aspx?Ac_Code=' + Ac_Code);     
        }
        function MW() {
            window.open('rptStockBookMillWice.aspx');     
        }
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

                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;

                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";

                document.getElementById("<%=txtSearchText.ClientID %>").value = "";

                var grid = document.getElementById("<%= grdPopup.ClientID %>");

                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;


                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }
                if (hdnfClosePopupValue == "txtGroupCode") {
                    document.getElementById("<%= txtGroupCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblGroupName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                }
                if (hdnfClosePopupValue == "txtAcCode") {
                    document.getElementById("<%= txtAcCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                         document.getElementById("<%= lblAcName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                     }

                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
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
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript" language="javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Summary Report=>Ok / PartyWise=>Cancel ")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
       
        function Groupcode(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnGroupCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGroupCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGroupCode.ClientID %>").val(unit);
                __doPostBack("txtGroupCode", "TextChanged");

            }

        }
        function Accode(e) {
                debugger;
                if (e.keyCode == 112) {
                    debugger;
                    e.preventDefault();
                    $("#<%=pnlPopup.ClientID %>").show();
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
                if (hdnfClosePopupValue == "txtGroupCode") {
                    document.getElementById("<%=txtGroupCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtAcCode") {
                    document.getElementById("<%=txtAcCode.ClientID %>").focus();
                  }
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="  Purchase And Sale Details Report   "
                Font-Names="verdana" ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdHelpPageCount" runat="server" />
    <asp:HiddenField ID="hdconfirm" runat="server" />
    <asp:HiddenField ID="hdnfgid" runat="server" />
      <center>
        <table width="100%" align="left" cellspacing="5">
            
              <tr> 
                <td align="right" style="width: 40%;">
                    <asp:RadioButtonList ID="radioFilter" runat="server" RepeatDirection="Horizontal"
                        CellPadding="6" CellSpacing="6" AutoPostBack="True" OnSelectedIndexChanged="radioFilter_SelectedIndexChanged">
                        <asp:ListItem Text="Group Wice" Value="B" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Account Wice" Value="A"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                  
              </tr> 
              <tr> 
              
                <td align="right" style="width: 40%;">
                    <b>Group Code:</b>
                </td>
             <td align="left" colspan="5" style="width: 60%;">
                <asp:TextBox ID="txtGroupCode" runat="server" Width="80px" CssClass="txt" AutoPostBack="false"
                    OnTextChanged="txtGroupCode_TextChanged" Height="24px" onkeydown="Groupcode(event);"></asp:TextBox>
                <asp:Button ID="btnGroupCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGroupCode_Click"
                    Height="24px" Width="20px" />
                <asp:Label ID="lblGroupName" runat="server" CssClass="lblName"></asp:Label> 
               
            </td> 
                </tr>
              <tr> 
              
                <td align="right" style="width: 40%;">
                    <b>Ac Code:</b>
                </td>
             <td align="left" colspan="5" style="width: 60%;">
                <asp:TextBox ID="txtAcCode" runat="server" Width="80px" CssClass="txt" AutoPostBack="false"
                    OnTextChanged="txtAcCode_TextChanged" Height="24px" onkeydown="Accode(event);"></asp:TextBox>
                <asp:Button ID="btnAcCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnAcCode_Click"
                    Height="24px" Width="20px" />
                <asp:Label ID="lblAcName" runat="server" CssClass="lblName"></asp:Label> 
               
            </td> 
                </tr>
         <tr>
               <td align="right">
                    <b>Prosess:</b>
                </td>
                <td align="left" >
                <asp:DropDownList ID="drpFilter" runat="server" CssClass="ddl" Width="280px" Height="25px"
                     AutoPostBack="true">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </td>
            </tr> 
         <tr>
               <td align="right">
                    <b>Is Accounted:</b>
                </td>
                <td align="left" >
                <asp:DropDownList ID="drpIsAccounted" runat="server" CssClass="ddl" Width="280px" Height="25px"
                     AutoPostBack="true">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </td>
            </tr> 
                
         <tr>
                <td align="right">
                    <b>From:</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                        TabIndex="3" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode);" onkeydown="DateFormat(this,event.keyCode);"> </asp:TextBox>
                    <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                        Width="25px" Height="15px" />
                    <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                </td>
             
                </tr>
            <tr>
                <td align="right">
                    <b>To:</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                        TabIndex="4" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"> </asp:TextBox>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                        Height="15px" />
                    <ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="Image1" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                </td>
            </tr>
            
        </table>
        <table>
        <tr>
             <td align="left" style="width: 40%;">
                <asp:Button runat="server" ID="btnPaindingReport" CssClass="btnHelp" Text="Party Wice Report"
                    OnClick="btnPaindingReport_Click" Width="180px" Height="24px" OnClientClick="purchaseReprt();" />
            </td> 
           <td align="right" style="width: 50%;">
                <asp:Button runat="server" ID="btnGroupwaiceTender" CssClass="btnHelp" Text="Group Summary"
                    OnClick="btnGroupwaiceTender_Click" Width="180px" Height="24px" />
            </td>
             
        </tr> 
              <tr>
             <td align="left" style="width: 40%;">
                <asp:Button runat="server" ID="btnSelfGroupwaice" CssClass="btnHelp" Text="Self Balance Groupwaice"
                    OnClick="btnSelfGroupwaice_Click" Width="180px" Height="24px"  />
            </td>  
                    <asp:Button runat="server" ID="btnStockBookMill" CssClass="btnHelp" Text="Stock Book Mill Waice"
                    OnClick="btnStockBookMill_Click" Width="180px" Height="24px"  />
            </td>
             
        </tr>   
    </table>
          
    </center>
    <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="70%"
        align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
        Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
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
                        Width="250px" Height="20px" AutoPostBack="true"></asp:TextBox>
                    <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                        CssClass="btnSubmit" OnClick="btnSearch_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:Panel ID="pnlScroll" runat="server" Width="680px" ScrollBars="Both" Direction="LeftToRight" BackColor="#FFFFE4" style="z-index:5000 ;  float:right; max-height:380px; height:380px;">--%>
                    <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                        Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                        <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                            EmptyDataText="No Records Found" ViewStateMode="Disabled" PageSize="20" AllowPaging="true"
                            HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated"
                            OnPageIndexChanging="grdPopup_PageIndexChanging" Style="table-layout: fixed;"
                            OnRowDataBound="grdPopup_RowDataBound">
                            <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                            <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                            <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                            <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                            <PagerSettings Position="TopAndBottom" />
                        </asp:GridView>
                    </asp:Panel>
                    <%--</asp:Panel>--%>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
