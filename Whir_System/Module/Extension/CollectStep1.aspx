<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectStep1.aspx.cs" Inherits="Whir_System_Module_Extension_CollectStep1" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h3 {
            text-align: center;
            font-size: 12px;
        }
        h3 span {
            margin: 0px 10px 0px 10px;
        }
        .note_green {
            color: green;
        }
        .pager {
            cursor: pointer;
            color: green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading" align='center'>
                <span class="text-danger"><b><%="第一步：基本设置".ToLang()%></b></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
                <span><%="第二步：列表页规则设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
                <span><%="第三步：内容页规则设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;<span><%="完成".ToLang()%></span>
            </div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/Collect.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ItemName">
                            <%="项目名称：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="ItemName" name="ItemName" class="form-control" required="true"
                                maxlength="512" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ColumnId">
                            <%="入库栏目：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <select id="ColumnId" name="ColumnId" required="true" class="form-control" data-toggle="tooltip"
                                data-placement="top" title="<%="设置采集信息后，导入到站点的栏目".ToLang()%>">
                                <option value="0"><%="==请选择==".ToLang()%></option>
                                <% foreach (var item in Columns)
                                   {
                                       Response.Write("<option value=\""+item.ColumnId+"\">"+item.ColumnName+"</option>");
                                   } %>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="WebName">
                            <%="采集网站：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="WebName" name="WebName" class="form-control" required="true"
                                maxlength="64" data-toggle="tooltip" data-placement="top" title="<%="设置被采集网站的名称".ToLang()%>" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="WebUrl">
                            <%="列表页URL：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="WebUrl" value="http://" name="WebUrl" class="form-control"
                                required="true" maxlength="64" data-toggle="tooltip" data-placement="top" title="<%="设置采集的网址(必须为不带任何页面名称的域名,如http://www.baidu.com/)".ToLang()%>" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="PageRules">
                            <%="分页规则：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="PageRules" value="http://" name="PageRules" class="form-control"
                                required="true" maxlength="256" data-toggle="tooltip" data-placement="top" title="<%="设置采集的网址分页规则,如：".ToLang()%>http://ezeip.gzwhir.com/xwzx/list_575.aspx?page={$page}" />
                            <span class="pager">{$page}</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="PageNum">
                            <%="列表页数：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" onkeyup='this.value=this.value.replace(/[^1-9]/D*$/,"")' ondragenter="return false" onpaste="return !clipboardData.getData('text').match(//D/)""  id="PageNum" value="" name="PageNum" class="form-control"
                                required="true" maxlength="8" data-toggle="tooltip" data-placement="top"  />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 text-right" for="IsOrderDesc">
                            <%="采集顺序：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <ul class="list" id="Ul1">
                                <li>
                                    <input type="radio" id="IsOrderDesc_True" checked="checked" name="IsOrderDesc" value="0" />
                                    <label for="IsOrderDesc_True"><%="顺序".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsOrderDesc_False" name="IsOrderDesc" value="1" />
                                    <label for="IsOrderDesc_False"><%="倒序".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 text-right" for="IsDownloadImages">
                            <%="下载图片到本地：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <ul class="list" id="Ul2">
                                <li>
                                    <input type="radio" id="IsDownloadImages_True" checked="checked" name="IsDownloadImages" value="0" />
                                    <label for="IsDownloadImages_True"><%="不保存".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsDownloadImages_False" name="IsDownloadImages" value="1" />
                                    <label for="IsDownloadImages_False"><%="保存".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Remark">
                            <%="项目说明：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <textarea id="Remark" name="Remark" maxlength="1024" data-toggle="tooltip" class="form-control"
                                data-placement="top" title="<%="设置采集任务的说明".ToLang()%>" style=" height: 300px;"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="_action" value="Save" />
                            <input type="hidden" name="CollectId" value="" />
                            <button form-action="submit" form-success="refresh" class="btn btn-info"><%="下一步".ToLang()%></button>
                            <a class="btn text-danger border-danger" href="CollectList.aspx"><%="返回采集管理".ToLang()%></a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        //绑定值
        var IsOrderDesc = "<%=CurrentCollect.IsOrderDesc%>";
        if (IsOrderDesc == "True") {
            IsOrderDesc = "1";
        } else {
            IsOrderDesc = "0";
        }
        $("[name='IsOrderDesc'][value='" + IsOrderDesc + "']").prop("checked", "checked");
        var IsDownloadImages = "<%=CurrentCollect.IsDownloadImages%>";
        if (IsDownloadImages == "True") {
            IsDownloadImages = "1";
        } else {
            IsDownloadImages = "0";
        }
        $("[name='IsDownloadImages'][value='" + IsDownloadImages + "']").prop("checked", "checked");

        $("[name='ItemName']").val("<%=CurrentCollect.ItemName%>");
        $("[name='CollectId']").val("<%=CurrentCollect.CollectId%>");
        $("[name='ColumnId']").val("<%=CurrentCollect.ColumnId%>");
        $("[name='WebName']").val("<%=CurrentCollect.WebName%>");
        $("[name='WebUrl']").val("<%=CurrentCollect.WebUrl%>");
        $("[name='PageRules']").val("<%=CurrentCollect.PageRules%>");
        $("[name='PageNum']").val("<%=CurrentCollect.PageNum%>");
        $("[name='Remark']").val("<%=CurrentCollect.Remark%>");

        //下一步
        var options = {
            fields: {
                WebUrl: {
                    validators: {
                        uri: {
                            message: '<%="列表页URL格式不正确".ToLang() %>'
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
         function () {
             var actionSuccess = $(this).attr("form-success");
             var $form = $("#formEdit");
             subMit($form, actionSuccess);
         });

        function subMit(form, actionSuccess) {
            form.post({
                success: function (response) {
                    whir.loading.remove();
                    location.href = response.Message;
                },
                error: function (response) {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            });
            return false;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $(".pager").click(function () {
                $("#PageRules").val($("#PageRules").val() + "{$page}");
            });

        })
    </script>
</asp:Content>
