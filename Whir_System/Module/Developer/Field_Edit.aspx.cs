/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：modelfield_edit.aspx.cs
 * 文件描述：模型字段添加和修改页面
 *
 *          1. 获取模型ID和当前字段ID, 模型ID任何情况下不应该为空, 字段ID为空时为添加字段, 不为空时为编辑字段
 *          2. 编辑字段时, 根据输入项, 修改字段表中的相关列, 注:此处不修改tableName中的列
 *          3. 添加字段时, 在字段记录表中添加记录, 在模型的tableName对应的表中添加列
 */

using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_developer_field_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前模型ID
    /// </summary>
    protected int ModelId { get; set; }

    /// <summary>
    /// 当前编辑的字段ID
    /// </summary>
    protected int FieldId { get; set; }

    /// <summary>
    /// 当前编辑的字段ID
    /// </summary>
    protected Field Field { get; set; }
    public Model Model { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        ModelId = RequestUtil.Instance.GetQueryInt("ModelId", 0);
        FieldId = RequestUtil.Instance.GetQueryInt("FieldId", 0);

        if (!IsPostBack)
        {
            Model = ServiceFactory.ModelService.SingleOrDefault<Model>(ModelId) ?? new Model();
            Field = ServiceFactory.FieldService.SingleOrDefault<Field>(FieldId) ?? new Field();
        }
    }


    /// <summary>
    /// 保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Command(object sender, CommandEventArgs e)
    {
        string cmdArgs = e.CommandArgument.ToStr();

        switch (PageMode)
        {
            case EnumPageMode.Insert:
                {
                    Field field = ModelFactory<Field>.Insten();
                    field.ModelId = ModelId;
                    //field.FieldType = ddlFieldType.SelectedValue.ToInt();
                    //field.FieldName = txtFieldName.Text.Trim();
                    //field.FieldAlias = txtFieldAlias.Text.Trim();
                    //field.IsSystemField = rblIsSystemField.SelectedValue == "1";
                    //field.IsHidden = rblIsHidden.SelectedValue == "1";
                    field.IsDefaultForm = false;
                    field.IsDefaultListShow = false;
                    field.IsEnableListShow = 1;

                    try
                    {
                        ServiceFactory.FieldService.AddField(field, ModelId);
                        //操作记录
                        ServiceFactory.OperateLogService.Save("添加字段【{0}】".FormatWith(field.FieldName));

                        switch (cmdArgs)
                        {
                            case "Save":
                                Alert("操作成功".ToLang(), SysPath + "module/developer/fieldlist.aspx?ModelId=" + ModelId);
                                break;
                            case "SaveContinue":
                                Alert("操作成功".ToLang(), true);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorAlert("操作失败：".ToLang() + ex.Message);
                        return;
                    }
                }
                break;
            case EnumPageMode.Update:
                {
                    Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(FieldId);
                    //field.FieldType = ddlFieldType.SelectedValue.ToInt();
                    //field.FieldName = txtFieldName.Text.Trim();
                    //field.FieldAlias = txtFieldAlias.Text.Trim();
                    //field.IsSystemField = rblIsSystemField.SelectedValue == "1";
                    //field.IsHidden = rblIsHidden.SelectedValue == "1";

                    ServiceFactory.FieldService.Update(field);
                    //操作日志
                    ServiceFactory.OperateLogService.Save("修改字段【{0}】".FormatWith(field.FieldName));
                    switch (cmdArgs)
                    {
                        case "Save":
                            Alert("操作成功".ToLang(), SysPath + "module/developer/fieldlist.aspx?ModelId=" + ModelId);
                            break;
                        case "SaveContinue":
                            Alert("操作成功".ToLang(), true);
                            break;
                    }
                }
                break;
        }


    }
}