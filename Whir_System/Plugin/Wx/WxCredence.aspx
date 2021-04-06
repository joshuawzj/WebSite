<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="WxCredence.aspx.cs" Inherits="Whir_System_Plugin_Wx_WxCredence" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
     <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
         <div class="panel-heading"><%="公众号管理".ToLang()%><label style="float:right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>") %></label></div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                    <a class="btn btn-white" href="EditCredence.aspx"><%="添加公众号".ToLang()%></a>
                </div>
                 <table id="Common_Table">
                   </table> 
                  <div class="space10"></div>
                <div class="operate_foot">
                    <input type="hidden" id="hidChoose"/>
                    <a id="lbDel" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                </div>
                 <div class="clear"></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");

        });

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=WxCredences",
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
                    { title: 'AppId', field: 'AppId', width: 200, align: 'center', valign: 'middle' },
                    { title: 'AppName', field: 'AppName', align: 'center', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 450, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]

            });
            whir.loading.remove();
        }

        initTable();


        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };


        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.AppId + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = "";
            html += '<a name="aActive" class="btn btn-primary" onclick="setCurrent(\'' + row.AppId + '\');" href="javascript:void(0);"><%="设为当前公众号".ToLang() %></a>';
            html += '<a style="margin:0px 2px;" name="aActive" class="btn btn-default" onclick="showConfig(\'' + (row.Token || '') + '\');" href="javascript:void(0);"><%="如何配置".ToLang() %></a>';
            html += '<a style="margin:0px 2px;" name="aEdit" class="btn btn-info" href="EditCredence.aspx?appid=' + row.AppId + '"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(\'' + row.AppId + '\');" href="javascript:;"><%="删除".ToLang()%></a>';
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
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx",
                    {
                        data: {
                            _action: "RemoveCredence",
                            appids: id
                        },
                        success: function (response) {
                            whir.loading.remove();
                            whir.dialog.remove();
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

        function setCurrent(id) {
            whir.dialog.confirm("<%="确认要设置为当前公众号吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx",
                    {
                        data: {
                            _action: "SetCurrentCredence",
                            appid: id
                        },
                        success: function (response) {
                            whir.loading.remove();
                            whir.dialog.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                window.setTimeout(function () {
                                    window.location.reload();
                                }, 1000);
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            });
        }

        function showConfig(token) {
            var views = [];
            if (token) {
                views.push('<p><%="需要进入您公众号后台的“基本配置”里面开启“服务器配置”，并填写服务器地址，您的公众号信息推送接收服务器地址如下。".ToLang()%></p>');
                views.push('<p style="word-break:break-all;">' + window.location.protocol + '//' + window.location.host + '<%=SysPath%>Plugin/Wx/Ajax/Callback.aspx?token=' + token + '</p>');
            } else {
                views.push('<p><%="您的公众号信息不完整，请完善公众号配置信息".ToLang()%></p>');
            }
            whir.dialog.confirm('<div style="margin:4px;max-width:500px;text-align:left;">'+views.join('')+'</div>', function () {
                whir.dialog.remove();
                return false;
            });
        }

        $("#lbDel").click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                        data: {
                            _action: "RemoveCredence",
                            appids: $("#hidChoose").val()
                        },
                        success: function (response) {
                            whir.loading.remove();
                            whir.dialog.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    });
                    return false;
                });
            }
        });

    </script>
</asp:content>

