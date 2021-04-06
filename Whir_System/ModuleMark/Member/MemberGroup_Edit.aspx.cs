/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：membergroup_edit.aspx.cs
 * 文件描述：会员组添加、编辑页面
 */

using System;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.Language;
using Whir.Config;

public partial class Whir_System_ModuleMark_Member_MemberGroup_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    //主键
    public int GroupId { get; set; }

    public MemberGroup CurrentMemberGroup { get; set; }

    public string ImageUrl { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("299") || IsCurrentRoleMenuRes("303"));

        GroupId = RequestUtil.Instance.GetQueryInt("GroupID", -1);
        trImageUrl.Visible = ConfigHelper.GetMemberConfig().EnableMemGroupImage;
        if (GroupId > 0)
        {
            PageMode = EnumPageMode.Update;
        }
        CurrentMemberGroup = ServiceFactory.MemberGroupService.SingleOrDefault<MemberGroup>(GroupId) ??
                             ModelFactory<MemberGroup>.Insten();
        ImageUrl = CurrentMemberGroup.ImageUrl;

    }
}