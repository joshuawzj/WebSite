<%@ Application Language="C#" %>
<%@ Import Namespace="Whir.Framework" %>

<script RunAt="server">

    private void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码
    }

    private void Application_BeginRequest(object sender, EventArgs e)
    {

        //HttpContext.Current.Response.Headers.Remove("Server");
        string reqPath = Request.Path.ToLower();
        string appName = WebUtil.Instance.AppPath();
        string sysPath = (appName + AppSettingUtil.AppSettings["SystemPath"]).ToLower();

        if (reqPath.Contains(sysPath) || reqPath.Contains("Ajax/Login.ashx".ToLower()))
        {
            //if (reqPath.ToLower().Contains("Login.aspx".ToLower()) || reqPath.ToLower().Contains("Main.aspx".ToLower()) || reqPath.ToLower().Contains("RegKey.aspx".ToLower()))
            //{

            //}
            //else if (Request.ServerVariables["HTTP_REFERER"] == null || !Request.ServerVariables["HTTP_REFERER"].ToString().Contains(Request.ServerVariables["HTTP_HOST"].ToString()))
            //{
            //    Server.Transfer(sysPath + "Login.aspx");
            //}
            //Safe360ForSystem.Procress();
        }
        if (!reqPath.Contains(sysPath)&&!reqPath.EndsWith("error.aspx")&&!reqPath.EndsWith("404.aspx"))
        {
            Safe360.Procress();
        }

        #region 伪静态处理 迁移到404页面处理
        //if (reqPath.EndsWith(".html")&&!reqPath.Contains(sysPath))//要排除 编辑器的页面
        //{
        //    string createType = "";
        //    int createID = 0;
        //    bool canCreateHtml;
        //    string aspxPath = Whir.Label.Static.StaticLabelHelper.Instance.GetAspxPath(reqPath, out createType, out createID, out canCreateHtml);
        //    Server.Transfer(aspxPath);
        //    return;
        //}
        #endregion

        #region 生成静态页 迁移到404页面处理
        //if (reqPath.EndsWith(".html"))
        //{
        //    string reqDir = Server.MapPath(reqPath);
        //    if (!FileSystemHelper.IsFieldExist(reqDir))
        //    {
        //        if (!reqPath.ToLower().Contains(sysPath))
        //        {
        //            if (!reqPath.IsEmpty())
        //            {
        //                bool writeSuccess = StaticLabelHelper.Instance.Build(reqPath);

        //                if (writeSuccess)
        //                {
        //                    Response.Redirect(reqPath);
        //                    return;
        //                }
        //                Response.Redirect(appName + "404.aspx");
        //            }
        //        }
        //    }
        //}
        #endregion


    }

    private void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    private void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码
        HttpException ex = Server.GetLastError() as HttpException;
        if (ex != null)
        {
            LogHelper.Log(ex);

            var msg = "";
            //EnableErrorPage在web.config配置 0则显示错误详情 1则显示基本错误信息 默认是0（测试站显示错误详情），上线授权后变成1（正式站显示基本错误信息）
            if (!AppSettingUtil.AppSettings["EnableErrorPage"].ToBoolean())
                msg = HttpUtility.UrlEncode(ex.InnerException == null ? ex.Message : ex.InnerException.Message + " " + ex.InnerException.StackTrace);
            else
                msg = HttpUtility.UrlEncode(ex.InnerException == null ? ex.Message : ex.InnerException.Message + " " + ex.Message);

            Response.StatusCode = ex.GetHttpCode();
            if (new int[] { 400, 403, 404 }.Contains(Response.StatusCode))
                Server.TransferRequest("~/404.aspx?aspxerrorpath=" + Request.Path.ToLower(), true);
            else
                Server.TransferRequest("~/Error.aspx?Code=" + ex.GetHttpCode() + "&Msg=" + msg, true);

            Server.ClearError();
        }
    }


    private void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码
    }

    private void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。
    }

</script>
