using System;
using System.Web;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;


public partial class whir_system_ajax_extension_uploadfiles_form : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

            string controlID = RequestUtil.Instance.GetQueryString("controlID");
            string fileName = RequestUtil.Instance.GetQueryString("file");
            string formId = RequestUtil.Instance.GetQueryString("formid");

            string savePath = Server.MapPath(UploadFilePath);
            string result = "";
            JsonObject jsonObject = new JsonObject();
            if (!fileName.IsEmpty())
            {
                HttpFileCollection files = Request.Files;
                int index = 0;//需保存图片的索引
                for (int i = 0; i < files.Count; i++)
                {
                    if (files.AllKeys[i].Contains(controlID))
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
                    FormUpload formUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(formId.ToInt());
                    string fileExts = formUpload == null ? "" : formUpload.FileExts;

                    string saveFileName = ServiceFactory.UploadFilesService.UploadFileBeDateDir(file, savePath, fileExts);
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