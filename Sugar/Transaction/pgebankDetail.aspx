<%@ Page Title="Bank Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgebankDetail.aspx.cs" Inherits="Sugar_Transaction_pgebankDetail" %>


<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.2.js"></script>

    <link href="../../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="../../JQuery/jquery.keynavigation.js" type="text/javascript"></script>
    <link type="text/css" href="../../menu/menu.css" rel="stylesheet" />
    <script type="text/javascript" src="../../menu/jquery.js"></script>
    <script type="text/javascript" src="../../menu/menu.js"></script>


    <script src="../../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../Scripts/selectfirstrow.js" type="text/javascript"></script>

     <script type="text/javascript">
         function noCopyMouse(e) {
             var isRight = (e.button) ? (e.button == 2) : (e.which == 3);

             if (isRight) {
                 alert('You Cant Copy And Paste The Account Number!');
                 return false;
             }
             return true;
         }
         function noCopyKey(e) {
             var forbiddenKeys = new Array('c', 'x', 'v');
             var keyCode = (e.keyCode) ? e.keyCode : e.which;
             var isCtrl;


             if (window.event)
                 isCtrl = e.ctrlKey
             else
                 isCtrl = (window.Event) ? ((e.modifiers & Event.CTRL_MASK) == Event.CTRL_MASK) : false;


             if (isCtrl) {
                 for (i = 0; i < forbiddenKeys.length; i++) {
                     if (forbiddenKeys[i] == String.fromCharCode(keyCode).toLowerCase()) {
                         alert('You Cant Copy And Paste The Account Number!');
                         return false;
                     }
                 }
             }
             return true;
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
               
                if (hdnfClosePopupValue == "txtAC_Code") {
                    document.getElementById("<%=txtAC_Code.ClientID %>").focus();
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
              debugger;
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
                if (hdnfClosePopupValue == "txtAC_Code") {
                    document.getElementById("<%=txtAC_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblACName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=hdnfpopup.ClientID %>").value = "0";
                    document.getElementById("<%=txtAC_Code.ClientID %>").focus();
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

 
function ac(e) {
    debugger;
    if (e.keyCode == 112) {
        debugger;
        e.preventDefault();
        $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnAC_Code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtAC_Code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtAC_Code.ClientID %>").val(unit);
        __doPostBack("txtAC_Code", "TextChanged");

    }

}
    </script>

     
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="Bank Detail" Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfpopup" runat="server" />
            <asp:HiddenField ID="hdnftenderno" runat="server" />
            <asp:HiddenField ID="hdnftenderdetailid" runat="server" />
            <asp:HiddenField ID="hdnfID" runat="server" />
            <asp:HiddenField ID="hdnfacid" runat="server" />
            <asp:HiddenField ID="hdnfway" runat="server" />
            <asp:HiddenField ID="hdnfWhatsappNumber" runat="server" />

            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table style="width: 30%;" align="left" cellpadding="4" cellspacing="4">
                     <tr>
                        <td align="left" style="width: 10%;">Payment For:
                        </td>
                        <td align="left" style="width: 10%;">
                      <asp:DropDownList ID="drpTrnType" runat="server" CssClass="ddl" Width="200px" Height="24px"  TabIndex="1"
                AutoPostBack="true" OnSelectedIndexChanged="drpTrnType_SelectedIndexChanged"> 
                <asp:ListItem Text="Against Frieght" Value="AF"></asp:ListItem>
                <asp:ListItem Text="Mill Payment" Value="MP"></asp:ListItem>
            </asp:DropDownList>
                               </td>
                    </tr> 
                     <tr>
                        <td align="left" style="width: 10%;">Default Bank:
                        </td>
                        <td align="left" style="width: 10%;">
                      <asp:DropDownList ID="drpSelectedBank" runat="server" CssClass="ddl" Width="200px" Height="24px"  TabIndex="2"
                AutoPostBack="true" OnSelectedIndexChanged="drpSelectedBank_SelectedIndexChanged"> 
                <asp:ListItem Text="Bank 1" Value="B1"></asp:ListItem>
                <asp:ListItem Text="Bank 2" Value="B2"></asp:ListItem>
                <asp:ListItem Text="Bank 3" Value="B3"></asp:ListItem>
            </asp:DropDownList>
                               </td>
                    </tr>
                       <tr>
                        <td align="left" style="width: 10%;">AC Code:
                        </td>
                        <td align="left" style="width: 40%;">
                            <asp:TextBox ID="txtAC_Code" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtAC_Code_TextChanged" TabIndex="3" Height="24px" onkeydown="ac(event);"></asp:TextBox>
                            <asp:Button ID="btnAC_Code" runat="server" Text="..." CssClass="btnHelp" OnClick="btnAC_Code_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblACName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                    </tr>
                    
                    <tr>
                        <td align="left" style="width: 10%;">Bank A/c No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBankAcNo" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtBankAcNo_TextChanged" TabIndex="4"></asp:TextBox> 
                        </td>
                    </tr>
                       <tr>
                        <td align="left" style="width: 10%;">Verify  A/c No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVerifyAcNo" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtVerifyAcNo_TextChanged" TabIndex="5"></asp:TextBox> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left" style="width: 10%;">IFSC:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtIFSC" runat="server" Width="200px" Height="24px" CssClass="txtUpper"
                              AutoPostBack="true"  OnTextChanged="txtIFSC_TextChanged" TabIndex="6"></asp:TextBox> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left" style="width: 10%;">Bank Name:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtbankname" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbankname_TextChanged" TabIndex="7"></asp:TextBox> 
                        </td>
                    </tr>
                         <tr>
                        <td align="left" style="width: 0%;">Beneficiary Name:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtbeneficiary" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbeneficiary_TextChanged" TabIndex="8"></asp:TextBox> 
                        </td>
                    </tr>
                    

                  
                <table  style="width: 30%;" align="left" cellpadding="4" cellspacing="4">
                     <tr>
                        <td align="right" style="width: 50%;"> &nbsp;  
                        </td>
                         
                        <td align="left" style="width: 10%;">&nbsp;
                            
                        </td>
                    </tr>
                     <tr>
                        <td align="right" style="width: 30%;"> &nbsp;
                        </td>
                        <td align="left" style="width: 50%;">&nbsp;
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Bank A/c No 2:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtBankAcNo2" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtBankAcNo2_TextChanged" TabIndex="9"></asp:TextBox> 
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Verify  A/c No 2:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVerifyAcNo2" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtVerifyAcNo2_TextChanged" TabIndex="10"></asp:TextBox> 
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">IFSC 2:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtIFSC2" runat="server" Width="200px" Height="24px" CssClass="txtUpper"
                              AutoPostBack="true"  OnTextChanged="txtIFSC2_TextChanged" TabIndex="11"></asp:TextBox> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="right" style="width: 10%;">Bank Name 2:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtbankname2" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbankname2_TextChanged" TabIndex="12"></asp:TextBox> 
                        </td>
                    </tr>
                         <tr>
                        <td align="right" style="width: 10%;">Beneficiary Name 2:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtbeneficiary2" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbeneficiary2_TextChanged" TabIndex="13"></asp:TextBox> 
                        </td>
                    </tr>
                </table>

                <table  style="width: 30%;" align="left" cellpadding="4" cellspacing="4">
                     <tr>
                        <td align="right" style="width: 50%;"> &nbsp;  
                        </td>
                         
                        <td align="left" style="width: 10%;">&nbsp;
                            
                        </td>
                    </tr>
                     <tr>
                        <td align="right" style="width: 30%;"> &nbsp;
                        </td>
                        <td align="left" style="width: 50%;">&nbsp;
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Bank A/c No 3:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtBankAcNo3" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtBankAcNo3_TextChanged" TabIndex="14"></asp:TextBox> 
                        </td>
                    </tr>
                      <tr>
                        <td align="right" style="width: 10%;">Verify  A/c No 3:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVerifyAcNo3" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"  OnTextChanged="txtVerifyAcNo3_TextChanged" TabIndex="15"></asp:TextBox> 
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">IFSC 3:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtIFSC3" runat="server" Width="200px" Height="24px" CssClass="txtUpper"
                              AutoPostBack="true"  OnTextChanged="txtIFSC3_TextChanged" TabIndex="16"></asp:TextBox> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="right" style="width: 10%;">Bank Name 3:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtbankname3" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbankname3_TextChanged" TabIndex="17"></asp:TextBox> 
                        </td>
                    </tr>
                         <tr>
                        <td align="right" style="width: 10%;">Beneficiary Name 3:
                        </td>
                        <td align="left" style="width: 50%;">
                            <asp:TextBox ID="txtbeneficiary3" runat="server" Width="200px" Height="24px" CssClass="txt"
                              AutoPostBack="true"   OnTextChanged="txtbeneficiary3_TextChanged" TabIndex="18"></asp:TextBox> 
                        </td>
                    </tr>
                </table>
                     <table  style="width: 30%;" align="left" cellpadding="4" cellspacing="4">
                      <tr align="center">
                        <td>
                            
                            <asp:Button ID="btnOtp" runat="server" Text="Send Otp" CssClass="btnHelp" TabIndex="19"
                                Width="90px" Height="24px" ValidationGroup="save"  OnClick="btnOtp_Click" />

                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnHelp" TabIndex="20"
                                Width="90px" Height="24px" ValidationGroup="save"  OnClick="btnUpdate_Click" />
                           <%--  <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnHelp" TabIndex="21"
                                Width="90px" Height="24px" ValidationGroup="save" OnClientClick = "Confirm()" OnClick="btnUpdate_Click" />--%>
                             

                        </td>
                    </tr>
                   
                </table>
            </asp:Panel>

            <br />
            <br />

            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="85%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%; margin: 0 auto;">
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
                            <asp:Panel ID="pnlInner" runat="server" ScrollBars="Both" Width="100%" Direction="LeftToRight"
                                BackColor="#FFFFE4" Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="13" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
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

              <div id="otpVerification" class="otp-verification">
                <asp:UpdatePanel runat="server" ID="upl4">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="2" align="center">
                                    <h3 style="color: Olive; font-weight: bold;">OTP Verification</h3>
                                    <asp:Label runat="server" ID="lblWrongOtp" ForeColor="Red"></asp:Label>
                                    <asp:Label runat="server" ID="lblResendOtp" ForeColor="BlueViolet"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Please Enter One Time Password(OTP) You have Recieved:
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtOtpVerification" CssClass="textbox" Style="focus: true;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" ID="btnConfirm" CssClass="button2" Text="VERIFY" OnClick="btnConfirm_Click" />
                                </td>
                                <td align="left">
                                    <asp:LinkButton runat="server" Text="Resend OTP" OnClick="resendlnk_Click" ID="resendlnk"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <ajax1:ModalPopupExtender ID="ModalPopupVerification" PopupControlID="otpVerification"
                BackgroundCssClass="bagroundPopup" TargetControlID="btn3" runat="server">

            </ajax1:ModalPopupExtender>

             <div class="otp-popup" id="divOtp" style="display: none;">
                <asp:UpdatePanel ID="up1" runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlOTP">
                        
                            <table>
                                <tr>
                                    <td align="center">
                                        <h3 style="color: White;">User Verification</h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblSendOtp" runat="server" Text="One Time Password" Font-Bold="true"
                                            Font-Italic="true" ForeColor="BurlyWood"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <p style="font-size: large; outline-color: Red; color: White;">
                                          Send OTP For Email & WhatsApp...<br></br>
                                             Email :  <asp:Label ID="lblEmailId" runat="server" Font-Bold="true" /> 
                                            <br> </br>
                                             WhatsApp Number :  <asp:Label ID="lblMo_Number" runat="server" Font-Bold="true" /> 
                                        </p>
                                        
                                    </td>
                                 
                                </tr>
                                <tr>
                                  <%--  <td>
                                        <asp:RadioButtonList runat="server" ID="rblist" OnSelectedIndexChanged="rblist_SelectedIndexChanged"
                                            ForeColor="#FFFFCC">
                                            <asp:ListItem Text="SMS" Value="S"></asp:ListItem>
                                            <asp:ListItem Text="Email" Value="E"></asp:ListItem>
                                            <asp:ListItem Text="WhatsApp" Value="W"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button runat="server" ID="btnSend" Text="SEND" CssClass="button2" OnClientClick="return Validate();"
                                             Visible="false" />
                                        <%--OnClick="btnSend_Click"--%>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                     <%--   <asp:Button runat="server" ID="Button2" Text="OK" CssClass="button2" OnClientClick="return Validate();"
                                            OnClick="btnOk_Click" />--%>
                                           <asp:Button runat="server" ID="btnOk" Text="OK" CssClass="button2"  
                                            OnClick="btnOk_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnCancelOtp" Text="CANCEL"
                                            CssClass="button2" OnClick="btnCancelOtp_Click" />
                                    </td>
                                </tr>
                            </table> 
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <ajax1:ModalPopupExtender ID="ModalPopupOTP" PopupControlID="divOtp" BackgroundCssClass="bagroundPopup"
                TargetControlID="btn2" runat="server">
            </ajax1:ModalPopupExtender>
            <div id="gridview">
                            <asp:GridView runat="server" ID="AdminGrid" PageSize="5" AutoGenerateColumns="false"
                                Width="100%" HeaderStyle-BackColor="ButtonShadow" HeaderStyle-ForeColor="White"
                                AlternatingRowStyle-BackColor="BlueViolet" RowStyle-Height="25px" RowStyle-ForeColor="Black"
                                AlternatingRowStyle-ForeColor="White" PagerStyle-BackColor="Black" PagerStyle-ForeColor="White">
                                <Columns>
                                    <asp:BoundField DataField="User_Name" HeaderText="Admin Name" ItemStyle-Width="300px" />
                                  <%--  <asp:BoundField DataField="EmailId" HeaderText="EmailId" ItemStyle-Width="100px" />--%>
                                    <asp:BoundField DataField="mobile" HeaderText="Mobile" ItemStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="grdCB" onclick="CheckBoxCheck(this);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
            <asp:Button ID="btn3" runat="server" Style="display: none;" /> 
            <asp:Button ID="btn2" runat="server" Style="display: none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

