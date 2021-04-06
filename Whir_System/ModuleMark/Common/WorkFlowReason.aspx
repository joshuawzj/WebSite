<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="WorkFlowReason.aspx.cs" Inherits="Whir_System_ModuleMark_Common_WorkflowReason" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                $("#formEdit").bootstrapValidator('validate');
                if ($("#formEdit").data('bootstrapValidator').isValid()) {
                    var data = $("#formEdit").serialize() +
                        "&ColumnID=<%=ColumnId %>&SubjectId=<%=SubjectId %>&CurrentActivityId=<%=CurrentActivityId %>&selected=<%=Selected %>";
                    whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=ReturnFlow", {
                        data: data,
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                window.parent.whir.toastr.success(response.Message);
                                window.parent.$table.bootstrapTable('refresh');
                                window.parent.whir.dialog.remove();
                            } else {
                                window.parent.whir.toastr.error(response.Message);
                            }
                        }
                    });
                    return false;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div  >
            <div class="panel-body">
                <div class="form_center">
                        <form id="formEdit" enctype="multipart/form-data" class="form-horizontal" form-url="">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Reasion">
                                <%="退审理由：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <textarea id="Reasion" name="Reasion" style="width:100%"  rows="10" class="form-control" required="true"
                                    maxlength="256"></textarea>
                            </div>
                        </div>
                        </form>
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
