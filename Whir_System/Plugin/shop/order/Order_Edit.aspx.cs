/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Order_Edit.aspx.cs
 * 文件描述：编辑订单
 * 
 * 创建标识: liuyong 2013-02-06
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Shop.Domain;
using Shop.Service;
using Whir.Language;

public partial class whir_system_Plugin_shop_order_Order_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 订单ID
    /// </summary>
    public int OrderID { get; set; }

    /// <summary>
    /// 支付方式字典
    /// </summary>
    public Dictionary<int, string> PaymentList = new Dictionary<int, string>();

    public ShopOrderInfo ShopOrderInfo { get; set; }

    /// <summary>
    /// 该订单是否允许编辑
    /// </summary>
    public bool IsCanEdit = false;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("407"));
        OrderID = RequestUtil.Instance.GetQueryInt("orderid", 0);

        if (OrderID == 0)
        {
            Response.Redirect("orderlist.aspx");
        }
        if (!IsPostBack)
        {
            BandInfo();
        }
    }
    
    /// <summary>
    /// 绑定信息
    /// </summary>
    private void BandInfo()
    {
        string SQL = "SELECT o.*,m.LoginName FROM Whir_Shop_OrderInfo o LEFT JOIN Whir_Mem_Member m ON o.MemberID=m.Whir_Mem_Member_PID WHERE o.OrderID=@0";
        ShopOrderInfo = ShopOrderInfoService.Instance.Query<ShopOrderInfo>(SQL, OrderID).SingleOrDefault();
        if (ShopOrderInfo != null)
        {
            #region 基本信息
            ltMember.Text = ShopOrderInfo.LoginName;
            ltOrderNo.Text = ShopOrderInfo.OrderNo;
            ltCreatedate.Text = ShopOrderInfo.CreateDate.ToString().Replace('/', '-');
            ltFinishDate.Text = ShopOrderInfo.FinishDate.ToString().Replace('/', '-');
            ltPayState.Text = ShopOrderInfo.IsPaid ? "已支付".ToLang() : "未支付".ToLang();
            ltProductAmount.Text = ShopOrderInfo.ProductAmount.ToString("f2");
            if (ShopOrderInfo.IsCancel)
            {
                ltStatus.Text = "已取消".ToLang();
            }
            else
            {
                ltStatus.Text = ShopOrderInfo.Status == 0 ? "已完成".ToLang() : "未完成".ToLang();

            }
            #endregion 基本信息

            #region 支付及配送方式

            DataTable dt = PayInterface.Common.Tools.GetPayTypeList();//支付列表

            ddlPayment.DataSource = dt;
            ddlPayment.DataTextField = "name";
            ddlPayment.DataValueField = "id";
            ddlPayment.DataBind();
            ddlPayment.Items.Insert(0, new ListItem("==请选择==".ToLang(), ""));
            ddlPayment.SelectedValue = ShopOrderInfo.PaymentID == 0 ? "" : ShopOrderInfo.PaymentID.ToStr();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PaymentList.Add(dt.Rows[i]["id"].ToInt(), dt.Rows[i]["name"].ToStr());
                }
            }

            ddlCourier.DataSource = ShopCourierService.Instance.Query<ShopCourier>("Where IsDel=0 Order By Sort Desc,Createdate Desc", null);
            ddlCourier.DataTextField = "CourierName";
            ddlCourier.DataValueField = "CourierID";
            ddlCourier.DataBind();
            ddlCourier.Items.Insert(0, new ListItem("==请选择==".ToLang(), ""));
            ddlCourier.SelectedValue = ShopOrderInfo.CourierID.ToStr();

            #endregion 支付及配送方式

            #region 商品信息
            DataTable dtProduct = ShopOrderProductService.Instance.GetShopOrderProductsByOrderID(OrderID);
            rpList.DataSource = dtProduct;
            rpList.DataBind();

            #endregion 商品信息

            #region 订单已完成或已取消时，不可编辑
            if (ShopOrderInfo.IsCancel || ShopOrderInfo.Status == 0)//订单已完成或已取消时，不可编辑
            {
                IsCanEdit = false;
            }
            else
                IsCanEdit = true;

            #endregion 订单已完成或已取消时，不可编辑
        }
    }



}