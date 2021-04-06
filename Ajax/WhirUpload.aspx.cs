using System;
using System.Web;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class Ajax_WhirUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var UploadFilePath = new FrontUserControl().UploadFilePath;
        try
        {
            int formId = RequestUtil.Instance.GetQueryInt("formid", 0);
            bool isPic = RequestUtil.Instance.GetQueryInt("isPic", 0).ToBoolean();
            string fileName = RequestUtil.Instance.GetString("image");
            if (fileName.IsEmpty())
                fileName = RequestUtil.Instance.GetString("name");

            string savePath = Server.MapPath(UploadFilePath);
            string result = "";
            bool isMarkWater = false;

            JsonObject jsonObject = new JsonObject();

            if (!fileName.IsEmpty())
            { 
                HttpPostedFile file = Request.Files[0];

                if (file.ContentLength <= 0)
                {
                    jsonObject.Result = false;
                    jsonObject.Msg = "文件" + " {0} ".FormatWith(file.FileName) + "容量为0KB";
                }
                else
                {
                    string saveFileName = "";
                    if (isPic)
                    {
                        Whir.Config.Models.PictureConfig pictureConfig = Whir.Config.ConfigHelper.GetPictureConfig();
                        if (pictureConfig != null && pictureConfig.IsAutoMakeWatermark)
                        {
                            try
                            {
                                FormUpload fUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(formId);
                                if (fUpload.IsWaterMark)
                                {
                                    isMarkWater = true;
                                }
                            }
                            catch { isMarkWater = false; }

                        }

                        saveFileName = ServiceFactory.UploadFilesService.UploadImageBeDateDir(file, savePath, UploadFilePath, formId, isMarkWater);
                        result += saveFileName;
                    }
                    else
                    {
                        FormUpload fUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(formId);

                        saveFileName = ServiceFactory.UploadFilesService.UploadFileBeDateDir(file, savePath, fUpload == null ? "" : fUpload.FileExts);
                        result += saveFileName;
                    }
                    //上传文件名称和物理名称到数据库
                    Upload upload = new Upload();
                    upload.Name = file.FileName.Substring(file.FileName.Replace("/", "\\").LastIndexOf('\\') + 1);
                    upload.RealName = saveFileName.Substring(saveFileName.LastIndexOf('/') + 1);
                    upload.Path = saveFileName.Replace(Server.MapPath(UploadFilePath), "");
                    upload.IsDel = false;
                    upload.State = 0;
                    upload.CreateDate = DateTime.Now;
                    upload.UpdateDate = DateTime.Now;
                    upload.UpdateUser = "前台上传";
                    upload.CreateUser = "前台上传";
                    ServiceFactory.UploadService.Save(upload);

                    result = result.TrimEnd(',');
                    jsonObject.Result = true;
                    jsonObject.Msg = result;
                }
                Response.Write(jsonObject.ToJson());
            }
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            if (msg.IndexOf("已经存在") > -1)
            {
                msg = "上传的文件已存在，请更改文件名称或修改上传配置。";
            }

            JsonObject jsonObject = new JsonObject()
            {
                Result = false,
                Msg = msg
            };
            Response.Write(jsonObject.ToJson());
        }

    }
    public class JsonObject
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
    }
}