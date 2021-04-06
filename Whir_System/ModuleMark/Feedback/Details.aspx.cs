/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：details.aspx.cs
* 文件描述：查看留言页面
*/

using System;

using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_FeedBack_Details : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);

        detailsForm1.ColumnId = detailsForm2.ColumnId = ColumnId;
        detailsForm1.ItemID = detailsForm2.ItemID = ItemID;


        if (!IsPostBack)
        {
            if (ItemID != 0)
            {
                PageMode = EnumPageMode.Update;
            }
        }
    }
}