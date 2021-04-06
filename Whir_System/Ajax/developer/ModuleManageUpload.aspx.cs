/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：ModuleManage_edit.aspx.cs
 * 文件描述： 添加模块
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Whir.Service;
using Whir.Framework;
using Whir.Config.Models;
using Whir.Domain;
using Whir.Language;
public partial class Whir_System_ajax_developer_ModuleManageUpload : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Files.Count > 0)
        {
            HttpPostedFile uploadPostedFile = Request.Files[0];
            string savePath = RequestUtil.Instance.GetQueryString("savePath");
            string Folder = DateTime.Now.ToString("yyyyMMddHHmmssff");
            savePath = Server.MapPath(savePath);
            //模块上传的文件夹
            string FolderPath = savePath + Folder + "/";

            FileUtil.Instance.CreateDirectory(FolderPath);//在uploadfiles/ModuleTemp创建文件夹
            string saveFileName = ServiceFactory.UploadFilesService.UploadFile(uploadPostedFile, FolderPath);

            string Msg = ServiceFactory.PluginService.UploadPlugin(FolderPath, FolderPath + saveFileName, saveFileName);
            JsonObject jsonObject = new JsonObject();
            jsonObject.Result = true;
            jsonObject.Msg = Msg;
            Response.Write(jsonObject.ToJson());
        }
    }
}

public class JsonObject
{
    public bool Result { get; set; }
    public string Msg { get; set; }
}