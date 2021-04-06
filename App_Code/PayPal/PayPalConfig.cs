using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PayPalConfig 的摘要说明
/// </summary>
public class PayPalConfig
{


    public static string sellerEmail = "renfan.daxian@yahoo.com"; //卖家Email
    public static string redirectUrl = "https://www.paypal.com/cgi-bin/webscr";  //网关地址
    public static string notifyUrl = "https://www.guangyancaijing.com/ajax/Return_Notify.aspx";    //ipn异步回调地址
    public static string returnUrl = "https://www.guangyancaijing.com/index.aspx";    //即时回调地址 
    public static string cancelUrl = "https://www.guangyancaijing.com/index.aspx";    //交易取消时的返回地址
    public PayPalConfig()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    //public string SellerEmail
    //{ get { return sellerEmail; } }

    //public string RedirectUrl
    //{ get { return redirectUrl; } }

    //public string NotifyUrl
    //{ get { return notifyUrl; } }

    //public string ReturnUrl
    //{ get { return returnUrl; } set { returnUrl = value; } }

    //public string CancelUrl
    //{ get { return cancelUrl; } set { cancelUrl = value; } }

      

   
}