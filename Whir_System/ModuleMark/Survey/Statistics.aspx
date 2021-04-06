<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="Whir_System_ModuleMark_Survey_Statistics" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/progressbar/jquery.progressbar.min.js" type="text/javascript"></script>
    <script language="javascript">
        function killErrors() {
            return true;
        }
        //假如总数为0时，js会报错,so屏蔽js报错
        window.onerror = killErrors;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">

            <form enctype="multipart/form-data" class="form-horizontal bv-form">
                <div class="panel-body">

                    <ul class="nav nav-tabs">
                        <li>
                            <a href="Surveylist.aspx?columnid=<%=ColumnId %>&subjectid=<%=SubjectId %>"><%=ColumnName%></a>
                        </li>
                        <li class="active"><a data-toggle="tab" aria-expanded="true"><%="统计".ToLang()%></a></li>
                    </ul>
                    <br />
                    <div class="form-group" style="text-align: center">
                        <asp:Literal ID="ltlSurveyTitle" runat="server"></asp:Literal>
                    </div>
                    <table width="100%" border="0" cellspacing="0" cellpadding="3" class="table table-bordered table-noPadding">
                        <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Container.ItemIndex + 1 + "． " + Eval("Name")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <asp:Repeater ID="rptAnswers" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <span style="padding-left: 20px;"></span>
                                                            <%# Eval("Name")%>
                                                        </td>
                                                        <td>
                                                            <span class="progressbar">
                                                                <%# Eval("AnswerCount") %></span> <span style="display: none;">
                                                                    <%# GetTotalCount(Eval("QuestionId").ToInt())%></span>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </form>
        </div>
    </div>
    <script language='javascript' type='text/javascript'>
        $(function () {
            $(".progressbar").each(function () {
                var jq = $(this);
                whir.progressbar.setProgressbarByFraction("<%=SysPath %>", jq, jq.next().text());
            })
        });
    </script>
</asp:Content>
