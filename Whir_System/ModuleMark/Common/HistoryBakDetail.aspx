<%@ Page Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="HistoryBakDetail.aspx.cs" Inherits="whir_system_ModuleMark_Common_HistoryBakDetail" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <div class="form_center" style="width: 100%">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="panel panel-default" id="panelleft">
                                <div class="panel-heading">表单内容</div>
                                <div class="panel-body">
                                    <whir:DetailsForm_Bak ID="detailsForm1" runat="server" FormType="Left"></whir:DetailsForm_Bak>
                                    <div class="form-group">
                                        <div class="col-md-offset-3 col-md-9 ">
                                            <a class="btn btn-warning" href="javascript:history.back();"><%="返回".ToLang()%></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">表单相关</div>
                                <div class="panel-body">
                                    <whir:DetailsForm_Bak ID="detailsForm2" runat="server" FormType="Right"></whir:DetailsForm_Bak>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
