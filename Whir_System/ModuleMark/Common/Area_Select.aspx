<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Area_Select.aspx.cs" Inherits="Whir_System_ModuleMark_Common_Area_Select" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //加载树
            whir.ztree.area("areaTree", "<%=SysPath%>ajax/common/area.aspx?AreaLevel=<%= AreaLevel %>");

            $("[name=_dialog] .btn-primary", parent.document).click(function() {
                var selected = whir.ztree.getSelected("areaTree");
                if (selected.length <= 0) {
                    TipMessage('<%="请选择".ToLang()%>');
                    return false;
                } else {
                    var selectedLevel = whir.ztree.getRadioSelectedLevel("areaTree");

                    var radioSelected = selected.split(',')[0];
                    //获取选中地区的父级名称
                    $.ajax({
                        url: '<%=SysPath%>ajax/common/getAreaParentsName.aspx?id=' + radioSelected,
                        type: 'GET',
                        cache: false,
                        success: function(result) {
                            //回传值给父页面
                            if (frameElement.api && frameElement.api.get('Select')) { //用于邮件群发选择会员界面，对会员进行地区搜索；用于多重弹出层
                                frameElement.api.get('Select').<%= CallBack %>(radioSelected, result);
                                frameElement.api.close();
                            } else {
                                window.parent.<%= CallBack %>(radioSelected, result,'<%=Field %>');
                                if (window.parent._dialog) {
                                    window.parent._dialog.remove();
                                };
                            }
                        },
                        error: function() {
                            alert('<%="获取地区名发生错误".ToLang()%>');
                        }
                    });
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
