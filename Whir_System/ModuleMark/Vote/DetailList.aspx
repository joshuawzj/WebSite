<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="DetailList.aspx.cs" Inherits="Whir_System_ModuleMark_Vote_DetailList" %>

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
                            <a href="Votelist.aspx?columnid=<%=VoteColumnID%>&subjectid=<%=SubjectId%>"><%="投票主题".ToLang()%></a>
                        </li>
                        <li class="active"><a data-toggle="tab" aria-expanded="true"><%=ColumnName%></a></li>
                    </ul>
                    <div class="space15">
                    </div>
                    <whir:ContentManager ID="contentManager1" runat="server"></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
