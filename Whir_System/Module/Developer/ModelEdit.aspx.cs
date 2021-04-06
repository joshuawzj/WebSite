/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：model_edit.aspx.cs
 * 文件描述：模型添加和编辑页
 *
 *          1. 获取模型ID, 模型ID为空时为添加, 不为空时为编辑
 *          2. 修改模型时, 只修改模型表中的部分字段
 *          3. 添加模型时, 在模型表中添加记录, 在字段表中添加此模型的默认字段, 创建此模型tableName对应的表, 并在这些表里面添加字段
 *          4. 注: 添加模型时, 会关联性的添加子模型, 如"简单信息"会关联添加"简单信息_类别"
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Module_Developer_ModelEdit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的ID
    /// </summary>
    protected int ModelId { get; set; }
    /// <summary>
    /// 所有功能模型，用于绑定下拉框
    /// </summary>
    protected List<Module> AllModule { get; set; }
    protected Model CurrentModel { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        ModelId = RequestUtil.Instance.GetQueryInt("modelid", 0);
        CurrentModel = ServiceFactory.ModelService.SingleOrDefault<Model>(ModelId)
                       ?? ModelFactory<Model>.Insten();
        AllModule =ServiceFactory.ModuleService.GetListByParentID(0).OrderBy(p => p.ModuleName).ToList();
    }
    
}