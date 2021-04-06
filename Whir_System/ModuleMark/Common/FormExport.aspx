<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="FormExport.aspx.cs" Inherits="Whir_System_ModuleMark_Common_FormExport" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                var isSelect = whir.checkbox.isSelect("cbxSelected");
                if (!isSelect) {
                    window.parent.TipMessage('<%="请选择".ToLang() %>');
                    return false;
                } else {
                    __doPostBack('<%= lbnSave.UniqueID %>', '');
                    window.parent.$("[name=_dialog] .btn-primary").attr("disabled", true); //避免误按，重复导出数据
                    window.parent.whir.toastr.success('<%="正在导出，请在导出文件结束后关闭窗口！".ToLang() %>');
                    return false;
                }
            });

            $('#cbxAll').next().click(function () {
                var selected = $(this).prev().prop('checked');
                if (selected)
                    whir.checkbox.selectAll('', '', 'cbxSelected');
                else
                    whir.checkbox.cancelSelectAll('', '', 'cbxSelected');
            });

        });
        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<form runat="server">
   <div class="bootstrap-table">
       <asp:HiddenField ID="filter" runat="server" />
        <table width="100%" class="table table-hover" border="0" cellspacing="0" cellpadding="0">
            <tr class="trClass">
                <th width="40px">
                    <div class="th-inner ">  <input type="checkbox" id="cbxAll" /></div>
                </th>
                <th>
                    <%="列名".ToLang()%>
                </th>
                <th>
                    <%="字段名".ToLang()%>
                </th>
            </tr>
            <tbody id="tbdForms">
            <asp:Repeater ID="rptFormInList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbxSelected" runat="server" />
                        </td>
                        <td>
                            <%# Eval("Key.FieldAlias") %>
                        </td>
                        <td>
                            <%# Eval("Value.FieldName") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div style="display: none;">
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:LinkButton ID="lbnSave" runat="server" OnClick="Save_Click"></asp:LinkButton>
    </div>
    </form>
    <script>
        var filter = window.parent._that.filterColumnsPartial;
        $("#<%=filter.ClientID%>").val(JSON.stringify(filter));
    </script>
</asp:Content>
