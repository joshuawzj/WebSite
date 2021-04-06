
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using PayInterface.Common;
using Whir.Framework;
using XmlUtil = Whir.Framework.XmlUtil;

using Whir.Service;
using Whir.Language;
using Whir.Config;
using Whir.Config.Models;
using Whir.Domain;
public partial class whir_system_Plugin_shop_order_payment_payment_edit : Whir.ezEIP.Web.SysManagePageBase
{
    //支付方式编号
    public string ItemId
    {
        get;
        set;
    }

    public string Name { get; set; }
    public int IsStart { get; set; }
    public string Account { get; set; }
    public string Md5Key { get; set; }
    public string AlipayAccount { get; set; }
    public string ReturnUrl { get; set; }
    public string NotifyUrl { get; set; }
    public string PaymentMode { get; set; }
    public string Descn { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ItemId = RequestUtil.Instance.GetString("ID");

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("408"));
            IntData();
            Bind();
        }
    }
    /// <summary>
    /// 加载绑定数据
    /// </summary>
    private void Bind()
    {
        
        //返回所有支付方式
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();

        DataRow[] foundRows = dt.Select("id='" + ItemId.ToStr() + "'");
        if (foundRows.Length > 0)
        {
            Name = foundRows[0]["name"].ToString();
            if (foundRows[0]["isopen"].ToString() == "1")
                IsStart = 1;

           PaymentMode= foundRows[0]["paytype"].ToString();
            Descn = foundRows[0]["introduce"].ToString();

            // PayRate.Text = foundRows[0]["paycharge"].ToString();

            string PayType = foundRows[0]["paytype"].ToString();
           PaymentMode= PayType;

            if (PayType == "alipay")
            {
                trAccount.InnerText = "合作身份者ID：".ToLang();
                trMd5Key.InnerText = "安全检验码：".ToLang();
                trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;

                PayInterface.Model.Alipay model1 = new PayInterface.Model.Alipay().Insten();
                NotifyUrl = model1.Notify_url;
                ReturnUrl = model1.ReturnURL;
                Md5Key = model1.PayOnlineKey.ToStr();
                Account = model1.V_Mid.ToStr();
                AlipayAccount = model1.Seller_email;
            }
            else if (PayType == "alipayinstant")
            {
                trAccount.InnerText = "合作身份者ID：".ToLang();
                trMd5Key.InnerText = "安全检验码：".ToLang();
                trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;

                PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
                NotifyUrl = model.Notify_url;
                ReturnUrl = model.ReturnURL;
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
                AlipayAccount = model.Seller_email;
            }
            else if (PayType == "alipaywap")
            {
                trAccount.InnerText = "合作身份者ID：".ToLang();
                trMd5Key.InnerText = "安全检验码：".ToLang();
                trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;

                PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
                NotifyUrl = model.Notify_url;
                ReturnUrl = model.ReturnURL;
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
                AlipayAccount = model.Seller_email;
            }
            else if (PayType == "99bill")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model._99Bill model = new PayInterface.Model._99Bill().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "chinabank")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.ChinaBank model = new PayInterface.Model.ChinaBank().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "chinapay")
            {
                tr2.Visible = false;
                trAccount.InnerText = "商户号：".ToLang();
                PayInterface.Model.ChinaPay model = new PayInterface.Model.ChinaPay().Insten();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "cncard")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.CnCard model = new PayInterface.Model.CnCard().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "ipay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.IPay model = new PayInterface.Model.IPay().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "ips")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.Ips model = new PayInterface.Model.Ips().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "tenpay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.TenPay model = new PayInterface.Model.TenPay().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "xpay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.Xpay model = new PayInterface.Model.Xpay().Insten();
                Md5Key = model.PayOnlineKey.ToStr();
                Account = model.V_Mid.ToStr();
            }
            else if (PayType == "wechatpay")
            {
                trAccount.InnerText = "绑定支付的APPID：".ToLang();
                trMd5Key.InnerText = "商户支付密钥：".ToLang();
                //trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;
                //trAlipayAccount.InnerText = "商户号";
                //trReturnUrl.InnerText = "公众帐号secert";
                //returnMsg.Visible = false;
                WeChatPayConfig wechatPayConfig = ConfigHelper.GetWeChatPayConfig();
                Account = wechatPayConfig.APPID;
                Md5Key = wechatPayConfig.KEY;
                AlipayAccount = wechatPayConfig.MCHID;
                ReturnUrl = wechatPayConfig.APPSECRET;
                NotifyUrl = wechatPayConfig.NOTIFY_URL;
            }
            else if (PayType == "cod")
            {
                tr1.Visible = tr2.Visible = false;
            }

        }
    }

    /// <summary>
    /// 初始最初值
    /// </summary>
    private void IntData()
    {
        trAccount.InnerText = "支付宝帐号：".ToLang();
        trMd5Key.InnerText = "安全校验码：".ToLang();
 
    }
   
     
}