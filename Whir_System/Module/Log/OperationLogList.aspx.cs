/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：operationloglist.aspx.cs
 * 文件描述：日志操作列表页面
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Log_OperationLogList : Whir.ezEIP.Web.SysManagePageBase
{
    
    /// <summary>
    /// 日志操作类型,0:网络操作日志，1：模版操作日志，2：系统运行日志
    /// </summary>
    public int OperateType = 0;



    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("39"));
        OperateType = RequestUtil.Instance.GetQueryInt("type", 0);
      
    }
}
