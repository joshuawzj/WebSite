<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Option_Edit.aspx.cs" Inherits="Whir_System_Module_Column_Option_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $(function() {
        var options = {
            fields: {
                 
            }
        };
        $('#formSingle').Validator(options,
            function(el) {

            });

            $("[name=_dialog] .btn-primary", parent.document)
                .click(function() {
                $('#formSingle').bootstrapValidator('validate');
                    if ($('#formSingle').data('bootstrapValidator').isValid())
                        callback($("#OptionValue").val(),$("#OptionText").val());
                });

        });

        //回传给父页面
        function callback(option_value, option_text) { 
            if('<%= JsCallback %>' != ''){
                window.parent.<%= JsCallback %>(option_value, option_text);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <form id="formSingle"  class="form-horizontal">
                <div class="form_center">
                        <div class="form-group">
                            <div class="col-md-2 control-label" style="margin-bottom: 0px; " for="OptionValue">
                                <%="值：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="OptionValue" name="OptionValue" value="<%=OptionValue %>"
                                    class="form-control" required="true" maxlength="50" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" style="margin-bottom: 0px; " for="OptionText">
                                <%="文本：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <input type="text" id="OptionText" name="OptionText" value="<%=OptionText %>" class="form-control"
                                    required="true" maxlength="50" />
                            </div>
                        </div>
                    </div>
                    </form>
            </div>

</asp:Content>
