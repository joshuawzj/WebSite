<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>

<script type="text/C#" runat="server">

    /// <summary>
    /// 页面加载, 处理异步请求, 根据菜单ID修改菜单排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            int menuID = RequestUtil.Instance.GetQueryInt("menuid", 0);
            int sort = RequestUtil.Instance.GetQueryInt("sort", 0);

            ServiceFactory.MenuService.ModifySort(menuID, sort);
        }
        catch (Exception ex)
        {
            Response.Write(Server.UrlEncode(ex.Message));
        }
        Response.Write("ok");
    }
    
</script>