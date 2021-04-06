using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Shop.Common;
using Shop.Domain;
using Shop.Service;
using Whir.Framework;

public partial class Shop_member_Orders : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUser.IsLogin("shop/member/login.aspx");
        if (!IsPostBack)
        {
            int cancelid = RequestUtil.Instance.GetQueryInt("cancelid",0);
            if (cancelid > 0)
            {
                int result = ShopOrderInfoService.Instance.CancelOrder(cancelid);
                switch (result)
                {
                    case -1:
                        Alert("订单取消失败!");
                        break;
                    case 0:
                        Alert("取消订单成功!", string.Format("orders.aspx?status={0}&page={1}", RequestUtil.Instance.GetQueryInt("status", 0), RequestUtil.Instance.GetQueryInt("page", 1)));
                        break;
                    case 1:
                        Alert("已支付订单不能取消!");
                        break;
                }
            }
            else
            {

                BindOrdes();
            }
        }
    }
    /// <summary>
    /// 绑定订单
    /// </summary>
    private void BindOrdes()
    {
        int status = RequestUtil.Instance.GetQueryInt("status", 0);
        string condition="";
        string key = Server.UrlDecode(RequestUtil.Instance.GetQueryString("key"));
        
        switch (status)
        { 
            case 1:
                condition = " AND IsCancel=0 AND Status>0";
                break;
            case 2:
                condition = " AND IsCancel=0 AND Status=0 AND IsPaid=1";
                break;
            case 3:
                condition = " AND IsCancel=1";
                break;
        }


        condition += " AND (OrderNo LIKE @1 OR (SELECT COUNT(*) FROM Whir_Shop_OrderProduct WHERE (ProNO LIKE @1 OR ProName LIKE @1) AND Whir_Shop_OrderInfo.OrderID=OrderID)>0)";
        //condition += " AND OrderNo LIKE @1";
        txtSearch.Text = key;
        key = "%" + key+"%";
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("SELECT Whir_Shop_OrderInfo.* FROM Whir_Shop_OrderInfo WHERE IsDel=0 AND MemberID=@0{0} ORDER BY CreateDate DESC", condition);
        var list = ShopOrderInfoService.Instance.Page(pager1.PageIndex, pager1.PageSize, sql.ToStr(), WebUser.GetUserValue("Whir_Mem_Member_PID"),key);
        rptOrder.DataSource = list.Items;
        rptOrder.DataBind();
        pager1.RecordsTotal = list.TotalItems.ToInt(0);
        if (list.TotalItems.ToInt(0) == 0)
            NoDate.Visible = true;

    }

    
    /// <summary>
    /// 生成统计及操作HTMl
    /// </summary>
    /// <param name="itemIndex">绑定控件数据项索引</param>
    /// <param name="ProductAmount">订单商品总金额</param>
    /// <param name="PayId">支付方式ＩＤ</param>
    /// <param name="IsPay">是否已支付</param>
    /// <param name="TakeName">收货人</param>
    /// <param name="OrderID">订单ID</param>
    /// <param name="IsCancel">是否取消</param>
    /// <param name="ProLength">商品种类数</param>
    /// <returns></returns>
    protected string GetOrderInofHtml(int itemIndex, string ProductAmount, string PayId, string IsPay, string TakeName, string OrderID,string IsCancel, string ProLength)
    {
        if (itemIndex > 0)
            return "";

        string payName="";
        try{
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();
        DataRow dr = dt.Select("id='" + PayId+"'")[0];
        payName = dr["name"].ToStr();
        }catch{}
       
        
        StringBuilder result = new StringBuilder();
        result.AppendFormat("<td width=\"120\" rowspan=\"{0}\"><b class=\"f_price\">{1}</b></td>", ProLength, ProductAmount);
        result.AppendFormat("<td width=\"100\" rowspan=\"{0}\"><span class=\"{1}\">{2}</span><br />{3}<br />{4}<br /></td>", ProLength, IsPay.ToBoolean(false)?"" : "f_red", IsPay.ToBoolean(false) ? "已支付" : "未支付", payName, TakeName);
        result.AppendFormat("<td width=\"80\" rowspan=\"{0}\"><a href=\"orderinfo.aspx?id={1}\" class=\"a_operate\">查看详细</a>", ProLength, OrderID);

        if (!IsPay.ToBoolean(false) && IsCancel.ToBoolean(false)==false)
        {
            result.AppendFormat("<br><a href=\"orders.aspx?cancelid={0}&status={1}&page={2}\" class=\"a_operate\">取消</a>", OrderID, RequestUtil.Instance.GetQueryInt("status", 0), RequestUtil.Instance.GetQueryInt("page", 1));
        }
        result.Append("</td>");
        return result.ToString();
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string url = string.Format("{0}shop/member/orders.aspx?status={1}&page={2}&key={3}", AppName, RequestUtil.Instance.GetQueryInt("status", 0)
            , RequestUtil.Instance.GetQueryInt("page", 1),Server.UrlEncode(txtSearch.Text.Trim()));

        Response.Redirect(url);
    }


    /// <summary>
    /// 绑定订单商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ShopOrderInfo order = (ShopOrderInfo)e.Item.DataItem;
        Repeater repeater = (Repeater)e.Item.FindControl("rptOrderPro");

        repeater.DataSource = ShopOrderProductService.Instance.GetShopOrderProductsByOrderID(order.OrderID);
        repeater.DataBind();
    }
}