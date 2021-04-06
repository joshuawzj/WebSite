<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master" EnableViewState="false"
    AutoEventWireup="true" CodeFile="HistoryBak.aspx.cs" Inherits="Whir_System_ModuleMark_Common_HistoryBak"%>

<%@ Import Namespace="Whir.Language"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"> 
    <div class="content-wrap">
        <div class="space15"></div>
        <table id="Common_Table"></table>
        <input type="hidden" id="hidChoose" /> 
        <div class="space15"></div>
        <button id="remove" class="btn text-danger border-normal" disabled><%="批量删除".ToLang()%></button>
    </div>
    <script type="text/javascript">

        var $table = $('#Common_Table'),_that;

        function initTable() {
            $table.bootstrapTable({
                url:
                    "<%=SysPath%>Handler/Common/Common.aspx?_action=GetHistoryBakList&ColumnId=<%=ColumnId%>&SubjectId=<%=SubjectId%>&itemid=<%=ItemID%>",
                dataType: "json",
                toolbar: "#toolbar",
                detailview: true,
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent:true,
                sortable: true,
                clickToSelect: false,
                idField: '<%=IdField%>',
                onLoadSuccess: function() {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
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
            whir.checkbox.checkboxOnload('#remove',
                'hidChoose');
        }

        function GetSort(value, row, index) {
            return ' <input type="text" class="form-control input-sm apid_sort" apid="' +
                row.<%=IdField%> +
                '" value="' +
                row.Sort +
                '" /> ';
        }

        function GetOperation(value, row, index) {
            var e = "<div class='btn-group'>";
            e += '<a name="aEdit" class="btn btn-white" href="HistoryBakDetail.aspx?columnid=<%=ColumnId%>&SubjectId=<%=SubjectId%>&itemid=' +
                row.Id +
                '"><%="详细".ToLang()%></a> ';
            e += ' <a class="btn btn-white" href="javascript:;" onclick="Restore(\'' +
                row.Id +
                '\')"><%="恢复".ToLang()%></a> ';
            e += ' <a class="btn text-danger border-normal" href="javascript:;" onclick="Del(\'' +
                row.Id +
                '\')"><%="删除".ToLang()%></a> ';
            e += "</div>";
            return e;
        }


        function Del(id) {
            if (!id) {
                window.parent.whir.toastr.warning("<%="参数错误".ToLang()%>");
            } else {
                whir.dialog.confirm('<%="确定要删除吗？".ToLang()%>',
                    function() {
                        whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=DeleteHistoryRow",
                            {
                                data: {
                                    ColumnID: "<%=ColumnId%>", 
                                    SubjectId: "<%=SubjectId%>",
                                    itemid: id
                                },
                                success: function(response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        window.parent.whir.toastr.success(response.Message);
                                        $table.bootstrapTable('refresh');
                                    } else {
                                        window.parent. whir.toastr.error(response.Message);
                                    }
                                }
                            }
                        );
                        whir.dialog.remove();
                        return false;
                        
                    });

            }
        }


        $("#remove")
            .click(function() {
                if ($("#hidChoose").val() == "") {
                    window.parent.whir.toastr.warning("<%="请选择要删除的数据行！".ToLang()%>");
                } else {
                    whir.dialog.confirm('<%="确定要删除吗？".ToLang()%>',
                        function() {
                            whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=DeleteHistoryAll",
                                {
                                    data: {
                                        selected: $("#hidChoose").val(),
                                        SubjectId: "<%=SubjectId%>",
                                        ColumnID: "<%=ColumnId%>"
                                    },
                                    success: function(response) {
                                        whir.loading.remove();
                                        if (response.Status == true) {
                                            window.parent.whir.toastr.success(response.Message);
                                            $table.bootstrapTable('refresh');
                                        } else {
                                            window.parent.whir.toastr.error(response.Message);
                                        }
                                    }
                                }
                            );
                            whir.dialog.remove();
                            return false;

                        });

                }

            });

        function Restore(id) {
               whir.ajax.post("<%=SysPath%>Handler/Common/Common.aspx?_action=DeleteHistoryRestore",
                    {
                        data: {
                            itemid: '<%=ItemID%>',
                            ColumnID: "<%=ColumnId%>",
                            SubjectID: "<%=SubjectId%>",
                            backid: id
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                window.parent.whir.toastr.success(response.Message);
                                $table.bootstrapTable('refresh');
                                  whir.dialog.remove();
                            } else {
                                window.parent.whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
              whir.dialog.remove();
                return false;
        }

        //显示是、否图片
        function GetBoolText(value, row, index) {
            if (value == "False") {
                return '<span class="fontawesome-remove text-danger"></span>';
            } else {
                return '<span class="fontawesome-ok text-success"></span>';
            }
        }
        //时间格式
        function GetDateTimeFormat(value, row, index, format) {
            if (format == "yyyy-mm-dd") {
                return whir.ajax.fixJsonDate(value,"-");
            }
            return whir.ajax.fixJsonDate(value);;
        }
        //区域选择
        function setAreaPickerValue(value, text, field) {
            _that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + field).attr('areaid',value).val(text);
            _that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + field).keyup();
        };

    </script>
</asp:Content>
