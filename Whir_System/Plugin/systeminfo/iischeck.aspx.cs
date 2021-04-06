/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：iischeck.aspx.cs
 * 文件描述：系统信息页面
 */
using System;
using System.Drawing;
using System.IO;
using System.Text;
using Whir.Framework;

using Whir.Language;

public partial class whir_system_iischeck :Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("18"));
        local();
    }

    /// <summary>
    /// 获取系统信息
    /// </summary>
    private void local()
    {
        Label_QX.Text = "未知".ToLang();
        btnCheckWrite.Text = "检查".ToLang();

        try
        {
            this.Label_ServerName.Text = base.Server.MachineName;
            this.Label_OS.Text = Environment.OSVersion.ToString();
            this.Label_ServerIP.Text = base.Request.ServerVariables["LOCAL_ADDR"];
            this.Label_ServerDomain.Text = base.Request.ServerVariables["SERVER_NAME"];
            this.Label_ScriptTimeout.Text = base.Server.ScriptTimeout.ToString();
            this.Label_now.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Label_SessionCount.Text = this.Session.Contents.Count.ToString();
            this.Label_ApplicationCount.Text = base.Application.Contents.Count.ToString();
            this.Label_IISVER.Text = base.Request.ServerVariables["SERVER_SOFTWARE"];
            this.Label_framework.Text = Environment.Version.ToString();
            this.Label_XDLJ.Text = base.Request.ServerVariables["PATH_INFO"];
            this.Label_WLLJ.Text = base.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            decimal r = (Environment.TickCount/600)/60;
            this.Label_ServerRunTime.Text = (Math.Round(r)/100M).ToString();
        }
        catch (Exception ex)
        {
            ServiceInfo.Visible = false;
            notData.Visible = true;
        }

        string isObject = "";
        string objectVer = "";
        this.CheckObject("ADODB.RecordSet", out isObject, out objectVer);
        this.Label_AccessObject.Text = isObject + " " + objectVer;
        this.CheckObject("Scripting.FileSystemObject", out isObject, out objectVer);
        this.Label_FSO.Text = isObject + " " + objectVer;
        this.CheckObject("JMail.SmtpMail", out isObject, out objectVer);
        this.Label_JMAIL.Text = isObject + " " + objectVer;
        this.CheckObject("CDONTS.NewMail", out isObject, out objectVer);
        this.Label_CDONTS.Text = isObject + " " + objectVer;
        this.CheckObject("Persits.Jpeg", out isObject, out objectVer);
        this.Label_AspJpeg.Text = isObject + " " + objectVer;
        this.CheckObject("Persits.Upload.1", out isObject, out objectVer);
        this.Label_ASPUpload.Text = isObject + " " + objectVer;
    }
    /// <summary>
    /// 空间是否支持写入
    /// </summary>
    protected void btnCheckWrite_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsCurrentRoleMenuRes("370"))
            {
                string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            }
            else
            {
                StreamWriter write = new StreamWriter(base.Server.MapPath("iischeck.htm"), false, Encoding.GetEncoding("gb2312"));
                write.WriteLine(DateTime.Now);
                write.Close();
                File.Delete(base.Server.MapPath("iischeck.htm"));
                this.Label_QX.Text = "√支持".ToLang();
                this.Label_QX.ForeColor = Color.Green;
            }
        }
        catch
        {
            this.Label_QX.Text = "×不支持".ToLang();
            this.Label_QX.ForeColor = Color.Red;
        }
    }
    /// <summary>
    /// 检查自定义组件
    /// </summary>
    /// <param name="progID"></param>
    private void CheckObject(string progID, out string isObject, out string objectVer)
    {
        try
        {
            object obj = base.Server.CreateObject(progID);
            isObject = "<font color=\"green\">"+"√支持".ToLang()+"</font>";
            objectVer = "";
        }
        catch
        {
            isObject = "<font color=\"red\">"+"×不支持".ToLang()+"</b>";
            objectVer = "";
        }
    }
}