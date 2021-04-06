<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="ContentDetails.aspx.cs" Inherits="Whir_System_ModuleMark_Jobs_ContentDetails" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <div class="form_center" style="width: 100%; height: 100%">
                    <div class="panel panel-default panels col-md-7" id="panelleft" style=" width:66%; margin-right: 5px; padding-left: 0px; padding-right: 0px">
                        <div class="panel-heading">
                            <%="表单内容".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <whir:DetailsForm ID="detailsForm1" runat="server" FormType="Left"></whir:DetailsForm>
                            <div class="form-group">
                                <div class="col-md-offset-3 col-md-9">
                                    <a class="btn btn-warning" href="javascript:history.back();">
                                        <%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default panels col-md-5 " style=" width:33%; padding-right: 0px; padding-left: 0px">
                        <div class="panel-heading">
                            <%="表单相关".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <whir:DetailsForm ID="detailsForm2" runat="server" FormType="Right"></whir:DetailsForm>
                        </div>
                </div>
            </div>

        </div>
    </div>
    </div>
</asp:Content>

