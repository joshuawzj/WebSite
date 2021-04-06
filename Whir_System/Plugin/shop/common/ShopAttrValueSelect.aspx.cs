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
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//非系统
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Shop.Domain;
using Shop.Service;


public partial class whir_system_Plugin_shop_common_shopattrvalueselect : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 回传给父页面的JS函数名
    /// </summary>
    public string CallBack { get; set; }
    /// <summary>
    /// 当前编辑的商品ID
    /// </summary>
    protected int ProID
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
     /// 规格信息
     /// </summary>
    public IList<ShopAttr> searchlist { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        ProID = RequestUtil.Instance.GetQueryInt("proid", 0);
        CallBack = RequestUtil.Instance.GetQueryString("callback");
        BindAttr();

    }
    #region 绑定搜选项
    private void BindAttr()
    { 
        searchlist = new List<ShopAttr>();
        if (ProID > 0)
        {

            ShopProInfo spi = ShopProInfoService.Instance.GetShopProById(ProID);
            if (!string.IsNullOrEmpty(spi.AttrIDs))
            {
                searchlist = ShopAttrService.Instance.Query<ShopAttr>("SELECT * FROM Whir_Shop_Attr WHERE AttrID IN(" + spi.AttrIDs + ")").ToList();
            }
        }
    }
    #endregion
}