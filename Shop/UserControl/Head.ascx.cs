/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Shop_UserControl_Head.cs
 * 文件描述：网页head部分
 * 
 * 创建标识: lurong 2013-2-19
 * 
 * 修改标识：
 */
using System;
using System.Text;
using Shop.Common;
public partial class Shop_UserControl_Head : UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        
        StringBuilder seo = new StringBuilder();
        seo.AppendFormat("<title>{0}</title>", "商城_万户网络ezEIP5.0");
        seo.AppendFormat("<meta name='Author' content='{0}'></meta>", "万户网络设计制作");
        seo.AppendFormat("<meta name='description' content='{0}'></meta>", "商城_万户网络ezEIP5.0");
        litSeo.Text = seo.ToString();
    }
}