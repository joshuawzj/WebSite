<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Language" %>

<script type="text/C#" runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string info = WebUtil.Instance.GetQueryString("msg");

        Response.Write(info.ToLang());
        Response.End();
    }
    
</script>