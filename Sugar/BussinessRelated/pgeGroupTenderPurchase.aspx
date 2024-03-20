<%@ Page Title="Group Tender Purchase" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeGroupTenderPurchase.aspx.cs" Inherits="Sugar_BussinessRelated_pgeGroupTenderPurchase" %>


<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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

                if (hdnfClosePopupValue == "GR") {
                    document.getElementById("<%=txtGrade.ClientID %>").focus();
                }

              
                if (hdnfClosePopupValue == "txtMillCode") {
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                }
        

                if (hdnfClosePopupValue == "BU") {
                    document.getElementById("<%=txtMemberName.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "TN") {
                    document.getElementById("<%=txtTenderNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtTenderNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=btnSearch.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "MM") {
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtitem_code") {
                    document.getElementById("<%=txtitem_code.ClientID %>").focus();
                }
               
                if (hdnfClosePopupValue == "Group") {
                    document.getElementById("<%=txtGroup.ClientID %>").focus();
                }


                if (hdnfClosePopupValue == "txtotherpaidAc") {
                    document.getElementById("<%=txtotherpaidAc.ClientID %>").focus();
                } 

                if (hdnfClosePopupValue == "txtsaudaReverseAc") {
                    document.getElementById("<%=txtsaudaReverseAc.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtmillDeffAc") {
                    document.getElementById("<%=txtmillDeffAc.ClientID %>").focus();
                 }
                if (hdnfClosePopupValue == "txtourTenderNo") {
                    document.getElementById("<%=txtourTenderNo.ClientID %>").focus();
                }
                  document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
    </script>
    <script type="text/javascript">
        function disableClick(elem) { 
            elem.disabled = true;
            $("#loader").show();
        }
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




                if (hdnfClosePopupValue == "GR") {
                    document.getElementById("<%=txtGrade.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtGrade.ClientID %>").focus();
                }
                 
                if (hdnfClosePopupValue == "BU") {

                    document.getElementById("<%=txtMemberName.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMemberName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMemberName.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "TN") {
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = "";
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtTenderNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtTenderNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = "";
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                    document.getElementById("<%=btnSearch.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtMillCode") {
                    document.getElementById("<%=txtMillCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMillName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtotherpaidAc") {
                    document.getElementById("<%=txtotherpaidAc.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                     document.getElementById("<%=lblOtherPaidName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                     document.getElementById("<%=txtotherpaidAc.ClientID %>").focus();
                } 
                if (hdnfClosePopupValue == "txtsaudaReverseAc") {
                    document.getElementById("<%=txtsaudaReverseAc.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblsaudaReverseAcName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtsaudaReverseAc.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtmillDeffAc") {
                    document.getElementById("<%=txtmillDeffAc.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                     document.getElementById("<%=lblmillDeffAcName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                     document.getElementById("<%=txtmillDeffAc.ClientID %>").focus();
                 }
                if (hdnfClosePopupValue == "txtitem_code") {
                    document.getElementById("<%=txtitem_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblitemname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;

                    document.getElementById("<%=txtitem_code.ClientID %>").focus();
                }
                
                if (hdnfClosePopupValue == "Group") {
                    document.getElementById("<%=txtGroup.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblGroup_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGroup.ClientID %>").focus();
                }


                if (hdnfClosePopupValue == "txtourTenderNo") {
                    document.getElementById("<%=txtourTenderNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblDate.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=lblQuantal.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[3].innerText;
                    document.getElementById("<%=txtourTenderNo.ClientID %>").focus();
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
        function refreshparent(source) {
            if (source == 'R') {
                window.close();
                window.opener.location = "";
                window.opener.location.reload();
            }
        }

    </script>
    <script type="text/javascript">
        function MillCode(e) { 
            if (e.keyCode == 112) { 
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnMillCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMillCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMillCode.ClientID %>").val(unit);
                __doPostBack("txtMillCode", "TextChanged");

            }

        } 
        function otherpaidAc(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnotherpaidAc.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtotherpaidAc.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtotherpaidAc.ClientID %>").val(unit);
                __doPostBack("txtotherpaidAc", "TextChanged");

            }

        }

        function ReverseAc(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnsaudaReverseAc.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtsaudaReverseAc.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtsaudaReverseAc.ClientID %>").val(unit);
                __doPostBack("txtsaudaReverseAc", "TextChanged");

            }

        }
        


        function millDeffAc(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnmillDeffAc.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtmillDeffAc.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtmillDeffAc.ClientID %>").val(unit);
                __doPostBack("txtmillDeffAc", "TextChanged");

            }

        }

        function Grade(e) {
            debugger;

            if (e.keyCode == 112) { 
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnGrade.ClientID %>").click();

            }


        }  
        function Party(e) {
            debugger;

            if (e.keyCode == 112) { 
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnMember.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMemberName.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMemberName.ClientID %>").val(unit);
                __doPostBack("txtMemberName", "TextChanged");

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }
        }  
        function item(e) {
       

            if (e.keyCode == 112) { 
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtitem_code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtitem_code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtitem_code.ClientID %>").val(unit);
                __doPostBack("txtitem_code", "TextChanged");

            }

        }
        function BP_Account(e) {
           

            if (e.keyCode == 112) { 
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnGroup.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGroup.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGroup.ClientID %>").val(unit);
                __doPostBack("txtGroup", "TextChanged");

            }

        }
        function TenderCode(e) {
            if (e.keyCode == 112) {
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnourTenderNo.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtourTenderNo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtourTenderNo.ClientID %>").val(unit);
                __doPostBack("txtourTenderNo", "TextChanged");

            }

        }
    </script>
    <script type="text/javascript">
        function Back() {

            //alert(td);
            window.open('../BussinessRelated/pgeGroupTenderPurchaseUtility.aspx', '_self');
        }

        function TenderOPen(TenderID) {
            var Action = 1;
            window.open('../BussinessRelated/pgeGroupTenderPurchase.aspx?grouptenderid=' + TenderID + '&Action=' + Action, "_self");
        }
    </script>

    <script type="text/javascript">
        function DeleteConform() {
            debugger;
            $("#loader").show();
            var DocNo = document.getElementById("<%= hdnftenderno.ClientID %>").value;
            var Tenderid = document.getElementById("<%= hdnftenderid.ClientID %>").value; 
           

            var Branch_Code = '<%= Session["Branch_Code"] %>';
            var Company_Code = '<%= Session["Company_Code"] %>';
            var Year_Code = '<%= Session["year"] %>';

            var XML = "<ROOT><GroupTenderHead Tender_No='" + DocNo + "' grouptenderid='" + Tenderid + "' Company_Code='" + Company_Code + "' " +
                "Year_Code='" + Year_Code + "'></GroupTenderHead></ROOT>";
            var spname = "SPGroupTenderPurchase";
            var status = document.getElementById("<%= btnDelete.ClientID %>").value;
            ProcessXML(XML, status, spname);
        }
        function Validate() {
            debugger;
            $("#loader").show();
            var StartDate = '<%= Session["Start_Date"] %>';
            var EndDate = '<%= Session["End_Date"] %>';

            var d = $("#<%=txtDate.ClientID %>").val();
            var DocDates = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);

            StartDate = StartDate.slice(6, 11) + "/" + StartDate.slice(3, 5) + "/" + StartDate.slice(0, 2);
            EndDate = EndDate.slice(6, 11) + "/" + EndDate.slice(3, 5) + "/" + EndDate.slice(0, 2);

            if (DocDates >= StartDate && DocDates <= EndDate) {
            }
            else {
                $("#<%=txtDate.ClientID %>").focus();
                alert('Not A Valid Date Range')
                $("#loader").hide();
                return false;
            }

            if ($("#<%=txtMemberName.ClientID %>").val() != "") {
                alert('Please add details');
                $("#<%=txtMemberName.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }

            if ($("#<%=txtMillCode.ClientID %>").val() == "") {
                $("#<%=txtMillCode.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }
            if ($("#<%=txtGrade.ClientID %>").val() == "") {
                $("#<%=txtGrade.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }
            if ($("#<%=txtQuantal.ClientID %>").val() == "") {
                $("#<%=txtQuantal.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }
            if ($("#<%=txtPacking.ClientID %>").val() == "") {
                $("#<%=txtPacking.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }
            if ($("#<%=txtBags.ClientID %>").val() == "" || $("#<%=txtBags.ClientID %>").val() == "0") {
                $("#<%=txtBags.ClientID %>").focus();
                $("#loader").hide();
                return false;
            }


         //   if ($("#<%=txtmillDeff.ClientID %>").val() != "" || $("#<%=txtmillDeff.ClientID %>").val() != "0" || $("#<%=txtmillDeff.ClientID %>").val() != "0.00" || $("#<%=txtmillDeffAc.ClientID %>").val() == "0" || $("#<%=txtmillDeffAc.ClientID %>").val() == "") {
          //      $("#<%=txtmillDeffAc.ClientID %>").focus();
           //     $("#loader").hide();
           //     return false;
           // }
            // 
            return true;


        } 
        function pagevalidation() {
            debugger;

            $("#loader").show();
            var TenderNo = 0, tenderId = 0, tdetailid = 0, commiDoc_No = 0, commisionid = 0;
            var status = document.getElementById("<%= btnSave.ClientID %>").value;
            if (status == "Update") {
                TenderNo = document.getElementById("<%= hdnftenderno.ClientID %>").value;
                tenderId = document.getElementById("<%= hdnftenderid.ClientID %>").value;
            } 
            debugger;
            //Validation
            var spname = "SPGroupTenderPurchase";
            var XML = "<ROOT>";
            var d = $("#<%=txtLiftingDate.ClientID %>").val();
            var Lifting_Date = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2); 
            var d = $("#<%=txtDate.ClientID %>").val();
            var Tender_Date = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);
            var AUTO_VOUCHER = '<%= Session["AUTO_VOUCHER"] %>';
            var Mill_Code = $("#<%=txtMillCode.ClientID %>").val(); 
            var Mill_Rate = parseFloat($("#<%=txtMillRate.ClientID %>").val()); 
            var Grade = $("#<%=txtGrade.ClientID %>").val();
            var Quantal = parseFloat($("#<%=txtQuantal.ClientID %>").val());
            var Packing = parseFloat($("#<%=txtPacking.ClientID %>").val() == "" ? 0 : $("#<%=txtPacking.ClientID %>").val());
            var Bags = parseFloat($("#<%=txtBags.ClientID %>").val() == "" ? 0 : $("#<%=txtBags.ClientID %>").val());
            var Group_Account = $("#<%=txtGroup.ClientID %>").val() == "" ? 0 : $("#<%=txtGroup.ClientID %>").val(); 
            var USER = '<%= Session["user"] %>';
            var Branch_Id = '<%= Session["Branch_Code"] %>'; 
            var gid =  $("#<%=hdnfgid.ClientID %>").val() == "" ? 0 : $("#<%=hdnfgid.ClientID %>").val();
            var myNarration = ""; 
            var str = "";
            var docno = 0;
            var Tender_No = $("#<%=txtTenderNo.ClientID %>").val();
            var Year_Code = '<%= Session["year"] %>';
            var Company_Code = '<%= Session["Company_Code"] %>';
            var itemcode = $("#<%=txtitem_code.ClientID %>").val() == "" ? 0 : $("#<%=txtitem_code.ClientID %>").val();
            var mc = document.getElementById("<%= hdnfmc.ClientID %>").value == "" ? 0 : document.getElementById("<%= hdnfmc.ClientID %>").value;
            var millDeff = $("#<%=txtmillDeff.ClientID %>").val() == "" ? 0 : $("#<%=txtmillDeff.ClientID %>").val();
            var TenderInsertUpdate = ""; TenderDetail_Insert = ""; TenderDetail_Update = ""; TenderDetail_Delete = ""; 
            var calculateStock = $("#<%=drCalculateStock.ClientID %>").val();
            var isAccounted = $("#<%=drAccounted.ClientID %>").val();
            var ourTenderNo = $("#<%=txtourTenderNo.ClientID %>").val() == "" ? 0 : $("#<%=txtourTenderNo.ClientID %>").val();
            var ourTenderNoid = $("#<%=hdnftenderhelpid.ClientID %>").val() == "" ? 0 : $("#<%=hdnftenderhelpid.ClientID %>").val();
            var MillDeffAc = $("#<%=txtmillDeffAc.ClientID %>").val() == "" ? 0 : $("#<%=txtmillDeffAc.ClientID %>").val(); 
            var MillDeffacid = $("#<%=hdnfmillDiffacid.ClientID %>").val() == "" ? 0 : $("#<%=hdnfmillDiffacid.ClientID %>").val(); 
            var Narration = $("#<%=txtNarration.ClientID %>").val() == "" ? 0 : $("#<%=txtNarration.ClientID %>").val();
            if (status == "Save") {
                TenderInsertUpdate = "Created_By='" + USER + "'";
            }
            else {
                TenderInsertUpdate = "Modified_By='" + USER + "'";
            } 
            debugger;
            XML = XML + "<GroupTenderHead Tender_No='" + TenderNo + "' Company_Code='" + Company_Code + "' Tender_Date='" + Tender_Date + "' " +
                        " Lifting_Date='" + Lifting_Date + "' Mill_Code='" + Mill_Code + "' " +
                        " mc='" + mc + "' Grade='" + Grade + "' Quantal='" + Quantal + "' Packing='" + Packing + "' Bags='" + Bags + "' " +
                        " Mill_Rate='" + Mill_Rate + "' MillDeff='" + millDeff + "' " + TenderInsertUpdate + " Year_Code='" + Year_Code + "' "+ 
                        " grouptenderid='" + tenderId + "' Group_Account='" + Group_Account + "' gid='" + gid + "' itemcode='" + itemcode + "' "+
                        " calculateStock='" + calculateStock + "' ourTenderNo='" + ourTenderNo + "' ourTenderNoid='" + ourTenderNoid + "' Prosess='N' "+
                        " MillDeffAc='" + MillDeffAc + "' MillDeffacid='" + MillDeffacid + "' isAccounted='" + isAccounted + "' Narration='" + Narration + "' IsDeleted='1'>";

        
         
            //Detail Calculation
            var TenderDetail_Value = ""; concatid = "";

            var gridView = document.getElementById("<%=grdDetail.ClientID %>");
            var grdrow = gridView.getElementsByTagName("tr");

            for (var i = 1; i < grdrow.length; i++) {
                debugger;
                if (gridView.rows[i].cells[11].innerHTML != "N") {
                    var ID = gridView.rows[i].cells[2].innerHTML;
                    var membercode = gridView.rows[i].cells[3].innerHTML;
                    var memberId = gridView.rows[i].cells[13].innerHTML;  
                    var DQty = gridView.rows[i].cells[5].innerHTML; //Quantal
                    var SaleRate = gridView.rows[i].cells[6].innerHTML;
                    var paid = gridView.rows[i].cells[7].innerHTML; 
                    var d = gridView.rows[i].cells[14].innerHTML;
                    var paidDate = d.slice(6, 11) + "/" + d.slice(3, 5) + "/" + d.slice(0, 2);
                    var ItemAmount = gridView.rows[i].cells[8].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[8].innerHTML;
                    var SaudaNarration = gridView.rows[i].cells[9].innerHTML == "&nbsp;" ? "" : gridView.rows[i].cells[9].innerHTML;
                    var profit = gridView.rows[i].cells[15].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[15].innerHTML;  
                    var otherpaidAc = gridView.rows[i].cells[16].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[16].innerHTML;
                    var otherPaid = gridView.rows[i].cells[18].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[18].innerHTML;
                    var otherPaidid = gridView.rows[i].cells[19].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[19].innerHTML;
                    var indivisulProfit = gridView.rows[i].cells[20].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[20].innerHTML;
                    var millContribution = gridView.rows[i].cells[21].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[21].innerHTML;
                    var paidorreseive = gridView.rows[i].cells[22].innerHTML == "&nbsp;" ? "0.00" : gridView.rows[i].cells[22].innerHTML;
                    var saudaReverseAc = gridView.rows[i].cells[23].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[23].innerHTML;
                    var saudaReverseId = gridView.rows[i].cells[24].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[24].innerHTML;
                    var RatePerQtl = gridView.rows[i].cells[26].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[26].innerHTML;
                    var ReverseAmount = gridView.rows[i].cells[27].innerHTML == "&nbsp;" ? "0" : gridView.rows[i].cells[27].innerHTML;
               
                      
                    var ddi = tdetailid;
                    if (gridView.rows[i].cells[11].innerHTML == "A") {
                        XML = XML + "<GroupTenderDetailInsert Tender_No='" + TenderNo + "' Company_Code='" + Company_Code + "' membercode='" + membercode + "' mid='" + memberId + "' " +
                   " Buyer_Quantal='" + DQty + "' Sale_Rate='" + SaleRate + "' paid='" + paid + "'  paidDate='" + paidDate + "'  " +
                   " Narration='" + SaudaNarration + "' grouptenderid='" + tenderId + "' ID='" + ID + "'  year_code='" + Year_Code + "' trnderdetailid='0' ItemAmount= '" + ItemAmount + "' profit= '" + profit + "' " +
                   " otherpaidAc= '" + otherpaidAc + "' otherPaid= '" + otherPaid + "' otherPaidid= '" + otherPaidid + "' " +
                   " indivisulProfit= '" + indivisulProfit + "' millContribution= '" + millContribution + "' paidorreseive= '" + paidorreseive + "' " +
                   "  saudaReverseAc= '" + saudaReverseAc + "' saudaReverseId= '" + saudaReverseId + "' RatePerQtl= '" + RatePerQtl + "' ReverseAmount= '" + ReverseAmount + "' />";
                        ddi = parseInt(ddi) + 1;
                    }
                    else if (gridView.rows[i].cells[11].innerHTML == "U") {
                        var tender_detailid = gridView.rows[i].cells[10].innerHTML;
                        XML = XML + "<TenderDetail Tender_No='" + TenderNo + "' Company_Code='" + Company_Code + "' membercode='" + membercode + "' mid='" + memberId + "' " +
                   " Buyer_Quantal='" + DQty + "' Sale_Rate='" + SaleRate + "' paid='" + paid + "'  paidDate='" + paidDate + "'  " +
                   " Narration='" + SaudaNarration + "' grouptenderid='" + tenderId + "' ID='" + ID + "'  year_code='" + Year_Code + "' trnderdetailid='" + tender_detailid + "' ItemAmount= '" + ItemAmount + "'  profit= '" + profit + "' " +
                   " otherpaidAc= '" + otherpaidAc + "' otherPaid= '" + otherPaid + "' otherPaidid= '" + otherPaidid + "' " +
                   " indivisulProfit= '" + indivisulProfit + "' millContribution= '" + millContribution + "' paidorreseive= '" + paidorreseive + "' " +
                   "  saudaReverseAc= '" + saudaReverseAc + "' saudaReverseId= '" + saudaReverseId + "' RatePerQtl= '" + RatePerQtl + "' ReverseAmount= '" + ReverseAmount + "' />";
                    }
                    else if (gridView.rows[i].cells[11].innerHTML == "D") {
                        var tender_detailid = gridView.rows[i].cells[10].innerHTML;
                        XML = XML + "<GroupTenderDetailIDelete Tender_No='" + TenderNo + "' Company_Code='" + Company_Code + "' membercode='" + membercode + "' mid='" + memberId + "' " +
                   " Buyer_Quantal='" + DQty + "' Sale_Rate='" + SaleRate + "' paid='" + paid + "'  paidDate='" + paidDate + "'  " +
                   " Narration='" + SaudaNarration + "' grouptenderid='" + tenderId + "' ID='" + ID + "'  year_code='" + Year_Code + "' trnderdetailid='" + tender_detailid + "' ItemAmount= '" + ItemAmount + "'  profit= '" + profit + "' " +
                   " otherpaidAc= '" + otherpaidAc + "' otherPaid= '" + otherPaid + "' otherPaidid= '" + otherPaidid + "' " +
                   " indivisulProfit= '" + indivisulProfit + "' millContribution= '" + millContribution + "' paidorreseive= '" + paidorreseive + "' " +
                   "  saudaReverseAc= '" + saudaReverseAc + "' saudaReverseId= '" + saudaReverseId + "'  RatePerQtl= '" + RatePerQtl + "' ReverseAmount= '" + ReverseAmount + "' />";
                    } 
                }

       
            }
            XML = XML + "</GroupTenderHead></ROOT>";
            ProcessXML(XML, status, spname);
        }
        function ProcessXML(XML, status, spname) { 
            $.ajax({
                type: "POST",
                url: "../xmlExecuteDMLQry.asmx/ExecuteXMLQry",
                data: '{XML:"' + XML + '",status:"' + status + '",spname:"' + spname + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                    //$("#loader").hide();
                },
                error: function (response) {
                    alert(response.d);
                    $("#loader").hide();

                }
            });
            function OnSuccess(response) { 
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
                        window.open('../BussinessRelated/pgeGroupTenderPurchase.aspx?grouptenderid=' + id + '&Action=1', "_self");

                    }
                }
                else {
                    var num = parseInt(response.d);

                    if (isNaN(num)) {
                        alert(response.d)

                    }
                    else {
                        window.open('../BussinessRelated/pgeGroupTenderPurchaseUtility.aspx', "_self");
                    }
                }

            } 

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
    <script language="JavaScript" type="text/javascript">

        //window.onbeforeunload = function (e) {
        //    var e = e || window.event;
        //    if (e) e.returnValue = 'Browser is being closed, is it okay?'; //for IE & Firefox
        //    return 'Browser is being closed, is it okay?'; // for Safari and Chrome
        //}; 
         

        function GD(Tender_no,Confim) {
            window.open('../Report/rptGroupTender.aspx?Tender_no=' + Tender_no + '&Confim='+ Confim);

        }

        function GS(Tender_no) {
            window.open('../Report/rptGroupTenderSummery.aspx?Tender_no=' + Tender_no);

        }

        function Open(groupid) {
            var Action = 1;
            window.open('../BussinessRelated/pgeGroupTenderPurchase.aspx?grouptenderid=' + groupid + '&Action=' + Action, "_Self");
        }
        function Prosess() {
             

            window.open('../Report/rptpendingGroupTender.aspx');
            // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
        }
        function SelfBalance() {
            

            window.open('../Report/rptSelfBalance.aspx');
            // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
        }
         



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text=" Group Tender Purchase   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <div id="loader" align="center" style="display: none; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px;">
        <img height="40%" width="40%" src="../Images/ajax-loader3.gif" />
    </div>

    <asp:UpdatePanel ID="upPnlPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfIsClick" runat="server" Value="0" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfNextFocus" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="vouchernumber" runat="server" />
            <asp:HiddenField ID="hdnfmc" runat="server" />
            <asp:HiddenField ID="hdnfmillshort" runat="server" />
            <asp:HiddenField ID="hdnfic" runat="server" />   
            <asp:HiddenField ID="hdnfgid" runat="server" />
            <asp:HiddenField ID="hdnfbpdetail" runat="server" />
            <asp:HiddenField ID="hdnftenderno" runat="server" />
            <asp:HiddenField ID="hdnftenderid" runat="server" />
            <asp:HiddenField ID="hdnfvoucherbygst" runat="server" /> 
            <asp:HiddenField ID="hdnfmillpaymentdate" runat="server" />
            <asp:HiddenField ID="hdnfSaleTDS" runat="server" />
            <asp:HiddenField ID="hdnfTCSRate" runat="server" />
            <asp:HiddenField ID="hdnfistds" runat="server" /> 
            <asp:HiddenField ID="hdnfvouchertds" runat="server" />
            <asp:HiddenField ID="hdnfVSaleTDS" runat="server" />
            <asp:HiddenField ID="hdnfVTCSRate" runat="server" />
            <asp:HiddenField ID="hdnfmember" runat="server" />
            <asp:HiddenField ID="hdnfProfit" runat="server" />
            <asp:HiddenField ID="hdnfonAc" runat="server" />
            <asp:HiddenField ID="hdnfotherpaid" runat="server" />
            <asp:HiddenField ID="hdnftenderhelpid" runat="server" />
            <asp:HiddenField ID="hdnfmillDiffacid" runat="server" />
            <asp:HiddenField ID="hdnfReverseAcId" runat="server" />

            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="True" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" align="left" cellspacing="4px" cellpadding="1px">
                    <tr>
                        <td colspan="4">
                            <table width="100%" align="left">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnAdd_Click" />
                                        &nbsp;
                                        <asp:Button OnClientClick="if (!Validate()) return false;" ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="80px"
                                            UseSubmitBehavior="false" Height="25px" ValidationGroup="add" OnClick="btnSave_Click" TabIndex="28" />
                                        &nbsp;
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="save" OnClick="btnEdit_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnCancel_Click" TabIndex="29" />
                                        &nbsp;
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="80px" OnClientClick="Back()"
                                            Height="25px" TabIndex="30" />
                                           &nbsp;
                                        <asp:Button ID="btnProsess" runat="server" Text="prosess" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="save" OnClick="btnProsess_Click" />
                                           &nbsp;
                                        <asp:Button ID="btnGroupDetailPrint" runat="server" Text="Group Detail Print All " CssClass="btnHelp" Width="150px"
                                            Height="25px" ValidationGroup="save" OnClick="btnGroupDetailPrint_Click" /> 
                                         <asp:Button ID="btnGroupSummery" runat="server" Text="Group Summery " CssClass="btnHelp" Width="150px"
                                            Height="25px" ValidationGroup="save" OnClick="btnGroupSummery_Click"/> 
                                              &nbsp;
                                        <asp:Button ID="btnPandingProsess" runat="server" Text="Panding Prosess" CssClass="btnHelp" Width="140px"
                                            Height="25px" ValidationGroup="save" OnClientClick="Prosess()" />
                                          <asp:Button ID="btnSelfBalance" runat="server" Text="Self Balance" CssClass="btnHelp" Width="140px"
                                            Height="25px" ValidationGroup="save" OnClientClick="SelfBalance()" />
                                         
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

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Change No:</td>

                        <td colspan="4" align="left">
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged" TabIndex="1"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Names="verdana" Font-Italic="true"
                                Font-Size="Small" ForeColor="red" Visible="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Tender No:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtTenderNo" runat="server" CssClass="txt" Width="100px" TabIndex="1"
                                AutoPostBack="false" OnTextChanged="txtTenderNo_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnChangeNo" runat="server" Text="Change No" CssClass="btnHelp" Width="69px"
                                TabIndex="1" OnClick="changeNo_click" Height="24px" />
                            <asp:Label ID="lblTender_Id" runat="server" Text="" Font-Names="verdana"
                                ForeColor="Blue" Font-Bold="true" Font-Size="12px" Visible="true"></asp:Label></legend>
                            
                          
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Date:
                        </td>
                        <td align="left" colspan="3">
                            <%--<asp:TextBox ID="txtDate" runat="server" CssClass="txt" Width="100px" AutoPostBack="True"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                OnTextChanged="txtDate_TextChanged" TabIndex="3" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDate"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>--%>


                            <asp:TextBox ID="txtDate" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDate"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>

                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lifting Date:
                            &nbsp;&nbsp;<asp:TextBox ID="txtLiftingDate" runat="server" CssClass="txt" Width="100px"
                                AutoPostBack="true" OnTextChanged="txtLiftingDate_TextChanged" TabIndex="2" onkeyup="ValidateDate(this,event.keyCode)"
                                onkeydown="return DateFormat(this,event.keyCode)" MaxLength="10" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderLiftingdate" runat="server" TargetControlID="txtLiftingDate"
                                PopupButtonID="imgcalender1" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            <asp:Label runat="server" ID="lblMesg"></asp:Label>

                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
                            &nbsp;&nbsp;
                            &nbsp;Calculate Stock:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drCalculateStock" runat="server" AutoPostBack="true" Width="120px"
                                CssClass="ddl"  >
                                <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N" ></asp:ListItem>

                            </asp:DropDownList>
                             &nbsp;&nbsp;
                            &nbsp;Is Accounted:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drAccounted" runat="server" AutoPostBack="true" Width="120px"
                                CssClass="ddl"  >
                                <asp:ListItem Text="Yes" Value="Y" ></asp:ListItem>
                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>

                            </asp:DropDownList>
                        </td>
                        <td>

                        </td>
                    </tr>
                          <tr>
                         <td align="right"> Group Name:
                             </td>
                             <td align="left" colspan="4"> 
                             <asp:TextBox ID="txtGroup" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtGroup_TextChanged" TabIndex="3" Height="24px" onkeydown="BP_Account(event);"></asp:TextBox>
                            <asp:Button ID="btnGroup" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGroup_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblGroup_Name" runat="server" CssClass="lblName"></asp:Label>
                             
                         
                        </td>
                    
                   
                    </tr>
                    <tr>
                        <td align="right">Mill Code:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtMillCode" runat="server" CssClass="txt" Width="100px" Style="text-align: right;" onkeydown="MillCode(event);"
                                AutoPostBack="false" OnTextChanged="txtMillCode_TextChanged"
                                TabIndex="4" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnMillCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnMillCode_Click"
                                Height="24px" Width="20px" />
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillCode" FilterType="Numbers" TargetControlID="txtMillCode"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Label ID="lblMillName" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblMill_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                           
                            Item Code: 
                            <asp:TextBox ID="txtitem_code" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                onkeydown="item(event);" AutoPostBack="false" OnTextChanged="txtitem_code_TextChanged"
                                TabIndex="5" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtitem_code" runat="server" Text="..." CssClass="btnHelp" OnClick="btntxtitem_code_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblitemname" runat="server" CssClass="lblName"></asp:Label>
                        </td> 
                    </tr>
                    <tr>
                        <td align="right">Grade:
                        </td>
                        <td align="left" colspan="6">
                            <asp:TextBox ID="txtGrade" runat="server" CssClass="txt" Width="100px" TabIndex="6"
                                AutoPostBack="false" OnTextChanged="txtGrade_TextChanged" Height="24px" onkeydown="Grade(event);"></asp:TextBox>
                            <asp:Button ID="btnGrade" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGrade_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblGrade_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quintal:
                            <asp:TextBox ID="txtQuantal" runat="server" CssClass="txt" Width="100px" Style="text-align: left;"
                                AutoPostBack="True" OnTextChanged="txtQuantal_TextChanged" TabIndex="7" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtQuantal" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtQuantal">
                            </ajax1:FilteredTextBoxExtender>
                            Packing:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPacking" runat="server" CssClass="txt" AutoPostBack="True" Width="100px"
                                Style="text-align: left;" OnTextChanged="txtPacking_TextChanged" TabIndex="8"
                                Height="24px"></asp:TextBox>
                           <%-- <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPacking" FilterType="Numbers,cutom"
                                TargetControlID="txtPacking">
                            </ajax1:FilteredTextBoxExtender>--%>
                               <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPacking" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtPacking">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;Bags:
                            <asp:TextBox ID="txtBags" runat="server" CssClass="txt" ReadOnly="true" Width="100px"
                                TabIndex="9" Style="text-align: left;" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBags" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBags">
                            </ajax1:FilteredTextBoxExtender>
                          Mill Rate:  
                            <asp:TextBox ID="txtMillRate" runat="server" CssClass="txt" AutoPostBack="true" Width="100px"
                                Style="text-align: right;" OnTextChanged="txtMillRate_TextChanged" TabIndex="10"
                                Height="24px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtMillRate" SetFocusOnError="true" runat="server"
                                ControlToValidate="txtMillRate" CssClass="validator" Display="Dynamic" Text="Required"
                                ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtMillRate">
                            </ajax1:FilteredTextBoxExtender> 
                          
                             </td> 
                        </tr>
                           <tr>
                               <td align="right"> Mill Diff:  
                        </td>
                        <td align="left" colspan="4">
                             <asp:TextBox ID="txtmillDeff" runat="server" CssClass="txt" AutoPostBack="true" Width="100px"
                                Style="text-align: right;" TabIndex="11"  OnTextChanged="txtmillDeff_TextChanged" 
                                Height="24px"></asp:TextBox>
                            <asp:Label Text="" runat="server" ID="lblValue" ForeColor="Yellow" />
                             Mill Diff Ac: 
                            <asp:TextBox ID="txtmillDeffAc" runat="server" CssClass="txt" Width="100px" Style="text-align: right;" onkeydown="millDeffAc(event);"
                                AutoPostBack="false" OnTextChanged="txtmillDeffAc_TextChanged"
                                TabIndex="12" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnmillDeffAc" runat="server" Text="..." CssClass="btnHelp" OnClick="btnmillDeffAc_Click"
                                Height="24px" Width="20px" />
                               <asp:Label ID="lblmillDeffAcName" runat="server" CssClass="lblName"></asp:Label> 
                          
                             </td>  
                        </tr> 
                           <tr>
                        <td align="right">Our Tender No:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtourTenderNo" runat="server" CssClass="txt" Width="100px" Style="text-align: right;" onkeydown="TenderCode(event);"
                                AutoPostBack="false" OnTextChanged="txtourTenderNo_TextChanged"
                                TabIndex="13" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnourTenderNo" runat="server" Text="..." CssClass="btnHelp" OnClick="btnourTenderNo_Click"
                                Height="24px" Width="20px" />
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillCode" FilterType="Numbers" TargetControlID="txtMillCode"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Label ID="lblDate" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblQuantal" runat="server" CssClass="lblName" ></asp:Label>
                          

                             Narration:
                           
                                <asp:TextBox ID="txtNarration" runat="server" Width="360px" CssClass="txt" Height="50px"
                                    TextMode="MultiLine" TabIndex="14" AutoPostBack="true" OnTextChanged="txtNarration_TextChanged"></asp:TextBox>
                             </td> 
                        </tr> 
              
                </table>
                
                <fieldset style="border-top: 3px dotted rgb(131, 127, 130); border-radius: 3px; width: 172%; margin-left: -1160px; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 5px;">
                    <legend style="text-align: left;">
                        <h2 style="color: Black; text-align: left;" font-names="verdana" font-size="large">Tender Detail Section</h2>
                    </legend>
                </fieldset>
                <asp:Panel ID="Panel2" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-top: 0px; z-index: 100;">

                    <table width="100%" cellpadding="4px" cellspacing="4px">
                        <tr>
                            <td align="left">ID:
                           
                                <asp:Label ID="lblID" runat="server"></asp:Label>
                                <asp:Label ID="lblno" runat="server" ForeColor="Azure"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Member Name:
                           
                                <asp:TextBox ID="txtMemberName" runat="server" Width="80px" Height="24px" AutoPostBack="false"
                                    CssClass="txt" OnTextChanged="txtMemberName_TextChanged" onkeydown="Party(event);" TabIndex="15"></asp:TextBox>
                                <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyer" FilterType="Numbers" TargetControlID="txtMemberName"></ajax1:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnMember" runat="server" Text="..." Height="24px" Width="20px" CssClass="btnHelp"
                                    OnClick="btnMember_Click" />
                                <asp:Label ID="lblMemberName" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblMemberID" runat="server" Visible="false"></asp:Label>
                               
                                Buyer Quantal:
                                

                                <asp:TextBox ID="txtBuyerQuantal" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtBuyerQuantal_TextChanged" TabIndex="16"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerQuantal" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtBuyerQuantal">
                                </ajax1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvtxtBuyerQuantal" runat="server" ControlToValidate="txtBuyerQuantal"
                                    CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                    Text="Required" ValidationGroup="addBuyerDetails">
                                </asp:RequiredFieldValidator>

                                Sale Rate:
                           
                                <asp:TextBox ID="txtBuyerSaleRate" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtBuyerSaleRate_TextChanged" TabIndex="16"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerSaleRate" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtBuyerSaleRate">
                                </ajax1:FilteredTextBoxExtender>
                               
                              
                                 Item Amount:
                          
                                <asp:TextBox ID="txtItemAmount" runat="server" CssClass="txt" Height="24px"
                                    TabIndex="17" Width="80px" AutoPostBack="true" OnTextChanged="txtItemAmount_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerCommission" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtItemAmount">
                                </ajax1:FilteredTextBoxExtender>
                               
                                 Paid:
                                <asp:TextBox ID="txtPaid" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtPaid_TextChanged" TabIndex="18"></asp:TextBox>
                                  paid Date:
                          
                                <asp:TextBox ID="txtDetailpaidDate" runat="server" CssClass="txt" Width="100px"
                                    AutoPostBack="True" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                    onkeydown="return DateFormat(this,event.keyCode)" OnTextChanged="txtDetailpaidDate_TextChanged"
                                    TabIndex="19" Height="24px"></asp:TextBox>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                    Height="15px" />
                                <ajax1:CalendarExtender ID="calendertxtDetailpaidDate" runat="server" TargetControlID="txtDetailpaidDate"
                                    PopupButtonID="Image1" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                          
                                

                                Narration:
                           
                                <asp:TextBox ID="txtBuyerNarration" runat="server" Width="360px" CssClass="txt" Height="50px"
                                    TextMode="MultiLine" TabIndex="20" AutoPostBack="true" OnTextChanged="txtBuyerNarration_TextChanged"></asp:TextBox>

                                <%--<td colspan="2" align="center">--%>
                             
                               
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Other Paid Name:
                           
                                <asp:TextBox ID="txtotherpaidAc" runat="server" Width="80px" Height="24px" AutoPostBack="false"
                                    CssClass="txt" OnTextChanged="txtotherpaidAc_TextChanged" onkeydown="otherpaidAc(event);" TabIndex="21"></asp:TextBox>
                                 <asp:Button ID="btnotherpaidAc" runat="server" Text="..." Height="24px" Width="20px" CssClass="btnHelp"
                                    OnClick="btnotherpaidAc_Click" />
                                <asp:Label ID="lblOtherPaidName" runat="server" CssClass="lblName"></asp:Label> 
                                
                              Other   Paid:
                                <asp:TextBox ID="txtotherPaid" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtotherPaid_TextChanged" TabIndex="22"></asp:TextBox>

                                Sauda Reverse Ac:
                                 <asp:TextBox ID="txtsaudaReverseAc" runat="server" Width="80px" Height="24px" AutoPostBack="false"
                                    CssClass="txt" OnTextChanged="txtsaudaReverseAc_TextChanged" onkeydown="ReverseAc(event);" TabIndex="23"></asp:TextBox>
                                 <asp:Button ID="btnsaudaReverseAc" runat="server" Text="..." Height="24px" Width="20px" CssClass="btnHelp"
                                    OnClick="btnsaudaReverseAc_Click" />
                                <asp:Label ID="lblsaudaReverseAcName" runat="server" CssClass="lblName"></asp:Label> 
                                    Rate Per Qtl:
                                <asp:TextBox ID="txtRatePerQtl" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true"   OnTextChanged="txtRatePerQtl_TextChanged" TabIndex="24"></asp:TextBox>
                              
                                   Reverse Amount:
                                <asp:TextBox ID="txtReverseAmount" runat="server" Width="80px" Height="24px" CssClass="txt"
                                   ReadOnly="true" AutoPostBack="true"  OnTextChanged="txtReverseAmount_TextChanged" TabIndex="25"></asp:TextBox>
                                   
                            </td>
                        </tr>
                        <tr>
                            <td> 

                                
                                <asp:Button ID="btnADDBuyerDetails" runat="server" Text="ADD" CssClass="btnHelp"
                                    Font-Bold="false" OnClick="btnADDBuyerDetails_Click" Width="90px" Height="24px" ValidationGroup="addBuyerDetails"
                                    TabIndex="26" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btnHelp"
                                    TabIndex="27" Font-Bold="false" CausesValidation="false" Width="90px" Height="24px" />

                                <asp:Label ID="lbltenderdetailid" runat="server" Visible="false"></asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <div>
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="220px" Width="1500px"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="150%"
                                Height="65%" OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5"
                                OnRowDataBound="grdDetail_RowDataBound" Style="table-layout: fixed;">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRecord" Text="Edit"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRecord" Text="Delete"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdNew" runat="server" ScrollBars="Both" Height="220px" Width="40%"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="Gridnew" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="100%"
                                Height="65%"  CellPadding="5" CellSpacing="5"
                                OnRowDataBound="pnlgrdNew_RowDataBound" Style="table-layout: fixed;"> 
                                <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="90%"
                ScrollBars="Both" align="center" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: left; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 5%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                    ToolTip="Close" />
                <table width="95%">
                    <tr>
                        <td align="center" styles="background-color: #F5B540; width: 100%;">
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
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                        To:
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
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
                    </tr>
                </table>
            </asp:Panel>

            <%-- <asp:Panel ID="pnlPopupTenderDetails" runat="server" BackColor="GhostWhite" Width="800px"
                BorderColor="Teal" BorderWidth="1px" Height="430px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 10%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
