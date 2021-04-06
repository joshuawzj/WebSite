<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Picker.aspx.cs" Inherits="whir_system_ModuleMark_common_Picker" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--树状结构-->
    <script src="<%=SysPath %>Res/assets/js/tree/bootstrap-treeview.js" type="text/javascript"></script>
    <script>
        $(function () {

            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                var str = $("#hidChoose").val();
                var strTxt = getSelectStr();
                if (str != "") {
                    window.parent.$("#<%=HidChooseId %>").val(str).change();

                    formRequiredValidator('<%=HidChooseId %>', true,'<%=FormId%>'); //校验必填项的方法

                    var dropId = window.parent.$("#<%=ControlId %>_drop");
                    var controlId = window.parent.$("#<%=ControlId %>");//_preview
                    var initialPreview = [];
                    var initialPreviewConfig = [];
                    var strs = str.split("*");
                    var strTxts = strTxt.split("|");
                    for (var i = 0; i < strs.length; i++) {
                        initialPreview.push(_uploadFilesPath + strs[i]);
                        var params = strTxts[i].split("*");
                        initialPreviewConfig.push({ caption: params[1], size: params[0], name: strs[i], key: i + 1 });
                    }
                    controlId.fileinput('refresh', { isCustom: true, initialPreview: initialPreview, initialPreviewConfig: initialPreviewConfig });

                    window.parent.whir.dialog.remove();
                } else {
                    whir.toastr.warning('<%="请选择".ToLang() %>');

                }
                return false;
            });
        });

        function getSelectStr() {
            var el = "input[name=btSelectItem]";
            var selected = "";
            $(el)
                .each(function () {
                    if ($(this).prop('checked')) {
                        if (selected == "") {
                            selected = parseInt($(this).attr("size").replace("KB", "")) * 1024 + "*" + $(this).attr("filename");
                        } else {
                            selected = parseInt($(this).attr("size").replace("KB", "")) * 1024 + "*" + $(this).attr("filename") + "|" + selected;
                        }
                    }
                });
            return selected;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel">
        <div class="panel-body">
            <div class="row">
                <div class="col-xs-3" style="margin-right: -15px;">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <%="文件夹".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="site_select" id="folderList">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-9 ">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <%= IsPic==1?"图片".ToLang():"文件".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-10 ">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="showFolder">
                                            <%=UploadFilePath %></span>
                                        <input type="text" class="form-control" id="key" placeholder="<%="搜索内容".ToLang()%> ">
                                        <span class="input-group-addon btn btn-info" onclick="search();">
                                            <%="搜索".ToLang()%>
                                        </span><span class="input-group-addon btn btn-info" onclick="reload();">
                                            <%="刷新".ToLang()%>
                                        </span>
                                    </div>
                                </div>

                            </div>
                            <br />
                            <table id="Common_Table">
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="txtPath" value="<%=UploadFilePath %>" name="savePath" />
    <input type="hidden" id="hidChoose" value="" />


    <script type="text/javascript">


        function getTreeJson() {
            whir.ajax.post('<%= SysPath%>Handler/Extension/UploadImages.aspx', {
                data: {
                    _action: "GetFolderTree",
                    rootFolder: '<%=UploadFilePath %>'
                },
                success: function (result) {
                    if (result.Status) {
                        bindTree(result.Message);
                    } else {
                        whir.toastr.warning(result.Message);
                    }
                    whir.loading.remove();
                }

            });
            whir.loading.remove();
        }

        var $table = $('#Common_Table'), _that;

        //绑定文件夹树
        function bindTree(treeJson) {
            var defaultData = eval(treeJson);
            $('#folderList').treeview1({
                data: defaultData,
                multiSelect: $('#chk-select-multi').is(':checked'),
                onNodeSelected: function (event, node) { //切换文件夹
                    var path = '<%=UploadFilePath %>' + node.href;
                    if (path.indexOf("//") > 0)                        //防止根目录多了//
                        path = path.substring(0, path.length - 1);
                    $("#txtPath").val(encodeURIComponent(path)); //防止中文文件夹 出现乱码
                    reload();
                    $("#showFolder").attr("title", path);
                    if (path.length > 25) { //目录太多太长，剪短点
                        path = path.substring(0, 10) + "..." + path.substring(path.length - 10, path.length);
                    }
                    $("#showFolder").html(path);
                }
            });
        }

        //列表
        function initTable() {
            $table.bootstrapTable({
                  <% if (IsPic == 1)
        {%>
                url:"<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=<%= UploadFilePath %>",
                  <% }
        else
        {  %>
                url:"<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=<%= UploadFilePath %>",
                   <% } %>
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();

                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {

                },
                onPageChange: function () {
                    var path = $("#txtPath").val();
                    var key = $("#key").val();

                       <% if (IsPic == 1)
        {%>
                    if (key == "")
                        $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + path });
                    else
                        $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&key=" + key + "&path=" + path });
                    <% }
        else
        {  %>
                    if (key == "")
                        $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + path });
                    else
                        $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&key=" + key + "&path=" + path });
                        <% } %>

                },
                columns: [
                     <%if (IsPic == 1)
        {%> { title: '预览', field: 'View', width: 100, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetView(value, row, index); } },
                    <% }%>
                     { title: '图标', type: 'Type', align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetIcon(value, row, index); } },
                     { title: '文件名称', field: 'Name', align: 'left', valign: 'middle' },
                     { title: '物理名称', field: 'RealName', align: 'left', valign: 'middle' },
                     { title: '大小', field: 'Size', align: 'left', valign: 'middle' },
                     { title: '操作', field: 'Op', width: 70, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                    <%if (IsMultiple == "checkbox")
        {%>
                     { title: '<input type="<%=IsMultiple  %>" id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } }
                    <% }
        else
        {%>
                    {title: '', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } }
                      <%  } %>
                ],
                onDblClickRow: function (row) {
                    openDir(row);
                }

            });
            whir.loading.remove();
        }

        //双击打开文件夹
        function openDir(row) {
            if (row.Type != "File") {

                $("#txtPath").val($("#txtPath").val() + encodeURIComponent(row.RealName + "/"));        //防止中文文件夹 出现乱码 
                var path = decodeURIComponent($("#txtPath").val());
                currentpath = encodeURIComponent(path);            //更改上传路径

                         <% if (IsPic == 1)
        {%>
                $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + encodeURIComponent(path) });
                          <% }
        else
        {  %>
                $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + encodeURIComponent(path) });
                   <% } %>

                $("#showFolder").attr("title", path);
                if (path.length > 25) {                            //目录太多太长，剪短点
                    path = path.substring(0, 10) + "..." + path.substring(path.length - 10, path.length);
                }
                $("#showFolder").html(path);
            }
        }


        initTable();


        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem', '*'); //需要使用*号隔开
                    // whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem','');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidChoose', 'btSelectItem');
                }

            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem', '*'));
                });
            });
        }

        $(document).ready(function () {

            //绑定文件夹树
            getTreeJson();

            //注册回车事件
            document.onkeydown = function (e) {
                var ev = document.all ? window.event : e;
                if (ev.keyCode == 13 && $("#key").is(":focus")) {
                    search();
                }
            };

        });


         <%--列表绑定 --%>
        //获取图标HTML
        function GetIcon(value, row, index) {
            var icon = "";
            if (row.Type != "File") {
                icon = "<img src='<%=SysPath %>res/images/ext/folder.gif'>";
            } else {
                icon = "<img src='<%=SysPath %>res/images/ext/" + row.Ext.substring(1, row.Ext.length) +".gif' onerror=\"this.src='<%=UploadFilePath%>nopic/1.gif'\">";
            }
            return icon;
        }
        //获取预览HTML
        function GetView(value, row, index) {
            var html = "";
            var imgUrl = '<%=UploadFilePath %>' + row.FullName;
            var ext = row.Ext.toLowerCase();
            if (ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".bmp") {
                html += '<a  href="javascript:view(\'' + imgUrl + '\');" >';
                html += "<img src='" + imgUrl + "' style='max-width:100px;max-height:75px;padding:3px;' onerror=\"this.src='<%=UploadFilePath%>nopic/1.gif'\"></a>";
            }
            return html;
        }

        //获取操作HTML 
        function GetOperation(value, row, index) {
            var path = $("#txtPath").val();
            var html = "";
            if (row.Type == "File") {
                html += '<a class="btn btn-info" href="<%=SysPath %>Module/Extension/Download.aspx?path=<%=UploadFilePath %>&name=' + encodeURIComponent(row.FullName) + '" target="_blank">下载</a>';
            }
            return html;
        }
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            if (row.Type == "File") {
                var html = '<input type="<%=IsMultiple %>" name="btSelectItem" value="' + row.FullName + '"  size="' + row.Size + '"  filename="' + row.Name + '"/>';
                return html;
            } else {
                return "";
            }
        }

        <%--按钮操作 --%>
        //刷新列表
        function reload() {
            var path = $("#txtPath").val();
            var url = "";
              <% if (IsPic == 1)
        {%>
                url = "<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + path;
                  <% }
        else
        {  %>
            url = "<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&path=" + path;
                   <% } %>
            $table.bootstrapTable('refresh', { url: url });


        }

        //搜索
        function search() {
            var path = $("#txtPath").val();
            var key = $("#key").val();
            var url = "";
            <% if (IsPic == 1)
        {%>
                url = "<%=SysPath%>Handler/Extension/UploadImages.aspx?_action=GetList&allowFileType=<%=FileExts%>&key=" + key + "&path=" + path;
            <% }
        else
        {  %>
            url = "<%=SysPath%>Handler/Extension/UploadFiles.aspx?_action=GetList&allowFileType=<%=FileExts%>&key=" + key + "&path=" + path;
            <% } %>

            $table.bootstrapTable('refresh', { url: url });
            whir.loading.remove();
            }

    </script>

</asp:Content>
