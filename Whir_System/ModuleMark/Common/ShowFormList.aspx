<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="ShowFormList.aspx.cs" Inherits="Whir_System_ModuleMark_Common_ShowFormList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document)
                .click(function() {
                    var isSelect = whir.checkbox.isSelect('cbxIsListShow');
                    if (!isSelect) {
                        window.parent.TipMessage('<%="请选择".ToLang()%>');
                        return false;
                    } else {
                        var selete = "";
                        $('input[name$=cbxIsListShow]')
                            .each(function() {
                                var checkstr = "0";
                                if ($(this).prop('checked')) {
                                    checkstr = "1";
                                }
                                selete += $(this).attr("FormId") + "," + checkstr + "|";
                            });
                        selete = selete.substring(0, selete.length - 1);
                        whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=ShowFormatList",
                            {
                                data: {
                                    selected: selete
                                },
                                success: function(response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        window.parent.whir.toastr.success(response.Message);
                                        window.parent.location.href = window.parent.location.href;
                                    } else {
                                        window.parent.whir.toastr.error(response.Message);
                                    }
                                }
                            }
                        );
                        return false;
                    }
                });
        });

    </script>
    <script type="text/javascript">
        //选中已有表单
        function getSelected() {
            var selected = "";
            $("#tbdFormList").find("input[type='checkbox']:checked").each(function () {
                var formId = $(this).attr("FormId");
                selected += formId + ",";
            });
            selected = selected.substring(0, selected.length - 1);
            $("#hidSelected").val(selected);
        }

        //获取排序的值, 并赋值给hidSort
        function getSort() {
            var result = "";
            $("#tbdFormList").find("input[type='text']").each(function () {
                var formId = $(this).attr("FormId");
                var sort = $(this).val();
                result += formId + "|" + sort + ",";
            });

            if (result != '') {
                result = result.substring(0, result.length - 1);
                $("#hidSort").val(result);
            } else {
                window.parent.TipMessage('<%="请选择".ToLang() %>');
                return false;
            }
            return true;
        }

        //全选
        function selectAll(flag) {
            $("#tbdFormList").find("input[type='checkbox']").each(function () {
                if (flag)
                    $(this).attr("checked", true);
                else
                    $(this).attr("checked", false);
            });
            getSelected();
        }

        $(function () {
            //表头全选
            $("#cbxAll").click(function () {
                var checked = $(this).attr("checked");
                selectAll(checked);
            });
            //拖动排序
            $(".sortTable").sortable(
            {
                items: "tr[sort]",
                appendTo: 'body',
                handle: '.pointer',
                stop: function (event, ui) {
                    saveSort(event, ui);
                },
                axis: 'y'
            });
        });


        //异步保存排序
        function saveSort(event, ui) {
            var intdata, intdata2;
            intdata = $("#tbdFormList tr").eq(ui.item[0].rowIndex - 1).find("td:first").attr("itemid");
            if (ui.item[0].rowIndex == 1) {
                intdata2 = intdata; //取sort字段的值desc
            } else {
                intdata2 = $("#tbdFormList tr").eq(ui.item[0].rowIndex - 2).find("td:first").attr("itemid");
            }
            var datas = { "formid1": intdata, "formid2": intdata2 };
            $.ajax({
                type: "get",
                url: "<%=SysPath %>ajax/developer/formsort.aspx?time=" + new Date().getMilliseconds(),
                data: datas,
                success: function (msg) {
                    if (msg == "") {
                        window.parent.TipError('<%="排序失败".ToLang()%>');
                    } else {
                        setTableStyle();
                       window.parent.whir.toastr.success('<%="排序成功".ToLang()%>');
                    }
                }
            });
        }

        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[sort]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="sortTable controller table table-bordered table-noPadding">
            <tr class="trClass">
                <th width="40px">
                    &nbsp;
                </th>
                <th>
                    <%="列名".ToLang()%>
                </th>
                <th>
                    <%="字段名".ToLang()%>
                </th>
            </tr>
            <tbody id="tbdFormList">
                <asp:Repeater ID="rptFormInList" runat="server">
                    <ItemTemplate>
                        <tr sort="true">
                            <td style="vertical-align:middle" sortid='<%# Eval("Key.Sort") %>' itemid='<%# Eval("Key.FormId") %>'>
                                <input type="checkbox" name="cbxIsListShow" IsEnable="<%# Eval("Value.IsEnableListShow") %>" FormId="<%# Eval("Key.FormId") %>" IsListShow="<%# Eval("Key.IsListShow") %>" />
                            </td>
                            <td class="pointer" style="cursor: move;">
                                <%# Eval("Key.FieldAlias") %>
                            </td>
                            <td>
                                <%# Eval("Value.FieldName")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hidSelected" />
        <input type="hidden" id="hidSort" />
    </div>
   </div>
   <script type="text/javascript">
       $(document).ready(function() {
           $("#tbdFormList")
               .find("[name=cbxIsListShow]")
               .each(function() {
                   if ($(this).attr("IsEnable") == "0") {
                       $(this).attr("disabled", true);
                   }
                   if ($(this).attr("IsListShow") == "True") {
                       $(this).iCheck('check');
                   }
               });
       })
   </script>
</asp:Content>
