<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="InForList.aspx.cs" Inherits="Whir_System_ModuleMark_Magazine_InForList"%>
<%@ Import Namespace="Whir.Language"%>
<%@ Import Namespace="Whir.Framework"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
            <div class="panel-body">
                  <ul class="nav nav-tabs">
                       <% if (AttchlistColumn != null)
                       {%>
                    <% foreach (var item in AttchlistColumn)
                       {%>
                    <% if (item.MarkType == "Chapter")
                       {%>
                    <li><a   href="<%="CategoryList.aspx?columnid={0}&itemid={1}&subjectid={2}".FormatWith(item.ColumnId,ItemID,SubjectId)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <% }%>
                    <% else if (item.MarkType == "Infor")
                       {%>
                    <li><a   href="<%="Inforlist.aspx?columnid={0}&itemid={1}&subjectid={2}".FormatWith(item.ColumnId,ItemID,SubjectId)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <% }%>
                      <%  else
                       {%>
                   <li> <a   href="<%="Contentlist.aspx?columnid={0}&subjectid={1}".FormatWith(item.ColumnId, SubjectId)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <%  }%>
                    <%  }%>
                    <% }%>
                     <li class="active"><a data-toggle="tab"  aria-expanded="true"><%=ColumnName.ToLang()%></a></li>
                       <%if (IsRoleHaveColumnRes("回收站"))
                          { %>
                    <li > <a  href="Recycle.aspx?columnid=<%=ColumnId%>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>&contentid=<%=ItemID%>&subjectid=<%=SubjectId%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                      <%} %>
                  </ul>
                <br />

                <div class="actions btn-group pull-left">
                   
                     <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                    <a class="btn btn-white" href="Content_edit.aspx?&columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&magazineid=<%=ItemID%>&BackPageUrl=<%=CurrentPageUrl%>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang()%>
                    </a> 
                     <% }%>
                     
                </div>
                <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
</asp:Content>


