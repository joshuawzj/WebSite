<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Jurisdiction_category.aspx.cs" Inherits="whir_system_module_security_Jurisdiction_category" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath %>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
        type="text/javascript"></script>
    <script type="text/javascript">

        var zNodes =<%=BindTree() %>;
        var setting = {
            check: {
                enable: true,
                chkboxType: { "Y": "ps", "N": "ps" } //Y:勾选（参数：p:影响父节点），N：不勾（参数s：影响子节点）[p 和 s 为参数]
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "Id",
                    pIdKey: "pId",
                    rootPId: 0
                }
            },
            callback: {
                onClick: OnClick

            },
            view: {
                dblClickExpand: false
            }
        };

        $(document).ready(function () {
            //取父级权限按钮的值
            var categoryids = window.parent.$("a[maincolumnid='<%=MainColumnid%>']").attr("selectCategory");
            $.fn.zTree.init($("#browser"), setting, zNodes);
            //设打勾
            var treeObj = $.fn.zTree.getZTreeObj("browser");
            var idsStr = categoryids;
            var ids = idsStr.split(',');
            for (var i = 0; i < ids.length; i++) {
                var id = ids[i];
                if (id == "") {
                    continue;
                }
                var node = treeObj.getNodeByParam("Id", id, null);
                if (node.children == undefined)
                    treeObj.checkNode(node, true, true);
            }

        });

        function OnClick(event, treeId, treeNode) {
            $.fn.zTree.getZTreeObj("browser").expandNode(treeNode);
        }

        function zTreeOnCheck(event, treeId, treeNode) {
        };

        //添加一个确定按钮, 点击后调用付页面的fill_txtDefaultTemp()函数
        $("[name=_dialog] .btn-primary", parent.document).click(function () {
            var nodes = $.fn.zTree.getZTreeObj("browser").getCheckedNodes(true);
            var treeids = "";

            for (var i = 0; i < nodes.length; i++) {
                treeids += nodes[i].Id + ",";
            }
            if ('<%= JsCallback %>' != '') {
                 window.parent.<%= JsCallback %>(treeids,<%=ColumnId %>,<%=MainColumnid %>,<%=SubjectId %>);
                 window.parent.whir.dialog.remove();
             }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <div style="overflow: auto;">
            <ul id="browser" class="ztree">
            </ul>
        </div>
    </div>
</asp:Content>
