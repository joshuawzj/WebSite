<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="FormImport.aspx.cs" Inherits="Whir_System_ModuleMark_Common_FormImport" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script>
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                $("#<%=rdbStrategy.ClientID%>").val($("input[name='Strategy']:checked").val());
                WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("<%= lbnImport.UniqueID %>", "", true, "", "", false, true));
                return false;
            });

            $("#txt_file")
                .fileinput({
                    language: '<%=GetLoginUserLanguage()%>',
                    uploadUrl: "<%=SysPath%>Ajax/Extension/uploadfiles.aspx?FormId=0&savePath=<%= UploadFilePath%>file/&fileName=",
                    allowedFileExtensions: ['xls', 'xlsx'],
                    initialCaption: '<%="支持格式：".ToLang()%>xls、xlsx',
                    noPicker: true
                })
                .on("filebatchselected",
                function (event, files) {
                    $(this).fileinput("upload");
                })
                .on("fileuploaded", function (event, data) {
                    if (data.response && data.response.Result == true) {
                        $(".file-input").hide();
                        $("#<%=txtPath.ClientID%>").val(data.response.Msg);
                        __doPostBack('ctl00$ContentPlaceHolder1$lbnUploaded', '');
                    } else {
                        whir.toastr.error(data.response.Msg);
                    }
                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-body">
        <form runat="server">
            <div class="alert alert-danger no-opacity">
                <button data-dismiss="alert" class="close" type="button">×</button>
                <ul>
                    <li><%="请参照模板填充数据：".ToLang() %><a href="#"><asp:LinkButton ID="lbnDownTemplate" ValidationGroup="100"
                        runat="server" OnClick="lbnDownTemplate_Click"> <%="模板下载".ToLang()%></asp:LinkButton></a></li>
                    <li><%="导入的数据文件必须为可识别的Excel文件，后缀名为.xls、.xlsx，且数据文件大小不可大于20M".ToLang() %></li>
                    <li><%="不导入CreateDate时，系统默认添加当前时间".ToLang() %></li>
                </ul>
            </div>
            <asp:PlaceHolder ID="phUpload" runat="server" >
                <input id="txt_file" type="file" class="file-loading">
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phField" runat="server" Visible="false">

                <div class="form-horizontal">
                    <div class=" form-group">
                        <div class="col-sm-3"><%="重复策略：".ToLang()%></div>
                        <div class="col-sm-9" >
                            <ul class="list" >
                                <li>
                                    <input type="radio" checked="checked" name="Strategy" value="0" />
                                    <label for="IsAllowNull_True">
                                        <%="跳过".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" name="Strategy" value="1" />
                                    <label for="IsAllowNull_False">
                                        <%="覆盖".ToLang()%></label>
                                </li>
                            </ul>

                            <asp:HiddenField ID="rdbStrategy" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                </div>

                <div class="All_list">
                    <table class="list_table" class="table table-hover" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <th width="150">
                                <%="模板列".ToLang() %>
                            </th>
                            <th>
                                <%="列名".ToLang() %>
                            </th>
                            <th>
                                <%="字段名".ToLang() %>
                            </th>
                            <th>
                                <%="数据类型".ToLang() %>
                            </th>
                            <th>
                                <%="检测唯一性".ToLang() %>
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="150px">
                                        <whir:DropDownList ID="ddlExcelColumn" Width="120px" CssClass="form-control" runat="server">
                                        </whir:DropDownList>
                                    </td>
                                    <td>
                                        <%# Eval("Key.FieldAlias") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litFieldName" runat="server" Text='<%# Eval("Value.FieldName") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litTypeName" runat="server" Text='<%# Eval("Value.TypeName") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ckbOnly" runat="server" TypeName='<%# Eval("Value.TypeName") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="4"></td>
                        </tr>
                    </table>
                </div>
            </asp:PlaceHolder>

            <div style="display: none">
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                <asp:HiddenField ID="txtPath" runat="server"></asp:HiddenField>
                <asp:LinkButton ID="lbnUploaded" runat="server" OnClick="lbnUploaded_Click" OnClientClick=""><%="上传后".ToLang()%></asp:LinkButton>
                <asp:LinkButton ID="lbnImport" runat="server" OnClick="lbnImport_Click"><%="导入".ToLang()%></asp:LinkButton>
            </div>
        </form>
    </div>
</asp:Content>
