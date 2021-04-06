<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="View.aspx.cs" Inherits="Whir_System_ModuleMark_Survey_View" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath%>res/js/jquery.form.js" type="text/javascript"></script>
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
                        <asp:Literal ID="ltlSurveyTitle" runat="server"></asp:Literal></div>
                    <div class="col-md-10 " id="Div1">
                        <table width="100%" border="0" cellspacing="0" cellpadding="3" id="tbQuestions">
                            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <b>
                                                <%# Container.ItemIndex + 1 + "． " + Eval("Name")%></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px;">
                                            <whir:AnswerForm ID="AnswerForm1" runat="server"></whir:AnswerForm>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
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
