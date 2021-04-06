<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="sort_category.aspx.cs" Inherits="whir_system_ModuleMark_common_sort_category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript">
        //添加按钮
        whir.dialog.addButtons(function () {
            var selected = whir.checkbox.getSelect("cbx_");
            if (selected.length <= 0) {
                TipMessage('请选择');
                return false;
            } else {
                frameElement.api.opener.sortAction(selected);
                return true;
            }
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="mainbox">
    
        <whir:ContentManager id="contentManager1" runat="server"  ></whir:ContentManager>

    </div>

</asp:Content>