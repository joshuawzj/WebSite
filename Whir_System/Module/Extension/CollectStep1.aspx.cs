/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Cache;
using Whir.Cache.Enum;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_Module_Extension_CollectStep1 : SysManagePageBase
{
    /// <summary>
    /// 采集项目ID
    /// </summary>
    protected int CollectId { get; set; }

    protected Collect CurrentCollect { get; set; }

    protected IList<Column> Columns { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CollectId = RequestUtil.Instance.GetQueryInt("collectid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("38"));
            Columns = ServiceFactory.ColumnService.GetList(0, CurrentSiteId);

            BindEditInfo();

        }
    }

    private void BindEditInfo()
    {
        CurrentCollect = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId) ?? ModelFactory<Collect>.Insten();
    }


    protected void lbSaveContinue_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "next")
        {
            if (CollectId > 0)//编辑
            {
                var model = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId);
                if (model != null)
                {
                    GetModel(model);
                    ServiceFactory.CollectService.Update(model);

                    //清理缓存
                    string ruleCacheKey = CacheKeys.CollectionRulesPrefix + model.CollectId;
                    SiteCache.Remove(ruleCacheKey);

                    Response.Redirect(SysPath + "module/extension/CollectStep2.aspx?collectid=" + CollectId);
                }
                else
                {
                    ErrorAlert("未找到对应采集项目");
                }
            }
            else
            {
                var model = ModelFactory<Collect>.Insten();
                GetModel(model);
                CollectId = ServiceFactory.CollectService.Insert(model).ToInt();
                Response.Redirect(SysPath + "module/extension/CollectStep2.aspx?collectid=" + CollectId);
            }

        }
    }

    /// <summary>
    /// 获取模型
    /// </summary>
    /// <param name="model"></param>
    private void GetModel(Collect model)
    {
        //model.ItemName = TxtItemName.Text.Trim();
        //model.ColumnId = ddlColumns.SelectedValue.ToInt();
        //model.WebName = TxtWebName.Text.Trim();
        //model.WebUrl = TxtWebUrl.Text.Trim();
        //model.PageNum = TxtPageNum.Text.ToInt();
        //model.PageRules = TxtPageRules.Text.Trim();
        //model.IsOrderDesc = rblIsDesc.SelectedValue.ToBoolean();
        //model.Remark = TxtItemRemark.Text.Trim();
        //model.IsDownloadImages = rblIsDownloadImages.SelectedValue.ToBoolean();
    }
}