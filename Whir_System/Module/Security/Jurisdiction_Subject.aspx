<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Jurisdiction_subject.aspx.cs" Inherits="whir_system_module_security_Jurisdiction_subject" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register Src="Jurisdiction_column_top.ascx" TagName="Jurisdiction_column_top"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        whir.skin.checkbox(false); //取消美化checkbox
        jQuery(document).ready(function () {
            //快速选择、按功能快速选择，一定要位于点击事件前面，否则无法触发点击事件
            var arr = new Array();
            $("input[functionname]").each(function (index) {
                arr[index] = $(this).attr("functionname");
            });
            arr = unique(arr);
            var str = "&nbsp;&nbsp;";
            for (var i = 0; i < arr.length; i++) {
                str += " <input type=\"checkbox\"  name=\"selectColumn\" value=\"" + arr[i] + "\"  functionname=\"" + arr[i] + "\" />" + arr[i] + "";
            }
            $("#sampleSelect").html(str);

            //按功能快速选择
            $("input[name='selectColumn']").click(function () {
                var functionName = $(this).val();
                var result = $(this).prop("checked");
                if (result == "checked" || result == "true" || result == true) {
                    $("input[functionname='" + functionName + "']").prop("checked", true);
                } else {
                    $("input[functionname='" + functionName + "']").prop('checked', false);
                }
            });

            //按栏目快速选择
            $("input[name='SelectByColumnId']").click(function () {
                var result = $(this).prop("checked");
                if (result == "checked" || result == "true" || result == true) {
                    $(this).parent().parent().find(":checkbox").prop("checked", true);
                } else {
                    $(this).parent().parent().find(":checkbox").prop('checked', false);
                }
            });

            //全选 
            $("#selectAll").click(function () {
                $("input[functionname]").prop("checked", true);
            });
            //取消
            $("#cancelAll").click(function () {
                $("input[functionname]").prop('checked', false);
            });

            //打开类别窗口
            $("a[maincolumnid]").click(function () {
                var columnid = $(this).attr("columnid");
                var maincolumnid = $(this).attr("maincolumnid");
                var categoryids = $(this).attr("selectCategory");
                var subjectId = $(this).attr("subjectid");
                whir.dialog.frame('<%="分配类别权限".ToLang()%>'
                    , "<%=SysPath%>module/security/Jurisdiction_category.aspx?columnid=" + columnid + "&maincolumnid=" + maincolumnid +
                    "&subjectId=" + subjectId + "&roleid=<%=RoleId%>&siteId=<%=SiteId%>&SubjectTypeId=<%=SubjectTypeId%>&jscallback=CategoryCallBack&ids=" + categoryids, 'showCategory', 500, 300);
            });

        });


        //分配类别权限回调函数
        function CategoryCallBack(data, columnid, maincolumnid) {
            $("a[maincolumnid='" + maincolumnid + "']").attr("selectCategory", data);
            var d = $("a[maincolumnid='" + maincolumnid + "']").attr("selectCategory");
        }

        //获取类别信息
        function getCategoryData() {
            var categoryData = "";
            $("a[maincolumnid]").each(function () {
                var columnid = $(this).attr("columnid");
                var subjectid = $(this).attr("subjectid");
                var categoryids = $(this).attr("selectCategory");
                var ids = categoryids.split(',');
                for (var i = 0; i < ids.length; i++) {
                    if (ids[i] != "") {
                        categoryData += "category|siteId<%=SiteId%>|type<%=SubjectTypeId%>|" + subjectid + "|" + columnid + "|" + ids[i] + ",";
                    }
                }
            });
            $("#hidCategoryData").val(categoryData);
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="formEdit" form-url="<%=SysPath%>Handler/Developer/Jurisdiction.aspx">
        <div class="content-wrap">
            <div class="space15">
            </div>
            <div class="panel">

                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li><a href="RoleList.aspx" aria-expanded="true"><%="角色管理".ToLang() %></a></li>
                        <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="栏目权限".ToLang() %></a></li>
                    </ul>
                    <div class="space15"></div>
                    <uc1:Jurisdiction_column_top ID="Jurisdiction_column_top1" runat="server" />

                    <div class="panel panel-default" id="Table_ColumnFunctionList" runat="server" visible="false">
                        <div class="panel-heading">
                            &nbsp; <%="按功能快速选择".ToLang() %>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="site_select" id="sampleSelect">
                                </div>
                            </div>
                        </div>
                    </div>

                    <table width="100%" border="0" cellspacing="0" cellpadding="0" id="Table_ColumnList"
                        runat="server" visible="false">
                        <tr>
                            <td class="box_common" valign="top">
                                <div class="list-group-item ">
                                    <table width="100%" class=" table-hover">
                                        <tr style="height: 40px">
                                            <th width="30" class="th_center"></th>
                                            <th width="20%">
                                                <%="栏目名称".ToLang()%>
                                            </th>
                                            <th>
                                                <a id="selectAll" style="cursor: pointer;"><%="全选".ToLang() %></a>/<a id="cancelAll" style="cursor: pointer;"><%="取消".ToLang() %></a>
                                            </th>
                                        </tr>
                                        <asp:Repeater ID="rptColumnList" runat="server">
                                            <ItemTemplate>
                                                <tr style="height: 40px">
                                                    <td align="center">
                                                        <input type="checkbox" name="SelectByColumnId" value=" " />
                                                    </td>
                                                    <td>
                                                        <%#Eval("name")%><%# GetColumnCategoryFunction(Eval("ResultType").ToStr(), Eval("IdValue").ToStr(), Eval("SubjectID").ToStr())%>
                                                    </td>
                                                    <td>
                                                        <%#GetColumnFunctionCheckBox(Eval("ResultType").ToStr(), Eval("IdValue").ToStr(), Eval("SubjectID").ToStr(), Eval("FunctionIDs").ToStr())%>
                                                        <br />
                                                        <%# GetColumnWorkFlowCheckBox(Eval("IdValue").ToStr(), Eval("SubjectID").ToStr(),Eval("ResultType").ToStr())%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                                <div class="space15"></div>
                                <div class="row">
                                    <div class="col-md-offset-2 col-md-6">
                                        <input type="hidden" id="hidCategoryData" name="CategoryData" />
                                        <input type="hidden" name="_action" value="SaveSubjectColumn" />
                                        <input type="hidden" name="SiteId" value="<%=SiteId %>" />
                                        <input type="hidden" name="RoleId" value="<%=RoleId %>" />
                                        <input type="hidden" name="SubjectId" value="<%=SubjectId %>" />
                                        <input type="hidden" name="ClassId" value="<%=ClassId %>" />
                                        <input type="hidden" name="type" value="<%=SubjectTypeId %>" />
                                        <button id="btnSave" class="btn btn-info btn-block">
                                            <%="保存".ToLang()%>
                                        </button>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
    <script>
        $("#btnSave").click(function () {
            $("#btnSave").attr("disabled", "disabled");
            if ($("input[name='isSame']").prop("checked"))
                $("input[name='isSame']").val(1);
            getCategoryData();
            $("#formEdit").post({
                success: function (response) {
                    if (response.Status == true) {
                        whir.toastr.success(response.Message, true, false);
                    } else {
                        whir.toastr.error(response.Message);
                    }
                    whir.loading.remove();
                },
                error: function (response) {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            });
            return false;
        });
    </script>
</asp:Content>
