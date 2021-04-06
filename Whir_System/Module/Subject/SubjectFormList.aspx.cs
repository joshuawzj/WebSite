/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subjectformlist.aspx.cs
 * 文件描述：专题/模板子站的表单列表页面
 */


using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_module_subject_subjectformlist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前的子站/专题类型ID
    /// </summary>
    protected int SubjectClassId { get; set; }

    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    protected IList<Column> ListColumnP { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindColumnList();
            BindFormList();
        }
    }

    //绑定栏目选项卡, 包含主栏目和附属栏目
    private void BindColumnList()
    {
        //添加附属栏目到集合中
        ListColumnP = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId);
        ltNoRecord.Text = ListColumnP.Count > 0 ? "" : "无数据".ToLang();
    }

    //绑定当前栏目的表单列表
    private void BindFormList()
    {
        IList<Form> listForm = ServiceFactory.FormService.GetListByColumnId(ColumnId);
        rptFormList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(FormList_ItemDataBound);
        rptFormList.DataSource = listForm;
        rptFormList.DataBind();

        ltNoRecord.Text = listForm.Count > 0 ? "" : "无数据".ToLang();
    }

    /// <summary>
    /// 表单列表行绑定行为
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FormList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Form form = e.Item.DataItem as Form;
            if (form == null) return;

            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);
            if (field != null)
            {

                HtmlInputCheckBox cbxSelected = e.Item.FindControl("cbxSelected") as HtmlInputCheckBox;
                Literal litFieldName = e.Item.FindControl("litFieldName") as Literal;
                Literal litTypeName = e.Item.FindControl("litTypeName") as Literal;
                Literal litIsSystemField = e.Item.FindControl("litIsSystemField") as Literal;
                Literal litIsHidden = e.Item.FindControl("litIsHidden") as Literal;

                PlaceHolder lbnDelete = e.Item.FindControl("lbnDelete") as PlaceHolder;

                //选择框
                if (cbxSelected != null)
                    cbxSelected.Visible = !field.IsSystemField;

                //数据库字段名
                if (litFieldName != null)
                    litFieldName.Text = field.FieldName;

                //字段类型
                if (litTypeName != null)
                    litTypeName.Text = ServiceFactory.FieldService.GetFieldTypeName(field.FieldType).ToLang();

                //是否系统字段
                if (litIsSystemField != null)
                    litIsSystemField.Text = GetTrueOrFalseImg(field.IsSystemField);

                //是否隐藏
                if (litIsHidden != null)
                    litIsHidden.Text = GetTrueOrFalseImg(field.IsHidden);

               
                if (lbnDelete != null)
                {
                    lbnDelete.Visible = !field.IsSystemField;
                }
            }

        }
    }




   

}