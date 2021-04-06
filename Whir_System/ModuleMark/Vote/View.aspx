<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="View.aspx.cs" Inherits="Whir_System_ModuleMark_Vote_View" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%=ColumnName%></div>
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
            <div class="panel-body">
                <div class="actions btn-group">
                    <a class="btn btn-white" href='javascript:history.back();'><b>
                        <%="返回".ToLang()%></b></a>
                </div>
                <div class="form-group">
                    <div class="col-md-2 text-right">
                        <asp:Literal ID="ltlVoteTitle" runat="server"></asp:Literal></div>
                    <div class="col-md-10 " id="tbQuestions">
                        <whir:AnswerForm ID="AnswerForm1" runat="server"></whir:AnswerForm>
                    </div>
                </div>
            </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var idStr = "<%=Ids %>";
            var ids = idStr.split(',');
            for (var i = 0; i < ids.length; i++) {
                $("input[value='" + ids[i] + "']").iCheck('check');
                //$("input[value='" + ids[i] + "']").next().css("color", "#F00");
            }
            $("#tbQuestions input").attr("disabled", true);
        });
    </script>
</asp:Content>
