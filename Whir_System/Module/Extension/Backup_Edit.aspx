<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Backup_Edit.aspx.cs" Inherits="Whir_System_Module_Extension_Backup_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            var isShow = '<%=IsShow %>';
            if (isShow == "true") {
                $("#divInfo").text('<%=Msg %>');
                $("#divShow").css("display", "");
                $("#divMain").css("display", "none");
            }
            else {
                $("#divShow").css("display", "none");
                $("#divMain").css("display", "");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
             
            <div class="panel-body">
                  <ul class="nav nav-tabs">
                    <li ><a  href="backup.aspx" aria-expanded="true"><%="数据库备份".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%="新建备份".ToLang()%></a></li>
                  </ul>
                <br />
 


                <div class="note_yellow" id="divInfo">
                </div>
                <div class="note_yellow" id="divMySQL" style="display: none">
                    <%=@"温馨提示：当前使用的数据库为MySQL,就必须提供服务器中安装MySQL的bin目录的路径，否则就默认为：C:\Program Files\MySQL\MySQL Server 5.0\bin 。".ToLang() %>
                </div>
                <div class="note_yellow" id="divOracle">
                    <%="温馨提示：当前使用的数据库是Oracle，请到服务器中进行备份。".ToLang() %>
                </div>
                <div class="form_center" id="divMain">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Extension/Backup.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Remark">
                            <%="备注：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <textarea id="Remark" name="Remark" style="width: 100%;" rows="4" minlength="2" maxlength="100" required="true"></textarea>
                        </div>
                    </div>
                    <div class="form-group" id="trMySQL" style="display: none">
                        <div class="col-md-2 control-label" for="MySQLBinPath">
                            <%="MySQL安装的bin目录：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="MySQLBinPath" style="width: 100%;" name="MySQLBinPath" value="" class="form-control"
                                minlength="2" maxlength="1000" data-toggle="tooltip" data-placement="top" title="<%="格式：".ToLang()%>C:\Program Files\MySQL\MySQL Server 5.0\bin" />
                        </div>
                    </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <%if (IsCurrentRoleMenuRes("333"))
                                        { %>
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <% if (PageMode == Whir.Service.EnumPageMode.Insert)
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    <% } %>
                                    <%} %>
                                </div>
                                <a class="btn btn-white" href="Backup.aspx"><%="返回".ToLang()%></a>
                            </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('#formEdit').Validator(null,
             function (el) {
                 var actionSuccess = el.attr("form-success");
                 var $form = $("#formEdit");
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "Backup.aspx");
                         } else {
                             whir.toastr.error(response.Message);
                         }
                         whir.loading.remove();
                     },
                     error: function (response) {
                         whir.toastr.error(response.Message);
                         whir.loading.remove();
                     }
                 });
                 return false;
             });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#trMySQL").css("display", "<%=IsMySql %>");
            $("#divMySQL").css("display", "<%=IsMySql %>");

            $("#divOracle").css("display", "<%=IsOracle %>");

            $("#tdRemark").width(parseFloat("<%=DisplayWidth %>"));

        });
    </script>
</asp:Content>
