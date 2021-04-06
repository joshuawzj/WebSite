using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// PayPalSubmit 的摘要说明
/// </summary>
public class PayPalCore
{

    public PayPalCore()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 支付请求
    /// </summary>
    /// <param name="model">请求实体</param>
    /// <param name="IsNewBlank">是否新窗口打开</param>
    /// <returns></returns>
    public static string Submit(PayPalModel model, bool IsNewBlank)
    {
        StringBuilder s = new StringBuilder();
        string formid = "paypalPayment";// +Guid.NewGuid().ToString("N");
        s.Append("<form id=\""+formid+"\" name=\""+formid+"\" action=\"" + PayPalConfig.redirectUrl + "\" method=\"post\" target=\"" + (IsNewBlank ? "_blank" : "_self") + "\" accept-charset=\"UTF-8\">\r\n");
        s.Append("<input name=\"cmd\" type=\"hidden\" value=\"_xclick\">\r\n");
        s.Append("<input name=\"business\" type=\"hidden\" value=\"" + PayPalConfig.sellerEmail + "\">\r\n");
        s.Append("<input name=\"item_name\" type=\"hidden\" value=\"" + model.Title + "\">\r\n");
        s.Append("<input name=\"item_number\" type=\"hidden\" value=\"" + model.ProductID + "\">\r\n");
        s.Append("<input name=\"amount\" type=\"hidden\" value=\"" + model.Amount + "\">\r\n");
        s.Append("<input name=\"quantity\" type=\"hidden\" value=\"" + model.Quantity + "\">\r\n");
        s.Append("<input name=\"no_shipping\" type=\"hidden\" value=\"" + model.NoShipping + "\">\r\n");
        s.Append("<input name=\"currency_code\" type=\"hidden\" value=\"" + model.CurrencyCode + "\">\r\n");
        s.Append("<input name=\"invoice\" type=\"hidden\" value=\"" + model.OrderNo + "\">\r\n");
        s.Append("<input type=\"hidden\" name=\"return\" value=\"" + model.ReturnUrl + "\">\r\n");
        s.Append("<input type=\"hidden\" name=\"cancel_return\" value=\"" + model.CancelUrl + "\">\r\n");
        s.Append("<input type=\"hidden\" name=\"notify_url\" value=\"" + PayPalConfig.notifyUrl + "\">\r\n");
        s.Append("<input name=\"btnSend\" type=\"submit\" value=\"Send\" style=\"display:none;\" />\r\n");
        s.Append("</form>\r\n");
        s.Append("<script>document.forms['" + formid + "'].submit();</script>\r\n");
        PayPalLog.Info(typeof(PayPalCore).GetType().ToString(), "发送支付请求！订单号【" + model.OrderNo + "】");
        return s.ToString();
    }

    public static string Submit(PayPalModel model)
    {
        return Submit(model, false);
    }

    /// <summary>
    /// 验证回调
    /// </summary>
    /// <returns></returns>
    public static bool VerifyIPN()
    {
        string strFormValues = HttpContext.Current.Request.Form.ToString();
        string strNewValue;
        string strResponse;
        string serverURL = PayPalConfig.redirectUrl;
        PayPalLog.Info(typeof(PayPalCore).GetType().ToString(), "回调验证！参数：" + strFormValues);
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(serverURL);
            req.Method = "POST";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; // (SecurityProtocolType)3072
            req.ContentType = "application/x-www-form-urlencoded";
            strNewValue = strFormValues + "&cmd=_notify-validate";
            req.ContentLength = strNewValue.Length;

            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), Encoding.UTF8);
            stOut.Write(strNewValue);
            stOut.Close();

            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();
            PayPalLog.Info(typeof(PayPalCore).GetType().ToString(), "回调验证！参数：" + strFormValues + "  返回值：" + strResponse);
            return strResponse == "VERIFIED";
        }
        catch (Exception ex)
        {
            PayPalLog.Info(typeof(PayPalCore).GetType().ToString(), "回调验证！异常：" + ex.Message+"  "+ex.Source+"  "+ex.StackTrace);
            return false;
        }
    }
}