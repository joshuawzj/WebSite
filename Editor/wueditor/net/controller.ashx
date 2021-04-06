<%@ WebHandler Language="C#" Class="UEditorHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Whir.Config.Models;

public class UEditorHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //ÅÐ¶ÏÊÇ·ñµÇÂ¼ÁË
        PictureConfig pictureConfig = Whir.Config.ConfigHelper.GetPictureConfig();
        UploadConfig uploadConfig = Whir.Config.ConfigHelper.GetUploadConfig();
        var allowPicExtensions = ("." + uploadConfig.AllowPicType.Replace("|", "|.")).Split('|');
        var allowFileExtensions = ("." + uploadConfig.AllowFileType.Replace("|", "|.")).Split('|');
              
        Handler action = null;
        switch (context.Request["action"])
        {
            case "config":
                action = new ConfigHandler(context);
                break;
            case "uploadimage":
                action = new UploadHandler(context, new EditorUploadConfig()
                {
                    Type="image",
                    AllowExtensions =allowPicExtensions,
                    PathFormat = Config.GetString("imagePathFormat"),
                    SizeLimit = uploadConfig.MaxPicSize * 1024.0,
                    UploadFieldName = Config.GetString("imageFieldName")
                });
                break;
            case "uploadscrawl":
                action = new UploadHandler(context, new EditorUploadConfig()
                {
                    Type = "image",
                    AllowExtensions = new string[] { ".png" },
                    PathFormat = Config.GetString("scrawlPathFormat"),
                    SizeLimit = uploadConfig.MaxPicSize * 1024.0,
                    UploadFieldName = Config.GetString("scrawlFieldName"),
                    Base64 = true,
                    Base64Filename = "scrawl.png"
                });
                break;
            case "uploadvideo":
                action = new UploadHandler(context, new EditorUploadConfig()
                {
                    Type = "file",
                    AllowExtensions =allowFileExtensions,
                    PathFormat = Config.GetString("videoPathFormat"),
                    SizeLimit = uploadConfig.MaxFileSize * 1024.0,
                    UploadFieldName = Config.GetString("videoFieldName")
                });
                break;
            case "uploadfile":
                action = new UploadHandler(context, new EditorUploadConfig()
                {
                    Type = "file",
                    AllowExtensions = allowFileExtensions,
                    PathFormat = Config.GetString("filePathFormat"),
                    SizeLimit = uploadConfig.MaxFileSize * 1024.0,
                    UploadFieldName = Config.GetString("fileFieldName")
                });
                break;
            case "listimage":
                action = new ListFileManager(context, new Whir.ezEIP.Web.SysManagePageBase().UploadFilePath, allowPicExtensions);
                break;
            case "listfile":
                 action = new ListFileManager(context, new Whir.ezEIP.Web.SysManagePageBase().UploadFilePath, allowFileExtensions);
                break;
            case "catchimage":
                action = new CrawlerHandler(context);
                break;
            default:
                action = new NotSupportedHandler(context);
                break;
        }
        action.Process();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}