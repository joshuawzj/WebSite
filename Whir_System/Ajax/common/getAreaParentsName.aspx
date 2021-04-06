<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>

<script type="text/C#" runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        int areaID = RequestUtil.Instance.GetQueryInt("id", 0);
        string parentsName = ServiceFactory.AreaService.GetParentsName(areaID);
        Response.Write(parentsName);
        Response.End();
    }

</script>