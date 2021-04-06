/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：area_select.aspx.cs
 * 文件描述：地区选择页面
 */

using System;

using Whir.Framework;

public partial class Whir_System_ModuleMark_Common_Area_Select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 回传给父页面的JS函数名
    /// </summary>
    public string CallBack { get; set; }

    /// <summary>
    /// 显示级别
    /// </summary>
    public int AreaLevel { get; set; }

    public string Field { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CallBack = RequestUtil.Instance.GetQueryString("callback");
        AreaLevel = RequestUtil.Instance.GetQueryInt("arealevel", 3);
        Field = RequestUtil.Instance.GetQueryString("field");
    }
}