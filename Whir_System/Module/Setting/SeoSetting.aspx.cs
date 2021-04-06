/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：seosetting.aspx.cs
 * 文件描述：基本配置页面
 *
 *          1. 站点群的SEO配置, 版权和备案号配置
 *          2. 系统基本配置, 后台登录页面LOGO, 后台主页面LOGO, 分页大小, 分页范围等配置
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

using Whir.Config;
using Whir.Config.Models;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using System.Collections.Specialized;
using Whir.Controls.UI.Controls;

public partial class Whir_System_Module_Setting_SeoSetting : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的站点ID
    /// </summary>
    protected int SiteId { get; set; }

    protected SystemConfig SystemConfig { get; set; }
    protected UploadConfig UploadConfig { get; set; }

    protected string EditorCode { get; set; }
    protected List<SiteInfo> SiteInfo { get; set; }
    protected SiteInfo CurrentSiteInfo { get; set; }

    /// <summary>
    /// 无数据提示文字 输出的编辑器html 跟着系统设置的编辑器
    /// </summary>
    protected string NullTipHtml { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("27"));
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", CurrentSiteId);
        BindMutilSite();

    }

    //绑定多站点
    private void BindMutilSite()
    {
        SiteInfo = ServiceFactory.SiteInfoService.Query<SiteInfo>("WHERE IsDel=0").ToList();
        CurrentSiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId) ?? ModelFactory<SiteInfo>.Insten();

        //(1单行文本框，2单选按钮，3多选按钮，4多行文本框，5下拉框，6HTML编辑器，7文件上传)
        var field = new Field
        {
            FieldAlias = "无数据显示",
            FieldName = "NullTip",
            FieldId = Rand.Instance.Number(4, true).ToInt(),
            DefaultValue = ""
        };
        var control =
                    new ControlContext(new Editer(new Column(), new Form() { DefaultValue = "", FormId = Rand.Instance.Number(4, true).ToInt() }, field, RegularEnum.Never));
        NullTipHtml = control.Render(CurrentSiteInfo.NullTip);

    }

}