<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptcheckPendingTransaction.aspx.cs" Inherits="Sugar_Report_rptcheckPendingTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        function Open(id) {
            var Action = 1;
            window.open('../Transaction/pgeTransactionEntry.aspx?id=' + id + '&Action=' + Action, "_blank");
        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js"></script>
    <script type="text/javascript" src="../../Script/jquery-1.4.2.js"></script>

    <title></title>
    <style type="text/css">
        .my-table {
            font-size: large;
            border: 1px solid;
            width: 1000px; /* Set the desired width for the table */
        }

        .my-row {
            height: 25px; /* Set the desired row height */
        }

        .my-cell {
            width: 100px; /* Set the desired column width */
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <ajax1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajax1:ToolkitScriptManager>
        <div align="center">
        </div>
        <div align="center">
   



            <asp:DataList ID="dtlAcList" runat="server">
                <HeaderTemplate>
                    <table class="my-table" width="1000px" align="center">
                        <tr class="my-row-header">
                            <td class="my-cell">DOC NO</td>
                            <td class="my-cell">Doc Date</td>
                            <td class="my-cell">Account Name</td>
                            <td class="my-cell">Name</td>
                            <td class="my-cell">Amount</td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="my-table" width="1000px" align="center">
                        <tr class="my-row">
                            <td class="my-cell">
                                <asp:LinkButton runat="server" ID="lbkTenderNo" Text='<%#Eval("doc_no") %>' OnClick="lbkTenderNo_Click"></asp:LinkButton>

                                <%--<asp:Label ID="lblAcCode" runat="server" Text='<%#Eval("DO_NO") %>'></asp:Label>--%>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label" runat="server" Text='<%#Eval("doc_Date") %>'></asp:Label>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("Account_Name") %>'></asp:Label>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Amount") %>'></asp:Label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>

        </div>
    </form>
</body>
</html>
