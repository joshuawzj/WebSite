<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Jurisdiction_Menu.aspx.cs" Inherits="Whir_System_Module_Security_Jurisdiction_Menu" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../res/js/jquery_treetable/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document).click(function () {

                __doPostBack('<%= btnSave.UniqueID %>', '');
                window.parent.whir.toastr.success('<%="操作成功".ToLang() %>');
                setTimeout("window.parent.whir.dialog.remove()", 500);
                return false;

            });

        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript" src="../../res/js/jquery_treetable/jquery.treetable.js"></script>
        <div class="mainbox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="box_common" valign="top">
                        <div class="All_list">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="dnd-example" class="treeTable"
                                style="display: none;">
                                <tr>
                                    <th width="100" class="th_center"></th>
                                    <th>
                                        <span onclick="javascript:$('input[name=menucheckbox]').prop('checked', true)" style="cursor: pointer;">
                                            <%="全选".ToLang()%></span>/<span onclick="javascript:$('input[name=menucheckbox]').prop('checked', false)"
                                                style="cursor: pointer;"><%="取消".ToLang()%></span>
                                    </th>
                                </tr>
                                <asp:Repeater ID="rptMenuList" runat="server">
                                    <ItemTemplate>
                                        <tr id='node-<%#Eval("MenuId") %>' <%#Eval("ParentId").ToInt()==0?"":"class='child-of-node-"+Eval("ParentId") +"'"%>>
                                            <td align="center">&nbsp;
                                            </td>
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
                        <div class="button_submit_div" style="display: none;">
                            <asp:UpdatePanel ID="upSave" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="aLink" CommandArgument="Save"
                                        OnClick="btnSave_Click">
                                        <em><img src="<%=SysPath%>res/images/button_submit_icon_1.gif" /></em><b><%="提交".ToLang()%></b>
                                    </asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            whir.skin.checkbox(false); //取消美化checkbox
            $(document).ready(function () {
                $("#dnd-example").treeTable({
                    indent: 20
                }).show();
                //RestTrClass();

            });

            function checknode(obj) {
                var chk = $("input[type='checkbox']");
                var count = chk.length;
                var num = chk.index(obj);
                var level_top = level_bottom = chk.eq(num).attr('level')
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
