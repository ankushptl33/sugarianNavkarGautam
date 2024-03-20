<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeBankTransaction.aspx.cs" Inherits="Sugar_Transaction_pgeBankTransaction" %>

<%@ MasterType VirtualPath="~/MasterPage2.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

     

     

     


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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


                <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;" BackColor="Yellow">
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
                            <td align="right" style="width: 30%;">Ac Code:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtAcCode" runat="Server" CssClass="txt" TabIndex="1" Width="90px"
                                    Style="text-align: right;" AutoPostBack="true" Height="24px"
                                    ></asp:TextBox> 
                                <asp:Label ID="lblAcName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Name:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtName" runat="Server" CssClass="txt" TabIndex="2" Width="160px"
                                    Style="text-align: left;" AutoPostBack="True" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Account Number:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtAccountNumber" runat="Server" CssClass="txt" TabIndex="3" Width="160px"
                                    Style="text-align: left;" AutoPostBack="false" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">IFSC Code:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtIFSCCode" runat="Server" CssClass="txt" TabIndex="4" Width="160px"
                                    Style="text-align: left;" AutoPostBack="false" Height="24px"></asp:TextBox>
                            </td>
                        </tr>  
                        <tr>
                            <td align="right" style="width: 30%;">Amount:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtamount" runat="Server" CssClass="txt" TabIndex="7" Width="160px"
                                    Style="text-align: right;" AutoPostBack="false" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                            <tr>
                            <td align="right" style="width: 30%;">Remark:
                            </td>
                            <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtRemark" runat="Server" CssClass="txt" TabIndex="9" Width="260px"
                                    Style="text-align: left;" AutoPostBack="false" TextMode="MultiLine" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;">Narration:
                            </td>
                           <td align="left" style="width: 30%;">
                                <asp:TextBox ID="txtnarration" runat="Server" CssClass="txt" TabIndex="11" Width="260px"
                                    Style="text-align: left;" TextMode="MultiLine" AutoPostBack="false" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 30%;"></td>
                            <td align="left" style="width: 30%;">
                                <asp:Button ID="btnPay" runat="server"
                                    Text="Pay" CssClass="btnHelp" Width="90px" TabIndex="9" Height="24px" ValidationGroup="add"
                                    OnClick="btnPay_Click" />
                                &nbsp;&nbsp; 
                            </td>
                        </tr>
                    </table>
                </asp:Panel>  
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

