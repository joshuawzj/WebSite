<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContentManager.ascx.cs"
    Inherits="Whir_ContentManager" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:PlaceHolder ID="phData" runat="server">

    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/label/bootstrap-table-label.js"></script>
    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/filter-control/bootstrap-table-filter-control.js"></script>
    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/multiple-sort/bootstrap-table-multiple-sort.js"></script>
    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/cookie/bootstrap-table-cookie.js"></script>

    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>

    <style type="text/css">
        .fixed-table-toolbar .btn {
            height: 32px;
        }

        .icheckbox_flat-red {
            float: left;
        }
    </style>

    <div id="toolbar"></div>
    <table id="Common_Table"></table>
    <div class="space10"></div>
    <input type="hidden" id="hidChoose" />

    <div class="btn-group">
        <% if (!IsOpenFrame)
            {%>
        <% if (IsShowOpenSort)
            {%>
        <button id="Opensort" class="btn btn-white" disabled onclick="return openSort();"><%="排序".ToLang()%></button>

        <% }%>
        <%if (IsShowEdit)
            {%>
        <button id="Update" class="btn btn-white" disabled onclick="return openBatchUpdate();"><%="批量修改".ToLang()%></button>

        <% }%>
    </div>

    <div class="btn-group">
        <% if (IsShowAudit)
            {%>
        <button id="Audit" class="btn btn-white" disabled onclick="return passFlow();"><%="批量审核".ToLang()%></button>

        <% }%>
        <% if (IsShowReturned)
            {%>
        <button id="Returned" class="btn btn-white" disabled onclick="return openReturnReason();"><%="批量退审".ToLang()%></button>

        <% }%>
    </div>

    <div class="btn-group">
        <% if (IsShowPush)
            {%>
        <button id="Push" class="btn btn-white" disabled onclick="return openCopy();"><%="批量推送".ToLang()%></button>

        <% }%>
        <% if (IsShowTransfer)
            {%>
        <button id="Transfer" type="button" class="btn btn-white" disabled onclick="return openCut();"><%="批量转移".ToLang()%></button>

        <% }%>
    </div>

    <div class="btn-group">
        <% if (IsShowDelete)
            {%>
        <button id="remove" class="btn text-danger border-danger" disabled><%="批量删除".ToLang()%></button>
        <% }%>
        <% if (IsDel)
            {%>
        <button id="Restore" class="btn btn-white" disabled><%="批量还原".ToLang()%></button>
        <% }%>
        <% }%>
    </div>
    <script type="text/javascript">
        //打开排序页面
        function openSort() {
            if ($("#hidChoose").val() == '') {
                 whir.toastr.warning('<%="请选择".ToLang()%>');
                return false;
            }
            var totalCount = whir.checkbox.getSelectTotalCount("btSelectItem");
            var totalItemsCount = $table.bootstrapTable('getOptions').totalRows;
            if (totalItemsCount == 1) {
                 whir.toastr.warning('<%="只有一条数据不能排序".ToLang()%>');
                return false;
            }
            if (totalCount >= totalItemsCount) {
                 whir.toastr.warning('<%="不能选择全部记录进行排序".ToLang()%>');
                return false;
            }
            var except = $("#hidChoose").val();
            whir.dialog.frame('<%="排序".ToLang()%>', "<%=SysPath%>Modulemark/Common/Sort.aspx?columnid=<%=ColumnId%>&except=" + except + "&flowid=<%=CurrentActivityId%>&subjectid=<%=SubjectId%>&total=" + $table.bootstrapTable('getOptions').totalRows, null, 1000, 600);
            return false;
        }
        function sortAction(primaryId) {
            OpenSort(primaryId);
        }

        //打开批量推送页面
        function openCopy() {
            if ($("#hidChoose").val() == '') {
                 whir.toastr.warning('<%="请选择".ToLang()%>');
                return false;
            }
            whir.dialog.frame('<%="选择栏目推送".ToLang()%>', "<%=SysPath%>ModuleMark/Common/Column_Select.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&selectedtype=checkbox&callback=copyAction", null, 800, 500);
            return false;
        }
        //打开批量更新页面
        function openBatchUpdate() {
            var itemIds = $("#hidChoose").val();
            whir.dialog.frame('<%="批量修改".ToLang()%>',
                "<%=SysPath%>Modulemark/Common/SelectField.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&itemIds=" + itemIds,
                null,
                1200,
                500);
            return false;
        }
        function copyAction(json) {
            whir.dialog.remove();
            json = eval('(' + json + ')');
            var column = "";
            $(json).each(function (idx, item) {
                column += item.id + ",";
            });
            if (column.length > 0)
                column = column.substring(0, column.length - 1);
            whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=Copy",
                {
                    data: {
                        selected: $("#hidChoose").val(),
                        ColumnID: "<%=ColumnId%>",
                        SubjectId: "<%=SubjectId%>",
                        SelectedOpen: column
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            resetChoose();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
                return false;
            }

            //打开退审页面
            function openReturnReason() {
                if ($("#hidChoose").val() == '') {
                     whir.toastr.warning('<%="请选择".ToLang()%>');
                return false;
            }
            whir.dialog.frame('<%="退审理由".ToLang()%>', "<%=SysPath%>ModuleMark/Common/WorkflowReason.aspx?ColumnId=<%=ColumnId%>&subjectid=<%=SubjectId%>&CurrentActivityId=<%=CurrentActivityId%>&selected=" + $("#hidChoose").val(), "", 400, 300);
            return false;
        }
        //批量通过审核CurrentActivityId
        function passFlow() {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要操作的数据行！".ToLang()%>");
            } else {
                whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=PassFlow",
                    {
                        data: {
                            selected: $("#hidChoose").val(),
                            ColumnID: "<%=ColumnId%>",
                            CurrentActivityId: "<%=CurrentActivityId%>"
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                resetChoose();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                    return false;
                }
            }
            //打开批量转移页面
            function openCut() {

                if ($("#hidChoose").val() == '') {
                     whir.toastr.warning('<%="请选择".ToLang()%>');
                return false;
            }
            whir.dialog.frame('<%="选择栏目转移".ToLang()%>', "<%=SysPath%>ModuleMark/Common/Column_Select.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&selectedtype=radiobox&callback=cutAction", null, 800, 500);
            return false;
        }
        function cutAction(json) {
            whir.dialog.remove();
            json = eval('(' + json + ')');
            var column = "";
            $(json).each(function (idx, item) {
                column += item.id + ",";
            });
            if (column.length > 0)
                column = column.substring(0, column.length - 1);
            whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=Cut",
                {
                    data: {
                        selected: $("#hidChoose").val(),
                        ColumnID: "<%=ColumnId%>",
                        SubjectId: "<%=SubjectId%>",
                        SelectedOpen: column
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            resetChoose();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
                return false;
            }

            var _sysCurrentPage =<%=CurrentPage%>;
        var _sysPageSizeCookieName = "ezEIPListPageSize";
        var $table = $('#Common_Table'), _that, _dialog;
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Common/Common.aspx?_action=GetList&ColumnId=<%=ColumnId%>&SubjectId=<%=SubjectId%>&IsDel=<%=IsDel%>&Where=<%=Where%>&IsMarkType=<%=IsMarkType%>&user=<%=user%>",
                dataType: "json",
                toolbar: "#toolbar",
                <%if (!IsOpenFrame)
        { %>
                showRefresh: true,  //刷新按钮
                filterShowClear: true,//清空搜索按钮
                showToggle: false,
                showColumns: false,
                advancedSearch: true,
                showSelectColumn: true,
                <%if (IsShowExport)
        {%>
                showExport: true,
                <% }%>   
                <%if (IsShowImport)
        {%>
                showImport: true,
                <% }%>
                <%if (Whir.ezEIP.Web.SysManagePageBase.IsDevUser)
        {%>
                showLabel: true,
                <% }%>
                showPrint: true,
                detailview: true,               
                <%}%>
                search: false, //显示搜索框
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                sortable: true,
                clickToSelect: false,
                pageSize: <%=MinPageSize%>,
                idField: '<%=IdField%>',
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                    whir.dialog.frame('<%="选择列".ToLang()%>', '<%=SysPath%>ModuleMark/Common/ShowFormlist.aspx?columnid=<%=ColumnId%>' + "&time=" + new Date().getMilliseconds(), null, 600, 400);
                },
                customSearch: function (text) {

                },
                <%=Columns%>
            });
        }

        initTable();
        setTimeout(function () {
            $table.bootstrapTable('resetView');
        }, 200);

        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            whir.checkbox.checkboxOnload('#Update,#remove,#Returned,#Audit,#Push,#Transfer,#Restore,#Opensort', 'hidChoose');
        }

  <% if (IsShowSort && !IsOpenFrame)
        { %>
        $(function () {

            //拖动排序
            $("#Common_Table").sortable(
                {
                    items: "tr[data-index]",
                    appendTo: 'parent',
                    handle: '.dragCursor',
                    stop: function (event, ui) {
                        saveSort(event, ui);
                    },
                    axis: 'y'
                });

        });

        //异步保存排序
        function saveSort(event, ui) {

            var ids = "";
            $(".dragCursor").each(function () {
                ids += $(this).attr("data-id") + ",";
            });

            whir.ajax.post("<%=SysPath%>Handler/Common/Sort.aspx",
                {
                    data: {
                        _action: "SortDrag",
                        columnId: '<%=ColumnId%>',
                        subjectId:'<%=SubjectId%>',
                        Ids: ids,
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            resetChoose();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });

            }
        <% }%>

        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };

        //实现拖拽排序
        function ForSort(value, row, index) {
            <%if (IsShowSort && !IsOpenFrame)
        { %>
            return ' <div  class="dragCursor fontawesome-move" sort="' +
                row.Sort +
                '"  title="<%="点击可以拖拽排序".ToLang()%> Id:' + row.Id+'" data-id="' +
                row.Id +
                '"></div> ';
            <%}
        else
        { %>
            return ' <div class="fontawesome-move" sort="' +
                row.Sort +
                '"  title="<%="点击可以拖拽排序".ToLang()%> Id:' + row.Id +'" data-id="' +
                row.Id +
                '"></div> ';
            <%}%>

        }

        function GetOperation(value, row, index) {
            var e = "";
            //投票
            if ("<%=IsShowQuestion.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="questionlist.aspx?columnid=<%=SurveyQuestionColumnId%>&PreColumnID=<%=ColumnId%>&topicid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="问题".ToLang()%></a> ';
            }
            if ("<%=IsShowSurveyPreview.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="Preview.aspx?columnid=<%=ColumnId%>&topicid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="预览".ToLang()%></a> ';
            }
            if ("<%=IsShowPreview.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="Preview.aspx?columnid=<%=ColumnId%>&voteid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="预览".ToLang()%></a> ';
            }

            if ("<%=IsShowSurveyStatistics.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="Statistics.aspx?columnid=<%=ColumnId%>&topicid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="统计".ToLang()%></a> ';
            }

            if ("<%=IsShowSurveyDetail.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="DetailList.aspx?columnid=<%=SurveyDetailColumnId%>&surveycolumnid=<%=ColumnId%>&topicid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="明细".ToLang()%></a> ';
            }
            if ("<%=IsShowVoteDetail.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="DetailList.aspx?columnid=<%=VoteDetailColumnId%>&votecolumnid=<%=ColumnId%>&topicid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="明细".ToLang()%></a> ';
                  }

                  if ("<%=IsShowSurveyAnswer.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="AnswerList.aspx?columnid=<%=SurveyAnswerColumnId%>&questionid=' + row.Id + '&subjectid=<%=SubjectId%>&topicid=<%=RequestUtil.Instance.GetQueryInt("topicid", 0)%>"><%="答案管理".ToLang()%></a> ';
                  }
                  if ("<%=IsShowAnswer.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="AnswerList.aspx?columnid=<%=AnswerColumnId%>&votecolumnid=<%=ColumnId%>&questionid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="答案".ToLang()%></a> ';
            }

            if ("<%=IsShowStatistics.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="Statistics.aspx?columnid=<%=ColumnId%>&voteid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="统计".ToLang()%></a> ';
            }

            if ("<%=IsShowEnable.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="javascript:;" onclick="VoteEnable(\'' + row.Id + '\',\'Enable\')"><%="启用".ToLang()%></a> ';
                e += '<a class="btn btn-white" href="javascript:;" onclick="VoteEnable(\'' + row.Id + '\',\'Disable\')"><%="禁用".ToLang()%></a> ';
            }

            if ("<%=IsShowExportDoc.ToStr().ToLower()%>" == "true") {
                e += '<a class="btn btn-white" href="javascript:;" onclick="InportJobRequest(\'' + row.Id + '\')"><%="导出".ToLang()%></a> ';
            }

            if ("<%=IsShowDetail.ToStr().ToLower()%>" == "true") {
                var url = '<%=DetailUrl%>'.replace(new RegExp(/{itemid}/g), row.Id);
                  e += '<a class="btn btn-white" href="' + url + '"><%="详细".ToLang()%></a> ';
              }

              if ("<%=IsShowJobRequest.ToStr().ToLower()%>" == "true") {
                e += '<a name="aApply" class="btn btn-white" href="<%=SysPath%>ModuleMark/Jobs/JobRequestlist.aspx?columnid=<%=JobRequestColumnId%>&jobsid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="应聘".ToLang()%></a> ';
            }

            if ("<%=IsShowChapter.ToStr().ToLower()%>" == "true") {
                e += '<a name="aChapter" class="btn btn-white" href="<%=SysPath%>ModuleMark/Magazine/ChapterList.aspx?columnid=<%=ChapterColumnId%>&itemid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="章节".ToLang()%></a> ';
            }

            if ("<%=IsShowArticle.ToStr().ToLower()%>" == "true") {
                e += '<a name="aArticle" class="btn btn-white" href="<%=SysPath%>ModuleMark/Magazine/InForList.aspx?columnid=<%=ArticleColumnId%>&itemid=' + row.Id + '&subjectid=<%=SubjectId%>"><%="文章".ToLang()%></a> ';
             }

             if ("<%=IsShowHistory.ToStr().ToLower()%>" == "true") {
                e += '<a name="aHistory" class="btn btn-white" href="javascript:;" onclick="History(\'' + row.Id + '\')"><%="历史".ToLang()%></a> ';
            }

            if ("<%=(IsShowEdit||(!IsDel && SysManagePageBase.IsRoleHaveColumnRes("查看"))).ToStr().ToLower()%>" == "true") {
                var editPageUrl ='<%=EditPageUrl%>';
                  if (editPageUrl.indexOf("%26filter%3d") > -1) {
                      editPageUrl = editPageUrl.replace(/%26isloadfilter%3d\d+/g, "%26filter%3d" + 1);
                  } else {
                      editPageUrl += "%26filter%3d" + 1;
                  }

                  if (editPageUrl.indexOf("?") > -1) {
                      e += '<a name="aEdit" class="btn btn-white" href="' + editPageUrl + '&itemid=' + row.Id + '"><%=(SysManagePageBase.IsRoleHaveColumnRes("修改")||IsShowEdit?"编辑":"查看").ToLang()%></a> ';
                } else {
                    e += '<a name="aEdit" class="btn btn-white" href="' + editPageUrl + '?itemid=' + row.Id + '"><%=(SysManagePageBase.IsRoleHaveColumnRes("修改")||IsShowEdit?"编辑":"查看").ToLang()%></a> ';
                }

            }
            if ("<%=IsShowDelete.ToStr().ToLower()%>" == "true") {
                e += ' <a class="btn text-danger border-normal" href="javascript:;" onclick="Del(\'' + row.Id + '\')"><%="删除".ToLang()%></a> ';
            }
            if ("<%=IsDel.ToStr().ToLower()%>" == "true") {
                e += ' <a class="btn btn-white" href="javascript:;" onclick="Rest(\'' + row.Id + '\')"><%="还原".ToLang()%></a> ';
            }
            e = "<div class='btn-group'>" + e + "</div>";
            return e;
        }

        //完成操作后，清理选择，刷新table
        function resetChoose() {
            $("#hidChoose").val("");
            $table.bootstrapTable('refresh');
            whir.checkbox.checkboxOnload('#Update,#remove,#Returned,#Audit,#Push,#Transfer,#Restore,#Opensort', 'hidChoose');
        }

        function OpenSort(selectid) {
            whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=OpenSort&ColumnId=<%=ColumnId%>&SubjectId=<%=SubjectId%>", {
                data: {
                    Selected: $("#hidChoose").val(),
                    SelectedOpen: selectid
                },
                success: function (response) {
                    whir.dialog.remove();
                    if (response.Status == true) {
                        whir.toastr.success(response.Message);
                        resetChoose();
                        whir.loading.remove();
                    } else {
                        whir.toastr.error(response.Message);
                    }
                }
            });
            whir.loading.remove();
            return false;
        }

        function Del(id) {
            if (!id) {
                whir.toastr.warning("<%="参数错误".ToLang()%>");
            } else {
                whir.dialog.confirm('<%=DeletString%>', function () {
                    whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=DeleteRow", {
                        data: {
                            id: id,
                            ColumnID: "<%=ColumnId%>",
                            SubjectID: "<%=SubjectId%>",
                            IsDel: "<%=IsDel%>"
                        },
                        success: function (response) {
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                resetChoose();
                                whir.loading.remove();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    });
                    whir.dialog.remove();
                    return false;
                });
                  }
                  return false;
              }

              $("#remove").click(function () {
                  if ($("#hidChoose").val() == "") {
                      whir.toastr.warning("<%="请选择要删除的记录".ToLang()%>");
            } else {
                whir.dialog.confirm('<%=DeletString%>',
                    function () {
                        whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=DeleteAll",
                                  {
                                      data: {
                                          selected: $("#hidChoose").val(),
                                          ColumnID: "<%=ColumnId%>",
                                          SubjectID: "<%=SubjectId%>",
                                          IsDel: "<%=IsDel%>"
                                      },
                                      success: function (response) {
                                          if (response.Status == true) {
                                              whir.toastr.success(response.Message);
                                              resetChoose();
                                              whir.loading.remove();
                                          } else {
                                              whir.toastr.error(response.Message);
                                          }

                                      }
                                  }
                        );
                        whir.dialog.remove();
                        return false;
                    });
                          }
            return false;
        });

                      $("#Restore").click(function () {
                          if ($("#hidChoose").val() == "") {
                              whir.toastr.warning("<%="请选择要还原的数据行".ToLang()%>");
            } else {
                whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=Restore", {
                    data: {
                        selected: $("#hidChoose").val(), ColumnId: "<%=ColumnId%>", SubjectID: "<%=SubjectId%>", IsDel: "<%=IsDel%>"
                          },
                          success: function (response) {
                              whir.loading.remove();
                              if (response.Status == true) {
                                  whir.toastr.success(response.Message);
                                  resetChoose();
                                  whir.loading.remove();
                              } else {
                                  whir.toastr.error(response.Message);
                              }
                          }
                      });
                      whir.loading.remove();
                      return false;
                  }
            return false;
        });


              function Rest(id) {
                  if (!id) {
                      whir.toastr.warning("<%="参数错误".ToLang()%>");
            } else {
                whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=Rest",
                    {
                        data: {
                            id: id,
                            ColumnId: "<%=ColumnId%>",
                            SubjectId: "<%=SubjectId%>"
                              },
                              success: function (response) {
                                  if (response.Status == true) {
                                      whir.toastr.success(response.Message);
                                      resetChoose();
                                      whir.loading.remove();
                                  } else {
                                      whir.toastr.error(response.Message);
                                  }
                              }

                          });
                          whir.loading.remove();
                          return false;
                      }
                      return false;
                  }

                  function InportJobRequest(id) {
                      if (!id) {
                          whir.toastr.warning("<%="参数错误".ToLang()%>");
                  } else {
                      var url = "<%=SysPath%>Handler/Common/Common.aspx?_action=InportJobRequest";
                      $('<form method="post" action="' +
                          url +
                          '"><input type="hidden" name="itemid" value="' +
                          id +
                              '"><input type="hidden" name="ColumnId" value="<%=ColumnId%>"></form>')
                    .appendTo('body')
                    .submit()
                    .remove();
            }
        }

        function History(itemid) {
            whir.dialog.frame("<%="历史记录".ToLang()%>", "<%=SysPath%>ModuleMark/Common/HistoryBak.aspx?columnid=<%=ColumnId%>&itemid=" +
                itemid +"&subjectid=<%=SubjectId%>&time=" + new Date().getMilliseconds(), null, 1200, 500, false);
              }
              //设置启用禁用
              function VoteEnable(id, action) {
                  if (!id) {
                      whir.toastr.warning("<%="参数错误".ToLang()%>");
            } else {
                whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=" + action,
                    {
                        data: {
                            id: id,
                            ColumnId: "<%=ColumnId%>",
                            SubjectId: "<%=SubjectId%>"
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                resetChoose();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                    return false;
                }
                return false;
            }


    </script>
    <script type="text/javascript">
        //获取主键Id
        function collectApid_Sort() {
            var apidsort = "";
            $(".apid_sort").each(function (i) {
                if (i == 0) {
                    apidsort += $(this).attr("apid") + "|" + $(this).val();
                } else {
                    apidsort += "," + $(this).attr("apid") + "|" + $(this).val();
                }
            });
            return apidsort;
        }
 
        //打开导出选择列的页面
        function openSelectColumn() {
            whir.dialog.frame('<%="导出".ToLang()%>', "<%=SysPath%>ModuleMark/Common/Formexport.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&flowid=<%=CurrentActivityId%>", null, 400, 300);
                }

                //打开导入页面
                function openImport() {
                    whir.dialog.frame('<%="导入".ToLang()%>', "<%=SysPath%>ModuleMark/Common/Formimport.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>", null, 800, 500);
                }

                //显示是、否图片
                function GetBoolText(value, row, index) {
                    if (value == "False" || value == "") {
                        return '<span class="fontawesome-remove text-danger"></span>';
                    } else {
                        return '<span class="fontawesome-ok text-success"></span>';
                    }
                }

                //显示获取隐藏的title
                function GetTitle(value, row, index) {

                    return '<input type="hidden" id="hidTitle' + row.Id + '" value="' + row.Title + '"/>';

                }

                //时间格式
                function GetDateTimeFormat(value, row, index, format) {
                    if (format == "yyyy-mm-dd") {
                        return whir.ajax.fixJsonDate(value, "-");
                    }
                    return whir.ajax.fixJsonDate(value);;
                }
                //区域选择
                function setAreaPickerValue(value, text, field) {
                    _that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + field).attr('areaid', value).val(text);
                    _that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + field).keyup();
                };

    </script>



</asp:PlaceHolder>
