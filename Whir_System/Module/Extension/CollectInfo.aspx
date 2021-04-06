<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectInfo.aspx.cs" Inherits="whir_system_module_extension_CollectInfo" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .listContainer {
            border: 1px solid gray;
            height: 180px;
            overflow: auto;
        }

        .listResult {
            border: 1px solid gray;
            height: 180px;
            overflow: auto;
        }

        .tdContainer {
            height: 100%;
        }

        .All_table table td {
            padding: 3px 4px !important;
        }

        .collectionConfig {
            margin-left: 180px;
        }

        p {
            margin: 3px 0px;
            padding: 0px;
        }
    </style>
    <script type="text/javascript">
        var collectid = <%= CollectId %>;

        $(function () {
            $("#btnLoadPageSource").click(function () {
                loadPageCode();
            });
            $("#btnCollection").click(function () {
                collectionItems();
            });
        });

        //加载页面源代码
        function loadPageCode() {
            $(".listContainer").text("<%="列表加载中...".ToLang()%>");
            $.ajax({
                type: "GET",
                url: "<%=SysPath%>ajax/extension/GetCollectList.aspx",
                data: { collectid: collectid },
                success: function (data) {
                    $(".listContainer").html("");
                    var items = eval(data);
                    for (i = 0; i < items.length; i++) {
                        var a = $("<a></a>").attr("href", items[i].Url).attr("target", "_blank").text(items[i].Title);
                        var checkbox = $("<input type='checkbox' checked='checked' />");
                        var li = $("<li></li>").append(checkbox).append(a);
                        $(".listContainer").append(li);
                    }
                    $(".listCount").html("<input type='checkbox' checked='checked' id='checkAll' />" +
                        "<a href='javascript:void(0)' id='selectAll'><%="全选".ToLang()%></a>" + "/" +
                        "<a href='javascript:void(0)' id='selectNone'><%="取消".ToLang()%></a>" +
                        "&nbsp;&nbsp;<%="共计：".ToLang()%>" + items.length + "<%="条记录".ToLang()%>");

                    //全选/取消
                    $("#checkAll").change(function () {
                        var checked = ($(this).attr("checked") == "true" || $(this).attr("checked") == true);
                        if (checked) {
                            $(".listContainer input").attr("checked", 'true'); //全选
                        } else {
                            $(".listContainer input").removeAttr("checked"); //取消全选
                        }
                    });

                    //全选
                    $("#selectAll").click(function () {
                        $("#checkAll").attr("checked", 'true');
                        $(".listContainer input").attr("checked", 'true'); //全选
                    });
                    //取消
                    $("#selectNone").click(function () {
                        $("#checkAll").removeAttr("checked");
                        $(".listContainer input").removeAttr("checked"); //取消全选
                    });

                },
                error: function (msg) {
                    $(".listContainer").text(" <%="加载出错：".ToLang()%>" + msg);
                }
            });
        }

        var count = 0;

        //采集列表选中项
        function collectionItems() {
            var divResult = document.getElementById("divResult");
            count = 0;
            $(".listResult").html("");
            $("#btnCollection").attr({ "disabled": "disabled" });
            $(".listContainer input:checked").each(function (idx) {
                var title = $(this).next().text();
                var url = $(this).next().attr("href");
                var isAllowRepeat = $("#isAllowRepeat").prop("checked") ? false : true;
                var filterField = $("#<%= dropFilterField.ClientID %>").val();
                var sort = "";
                var date = new Date();
                sort += date.getFullYear().toString().substring(2, 4);//取后2位年份
                sort += date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                sort += date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                sort += date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
                sort += date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
                //sort+=date.getSeconds()<10?"0"+date.getSeconds():date.getSeconds();
                //sort+=date.getMilliseconds()<10?"0"+date.getMilliseconds():date.getMilliseconds()>99?date.getMilliseconds().toString().substring(0,2):date.getMilliseconds();
                sort += ("0000" + idx).substr(-4)
                //sort+=idx<10?"0"+idx:idx>99?idx.toString().substring(0,2):idx;  //只取2位
                $.ajax({
                    url: "<%=SysPath%>module/extension/CollectSaveToDb.aspx",
                    type: "POST",
                    data: {
                        collectid: collectid,
                        title: title,
                        url: url,
                        sort: sort,
                        isAllowRepeat: isAllowRepeat,
                        filterField: filterField
                    },
                    success: function (data) {
                        count++;
                        $(".listResult").html($(".listResult").html() + "<%="第".ToLang()%>" + count + "<%="条：".ToLang()%>" + data + "<br/>");
                        if ($(".listContainer input:checked").length == count) {
                            $("#btnCollection").removeAttr("disabled");
                            $(".listResult").html($(".listResult").html() +" <%="采集完成" .ToLang()%><br/>");
                        }

                        divResult.scrollTop = divResult.scrollHeight;
                    }
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">

        <div class="panel-body">
            <div class="row">
                <div class="col-xs-4">
                    <div class="input-group">
                        <%="项目名称：".ToLang()%><%= Collection.ItemName %>
                    </div>
                </div>

                <div class="col-xs-3">
                    <div class="input-group">
                        <input type='checkbox' checked='checked' id='isAllowRepeat' />
                        <%="是否允许重复".ToLang()%>
                    </div>
                </div>
                <div class="col-xs-5  ">
                    <div class="list">
                        <ul>
                            <li><%="查重字段：".ToLang()%></li>
                            <li>
                                <asp:DropDownList ID="dropFilterField" Width="120" runat="server" CssClass=" form-control">
                                </asp:DropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="col-xs-12">
                    <%="采集列表：".ToLang()%>
                    <div class="listContainer form-control">
                    </div>
                </div>
                <div class="col-xs-12">
                    <%="采集结果：".ToLang()%>
                    <div id="divResult" class="listResult form-control">
                    </div>
                </div>
            </div>
            <div class="space15"></div>
            <div class="button_submit_div" style="text-align: center;">
                <a class="btn btn-white" id="btnLoadPageSource"><b><%="加载列表".ToLang()%></b></a>
                <a class="btn btn-info " id="btnCollection">
                    <b><%="开始采集".ToLang()%></b></a>
            </div>
        </div>
    </form>
</asp:Content>
