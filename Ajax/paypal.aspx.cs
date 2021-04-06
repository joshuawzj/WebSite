using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Ajax_paypal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
      
    }

    /// <summary>
    /// PayPal支付
    /// </summary>
    /// <param name="orderno">订单号</param>
    /// <param name="subject">主题</param>
    /// <param name="price">支付金额</param>
    /// <returns></returns>
    private string SendPayPal(string orderno, string subject, decimal price)
    {
        PayPalModel model = new PayPalModel();
        model.OrderNo = orderno;
        model.Amount = price;
        model.Title = subject;
        string sHtmlText = PayPalCore.Submit(model, false);
        return sHtmlText;

    }
}