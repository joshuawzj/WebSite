using System.Text;
using Whir.Domain;
using Whir.Config;
using Whir.Framework;
using System.IO;
using Whir.Config.Models;
using Whir.Language;
using Whir.Controls.UI.Controls;

namespace Shop.Controls
{
    /// <summary>
    /// Image 的摘要说明
    /// </summary>
    public class Image : Control
    {
        public Image(Column column, Form form, Field field, RegularEnum regular)
            : base(column, form, field, regular)
        {
        }
        public override string Render(string val)
        {
            var newLine = System.Environment.NewLine;

            UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();
            PictureConfig picConfig = ConfigHelper.GetPictureConfig();
            
            FormUpload formUpload = new FormUpload() { IsWaterMark = picConfig.IsAutoMakeWatermark, FileExts = uploadConfig.AllowPicType, UploadMode = Form.FormId  }; 
            //只有formId=1 为单选，其他都为多选
            string allowedExtensions = "";
            string allowExt = "[";
            string selectExt = "";
            allowedExtensions = formUpload.FileExts;
            foreach (string ext in allowedExtensions.Split('|'))
            {
                allowExt += "'" + ext + "'" + ",";
                selectExt += "." + ext + ",";
            }
            if (!selectExt.IsEmpty())
            {
                allowExt = allowExt.TrimEnd(',') + "]";
                selectExt = selectExt.TrimEnd(',');
            }

            var html = new StringBuilder();
            html.AppendFormat(
                " <input id=\"file{0}\" value=\"\" name=\"file{0}\" type=\"file\"  for=\"{3}\" class=\"file-loading\" {2} accept=\"{1}\" />" +
                newLine, Form.FormId, selectExt, formUpload.UploadMode == 1 ? "" : " multiple ", Field.FieldName);
            html.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" name=\"{0}\" {2}/>" + newLine,
               Field.FieldName, val, Form.IsAllowNull ? "" : " required=\"true\"");


