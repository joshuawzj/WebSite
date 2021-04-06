/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SendEmailLogEdit.aspx.cs
 * 文件描述：邮件群发发送编辑页面
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Plugin_Email_SendEmailLogEdit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 发送记录ID
    /// </summary>
    protected int RecordId { get; set; }

    protected SendEmailRecord CurrentRecord { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        RecordId = RequestUtil.Instance.GetQueryInt("recordId", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("366"));
            LoadInfo();
        }
    }

    /// <summary>
    /// 加载绑定信息
    /// </summary>
    private void LoadInfo()
    {
        StringBuilder builder = new StringBuilder("可用变量：".ToLang());
        IList<Field> listField = ServiceFactory.FieldService.Query<Field>("SELECT field.* FROM Whir_Dev_Field field INNER JOIN Whir_Dev_Form form ON field.FieldID=form.FieldID WHERE field.ModelID=1 AND field.IsHidden=0").ToList();
        foreach (var item in listField)
        {
            builder.Append("<spen>  " + item.FieldAlias + " - </spen>");
            builder.Append("<a href=\"javascript:Insert('{" + item.FieldName + "}',1)\">{" + item.FieldName + "}，</a>");
        }
        Div_Mark.InnerHtml = builder.ToStr() + "<span>  "+"点击认证地址".ToLang()+" - </spen><a href=\"javascript:Insert('{Click}',1)\">{Click}</a>";

        SendEmailRecord model = ServiceFactory.SendEmailRecordService.SingleOrDefault<SendEmailRecord>(RecordId) ?? ModelFactory<SendEmailRecord>.Insten(); ;
        CurrentRecord = model;
        if (model != null)
        {
            switch (model.Category)
            {
                case 0:

                    ltCatgory.Text = "按选择会员".ToLang();
                    break;
                case 1:
                    ltCatgory.Text = "按会员类别".ToLang();
                    //显示会员类别名称
                    MemberGroup group=Whir.Service.ServiceFactory.MemberGroupService.SingleOrDefault<MemberGroup>(model.MemberGroupId);
                    ltGroupName.Text = group == null ? "" : " ({0})".FormatWith(group.GroupName);
                    break;
                case 2:
                    ltCatgory.Text = "指定邮箱".ToLang();
                    break;
            }

           

            if (model.SendState == 3 && model.Category == 1)//按会员组，并且还没发送的
            {
                var memberGroup = ServiceFactory.MemberGroupService.SingleOrDefault<MemberGroup>(model.MemberGroupId);
                if (memberGroup != null)
                {
                    model.FailEmail = "会员组：{0}，所有会员".ToLang().FormatWith(memberGroup.GroupName);
                }
            }
        }

    }

    
}