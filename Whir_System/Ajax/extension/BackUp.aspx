<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Framework" %>

<script type="text/C#" runat="server">
    /// <summary>
    /// 页面加载, 异步处理请求,备份数据库
    /// 判断当前下载数据是否存在
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            int backupid = RequestUtil.Instance.GetQueryInt("backupid", 0);

            Backup model = ServiceFactory.BackupService.SingleOrDefault<Backup>(backupid);

            if (model != null)
            {
                string appName = new Whir.ezEIP.Web.SysManagePageBase().AppName;
                EnumBackupMsg result = ServiceFactory.BackupService.DownloadData(appName+model.BackupPath);

                if (result == EnumBackupMsg.DownloadSuccess)
                     Response.Write(appName+model.BackupPath);
                else
                    Response.Write("no");
            }
            else
                Response.Write("no");
        }
        catch (Exception ex)
        {
            Response.Write("no");
        }
    }
</script>
