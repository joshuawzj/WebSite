<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="option_select.aspx.cs" Inherits="whir_system_ModuleMark_common_option_select" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //加载树
            whir.ztree.area("areaTree", "<%=SysPath%>ajax/common/option.aspx?formid=<%=FormId%>&SubjectId=<%=SubjectId%>&exceptid=<%=ExceptId%>");

            whir.dialog.addButtons(function () {
                var selected = whir.ztree.getSelected("areaTree");
                if (selected.length <= 0) {
                    TipMessage('<%="请选择".ToLang()%>');
                    return false;
                } else {
                    var radioSelected = selected.split(',')[0];
                    //回传值给父页面
                    if (frameElement.api.get('Select')) { //用于批量修改界面，多重弹出层
                        frameElement.api.get('Select').<%= CallBack %>(radioSelected);
                        frameElement.api.close();
                    } else {
                        frameElement.api.opener.<%= CallBack %>(radioSelected);
                        frameElement.api.close();
                    }
                }
                return false;
            });
        });
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <ul id="areaTree" class="ztree"></ul>
    </div>
</asp:Content>
