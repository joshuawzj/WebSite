<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="Sort.aspx.cs" Inherits="Whir_System_ModuleMark_Common_Sort" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        //添加按钮
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document)
                .click(function () {
                    if ($(".active.tab-pane").attr("Id") == "num") {

                        if ($("[name=SelectedOpen]").val() >= 1 && $("[name=SelectedOpen]").val() <= <%=Total%>) {
                            var $form = $("#formEdit");
                        $form.post({
                            success: function (response) {
                                if (response.Status == true) {
                                    window.parent.whir.toastr.success(response.Message);
                                    window.parent.resetChoose();
                                    window.parent.whir.dialog.remove();
                                } else {
                                    window.parent.whir.toastr.error(response.Message);
                                }
                                whir.loading.remove();
                            },
                            error: function (response) {
                                window.parent.whir.toastr.error(response.Message);
                                whir.loading.remove();
                            }
                        });
                    }
                    else
                        window.parent.whir.toastr.error('<%="请输入 1-"+(Total)+" 数字进行排序" %>');
                    return false;

                }
                else {
                        var selected = $('#hidChoose').val();
            if (selected == "") {
                window.parent.TipMessage('<%="请选择".ToLang() %>');
                return false;
            } else {
                window.parent.sortAction(selected);
                return false;
            }
        }
        });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel-body">
        <div class="panel">
            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#num" aria-expanded="true"><%="输入序号排序".ToLang()%></a></li>
                <li class=""><a data-toggle="tab" href="#content" aria-expanded="false"><%="选择位置排序".ToLang()%></a></li>
            </ul>
            <div class="tab-content">
                <div id="num" class="tab-pane active">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Common.aspx">
                        <div class="panel-body">
                            <span class="note_gray"><%="请输入".ToLang()+" 1-"+(Total)+" "+"的数字进行排序，已选中的数据将会排列在该数字后面".ToLang()%></span>
                            <input type="number" name="SelectedOpen" value="1" style="width: 100px" min="1" max="<%=Total %>" class="form-control" />
                            <input type="hidden" name="ColumnId" value="<%=ColumnId %>" class="form-control" />
                            <input type="hidden" name="Total" value="<%=Total %>" class="form-control" />
                            <input type="hidden" name="SubjectId" value="<%=SubjectId %>" class="form-control" />
                            <input type="hidden" name="Selected" value="<%=ExceptPrimaryIDs %>" class="form-control" />
                            <input type="hidden" name="_action" value="OpenSortByNum" class="form-control" />
                        </div>
                    </form>
                </div>
                <div id="content" class="tab-pane">
                    <div class="space15"></div>
                       <span class="note_gray"><%="已选中的数据将会排列在下面放选中的数据前面".ToLang() %></span>
                    <whir:ContentManager ID="contentManager1" runat="server"></whir:ContentManager>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

