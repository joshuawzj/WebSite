<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SubmitForm_Template.aspx.cs" Inherits="Whir_System_Module_Developer_SubmitForm_Template" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
    <script type="text/javascript">
        var isPreview = 0;
        var columnid = "";
        var clip = null;

        function preview() {
            if (isPreview == 1) {
                whir.dialog.frame('<%="预览".ToLang() %>', "<%=AppName%>Whir_System/module/developer/submitform_preview.aspx?istemplatepreview=1&columnid=" + columnid + "&time=" + new Date(), "", 800, 600, false);
            }
        }

        function setPreviewEnable() {
            isPreview = 1;
            var content = $('#<%=InvokeCode.ClientID%>').val();
            if ($.trim(content) == "") {
                TipMessage('<%="无数据，不可预览".ToLang() %>');
                return false;
            } else {
                preview();
            }
        }

        function getContent() {
            return $('#<%=InvokeCode.ClientID%>').val();
        }

        ///选择目标栏目
        function selectColumn() {
            whir.dialog.frame('<%="选择目标栏目".ToLang()%>', "<%=AppName%>Whir_System/ajax/developer/column_select.aspx?selectedtype=radiobox&callback=selectColumnBack", "", 800, 600);
        }
        //返回选择栏目信息
        function selectColumnBack(json) {
            json = eval('(' + json + ')');

            if (json.length > 0) {
                $("#hidColumnId").val(json[0].id.split('|')[0]);
                $("#Column").val(json[0].name);
                whir.ajax.post("<%=SysPath%>Handler/Developer/SubmitForm.aspx", {
                    data: {
                        _action: "GetColumns",
                        columnid: $("#hidColumnId").val()
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            var list = eval('(' + response.Message + ')');
                            $("#ChildColumn").empty();
                            if (list.length > 0) {
                                var temp = "";
                                for (var i = 0; i < list.length; i++) {
                                    temp += "<option value=\"" + list[i].Value + "\">" + list[i].Text + "</option>";
                                }
                                $("#ChildColumn").append(temp);
                            }
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.dialog.remove();
                    }
                });
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="SubmitFormList.aspx" aria-expanded="true"><%="提交表单生成器".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true">自定义模板</a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/SubmitForm.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Name">
                                <%= "表单名称：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Name">
                                <%= "调用标签：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <span id="spanRemark" runat="server"></span>
                                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Name">
                                <%= "数据提交到栏目：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <div class="input-group ">
                                    <input type="text" id="Column" value="<%= Column %>" name="Column" readonly="readonly"
                                        class="form-control" maxlength="20" />
                                    <span class="input-group-addon pointer" for="Column" onclick="selectColumn()">选择</span>
                                </div>
                                <input type="hidden" id="hidColumnId" value="<%= CurrentSubmitForm.ColumnId %>" name="hidColumnId" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ChildColumn">
                            </div>
                            <div class="col-md-10 ">
                                <select id="ChildColumn" name="ChildColumn" class="form-control" required="true">
                                    <% if (CurrentSubmitForm.ColumnId <= 0)
                                       {
                                           foreach (var item in ChildColumn)
                                           { %>
                                    <option value="<%= item.Value.ToInt()==-1?"":item.Value %>">
                                        <%= item.Text %></option>
                                    <% }
                                       }
                                       else
                                       {
                                           foreach (var item in Columnlist)
                                           { %>
                                    <option value="<%= item.ColumnId.ToInt()==-1?"":item.ColumnId.ToStr() %>">
                                        <%= item.ColumnName %></option>
                                    <% }
                                       }
                                    %>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Name">
                                <%="示例代码：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea class="form-control" style="height: 280px" id="DemoCode"></textarea>

                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Name">
                                <%="调用代码：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea class="form-control" id="InvokeCode" style="height: 280px" runat="server"></textarea>
                                <input type="hidden"  name="InvokeCode"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="SubmitId" value="<%=SubmitId %>" />
                                <input type="hidden" name="_action" value="SaveCode" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交".ToLang()%></button>
                                    <a id="btnPreview" class="btn btn-info" onclick="return setPreviewEnable();"><%="预览".ToLang() %></a>
                                </div>
                                <a class="btn btn-white" href="<%=BackUrl %>"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("#ChildColumn").val("<%=CurrentSubmitForm.ColumnId %>");
        $("#ChildColumn").change(function () {
            GetDemoCode();
        });

        function GetDemoCode() {
            whir.ajax.post("<%=SysPath%>Handler/Developer/SubmitForm.aspx", {
                data: {
                    _action: "GetDemoCode",
                    columnid: $("#ChildColumn").val(),
                    submitId: '<%=SubmitId %>'
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        $("#DemoCode").val(response.Message);
                    } else {
                        whir.toastr.error(response.Message);
                    }
                    whir.dialog.remove();
                }
            });
            return false;
        }

        GetDemoCode();
 
        var options = {
            fields: {
                 
            }
        };


        //提交内容
        $('#formEdit').Validator(options,
             function (el) {
                 $("input[name='InvokeCode']").val($('#<%=InvokeCode.ClientID%>').val());
                 var actionSuccess = el.attr("form-success");

                 var $form = $("#formEdit");
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "<%=BackUrl %>");

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
</asp:Content>
