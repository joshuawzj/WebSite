<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DynamicForm.ascx.cs" Inherits="whir_system_UserControl_ContentControl_DynamicForm" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Framework" %>

<!--颜色控件-->
<link href="<%=SysPath%>res/assets/js/pickcolor/css/pick-a-color-1.2.3.min.css" rel="stylesheet" type="text/css" />
<script src="<%=SysPath%>res/assets/js/pickcolor/js/pick-a-color-1.2.3.min.js" type="text/javascript"></script>
<script src="<%=SysPath%>res/assets/js/pickcolor/js/tinycolor-0.9.15.min.js" type="text/javascript"></script>
<!--滑动控件-->
<link href="<%=SysPath%>res/assets/js/slider/css/bootstrap-slider.min.css" rel="stylesheet" type="text/css" />
<script src="<%=SysPath%>res/assets/js/slider/js/bootstrap-slider.js" type="text/javascript"></script>

<div class="content-wrap">
    <div class="space15">
    </div>
    <div class="panel">
        <input type="hidden" id="hidChoose" />
        <form id="formEdit" enctype="multipart/form-data" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Common.aspx">
            <div class="panel-body">
                <ul class="nav nav-tabs">

                    <li><a href="<%=BackPageUrl%>" aria-expanded="true"><%=ColumnName%></a></li>
                    <li class="active"><a data-toggle="tab" aria-expanded="true"><%=(ItemId ==0?"添加内容":"编辑内容").ToLang()%></a></li>
                    <%if (IsSinglePage && new Whir.ezEIP.Web.SysManagePageBase().IsRoleHaveColumnRes("历史记录")) {%>
                    <li><a aria-expanded="true" href="javascript:openBak()"><%="历史".ToLang()%></a></li>
                    <%}%>
                </ul>
                 <br />
                <div class="form_center" style="width: 100%">
                    <div class="panel panel-default panels col-sm-4" id="panelleft" style="width: 75%; margin-right: 5px; padding-left: 0px; padding-right: 0px">
                        <div class="panel-heading">
                            <%="表单内容".ToLang()%>&nbsp;
                            <%=SinglePageFlowStatusStr %>
                            <%if (Whir.ezEIP.Web.SysManagePageBase.IsDevUser) { %>
                            <button onclick="whir.label.dialog('<%="置标".ToLang()%>','<%=ColumnId %>','<%=SubjectId %>','<%=ItemId %>')" class="btn btn-white" style="float: right; margin-top: -5px;"><%="置标".ToLang()%></button>
                            <%} %>
                        </div>
                        <div class="panel-body">
                            <%=LeftHtml%>
                        </div>
                        <%if (IsShowBtn()) {%>
                        <div class="panel-footer">
                            <div class="col-lg-offset-2 col-lg-10">
                                <input type="hidden" name="TypeId" value="<%=ColumnId%>" />
                                <input type="hidden" name="ColumnId" value="<%=ColumnId%>" />
                                <input type="hidden" name="SubjectId" value="<%=SubjectId%>" />
                                <input type="hidden" name="ItemId" value="<%=ItemId%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <%if (PageMode == EnumPageMode.Insert) {%>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    <% }%>
                                </div>
                                <%if (IsSaveToFlow) {%>
                                <button type="submit" onclick="return saveToFlow();" form-success="back" commandname="SaveToFlow" class="btn btn-info"><%="转入审核流程".ToLang()%></button>
                                <% }%>
                                <%if (IsSinglePage && IsShowSinglePageFlowBtn && SinglePageFlowStatus != -1) {%>
                                <button type="submit" onclick="return audit();" form-success="back" commandname="Audit" class="btn btn-info"><%="审核".ToLang()%></button>
                                <%}%>
                                <%if (IsSinglePage) {%>
                                <button type="submit" onclick="return singlePageCopy();" form-success="refresh" commandname="SinglePageCopy" class="btn btn-info"><%="内容推送".ToLang()%></button>
                                <%}%>
                                <%if (IsShowShare()) {%>
                                <button onclick="whir.label.shareDialog('<%="分享".ToLang() %>', '<%=ColumnId %>','<%=SubjectId %>','<%=ItemId %>')" class="btn btn-info"><%="分享".ToLang()%></button>
                                <%} %>
                                <% if (!IsOpenFrame && ItemId == 0 && ColumnId != 1) {%>
                                <a id="tagTrigger" class="btn btn-info" data-toggle="tooltip" title="<%="选择栏目后,需要点击提交按钮保存".ToLang()%>" href="javascript:;"><%="同时发布到其它栏目/站点群".ToLang()%></a>
                                <%  }%>
                                <%if (Column != null && !Column.PreviewTemp.IsEmpty()) {%>
                                <button type="submit" onclick="return preview();" commandname="Preview" class="btn btn-white"><%="预览".ToLang()%></button>
                                <%}%>
                                <a class="btn btn-white" href="<%=BackPageUrl%>"><%="返回".ToLang()%></a>
                                <% if (!IsOpenFrame && ItemId == 0 && ColumnId != 1) {%>
                                <span id="spanColumn" class="select-columns"></span>
                                <input type="hidden" id="hidColumnID" name="hidColumnID" />
                                <script type="text/javascript" src='<%=SysPath%>res/js/Whir/whir.content.js'></script>
                                <script type="text/javascript">
                                    var openUrl = "<%=SysPath%>modulemark/common/column_select.aspx?columnid=<%=ColumnId%>&subjectid=<%= SubjectId%>&selectedtype=checkbox";
                                    whir.content.createToColumn('<%="选择栏目".ToLang()%>', openUrl, 'tagTrigger', 'hidColumnID', 'spanColumn');
                                </script>
                                <%} %>
                            </div>
                    </div>
                    <% }%>
                </div>
                <%if (!IsOpenFrame) {%>
                <div class="panel panel-default panels col-sm-4" style="width: 24%; padding-right: 0px; padding-left: 0px">
                    <div class="panel-heading">
                        <%="表单相关".ToLang()%>
                    </div>
                    <div class="panel-body">
                        <%=RightHtml%>
                        <% if (IsRelated) {%>
                        <div class="form-group no-margin ">
                            <div>
                                <%="相关文章".ToLang()%>
                                    <ul id="ulRelation"></ul>
                            </div>
                            <div>
                                <input type="hidden" id="hidRelationData" name="hidRelationData" />
                                <div class="btn-group">
                                    <a href="javascript:;" onclick="openRelationSelect();" class="btn btn-white"><%="添加相关".ToLang()%></a>
                                    <a href="javascript:;" onclick="openRelationLinkAdd();" class="btn btn-white"><%="添加链接".ToLang()%></a>
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            //打开添加相关文章的页面
                            function openRelationSelect() {
                                var exceptId = "";
                                var innerData = $("#hidRelationData").val().split(',');
                                if (innerData.length > 0) {
                                    $(innerData).each(function (idx, item) {
                                        exceptId += item.split('|')[0] + ","; //已经选择过的Id, 传入选择页面, 在页面中控制不能重复选择
                                    });
                                }
                                var url = "<%=SysPath%>ModuleMark/Common/Content_Select.aspx?columnid=<%=ColumnId%>&subjectid=<%= SubjectId%>&itemid=<%=ItemId%>&exceptId=" + exceptId;
                                whir.dialog.frame('<%="选择相关文章".ToLang()%>', url, "", 1024, 600);
                            }

                            //打开添加链接的页面
                            function openRelationLinkAdd() {
                                var url = "<%=SysPath%>ModuleMark/Common/Relation_Link.aspx?columnid=<%=ColumnId%>&subjectid=<%= SubjectId%>&itemid=<%=ItemId%>";
                                whir.dialog.frame('<%="添加相关链接".ToLang()%>', url, "", 500, 300);
                            }

                            //添加一个相关文章
                            function addRelation(json) {
                                json = eval("(" + json + ")");

                                var innerHtml = "";
                                var innerData = "";
                                $(json)
                                    .each(function (idx, item) {
                                        var now = new Date();
                                        var number =
                                            now.getHours() +
                                            now.getMinutes() +
                                            now.getSeconds() +
                                            now.getMilliseconds() +
                                            parseInt(Math.random() * 10000);

                                        innerHtml += "<li id='relationItem" + number + "'>";
                                        innerHtml += (item.title || item.linkText);
                                        innerHtml +=
                                            "<a class='relation_delete' onclick=\"removeRelation('" +
                                            number +
                                            "')\">[<%="移除".ToLang()%>]</a>";
                                        innerHtml += "</li>";

                                        innerData += (item.pID || "") + "|";
                                        innerData += (item.columnID || "") + "|";
                                        innerData += escape(item.linkText || "") + "|";
                                        innerData += escape(item.linkUrl || "") + "|";
                                        innerData += number + ",";
                                    });

                                $("#ulRelation").append(innerHtml);
                                document.getElementById("hidRelationData").value += innerData;
                                whir.dialog.remove();
                            }

                            addRelation('<%=RelatedList%>');
                            //移除一个相关文章
                            function removeRelation(number) {
                                $("#relationItem" + number).remove();
                                var innerData = $("#hidRelationData").val();
                                var datas = innerData.split(',');
                                var newInnerData = "";
                                $(datas)
                                    .each(function (idx, item) {
                                        var temp = item.split('|');
                                        if (temp.length < 5)
                                            return true;
                                        var tempNumber = temp[4];
                                        if (tempNumber != number) {
                                            newInnerData += temp[0] + "|";
                                            newInnerData += temp[1] + "|";
                                            newInnerData += temp[2] + "|";
                                            newInnerData += temp[3] + "|";
                                            newInnerData += temp[4] + ",";
                                        }
                                    });
                                $("#hidRelationData").val(newInnerData);
                            }
                        </script>
                        <% }%>
                        <% if (IsShowWorkFlowLogs) {%>
                        <div class="form-group no-margin">
                            <%="审核日志".ToLang()%>
                        </div>
                        <div>
                            <ul>
                                <asp:Repeater ID="rptList" runat="server">
                                    <ItemTemplate>
                                        <li style="display: block; border-bottom: 1px solid #ddd; padding: 5px 0;"><span>
                                            <%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}")%></span><br />
                                            <%#Eval("CreateUser")%>：<%#Eval("Describe").ToStr()%></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <% }%>
                    </div>
                </div>
                <% }%>
            </div>
    </div>

    </form>
