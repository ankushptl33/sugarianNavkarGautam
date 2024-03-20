<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeGroupDispatch.aspx.cs" Inherits="Sugar_BussinessRelated_pgeGroupDispatch" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../JS/DateValidation.js">
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


                if (hdnfClosePopupValue == "txtMILL_CODE") {
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLMILL_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGETPASS_CODE") {
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLGETPASS_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtvoucher_by") {
                    document.getElementById("<%=txtvoucher_by.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblvoucherbyname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtvoucher_by.ClientID %>").focus();
                    var vouchercity = grid.rows[SelectedRowIndex + 1].cells[3].innerText;
                }
                if (hdnfClosePopupValue == "txtGRADE") {
                    document.getElementById("<%=txtGRADE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    document.getElementById("<%=txtquantal.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtDOC_DATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").disabled = false;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION4") {
                    document.getElementById("<%=txtNARRATION4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                }
                if (hdnfClosePopupValue == "txtparty") {
                    document.getElementById("<%=txtNARRATION4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtSaleBillTo") {
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION4.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cell1[2].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID%>").focus();
                }
                if (hdnfClosePopupValue == "txtPurcNo") {
                    document.getElementById("<%=txtPurcNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtPurcOrder.ClientID %>").disabled = false;
                    document.getElementById("<%=txtPurcNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtPurcOrder.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[9].innerText;
                    document.getElementById("<%=txtquantal.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    document.getElementById("<%=txtPurcNo.ClientID%>").focus();

                    if (cs == '') {
                        if (cs == 0) {
                            document.getElementById("<%=txtquantal.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[9].innerText;
                        }
                    }
                    document.getElementById("<%=txtPurcNo.ClientID %>").focus();
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
    <script type="text/javascript" language="javascript">

        document.addEventListener('keyup', function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var key = event.key || event.keyCode;

            if (key === 'Escape' || key === 'Esc' || key === 27) {
                //                doWhateverYouWantNowThatYourKeyWasHit();

                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;

                if (hdnfClosePopupValue == "txtMILL_CODE") {

                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
               
                if (hdnfClosePopupValue == "txtGETPASS_CODE") {

                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
                }  
                if (hdnfClosePopupValue == "txtvoucher_by") {

                    document.getElementById("<%=txtvoucher_by.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtGRADE") {

                    document.getElementById("<%=txtGRADE.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtdoc_no") {

                    document.getElementById("<%=txtdoc_no.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtNARRATION4") {

                    document.getElementById("<%=txtNARRATION4.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtparty") {

                    document.getElementById("<%=txtNARRATION4.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtPurcNo") {

                    document.getElementById("<%=btntxtPurcNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtSaleBillTo") {
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").focus();
                }
                
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";

            }

        });
    </script>
    

    
    <script type="text/javascript" language="javascript">
     
        function millcode(e) { 
            debugger;
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtMILL_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMILL_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMILL_CODE.ClientID %>").val(unit);
                __doPostBack("txtMILL_CODE", "TextChanged");

            }
        }
        function distance(e) {
            if (e.keyCode == 13) {
                debugger;

                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }

        } 
        function purcno(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtPurcNo.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtPurcNo.ClientID %>").val();
                unit = "0" + unit;
                $("#<%=txtPurcNo.ClientID %>").val(unit);
                __doPostBack("txtPurcNo", "TextChanged");

            }
        } 
        function getpass(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGETPASS_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGETPASS_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGETPASS_CODE.ClientID %>").val(unit);
                __doPostBack("txtGETPASS_CODE", "TextChanged");

            }

        } 
                 
        function shipto(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtvoucher_by.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtvoucher_by.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtvoucher_by.ClientID %>").val(unit);
                __doPostBack("txtvoucher_by", "TextChanged");

            }
        } 
        function narration4(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtNARRATION4.ClientID %>").click();

            }

        } 
        function grade(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGRADE.ClientID %>").click();

            }

        } 
        function salebillto(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "txtSaleBillTo";
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnSearch.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtSaleBillTo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtSaleBillTo.ClientID %>").val(unit);
                __doPostBack("txtSaleBillTo", "TextChanged");

            }
        } 
    </script>
     
      <script type="text/javascript">
          function DeleteConform() {
              debugger;
              $("#loader").show();

              {
                  var DocNo = document.getElementById("<%= hdnflvdoc.ClientID %>").value;
                var Autoid = document.getElementById("<%= hdnf.ClientID %>").value;
                var Branch_Code = '<%= Session["Branch_Code"] %>';
                var Company_Code = '<%= Session["Company_Code"] %>';
                var Year_Code = '<%= Session["year"] %>';

                var XML = "<ROOT><GroupDispatch Doc_no='" + DocNo + "' dispatchId='" + Autoid + "' Company_Code='" + Company_Code + "' " +
                       "Year_Code='" + Year_Code + "'></GroupDispatch></ROOT>";
                var spname = "SPGroupDispatch";
                var status = document.getElementById("<%= btnDelete.ClientID %>").value;
                $.ajax({
                    type: "POST",
                    url: "../xmlExecuteDMLQry.asmx/ExecuteXMLQry",
                    data: '{XML:"' + XML + '",status:"' + status + '",spname:"' + spname + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", 
                });
                window.open('../Transaction/pgePaymentNote_Utility.aspx', "_self");
                //   ProcessXML(XML, status, spname);
            }
        }
        function Validate() {
            $("#loader").show();
            debugger;

            var StartDate = '<%= Session["Start_Date"] %>';
            var EndDate = '<%= Session["End_Date"] %>';

            var d = $("#<%=txtDOC_DATE.ClientID %>").val();
            var DocDates = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);

            StartDate = StartDate.slice(6, 11) + "/" + StartDate.slice(3, 5) + "/" + StartDate.slice(0, 2);
            EndDate = EndDate.slice(6, 11) + "/" + EndDate.slice(3, 5) + "/" + EndDate.slice(0, 2);

            if (DocDates >= StartDate && DocDates <= EndDate) {
            }
            else {
                $("#<%=txtDOC_DATE.ClientID %>").focus();
                alert('Not A Valid Date Range')
                $("#loader").hide();
                return false;
            }
             

            return true;
        }

        function pagevalidation() {
            debugger;
            try {
                $("#loader").show();
                var Doc_no = 0, dispatchId = 0;
                var status = document.getElementById("<%= btnSave.ClientID %>").value;
                var spname = "SPGroupDispatch";
                var XML = "<ROOT>";
                if (status == "Update") {
                    Doc_no = document.getElementById("<%= hdnflvdoc.ClientID %>").value;
                    dispatchId = document.getElementById("<%= hdnf.ClientID %>").value;
                }

                var d = $("#<%=txtDOC_DATE.ClientID %>").val();
                var Doc_Date = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);

                var MillCode = $("#<%=txtMILL_CODE.ClientID  %>").val() == "" ? 0 : $("#<%=txtMILL_CODE.ClientID %>").val(); 
                var mc = document.getElementById("<%= hdnfmc.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfmc.ClientID %>").value;
                var purchaseCode = $("#<%=txtPurcNo.ClientID  %>").val() == "" ? 0 : $("#<%=txtPurcNo.ClientID %>").val();
                var purchaseNmber = $("#<%=txtPurcOrder.ClientID  %>").val() == "" ? 0 : $("#<%=txtPurcOrder.ClientID %>").val();
                var getpass = $("#<%=txtGETPASS_CODE.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtGETPASS_CODE.ClientID %>").val();
                var gid = document.getElementById("<%= hdnfgp.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfgp.ClientID %>").value;
                var shipto = $("#<%=txtvoucher_by.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtvoucher_by.ClientID %>").val();
                var shiptoid = document.getElementById("<%= hdnfst.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfst.ClientID %>").value;
                var saleBillTo = $("#<%=txtSaleBillTo.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtSaleBillTo.ClientID %>").val();
                var saleBillToId = document.getElementById("<%= hdnfsb.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfsb.ClientID %>").value;
                var grade = $("#<%=txtGRADE.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtGRADE.ClientID %>").val();
                var quantal = $("#<%=txtquantal.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtquantal.ClientID %>").val();
                var packing = $("#<%=txtPACKING.ClientID  %>").val() == "" ? 0 : $("#<%=txtPACKING.ClientID %>").val();
                var bags = $("#<%=txtBAGS.ClientID  %>").val() == "" ? 0 : $("#<%=txtBAGS.ClientID %>").val();
                var millRate = $("#<%=txtmillRate.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtmillRate.ClientID %>").val();
                var saleRate = $("#<%=txtSALE_RATE.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtSALE_RATE.ClientID %>").val();
                var purchaseRate = $("#<%=txtpurchaserate.ClientID  %>").val() == "" ? 0.00 : $("#<%=txtpurchaserate.ClientID %>").val();

                var tenderdetailid = document.getElementById("<%= hdnfTenderDetailid.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfTenderDetailid.ClientID %>").value;
                var USER = '<%= Session["user"] %>';
                var Branch_Id = '<%= Session["Branch_Code"] %>';
                var Year_Code = '<%= Session["year"] %>';
                var Company_Code = '<%= Session["Company_Code"] %>';

                

                var LocalVoucherInsertUpdet;
                var Gledger_Insert = ""; Gledger_values = "";
                var Gledger_Delete = "";
                debugger;
                var DOCNO = "";
                if (status == "Save") {
                    LocalVoucherInsertUpdet = "CreatedBy='" + USER + "' ModifiedBy=''";
                }
                else {
                    LocalVoucherInsertUpdet = "ModifiedBy='" + USER + "' CreatedBy=''";
                }
                debugger;

                XML = XML + "<GroupDispatch  Doc_No='" + Doc_no + "' Doc_Date='" + Doc_Date + "' MillCode='" + MillCode + "' mc='" + mc + "' " +
                   " purchaseCode ='" + purchaseCode + "' purchaseNmber ='" + purchaseNmber + "' getpass ='" + getpass + "' gid ='" + gid + "' " +
                   " shipto ='" + shipto + "' shiptoid ='" + shiptoid + "' saleBillTo ='" + saleBillTo + "' saleBillToId ='" + saleBillToId + "' " +
                   " grade ='" + grade + "' quantal ='" + quantal + "' packing ='" + packing + "' bags ='" + bags + "' " +
                   " millRate ='" + millRate + "' saleRate ='" + saleRate + "' purchaseRate ='" + purchaseRate + "' " +
                   "Company_Code='" + Company_Code + "' Year_Code='" + Year_Code + "' " + LocalVoucherInsertUpdet + " dispatchId='" + dispatchId + "' tenderdetailid='" + tenderdetailid + "' >";


                var Order_Code = 1; 

                XML = XML + "</GroupDispatch></ROOT>";
                ProcessXML(XML, status, spname);
            }
            catch (exx) {
                $("#loader").hide();
                alert(exx)
            }

            function ProcessXML(XML, status, spname) {
                debugger;

                $.ajax({
                    type: "POST",
                    url: "../xmlExecuteDMLQry.asmx/ExecuteXMLQry",
                    data: '{XML:"' + XML + '",status:"' + status + '",spname:"' + spname + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response.d);
                        $("#loader").hide();
                    },
                    error: function (response) {
                        alert(response.d);
                        $("#loader").hide();
                    }
                });


                function OnSuccess(response) {
                    debugger;
                    $("#loader").hide();
                    if (status != "Delete") {
                        if (response.d.length > 0) {
                            var word = response.d;
                            var len = word.length;
                            var pos = word.indexOf(",");
                            var id = word.slice(0, pos);
                            var doc = word.slice(pos + 1, len);
                            if (status == "Save") {
                                alert('Sucessfully Added Record !!! Doc_no=' + doc)
                            }

                            else {
                                alert('Sucessfully Updated Record !!! Doc_no=' + doc)
                            }
                            window.open('../BussinessRelated/pgeGroupDispatch.aspx?dispatchId=' + id + '&Action=1', "_self");

                        }
                    }
                    else {
                        var num = parseInt(response.d);

                        if (isNaN(num)) {
                            alert(response.d)

                        }
                        else {
                            window.open('../BussinessRelated/pgeGroupDispatchUtility.aspx', "_self");
                        }
                    }

                }

            }

        }
    </script>
    
    <script type="text/javascript">
        function Back() {

            //alert(td);
            window.open('../BussinessRelated/pgeGroupDispatchUtility.aspx', '_self');
        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data ?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>   
    <style>
        #loader {
            width: 100%;
            height: 100%;
            background-color: gray;
            position: fixed;
            margin: -0.7%;
            opacity: 0.6;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Group Dispatch   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <div id="loader" align="center" style="display: none; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px;">
        <img height="40%" width="40%" src="../Images/ajax-loader3.gif" />
    </div>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfyearcode" runat="server" />
            <asp:HiddenField ID="hdnfTenderID" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfTenderQty" runat="server" />
            <asp:HiddenField ID="hdnfTenderDetailid" runat="server" />
            <asp:HiddenField ID="hdnfmc" runat="server" />
            <asp:HiddenField ID="hdnfmillshortname" runat="server" />
            <asp:HiddenField ID="hdnfQTY" runat="server" />
            <asp:HiddenField ID="hdnfsb" runat="server" />
            <asp:HiddenField ID="hdnfst" runat="server" />
            <asp:HiddenField ID="hdnfgp" runat="server" />
            <asp:HiddenField ID="hdnfSaleRate" runat="server" />
            <asp:HiddenField ID="hdnflvdoc" runat="server" />
            <asp:HiddenField ID="hdnfcompanycode" runat="server" />
            <asp:HiddenField ID="hdconfirm" runat="server" />
                      <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 90; float: left">
                      <table cellspacing="5" align="left">
                        <tr>
                            <td style="width: 100%;" colspan="6">
                                <table width="100%" align="left" style="border: 1px solid white;">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                    ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                                &nbsp;
                                <asp:Button ID="btnSave" runat="server" OnClientClick="if (!Validate()) return false;"
                                    Text="Save" CssClass="btnHelp" Width="90px" TabIndex="7" Height="24px" ValidationGroup="add"
                                    OnClick="btnSave_Click" />
                                &nbsp;
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                                    TabIndex="8" Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" />
                                &nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                                    Height="24px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()"
                                    TabIndex="9" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                                    TabIndex="10" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                                    ValidationGroup="save" Height="24px" OnClientClick="Back();" />
                              
                            </td>

                             <td align="center">
                                            <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                                                OnClick="btnFirst_Click" Width="90px" Height="24px" />
                                            <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="Previous" CssClass="btnHelp"
                                                OnClick="btnPrevious_Click" Width="90px" Height="24px" />
                                            <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="Next" CssClass="btnHelp"
                                                OnClick="btnNext_Click" Width="90px" Height="24px" />
                                            <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="Last" CssClass="btnHelp"
                                                OnClick="btnLast_Click" Width="90px" Height="24px" />
                                        </td>
                        </tr>
                    </table>
                            </tr>
                      <tr>
                      <td align="left">Change No:</td>
                            <td align="left" colspan="2">
                                <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                    TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                    TargetControlID="txtEditDoc_No">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                    Font-Size="Small" ForeColor="Red"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Entry No: &nbsp;
                            </td>
                            <td align="left" colspan="7" style="width: 100%;">
                                <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                    AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                    CssClass="btnHelp" Height="24px" />
                                <asp:Label ID="lbldoid" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                    Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                  Date:
                                <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="2" Width="80px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                                </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Mill Code:
                            </td>
                            <td align="left" colspan="2" >
                                <asp:TextBox ID="txtMILL_CODE" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                    onkeydown="millcode(event);" Style="text-align: right;" AutoPostBack="false"
                                    OnTextChanged="txtMILL_CODE_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtMILL_CODE" runat="server" Text="..." OnClick="btntxtMILL_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLMILL_NAME" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp; &nbsp;<asp:Label runat="server" ID="lblPaymenToLegBal" ForeColor="Yellow"
                                    Text=""></asp:Label>
                                </td>
                                  </tr>
                          <tr>
                                  <td align="left">Purc. No:
                            </td>
                              
                            <td colspan="3" align="left">
                                <asp:TextBox ID="txtPurcNo" runat="Server" Enabled="false" CssClass="txt" Width="80px"
                                    onkeydown="purcno(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtPurcNo_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtPurcNo" runat="server" Text="..." OnClick="btntxtPurcNo_Click"
                                    TabIndex="4" CssClass="btnHelp" Height="24px" Width="20px" />
                                &nbsp;
                                <asp:TextBox ID="txtPurcOrder" runat="Server" Enabled="false" AutoPostBack="false"
                                    OnTextChanged="txtPurcNo_TextChanged" CssClass="txt" Width="90px" Style="text-align: right;"
                                    Height="24px" TabIndex="5"></asp:TextBox>
                                <asp:Label ID="lbltenderDetailID" runat="server" CssClass="lblName"></asp:Label>
                                 </td>
                        </tr>
                        <tr>
                            <td align="left">Get Pass:
                            </td>
                            <td align="left" colspan="4">
                                <asp:TextBox ID="txtGETPASS_CODE" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                    onkeydown="getpass(event);" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtGETPASS_CODE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGETPASS_CODE" runat="server" Text="..." OnClick="btntxtGETPASS_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLGETPASS_NAME" runat="server" CssClass="lblName"></asp:Label>
                                 </td>
                        </tr>
                        <tr>
                            <td align="left"  >Shipped To:
                            </td>
                            <td align="left" colspan="4"  >
                                <asp:TextBox ID="txtvoucher_by" runat="Server" CssClass="txt" TabIndex="7" Width="80px"
                                    onkeydown="shipto(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtvoucher_by_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtvoucher_by" runat="server" Text="..." OnClick="btntxtvoucher_by_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblvoucherbyname" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblVoucherLedgerByBalance" ForeColor="Yellow"
                                    Text=""></asp:Label>
                                </td>
                                 </tr>
                        <tr>
                            <td align="left">Sale Bill To:
                            </td>
                            <td align="left" colspan="4">
                                <asp:TextBox runat="server" ID="txtSaleBillTo" Height="24px" Width="80px" Enabled="false"
                                    onkeydown="salebillto(event);" CssClass="txt" OnTextChanged="txtSaleBillTo_TextChanged" AutoPostBack="false"
                                    TabIndex="8"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtNARRATION4" runat="Server" CssClass="txt" TabIndex="9" Width="200px"
                                    onkeydown="narration4(event);" Style="text-align: left;" AutoPostBack="false"
                                    OnTextChanged="txtNARRATION4_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtNARRATION4" runat="server" Text="..." OnClick="btntxtNARRATION4_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                &nbsp; &nbsp;<asp:Label runat="server" ID="lblSaleBillToLedgerByBalance" ForeColor="Yellow"
                                    Text=""></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Grade:
                            </td>
                            <td align="left" colspan="6">
                                <asp:TextBox ID="txtGRADE" runat="Server" CssClass="txt" TabIndex="10" Width="150px"
                                    onkeydown="grade(event);" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGRADE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGRADE" runat="server" Text="..." OnClick="btntxtGRADE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                Quantal:
                                <asp:TextBox ID="txtquantal" runat="Server" CssClass="txt" TabIndex="11" Width="100px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtquantal_TextChanged"
                                    Height="24px" onkeydown="qntl(event);"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="filtertxtquantal" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtquantal">
                                </ajax1:FilteredTextBoxExtender>
                                <asp:Label runat="server" ID="count" ForeColor="White" Visible="false"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Packing:&nbsp;&nbsp;
                                <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="0" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtPACKING" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtPACKING">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;Bags:&nbsp;&nbsp;<asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt"
                                    TabIndex="0" Width="100px" ReadOnly="true" Style="text-align: right;" AutoPostBack="True"
                                    OnTextChanged="txtBAGS_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtBAGS" runat="server" FilterType="Numbers"
                                    TargetControlID="txtBAGS">
                                </ajax1:FilteredTextBoxExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Mill Rate:
                            </td>
                            <td align="left" colspan="6" style="vertical-align: top;">
                                <asp:TextBox ID="txtmillRate" runat="Server" CssClass="txt" Width="100px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtmillRate_TextChanged"
                                    Height="24px" TabIndex="0" ReadOnly="true"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtmillRate" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtmillRate">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Sale
                                Rate:
                                <asp:TextBox ID="txtSALE_RATE" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="0" AutoPostBack="True" OnTextChanged="txtSALE_RATE_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtSALE_RATE" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtSALE_RATE">
                                </ajax1:FilteredTextBoxExtender>

                                Purchase Rate:
                                <asp:TextBox ID="txtpurchaserate" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="0" AutoPostBack="True" OnTextChanged="txtpurchaserate_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="Filteredtxtpurchaserate" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtpurchaserate">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;
                                </td>
                          </tr>
                      </table>
                 </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>
    <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="90%"
                ScrollBars="Both" align="center" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: left; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 5%; top: 10%;">
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
                        <td style="width: 100%;" align="left" colspan="2">
                            <table id="Table1" runat="server" width="100%">
                                <tr>
                                    <td style="width: 40%;">Search Text:
                                        <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                            Width="250px" Height="20px" AutoPostBack="false" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                                        &nbsp;<asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server"
                                            Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                                    </td>
                                    <td align="left" runat="server" id="tdDate" visible="false">From:
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" ></asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                        To:
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="CalendarExtendertxtToDate" runat="server" TargetControlID="txtToDate"
                                            PopupButtonID="Image2" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 500px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="15" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                                    <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                    <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                                    <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td>
                    </tr>
                </table>
            </asp:Panel>
</asp:Content>

