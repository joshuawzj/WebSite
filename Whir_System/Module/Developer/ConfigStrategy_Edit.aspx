<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="configstrategy_edit.aspx.cs" Inherits="whir_system_module_developer_configstrategy_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <div class="mainbox">
            <dl class="title_column">
                <a href="configstrategy.aspx"><b>策略管理</b></a> <em class="line"></em>
                <asp:HyperLink ID="hlkEdit" runat="server" CssClass="aSelect" NavigateUrl="configstrategy_edit.aspx"><b>添加节点</b></asp:HyperLink>
            </dl>
            <div class="line_border">
            </div>
            <div class="All_table">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="item" width="80px">
                            配置名称：
                        </td>
                        <td>
                            <whir:DropDownList ID="ddConfigs" runat="server" AppendDataBoundItems="True" ErrorCss="form_error" TipCss="form_tip" Required="true"
                                InitialValue="0" RequiredErrorMessage="请选择配置名称">
                                <asp:ListItem Value="0">==选择配置模块==</asp:ListItem>
                            </whir:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="item" width="80px">
                            键名：
                        </td>
                        <td>
                            <whir:TextBox ID="txtKeyName" runat="server" CssClass="text_common" Width="150px"
                          ErrorCss="form_error" TipCss="form_tip" Required="True" MinLength="1" MaxLength="64" Message="1-64字符,任意字符组合" RequiredErrorMessage="请输入键名"></whir:TextBox>
                          <asp:LinkButton ID="lbtnGetNewValue" runat="server" 
                                onclick="lbtnGetNewValue_Click">获取当前值</asp:LinkButton>
                        </td>
                    </tr>
                    
                    <tr>
                    
                        <td class="item" width="80px" runat="server"  id="tdKeyOne" >
                            键值：
                        </td>
                        <td runat="server" id="tdKeyTwo"  >
                           <%-- <whir:TextBox Id="txtKeyValue" runat="server" CssClass="text_common" Width="300px"
                          ErrorCss="form_error" TipCss="form_tip" Required="True" MinLength="1" MaxLength="1000" Message="请输入键值" RequiredErrorMessage="1-1000字符,任意字符组合"></whir:TextBox>--%>
                          <whir:TextBox ID="txtKeyValue" runat="server" CssClass="text_common" Width="300px"
                          ErrorCss="form_error" TipCss="form_tip"   MaxLength="1000" Message="请输入键值" RequiredErrorMessage="1-1000字符,任意字符组合"></whir:TextBox>
                         <span id="spInfo" runat="server">请点击“获取当前值”</span> 
                        </td>
                    </tr>
                    <tr>
                        <td class="item" width="80px">
                            说明：
                        </td>
                        <td>
                            <whir:TextBox ID="txtDescription" runat="server" CssClass="text_common" Width="300px"
                          ErrorCss="form_error" TipCss="form_tip"  MaxLength="1000" Message="请输入说明" RequiredErrorMessage="2-1000字符,任意字符组合"></whir:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="button_submit_div">
                    <asp:LinkButton ID="lbtnSubmit" runat="server" CssClass="aLink" OnCommand="Save_Command"
                        CommandArgument="Save" > <em><img src="<%=SysPath%>res/images/button_submit_icon_1.gif" /></em><b>提交并返回</b> </asp:LinkButton>
                        <asp:LinkButton ID="lbtnSubmitContinue" runat="server" CssClass="aLink" OnCommand="Save_Command"
                        CommandArgument="SaveContinue"> <em><img src="<%=SysPath%>res/images/button_submit_icon_2.gif" /></em><b>提交并继续</b> </asp:LinkButton>
                    <a class="aBack" href="configstrategy.aspx?id=<%=ConfigSettingId %>&showselectedid=<%=ShowSelectedId %>"><b>返回</b></a>
                </div>
            </div>
        </div>


</asp:Content>
