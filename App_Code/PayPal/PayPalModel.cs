using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PayPalModel 的摘要说明
/// </summary>
public class PayPalModel
{
    public PayPalModel()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 订单标题
    /// </summary>
    public string Title
    { get; set; }

    /// <summary>
    /// 产品标识
    /// </summary>
    public string ProductID
    { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo
    { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal Amount
    { get; set; }

    private int quantity = 1;
    /// <summary>
    /// 商品数量
    /// </summary>
    public int Quantity
    { get { return quantity; } set { quantity = value; } }

    private int noshipping = 1;//默认不需要投递地址
    /// <summary>
    /// 是否需要投递地址
    /// </summary>
    public int NoShipping
    { get { return noshipping; } set { noshipping = value; } }

    private int nonote = 0; //默认不需要买家留言
    /// <summary>
    /// 是否允许买家留言
    /// </summary>
    public int NoNote
    { get { return nonote; } set { nonote = value; } }

   
    private string currencycode = "USD";
    /// <summary>
    /// 货币币种（默认美元）
    /// </summary>
    public string CurrencyCode
    { get { return currencycode; } set { currencycode = value; } }

    private string returnUrl = PayPalConfig.returnUrl;
    /// <summary>
    /// 即时回调地址
    /// </summary>
    public string ReturnUrl
    { get { return returnUrl; } set { returnUrl = value; } }

    private string cancelUrl = PayPalConfig.cancelUrl;
    /// <summary>
    /// 交易中止返回地址
    /// </summary>
    public string CancelUrl
    { get { return cancelUrl; } set { cancelUrl = value; } }


}