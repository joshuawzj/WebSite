<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Preview.aspx.cs" Inherits="Whir_System_ModuleMark_Survey_Preview" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                        <a  href="Surveylist.aspx?columnid=<%=ColumnId %>&subjectid=<%=SubjectId %>"><%="主题列表".ToLang()%></a>
                         </li>
                          <li class="active"><a data-toggle="tab"  aria-expanded="true"><%="预览".ToLang() %></a></li>
                  </ul>   
                  <br />
                <div class="form-group" style="text-align:center">
                   <asp:Literal ID="ltlSurveyTitle" runat="server"></asp:Literal>
                </div>
                <div class="form-group">
                    <table width="100%" border="0" cellspacing="0" cellpadding="3" id="Table1" class="table table-bordered table-noPadding">
                        <tr>
                            <td width="13%" align="right">
                                <%="创建人：".ToLang() %>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="ltlCreator" runat="server"></asp:Literal>
                            </td>
                            <td width="13%" align="right">
                                <%="开始日期：".ToLang() %>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="ltlBeginDate" runat="server"></asp:Literal>
                            </td>
                            <td width="13%" align="right">
                                <%="结束日期：".ToLang() %>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="ltlEndDate" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form-group">
                    <table width="100%" border="0" cellspacing="0" cellpadding="3" id="tbQuestions" class="table table-bordered table-noPadding">
                        <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <b>
                                            <%# Container.ItemIndex + 1 + "． " + Eval("Name")%></b>
                                    </td> 
                                    <td>
                                        <whir:AnswerForm ID="AnswerForm1" runat="server"></whir:AnswerForm>

                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            </form>
        </div>
        </div>
</asp:Content>
