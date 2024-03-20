<%@ Page Title="Payments" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Async="true"
    CodeFile="pgePayments.aspx.cs" Inherits="Sugar_pgePayments" %>

<%@ MasterType VirtualPath="~/MasterPage2.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        debugger;
        document.addEventListener('keyup', function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var key = event.key || event.keyCode;

            if (key === 'Escape' || key === 'Esc' || key === 27) {
                // doWhateverYouWantNowThatYourKeyWasHit();
                debugger;
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtTdsAccount") {
                    document.getElementById("<%=txtTdsAccount.ClientID %>").focus();
                }

                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                document.getElementById("<%= txtSearchText.ClientID %>").value = "";
            }

        });
    </script>
    <script type="text/javascript">
        function SB() {
            var billno = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            var TranType = document.getElementById('<%=drpPaymentFor.ClientID %>').value;
            window.open('./rptPendingPayments.aspx?Doc_no=' + billno + '&TranType=' + TranType)
        }</script>

    <script type="text/javascript" language="javascript">
        function GrdAmount(e, obj) {
            debugger;
            if (e.keyCode == 39) {
                var td = $(obj).parent().next("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().next("tr");
                    if (tr.length > 0) {
                        tr.next("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }

            if (e.keyCode == 37) {
                var td = $(obj).parent().prev("td");
                if (td.length > 0) {

                    var aaa = $(obj).closest("tr").prev().find("input[type=text]");

                    aaa[7].focus();
                }
                else {
                    var tr = $(obj).parent().parent().prev("tr");
                    if (tr.length > 0) {
                        tr.prev("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }
            if (e.keyCode == 40) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").next().find("input[type=text]");


                    aaa[0].focus();

                }

            }
            if (e.keyCode == 38) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").prev().find("input[type=text]");
                    aaa[0].focus();

                }

            }

            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();


            }

        }
        //---------------------------------------
        function GrdAdjAmount(e, obj) {
            debugger;
            if (e.keyCode == 39) {
                var td = $(obj).parent().next("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().next("tr");
                    if (tr.length > 0) {
                        tr.next("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }

            if (e.keyCode == 37) {
                var td = $(obj).parent().prev("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().prev("tr");
                    if (tr.length > 0) {
                        tr.prev("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }
            if (e.keyCode == 40) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").next().find("input[type=text]");


                    aaa[6].focus();

                }

            }
            if (e.keyCode == 38) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").prev().find("input[type=text]");
                    aaa[6].focus();

                }

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();


            }

        }

        //--------------------------------------

        function OnAc(e, obj) {
            debugger;
            if (e.keyCode == 39) {
                var td = $(obj).parent().next("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().next("tr");
                    if (tr.length > 0) {
                        tr.next("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }

            if (e.keyCode == 37) {
                var td = $(obj).parent().prev("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().prev("tr");
                    if (tr.length > 0) {
                        tr.prev("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }
            if (e.keyCode == 40) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").next().find("input[type=text]");


                    aaa[6].focus();

                }

            }
            if (e.keyCode == 38) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").prev().find("input[type=text]");
                    aaa[6].focus();

                }

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();


            }

        }
        //----------------------------------------
        function Narration(e, obj) {
            debugger;
            if (e.keyCode == 39) {
                var td = $(obj).parent().next("td");
                if (td.length > 0) {

                    var aaa = $(obj).closest("tr").next().find("input[type=text]");
                    aaa[0].focus();

                }
                else {
                    var tr = $(obj).parent().parent().next("tr");
                    if (tr.length > 0) {
                        tr.next("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }

            if (e.keyCode == 37) {
                var td = $(obj).parent().prev("td");
                if (td.length > 0) {
                    td.find("input[type='text']").focus();
                }
                else {
                    var tr = $(obj).parent().parent().prev("tr");
                    if (tr.length > 0) {
                        tr.prev("tr").find("td").first().find("input[type='text']").focus();

                    }

                }

            }
            if (e.keyCode == 40) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").next().find("input[type=text]");


                    aaa[7].focus();

                }

            }
            if (e.keyCode == 38) {
                var td = $(obj).parent().prev("td");

                var tr = $(obj).parent().parent().prev("tr");
                if (tr.length > 0) {
                    var $ReciveNo_zero = $(obj).find('input');
                    //var $usr = $(obj).find('input[name$=openid_username]');
                    var aaa = $(obj).closest("tr").prev().find("input[type=text]");
                    aaa[7].focus();

                }

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();


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
    </script>
    <script type="text/javascript">
        function Back() {
            window.open('../Transaction/pgeMultipleReceipt_utility.aspx', '_self');
        }
        function textgrid() {
            debugger;

        }
    </script>
    <script type="text/javascript" language="javascript">
        function cashbank(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtCashBank.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtCashBank.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtCashBank.ClientID %>").val(unit);
                __doPostBack("txtCashBank", "TextChanged");

            }
        }
        function accode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtACCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtACCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtACCode.ClientID %>").val(unit);
                __doPostBack("txtACCode", "TextChanged");

            }
        }

        function tdsaccode(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnTdsAccount.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtTdsAccount.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtTdsAccount.ClientID %>").val(unit);
                __doPostBack("txtTdsAccount", "TextChanged");

            }
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
                debugger;
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
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCashBank.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtCashBank.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblACName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtTdsAccount") {
                    document.getElementById("<%=txtTdsAccount.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblTdsAcName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtTdsAccount.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_no.ClientID %>").focus();
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
        function DeleteConform() {
            debugger;
            $("#loader").show();
            var DocNo = document.getElementById("<%= hdnfdoc.ClientID %>").value;
            var Autoid = document.getElementById("<%= hdnfid.ClientID %>").value;
            var PaymentFor = $("#<%=drpPaymentFor.ClientID %>").val();
            var Branch_Code = '<%= Session["Branch_Code"] %>';
            var Company_Code = '<%= Session["Company_Code"] %>';
            var Year_Code = '<%= Session["year"] %>';

            var XML = "<ROOT><MultipleHead doc_no='" + DocNo + "' mr_no='" + Autoid + "' Company_Code='" + Company_Code + "' " +
                "Year_Code='" + Year_Code + "' Tran_Type='" + PaymentFor + "'></MultipleHead></ROOT>";
            var spname = "MultipleReceipt";
            var status = document.getElementById("<%= btnDelete.ClientID %>").value;
            ProcessXML(XML, status, spname, PaymentFor);
        }
        function Validate() {
            debugger;
            $("#loader").show();

            var StartDate = '<%= Session["Start_Date"] %>';
            var EndDate = '<%= Session["End_Date"] %>';

            var d = $("#<%=txtdoc_date.ClientID %>").val();
            var DocDates = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);

            StartDate = StartDate.slice(6, 11) + "/" + StartDate.slice(3, 5) + "/" + StartDate.slice(0, 2);
            EndDate = EndDate.slice(6, 11) + "/" + EndDate.slice(3, 5) + "/" + EndDate.slice(0, 2);

            if (DocDates >= StartDate && DocDates <= EndDate) {
            }
            else {
                $("#<%=txtdoc_date.ClientID %>").focus();
                //alert('Not A Valid Date Range')
                $("#loader").hide();
                //return false;
            }

            if ($("#<%=txtCashBank.ClientID %>").val() == "" || $("#<%=txtCashBank.ClientID %>").val() == "0") {
                $("#<%=txtCashBank.ClientID %>").focus();
                $("#loader").hide();
                return false;

            }
            var gridView = document.getElementById("<%=grdDetail.ClientID %>");
            if (gridView == null) {
                alert('Please Enter Details!');
                $("#loader").hide();
                return false;
            }
            var grdrow = gridView.getElementsByTagName("tr");
            var ro = (grdrow.length - 1);
            if (ro == 0) {
                alert('Please Enter Details!');
                $("#loader").hide();
                return false;
            }
            var trntype = $("#<%=drpPaymentFor.ClientID %>").val();
            if (trntype == "BP" || trntype == "BR") {
                if ($("#<%=txtCashBank.ClientID %>").val() == "" || $("#<%=txtCashBank.ClientID %>").val() == "0") {
                    $("#<%=txtCashBank.ClientID %>").focus();
                    $("#loader").hide();
                    return false;

                }
            }
            if ($("#<%=txtBalance.ClientID %>").val() != "0") {
                $("#loader").hide();
                alert('Balance Must Be Zero')
                return false;
            }
            return true;
        }

        function validation() {
            debugger;
            $("#loader").show();
            var Doc_No = 0, mr_no = 0, mrd_no = 0;

            var status = document.getElementById("<%= btnSave.ClientID %>").value;
            var spname = "MultipleReceipt";
            var XML = "<ROOT>";
            var DOC_NO = $("#<%=txtdoc_no.ClientID %>").val();
            if (status == "Update") {
                Doc_No = document.getElementById("<%= hdnfdoc.ClientID %>").value;
                mr_no = document.getElementById("<%= hdnfid.ClientID %>").value;
            }

            var d = $("#<%=txtdoc_date.ClientID %>").val();

            var narrationledger = $("#<%=txtnarrationLedger.ClientID %>").val();
            var doc_date = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);
            var cashBank = $("#<%=txtCashBank.ClientID %>").val() == "" ? 0 : $("#<%=txtCashBank.ClientID %>").val();
            var AcCode = $("#<%=txtACCode.ClientID %>").val() == "" ? 0 : $("#<%=txtACCode.ClientID %>").val();
            var Amount = $("#<%=txtAmount.ClientID %>").val() == "" ? 0 : $("#<%=txtAmount.ClientID %>").val();
            var Branch_Code = '<%= Session["Branch_Code"] %>';
            var Company_Code = '<%= Session["Company_Code"] %>';
            var Year_Code = '<%= Session["year"] %>';
            var USER = '<%= Session["user"] %>';
            var PaymentFor = $("#<%=drpPaymentFor.ClientID %>").val();
            var bc = document.getElementById("<%= hdnfbankid.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfbankid.ClientID %>").value;
            if (bc == "" || bc == "&nbsp;") {
                bc = "0";
            }
            var ac = document.getElementById("<%= hdnfacid.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfacid.ClientID %>").value;
            if (ac == "" || ac == "&nbsp;") {
                ac = "0";
            }
            var DOCNO = "";
            if (status == "Save") {
                DOCNO = "Tran_Type='" + PaymentFor + "'";
            }
            else {
                DOCNO = "Tran_Type='" + PaymentFor + "' doc_no='" + Doc_No + "'";
            }
            XML = XML + "<MultipleHead " + DOCNO + " mr_no='" + mr_no + "' Doc_Date='" + doc_date + "' Bank_Code='" + cashBank + "' bc='" + bc + "' Ac_Code='" + AcCode + "' ac='" + ac + "' " +
                "Amount='" + Amount + "' Company_Code='" + Company_Code + "' Year_Code='" + Year_Code + "' Created_By='" + USER + "' Modified_By='" + USER + "' narration='" + narrationledger + "'>";

            var grid = document.getElementById("<%= grdDetail.ClientID%>");
            var ddid = mrd_no;
            var narr = "";
            var id = 1;
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var val = $("input[id*=txtgrdAmount]")
                if (val[i].value != "0" && val[i].value != '') {
                    // var detail_Id = grid.rows[i + 1].cells[0].innerHTML;
                    var detail_Id = id;

                    var Bill_Tran_Date = grid.rows[i + 1].cells[1].innerHTML;
                    Bill_Tran_Date = Bill_Tran_Date.slice(6, 11) + "/" + Bill_Tran_Date.slice(3, 5) + "/" + Bill_Tran_Date.slice(0, 2);
                    var Party_Code = grid.rows[i + 1].cells[2].innerHTML;
                    var Unit_code = "";
                    if (PaymentFor == "TP") {
                        Unit_code = grid.rows[i + 1].cells[4].innerHTML;
                        uc = "0";
                    }
                    else {
                        Unit_code = grid.rows[i + 1].cells[4].innerHTML;
                    }
                    var Unit_name = grid.rows[i + 1].cells[5].innerHTML;
                    if (Unit_name == "&nbsp;") {
                        Unit_name = "";
                    }
                    var Mill_Code = grid.rows[i + 1].cells[6].innerHTML;
                    var LorryNo = grid.rows[i + 1].cells[8].innerHTML = "" ? 0 : grid.rows[i + 1].cells[8].innerHTML;
                    if (LorryNo == "&nbsp;") {
                        LorryNo = "";
                    }
                    var Bill_No = grid.rows[i + 1].cells[9].innerHTML;
                    var Bill_Tran_Type = grid.rows[i + 1].cells[10].innerHTML;

                    var Quntal = grid.rows[i + 1].cells[11].innerHTML;
                    var Bill_Amount = grid.rows[i + 1].cells[12].innerHTML;
                    var Bill_Receipt = grid.rows[i + 1].cells[13].innerHTML;
                    var Bill_Balance = grid.rows[i + 1].cells[14].innerHTML;
                    var txtValue = $("input[id*=txtgrdAmount]")
                    var txtAdj_Value = $("input[id*=txtgrdAdjustedAmount]")
                    var txtNarration = $("input[id*=txtNarration]")
                    var txtOnAc = $("input[id*=txtOnAc]")

                    //if (txtValue[i].value != '') {
                    //    alert(txtAmountReceive[i].value);
                    //}
                    var Value = txtValue[i].value == "" ? 0 : txtValue[i].value;
                    var Adj_Value = txtAdj_Value[i].value == "" ? 0 : txtAdj_Value[i].value;
                    var Onac = txtOnAc[i].value == "" ? 0 : txtOnAc[i].value;

                    var Narration = txtNarration[i].value;
                    if (Narration == "&nbsp;") {
                        Narration = "";
                    }
                    var Bill_Year_Code = grid.rows[i + 1].cells[19].innerHTML;
                    var mrdno = grid.rows[i + 1].cells[20].innerHTML;
                    if (mrdno == "" || mrdno == "&nbsp;") {
                        mrdno = "0";
                    }
                    var Bill_Company_Code = grid.rows[i + 1].cells[21].innerHTML;
                    var billAuto_id = grid.rows[i + 1].cells[22].innerHTML;
                    var pc = grid.rows[i + 1].cells[23].innerHTML;
                    var uc = grid.rows[i + 1].cells[24].innerHTML;
                    var mc = grid.rows[i + 1].cells[25].innerHTML;
                    var millname = grid.rows[i + 1].cells[26].innerHTML;
                    var bill_comp_code = "";

                    var XML = XML + "<MultipleDetail Tran_Type='" + PaymentFor + "' DOC_NO='" + Doc_No + "' mr_no='" + mr_no + "' detail_Id='" + detail_Id + "' " +
                        "Bill_No='" + Bill_No + "' Bill_Tran_Type='" + Bill_Tran_Type + "' Bill_Tran_Date='" + Bill_Tran_Date + "' Party_Code='" + Party_Code + "' pc='" + pc + "' " +
                        "Unit_code='" + Unit_code + "' uc='" + uc + "' Mill_Code='" + Mill_Code + "' mc='" + mc + "' Quntal='" + Quntal + "' Bill_Amount='" + Bill_Amount + "' " +
                        "Bill_Receipt='" + Bill_Receipt + "' Bill_Balance='" + Bill_Balance + "' Value='" + Value + "' Adj_Value='" + Adj_Value + "' Narration='" + Narration + "' " +
                        "Bill_Year_Code='" + Bill_Year_Code + "' Bill_Auto_Id='" + billAuto_id + "' Year_Code='" + Year_Code + "' Doc_Date='" + doc_date +
                        "' bill_comp_code='" + Bill_Company_Code + "' OnAc='" + Onac + "' LorryNo='" + LorryNo + "' Company_Code='" + Company_Code + "'/>";

                    ddid = ddid + 1;
                    if (PaymentFor == "TP") {
                        narr = narr + " DONO:" + Bill_No + " Lorry:" + LorryNo + " Qntl:" + Quntal + " Amount:" + Value + " Shipto:" + Unit_name + " mill:" + millname;


                    }
                    else {
                        narr = narr + " Bill No=" + Bill_No + ",BillAmount=" + Value;
                    }
                    id = id + 1;
                }
            }

            var Order_Code = 1;
            var drcr = "", drcr0 = "";
            if (PaymentFor == "AB" || PaymentFor == "AS" || PaymentFor == "AF") {
                drcr = "C";
                drcr0 = "D";
            }
            else {
                drcr = "D";
                drcr0 = "C";
            }
            if (Amount > 0) {
                XML = XML + "<Ledger TRAN_TYPE='" + PaymentFor + "' CASHCREDIT='' DOC_NO='" + Doc_No + "' DOC_DATE='" + doc_date + "' AC_CODE='" + cashBank + "' " +
                    "UNIT_code='0' NARRATION='" + narr + "' AMOUNT='" + Amount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                    "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='" + drcr0 + "' DRCR_HEAD='" + AcCode + "' ADJUSTED_AMOUNT='0' Branch_Code='" + Branch_Code + "' " +
                    "SORT_TYPE='" + PaymentFor + "' SORT_NO='" + Doc_No + "' ac='" + bc + "' vc='0' progid='0' tranid='0'/>";

                Order_Code = Order_Code + 1;
                XML = XML + "<Ledger TRAN_TYPE='" + PaymentFor + "' CASHCREDIT='' DOC_NO='" + Doc_No + "' DOC_DATE='" + doc_date + "' AC_CODE='" + AcCode + "' " +
                    "UNIT_code='0' NARRATION='" + narr + "' AMOUNT='" + Amount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                    "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='" + drcr + "' DRCR_HEAD='" + cashBank + "' ADJUSTED_AMOUNT='0' Branch_Code='" + Branch_Code + "' " +
                    "SORT_TYPE='" + PaymentFor + "' SORT_NO='" + Doc_No + "' ac='" + ac + "' vc='0' progid='0' tranid='0'/>";
            }

            XML = XML + "</MultipleHead></ROOT>";
            ProcessXML(XML, status, spname, PaymentFor);

        }
        function ProcessXML(XML, status, spname, PaymentFor) {
            debugger;

            $.ajax({
                type: "POST",
                url: "../xmlExecuteDMLQry.asmx/ExecuteXMLQry",
                data: '{XML:"' + XML + '",status:"' + status + '",spname:"' + spname + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    $("#loader").hide();
                    alert(response.d);
                },
                error: function (response) {
                    $("#loader").hide();
                    alert(response.d);
                }
            });
            var action = 1;
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
                        window.open('../Transaction/pgeCarporateReciept.aspx?mr_no=' + id + '&Action=1&tran_type=' + PaymentFor, "_self");

                    }
                }
                else {
                    var num = parseInt(response.d);

                    if (isNaN(num)) {
                        alert(response.d)

                    }
                    else {
                        window.open('../Transaction/pgeMultipleReceipt_utility.aspx', "_self");
                    }
                }
            }
        }
        function DoOPen(DO, tran_type) {
            var Action = 1;
            window.open('../Transaction/pgeCarporateReciept.aspx?mr_no=' + DO + '&Action=' + Action + '&tran_type=' + PaymentFor, "_self");
        }

        function EntrySuccess(type, doc_no, id) {
            $("#loader").hide();
            var PaymentFor = $("#<%=drpPaymentFor.ClientID %>").val();
            if (type == "Save") {
                alert('Sucessfully Added Record !!! Doc_no=' + doc_no)
            }
            else {
                alert('Sucessfully Updated Record !!! Doc_no=' + doc_no)
            }
            window.open('../Payments/pgePayments.aspx?tranid=' + id + '&Action=1&tran_type=' + PaymentFor, "_self");
        }

        function DeleteSuccess(type, doc_no, id) {
            $("#loader").hide();
            var PaymentFor = $("#<%=drpPaymentFor.ClientID %>").val();
            if (type == "Delete") {
                alert('Sucessfully Deleted Record!')
            }
            window.open('../Payments/pgePayments.aspx?tranid=' + id + '&Action=1&tran_type=' + PaymentFor, "_self");
        }

        function EntryFailure() {
            $("#loader").hide();
            alert('Something went wrong! please contact admin.');
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
    <%--<script language="JavaScript" type="text/javascript">

         window.onbeforeunload = function (e) {
             var e = e || window.event;
             if (e) e.returnValue = 'Browser is being closed, is it okay?'; //for IE & Firefox
             return 'Browser is being closed, is it okay?'; // for Safari and Chrome
         };

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="loader" align="center" style="display: none; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px;">
        <img height="40%" width="40%" src="../Images/ajax-loader3.gif" />
    </div>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfTransportBalance" runat="server" />
            <asp:HiddenField ID="hdnfbankid" runat="server" />
            <asp:HiddenField ID="hdnfacid" runat="server" />
            <asp:HiddenField ID="hdnfdoc" runat="server" />
            <asp:HiddenField ID="hdnfid" runat="server" />
            <asp:HiddenField ID="hdnfyearcode" runat="server" />
            <asp:HiddenField ID="hdnfcompanycode" runat="server" />


            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px; border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Bank Payments  " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table align="left" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Payment For:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpPaymentFor" runat="server" CssClass="ddl" Width="500px"
                                Height="24px" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpPaymentFor_SelectedIndexChanged">
                                <asp:ListItem Text="Mill Payment" Value="MP"></asp:ListItem>
                                <asp:ListItem Text="Against Frieght" Value="AF"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <%--<td align="left">Change No:</td>--%>
                        <td align="left">
                          
                        </td>
                    </tr>
                    <tr>

                        <td align="right" style="width: 10%;">Entry No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px"
                                CssClass="btnHelp" Height="24px" Visible="false" />
                            <asp:Label ID="lblmr_no" Visible="false" runat="server" CssClass="lblName"></asp:Label>
                            <ajax1:FilteredTextBoxExtender ID="Filteretxtdoc_no" runat="server" TargetControlID="txtdoc_no"
                                FilterType="Numbers">
                            </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;<asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp; Date:<asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt"
                                TabIndex="4" Width="90px" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                onkeydown="return DateFormat(this,event.keyCode)" Style="text-align: left;" AutoPostBack="True"
                                OnTextChanged="txtdoc_date_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtdoc_date"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                        <td align="right" style="width: 10%;"></td>
                        <td align="left"></td>
                    </tr>
                    <tr>
                        <td align="right">Bank:
                        </td>
                        <td align="left" colspan="5">
                            <asp:TextBox ID="txtCashBank" runat="server" CssClass="txt" Style="text-align: right;"
                                onkeydown="cashbank(event);" AutoPostBack="false" Width="80px" TabIndex="5" OnTextChanged="txtCashBank_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtCashBank" runat="server" Text="..." OnClick="btntxtCashBank_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblCashBank" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Payment To:
                        </td>
                        <td align="left" style="width: 10%;" colspan="5">
                            <asp:TextBox ID="txtACCode" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                onkeydown="accode(event);" TabIndex="6" AutoPostBack="false" OnTextChanged="txtACCode_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtACCode" runat="server" Text="..." OnClick="btntxtACCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblACName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">TDS Account:
                        </td>
                        <td align="left" style="width: 10%;" colspan="5">
                            <asp:TextBox ID="txtTdsAccount" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                onkeydown="tdsaccode(event);" TabIndex="6" AutoPostBack="false" OnTextChanged="txtTdsAccount_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnTdsAccount" runat="server" Text="..." OnClick="btnTdsAccount_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblTdsAcName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td align="right" style="width: 10%;">
                            Unit:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;" colspan="5">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>--%>
                    
                    <tr>
                        <td align="right">Amount:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtAmount" Height="24px" CssClass="txt" Width="150px"
                                Style="text-align: right;" TabIndex="7" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filterAmount" runat="server" TargetControlID="txtAmount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                         <td align="right">TDS Percent %:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtTdsPercent" Height="24px" CssClass="txt" Width="150px"
                                Style="text-align: right;" TabIndex="7" AutoPostBack="true" OnTextChanged="txtTdsPercent_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTdsPercent"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            TDS Amount: 
                             <asp:TextBox runat="server" ID="txtTdsAmount" Height="24px" CssClass="txt" Width="150px"
                                Style="text-align: right;" TabIndex="7" AutoPostBack="true" OnTextChanged="txtTdsAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtTdsAmount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>&nbsp;
                            Balance:&nbsp;<asp:TextBox runat="server" ID="txtBalance" Width="100px" Height="24px"
                            Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Button runat="server" ID="btnGetvouchers" Text="Get Vouchers" Width="100px"
                                CssClass="btnHelp" Height="24px" OnClick="btnGetvouchers_Click" TabIndex="9" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--Narration:--%>
                            <asp:TextBox Visible="false" runat="server" ID="txtnarrationLedger" Height="40px" CssClass="txt" Width="250px" TextMode="MultiLine"
                                Style="text-align: left;" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative;">
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="300px" Width="100%"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" OnRowCommand="grdDetail_RowCommand"
                                CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="Detail_Id" HeaderText="ID"/>
                                    <asp:BoundField DataField="doc_no" HeaderText="Doc #" />
                                    <asp:BoundField DataField="doid" HeaderText="DO #" />
                                    <asp:BoundField DataField="Doc_date" HeaderText="Date" />
                                    <asp:BoundField DataField="tran_type" HeaderText="Tran Type" />
                                    <asp:BoundField DataField="quantal" HeaderText="Quintal" />
                                    <asp:BoundField DataField="millshortname" HeaderText="Mill" />
                                    <asp:BoundField DataField="LorryNo" HeaderText="LorryNo" />
                                    <asp:BoundField DataField="amounttopay" HeaderText="Amount To Pay" />
                                    <asp:BoundField DataField="Received" HeaderText="Received" />
                                    <asp:BoundField DataField="balance" HeaderText="Balance" />                                    
                                    <%--<asp:BoundField DataField="adjusted" HeaderText="Adjusted" />--%>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtgrdAmount" CssClass="txt" AutoPostBack="true"
                                                onKeyDown="GrdAmount(event,this);" TabIndex="10" Width="80px" Height="24px" Style="text-align: right;" Text='<%#Eval("amount") %>' OnTextChanged="txtgrdAmount_TextChanged"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="filterAmount1" runat="server" TargetControlID="txtgrdAmount"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </ajax1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adjusted_Amount">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtgrdAdjustedAmount" CssClass="txt" AutoPostBack="true"
                                                onKeyDown="GrdAdjAmount(event,this);" TabIndex="11" Width="80px" Height="24px" Style="text-align: right;" Text='<%#Eval("adjusted_amount") %>' OnTextChanged="txtgrdAdjustedAmount_TextChanged"> </asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="filterAmount2" runat="server" TargetControlID="txtgrdAdjustedAmount"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </ajax1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Narration">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtNarration" CssClass="txt" AutoPostBack="true"
                                                onKeyDown="Narration(event,this);" TabIndex="13" Width="250px" Height="24px" Style="text-align: left;" Text='<%#Eval("Narration") %>' OnTextChanged="txtNarration_TextChanged"> </asp:TextBox>
                                            <asp:Label ID="lblTenderId" runat="server" Text='<%#Eval("tenderid") %>' Style="visibility:hidden;" />
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EntryYearCode" Visible="false" HeaderText="Entry Year Code" />
                                </Columns>
                                <RowStyle Height="30px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <table width="70%" align="left">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotal" runat="server" CssClass="lblName" Font-Bold="true" Visible="false" Text="Total:"></asp:Label>&nbsp;<asp:TextBox
                            TabIndex="14" ID="txtTotal" runat="server" ReadOnly="true" CssClass="txt" Width="100px" Style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table width="80%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            TabIndex="11" ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" OnClientClick="if (!Validate()) return false;" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="12" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            TabIndex="13" Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" />
                        &nbsp;
                         <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                             TabIndex="13" Height="24px" ValidationGroup="save" OnClientClick="Confirm()" OnClick="btnDelete_Click" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="14" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                        &nbsp;
                        <asp:Button ID="btnBack" Visible="false" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                            TabIndex="14" Height="24px" ValidationGroup="save" OnClientClick="Back()" />
                        <asp:Button runat="server" ID="btnPendingPayments" Text="Pending Payments" CssClass="btnHelp"
                            Width="120px" Height="24px" OnClientClick="SB();" />

                    </td>
                    <td align="center"></td>
                </tr>
            </table>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="80%" align="center" ScrollBars="None"
                BackColor="#FFFFE4" Direction="LeftToRight" Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
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
                            <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server" Width="250px" Height="20px" AutoPostBack="false"
                                OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" CssClass="select" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records Found"
                                    HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White" AllowPaging="true"
                                    PageSize="20" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging" Style="table-layout: fixed;"
                                    OnSelectedIndexChanged="grdPopup_SelectedIndexChanged">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
