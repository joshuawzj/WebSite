<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="JobRequestDetails.aspx.cs" Inherits="Whir_System_ModuleMark_Jobs_JobRequestDetails" %>

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
                    <div class="panel panel-default panels col-md-7" id="panelleft" style=" margin-right: 5px; padding-left: 0px; padding-right: 0px">
                        <div class="panel-heading">
                            表单内容
                        </div>
                        <div class="panel-body">
                            <whir:DetailsForm ID="detailsForm1" runat="server" FormType="Left"></whir:DetailsForm>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <a class="btn btn-warning" href="javascript:history.back();">
                                        <%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default panels col-md-4" style="padding-right: 0px; padding-left: 0px">
                        <div class="panel-heading">
                            表单相关
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
