<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="configstrategy.aspx.cs" Inherits="whir_system_module_developer_configstrategy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath %>res/css/css_whir.css" rel="stylesheet" type="text/css" />
    <link href="<%=SysPath %>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
        type="text/javascript"></script>
    <script type="text/javascript">
		<!--
        var zNodes=<%=BindTree() %>;
      var showselectedid="<%=ShowSelectedId %>";
        var setting = {
            check: {
                enable: false,  //不显示 checkbox 
                chkboxType:{ "Y" : "s", "N" : "s" }
            },
            data: {
                simpleData: {
                    enable: true,    
                    idKey:"id",
                    pIdKey: "pId",
                    rootPId:0
                }
            },
            callback: {
                onClick:zTreeOnClick
            }
        };
       
        $(function() {
            $.fn.zTree.init($("#browser"), setting, zNodes);

              var treeObj=$.fn.zTree.init($("#browser"), setting, zNodes);
            treeObj.selectNode(treeObj.getNodeByTId(showselectedid));
        });

             function zTreeOnClick(event, treeId, treeNode) {
                var nodes = $.fn.zTree.getZTreeObj("browser").getSelectedNodes();
           var id=nodes[0].id;
           var parentId=nodes[0].pId;
          

           //根据 whir_system/Config/ConfigStrategy.config 中的配置，父节点，因此没有任何的链接,也就是 parentId 为0的都是父节点。
           if(parentId!=0)
           {
                //Url传递 ConfigSettingId
                window.location="configstrategy.aspx?id="+nodes[0].id+"&showselectedid="+treeNode.tId;
           }
           
           };

		//-->
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <dl class="title_column">
            <a href="configstrategy.aspx" class="aSelect"><b>策略管理</b></a> <em class="line"></em>
            <a href="configstrategy_edit.aspx?id=<%=ConfigSettingId %>&showselectedid=<%=ShowSelectedId %>"><b>添加节点</b></a>
        </dl>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="box_common" valign="top" width="180px">
                    <!--menutree-->
                    <div style="overflow: auto;">
                        <ul id="browser" class="ztree" >
                        </ul>
                    </div>
                    <!--menutree-->
                </td>
                <td width="5px">
                </td>
                <td class="box_common" valign="top">
                    <div class="All_list">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <th width="180px">
                                    键名
                                </th>
                                <th width="400px" >
                                    键值
                                    <asp:ImageButton ID="imgbtnRef" 
                                        runat="server"  ImageUrl="../../res/images/refurbish.jpg"
                                        onclick="imgbtnRef_Click" Height="16px" ToolTip="刷新键值" />
                                </th>
                                <th>
                                    说明
                                </th>
                                <th width="35px">
                                    操作
                                </th>
                            </tr>
                            <asp:Repeater ID="rptConfigs" runat="server" OnItemCommand="rptConfigs_ItemCommand"
                                OnItemDataBound="rptConfigs_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# Eval("ConfigKey")%>
                                        </td>
                                        <td style="wrap:break-word;word-break:break-all;">
                                            <%# Eval("ConfigValue")%>
                                        </td>
                                        <td>
                                            <%# Eval("ConfigDescription")%>
                                        </td>
                                        <td width="80px">
                                            <a href='configstrategy_edit.aspx?configstrategyid=<%#Eval("ConfigStrategyId") %>&configsettingid=<%#Eval("ConfigSettingId") %>&showselectedid=<%=ShowSelectedId %>'>
                                                编辑</a>
                                            <asp:LinkButton ID="lbtnDel" runat="server" CommandName="del" CommandArgument='<%# Eval("ConfigStrategyId") %>'>删除</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                    </div>
                </td>
            </tr>
        </table>     
    </div>
</asp:Content>
