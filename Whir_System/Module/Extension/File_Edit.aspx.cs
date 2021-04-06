/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：file_edit.aspx.cs
* 文件描述：文件在线编辑页 
*/
using System;
using System.IO;
using System.Linq;
using System.Text;

using Whir.Framework;
using Whir.Service;
using Whir.Language;
using System.Text.RegularExpressions;

public partial class whir_system_module_extension_file_edit : Whir.ezEIP.Web.SysManagePageBase
{
    // 文件相对路径，如：Uploadfiles/aa.txt 或 web.config（网站根目录下的aa.txt）
    public string FilePath
    {
        get;
        set;
    }
    /// <summary>
    /// 文件内容，用来判断是否有改变
    /// </summary>
    protected string PreFileContent
    {
        get { return ViewState["PreFileContent"].ToStr(); }
        set { ViewState["PreFileContent"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsDevUser) //只允许root角色查看该页面 
        {
            string script = "<script language=\"javascript\" defer=\"defer\">TipMessage('本页面只允许开发者角色查看！'); </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            return;
        }
        FilePath = RequestUtil.Instance.GetString("FilePath");
        if (!Server.MapPath(FilePath).Contains(Server.MapPath(CurrentSiteDirection).Trim('\\')))
        {
            btnSave.Visible = false;

        }
        if (Server.MapPath(FilePath).ToLower().Contains(Server.MapPath(SysPath).ToLower()))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">TipMessage('试图修改非法路径下的文件！'); </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            return;
        }

        if (!IsPostBack)
        {
            if (FilePath.IsEmpty() || !ValidatorPath())//文件不存在，提示并关闭
            {
                return;
            }
            string content = GetContent();//文件内容
            PreFileContent = content;
            //输出编辑内容
            EditorContent.Value = PreFileContent;
            lblPath.Text = FilePath;
        }
    }

    /// <summary>
    /// 验证地址是否正确
    /// </summary>
    /// <returns></returns>
    private bool ValidatorPath()
    {
        bool isSuccess = false;
        try//未映射地址
        {
            string path = Server.MapPath(FilePath);
            if (File.Exists(path))
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            if (!AppSettingUtil.GetString("FileAllowEditType").Split('|').Contains(Path.GetExtension(path)))
                isSuccess = false;

        }
        catch
        {
            isSuccess = false;
        }
        if (!isSuccess)
        {
            string message = Regex.Replace("文件不存在或者该文件不允许查看、编辑".ToLang(), @"\s+", " ");
            string script = "<script language=\"javascript\" defer=\"defer\">TipMessage('" + message.Replace("'", "\\'") + "'); </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }


        return isSuccess;
    }

    /// <summary>
    /// 获取文件内容信息
    /// </summary>
    /// <param name="filePath">文件地址</param>
    /// <returns>文件内容，null时文件不存在</returns>
    protected string GetContent()
    {
        string path = Server.MapPath(FilePath);
        if (File.Exists(path))
        {
            return FileSystemHelper.ReadFile(path);
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 保存编辑内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsDevUser) //只允许root角色查看该页面 
        {
            string script = "<script language=\"javascript\" defer=\"defer\">TipMessage('本操作只允许开发者角色操作！');</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            return;
        }
        if (!ValidatorPath())//文件不存在，提示并关闭
        {
            return;
        }
        //文件绝对路径
        string path = Server.MapPath(FilePath);
        string backFile = "";
        //编辑后的内容
        string content = EditorContent.Value;
        if (PreFileContent == content)
        {
            Alert("文件内容没有改变，不需要保存".ToLang());
            return;
        }
        else
        {
            backFile = path + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bak";
            File.Copy(path, backFile, false);
        }
        bool isSuccess = WriteFile(path, content);
        Whir.Service.ServiceFactory.OperateLogService.Save(LogType.TemplateLog, string.Format("添加，编辑文件【{0}】", path));

        if (isSuccess)
        {
            //写入操作日志
            ServiceFactory.OperateLogService.Save("修改文件：{0}".FormatWith(path));
            Alert("保存成功".ToLang(), true);
        }
        else
        {
            File.Delete(backFile);
            ErrorAlert("保存失败".ToLang());
        }
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="file">文件名</param>
    /// <param name="fileContent">文件内容</param>    
    private bool WriteFile(string file, string fileContent)
    {
        bool isSuccess = true;
        //向文件写入内容
        try
        {
            FileSystemHelper.WriteFile(file, fileContent);
        }
        catch (Exception exception)
        {
            isSuccess = false;
        }
        return isSuccess;
    }
}