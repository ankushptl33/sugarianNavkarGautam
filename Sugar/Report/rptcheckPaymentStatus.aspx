<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptcheckPaymentStatus.aspx.cs" Inherits="Sugar_Report_rptcheckPaymentStatus" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptcheckPaymentStatus.aspx.cs" Inherits="Sugar_Report_rptcheckPaymentStatus" Async="true" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <script type="text/javascript">
        function showLoader() {
            debugger;
            document.getElementById("loader").style.display = "block";
        }

        function hideLoader() {
            document.getElementById("loader").style.display = "none";
        }
</script>

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
  <style type="text/css">
  .loader-container {
    display: none;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(255, 255, 255, 0.8);
    z-index: 9999;
    text-align: center;
}

.loader {
    border: 5px solid #f3f3f3;
    border-top: 5px solid #3498db;
    border-radius: 50%;
    width: 50px;
    height: 50px;
    animation: spin 2s linear infinite;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

</style>

</head>
<body>
    <form id="form1" runat="server">  
        <div id="loader" class="loader-container">
              <div class="loader"></div>
            
        </div>
        <div align="center" > 
            <asp:DataList ID="dtlAcList" runat="server">
                <HeaderTemplate>
                    <table class="my-table" width="1000px" align="center">
                        <tr class="my-row-header">
                            <td style="width: 0px;">DOC NO</td> 
                            <td style="width: 10px; text-align:center;" >MessageId</td> 
                            <td style="width: 30px; text-align:center;" >detailid</td>
                            <td style="width: 12px; text-align:center;"></td>
                            <td style="width: 30px; text-align:center;">Date</td> 
                            <td style="width: 61px; text-align:center;"></td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="my-table" width="1200px" align="center">
                        <tr class="my-row">
                            <td style="width: 90px;">
                               
                                 <asp:Label ID="Label3" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                                <%--<asp:Label ID="lblAcCode" runat="server" Text='<%#Eval("DO_NO") %>'></asp:Label>--%>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label" runat="server" Text='<%#Eval("MessageId") %>'></asp:Label>
                            </td>
                            <td class="my-cell">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("detailid") %>'></asp:Label>
                                 </td> 
                            <td class="my-cell"> 
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("TranctionDateConverted") %>'></asp:Label>
                            </td> 
                            <td class="my-cell"> 
                                      <asp:Button ID="btncheckStatus" Width="100px"  Height="25px" runat="server" Text="Check Status" OnClick="btncheckStatus_Click" OnClientClick="showLoader()" CommandArgument='<%#Eval("detailid") %>'></asp:Button>
                                

                            </td> 
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>

        </div>
    </form>   
</body>
</html>
