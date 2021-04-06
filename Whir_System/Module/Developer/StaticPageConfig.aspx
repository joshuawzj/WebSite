<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="StaticPageConfig.aspx.cs" Inherits="Whir_System_Module_Developer_StaticPageConfig" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>Res/assets/js/tree/bootstrap-treeview.js" type="text/javascript"></script>
    <script type="text/javascript">

        var parameterId = 0;
        var type = 0;
        $(document).ready(function () {

            //var tempHtml = $("#temp").clone().html();
            //tempHtml = tempHtml.replace(/{Id}/g, parameterId);
            //$("#columnTab").before(tempHtml);

            //内容管理 0
            $.get("<%=SysPath %>ajax/content/column_generats.aspx?siteid=<%=CurrentSiteId%>&subjecttypeid=0&time=" + new Date().getMilliseconds(), "", function (data) {

                defaultData = eval(data);

                //初始化控件
                var $checkableTree = $('#treeview-checkable0').treeview1({
                    data: defaultData,
                    showIcon: false,
                    showCheckbox: false,
                    onNodeSelected: function (event, node) {
                        type = 0;
                        if (node.nodes != null) {
                            $("input[name='ColumnName']").val("");
                            $("input[name='ColumnId']").val(0);
                            return false;
                        }
                        getParameter(node.columnid);
                        $("input[name='ColumnName']").val(node.text);
                        $("input[name='ColumnId']").val(node.columnid);
                    }
                });
                //全部展开
                $('#btn-expand-all0').on('click', function (e) {
                    var levels = 99;//展开99层菜单，已经非常大了
                    $checkableTree.treeview1('expandAll', { levels: levels, silent: $('#chk-expand-silent').is(':checked') });
                });

                //全部折叠
                $('#btn-collapse-all0').on('click', function (e) {
                    $checkableTree.treeview1('collapseAll', { silent: $('#chk-expand-silent').is(':checked') });
                });
            });
            //子站 1
            $.get("<%=SysPath %>ajax/content/column_generats.aspx?siteid=<%=CurrentSiteId%>&subjecttypeid=1&time=" + new Date().getMilliseconds(), "", function (data) {

                defaultData = eval(data);

                //初始化控件
                var $checkableTree = $('#treeview-checkable1').treeview1({
                    data: defaultData,
                    showIcon: false,
                    showCheckbox: false,
                    onNodeSelected: function (event, node) {
                        type = 1;
                        if (node.nodes != null) {
                            $("input[name='SubSiteName']").val("");
                            $("input[name='SubSiteId']").val(0);
                            return false;
                        }
                        $("input[name='SubSiteName']").val(node.text);
                        $("input[name='SubSiteId']").val(node.columnid);
                    }
                });
                //全部展开
                $('#btn-expand-all1').on('click', function (e) {
                    var levels = 99;//展开99层菜单，已经非常大了
                    $checkableTree.treeview1('expandAll', { levels: levels, silent: $('#chk-expand-silent').is(':checked') });
                });

                //全部折叠
                $('#btn-collapse-all1').on('click', function (e) {
                    $checkableTree.treeview1('collapseAll', { silent: $('#chk-expand-silent').is(':checked') });
                });
            });
            //专题 2
            $.get("<%=SysPath %>ajax/content/column_generats.aspx?siteid=<%=CurrentSiteId%>&subjecttypeid=2&time=" + new Date().getMilliseconds(), "", function (data) {

                defaultData = eval(data);

                //初始化控件
                var $checkableTree = $('#treeview-checkable2').treeview1({
                    data: defaultData,
                    showIcon: false,
                    showCheckbox: false,
                    onNodeSelected: function (event, node) {
                        type = 2;
                        if (node.nodes != null) {
                            $("input[name='SubjectName']").val("");
                            $("input[name='SubjectId']").val(0);
                            return false;
                        }
                        $("input[name='SubjectName']").val(node.text);
                        $("input[name='SubjectId']").val(node.columnid);

                    }
                });
                //全部展开
                $('#btn-expand-all2').on('click', function (e) {
                    var levels = 99;//展开99层菜单，已经非常大了
                    $checkableTree.treeview1('expandAll', { levels: levels, silent: $('#chk-expand-silent').is(':checked') });
                });

                //全部折叠
                $('#btn-collapse-all2').on('click', function (e) {
                    $checkableTree.treeview1('collapseAll', { silent: $('#chk-expand-silent').is(':checked') });
                });
            });
        });

        //添加参数
        function addParameter(type) {
            switch (type) {
                case 1:
                    if ($("input[name='SubSiteId']").val() == 0) {
                        whir.toastr.error("<%="请选择栏目".ToLang() %>");
                        break;
                    }
                    var tempHtml = $("#temp").clone().html();
                    tempHtml = tempHtml.replace(/{Id}/g, parameterId++);
                    $("#subSiteTab").before(tempHtml);
                    break;
                case 2:
                    if ($("input[name='SubjectId']").val() == 0) {
                        whir.toastr.error("<%="请选择栏目".ToLang() %>");
                        break;
                    }
                    var tempHtml = $("#temp").clone().html();
                    tempHtml = tempHtml.replace(/{Id}/g, parameterId++);
                    $("#subjectTab").before(tempHtml);
                    break;
                case 0:
                default:
                    if ($("input[name='ColumnId']").val() == 0) {
                        whir.toastr.error("<%="请选择栏目".ToLang() %>");
                        break;
                    }

                    var tempHtml = $("#temp").clone().html();
                    tempHtml = tempHtml.replace(/{Id}/g, parameterId++);
                    $("#columnTab").before(tempHtml);
                    break;
            }
            whir.skin.radio();        }

        //删除参数 //0-内容，1-子站，2-专题
        function delParameter(type) {
            if (parameterId < 0) {
                return false;
            }
            $("#parameter" + --parameterId).remove();

        }

        function getParameter(columnId) {
            $("div[id^='parameter']").remove();
            whir.ajax.post('<%= SysPath%>Handler/Module/Release/Release.aspx', {
                data: {
                    _action: "GetColumnParameter",
                    ColumnId: columnId
                },
                success: function (result) {
                    if (result.Status) {
                        var name = "";
                        switch (type) {
                            case 1:
                                name = "formSubSite";
                                break;
                            case 2:
                                name = "formSubject";
                                break;
                            case 0:
                            default:
                                name = "formColumn";
                                break;
                        }
                        var $form = $('#'+name);
                        var data = eval(result.Message);
                        parameterId = 0;
                        for (var i = 0; i < data.length; i++) {
                            addParameter(type);
                            if(data[i].IsPage)
                                $form.find(":radio[value='1']").iCheck("check");
                            else
                                $form.find(":radio[value='0']").iCheck("check");
                            $("input[name='PageSize']").val(data[i].PageSize);
                            $("#ParameterName"+i).val(data[i].ParameterName);
                            $("#ParameterType"+i).val(data[i].ParameterType);
                            $("#ParameterSource" + i).val(data[i].ParameterSource);
                            $("input[name='ParameterBind'][parameterId='"+i+"']").hide();
                            $("input[name='ParameterBind'][parameterId='"+i+"'][for='bind" + data[i].ParameterSource + "']").val(data[i].ParameterBind).show();
                             
                        }
                    } else {
                        whir.toastr.warning(result.Message);
                    }
                    whir.loading.remove();
                }

            });

        }

        function saveParameter(type) {
            var name = "";
            switch (type) {
                case 1:
                    name = "formSubSite";
                    break;
                case 2:
                    name = "formSubject";
                    break;
                case 0:
                default:
                    name = "formColumn";
                    break;
            }
            var $form = $('#'+name);
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        whir.toastr.success(response.Message);
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

        }

        function changBind(obj,id) {
            var name = "";
            switch (type) {
                case 1:
                    name = "formSubSite";
                    break;
                case 2:
                    name = "formSubject";
                    break;
                case 0:
                default:
                    name = "formColumn";
                    break;
            }
            var $form = $('#'+name);
            
            $form.find("input[name='ParameterBind'][parameterId='"+id+"']").hide();
            $form.find("input[name='ParameterBind'][parameterId='"+id+"'][for='bind" + $(obj).val()+ "']").show();
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>

        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a data-toggle="tab" href="#list0">内容管理</a></li>
                    <li role="presentation"><a data-toggle="tab" href="#list1">子站</a></li>
                    <li role="presentation"><a data-toggle="tab" href="#list2">专题</a></li>
                </ul>
                <div class="tab-content">
                    <div id="list0" class="tab-pane active">
                        <div class="space15"></div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        <a id="btn-expand-all0" class="aLink" href="javascript:;">
                                            <b>全部展开</b> </a>
                                        <a id="btn-collapse-all0" class="aLink" href="javascript:;">
                                            <b>全部折叠</b> </a>

                                    </div>
                                    <div id="treeview-checkable0" class="">
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        静态发布配置
                                    </div>
                                    <div class="panel-body">
                                        <form id="formColumn" class=" form-horizontal" form-url="<%= SysPath%>Handler/Module/Release/Release.aspx">
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Column">
                                                    当前编辑栏目：
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" name="ColumnName" value="" readonly="readonly" class="form-control" />
                                                    <input type="hidden" name="ColumnId" value="0" class="form-control" />
                                                </div>
                                            </div>
                                              
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="IsPage">
                                                    是否分页：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <ul class="list">
                                                        <li>
                                                            <input type="radio" name="IsPage" checked="checked" data-toggle="tooltip" value="1"
                                                                class="form-control" />
                                                            <label>是</label></li>
                                                        <li>
                                                            <input type="radio" name="IsPage" data-toggle="tooltip" value="0"
                                                                class="form-control" />
                                                            <label>否</label></li>
                                                    </ul>
                                                </div>
                                                <div class="col-md-2 control-label" for="PageSize">
                                                    分页大小：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <input type="number" name="PageSize" value="10" data-toggle="tooltip" title="每页显示的记录数，默认10条"
                                                        class="form-control" maxlength="12" />
                                                </div>
                                            </div>

                                            <div class="form-group" id="columnTab">
                                                <div class="col-md-offset-2 col-md-10 ">
                                                    <input type="hidden" name="_action" value="SaveParameter" />
                                                    <input type="hidden" name="Type" value="0" />
                                                    <button type="button" onclick="saveParameter(0);" class="btn btn-info">保存</button>
                                                    <div class="btn-group ">
                                                        <button type="button" class="btn btn-white" onclick="addParameter(0);">
                                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;添加参数
                                                        </button>
                                                        <button type="button" class="btn text-danger border-normal" onclick="delParameter(0);">
                                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;删除参数
                                                        </button>
                                                    </div>

                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="list1" class="tab-pane ">
                        <div class="space15"></div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        <a id="btn-expand-all1" class="aLink" href="javascript:;">
                                            <b>全部展开</b> </a>
                                        <a id="btn-collapse-all1" class="aLink" href="javascript:;">
                                            <b>全部折叠</b> </a>

                                    </div>
                                    <div id="treeview-checkable1" class="">
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        静态发布配置
                                    </div>
                                    <div class="panel-body">
                                        <form class=" form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="SubSite">
                                                    当前编辑栏目：
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" name="SubSiteName" value="" readonly="readonly" class="form-control" />
                                                    <input type="hidden" name="SubSiteId" value="0" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="IsPage">
                                                    是否分页：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <ul class="list">
                                                        <li>
                                                            <input type="radio" name="IsPage" checked="checked" data-toggle="tooltip" value="1"
                                                                class="form-control" />
                                                            <label>是</label></li>
                                                        <li>
                                                            <input type="radio" name="IsPage" data-toggle="tooltip" value="0"
                                                                class="form-control" />
                                                            <label>否</label></li>
                                                    </ul>
                                                </div>
                                                <div class="col-md-2 control-label" for="PageSize">
                                                    分页大小：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <input type="number" name="PageSize" value="10" data-toggle="tooltip" title="每页显示的记录数，默认10条"
                                                        class="form-control" maxlength="12" />

                                                </div>
                                            </div>
                                            <div class="form-group" id="subSiteTab">
                                                <div class="col-md-offset-2 col-md-10 ">
                                                    <input type="hidden" name="_action" value="SaveSubSite" />
                                                    <button class="btn btn-info">保存</button>
                                                    <div class="btn-group ">
                                                        <button class="btn btn-white" onclick="addParameter(1);">
                                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;添加参数
                                                        </button>
                                                        <button class="btn text-danger border-normal" onclick="delParameter(1);">
                                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;删除参数
                                                        </button>
                                                    </div>

                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="list2" class="tab-pane ">
                        <div class="space15"></div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        <a id="btn-expand-all2" class="aLink" href="javascript:;">
                                            <b>全部展开</b> </a>
                                        <a id="btn-collapse-all2" class="aLink" href="javascript:;">
                                            <b>全部折叠</b> </a>

                                    </div>
                                    <div id="treeview-checkable2" class="">
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="panel panel-default " style="height: 100%">
                                    <div class="panel-heading">
                                        静态发布配置
                                    </div>
                                    <div class="panel-body">
                                        <form class=" form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Subject">
                                                    当前编辑栏目：
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" name="SubjectName" value="" readonly="readonly" class="form-control" />
                                                    <input type="hidden" name="SubjectId" value="0" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="IsPage">
                                                    是否分页：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <ul class="list">
                                                        <li>
                                                            <input type="radio" name="IsPage" checked="checked" data-toggle="tooltip" value="1"
                                                                class="form-control" />
                                                            <label>是</label></li>
                                                        <li>
                                                            <input type="radio" name="IsPage" data-toggle="tooltip" value="0"
                                                                class="form-control" />
                                                            <label>否</label></li>
                                                    </ul>
                                                </div>
                                                <div class="col-md-2 control-label" for="PageSize">
                                                    分页大小：
                                                </div>
                                                <div class="col-md-4 ">
                                                    <input type="number" name="PageSize" value="10" data-toggle="tooltip" title="每页显示的记录数，默认10条"
                                                        class="form-control" maxlength="12" />

                                                </div>
                                            </div>
                                            <div class="form-group" id="subjectTab">
                                                <div class="col-md-offset-2 col-md-10 ">
                                                    <input type="hidden" name="_action" value="SaveSubject" />
                                                    <button class="btn btn-info">保存</button>
                                                    <div class="btn-group ">
                                                        <button class="btn btn-white" onclick="addParameter(2);">
                                                            <span class="glyphicon glyphicon-plus"></span>&nbsp;添加参数
                                                        </button>
                                                        <button class="btn text-danger border-normal" onclick="delParameter(2);">
                                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;删除参数
                                                        </button>
                                                    </div>

                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script id="temp" style="display: none">
        <div id="parameter{Id}">
            <div class="form-group" id="ParameterName">
                <div class="col-md-2 control-label" for="Parameter" style="color:#428bca;">
                    选择模板{Id}：
                </div>
                <div class="col-md-10 ">
                     <ul class="list">
                            <li>
                                <input type="radio" name="Temp{Id}" checked="checked" data-toggle="tooltip" value="1"
                                    class="form-control" />
                                <label>栏目首页</label></li>
                            <li>
                                <input type="radio" name="Temp{Id}" data-toggle="tooltip" value="2"
                                    class="form-control" />
                                <label>列表页</label></li>
                            <li>
                                <input type="radio" name="Temp{Id}" data-toggle="tooltip" value="3"
                                    class="form-control" />
                                <label>详细页</label></li>
                        </ul>
                </div>
            </div>
            <div class="form-group" id="ParameterName">
                <div class="col-md-2 control-label" for="Parameter" >
                    参数名称{Id}：
                </div>
                <div class="col-md-10 ">
                    <input type="text" id="ParameterName{Id}" name="ParameterName" value="" class="form-control" maxlength="128" />

                </div>
            </div>
            <div class="form-group" id="ParameterType">
                <div class="col-md-2 control-label" for="ParameterType">
                    参数类型{Id}：
                </div>
                <div class="col-md-10 ">
                    <select id="ParameterType{Id}" name="ParameterType" class="form-control">
                        <option value="1">字符串</option>
                        <option value="2">数字</option>
                        <option value="3">货币</option>
                        <option value="4">日期和时间</option>
                        <option value="5">是/否</option>
                    </select>
                </div>
            </div>
            <div class="form-group" id="ParameterSource">
                <div class="col-md-2 control-label" for="ParameterSource">
                    参数来源{Id}：
                </div>
                <div class="col-md-10 ">
                    <select id="ParameterSource{Id}" name="ParameterSource" class="form-control" onchange="changBind(this,{Id});">
                        <option value="1">自定义</option>
                        <option value="2">指定栏目类别</option>
                        <option value="3">绑定Sql</option>
                        <option value="4">数值范围</option>
                        <option value="5">日期和时间范围</option>
                    </select>
                </div>
            </div>
            <div class="form-group" id="ParameterBind">
                <div class="col-md-2 control-label" for="ParameterBind">
                    数据绑定{Id}：
                </div>
                <div class="col-md-10 ">
                    <input type="text" name="ParameterBind" parameterId="{Id}" class="form-control" for="bind1" data-toggle="tooltip" title="示例：男|女" />
                    <input type="text" name="ParameterBind" parameterId="{Id}" class="form-control" style="display: none" for="bind2" data-toggle="tooltip" title="默认是当前栏目Id，可以输入其他栏目" />
                    <input type="text" name="ParameterBind" parameterId="{Id}" class="form-control" style="display: none" for="bind3" data-toggle="tooltip" title="示例：Select Whir_U_Content_PId From Whir_U_Content" />
                    <input type="text" name="ParameterBind" parameterId="{Id}" class="form-control" style="display: none" for="bind4" data-toggle="tooltip" title="示例：-13|99" />
                    <input type="text" name="ParameterBind" parameterId="{Id}" class="form-control" style="display: none" for="bind5" data-toggle="tooltip" title="示例：2016-08-15 12:00:00|2017-08-15 12:00:00" />
                </div>
            </div>
        </div>
    </script>

</asp:content>

