<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="bindcolumn.aspx.cs" Inherits="whir_system_module_column_bindcolumn" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%=SysPath %>res/js/jquery_treetable/jqtreetable.js"></script>
    <!--这个是表格显示的文件-->
    <link rel="stylesheet" type="text/css" href="<%=SysPath %>res/js/jquery_treetable/jqtreetable.css" />
    <script type="text/javascript">
        $(function () {
            //表格显示
            if ($("#<%=hfStrMap.ClientID %>").val() == "") return false;
            var map = $("#<%=hfStrMap.ClientID %>").val().split(",");
            var options = { openImg: "<%=SysPath %>res/images/TreeTable/tv-collapsable.gif", shutImg: "<%=SysPath %>res/images/TreeTable/tv-expandable.gif", leafImg: "<%=SysPath %>res/images/TreeTable/tv-item.gif", lastOpenImg: "<%=SysPath %>res/images/TreeTable/tv-collapsable-last.gif", lastShutImg: "<%=SysPath %>res/images/TreeTable/tv-expandable-last.gif", lastLeafImg: "<%=SysPath %>res/images/TreeTable/tv-item-last.gif", vertLineImg: "<%=SysPath %>res/images/TreeTable/vertline.gif", blankImg: "<%=SysPath %>res/images/TreeTable/blank.gif", collapse: false, column: 0, striped: true, highlight: true, state: false };
            $("#tbdColumnList").jqTreeTable(map, options);

            //根据栏目类型Id返回栏目类型的名称
            $("#tbdColumnList tr").find("td").find("span#spmodelname").each(function (i, item) {
                var te = $("#<%=dropColumn.ClientID %>").find("option[value=" + $(this).html() + "]").html();
                $(this).html(te);
            });

            //隐藏首页
            var indexTr = $(".treeTable tr:eq(1)");
            if (indexTr.find("td:eq(1)").attr("model") == "model-1") {
                indexTr.find("td:eq(1)").html("");
                indexTr.find("td:eq(3)").html("");
                indexTr.find("td:eq(4)").html("");
            }


            $(".tdMode").each(function () {
                var spanMode = $(this).find(".tempMode");
                $(this).html(spanMode); //隐藏树数据
                //判断是否外部链接
                var isLink = $(this).attr("url") == "" ? false : true;
                if (isLink) {
                    $(this).parent().find("td:gt(0)").html("");
                } else {//父节点处理
                    var pid = $(this).find("[columnpid]").attr("columnpid");
                    if (pid != "0") {
                        $(".span" + pid).hide(); //隐藏生成方式
                        $("[model='model" + pid + "']").html("");
                        $("[model='model" + pid + "']").parent().find("[name='TdTemp']").html("");
                    }

                    //选中生成方式
                    var checkedValue = $(this).find("[ckdvalue]").attr("ckdvalue");
                    $(this).find(":radio[value='" + checkedValue + "']").attr("checked", "checked");
                }
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <div>
            <asp:LinkButton ID="lbUpdate" runat="server" CssClass="aAll_button" OnClick="lbUpdate_Click" OnClientClick="return GetCreateMode()">
                <em><img src='<%=SysPath%>res/images/button_submit_icon_6.gif' /></em>
                <b><%="保存".ToLang()%></b>
            </asp:LinkButton>
        </div>
        <div class="line_border">
        </div>
        <div class="All_list">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="treeTable">
                <tr class="trClass">
                    <th style="text-align: center;">
                        <%="栏目名称".ToLang()%>
                    </th>
                    <th style="text-align: center;">
                        <%="功能模块".ToLang()%>
                    </th>
                    <th style="text-align: center;">
                        <%="首页模板".ToLang()%>
                    </th>
                    <th style="text-align: center;">
                        <%="列表模板".ToLang()%>
                    </th>
                    <th style="text-align: center;">
                        <%="内容模板".ToLang()%>
                    </th>
                    <th style="text-align: center;">
                        <%="生成模式".ToLang()%>
                    </th>
                </tr>
                <tbody id="tbdColumnList">
                    <asp:Repeater ID="rptSiteMap" runat="server">
                        <ItemTemplate>
                            <tr valign="bottom">
                                <td>
                                    <%# Eval("ColumnName")%>
                                </td>
                                <td model="model<%# Eval("ColumnId") %>" align="right">
                                    <em></em><span id="spmodelname">
                                        <%# Eval("ModelId")%>
                                    </span>
                                    <img class="model_delete" valueplace="CloumnModel|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_delete.gif" linkurl='<%# Eval("OutUrl") %>' />
                                    <img class="model_edit" valueplace="CloumnModel|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_edit.gif" linkurl='<%# Eval("OutUrl") %>' />
                                </td>
                                <td name="TdTemp" align="right">
                                    <em></em><span>
                                        <%# Eval("DefaultTemp")%>
                                    </span>
                                    <img class="eip_delete" valueplace="DefaultTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_delete.gif" />
                                    <img class="eip_edit" valueplace="DefaultTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_edit.gif" />
                                </td>
                                <td name="TdTemp" align="right">
                                    <em></em><span>
                                        <%# Eval("ListTemp")%>
                                    </span>
                                    <img class="eip_delete" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_delete.gif" />
                                    <img class="eip_edit" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_edit.gif" />
                                </td>
                                <td name="TdTemp" align="right">
                                    <em></em><span>
                                        <%# Eval("ContentTemp")%>
                                    </span>
                                    <img class="eip_delete" valueplace="ContentTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_delete.gif" />
                                    <img class="eip_edit" valueplace="ContentTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"
                                        src="<%=SysPath%>res/images/eip_edit.gif" />
                                </td>
                                 <td class="tdMode" align="center" url="<%# Eval("OutUrl") %>">
                                     <span class="tempMode span<%# Eval("ColumnId") %>" columnpid="<%# Eval("ParentId") %>" columnid="<%# Eval("ColumnId") %>"" ckdvalue="<%# Eval("CreateMode") %>">
                                        <label><input type="radio" value="0" name="rdo<%# Eval("ColumnId") %>" checked="checked"/><%=CreateModeNull%></label>
                                        <label><input type="radio" value="1" name="rdo<%# Eval("ColumnId") %>"/><%=CreateModeHtml%></label>
                                        <label><input type="radio" value="2" name="rdo<%# Eval("ColumnId") %>"/><%=CreateModelAspx%></label>
                                     </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <script type="text/javascript">
            $(".eip_delete").hide();
            $(".model_delete").hide();
            $("img[class='eip_edit']").click(function () {
                $(this).prevAll("em").html($("#DivDropTemplate").html());
                $(this).prevAll("span").hide();
                $(this).prevAll("img").show();
                GetValue();
                $(this).hide();
            });

            $("img[class='eip_delete']").click(function () {
                $(this).prevAll("em").html('');
                $(this).prevAll("span").show();
                $(this).next("img").show();
                GetValue();
                $(this).hide();
            });


            $("img[class='model_edit']").click(function () {
                if ($(this).attr("linkurl") != "") {
                    TipMessage('<%="外部链接不能选择改变功能模块".ToLang() %>');
                } else {
                    $(this).prevAll("em").html($("#DivDropColumn").html());
                    $(this).prevAll("span").hide();
                    $(this).prevAll("img").show();
                    GetColumnValue();
                    $(this).hide();
                }
            });

            $("img[class='model_delete']").click(function () {
                $(this).prevAll("em").html('');
                $(this).prevAll("span").show();
                $(this).next("img").show();
                GetColumnValue();
                $(this).hide();
            });

            function GetValue() {
                var str = "";
                $("select[name='ctl00$ContentPlaceHolder1$dropTemplate']").each(function () {
                    if (str == '') {
                        str = $(this).parent().nextAll("img").attr("valueplace");
                    }
                    else {
                        str = str + "," + $(this).parent().nextAll("img").attr("valueplace");
                    }
                });
                $("#<%= ColumnFieldValue.ClientID %>").val(str);
            }

            function GetColumnValue() {
                var str = "";
                $("select[name='ctl00$ContentPlaceHolder1$dropColumn']").each(function () {
                    if (str == '') {
                        str = $(this).parent().nextAll("img").attr("valueplace");
                    }
                    else {
                        str = str + "," + $(this).parent().nextAll("img").attr("valueplace");
                    }
                });
                $("#<%= ModelFieldValue.ClientID %>").val(str);
            }

            function GetCreateMode() {
                var str = "";
                $(".tempMode[columnpid][columnid]").find(":radio[checked='true']").each(function () {
                    var columnid = $(this).parent().parent().attr("columnid");
                    var createid = $(this).val();
                    str += columnid + "|" + createid + ",";
                });
                $("#<%=hdCreateMode.ClientID %>").val(str);
                return true;
            }
        </script>
        <asp:HiddenField ID="ColumnFieldValue" runat="server" />
        <asp:HiddenField ID="ModelFieldValue" runat="server" />
        <asp:HiddenField ID="hfStrMap" runat="server" />
        <asp:HiddenField ID="hdCreateMode" runat="server" />
        <div style="display: none" id="DivDropTemplate">
            <asp:DropDownList ID="dropTemplate"  Width="150" runat="server">
            </asp:DropDownList>
        </div>
        <div style="display: none" id="DivDropColumn">
            <asp:DropDownList ID="dropColumn" runat="server">
            </asp:DropDownList>
        </div>
        <asp:Label ID="ltLinkTest" runat="server"></asp:Label>
    </div>
</asp:Content>
