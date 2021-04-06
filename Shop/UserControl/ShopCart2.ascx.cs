/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Shop_UserControl_ShopCart2.cs
 * 文件描述：购物车结算
 * 
 * 创建标识: lurong 2013-2-18
 * 
 * 修改标识：
 */
using System;
using System.Data;
using Shop.Common;
using Shop.Service;
using Whir.Framework;
using Whir.Service;
using Whir.Domain;
using Whir.Repository;
public partial class Shop_UserControl_ShopCart2 : UserControlBase
{
    public Decimal Total = 0;//总计
    public int Count = 0;//总数
    public string AreaPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        WebUser.IsLogin("shop/member/login.aspx?BackPageUrl="+Server.UrlEncode(Request.Url.ToStr()));
        
        if (!IsPostBack)
        {
            PayTypeList();
            BindUserAddress();
            BindCartList();
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (WebUser.IsLogin())
        {
            DataSet ds = ShopCartService.Instance.GetShopCartDetailed();
            if (ds.Tables[0].Select("IsBuy=0").Length > 0)
            {
                ScriptUtil.Instance.Alert("您的购物车中存在已下架商品，请移除后再购买", AppName + "shop/shopcart.aspx");
            }
            else
            {
                #region 初始化参数
                int paymentID = RequestUtil.Instance.GetFormString("PaymentIDs").ToInt(-1);
                if (paymentID == -1)
                {
                    ScriptUtil.Instance.Alert("暂无支付方式，无法提交订单");
                    return;
                }
                string address = TakeAddress.Text.Trim();
                string email = TakeEmail.Text.Trim();
                string mobile = TakeMobile.Text.Trim();
                string tel = TakeTel.Text.Trim();
                string name = TakeName.Text.Trim();
                string postcode = TakePostcode.Text.Trim();
                int region = TakeRegion.Text.ToInt(0);
                int orderID = 0;
                #endregion
                int result = ShopOrderInfoService.Instance.AddOrder(paymentID, address, email, mobile, tel
                , name, postcode, region, out orderID);
                if (result == 0)
                    ScriptUtil.Instance.Alert("提交成功!",AppName + "shop/shopcart3.aspx?orderId=" + orderID.ToStr());
                else
                    ScriptUtil.Instance.Alert("提交失败!");
            }
        }
    }


    #region 数据绑定
    /// <summary>
    /// 绑定购物车
    /// </summary>
    private void BindCartList()
    {
        try
        {
            DataSet ds = ShopCartService.Instance.GetShopCartDetailed();
            rptShopCart.DataSource = ds.Tables[0];
            rptShopCart.DataBind();

            rptShopCartTotal.DataSource = ds.Tables[0];
            rptShopCartTotal.DataBind();

            Count = ds.Tables[1].Rows[0][0].ToInt(0);
            Total = ds.Tables[2].Rows[0][0].ToDecimal(0);

        }
        catch
        {
            Response.Redirect(AppName+"Shop/ShopCart.aspx");
        }
    }

    /// <summary>
    /// 绑定用户地址
    /// </summary>
    private void BindUserAddress()
    {

        DataSet ds = DbHelper.CurrentDb.Query("SELECT * FROM Whir_Mem_Member WHERE  LoginName=@0", WebUser.GetUserValue("LoginName"));
         if (ds.Tables[0].Rows.Count > 0)
         {
             TakeName.Text = ds.Tables[0].Rows[0]["TakeName"].ToStr();
             TakeRegion.Text = ds.Tables[0].Rows[0]["TakeRegion"].ToStr();
             TakeAddress.Text =ds.Tables[0].Rows[0]["TakeAddress"].ToStr();
             TakeMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToStr();
             TakeTel.Text = ds.Tables[0].Rows[0]["TakeTel"].ToStr();
             TakeEmail.Text = ds.Tables[0].Rows[0]["TakeEmail"].ToStr();
             TakePostcode.Text = ds.Tables[0].Rows[0]["TakePostcode"].ToStr();
         }

        Area area = ServiceFactory.AreaService.SingleOrDefault<Area>(TakeRegion.Text.ToInt(0));
        if (area == null)
        {
            AreaPath = "0,0,0";
        }
        else
        {
            AreaPath = area.ParentPath + area.Id.ToStr();

            AreaPath=AreaPath.Substring(3);
            
        }
        hidArea.Value = AreaPath;
    }

    /// <summary>
    /// 绑定支付方式
    /// </summary>
    private void PayTypeList()
    {
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList(true);
        rpPayment.DataSource = dt;
        rpPayment.DataBind();
        ltNoRecord.Text = dt.Rows.Count > 0 ? "" : " 暂无支付方式";
    }
    #endregion
}