/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：courierlist.aspx.cs
 * 文件描述：配送方式列表
 * 
 * 创建标识: liuyong 2013-02-05
 * 
 * 修改标识：lurong 2013-02-21  用快递公司代码字段替掉接口字段
 */
using System;
using System.Web.UI.WebControls;

using Shop.Domain;
using Shop.Service;
using Whir.Framework;

public partial class whir_system_Plugin_shop_order_courier_courierlist : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("409"));
            BandList();
        }
    }
    private void BandList()
    {
        //string SQL = "Where IsDel=0 Order By Sort Desc,Createdate Desc";
        //var list = ShopCourierService.Instance.Page(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, SQL);
        //rpList.DataSource = list.Items;
        //rpList.DataBind();
        ////总记录数
        //AspNetPager1.RecordCount = list.TotalItems.ToInt();

        //ltNoRecord.Text = AspNetPager1.RecordCount > 0 ? "" : "找不到记录";
    }

    /// <summary>
    /// 分页事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageChanged(object sender, EventArgs e)
    {
        BandList();
    }

    /// <summary>
    /// 订单行绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rpList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var lbDel = e.Item.FindControl("lbDel") as LinkButton;
            if (lbDel != null)
            {
                int courierId = lbDel.CommandArgument.ToInt();
                if (ShopOrderInfoService.Instance.ExecuteScalar<int>("SELECT COUNT(*) FROM Whir_Shop_OrderInfo WHERE CourierID=@0", courierId) > 0)
                {
                    lbDel.Text = "[删除]";
                    string callback = "__doPostBack('{0}','')".FormatWith(lbDel.UniqueID);
                    //lbDel.Attributes.Add("onclick", "return whir.dialog.confirm('此配送方式已被使用，删除会导致部分订单中的配送方式不显示，确认要删除吗？', function(){ " + callback + " })");
                    lbDel.Attributes.Add("confirmText", "此配送方式已被使用，删除会导致部分订单中的配送方式不显示，确认要删除吗？");
                    lbDel.Attributes.Add("class", "del_btn");
                    lbDel.Attributes.Add("confirmId", "lbDelete" + lbDel.ClientID);
                }
                else
                {
                    ConfirmDelete(lbDel);
                }
            }
        }
    }
    protected void rpList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("del"))
        {
            //规格组ID
            int courierId = e.CommandArgument.ToString().ToInt();
            //先删除对应的规格值
            ShopCourierService.Instance.Delete("Where CourierID=@0", courierId);
            ShopCourierService.Instance.Delete<ShopCourier>(courierId);
            Alert("操作成功", true);
        }
    }
}