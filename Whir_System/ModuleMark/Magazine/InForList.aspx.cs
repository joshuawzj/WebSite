/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：inforlist.aspx.cs
* 文件描述：文章列表(电子期刊信息)页面。 
*/
using System;
using Whir.Framework;
using Whir.Service;
using System.Collections.Generic;
using Whir.Domain;
using System.Linq;
using Whir.Language;
public partial class Whir_System_ModuleMark_Magazine_InForList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 期刊Id
    /// </summary>
    protected int ItemID { get; set; }

    /// <summary>
    /// 是否显示批量删除按钮
    /// </summary>
    protected bool IsShowBatchDelete { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 绑定附属操作
    /// </summary>
    protected IList<Column> AttchlistColumn { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);


        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        IsShowBatchDelete = contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.IsShowChapter = IsRoleHaveColumnRes("章节管理");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.EditPageUrl = "Content_Edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&history=false" + "&magazineid=" + ItemID + "&BackPageUrl=" + CurrentPageUrl;

        contentManager1.Where = "MagazineID=" + ItemID;
        AttchlistColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId)
           .Where(p => p.MarkType != "Infor" && p.MarkType != "Chapter").ToList();
    }
}