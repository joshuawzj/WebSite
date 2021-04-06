/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subinfo.aspx.cs
 * 文件描述：子站专题的默认提示页
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;

public partial class whir_system_ModuleMark_common_subinfo : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            int subid=WebUtil.Instance.GetQueryInt("subid", 0);
            switch (subid)
            { 
                case 1:
                    ltInfo.Text = "请点击左侧栏目树选择子站栏目！".ToLang();
                    break;
                case 2:
                    ltInfo.Text = "请点击左侧栏目树选择专题栏目！".ToLang();
                    break;
            }
        }
    }
}