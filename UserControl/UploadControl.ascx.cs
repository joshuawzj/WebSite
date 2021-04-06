using System;
using System.Text;
using Whir.Config;
using Whir.Config.Models;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;

public partial class UploadControl : FrontUserControl
{

    /// <summary>
    /// 文件扩展名，默认为系统后台配置参数，格式：*.jpg|*.png|*.gif|*.bmp
    /// </summary>
    public string FileExt;

    /// <summary>
    /// 文件大小，默认为系统后台配置参数
    /// </summary>
    public decimal FileSize;

    /// <summary>
    /// 是否开启多文件上传
    /// </summary>
    public bool IsMulti;

    /// <summary>
    /// 字段属性
    /// </summary>
    public Form Form=new Form();

    /// <summary>
    /// 字段属性
    /// </summary>
    public Field Field=new Field();

    /// <summary>
    ///默认后台上传配置
    /// </summary>
    public UploadConfig UploadConfig = ConfigHelper.GetUploadConfig();

    /// <summary>
    /// 上传后的文件路径
    /// </summary>
    public string FilePath;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FileExt = Whir.Config.ConfigHelper.GetUploadConfig().AllowFileType;
            GetUploadFileHtml();
        }

    }

    protected void GetUploadFileHtml()
    {

    }
}