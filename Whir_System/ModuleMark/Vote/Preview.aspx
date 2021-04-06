<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Preview.aspx.cs" Inherits="Whir_System_ModuleMark_Vote_Preview" %>

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
                    <a  href="votelist.aspx?columnid=<%=ColumnId %>&subjectid=<%=SubjectId %>"><%="投票主题".ToLang()%></a>
                    </li>
                    <li class="active"><a data-toggle="tab"  aria-expanded="true"><%="预览".ToLang() %></a></li>
                  </ul>   
                  <br />
                 
                <div class="form-group">
                    <div class="col-md-2 text-right">
                        <asp:Literal ID="ltlSurveyTitle" runat="server"></asp:Literal></div>
                    <div class="col-md-10 ">
                        <whir:AnswerForm ID="AnswerForm1" runat="server"></whir:AnswerForm>
                    </div>
                </div>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
