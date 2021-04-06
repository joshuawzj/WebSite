<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="FileManage.aspx.cs" Inherits="Whir_System_Plugin_FileManage_FileManage" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <!--树状结构-->
    <script src="<%=SysPath %>Res/assets/js/tree/bootstrap-treeview.js" type="text/javascript"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" runat="server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%="文件夹".ToLang()%></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="site_select" id="folderList">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-9 ">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%="文件".ToLang()%></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="input-group uploadimages-input-group">
                                            <span class="input-group-addon" id="showFolder"><%=CurrentSiteDirection %></span>
                                            <%if (IsCurrentRoleMenuRes("357"))
                                                { %>
                                            <input type="text" id="folderName" name="folderName" class="form-control" required="true" maxlength="20" placeholder="<%="新建文件夹名称".ToLang()%> " />
                                            <span class="input-group-addon btn btn-white" onclick="addNewFolder();"><%="新建文件夹".ToLang()%></span>
                                            <%} %>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group uploadimages-input-group">
                                        <%if (IsCurrentRoleMenuRes("358"))
                                          { %>
                                            <input type="text" class="form-control" id="key" placeholder="<%="搜索内容".ToLang()%> ">
                                            <span class="input-group-addon btn btn-white" onclick="search();"><%="搜索".ToLang()%> </span>
                                            <%} %>
                                            <span class="input-group-addon btn btn-white" onclick="reload();"><%="刷新".ToLang()%>
                                            </span>
                                             <%if (IsCurrentRoleMenuRes("360"))
                                               { %>
                                            <span class="input-group-addon btn text-danger border-normal" onclick="delAll();"><%="删除".ToLang()%></span>
                                            <%} %>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <table id="Common_Table"></table>
                                <br />
                                <%if (IsCurrentRoleMenuRes("362"))
                                    { %>
                                <div class="box_common" id="divUploadImg">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <%="上传文件".ToLang()%>
                                        </div>
                                        <div class="panel-body">
                                            <div class="col-md-12">
                                                <div id="divUploadFile">
                                                    <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" multiple  accept="<%=AcceptType %>"/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%} %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="txtPath" value="<%=CurrentSiteDirection %>" />
    <input type="hidden" id="hidChoose" value="" />
  <input type="hidden" id="browser" value="" name="browser" />
    </form>
    <!--[if lte IE 9]>
    <script type="text/javascript">  $("#browser").val("ie9");</script>
    <![endif]-->

    <script type="text/javascript">
        //上传文件
        var currentpath = "<%= Server.UrlEncode(CurrentSiteDirection) %>";
         
        //重新生成上传控件
        function RebuildUploadFile () {
            var html = '<input id="txt_file" value="" name="txt_file" type="file" class="file-loading" multiple  accept="<%=AcceptType %>"/>';
            $("#divUploadFile").html(html);
        }
 
        //注册上传控件
        function regeditUploadFile() {
            $("#txt_file").fileinput({
                language: '<%=GetLoginUserLanguage()%>',
                uploadUrl: "<%=SysPath%>Ajax/Extension/UploadSites.aspx?FormId=0&savePath=" + currentpath + "&fileName=",
                previewFileType: "any",
                allowedFileExtensions: [<%=AllowFileType %>],
                initialCaption: '<%="支持格式：".ToLang()%><%=EnableExtensionNames %>',
                previewClass: "bg-warning",
                initialPreviewAsData: true,
                initialPreviewFileType: 'other',
                noPicker: true
            }).on("filebatchselected", function(event, files) {
                $(this).fileinput("upload");
            }).on("fileuploaded", function(event, data) {
                if (data.response && data.response.Result == true) {
                    reload();
                } else {
                    whir.toastr.error(data.response.Msg);
                }
            });
        }

        function getTreeJson () {
            whir.ajax.post('<%= SysPath%>Handler/Extension/UploadSites.aspx', {
                data: {
                    _action: "GetFolderTree",
                    rootFolder: '<%=CurrentSiteDirection %>'
                },
                success: function(result) {
                    if (result.Status) {
                        whir.loading.remove();
                        bindTree(result.Message);
                    } else {
                        whir.toastr.warning(result.Message);
                        whir.loading.remove();
                    }
                }
            });
             whir.loading.remove();
        }        

        var $table = $('#Common_Table'),_that;
       
        //绑定文件夹树
        function bindTree (treeJson) {
            var defaultData =eval(treeJson);
            $('#folderList').treeview1({
                data: defaultData,
                multiSelect: $('#chk-select-multi').is(':checked'),
                onNodeSelected: function(event, node) {                                //切换文件夹
                    var path = '<%=CurrentSiteDirection %>' + node.href; 
                    if (path.indexOf("//") > 0)                                      //防止根目录多了//
                        path = path.substring(0, path.length - 1);
                    $("#txtPath").val(encodeURIComponent(path));
                    currentpath = encodeURIComponent(path);                                             //更改上传路径
                    RebuildUploadFile();                                           //重新生成上传控件
                    regeditUploadFile();                                           //注册上传控件
                    $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&path=" + encodeURIComponent(path) });
                     $("#showFolder").attr("title", path );
                    if (path.length > 25) {                                        //目录太多太长，剪短点
                        path = path.substring(0, 10) + "..." + path.substring(path.length - 10, path.length);
                    }
                    $("#showFolder").html(path );
                }
            });
        }


        //列表
        function initTable() {
            $table.bootstrapTable({
                url:"<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&path=<%=CurrentSiteDirection %>",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent:true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function() {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                    whir.loading.remove();
                },
                onPageChange: function () {
                    var path = $("#txtPath").val();
                    var key = $("#key").val();
                    if (key == "")
                    	$table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&path=" + path });
                    else
                        $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&key=" + key + "&path=" + path });
                },
                onColumnSwitch: function() {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function() {
                    
                },
               
                columns: [
                    {title: '<%="图标".ToLang()%>', type: 'Type',align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetIcon(value, row, index);}},
                    {title: '<%="文件名称".ToLang()%>', field: 'Name',align: 'left',valign: 'middle'},
                    {title: '<%="物理名称".ToLang()%>', field: 'RealName',align: 'left',valign: 'middle'},
                    {title: '<%="大小".ToLang()%>', field: 'Size',align: 'left',valign: 'middle'},
                    {title: '<%="操作".ToLang()%>',field: 'Op',width:200, align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetOperation(value, row, index);}},
                    {title: '<input type="checkbox" id="btSelectAll" />',field: 'Checkbox',width:40, align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetCheckbox(value, row, index);}}
                ],
                onDblClickRow: function (row) {
                    openDir(row);
                }

            });
        }
        //双击打开文件夹
        function openDir(row) {
            if (row.Type != "File") {

                $("#txtPath").val($("#txtPath").val()+ encodeURIComponent(row.RealName + "/"));        //防止中文文件夹 出现乱码 
                var path = decodeURIComponent($("#txtPath").val());
                currentpath = encodeURIComponent(path);            //更改上传路径
                RebuildUploadFile();                               //重新生成上传控件
                regeditUploadFile();                               //注册上传控件
                $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&path=" + encodeURIComponent(path) });
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
            $("#btSelectAll").next().click(function() {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem','*');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidChoose', 'btSelectItem');
                }
            });
             
            $("input[name=btSelectItem]").each(function() {
                $(this).next().click(function() {
                  $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem', '*'));
                });
            });
        }

        $(document).ready(function() {
            //注册上传控件
            regeditUploadFile();
            //绑定文件夹树
            getTreeJson();
            
            //注册回车事件
            document.onkeydown = function(e) {
                var ev = document.all ? window.event : e;
                if (ev.keyCode == 13 && $("#key").is(":focus")) {
                    search();
                } else if (ev.keyCode == 13 && $("#folderName").is(":focus")) {
                    addNewFolder();
                }
            };
        });
         
          <%--列表绑定 --%>
        //获取图标HTML
        function GetIcon(value, row, index) {
            var icon = "";
            if(row.Type!="File") {
                icon = "<img src='<%=SysPath %>res/images/ext/folder.gif'>";
            } else {
                icon = "<img src='<%=SysPath %>res/images/ext/"+row.Ext.substring(1,row.Ext.length)+".gif' onerror=\"this.src='<%=SysPath %>res/images/ext/unknow.gif'\">";
            }
            return icon;
        }
      

        //获取操作HTML
        function GetOperation(value, row, index) {
            var path = $("#txtPath").val();
            var html = "<div class='btn-group'  style='min-width:220px;'>";
            if(row.Type=="File") {
             <%if (IsCurrentRoleMenuRes("361"))
               { %>
                html += '<a class="btn btn-white" href="../../Module/Extension/Download.aspx?path=<%=CurrentSiteDirection %>&name=' + encodeURIComponent(row.FullName) + '" target="_blank"><%="下载".ToLang()%></a>';
            <%} %>
            }
            if('<%=AppSettingUtil.AppSettings["FileAllowEditType"]%>'.indexOf(row["Ext"])>=0) {
             <%if (IsDevUser) //原来菜单id368
               { %>
                html += '<a class="btn btn-white" href="<%=SysPath %>module/extension/file_edit.aspx?filepath=<%=CurrentSiteDirection %>' + encodeURIComponent(row.FullName) + '" target="_blank"><%="编辑文件".ToLang()%></a>';
              <%} %>
            }
            <%if (IsCurrentRoleMenuRes("360"))
               { %>
            html += '<a id="btnDel" class="btn text-danger border-normal" value="' + path + row.RealName + '"  onclick="del(\'' + row.FullName + '\')" ><%="删除".ToLang()%></a>';
            <%} %>
            html += "</div>";
            return html;
        }
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            if(row.Type=="File") {
                var html = '<input type="checkbox" name="btSelectItem" value="'+ row.FullName + '" />';
                return html;
            } else {
                return "";
            }
        }
       
        <%--按钮操作 --%>
        //刷新列表
        function reload() {
            var path=$("#txtPath").val();
            $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&path=" + path });
            whir.loading.remove();
        } 
         
       
        //创建文件夹
        function addNewFolder() {
            var folderName = $("#folderName").val();
            if (folderName == "" || addNewFolder == undefined) {
                whir.toastr.warning("<%="请输入文件夹名称".ToLang()%>");
                return false;
            }
            if (/\*|\//.test(folderName)) {
                whir.toastr.warning("<%="文件夹名称不能包含*、/ 等特殊符号".ToLang()%>");
                return false;
            }
            whir.ajax.post('<%= SysPath%>Handler/Extension/UploadSites.aspx', {
                data: {
                    _action: "AddNewFolder",
                    path: $("#txtPath").val(),
                    folderName: $("#folderName").val()
                },
                success: function (result) {
                    if (result.Status) {
                        whir.loading.remove();
                        getTreeJson();
                        reload();
                        whir.toastr.success(result.Message);
                    } else {
                        whir.toastr.warning(result.Message);
                    }
                }
            });
             whir.loading.remove();
        } 
        
        //搜索
        function search() {
            var path=$("#txtPath").val();
            var key=$("#key").val();
            $table.bootstrapTable('refresh', { url: "<%=SysPath%>Handler/Extension/UploadSites.aspx?_action=GetList&key="+key+"&path=" + path });
 
        } 

        //删除多条文件
        function delAll() {
            if($("#hidChoose").val()=="") {
                whir.toastr.warning("<%="请选择要删除的数据行！".ToLang()%>");
                return;
            }
            whir.dialog.confirm('<%="确定删除所选记录吗？".ToLang()%>', function() {
                whir.ajax.post('<%= SysPath%>Handler/Extension/UploadSites.aspx', {
                    data: {
                        _action: "Delete",
                        commandName: "CheckDelete",
                        hidChoose: $("#hidChoose").val()
                    },
                    success: function(result) {
                        if (result.Status) {
                            whir.loading.remove();
                            whir.toastr.success(result.Message);
                            reload();

                        } else {
                            whir.loading.remove();
                            whir.toastr.warning(result.Message);

                        }
                    }
                });
                whir.dialog.remove();
                 whir.loading.remove();
            });
        }

        //删除单条文件
        function del(value) {
            whir.dialog.confirm('<%="确认删除该文件/文件夹吗？".ToLang()%>', function() {
                whir.ajax.post('<%= SysPath%>Handler/Extension/UploadSites.aspx', {
                    data: {
                        _action: "Delete",
                        file: value,
                        commandName: "Delete"
                    },
                    success: function(result) {
                        if (result.Status) {
                            whir.loading.remove();
                            whir.toastr.success(result.Message);
                            reload();
                            getTreeJson();
                        } else {
                            whir.loading.remove();
                            whir.toastr.warning(result.Message);

                        }

                    },
                    error: function(response) {
                        whir.loading.remove();
                        whir.toastr.error(response.Message);

                    }                    
                });
                whir.dialog.remove();
                 whir.loading.remove();
            });
        }
         
    </script>
</asp:Content>

