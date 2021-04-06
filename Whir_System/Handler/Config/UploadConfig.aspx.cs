using System;
using System.Linq;
using System.Web.Mail;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;
using System.Net;
using System.IO;
using System.Text;


public partial class Whir_System_Handler_Config_UploadConfig : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);

    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("339"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(UploadConfig);
        var model = ConfigHelper.GetUploadConfig() ?? ModelFactory<UploadConfig>.Insten();
        try
        {
            model = GetPostObject(type, model) as UploadConfig;
            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("UploadConfig.config"), type, model);
            if (model != null && model.AllowPicType.IndexOf("|||", StringComparison.Ordinal) > 0)
            {
                return new HandlerResult { Status = false, Message = "不可包含字符“||”".ToLang() };
            }

            if (model != null && model.AllowFileType.IndexOf("|||", StringComparison.Ordinal) > 0)
            {
                return new HandlerResult { Status = false, Message = "不可包含字符“||”".ToLang() };
            }

            SystemConfig sysSetting = ConfigHelper.GetSystemConfig();
            if (sysSetting.Editor == "ewebeditor")
            {
                string sFileName = AppName + "editor/eWebEditor/aspx/config.aspx";

                string styleName = "standard600";
                string[] oldStyleConfig = EWebEditorHelper.GetStyleConfig(sFileName, styleName);
                string[] newStyleConfig = EWebEditorHelper.GetStyleConfig(sFileName, styleName);
                newStyleConfig[3] = "../.." + UploadFilePath;
                newStyleConfig[6] = model.AllowFileType;
                newStyleConfig[8] = model.AllowPicType;
                newStyleConfig[13] = model.MaxPicSize.ToStr();
                newStyleConfig[11] = model.MaxFileSize.ToStr(); //单位kb
                newStyleConfig[78] = (model.MaxFileSize / 1024).ToStr(); //总文件大小 单位mb
                newStyleConfig[39] = model.AllowPicType;//处理图形扩展名
                newStyleConfig[68] = model.IsRename ? "0" : "1";//是否重命名
                newStyleConfig[71] = model.DirectoryType == "1" ? "{yyyy}/{mm}/" : "{yyyy}/{mm}/{dd}/";//是否重命名
                string mapPath = Server.MapPath(sFileName);
                EWebEditorHelper.ModifyStyle(sFileName, mapPath, oldStyleConfig, newStyleConfig);
            }

            //记录操作日志
            if (model != null)
            {
                ServiceFactory.OperateLogService.Save("修改，上传文件配置，上传方式【{0}】，允许图片类型【{1}】，上传图片最大值【{2}】，允许文件类型【{3}】，上传文件最大值【{4}】".FormatWith(
                        model.UpLoadType,
                        model.AllowPicType,
                        model.MaxPicSize,
                        model.AllowFileType,
                        model.MaxFileSize));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// FTP保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveFtp()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("339"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(UploadConfig);
        var model = ConfigHelper.GetUploadConfig() ?? ModelFactory<UploadConfig>.Insten();
        try
        {
            model = GetPostObject(type, model) as UploadConfig;
            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("UploadConfig.config"), type, model);
            //记录操作日志
            if (model != null)
            {
                ServiceFactory.OperateLogService.Save("修改，Ftp配置，Ftp地址【{0}】，Ftp用户名【{1}】".FormatWith(
                    model.FtpAddress,
                    model.FtpUserName));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }


    /// <summary>
    /// FTP测试
    /// </summary>
    /// <returns></returns>
    public HandlerResult Test()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("364"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(UploadConfig);
        var model = ConfigHelper.GetUploadConfig() ?? ModelFactory<UploadConfig>.Insten();
        model = GetPostObject(type, model) as UploadConfig;
        if (CheckDirectoryExist(model))
        {
            return new HandlerResult { Status = true, Message = "测试成功".ToLang() };
        }
        else
        {
            return new HandlerResult
            {
                Status = false,
                Message = "测试失败，请检查输入是否有误（测试依据：是否能连接、同时还检查站点根目录是否存在{0}文件夹）".ToLang().FormatWith(
                    AppSettingUtil.GetString("UploadFilePath").Trim('/'))
            };

        }
    }

    /// <summary>
    /// 检查目录是否存在,主要是看能否检查到根目录的upladfiles文件夹是否存在作为依据
    /// </summary>
    /// <returns>存在返回true，否则false</returns>
    protected bool CheckDirectoryExist(UploadConfig model)
    {
        bool result = false;
        StreamReader sr = null;
        FtpWebResponse response = null;
        try
        {
            string dirName = AppSettingUtil.GetString("UploadFilePath").Trim('/').ToLower();
            if (dirName == "")
            {
                return false;
            }
            string ftpAddress = model.FtpAddress.Trim();
            string ftpUserName = model.FtpUserName.Trim();
            string ftpPassword = model.FtpPassword;

            string path = string.Format(@"ftp://{0}", ftpAddress, dirName);
            //实例化FTP连接
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(path));
            //指定FTP操作类型为创建目录
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword); //提供账号密码的验证   
            //获取FTP服务器的响应
            response = (FtpWebResponse)request.GetResponse();
            // ReSharper disable once AssignNullToNotNullAttribute
            sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            var str = new StringBuilder();
            var line = sr.ReadLine();
            while (line != null)
            {
                str.Append(line);
                str.Append("|");
                line = sr.ReadLine();
            }
            var datas = str.ToString().Split('|');

            foreach (var t in datas)
            {
                if (!t.Contains("<DIR>")) continue;
                var index = t.IndexOf("<DIR>", StringComparison.Ordinal);
                var name = t.Substring(index + 5).Trim().ToLower();
                if (name != dirName) continue;
                result = true;
                break;
            }

        }
        catch (Exception ex)
        {
            // ignored
        }
        finally
        {
            if (null != sr)
            {
                sr.Close();
                sr.Dispose();
            }
            if (null != response)
                response.Close();
        }
        return result;
    }
}