<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Relation_Link.aspx.cs" Inherits="Whir_System_ModuleMark_Common_Relation_Link" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="form_center">
            <form id="formEdit"  class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 control-label" style="margin-bottom: 0px;" for="LinkText">
                        <%="链接文字：".ToLang()%>
                    </div>
                    <div class="col-md-10 ">
                        <input type="text" id="LinkText" name="LinkText" value="" class="form-control" required="true" maxlength="200" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 control-label" style="margin-bottom: 0px;" for="LinkUrl"><%="链接地址：".ToLang()%></div>
                    <div class="col-md-10 ">
                        <input type="text" id="LinkUrl" name="LinkUrl" value="" class="form-control" required="true" maxlength="200" />
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        var options = {
            fields: {
                LinkText: {
                    validators: {
                        notEmpty: {
                            message: '<%="链接文字为必填项".ToLang() %>'
                        }
                    }
                }, LinkUrl: {
                    validators: {
                        notEmpty: {
                            message: '<%="链接地址为必填项".ToLang() %>'
                        },
                        uri: {
                            message: '<%="链接地址格式不正确".ToLang() %>'
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
            function (el) {
            });

        $("[name=_dialog] .btn-primary", parent.document)
            .click(function () {
                $('#formEdit').bootstrapValidator('validate');
                if ($('#formEdit').data('bootstrapValidator').isValid()) {
                    var json = "[{";
                    json += "linkText : \"" + $("#LinkText").val() + "\",";
                    json += "linkUrl : \"" + $("#LinkUrl").val() + "\"";
                    json += "}]";
                    window.parent.addRelation(json);
                    return false;
                }
            });
    </script>
</asp:Content>
