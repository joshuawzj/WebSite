<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="SelectField.aspx.cs" Inherits="Whir_System_ModuleMark_Common_SelectField" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript">
        // var $table = $('#Common_Table');
        $(document).ready(function () {
            whir.checkbox.checkboxOnload("", "hidSelected", "cb_Top", "cb_Position");
        });

        $(function () {

            $("[name=_dialog] .btn-primary", parent.document).text("<%="下一步".ToLang()%>");
            $("[name=_dialog] .btn-primary", parent.document)
                .click(function () {
                    var editFieldIds = $("#hidSelected").val();
                    if (editFieldIds === "" || editFieldIds.split(',') < 1) {
                        TipMessage("<%="至少选择一个字段进行修改".ToLang()%>");
                    } else {

                        // 将选择的数据附带到下一个处理页面
                        var values = '<%=Whir.Framework.RequestUtil.Instance.GetString("except") %>';
                        var url = "<%=SysPath%>Modulemark/Common/UpdateList.aspx?columnId=<%=ColumnId %>&fieldids=" + editFieldIds + "&SubjectId=<%=SubjectId %>&itemIds=<%=ItemIds%>";
                        // 将选择的数据附带到下一个处理页面

                        location.href = url;
                    }
                    return false;

                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <table id="Common_Table" width="100%" border="0" cellspacing="0" cellpadding="0" class="controller table sortTable table-bordered table-noPadding">
                    <tr class="trClass">
                        <th width="40px">
                             <div class="th-inner ">
                                <input id="chkAll" type="checkbox" name="cb_Top" title="<%="全选/全不选".ToLang()%>" /></div> 
                        </th>
                        <th>
                            <%="列名".ToLang()%>
                        </th>
                        <th>
                            <%="字段名".ToLang()%>
                        </th>
                        <th width="25%"><%="操作".ToLang()%>
                        </th>
                    </tr>
                    <tbody id="tbdFormList">
                        <asp:Repeater ID="rptFormInList" runat="server">
                            <ItemTemplate>
                                <tr sort="true">
                                    <td>
                                        <input type="checkbox" value='<%# Eval("Key.FieldId") %>' name="cb_Position" />
                                    </td>
                                    <td class="pointer">
                                        <%# Eval("Key.FieldAlias") %>
                                    </td>
                                    <td>
                                        <%# Eval("Value.FieldName")%>
                                    </td>
                                    <td align="center">
                                        <div class="alistButton">
                                            <a class="btn btn-info" href="<%=SysPath%>modulemark/common/updatelist.aspx?columnId=<%=ColumnId %>&fieldids=<%# Eval("Key.FieldId") %>&SubjectId=<%=SubjectId %>&itemIds=<%=ItemIds%>"><%="修改".ToLang()%></a>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div style="display: none;">
                <input type="hidden" id="hidSelected" />
            </div>
        </div>
    </div>
</asp:Content>
