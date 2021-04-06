/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：ColumnLeftTree.ascx.cs
 * 文件描述：置标生成器的左边置标树用户控件
 *          
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Linq;
using System.Text;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;

public partial class whir_system_UserControl_DeveloperControl_ColumnLeftTree : Whir.ezEIP.Web.SysControlBase
{
    /// <summary>
    /// 获取ajax传递的选中参数
    /// </summary>
    protected string ShowSelectID { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowSelectID = WebUtil.Instance.GetQueryString("showselectid").ToStr();
    }

    /// <summary>
    /// 绑定树
    /// </summary>
    /// <returns></returns>
    protected string BindTree()
    {
        IList<GenerateLabel> generateLabelList = ServiceFactory.GenerateLabelService.ReadXml();//置标列表
        List<ColumnLeftTree> ctList = new List<ColumnLeftTree>();

        foreach (GenerateLabel item in generateLabelList)
        {
            ColumnLeftTree ct = new ColumnLeftTree();
            if (item.LabelId == 0)
            {
                ct.Id = item.LabelId;
                ct.name = item.LabelName;
                ct.pId = item.ParentId;
                ct.open = true;
            }
            else
            {
                ct.Id = item.LabelId;
                ct.name = item.LabelName;
                ct.pId = item.ParentId;
                ct.open = false;
            }
            ctList.Add(ct);
        }

        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(ctList);
    }
}