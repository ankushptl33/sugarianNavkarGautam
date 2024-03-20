<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Async.aspx.cs" Inherits="Sugar_Transaction_Async" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:Button ID="btnSave" runat="server"
                                Text="Async" CssClass="btnHelp" Width="90px" Height="24px" 
                                OnClick="btnasync_Click" TabIndex="55" />
     <asp:Label ID="LblPartyname" runat="server" CssClass="lblName"></asp:Label>
</asp:Content>

