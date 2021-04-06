
using System;
using System.Web;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_ajax_extension_uploadimages : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Access-Control-Allow-Origin", "*");
        Response.AddHeader("Access-Control-Allow-Methods", "*");
        Response.AddHeader("Access-Control-Max-Age", "100");
        Response.AddHeader("Access-Control-Allow-Headers", "X-Custom-Header,accept, content-type");
        Response.AddHeader("Access-Control-Allow-Credentials", "false");
       

        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            int formID = RequestUtil.Instance.GetQueryInt("formID", 0);

            HttpPostedFile uploadPostedFile = Request.Files[0];
            string savePath = RequestUtil.Instance.GetString("savePath");
            savePath = HttpUtility.UrlDecode(savePath);
            string dir = savePath.Replace(UploadFilePath, "");
            savePath = Server.MapPath(savePath);

            if (!savePath.Contains(Server.MapPath(UploadFilePath).Trim('\\')))
            {
                string script = "<script language=\"javascript\" >whir.toastr.error('{0}', true, false)</script>".FormatWith("上传失败：试图在非法路径上传文件".ToLang());
                Response.Write(script);//上传失败
                return ;
            }
            string saveFileName = "";
            if (formID <= 0)
            {
                //正常图片库中上传图片
                saveFileName = ServiceFactory.UploadFilesService.UploadImage(uploadPostedFile, savePath, UploadFilePath);
            }
            else
            {
                //内容管理中，图片字段上传图片,需要根据FormId判断是否生成缩略图
                Whir.Config.Models.PictureConfig pictureConfig = Whir.Config.ConfigHelper.GetPictureConfig();
                saveFileName = ServiceFactory.UploadFilesService.UploadImage(uploadPostedFile, savePath, UploadFilePath, formID);
            }

            //上传文件名称和物理名称到数据库
            Upload upload = new Upload();
            upload.Name = uploadPostedFile.FileName;
            upload.RealName = saveFileName.Substring(saveFileName.LastIndexOf('\\') + 1); ;
            upload.Path = saveFileName.Replace(Server.MapPath(UploadFilePath),"").Replace("\\","/");
            upload.IsDel = false;
            upload.State = 0;
            upload.CreateDate = DateTime.Now;
            upload.UpdateDate = DateTime.Now;
            upload.UpdateUser = CurrentUserName;
            upload.CreateUser = CurrentUserName;
            ServiceFactory.UploadService.Save(upload);
            
            JsonObject jsonObject = new JsonObject();
            jsonObject.Result = true;
            jsonObject.Msg = dir + upload.RealName;

            //Response.StatusCode = 100;//不需要写本行代码，否则在IIS下运行会十分漫长
            Response.Write(jsonObject.ToJson());


        }
        catch (Exception ex)
        {
            string msg = "上传失败".ToLang();
            if (ex.Message.IndexOf("已经存在") > -1) {
                msg = "上传的文件已存在，请更改文件名称或修改上传配置。".ToLang();
            }
            string script = "<script language=\"javascript\" >whir.toastr.error('{0}', true, false)</script>".FormatWith(msg);
            Response.Write(script);//上传失败

        }

    }

    public class JsonObject
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
    }
}
