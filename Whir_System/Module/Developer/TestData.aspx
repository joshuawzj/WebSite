<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="TestData.aspx.cs" Inherits="Whir_System_Module_Developer_TestData" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form_center {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form_center">
                    <div class="alert alert-danger no-opacity">
                        <button data-dismiss="alert" class="close" type="button">×</button>
                        <span class="entypo-attention"></span>
                        <span>
                            <%="初始化测试数据会对现有数据产生影响，请谨慎操作。".ToLang() %>
                        </span>

                    </div>
                    
                </div>
                <div class="form_center" style="text-align: center;">
                    <button class="btn btn-danger" onclick="addTestData();">初始化测试数据</button>
                 </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function addTestData() {
            whir.dialog.confirm("确认要添加测试数据吗？", function () {
                whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=addTestData",
                    {
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);

                            } else {
                                whir.toastr.error(response.Message);
                            }
                            whir.dialog.remove();
                        }
                    });
            });
        }
    </script>
</asp:Content>