</div>
</div>

<script type="text/javascript">

    $(document).ready(function () {
        //如果是开启工作流的单篇而且审核通过的，需把state状态为改成0
        <%if (SinglePageFlowStatus != 0) //0-未审核，-1 审核通过 ，-2 退审
{%>
        $("input[name='State']").val("0");
        <%}%>

        <%=SubmitFormScript%>
    });
    <%if (IsOpenFrame) {%>
    $("#panelleft").css("width", "100%");
    <% }%>

    var op = {
        dataType: 'json',
        type: 'POST',
        success: function (response) {
            if (response.Status == true) {
                contentView(" <div class=\"panel-heading\"><a style=\"text-decoration:underline;\" target=\"_blank\" href=\"<%=SysPath%>ModuleMark/common/preview.aspx?columnId=<%=ColumnId%>&subjectid=<%= SubjectId%>&ItemId=<%=ItemId%>\"><%="生成预览页面成功，请点击此处访问预览页面！".ToLang() %></a></div>");
            } else {
                whir.toastr.error(response.Message);
            }
            whir.loading.remove();
            return false;
        },
        error: function (response) {
            whir.toastr.error(response.Message);
            whir.loading.remove();
        }
    };

    function preview() {
        //预览之前 触发编辑器赋值
        $(document).click();
        $("[name='_action']").val("Preview");
        var $form = $("#formEdit");
        $form.post(op);
        $("[name='_action']").val("Save");
        return false;
    }
    var opFlow = {
        dataType: 'json',
        type: 'POST',
        success: function (response) {
            if (response.Status == true) {
                whir.toastr.success(response.Message, true, false, "<%=BackPageUrl%>");
            } else {
                whir.toastr.error(response.Message);
            }
            whir.loading.remove();
            return false;
        },
        error: function (response) {
            whir.toastr.error(response.Message);
            whir.loading.remove();
        }
    };

    function saveToFlow() {
        $("[name='_action']").val("SaveToFlow");
        var $form = $("#formEdit");
        $form.post(opFlow);
        return false;
    }
    function audit() {
        $("[name='_action']").val("Audit");
        var $form = $("#formEdit");
        $form.post(opFlow);
        return false;
    }
    //单篇栏目推送
    function singlePageCopy() {
        whir.dialog.frame('<%="选择栏目推送".ToLang()%>', "<%=SysPath%>ModuleMark/Common/Column_Select.aspx?columnid=<%=ColumnId%>&subjectid=<%= SubjectId%>&selectedtype=checkbox&callback=copyAction", null, 800, 500);
        return false;
    }
    //选择完推送的栏目后回调函数
    function copyAction(json) {
        whir.dialog.remove();
        json = eval('(' + json + ')');
        var column = "";
        $(json).each(function (idx, item) {
            column += item.id + ",";
        });
        if (column.length > 0)
            column = column.substring(0, column.length - 1);
        whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=SinglePageCopy",
            {
                data: {
                    selected: "<%=ItemId%>",
                    ColumnID: "<%=ColumnId%>",
                    SelectedOpen: column
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        whir.toastr.success(response.Message);
                    } else {
                        whir.toastr.error(response.Message);
                    }
                }
            }
        );
        return false;
    }
    //预览
    function contentView(content) {
        var opts = {
            title: '<%="预览".ToLang()%>',
            content: content,
            ok: function (dialog) {
            },
            cancel: function (dialog) { dialog.close(); },
            okText: '<%="确定".ToLang()%>',
            cancelText: '<%="关闭".ToLang()%>',
            showOk: false,
            showCancel: true,
            iframe: {
                url: '',
                width: 500,
                height: 500,
                scroll: false
            },
            zIndex: 1003
        };
        whir.dialog.show(opts);
    }
</script>
