using Shop.Domain;
using Shop.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Repository;

public partial class Whir_System_Handler_Plugin_shop_OrderForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    public Dictionary<int, string> PaymentList = new Dictionary<int, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();//支付列表
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PaymentList.Add(dt.Rows[i]["id"].ToInt(), dt.Rows[i]["name"].ToStr());
            }
        }
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 获取订单列表
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("407"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;

        var list = GetOrderList(pageIndex, pageSize);
        List<ShopOrderInfo> items = list.Items;
        if (items.Count > 0)
        {
            foreach (var ShopOrderInfo in items)
            {
                ShopOrderInfo.PaymentName = GetPayment(ShopOrderInfo.PaymentID);
                ShopOrderInfo.ShopOrderProducts = ShopOrderProductService.Instance.GetShopOrderProductsByOrderID(ShopOrderInfo.OrderID).ToJson();
            }
        }

        string data = items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    private Page<ShopOrderInfo> GetOrderList(int pageIndex, int pageSize)
    {
        string sql = "SELECT o.*,m.LoginName FROM Whir_Shop_OrderInfo o LEFT JOIN Whir_Mem_Member m ON o.MemberID=m.Whir_Mem_Member_PID  ";
        #region 搜索条件
        string where = "";
        where = " WHERE 1=1 ";
        int i = 0;
        var parms = new List<object>();

        string filter = RequestUtil.Instance.GetQueryString("filter");
        Dictionary<string, string> searchDic = ToDictionary(filter);
        foreach (var kv in searchDic)
        {
            if (kv.Value.Contains("<&&<"))
            {
                where += " and {0} between @{1} and @{2} ".FormatWith("o." + kv.Key, i++, i++);
                parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
            }
            else if (kv.Key.ToLower().Contains("paymentname"))
            {
                where += " and PaymentID = @{0} ".FormatWith(i++);
                parms.Add(kv.Value);
            }
            else if (kv.Key.ToLower().Contains("name") || kv.Key.ToLower().Contains("orderno"))
            {
                where += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
            else
            {
                where += " and {0} = @{1} ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
        }

        int OrderType = Whir.ezEIP.BasePage.RequestString("ordertype").ToInt(0);
        #region 拼接订单类型SQL语句
        switch (OrderType)
        {
            case 0:
                break;
            case 1:
                where += " AND IsCancel<>1 AND IsPaid=0 ";
                break;
            case 2:
                where += "  AND IsCancel<>1 AND IsPaid=1";//订单流程状态：0.交易完成；1.新建；2.待发货；3.待收货
                break;
            case 3:
                where += "  AND IsCancel<>1 AND Status <> 0  ";
                break;
            case 4:
                where += "  AND IsCancel<>1 AND Status = 0  ";
                break;
            case 5:
                where += "  AND IsCancel=1  ";
                break;
        }
        #endregion

        #endregion
        sql = sql + where.ToString() + " and o.IsDel=0 Order By o.Sort Desc,o.Createdate Desc ";

        return ShopOrderInfoService.Instance.Page(pageIndex, pageSize, sql, parms.ToArray());
    }

    private Dictionary<string, string> ToDictionary(string str)
    {
        try
        {
            if (str.IsEmpty())
                return new Dictionary<string, string>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<Dictionary<string, string>>(str);
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }


    /// <summary>
    /// 过滤搜索关键字
    /// </summary>
    /// <param name="keyWord">要过滤的字符串</param>
    /// <returns></returns>
    private string FilterHotWord(string keyWord)
    {
        if (keyWord.Contains("@"))
        {
            keyWord = keyWord.Replace("@", "@@");
        }
        return keyWord.Replace("'", "''").Trim();
    }

    /// <summary>
    /// 获取支付方式名称
    /// </summary>
    /// <returns></returns>
    public string GetPayment(int paymentId)
    {
        if (PaymentList.Count == 0)
        {
            DataTable dt = PayInterface.Common.Tools.GetPayTypeList();//支付列表
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PaymentList.Add(dt.Rows[i]["id"].ToInt(), dt.Rows[i]["name"].ToStr());
            }
        }

        string payment = string.Empty;
        PaymentList.TryGetValue(paymentId, out payment);
        return payment;
    }

    /// <summary>
    /// 更新订单状态
    /// </summary>
    /// <returns></returns>
    public HandlerResult UpdateOrderStatus()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("407"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string[] orderidAction = RequestUtil.Instance.GetFormString("orderidAction").Split('|');
        ShopOrderInfo model;
        if (orderidAction.Length < 2)
        {
            return new HandlerResult { Status = false, Message = "操作失败！" };
        }
        int result = 1;
        int orderId = orderidAction[0].ToInt(0);

        try
        {
            switch (orderidAction[1])
            {
                case "unpay":
                    model = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(orderId);
                    model.IsPaid = false;
                    result = ShopOrderInfoService.Instance.Update(model) > 0 ? 0 : 1;
                    break;
                case "pay":
                    model = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(orderId);
                    model.IsPaid = true;
                    result = ShopOrderInfoService.Instance.Update(model) > 0 ? 0 : 1;
                    break;
                case "unfinish":
                    model = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(orderId);
                    model.Status = 1;
                    model.FinishDate = null;
                    result = ShopOrderInfoService.Instance.Update(model) > 0 ? 0 : 1;
                    break;
                case "finish":
                    result = ShopOrderInfoService.Instance.CompleteTheOrder(orderId);
                    break;
                case "cancel":
                    result = ShopOrderInfoService.Instance.CancelOrder(orderId);
                    break;
            }
            return new HandlerResult { Status = true, Message = "操作成功！" };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败" + ex.Message };
        }
    }

    /// <summary>
    /// 保存信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("407"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int OrderID = RequestUtil.Instance.GetFormString("OrderID").ToInt(0);
        ShopOrderInfo takeModel = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(OrderID) ?? ModelFactory<ShopOrderInfo>.Insten();
        var type = typeof(ShopOrderInfo);
        takeModel = GetPostObject(type, takeModel) as ShopOrderInfo;
        if (takeModel.OrderID > 0)
        {
            ShopOrderInfoService.Instance.Update(takeModel);

            return new HandlerResult { Status = true, Message = "操作成功！" };
        }
        else
            return new HandlerResult { Status = false, Message = "操作失败！" };
    }

    /// <summary>
    /// 保存信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveAmount()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("407"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int OrderID = RequestUtil.Instance.GetFormString("OrderID").ToInt(0);
        decimal TotalAmount = RequestUtil.Instance.GetFormString("TotalAmount").ToDecimal(0);
        ShopOrderInfo takeModel = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(OrderID) ?? ModelFactory<ShopOrderInfo>.Insten();
        var type = typeof(ShopOrderInfo);
        takeModel = GetPostObject(type, takeModel) as ShopOrderInfo;
        if (takeModel.OrderID > 0)
        {
            //takeModel.PayAmount = takeModel.DiscountAmount + TotalAmount;

            ShopOrderInfoService.Instance.Update(takeModel);

            return new HandlerResult { Status = true, Message = "操作成功！" };
        }
        else
            return new HandlerResult { Status = false, Message = "操作失败！" };
    }


}