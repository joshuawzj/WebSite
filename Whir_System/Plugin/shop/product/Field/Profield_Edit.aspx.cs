
using System;
//非系统的引用
using Whir.Framework;

using Shop.Domain;
using Whir.Language;

public partial class whir_system_Plugin_shop_product_field_profield_edit : Whir.ezEIP.Web.SysManagePageBase
{

    /// <summary>
    /// 操作字符串
    /// </summary>
    public string ProcessStr { get; set; }
    /// <summary>
    /// 保存编辑时的主键ID
    /// </summary>
    public int FieldID
    {
        get
        {
            if (ViewState["FieldID"] == null)
            {
                ViewState["FieldID"] = 0;
            }
            return ViewState["FieldID"].ToInt();
        }
        set
        {
            ViewState["FieldID"] = value;
        }
    }
    /// <summary>
    /// 字段实体类
    /// </summary>
    public ShopField Field { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("415"));
            FieldID = RequestUtil.Instance.GetQueryInt("fieldid", 0);
            if (FieldID > 0)
            {
                ProcessStr = "编辑自定义属性".ToLang();
                Field = ShopFieldService.Instance.GetShopFileById(FieldID);
            }
            else
            {
                Field = new ShopField();

                ProcessStr = "添加自定义属性".ToLang();
            }
        }
    }

}