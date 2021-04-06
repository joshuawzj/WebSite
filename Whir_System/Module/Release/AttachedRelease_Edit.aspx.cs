/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：attachedrelease_edit.aspx.cs
 * 文件描述：添加附带发布
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Language;

public partial class whir_system_module_release_attachedrelease_edit : SysManagePageBase
{

    /// <summary>
    /// 当前编辑的菜单ID
    /// </summary>
    protected int AttachedID { get; set; }

    protected Attached CurrentAttached { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        AttachedID = WebUtil.Instance.GetQueryInt("attachedid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            if (AttachedID != 0)
            {
                PageMode = EnumPageMode.Update;
            }
            bindAttachInfo();
        }
    }


    /// <summary>
    /// 绑定数据
    /// </summary>
    private void bindAttachInfo()
    {
        CurrentAttached = ServiceFactory.AttachedService.SingleOrDefault<Attached>(this.AttachedID) ?? ModelFactory<Attached>.Insten();
    }


}