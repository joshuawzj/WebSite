<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Answerlist.aspx.cs" Inherits="Whir_System_ModuleMark_Survey_Answerlist" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
         <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
             
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
            <div class="panel-body">
                 <ul class="nav nav-tabs">
                         <li> 
                             <a  href="Surveylist.aspx?columnid=<%=SurveyColumnID%>"><%="主题列表".ToLang()%></a>
                         </li>
                         <li>  
                             <a  href="Questionlist.aspx?columnid=<%=QuestionColumnID%>&subjectid=<%=SubjectId%>&PreColumnId=<%=SurveyColumnID%>&topicid=<%=TopicID%>"><%="问题列表".ToLang()%></a> 
                        </li>
                    <li class="active"><a data-toggle="tab"  aria-expanded="true"><%=ColumnName%></a></li>
                       <%if (IsRoleHaveColumnRes("回收站"))
                         { %>
                    <li > <a    href="Recycle.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                      <%} %>
                  </ul>
                <br />

                <div class="actions btn-group pull-left">
                    <%if (IsRoleHaveColumnRes("添加"))
                      { %>
                     <a class="btn btn-white" href="Content_edit.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&QuestionId=<%=QuestionID%>&BackPageUrl=<%=CurrentPageUrl%>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加答案".ToLang()%>
                    </a> 
                      <% }%>
                       
                </div>
                <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
</asp:content>
