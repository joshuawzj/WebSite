<%@ Page Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="option_select.aspx.cs" Inherits="whir_system_Plugin_shop_common_option_select" %>

<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:headcontainer id="HeadContainer2" runat="server" />
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>

    <script type="text/javascript">
        //加载树
        whir.ztree.area("areaTree", "<%=SysPath%>Plugin/shop/common/getshopcategorylist.aspx?cid=<%=CategoryID %>");
        $(function () {
           
            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                
                var text =whir.ztree.getSelectedText("areaTree");
                var value =whir.ztree.getSelected("areaTree");
                window.parent.<%= CallBack %>(value,text);
                window.parent.whir.dialog.remove();
                
                return false;
             });
           <%-- whir.dialog.addButtons(function () {
                var selected = whir.ztree.getSelected("areaTree");
                if (selected.length <= 0) {
                    TipMessage('请选择');
                    return false;
                } else {
                    var radioSelected = selected.split(',')[0];
                    //回传值给父页面
                    frameElement.api.opener.<%= CallBack %>(radioSelected);
                    frameElement.api.close();
                }
                return false;
            });--%>
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="content-wrap">
        <div class="space15">
        </div>
     <div class="panel">
        <ul id="areaTree" class="ztree"></ul>
    </div>
         </div>
</asp:Content>
