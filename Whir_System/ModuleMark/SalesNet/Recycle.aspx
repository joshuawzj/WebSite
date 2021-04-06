<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Recycle.aspx.cs" Inherits="Whir_System_ModuleMark_SalesNet_Recycle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%@ Import Namespace="Whir.Language" %>
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li><a href="<%=BackPageUrl%>" aria-expanded="true"><%=ColumnName%></a></li>
                        <li class="active"><a data-toggle="tab" aria-expanded="true"><span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%></a></li>
                    </ul>
                    <div class="space15">
                    </div>
                    <whir:ContentManager ID="contentManager1" runat="server"></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>

