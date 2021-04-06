<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Detaillist.aspx.cs" Inherits="Whir_System_ModuleMark_Survey_Detaillist"%>

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
                        <a  href="Surveylist.aspx?columnid=<%=SurveyColumnID%>&subjectid=<%=SubjectId%>"><%="主题列表".ToLang()%></a>
                         </li>
                          <li class="active"><a data-toggle="tab"  aria-expanded="true"><%=ColumnName%></a></li>
                         </ul>   
                     
                    <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
