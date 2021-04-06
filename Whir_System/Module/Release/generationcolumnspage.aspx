<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="generationcolumnspage.aspx.cs" Inherits="whir_system_module_release_generationcolumnspage" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath %>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
        type="text/javascript"></script>
    <script type="text/javascript">
		<!--
        var setting = {
            check: {
                enable: true
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: 0
                }
            },
            callback: {
                onClick: OnClick
            },
            view: {
                dblClickExpand: false
            }
        };
        function OnClick(e, treeId, treeNode) {
            $.fn.zTree.getZTreeObj("tree_data").expandNode(treeNode);
        }
        $(function () {
            var currentsiteid = "<%=CurrentSiteId %>";
            $.getJSON("<%=SysPath %>ajax/content/column_generat.aspx?siteid=" + currentsiteid + "&time=" + new Date().getMilliseconds(), function (data) {
                $.fn.zTree.init($("#tree_data"), setting, data);

                autoHeight();
            });
            $("#expandAllBtn").toggle(
                function () {
                    $(this).text('<%="全部折叠".ToLang() %>');
                    $.fn.zTree.getZTreeObj("tree_data").expandAll(true);

                },
                function () {
                    $(this).text('<%="全部展开".ToLang() %>');
                    $.fn.zTree.getZTreeObj("tree_data").expandAll(false);
                }
            );
            $("#checkAllTrue").click(function () {
                $.fn.zTree.getZTreeObj("tree_data").checkAllNodes(true);
            });
            $("#inverseTrue").click(
                function () {
                    var tree_data = $.fn.zTree.getZTreeObj("tree_data"); //用来节点刷新
                    var check_data = $.fn.zTree.getZTreeObj("tree_data").getCheckedNodes(true); //获取选中的节点
                    var nocheck_data = $.fn.zTree.getZTreeObj("tree_data").getCheckedNodes(false); //获取未选中的节点

                    for (var i in check_data) {
                        check_data[i].checked = false;
                        tree_data.updateNode(check_data[i], true);
                    }
                    for (var i in nocheck_data) {
                        nocheck_data[i].checked = true;
                        tree_data.updateNode(nocheck_data[i]);
                    }
                }
            )
        });
        //自动高度
        function autoHeight() {
            var tree_data = $("#tree_data").height();
            var div_alltable = $("#div_alltable").height();
            var table_result = $("#table_result").height();

            if (tree_data > div_alltable) {
                $("#table_result").height(tree_data + 24);
            } else {
                $("#tree_data").height(div_alltable - 24);
            }
        }
		//-->
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tbody>
                <tr>
                    <td class="box_common" valign="top" width="200">
                        <h3 class="f_title">
                            <a id="expandAllBtn" class="fl" style="width: 45%;"><em>
                                <%="全部展开".ToLang()%></em></a><a id="checkAllTrue" class="fl" style="width: 25%;"><em>
                                    <%="全选".ToLang()%></em></a><a id="inverseTrue" class="fl"><em>
                                        <%="反选".ToLang()%></em></a></h3>
                        <ul id="tree_data" class="ztree" style="width: 190px; margin: 0px;">
                        </ul>
                    </td>
                    <td width="5">
                    </td>
                    <td class="box_common" valign="top">
                        <div id="div_alltable" class="All_table">
                            <table style="border: #d6dfe5 1px solid; border-bottom: 0px;" width="100%" border="0"
                                cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <%="同时生成：".ToLang()%><asp:CheckBox ID="cbColumnHomePage" runat="server" /><%="栏目首页".ToLang()%><asp:CheckBox
                                            ID="cbColumnListPage" runat="server" /><%="栏目列表页".ToLang()%>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table id="table_local" style="border: #d6dfe5 1px solid;" width="100%" border="0"
                                cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="button_submit_div">
                                        <%="生成最新：".ToLang()%>
                                        <whir:TextBox ID="txtCount" runat="server" ErrorCss="form_error" TipCss="form_tip"
                                            Regular="Number" CssClass="text_common" Width="40"></whir:TextBox>
                                        <%="篇文章".ToLang()%>
                                        <asp:LinkButton ID="btnGeneratByNew" runat="server" CommandName="GeneratByNew" OnCommand="Generat_Command"
                                            CssClass="aLink">
                                            <b>
                                                <%="开始生成".ToLang()%>>></b></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button_submit_div">
                                        <%="生成更新时间为：".ToLang()%>
                                        <whir:DateBox ID="txtBeginDate" runat="server" CssClass="text_date"></whir:DateBox>
                                        <%="到".ToLang()%>
                                        <whir:DateBox ID="txtEndDate" runat="server" CssClass="text_date"></whir:DateBox>
                                        <%="的文章".ToLang()%>
                                        <asp:LinkButton ID="BtnGeneratByDate" runat="server" CommandName="GeneratByDate"
                                            OnCommand="Generat_Command" CssClass="aLink">
                                            <b>
                                                <%="开始生成".ToLang()%>>></b></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button_submit_div">
                                        <%="生成指定Id的文章（多个Id可用逗号隔开）：".ToLang()%>
                                        <asp:TextBox ID="txtArticleID" runat="server" CssClass="text_common">
                                        </asp:TextBox>
                                        <asp:LinkButton ID="btnGeneratByIDs" runat="server" CommandName="GeneratByIds" OnCommand="Generat_Command"
                                            CssClass="aLink">
                                            <b>
                                                <%="开始生成".ToLang()%>>></b></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button_submit_div">
                                        <%="生成指定栏目所有未生成的文章".ToLang()%>
                                        <asp:LinkButton ID="btnGeneratByFirst" runat="server" CommandName="GeneratByFirst"
                                            OnCommand="Generat_Command" CssClass="aLink">
                                            <b>
                                                <%="开始生成".ToLang()%>>></b></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: 0px;" class="button_submit_div">
                                        <%="生成指定栏目所有文章".ToLang()%>
                                        <asp:LinkButton ID="btnGeneratByAll" runat="server" CommandName="btnGeneratByAll"
                                            OnCommand="Generat_Command" CssClass="aLink">
                                            <b>
                                                <%="开始生成".ToLang()%>>></b></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <%="生成结果：".ToLang()%><br />
                            <table id="table_result" style="border: #d6dfe5 1px solid; height: 250px;" width="100%"
                                border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td valign="top">
                                        <div style="overflow: auto; height: 230px; width: 77%;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
