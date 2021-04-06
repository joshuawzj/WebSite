<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="AnswerList.aspx.cs" Inherits="Whir_System_ModuleMark_Vote_AnswerList"%>

<%@ Import Namespace="Whir.Language"%>
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
                             <a  href="Votelist.aspx?columnid=<%=VoteColumnID%>&subjectid=<%=SubjectId%>">
                            <%="投票主题".ToLang()%></a>
                         </li>
                        
                         <li class="active"><a data-toggle="tab" aria-expanded="true"><%=ColumnName%></a></li>
                       <%if (IsRoleHaveColumnRes("回收站"))
                         { %>
                        <li > <a   href="Recycle.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                           <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                        </a></li>
                      <%} %>
                  </ul>
                <br />


                    <div class="actions btn-group pull-left">
                       
                         <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                        <a class="btn btn-white" href="content_edit.aspx?columnid=<%=ColumnId%>&QuestionId=<%=QuestionID%>&BackPageUrl=<%=CurrentPageUrl%>&subjectid=<%=SubjectId%>">
                            <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加答案".ToLang()%>
                        </a>
                         <% }%>
                        
                    </div>
                    <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
