<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.ezEIP.Web" %>
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
            int gatherID = WebUtil.Instance.GetQueryInt("gatherid", 0);
            long sort = WebUtil.Instance.GetQueryString("sort").ToLong();

            ServiceFactory.GatherService.ModifySort(gatherID, sort);
        }
        catch (Exception ex)
        {
            Response.Write(Server.UrlEncode(ex.Message));
        }
        Response.Write("ok");
    }
    
</script>
