<%@ Page Language="C#" AutoEventWireup="true" CodeFile="columnmove.aspx.cs" Inherits="whir_system_module_subject_columnmove"
    MasterPageFile="~/whir_system/master/SystemMasterPage.master" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>Res/assets/js/tree/bootstrap-treeview.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="栏目移动".ToLang()%></div>
            <div class="panel-body">
                 
                <div class="panel panel-default panels col-md-4" style="width: 49%; margin-right: 5px;
                    padding-left: 0px; padding-right: 0px">
                    <div class="panel-heading">
                        <%="源栏目".ToLang()%></div>
                    <div class="panel-body">
                        <div id="treeview-origin" class="">
                        </div>
                    </div>
                </div>
                <div class="panel panel-default panels col-md-4" style="width: 50%; padding-right: 0px;
                    padding-left: 0px">
                    <div class="panel-heading">
                        <%="目标栏目".ToLang()%></div>
                    <div class="panel-body">
                        <div id="treeview-tager" class="">
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-group" style="width: 100%">
                    <div class="col-md-2 text-right">
                        <%="移动方式：".ToLang()%></div>
                    <div class="col-md-10 ">
                        <ul class="list">
                            <li>
                                <input type="radio" id="ColumnMove_Child" checked="checked" name="ColumnMove" value="Child" />
                                <label for="ColumnMove_Child">
                                    <%="移动为目标栏目的子栏目".ToLang()%></label>
                            </li>
                            <li>
                                <input type="radio" id="ColumnMove_Before" name="ColumnMove" value="Before" />
                                <label for="ColumnMove_Before">
                                    <%="移动到目标栏目之前".ToLang()%></label>
                            </li>
                            <li>
                                <input type="radio" id="ColumnMove_After" name="ColumnMove" value="After" />
                                <label for="ColumnMove_After">
                                    <%="移动到目标栏目之后".ToLang()%></label>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="form-group" style="width: 100%">
                    <div class="col-md-offset-2 col-md-6 ">
                        <input type="hidden" name="Origin" id="Origin" value="" />
                        <input type="hidden" name="Target" id="Target" value="" />
                        <button form-action="submit" form-success="refresh" class="btn btn-info btn-block">
                            <%="提交".ToLang()%></button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
         var zNodes=<%=BindTree() %>;
         //初始化控件
                 var origin = $('#treeview-origin').treeview1({
                     data: zNodes,
                     showIcon: false,
                     showCheckbox: false,
                     multiSelect:true,
                     onNodeSelected: function (event, node) {
                         $("#Origin").val($("#Origin").val()+node.columnid + ',');
                     },
                     onNodeUnselected: function (event, node) {
                          $("#Origin").val($("#Origin").val().replace((node.columnid + ','), ""));
                     }
                 });
                  var tager = $('#treeview-tager').treeview1({
                     data: zNodes,
                     showIcon: false,
                     showCheckbox: false,
                     multiSelect:false,
                     onNodeSelected: function (event, node) {
                          $("#Target").val($("#Target").val()+node.columnid + ',');
                     },
                     onNodeUnselected: function (event, node) {
                        $("#Target").val($("#Target").val().replace((node.columnid + ','), ""));
                     }
                 });

         $('[form-action="submit"]')
             .click(function() {
                 if ($("#Origin").val() == "") {
                     whir.toastr.error("<%="请选择源栏目".ToLang() %>");
                     return false;
                 } else if ($("#Target").val() == "") {
                     whir.toastr.error("<%="请选择目标栏目".ToLang() %>");
                     return false;
                 } else if ($("[name=ColumnMove]:checked").val()=="") {
                     whir.toastr.error("<%="请选择移动类型".ToLang() %>");
                     return false;
                 } else {
                     whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=ColumnMove",
                         {
                             data: {
                                 Origin: $("#Origin").val(),
                                 Target: $("#Target").val(),
                                 MoveType: $("[name=ColumnMove]:checked").val(),
                                 SubjectClassId:<%=SubjectClassId %>
                             },
                             success: function(response) {
                                 whir.loading.remove();
                                 if (response.Status == true) {
                                     whir.toastr.success(response.Message, true, false);        
                                 } else {
                                     whir.toastr.error(response.Message);
                                 }
                             }
                         }
                     );
                     return false;
                 };
             });
    </script>
</asp:Content>
