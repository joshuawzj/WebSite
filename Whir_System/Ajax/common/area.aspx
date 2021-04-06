<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Language" %>

<script type="text/C#" runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {

        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        int pid = RequestUtil.Instance.GetQueryInt("id", 0);
        int areaLevel = RequestUtil.Instance.GetQueryInt("arealevel", 3);

        LanguageType language = LanguageHelper.GetCurrentUseLanguage();        
        string cacheName = "AREA_{0}_{1}_{2}".FormatWith(language, pid, areaLevel);
        //CacheUtil.Instance.Remove(cacheName);
        string json = CacheUtil.Instance.GetCache(cacheName).ToStr();
        if (json.IsEmpty())
        {
            json = ServiceFactory.AreaService.GetJson(pid, areaLevel);
            CacheUtil.Instance.SetCache(cacheName, json);
        }
        
        Response.Write(json);
        Response.End();
    }
    
</script>