using System;
using Whir.Framework;

public partial class whir_system_Plugin_shop_common_option_select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 回传给父页面的JS函数名
    /// </summary>
    public string CallBack { get; set; }
    /// <summary>
    /// 保存编辑时的主键ID
    /// </summary>
    public int CategoryID
    {
        get
        {
            if (ViewState["CategoryID"] == null)
            {
                ViewState["CategoryID"] = 0;
            }
            return ViewState["CategoryID"].ToInt();
        }
        set
        {
            ViewState["CategoryID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        CategoryID = RequestUtil.Instance.GetQueryInt("lcid", 0);
        CallBack = RequestUtil.Instance.GetQueryString("callback");       
    }
}