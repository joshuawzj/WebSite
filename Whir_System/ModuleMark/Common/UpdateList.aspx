<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="UpdateList.aspx.cs" Inherits="Whir_System_ModuleMark_Common_UpdateList" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
	    $(function () {

            $("[name=_dialog] .btn-primary", parent.document).text("<%="保存".ToLang()%>");
	        $("[name=_dialog] .btn-primary", parent.document)
	            .click(function() {
	                $("#formEdit").bootstrapValidator('validate');
	                if ($("#formEdit").data('bootstrapValidator').isValid()) {
	                    $(document).click();
	                    var data = $("#formEdit").serialize() + "&columnid=<%=ColumnId %>&SubjectId=<%=SubjectId %>&itemIds=<%=ItemIds %>&exceptFields=<%=ExceptFields %>";
	                    whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=UpdateList",{
	                            data: data,
	                            success: function(response) {
	                                whir.loading.remove();
	                                if (response.Status == true) {
	                                    window.parent.whir.toastr.success(response.Message);
	                                    window.parent.$table.bootstrapTable('refresh');
	                                    window.parent.whir.dialog.remove();
	                                } else {
	                                    whir.toastr.error(response.Message);
	                                }
	                            }
	                        });
	                    return false;
	                }

	            });
	    });
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<whir:DynamicForm id="dynamicForm1" runat="server" FormType="All"></whir:DynamicForm>
</asp:Content>

