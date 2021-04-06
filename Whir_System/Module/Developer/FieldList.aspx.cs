/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：modelfieldlist.aspx.cs
 * 文件描述：模型字段列表页
 *
 *          1. 获取模型ID, 模型ID任何情况下不应该为空, 根据模型ID在字段表中查出所有属于此模型的字段 
 *          2. 注:此处的字段是字段记录表中的数据, 非真实数据库中的列名
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_developer_modelfieldlist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前模型ID
    /// </summary>
    protected int ModelId { get; set; }
    public Model Model { get; set; }
    protected IList<Field> AllFieldList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ModelId = RequestUtil.Instance.GetQueryInt("modelid", 0);

        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            Model = ServiceFactory.ModelService.SingleOrDefault<Model>(ModelId)??new Model();
            AllFieldList = ServiceFactory.FieldService.GetListByModelId(ModelId);
        }
    }

}