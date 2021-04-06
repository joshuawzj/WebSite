

using System;
using System.Web;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_ajax_extension_UploadSites : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        try
        {
            int formID = RequestUtil.Instance.GetQueryInt("formID", 0);

            HttpPostedFile uploadPostedFile = Request.Files[0];
            string savePath = RequestUtil.Instance.GetString("savePath");
            savePath = HttpUtility.UrlDecode(savePath);
            string dir = savePath.Replace(CurrentSiteDirection, "");
            savePath = Server.MapPath(savePath);

            if (!savePath.Contains(Server.MapPath(CurrentSiteDirection).Trim('\\')))
            {
                string script = "<script language=\"javascript\" >whir.toastr.error('{0}', true, false)</script>".FormatWith("上传失败：试图在非法路径上传文件".ToLang());
                Response.Write(script);//上传失败
                return;
            }

            string saveFileName = ServiceFactory.UploadFilesService.UploadFile(uploadPostedFile, savePath);

            //上传文件名称和物理名称到数据库
            Upload upload = new Upload();
            upload.Name = uploadPostedFile.FileName;
            upload.RealName = saveFileName.Substring(saveFileName.LastIndexOf('\\') + 1); ;
            upload.Path = saveFileName.Replace(Server.MapPath(CurrentSiteDirection), "").Replace("\\", "/");
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
            Response.Write(jsonObject.ToJson());

        }
        catch (Exception ex)
        {
            string msg = "上传失败".ToLang();
            if (ex.Message.IndexOf("已经存在") > -1)
            {
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