using System;
using System.Web;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_ajax_extension_uploadimages_form : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            int formId = RequestUtil.Instance.GetQueryInt("formid", 0);
            string controlId = RequestUtil.Instance.GetQueryString("controlID");
            string fileName = Request.QueryString["image"].ToStr();// RequestUtil.Instance.GetQueryString("image");// 使用后台会过滤文件名的+号或其它特殊符号 
            if (fileName.IsEmpty())
                fileName = Request.Form["name"].ToStr();

            string browser = RequestUtil.Instance.GetString("browser"); //ie9下使用

            string savePath = Server.MapPath(UploadFilePath);
            string result = "";
            bool isMarkWater = false;

            JsonObject jsonObject = new JsonObject();

            if (!fileName.IsEmpty())
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
                HttpFileCollection files = Request.Files;
                int index = 0;//需保存图片的索引
                for (int i = 0; i < files.Count; i++)
                {
                    if (files.AllKeys[i].Contains(controlId))
                    {
                        index = i;
                    }
                }

                HttpPostedFile file = files[index];

                if (file.ContentLength <= 0)
                {
                    jsonObject.Result = false;
                    jsonObject.Msg = "文件" + " {0} ".FormatWith(file.FileName) + "容量为0KB".ToLang();
                }
                else
                {
                    string saveFileName = ServiceFactory.UploadFilesService.UploadImageBeDateDir(file, savePath, UploadFilePath, formId, isMarkWater);
                    result += saveFileName;

                    //上传文件名称和物理名称到数据库
                    Upload upload = new Upload();
                    upload.Name = file.FileName.Substring(file.FileName.Replace("/", "\\").LastIndexOf('\\') + 1);
                    upload.RealName = saveFileName.Substring(saveFileName.LastIndexOf('/') + 1);
                    upload.Path = saveFileName.Replace(Server.MapPath(UploadFilePath), "");
                    upload.IsDel = false;
                    upload.State = 0;
                    upload.CreateDate = DateTime.Now;
                    upload.UpdateDate = DateTime.Now;
                    upload.UpdateUser = CurrentUserName;
                    upload.CreateUser = CurrentUserName;
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
                msg = "上传的文件已存在，请更改文件名称或修改上传配置。".ToLang();
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