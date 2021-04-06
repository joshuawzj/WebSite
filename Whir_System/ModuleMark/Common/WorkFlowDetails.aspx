<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="workflowdetails.aspx.cs" Inherits="whir_system_ModuleMark_jobs_workflowdetails" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $(function () {
            $(".panel-bottom", parent.document).append($(".hide").removeClass("hide"));

            $("#btn_passed", parent.document).click(function () {
                whir.dialog.confirm("<%="确定通过审核？".ToLang() %>", function () {
                    var keys = "<%=RequestString("itemid")+"|"+RequestString("columnid")+"|"+RequestString("workflow")+"|"+RequestString("state")+"|"+RequestString("subjectid")%>";
                    whir.ajax.post("<%=SysPath%>Handler/Common/UnauditedList.aspx?_action=BatchEvent",
                        {
                            data: {
                                cmd: "passflow",
                                cbPosition: keys,
                                reason: ""
                            },
                            success: function (response) {
                                if (response.Status == true) {
                                    window.parent.whir.toastr.success(response.Message);
                                    window.parent.search();
                                    window.parent.whir.dialog.remove();
                                } else {
                                    whir.toastr.error(response.Message);
                                    whir.dialog.remove();
                                }
                            }
                        });
                });
                 
                return false;
            });

            $("#btn_retired", parent.document).click(function () {
                //打开退审页面
                var keys = "<%=RequestString("itemid")+"|"+RequestString("columnid")+"|"+RequestString("workflow")+"|"+RequestString("state")+"|"+RequestString("subjectid")%>";
                whir.dialog.frame('<%="退审理由".ToLang()%>', "WorkFlowReasonBatch.aspx?subjectid=<%=RequestString("subjectid") %>&cbPosition=" + keys + "&time=" + new Date(), null, 500, 300);
                return false;

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-body">
        <div class="form_center" style="width: 100%">
            <div class="row">
                <div class="col-md-8">
                    <div class="panel panel-default panels " id="panelleft">
                        <div class="panel-heading">
                            <%="表单内容".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <whir:DetailsForm ID="detailsForm1" runat="server" FormType="Left"></whir:DetailsForm>
                        </div>

                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-default panels ">
                        <div class="panel-heading">
                            <%="表单相关".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <whir:DetailsForm ID="detailsForm2" runat="server" FormType="Right"></whir:DetailsForm>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="btn-group hide">
        <button type="button" id="btn_passed" class="btn btn-white"><%="审核通过".ToLang() %></button>
        <button type="button" id="btn_retired" class="btn text-danger border-danger"><%="退审".ToLang() %></button>
    </div>
</asp:Content>
