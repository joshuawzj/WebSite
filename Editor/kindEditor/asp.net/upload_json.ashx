<%@ webhandler Language="C#" class="Upload"%>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using System.Linq;

using LitJson;

using Whir.Framework;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;
using Whir.Service;
 

public class Upload : IHttpHandler
{
	private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        this.context = context;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];

        if (imgFile.ContentLength <= 0) showError("请选择文件".ToLang());

        String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);
        //文件保存目录路径
        String uploadFilePath = WebUtil.Instance.AppPath() + AppSettingUtil.AppSettings["UploadFilePath"];
        string uploadFileDir = context.Server.MapPath(uploadFilePath);

        //定义允许上传的文件扩展名
        Hashtable extTable = getAllowUploadType();

        String dirName = context.Request.QueryString["dir"] ?? "image";
        if (!extTable.ContainsKey(dirName)) showError("目录名不正确".ToLang());

        string saveFileName = "";
        try
        {
            if (dirName == "image")
            {
                //做为图片上传
                PictureConfig picConfig = ConfigHelper.GetPictureConfig();
                saveFileName = ServiceFactory.UploadFilesService.UploadImageBeDateDir(imgFile, uploadFileDir, uploadFilePath, picConfig.IsAutoMakeWatermark);
            }
            else
            {
                //做为文件上传
                saveFileName = ServiceFactory.UploadFilesService.UploadFileBeDateDir(imgFile, uploadFileDir,null);
            }
            //上传文件名称和物理名称到数据库
            Whir.Domain.Upload upload = new Whir.Domain.Upload();
            upload.Name = imgFile.FileName;
            upload.RealName = saveFileName.Substring(saveFileName.LastIndexOf('/') + 1); ;
            upload.Path = saveFileName.Replace(uploadFilePath, "").Replace("\\", "/");
            upload.IsDel = false;
            upload.State = 0;
            upload.CreateDate = DateTime.Now;
            upload.UpdateDate = DateTime.Now;
            upload.CreateUser = Whir.ezEIP.Web.SysManagePageBase.CurrentUserName;
            upload.UpdateUser = upload.CreateUser;
            Whir.Service.ServiceFactory.UploadService.Save(upload);
        }
        catch (Exception ex)
        {
            showError(ex.Message);
        }
        String fileUrl = uploadFilePath + saveFileName;

        Hashtable hash = new Hashtable();
        hash["error"] = 0;
        hash["url"] = fileUrl;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
		hash["error"] = 1;
		hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

    //获取可上传的文件格式哈希表
    private Hashtable getAllowUploadType()
    {
        Hashtable extTable = new Hashtable();
        UploadConfig config = ConfigHelper.GetUploadConfig();
        string[] allowPicType = config.AllowPicType.Split('|').Where(p => !p.IsEmpty()).ToArray();
        string[] allowFileType = config.AllowFileType.Split('|').Where(p => !p.IsEmpty()).ToArray();

        string tempPicType = "";
        foreach (string picType in allowPicType)
        {
            tempPicType += picType + ",";
        }
        extTable.Add("image", tempPicType.TrimEnd(','));

        string tempFileType = "";
        foreach (string fileType in allowFileType)
        {
            tempFileType += fileType + ",";
        }
        extTable.Add("file", tempFileType.TrimEnd(','));
        
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        return extTable;
    }

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}
