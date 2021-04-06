/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：modellist.aspx.cs
 * 文件描述：模型列表
 *
 *          1. 删除模型时, 删除在模型表中的记录, 在字段表中删除此模型的所有字段记录, 删除此模型tableName对应的表
 *          2. 注: 删除模型时, 会关联性的删除子模型, 如"简单信息"会关联删除"简单信息_类别"
 */

using System;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using System.Collections.Generic;
using System.Linq;

public partial class Whir_System_Module_Developer_ModelList : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 所有模型
    /// </summary>
    protected List<Model> AllModelList { get; set; }
    protected List<Model> ModelTreeList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        AllModelList = ServiceFactory.ModelService.GetList()
            .OrderBy(p => p.TableName)
            .ToList();

        ModelTreeList = new List<Model>();
        foreach (var model in AllModelList.Where(p=>p.ParentId==0&&!p.IsDel))
        {
            ModelTreeList.Add(model);
            var subModels = AllModelList.Where(p => p.ParentId == model.ModelId).OrderBy(p => p.ModelName).ToList();
            ModelTreeList.AddRange(subModels);
        }
    }
    
}