            html.Append("<script type=\"text/javascript\">" + newLine);
            if (val.IsEmpty())
            {
                html.AppendFormat("var config{0} = {1} " + newLine, Form.FormId, "{");
                html.Append("language: 'zh',");
                html.Append("showClose: false,");
                html.AppendFormat(
                    "uploadUrl: \"{0}Ajax/Extension/UploadImages_Form.aspx?formid={1}&controlID=file{1}&image=\",",
                    SysPath, Form.FormId);
                html.Append("previewFileType: \"image\"," + newLine);

                html.AppendFormat("maxFileSize: {0}," + newLine, uploadConfig.MaxPicSize);
                html.AppendFormat("initialCaption: '{0}'," + newLine, Form.TipText.IsEmpty() ? "支持格式：".ToLang() + allowedExtensions : Form.TipText);
                html.AppendFormat("allowedFileExtensions: {0}," + newLine, allowExt);
                html.Append("previewClass: \"bg-warning\"," + newLine);
                html.AppendFormat("pickerUrl: '{0}ModuleMark/Common/Picker.aspx?isPic=1&isMultiple={1}&HidChooseId={2}&ControlId={3}'," + newLine,
                SysPath, formUpload.UploadMode == 1 ? "radio" : "checkbox", Field.FieldName, "file" + Form.FormId);
                html.Append("isPic:true,");
                html.Append("initialPreviewAsData:true," + newLine);
                html.Append("initialPreviewFileType:'image'," + newLine);
                html.Append("};" + newLine);
            }
            else
            {
                html.AppendFormat("var config{0} = {1} " + newLine, Form.FormId, "{");
                html.Append("language: 'zh',");
                html.Append("showClose: false,");
                html.AppendFormat(
                    "uploadUrl: \"{0}Ajax/Extension/UploadImages_Form.aspx?formid={1}&controlID=file{1}&image=\",",
                    SysPath, Form.FormId);
                html.Append("previewFileType: \"image\"," + newLine);

                html.AppendFormat("maxFileSize: {0}," + newLine, uploadConfig.MaxPicSize);
                html.AppendFormat("initialCaption: '{0}'," + newLine, Form.TipText.IsEmpty() ? "支持格式：".ToLang() + allowedExtensions : Form.TipText);
                html.AppendFormat("allowedFileExtensions: {0}," + newLine, allowExt);
                html.Append("previewClass: \"bg-warning\"," + newLine);
                html.AppendFormat("overwriteInitial: {0}," + newLine, formUpload.UploadMode == 1 ? "true" : "false");
                html.AppendFormat("pickerUrl: '{0}ModuleMark/Common/Picker.aspx?isPic=1&isMultiple={1}&HidChooseId={2}&ControlId={3}'," + newLine,
                   SysPath, formUpload.UploadMode == 1 ? "radio" : "checkbox", Field.FieldName, "file" + Form.FormId);
                html.Append("isPic:true,");
                html.Append("initialPreview:  [");
                string fileconfig = "initialPreviewConfig:  [";
                string[] imageArr = val.TrimStart('*').TrimEnd('*').Split('*');
                for (int i = 0; i < imageArr.Length; i++)
                {
                    try
                    {
                        FileInfo file =
                            new FileInfo(System.Web.HttpContext.Current.Request.PhysicalApplicationPath +
                                         AppSettingUtil.AppSettings["UploadFilePath"] + imageArr[i]);
                        if (i == imageArr.Length - 1)
                        {
                            html.AppendFormat("\"{0}\"", UploadFilePath + imageArr[i]);
                        }
                        else
                        {
                            html.AppendFormat("\"{0}\",", UploadFilePath + imageArr[i]);
                        }

                        fileconfig +=
                       "{3}caption: \"{0}\", size: {1},name:\"{5}\", key: {2}{4},".FormatWith(
                           Whir.Service.ServiceFactory.UploadService.GetFileName(imageArr[i]),
                           file.Exists ? file.Length : 0, i, "{", "}", imageArr[i]);
                    }
                    catch
                    {
                        continue;
                    }

                }
                html.Append("],initialPreviewAsData:true" + newLine);
                html.Append(",initialPreviewFileType:'image'," + newLine);
                html.Append(fileconfig.TrimEnd(','));

                html.Append("],purifyHtml: true};" + newLine);
            }
            html.Append("$(document).ready(function() {" + newLine);
            html.AppendFormat("$(\"#file{0}\")" + newLine, Form.FormId);
            html.AppendFormat(" .fileinput(config{0})" + newLine, Form.FormId);
            html.Append(" .on(\"filebatchselected\"," + newLine);
            html.Append(" function (event, files) {" + newLine);
            html.Append("$(this).fileinput(\"upload\");" + newLine);
            html.Append("})" + newLine);
            html.Append(".on(\"fileuploaded\"," + newLine);
            html.Append("function (event, data) {" + newLine);
            html.Append(" if (data.response && data.response.Result == true) {" + newLine);
            if (formUpload.UploadMode == 1)
            {
                html.AppendFormat("$(\"#{0}\").val(data.response.Msg); " + newLine, Field.FieldName, Form.FormId);
            }
            else
            {
                html.AppendFormat(" var tempVal=$(\"#{0}\").val();" + newLine, Field.FieldName);
                html.AppendFormat(" if(tempVal!=''&&tempVal!='*')" + newLine, Field.FieldName);
                html.AppendFormat("$(\"#{0}\").val($(\"#{0}\").val()+'*'+data.response.Msg);" + newLine, Field.FieldName);
                html.AppendFormat("else" + newLine, Field.FieldName);
                html.AppendFormat("$(\"#{0}\").val(data.response.Msg);" + newLine, Field.FieldName);

            }
            html.AppendFormat("formRequiredValidator('{0}',true);" + newLine, Field.FieldName);
            html.Append("}" + newLine);
            html.Append("else { " + newLine);
            html.Append("whir.toastr.error(data.response.Msg);" + newLine);
            html.Append("}" + newLine);
            html.Append(" })" + newLine);
            html.Append(".on(\"filecleared\"," + newLine);
            html.Append("function (event, data) {" + newLine);
            html.AppendFormat("$(\"#{0}\").val('');" + newLine, Field.FieldName);
            html.AppendFormat("formRequiredValidator('{0}',false);" + newLine, Field.FieldName);
            html.Append(" });" + newLine);
            html.Append("});" + newLine);

            html.Append("</script>" + newLine);
            return html.ToString();
        }
    }
}