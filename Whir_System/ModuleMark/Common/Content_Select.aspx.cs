/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：content_select.aspx.cs
 * 文件描述：相关内容选择页面
 */

using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Framework;
using Whir.Service;
using Whir.Repository;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;
using Whir.Domain;
using System.Collections.Generic;

public partial class Whir_System_ModuleMark_Common_Content_Select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要排除的文章主键ID
    /// </summary>
    protected int ItemId { get; set; }

    /// <summary>
    /// 已经选择过的ID, 在此页面中不可再次选择
    /// </summary>
    protected string ExceptId { get; set; }

    /// <summary>
    /// 子站id
    /// </summary>
    protected int SubjectId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemId = RequestUtil.Instance.GetQueryInt("itemid", 0);
        ExceptId = RequestUtil.Instance.GetQueryString("exceptID");
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid",0);
        if (!IsPostBack)
        {
            contentManager1.ColumnId = ColumnId;
            contentManager1.SelectedType = SelectedType.CheckBox;
            contentManager1.IsDel = false;
            contentManager1.IsShowDelete = false;
            contentManager1.IsShowEdit = false;
            contentManager1.IsOpenFrame = true;
            contentManager1.SubjectId=SubjectId;
            if (ColumnId > 0)
            {
                var mainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
                var mainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(mainColumn.ModelId);
                if (ExceptId.Trim(',').Contains(","))
                {
                    var ids = string.Join(",", ExceptId.Split(',').Select(p => p.ToInt()).ToArray());  //先分割ExceptId，再转换int，再拼ids，防止中间有非数字类型
                    if (ids.Length > 0)
                        contentManager1.Where += "and  {0} not in ({1}) ".FormatWith(mainModel.TableName + "_PID", ids);
                }
                else
                    contentManager1.Where += " and {0}<>{1} ".FormatWith(mainModel.TableName + "_PID", ExceptId.ToInt());
                if (ItemId > 0)
                    contentManager1.Where += "and  {0}<>{1} ".FormatWith(mainModel.TableName + "_PID", ItemId);
            }

        }
    }

}