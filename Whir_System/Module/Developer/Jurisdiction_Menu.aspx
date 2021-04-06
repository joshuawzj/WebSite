<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Jurisdiction_menu.aspx.cs" Inherits="whir_system_module_developer_Jurisdiction_menu" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../res/js/jquery_treetable/jquery.treeTable.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript" src="../../res/js/jquery_treetable/jquery.treetable.js"></script>
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading"><%="超管权限设置".ToLang()%></div>
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="Jurisdiction_menu.aspx"><%="菜单权限".ToLang()%></a></li>
                        <li><a href="Jurisdiction_column.aspx"><%="栏目权限".ToLang()%></a></li>
                    </ul>
                    <div class="space15"></div>
                    <div class="All_list">
                        <table width="100%" id="dnd-example" class="treeTable" style="display: none;">
                            <tr style="height: 40px">
                                <th width="100" class="th_center"></th>
                                <th>
                                    <span onclick="javascript:$('input[name=menucheckbox]').prop('checked', true);" style="cursor: pointer;"><%="全选".ToLang()%></span>/<span onclick="javascript:$('input[name=menucheckbox]').prop('checked', false);"
                                        style="cursor: pointer;"><%="取消".ToLang()%></span>
                                </th>
                            </tr>
                            <asp:Repeater ID="rptMenuList" runat="server">
                                <ItemTemplate>
                                    <tr style="height: 40px" id='node-<%#Eval("MenuId") %>' <%#Eval("ParentId").ToInt()==0?"":"class='child-of-node-"+Eval("ParentId") +"'"%>>
                                        <td align="center">&nbsp;</td>
                                        <td>
                                            <ul class="list">
                                                <li>
                                                    <asp:Literal ID="litMenuName" runat="server"></asp:Literal>
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                    <div class="row">
                        <div class="col-md-offset-2 col-md-6">
                            <asp:UpdatePanel ID="upSave" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnSave" runat="server"
                                        CssClass="btn btn-info btn-block" CommandArgument="Save"
                                        OnClick="btnSave_Click"><%="提交".ToLang()%>
                                    </asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            whir.skin.checkbox(false); //取消美化checkbox

            $(document).ready(function () {
                $("#dnd-example").treeTable({
                    indent: 20
                }).show();
            });

            function checknode(obj) {
                var chk = $("input[type='checkbox']");
                var count = chk.length;
                var num = chk.index(obj);
                var level_top = level_bottom = chk.eq(num).attr('level');
                for (var i = num; i >= 0; i--) {
                    var le = chk.eq(i).attr('level');
                    if (eval(le) < eval(level_top)) {
                        chk.eq(i).prop("checked", true);
                        var level_top = level_top - 1;
                    }
                }

                for (var j = num + 1; j < count; j++) {
                    var le = chk.eq(j).attr('level');
                    //jquery 1.7.2要改成== "checked"
                    if (chk.eq(num).prop("checked") == true || chk.eq(num).prop("checked") == "checked") {
                        if (eval(le) > eval(level_bottom)) chk.eq(j).prop("checked", true);
                        else if (eval(le) == eval(level_bottom)) break;
                    }
                    else {
                        if (eval(le) > eval(level_bottom)) chk.eq(j).prop("checked", false);
                        else if (eval(le) == eval(level_bottom)) break;
                    }
                }
            }
        </script>
    </form>
</asp:Content>
