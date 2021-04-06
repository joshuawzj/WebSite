/*
 * Copyright © 2009-2013 万户网络技术有限公司
 * 文 件 名：product_edit.aspx.cs
 * 文件描述：商品自定义属性编辑页面
 *          
 * 
 * 创建标识: 
 * 
 * 修改标识：
 */
using Shop.Domain;
using Shop.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Whir.Config;
using Whir.Config.Models;
using Whir.Controls.UI.Controls;
//非系统
using Whir.Domain;
using Whir.Framework;
using Whir.Language;

public partial class whir_system_Plugin_shop_product_product_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的商品ID
    /// </summary>
    protected int ProId
    {
        get
        {
            if (ViewState["ProID"] == null)
            {
                ViewState["ProID"] = 0;
            }
            return ViewState["ProID"].ToInt();
        }

        set
        {
            ViewState["ProID"] = value;
        }
    }

    /// <summary>
    /// 当前编辑的商品AttrIDs
    /// </summary>
    protected string AttrIDs
    {
        get
        {
            if (ViewState["AttrIDs"] == null)
            {
                ViewState["AttrIDs"] = "";
            }
            return ViewState["AttrIDs"].ToStr();
        }

        set
        {
            ViewState["AttrIDs"] = value;
        }
    }

    DataTable Sp
    {
        get
        {
            if (ViewState["DataRowsp"] == null)
            {
                return new DataTable();

            }
            return (DataTable)ViewState["DataRowsp"];
        }

        set
        {
            ViewState["DataRowsp"] = value;
        }
    }

    protected ShopProInfo ShopProductInfo { get; set; }

    /// <summary>
    /// 商品分类下拉
    /// </summary>
    public DataTable OptionTable { get; set; }

    public IList<ShopAttr> SaList { get; set; }

    /// <summary>
    /// 自定义字段html
    /// </summary>
    public string FieldHtml { get; set; }

    /// <summary>
    /// 商品多图html
    /// </summary>
    public string ProductMultiPicHtml { get; set; }

    /// <summary>
    ///搜选项
    /// </summary>
    public IList<ShopSearch> Searchlist { get; set; }

    /// <summary>
    /// 操作提示
    /// </summary>
    public string ProcessStr { get; set; }

    /// <summary>
    /// 弹窗选文件只显示指定类型文件，格式： ".jpg,.png,.gif,.bmp"
    /// </summary>
    protected string AcceptPicType { get; private set; }

    /// <summary>
    /// 可上传文件后缀名, 'jpg','png','gif','bmp'
    /// </summary>
    protected string AllowPicType { get; private set; }

    //规格数据
    public string ScriptAttrPro { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("410"));
        UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();

        foreach (string extension in uploadConfig.AllowPicType.Split('|'))
        {
            AllowPicType += "'" + extension + "'" + ",";
            AcceptPicType += "." + extension + ",";
        }
        AllowPicType = AllowPicType.TrimEnd(',');
        AcceptPicType = AcceptPicType.TrimEnd(',');

        ProId = RequestUtil.Instance.GetQueryInt("proid", 0);
        StringBuilder scriptStr = new StringBuilder();
        if (ProId > 0)
        {
            BindField();//绑定自定义属性
        }
        if (!IsPostBack)
        {
            BindOption();//绑定上级类目
            BindSearch();//绑定搜选项
            BindAttr();//绑定规格选项
            if (ProId > 0)
            {
                ShopProductInfo = ShopProInfoService.Instance.GetShopProById(ProId);
                ProcessStr = "编辑商品".ToLang();
                //规格信息
                IList<ShopAttrPro> attrProList = ShopAttrProService.Instance.GetShopAttrProByProID(ProId);
                if (attrProList.Count > 0)
                {
                    for (int i = 0; i < attrProList.Count; i++)
                    {

                        string initialPreview = "";
                        string initialPreviewConfig = "";
                        string[] imageArr = attrProList[i].Images.TrimStart('*').TrimEnd('*').Split('*');
                        for (int j = 0; j < imageArr.Length; j++)
                        {
                            if (j == imageArr.Length - 1)
                            {
                                initialPreview += "\"{0}\"".FormatWith(UploadFilePath + imageArr[j]);
                            }
                            else
                            {
                                initialPreview += "\"{0}\",".FormatWith(UploadFilePath + imageArr[j]);

                            }
                            FileInfo file =
                                new FileInfo(System.Web.HttpContext.Current.Request.PhysicalApplicationPath +
                                             AppSettingUtil.AppSettings["UploadFilePath"] + imageArr[j]);
                            initialPreviewConfig +=
                                "{3}caption: \"{0}\", size: {1},name:\"{5}\", key: {2}{4},".FormatWith(
                                    Whir.Service.ServiceFactory.UploadService.GetFileName(imageArr[j]),
                                    file.Exists ? file.Length : 0, j, "{", "}", imageArr[j]);
                        }

                        scriptStr.Append("addAttrPro('" + attrProList[i].AttrValueNames + "','" + attrProList[i].AttrValueIDs + "','" +
                            string.Format("{0:F2}", attrProList[i].CostAmount) + "','" + (attrProList[i].IsUseMainImage ? "1" : "") + "','" +
                            attrProList[i].AttrProID + "','" + attrProList[i].Images + "','[" + initialPreview.TrimEnd(',') + "]','[" + initialPreviewConfig.TrimEnd(',').Replace("{", "&1").Replace("}", "&2") + "]');");

                        string tempid = attrProList[i].AttrValueIDs.Replace(",", "");
                        if (!string.IsNullOrEmpty(attrProList[i].Images))
                        {
                            //scriptStr.Append("getImageValueAndShow('" + attrProList[i].Images + "','hid" + tempid + "','div" + tempid + "');");
                        }
                        if (!string.IsNullOrEmpty(attrProList[i].ProImg))
                        {
                            //scriptStr.Append("SetProImg('" + attrProList[i].ProImg + "','','div" + tempid + "');");
                        }
                    }
                }
                ScriptAttrPro = scriptStr.ToStr();
            }
            else
            {
                ShopProductInfo = new ShopProInfo
                {
                    ProNO = DateTime.Now.ToString("yyMMddhhmmssfff") + Rand.Instance.Number(4)
                };
                ProcessStr = "添加商品".ToLang();
            }


            var field = new Field
            {
                FieldAlias = "商品多图".ToLang(),
                FieldName = "Images",
                FieldId = -1,
                DefaultValue = ""
            };
            var control = new ControlContext(new Image(new Column(), new Form() { FormId = new Random().Next(2, 9999) }, field, RegularEnum.Never));
            ProductMultiPicHtml = control.Render(ShopProductInfo.Images);
        }

    }

    /// <summary>
    /// 绑定自定义字段
    /// </summary>
    private void BindField()
    {
        IList<ShopField> list = ShopFieldService.Instance.GetProShopFileList();

        if (list.Count == 0) return;
        foreach (ShopField item in list)
        {
            FieldHtml += GetHtml(item, null);
        }

    }

    public string GetHtml(ShopField item, object val)
    {
        var builder = new StringBuilder();
        //(1单行文本框，2单选按钮，3多选按钮，4多行文本框，5下拉框，6HTML编辑器，7文件上传)
        var field = new Field
        {
            FieldAlias = item.FieldAlias,
            FieldName = item.FieldName,
            FieldId = item.FieldID,
            DefaultValue = item.DefaultValue
        };
        ControlContext control;

        switch (item.ShowType)
        {
            case 1:
                #region 单行文本
                if (item.FieldType == "datetime")//使用带日期控件的文本
                {
                    control = new ControlContext(new TimePicker(new Column(), new Form(), field, RegularEnum.Custom));
                }
                else
                {
                    control = new ControlContext(new Whir.Controls.UI.Controls.TextBox(new Column(), new Form(), field, RegularEnum.Custom, new DataTable().NewRow()));
                }
                #endregion 单行文本
                break;
            case 2: //单选按钮
                control = new ControlContext(new Shop.Controls.Select(new Column(), new Form(), field, RegularEnum.Never, item));
                break;
            case 3: //多选按钮                
                item.BindType = 4;
                control =
                  new ControlContext(new Shop.Controls.Select(new Column(), new Form(), field, RegularEnum.Never, item));
                break;
            case 4://多行文本
                #region 多行文本
                control =
                new ControlContext(new TextArea(new Column(), new Form(), field,
                    RegularEnum.Custom));
                #endregion 多行文本
                break;
            case 5: //下拉框
                control = new ControlContext(new Shop.Controls.Select(new Column(), new Form(), field, RegularEnum.Never, item));
                break;
            case 6://编辑器
                #region 编辑器
                control = new ControlContext(new Editer(new Column(), new Form() { DefaultValue = "", FormId = Rand.Instance.Number(4, true).ToInt() }, field, RegularEnum.Never));
                #endregion 编辑器
                break;
            case 7:
                #region 图片
                control = new ControlContext(new Image(new Column(), new Form(), field, RegularEnum.Never));
                #endregion 图片
                break;
            case 8:
                #region 文件
                control = new ControlContext(new Whir.Controls.UI.Controls.File(new Column(), new Form(), field, RegularEnum.Never));
                #endregion 文件
                break;
            default:
                #region 单行文本
                control = new ControlContext(new Whir.Controls.UI.Controls.TextBox(new Column(), new Form(), field, RegularEnum.Custom, new DataTable().NewRow()));
                #endregion 单行文本
                break;
        }
        string controlHtml = string.Empty;
        if (val == null)
        {
            var dt = Whir.Repository.DbHelper.CurrentDb.Query("SELECT * FROM dbo.Whir_Shop_ProInfo WHERE ProID=@0 AND IsDel=0", ProId).Tables[0];

            if (dt.Rows.Count > 0)
            {
                controlHtml = control.Render(dt.Rows[0]["" + field.FieldName + ""].ToStr());
            }
        }
        else
        {
            controlHtml = control.Render(val.ToStr());
        }
        const string style = "";
        const string leftclass = "col-md-2  control-label";
        const string rightclass = "col-md-10 ";
        builder.Append("<div class=\"form-group \" " + style + ">" + Environment.NewLine);
        builder.AppendFormat("<div class=\"{2}\" for=\"{0}\">{1}" + Environment.NewLine, field.FieldName, field.FieldAlias.ToLang() + "：", leftclass);
        builder.Append("   </div>");
        builder.AppendFormat("   <div class=\"{0}\">" + Environment.NewLine, rightclass);
        builder.Append("   " + controlHtml + Environment.NewLine);
        builder.Append("   </div>" + Environment.NewLine);
        builder.Append("</div>" + Environment.NewLine);
        return builder.ToString();
    }



    //绑定上级类目   
    private void BindOption()
    {
        List<DataRow> list = ShopCategoryService.Instance.GetAllCategoryList(0);
        if (list.Count == 0)
        {
            OptionTable = new DataTable();
        }
        else
        {
            OptionTable = list.CopyToDataTable();
        }
    }

    /// <summary>
    /// 检测文本中包含敏感词的文字, 忽略大小写
    /// </summary>
    /// <param name="listWord"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private string IsWord(IEnumerable<SensitiveWord> listWord, string value)
    {
        string result = string.Empty;
        foreach (SensitiveWord word in listWord)
        {
            if (value.ToLower().Contains(word.SensitiveWordName.ToLower()))
                result += word.SensitiveWordName;
        }
        return result;
    }

    #region 绑定搜选项
    private void BindSearch()
    {
        Searchlist = ShopSearchService.Instance.GetAllShopSearchList();
    }
    #endregion

    #region 绑定规格选项
    private void BindAttr()
    {
        SaList = ShopAttrService.Instance.GetAllShopAttrList();
        //if (saList.Count > 0)
        //{
        //    string maxstr = "";
        //    for (int i = 0; i < saList.Count; i++)
        //    {
        //        ListItem item = new ListItem(saList[i].SearchName, saList[i].AttrID.ToStr());
        //        //cbAttrList.Items.Add(item);
        //        //cbAttrList.Items[i].Attributes.Add("AttrID", saList[i].AttrID.ToStr());
        //        if (saList[i].SearchName.Length > maxstr.Length)
        //        {
        //            maxstr = saList[i].SearchName;
        //        }
        //    }
        //    //cbAttrList.RepeatColumns = 32 / maxstr.Length;
        //}
    }
    #endregion
}