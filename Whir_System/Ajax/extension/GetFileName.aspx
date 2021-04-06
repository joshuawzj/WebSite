<%@ Page Language="C#" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.ezEIP.Web" %>
<script type="text/C#" runat="server">
    //通过文件真名返回上传前的名字
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        string FileName = RequestUtil.Instance.GetString("filename");
        //if (FileName.ToStr()!="")
        //{
        //    FileName = FileName.Replace("\\", "/");
        //    if (FileName.Contains("/"))
        //    {
        //        FileName = FileName.Substring(FileName.LastIndexOf('/') + 1);
        //    }
        //     Response.Write(ServiceFactory.UploadService.GetFileName(FileName)); 
        //}
        Response.Write(ServiceFactory.UploadService.GetFileName(FileName)); 
        
    }
 
</script>