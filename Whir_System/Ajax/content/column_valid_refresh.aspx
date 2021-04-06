<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<script type="text/C#" runat="server">

    /// <summary>
    /// 页面加载, 处理异步请求, 返回cookie值
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        
        string type = WebUtil.Instance.GetQueryString("type");
        string column_refresh_flag = CookieUtil.Instance.GetCookieValue("column_refresh_flag", "0");
        string subsite_refresh_flag = CookieUtil.Instance.GetCookieValue("subsite_refresh_flag", "0");
        string subject_refresh_flag = CookieUtil.Instance.GetCookieValue("subject_refresh_flag", "0");
        string ref_value = string.Empty;
        switch (type)
        {
            case "column":
                ref_value = column_refresh_flag;
                break;
            case "subsite":
                ref_value = subsite_refresh_flag;
                break;
            case "subject":
                ref_value = subject_refresh_flag;
                break;
            default:
                ref_value = string.Empty;
                break;
        }
        Response.Write(ref_value);
        Response.End();
    }
</script>
