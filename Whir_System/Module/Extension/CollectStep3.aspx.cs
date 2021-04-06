/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;
using Whir.Repository;
using System.Web.Script.Serialization;

public partial class Whir_System_Module_Extension_CollectStep3 : Whir.ezEIP.Web.SysManagePageBase
{
    protected int CollectId { get; set; }

    protected Collect CollectModel { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("38"));
        CollectId = RequestUtil.Instance.GetQueryInt("collectid", 0);
        CollectModel = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId);
        if (CollectId <= 0)
        {
            Response.Redirect("CollectList.aspx");
            Response.End();
        }
        if (!IsPostBack)
        {
            if (CollectModel == null)
            {
                Response.Redirect("CollectList.aspx");
                Response.End();
            }
            else
            {
                lblItemName.Text = CollectModel.ItemName;
                BindFormList();
            }
        }
    }

    private void BindFormList()
    {
        var table = DbHelper.CurrentDb.Query(@"SELECT form.FormId,form.FieldAlias,form.DefaultValue,form.Sort FROM Whir_Dev_Form form INNER JOIN Whir_Dev_Field field ON form.FieldID=field.FieldID
                    WHERE form.IsDel=0 AND ColumnId=@0 AND field.IsHidden=0 AND field.FieldName!='RedirectUrl' AND field.fieldName!='EnableRedirectUrl' ORDER BY Sort Desc",
                    CollectModel.ColumnId).Tables[0];
        rptList.DataSource = table;
        rptList.DataBind();
    }

    /// <summary>
    /// 获取编辑信息
    /// </summary>
    /// <returns></returns>
    protected string GetEditInfo()
    {
        IList<CollectField> list = ServiceFactory.CollectFieldService.Query<CollectField>("WHERE CollectId=@0 AND IsDel=0", CollectId).ToList();
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(list);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbSaveContinue_Command(object sender, CommandEventArgs e)
    {
        //if (e.CommandName == "finish")
        //{
        //    string data = hidValue.Value.TrimEnd('□');
        //    string[] strArr = data.Split('□');
        //    IList<int> formidList = new List<int>();
        //    for (int i = 0; i < strArr.Length; i++)
        //    {
        //        string[] formidTypeValue = strArr[i].Split('§');
        //        if (formidTypeValue.Length < 3)
        //        {
        //            continue;
        //        }
        //        int formid = formidTypeValue[0].ToInt();
        //        formidList.Add(formid);
        //        int collectType = formidTypeValue[1].ToInt();
        //        string defaultValue = formidTypeValue[2].Trim();
        //        var model = ServiceFactory.CollectFieldService.Query<CollectField>("SELECT * FROM Whir_Ext_CollectField WHERE FormId=@0 AND CollectId=@1", formid, CollectId).FirstOrDefault();
        //        if (model == null)
        //        {
        //            model = ModelFactory<CollectField>.Insten();
        //            model.CollectId = CollectId;
        //            model.CollectType = collectType;
        //            model.FormId = formid;
        //            model.DefaultValue = defaultValue;
        //            ServiceFactory.CollectFieldService.Save(model);
        //        }
        //        else
        //        {
        //            model.CollectType = collectType;
        //            model.DefaultValue = defaultValue;
        //            ServiceFactory.CollectFieldService.Update(model);
        //        }
        //    }
        //    //清除之前保存不存在的表单
        //    ServiceFactory.CollectFieldService.Execute("DELETE FROM Whir_Ext_CollectField WHERE FormId NOT IN (@0) AND CollectId=@1", formidList.ToArray(), CollectId);
        //    Alert("设置成功", true);
        //}
    }
}