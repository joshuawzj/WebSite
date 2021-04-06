<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SubmitFormList.aspx.cs" Inherits="Whir_System_Module_Developer_SubmitFormList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function preview(id) {
            whir.dialog.frame('<%="预览".ToLang() %>', "SubmitForm_PreView.aspx?submitid=" + id + "&time=" + new Date().getTime(), "",800, 600,false);
        }

        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('请选择');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="提交表单生成器".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group">
                    <%if (IsDevUser)
                        {%> <a class="btn btn-white" href="SubmitForm_Edit.aspx"><%="添加表单".ToLang()%></a><%} %>
                </div>
                <table id="Common_Table" class="SubmitFormList_wap"></table>
                <div class="space10"></div>
                <div class="operate_foot">
                    <input type="hidden" id="hidChoose" />
                    <%if (IsDevUser)
                        {%> <a id="lbDel" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a><%} %>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document)
            .ready(function () {
                whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");
            });
        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Developer/SubmitForm.aspx?_action=GetList",
                dataType: "json",
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
                         whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'SubmitId', width: 60, align: 'left', valign: 'middle' },
                    { title: '<%="表单名称".ToLang()%>', field: 'Name', align: 'left', valign: 'middle' },
                    { title: '<%="需要审核".ToLang()%>', field: 'IsVerify', align: 'left', valign: 'middle', formatter: function (value, row, index) { return GetBoolText(value, row, index); } },
                    { title: '<%="类型".ToLang()%>', field: 'ColumnName', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 300, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]

            });
            whir.loading.remove();
        }

        initTable();

        //显示是、否图片
        function GetBoolText(value, row, index) {
            if (!value) {
                return '<span class="fontawesome-remove text-danger"></span>';
            } else {
                return '<span class="fontawesome-ok text-success"></span>';
            }
        }

        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.SubmitId + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            <%if (IsDevUser) {%>
            html += '<a name="aEdit" class="btn btn-white" href="submitform_template.aspx?submitid=' + row.SubmitId + '&backurl=<%= CurrentPageUrl %>"><%="自定义模板".ToLang() %></a>';
            html += '<a name="lbDelete" class="btn btn-white" onclick="preview(' + row.SubmitId + ')" href="javascript:;"><%="预览".ToLang()%></a>';
            <%}%>
            html += '<a name="aEdit" class="btn btn-white" href="submitform_edit.aspx?submitid=' + row.SubmitId + '"><%="编辑".ToLang() %></a>';
            <%if (IsDevUser) {%>
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.SubmitId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
              <%}%>
            html += '</div>';
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
            $table.bootstrapTable('refresh');
        }

        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                 function () {
                     whir.dialog.remove();
                     whir.ajax.post("<%=SysPath%>Handler/Developer/SubmitForm.aspx",
                     {
                         data: {
                             _action: "Del",
                             submitId: id
                         },
                         success: function (response) {
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

             $("#lbDel")
                 .click(function () {
                     if ($("#hidChoose").val() == "") {
                         whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
                     } else {
                         whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                             function () {
                                 whir.dialog.remove();
                                 whir.ajax.post("<%=SysPath%>Handler/Developer/SubmitForm.aspx",
                            {
                                data: {
                                    _action: "DelAll",
                                    selected: $("#hidChoose").val()
                                },
                                success: function (response) {
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

                 });
    </script>
</asp:Content>
