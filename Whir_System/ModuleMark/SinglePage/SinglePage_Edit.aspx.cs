
using System;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_SinglePage_SinglePage_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 所属子站ID, 为0则为站点群信息
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemID { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        ItemID = ServiceFactory.DynamicFormService.GetSinglePagePrimaryValue(ColumnId, SubjectId);

        dynamicForm1.ColumnId = ColumnId;
        dynamicForm1.ItemId = ItemID;
        dynamicForm1.SubjectId = SubjectId;
    }
    
}