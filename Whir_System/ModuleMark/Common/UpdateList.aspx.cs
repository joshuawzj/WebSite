/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：content_edit.aspx.cs
 * 文件描述：简单信息公用编辑页面
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Label;
using Whir.Language;
using Whir.Repository;
using Whir.Service;

public partial class Whir_System_ModuleMark_Common_UpdateList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 批量修改的字段ID组
    /// </summary>
    protected string FieldIds { get; set; }

    /// <summary>
    /// 子站和专题ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 编辑的信息ID集合
    /// </summary>
    protected string ItemIds { get; set; }

    protected string ExceptFields { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        FieldIds = RequestUtil.Instance.GetQueryString("fieldids");
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        ItemIds = RequestUtil.Instance.GetQueryString("itemIds");

        var list =
            ServiceFactory.FieldService.GetListByColumnId(ColumnId)
                .Where(x => !FieldIds.ToInts(',').Contains(x.FieldId))
                .Select(x => x.FieldName);
        dynamicForm1.SubjectId = SubjectId;
        dynamicForm1.ColumnId = ColumnId;
        var fields = "";
        foreach (var fieldName in list)
        {
            if (!fields.IsEmpty())
                fields += ",";
            fields += fieldName;
        }
        dynamicForm1.ExceptFields  = fields;
        dynamicForm1.IsOpenFrame = true;

        if (!IsPostBack)
        {
            if (!FieldIds.IsEmpty())
            {
                PageMode = EnumPageMode.Update;

            }

        }
    }

}