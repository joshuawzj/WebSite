<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteColumn_Copy.aspx.cs" Inherits="Whir_System_Module_Column_SiteColumn_Copy" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var options = {
            fields: {
                Site: {
                    validators: {
                        notEmpty: {
                            message: '<%="站点为必填项".ToLang() %>'
                        }
                    }
                },
                <% if (!IsDevUser)
                    { %>
                Column: {
                    validators: {
                        notEmpty: {
                            message: '<%= "栏目为必选项".ToLang() %>'
                        }
                    }
                },
                <% }%>
                Name: {
                    validators: {
                        notEmpty: {
                            message: '<%="目标名称为必填项".ToLang() %>'
                        }
                    }
                },
                Path: {
                    validators: {
                        regexp: {
                            regexp: /^[a-zA-Z0-9]{1,24}$/,
                            message: '<%="目标路径只能是1-24个英文或数字".ToLang() %>'
                           }
                       }
                   },
                   Quantity: {
                       validators: {
                           regexp: {
                               regexp: /^[0-9]{1,12}$/,
                               message: '<%="复制个数只能是正整数".ToLang() %>'
                        }
                    }
                }
            }
        };

        $(function () {

            $('#formSingle').Validator(options, function (el) {

            });

            $("[name=_dialog] .btn-primary", parent.document)
                .click(function () {
                    $('#formSingle').bootstrapValidator('validate');
                    if ($('#formSingle').data('bootstrapValidator').isValid()) {
                        var columnId = <%=ColumnId %>;

                    var ddlColumn = $("[name=Column]").val();
                    if (ddlColumn == columnId) {
                        window.parent.whir.toastr.error("<%="复制源栏目不能是目标栏目的父级！".ToLang()%>");
                        return false;
                    }
                    whir.ajax.post("<%=SysPath%>Handler/Developer/Site.aspx", {
                        data: {
                            SiteId: <%=SiteId %>,
                            ColumnId: columnId,
                            Quantity: $("[name=Quantity]").val(),
                            Name: $("[name=Name]").val(),
                            Path: $("[name=Path]").val(),
                            ddlSite: $("[name=Site]").val(),
                            ddlColumn: $("[name=Column]").val(),
                            _action: "SiteColumnCopy"
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                window.parent.whir.toastr.success(response.Message, true, true);
                            } else {
                                window.parent.whir.toastr.error(response.Message);
                            }
                        }
                    });
                }
                whir.dialog.remove();
                return false;
                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel-body">
            <form id="formSingle" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Site.aspx">
                <div class="form-group">
                    <div class="col-xs-4 text-right" for="Source"><%="复制源：".ToLang()%></div>
                    <div class="col-xs-8 ">
                        <asp:Literal ID="litSource" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-4 control-label text-right" for="Name">
                        <%=(ColumnId!=0?"目标栏目名称":"目标站点名称：").ToLang()%>
                    </div>
                    <div class="col-xs-8 ">
                        <input type="text" id="Name" name="Name" value="" class="form-control" required="true"
                            maxlength="24" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-4 control-label text-right" for="Path">
                        <%="英文目录：".ToLang()%>
                    </div>
                    <div class="col-xs-8 ">
                        <input type="text" id="Path" name="Path" value="" class="form-control" required="true"
                            maxlength="128" />
                    </div>
                </div>
                <asp:PlaceHolder ID="phColumnInfo" runat="server">
                    <div class="form-group">
                        <div class="col-xs-4 control-label text-right" for="Quantity">
                            <%="复制个数：".ToLang()%>
                        </div>
                        <div class="col-xs-8 ">
                            <input type="text" id="Quantity" name="Quantity" value="1" class="form-control" required="true"
                                maxlength="12" />
                        </div>
                    </div>
                    <whir:SiteColumn ID="SiteColumn1" runat="server" />
                </asp:PlaceHolder>
            </form>

        </div>
    </div>
</asp:Content>
