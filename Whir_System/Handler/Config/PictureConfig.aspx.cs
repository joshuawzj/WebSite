using System;
using System.Drawing;
using System.Linq;
using System.Web.Mail;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;


public partial class Whir_System_Handler_Config_PictureConfig : SysHandlerPageBase
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("338"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(PictureConfig);
        var model = ConfigHelper.GetPictureConfig() ?? ModelFactory<PictureConfig>.Insten();
        try
        {
            model = GetPostObject(type, model) as PictureConfig;
            if (model != null && model.WaterMarkFontText.IndexOf("|||", StringComparison.Ordinal) > 0)
            {
                return new HandlerResult { Status = false, Message = "不可包含字符“|||”".ToLang() };
            }
            if (model != null && model.WaterMarkFont.IndexOf("|||", StringComparison.Ordinal) > 0)
            {
                return new HandlerResult { Status = false, Message = "不可包含字符“|||”".ToLang() };
            }
            model.Proportion = float.Parse(RequestUtil.Instance.GetString("Proportion"));

            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("PictureConfig.config"), type, model);
            ModifyEditorConfig(model);//修改编辑器中配置

            //记录操作日志
            if (model != null)
            {
                ServiceFactory.OperateLogService.Save("修改图片水印设置，水印图片【{0}】，水印位置【{1}】".FormatWith(
                    model.WaterMarkPic,
                    model.WaterMarkWhere));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };


    }

    #region 修改EWebEditor的配置
    private void ModifyEditorConfig(PictureConfig model)
    {
        string sFileName = AppName + "editor/eWebEditor/aspx/config.aspx";
        string styleName = "standard600";
        string[] oldStyleConfig = EWebEditorHelper.GetStyleConfig(sFileName, styleName);
        string[] newStyleConfig = EWebEditorHelper.GetStyleConfig(sFileName, styleName);

        if (model.WaterMarkStyle == "0" && model.IsAutoMakeWatermark)//使用文字水印
        {
            newStyleConfig[32] = "1";//文字水印使用状态 - 使用
            newStyleConfig[52] = "0";//图片水印使用状态 - 不使用

            //文字水印的文本内容
            newStyleConfig[33] = model.WaterMarkFontText.Trim();
            //文字水印字体
            newStyleConfig[36] = model.WaterMarkFont;
            //文字水印的字体大小
            newStyleConfig[35] = newStyleConfig[49] = Convert.ToInt16(model.Proportion.ToDecimal() * 100).ToStr();
            //文字水印的字体颜色
            newStyleConfig[34] = newStyleConfig[41] = model.WaterMarkFontColor.IsEmpty() ? "000000" : model.WaterMarkFontColor.TrimStart('#');

            //文字水印占位 文字的宽度=文字大小*文字长度
            newStyleConfig[48] = (Convert.ToInt16(model.Proportion.ToDecimal() * 100) * model.WaterMarkFontText.Trim().Length).ToStr();
            newStyleConfig[49] = newStyleConfig[35];

            newStyleConfig[50] = "15";//文字水印边距
            newStyleConfig[51] = "15";


            newStyleConfig[40] = "15";//文字水印启用条件
            newStyleConfig[46] = "15";

            //水印位置
            switch (model.WaterMarkWhere)
            {
                case "1":
                    newStyleConfig[47] = "1";     //左上
                    break;
                case "2":
                    newStyleConfig[47] = "4";//中上
                    break;
                case "3":
                    newStyleConfig[47] = "7";//右上
                    break;
                case "4":
                    newStyleConfig[47] = "2";//左中
                    break;
                case "5":
                    newStyleConfig[47] = "5";//中中
                    break;
                case "6":
                    newStyleConfig[47] = "8";//右中
                    break;
                case "7":
                    newStyleConfig[47] = "3";//左下
                    break;
                case "8":
                    newStyleConfig[47] = "6";//中下
                    break;
                case "9":
                    newStyleConfig[47] = "9";//右下
                    break;
            }

        }
        else if (model.WaterMarkStyle == "1" && model.IsAutoMakeWatermark)//使用图片水印
        {
            Image markImg = Image.FromFile(Server.MapPath(UploadFilePath + model.WaterMarkPic));

            newStyleConfig[32] = "0";//文字水印使用状态 - 不使用
            newStyleConfig[52] = "1";//图片水印使用状态 - 使用

            newStyleConfig[37] = UploadFilePath + model.WaterMarkPic;//图片水印-图片路径

            newStyleConfig[58] = markImg.Width .ToStr();//图片水印图片占位 水印图的宽度
            newStyleConfig[59] = markImg.Height.ToStr(); 

            newStyleConfig[56] = "15";//图片水印边距
            newStyleConfig[57] = "15";

            newStyleConfig[53] = "15";//图片水印启用条件
            newStyleConfig[54] = "15";

            newStyleConfig[60] = (model.WaterMarkTransparent.ToDecimal(200) / 255).ToString("f2");

            //水印位置
            switch (model.WaterMarkWhere)
            {
                case "1":
                    newStyleConfig[55] = "1";     //左上
                    break;
                case "2":
                    newStyleConfig[55] = "4";//中上
                    break;
                case "3":
                    newStyleConfig[55] = "7";//右上
                    break;
                case "4":
                    newStyleConfig[55] = "2";//左中
                    break;
                case "5":
                    newStyleConfig[55] = "5";//中中
                    break;
                case "6":
                    newStyleConfig[55] = "8";//右中
                    break;
                case "7":
                    newStyleConfig[55] = "3";//左下
                    break;
                case "8":
                    newStyleConfig[55] = "6";//中下
                    break;
                case "9":
                    newStyleConfig[55] = "9";//右下
                    break;
            }
        }
        else//不使用水印
        {
            newStyleConfig[32] = "0";
            newStyleConfig[52] = "0";
        }

        string mapPath = Server.MapPath(sFileName);
        EWebEditorHelper.ModifyStyle(sFileName, mapPath, oldStyleConfig, newStyleConfig);
    }
    #endregion
}