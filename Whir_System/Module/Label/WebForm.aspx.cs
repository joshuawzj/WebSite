/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：survey.aspx.cs
 * 文件描述：survey置标生成器
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Whir.Domain;
using Whir.Service;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_WebForm : SysManagePageBase
{
    public IList<SubmitForm> Model { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindModelList();
        }
    }
    //绑定提交表单类型下拉框,数据源为模型列表
    private void BindModelList()
    {
        Model = ServiceFactory.SubmitFormService.GetList().ToList();
    }
}