<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="OperationLogList.aspx.cs" Inherits="Whir_System_Module_Log_OperationLogList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/filter-control/bootstrap-table-filter-control.js"></script>
    <script type="text/javascript">

        $(function () {
            $("#atype<%=OperateType%>").addClass("aSelect");
        });

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">

                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li <%=RequestInt32("type")==0? "class='active'":"" %>><a id="atype0" href="OperationLogList.aspx?type=0"><%="网站操作日志".ToLang() %></a> </li>
                        <li <%=RequestInt32("type")==1? "class='active'":"" %>><a id="atype1" href="OperationLogList.aspx?type=1"><%="模板操作日志".ToLang() %></a> </li>
                        <li <%=RequestInt32("type")==2? "class='active'":"" %>><a id="atype2" href="OperationLogList.aspx?type=2"><%="系统运行日志".ToLang() %></a></li>
                    </ul>
                    <br />

                    <table id="Common_Table" class="OperationLogList_table"></table>
                    <div class="space15"></div>
                    <div class="operate_foot">
                        <input type="hidden" id="hidChoose" />
                        <%if (IsCurrentRoleMenuRes("334"))
                            { %>
                        <div class="btn-group">
                            <a id="lbDel" class="btn text-danger border-danger" onclick="DeleteAll()"><%="批量删除".ToLang()%></a>
                            <a id="lbClear" class="btn text-danger border-danger" onclick="Clear()"><%="清空日志".ToLang()%></a>
                        </div>
                        <%} %>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
       
        
        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Module/Extension/Log.aspx?_action=GetList&type=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("type", 0)%>",
                dataType: "json",
                filterControl: true,
                showExport: false,
                showSelectColumn: false,
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data,mes) {
                    if(mes && mes.responseText){
                        whir.toastr.warning(mes.responseText);
                    }else{
                        whir.toastr.error("<%="获取数据失败！".ToLang() %>");
                     }
                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'OperateId', width: 60, align: 'left', valign: 'middle' },
                    { title: '<%="发生时间".ToLang()%>', field: 'CreateDate', width: 180, filterControl: '7',format:'yyyy-mm-dd hh:ii:ss',align: 'left', valign: 'middle',  formatter: function (value, row, index) { return whir.ajax.fixJsonDate(value); }},
                    { title: '<%="操作人".ToLang()%>', field: 'CreateUser', width: 180,filterControl: '1',align: 'left', valign: 'middle' },
                    { title: '<%="描述".ToLang()%>', field: 'Description', filterControl: '1',align: 'left', valign: 'middle',formatter: function (value, row, index) { return replaceHtml(value); }},
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 80, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]
            });
            whir.loading.remove();
        }

        initTable();
 
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem"  value="' + row.OperateId + '" />';

        }
        //替换html代码
        function replaceHtml(value)
        { 
            return value.replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/\"/g,"&quot;").replace(/&/g, "&amp;");
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = "";
            <%if (IsCurrentRoleMenuRes("334"))
        { %>
            html = '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.OperateId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
            <%}%>
            return html;
        }
        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidChoose', 'btSelectItem');
                }
            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem'));
                });
            });
        }

        function reload() {
           
            var type='<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("type", 0)%>';
            $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Module/Extension/Log.aspx?_action=GetList&type="+type+"&StartDate=&EndDate=" });
        }

        //删除
        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",function() {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Module/Extension/Log.aspx",
                        {
                            data: {
                                _action: "Delete",
                                logId: id
                            },
                            success: function(response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                    reload();
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
                    return false;
            });
            }

            //删除选择
            function DeleteAll() {
                if ($("#hidChoose").val() == "") {
                    whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function() {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Module/Extension/Log.aspx", {
                        data: {
                            _action: "DeleteAll",
                            logIds: $("#hidChoose").val()
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                    );
                    return false;
                });
            }
        }

        //清空
        function Clear() {
            whir.dialog.confirm("<%="确认要清空吗？".ToLang() %>", function() {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Module/Extension/Log.aspx",
                    {
                        data: {
                            _action: "Clear",
                            logType: <%=OperateType %>,
                            startDate: '<%=new DateTime(1970,1,1).ToString("yyyy-MM-dd")%>',
                            endDate: '<%=DateTime.Now.ToString() %>'
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            });
            }
    </script>
</asp:Content>
