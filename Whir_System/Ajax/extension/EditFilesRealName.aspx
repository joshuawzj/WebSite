<%@ Page Language="C#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Whir.Framework" %>

<script type="text/C#" runat="server">

    /// <summary>
    /// 页面加载, 异步处理请求, 上传文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            string newfile = RequestUtil.Instance.GetQueryString("newfile");
            string oldfile = RequestUtil.Instance.GetQueryString("oldfile");
            string path = RequestUtil.Instance.GetQueryString("path");

            if (File.Exists(path + "\\" + oldfile))
            {
                File.Move(path + "\\" + oldfile, path + "\\" + newfile);
                Response.Write("true");
            }
            else
            {
                Response.Write("编辑的文件不存在".ToLong());
            }
        }
        catch (Exception ex)
        {
          //  Response.StatusCode = 100;
            Response.Write(":" + ex.Message);//"An error occured" 
          
        }
        
    }

</script>