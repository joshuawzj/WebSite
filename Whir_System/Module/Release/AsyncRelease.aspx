<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="AsyncRelease.aspx.cs" Inherits="Whir_System_Module_Release_AsyncRelease" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
        type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>

</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
       <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <div class="row">
                    <div id="paneltres" class="panel-body col-xs-4">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                               栏目
                            </div>
                            <div class="panel-body">
                                <div class="tab-content">
                                          <ul id='columnTree<%=CurrentSiteId %>' class="ztree">
                                                </ul>
                                      <asp:Literal ID="litScript" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="panelcontent" class="panel-body col-xs-8">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                静态发布参数设置
                            </div>
                            <div class="panel-body">
                                <div class="form_center">
                                    <div class="form-group">
                                        <div class="col-md-10 ">
                                           
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

        //勾选
        function onClick(e, treeId, treeNode) {
            var columnid = treeNode
        }

    </script>
</asp:content>

