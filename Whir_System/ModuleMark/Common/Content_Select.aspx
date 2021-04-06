<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Content_Select.aspx.cs" Inherits="Whir_System_ModuleMark_Common_Content_Select" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function() {
            $("[name=_dialog] .btn-primary", parent.document)
                .click(function () {
                    var isSelect = whir.checkbox.isSelect('btSelectItem');
                    if (!isSelect) {
                        window.parent.TipMessage('<%="请选择".ToLang()%>');
                        return false;
                    }  

                    var json = "[";
                    $('input[name$=btSelectItem]')
                        .each(function () {
                            if ($(this).prop('checked')) {
                                json += "{";
                                json += "columnID : \"<%=ColumnId%>\",";
                                json += "columnName : \"\",";
                                json += "pID : \"" + $(this).val() + "\",";
                                json += "title : \"" + $("#hidTitle" + $(this).val()).val() + "\"";
                                json += "},";
                            }
                        });
                    json = json.substring(0, json.length - 1);
                    json += "]";
                    window.parent.addRelation(json);
                    return false;;
                });
        });

     

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="panel-body">
          <div class="panel">
                 <whir:ContentManager id="contentManager1" runat="server"  ></whir:ContentManager>
        </div>
    </div>
     
</asp:Content>
