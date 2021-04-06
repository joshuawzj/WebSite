/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：form_edit.aspx.cs
 * 文件描述：站点栏目的表单输入项编辑页面
 *          
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.Config.Models;
using Whir.Config;

public partial class Whir_System_Module_Column_Form_BatchAdd : Whir.ezEIP.Web.SysManagePageBase
{
    #region URL参数

    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前编辑的表单输入项ID
    /// </summary>
    protected int FormId { get; set; }

    /// <summary>
    /// 已存在字段
    /// </summary>
    protected string ExsitFields { get; set; }

    //语言
    public string Lang { get; set; }

    #endregion URL参数

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(IsDevUser);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        FormId = RequestUtil.Instance.GetQueryInt("formid", 0);

        //var fields = ServiceFactory.FormService.GetListByColumnID(ColumnId);
        //foreach (Form field in fields)
        //{
        //    ExsitFields+=field
        //}

    }


}