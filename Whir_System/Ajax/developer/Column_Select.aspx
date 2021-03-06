<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Column_Select.aspx.cs" Inherits="Whir_System_Ajax_Developer_Column_Select" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
        type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document).click(function() {
                var selected = $('#hidSelectedColumn').val();
                if (selected.length < 5) {
                    TipMessage('<%="请选择".ToLang() %>');
                    return false;
                } else {
                    window.parent.<%= Callback %>(selected);
                    return true;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body" style="height: 100%">
                <div class="form_center" style="width: 100%">
                    <div >
                           <ul class="nav nav-tabs" role="tablist">
                            
                                  <li class="active"><a data-toggle="tab" href="#tab-0" aria-expanded="true"><%="会员系统".ToLang()%></a></li>
                                   <asp:Repeater ID="rptMultiSite" runat="server">
                                   <ItemTemplate>
                                  <li class=""><a data-toggle="tab" href="#tab-<%# Eval("SiteId") %>" aria-expanded="false"> <%# Eval("SiteName").ToLang() %></a></li>
                                  </ItemTemplate>
                                 </asp:Repeater>

                                 
                            </ul>
                        
                        <div id="paneltres" class="panel-body  col-xs-6"  >
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="栏目".ToLang()%></div>
                                <div class="panel-body">
                                    <div class="tab-content">
                                        <div id="tab-0" class="tab-pane active">
                                            <ul id='columnTree-1' class="ztree">
                                            </ul>
                                            <script type='text/javascript'>
                                                $(function() {
                                                    whir.ztree.column(
                                                        'columnTree-1',
                                                        [{ name: '会员系统', id: '-1|0', open: true }],
                                                        onCheck,
                                                        { enable: true, chkStyle: 'radio', radioType: 'all' });
                                                });
                                            </script>
                                        </div>
                                        <asp:Repeater ID="rptMultiSiteColumn" runat="server">
                                            <ItemTemplate>
                                                <div id="tab-<%# Eval("SiteId") %>" class="tab-pane">
                                                    <ul id='columnTree<%# Eval("SiteId") %>' class="ztree">
                                                    </ul>
                                                </div>
                                            <script>
                                                $(function(){ 
                                                      whir.ztree.area("columnTree<%# Eval("SiteId") %>", 
                                                    "<%=SysPath%>Ajax/common/GetColumnSelect.aspx?siteId=<%# Eval("SiteId") %>&columnId=<%=ColumnId%>&subjectid=<%=SubjectId%>&isAll=1&selectedtype=<%=Whir.Framework. RequestUtil.Instance.GetQueryString("selectedtype")%>",
                                                      { enable: true, chkStyle: 'radio', radioType: 'all'},onCheck);                                                
                                                });
                                            </script>
                                            
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="panelcontent" class="panel-body  col-xs-6"  >
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="已选栏目：".ToLang()%></div>
                                <div class="panel-body">
                                    <div class="form_center">
                                        <div class="form-group">
                                            <div class="col-md-10 ">
                                                <ul id="ulSelectedColumn">
                                                </ul>
                                                <input type="hidden" id="hidSelectedColumn" name="hidSelectedColumn" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
       // $("#paneltres").height($("#panelcontent").height());
        //勾选
        function onCheck(e, treeId, treeNode) {
            var jsonStr = whir.ztree.getSelectedJson(treeId);
            var json = eval("(" + jsonStr + ")");
            var innerHtml = "";
            $(json).each(function (idx, item) {
                var id = item.id;
                var tid = item.tid;
                var name = item.name;
                var subjectId = item.subjectID;
                var cId = id.split('|')[0];
                var sId = id.split('|')[1];
                innerHtml += '<li>' + name + '[' + cId + ']' + '[' + sId + ']' + '</li>';
            });
            $("#hidSelectedColumn").val(jsonStr);
            $("#ulSelectedColumn").html(innerHtml);
        }

    </script>
</asp:Content>
