/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：configstrategy..aspx.cs
 * 文件描述：配置策略页面
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Linq;
using System.Web.Script.Serialization;

using Whir.ezEIP.Web;
using Whir.Service;
using Whir.Framework;
using Whir.Domain;

public partial class whir_system_module_developer_configstrategy : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 字段，配置集合
    /// </summary>
    IList<ConfigSetting> configSettingList;

    /// <summary>
    /// 左边的配置树，选中的Id
    /// </summary>
    public string ShowSelectedId { get; set; }

    /// <summary>
    /// 配置设置Id
    /// </summary>
    protected int ConfigSettingId { get; set; }

    #endregion 属性

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowSelectedId = WebUtil.Instance.GetQueryString("showselectedid");
        configSettingList = ServiceFactory.ConfigStrategyService.ReadXml();
        ltNoRecord.Text = "";
        ConfigSettingId = RequestUtil.Instance.GetQueryInt("id", 2);  //默认为2 ,因为1为父节点，没有任何数据记录

        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            //默认
            if (2 == ConfigSettingId)
            {
                ShowSelectedId = "browser_9";
            }
            try
            {
                //if (ConfigSettingId <= 1)
                //{
                //    //绑定数据
                //    BindData();

                //}
                //else
                //{
                var objs = ServiceFactory.ConfigStrategyService.Query<ConfigStrategy>("WHERE ConfigSettingId=@0 AND IsDel=0", ConfigSettingId).ToList<ConfigStrategy>();

                if (objs.Count > 0)
                {
                    rptConfigs.DataSource = objs;
                    rptConfigs.DataBind();
                }
                else
                {
                    ltNoRecord.Text = "没有任何记录";
                }
                // }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return;
            }
        }
        else
        {
            //一打开页面就点击刷新
            if (string.IsNullOrEmpty(ShowSelectedId))
            {
                ConfigSettingId = 2;
                ShowSelectedId = "browser_9";
            }
        }
    }

    /// <summary>
    /// 绑定树
    /// </summary>
    /// <returns></returns>
    protected string BindTree()
    {
        var ctList = configSettingList.Select(cs => new
        {
            id = cs.ConfigId,
            name = cs.ConfigDescription,
            pId = cs.ParentId,
            open = cs.ParentId == 0
        });
        return ctList.ToJson();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        //每一次绑定都首先清空
        ltNoRecord.Text = "";

        IList<ConfigStrategy> list = ServiceFactory.ConfigStrategyService.Query<ConfigStrategy>("WHERE ConfigSettingId={0} AND IsDel=0 ORDER BY CreateDate DESC ".FormatWith(ConfigSettingId)).ToList<ConfigStrategy>();

        //判断
        if (list.Count>0)  //错误的，不是 list != null ，而是 list.Count >0
        {
            rptConfigs.DataSource = list;
            rptConfigs.DataBind();
        }
        else
        {
            ltNoRecord.Text = "没有任何记录";
        }
    }

    #region 事件

    /// <summary>
    /// 行 事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptConfigs_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string commandArgs = e.CommandArgument.ToStr();
        //删除
        if (e.CommandName.Equals("del"))
        {
            if (ServiceFactory.ConfigStrategyService.Delete<ConfigStrategy>(commandArgs.ToInt()) == 0)
            {
                ErrorAlert("删除失败");
            }
        }
        BindData();
    }

    /// <summary>
    /// 行 绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptConfigs_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lbtnDel = e.Item.FindControl("lbtnDel") as LinkButton;

            if (lbtnDel != null)
            {
                ConfirmDelete(lbtnDel);
            }
        }
    } 
    #endregion
   
    //刷新
    protected void imgbtnRef_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            IList<ConfigStrategy> list = ServiceFactory.ConfigStrategyService.ReadXmlToUpdate(ConfigSettingId);

            rptConfigs.DataSource = list;
            rptConfigs.DataBind();
        }
        catch (Exception ex)
        {
            Alert(ex.Message);
        }
    }
}