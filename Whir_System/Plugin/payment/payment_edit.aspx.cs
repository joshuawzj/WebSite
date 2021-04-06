/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：payment_edit.aspx.cs
 * 文件描述：支付方式编辑
 * 
 * 创建标识: liuyong 2012-08-21
 * 
 * 修改标识：
 */
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

public partial class whir_system_Plugin_payment_payment_edit : Whir.ezEIP.Web.SysManagePageBase
{
    //支付方式编号
    public string ItemId
    {
        get;
        set;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ItemId = RequestUtil.Instance.GetString("ID");

        if (!IsPostBack)
        {
            IntData();
            Bind();
        }
    }
    /// <summary>
    /// 加载绑定数据
    /// </summary>
    private void Bind()
    {
        CheckBox1.Text = "开启".ToLang();
        //返回所有支付方式
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();

        DataRow[] foundRows = dt.Select("id='" + ItemId.ToStr() + "'");
        if (foundRows.Length > 0)
        {
            Name.Text = foundRows[0]["name"].ToString();
            if (foundRows[0]["isopen"].ToString() == "1") CheckBox1.Checked = true;

            ddlPaymentMode.Text = foundRows[0]["paytype"].ToString();
            txtDescn.Text = foundRows[0]["introduce"].ToString();

            // PayRate.Text = foundRows[0]["paycharge"].ToString();

            string PayType = foundRows[0]["paytype"].ToString();
            ddlPaymentMode.Text = PayType;

            if (PayType == "alipay")
            {
                trAccount.InnerText = "合作身份者ID：".ToLang();
                trMd5Key.InnerText = "安全检验码：".ToLang();
                trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;

                PayInterface.Model.Alipay model1 = new PayInterface.Model.Alipay().Insten();
                txtNotifyUrl.Text = model1.Notify_url;
                txtReturnUrl.Text = model1.ReturnURL;
                Md5Key.Text = model1.PayOnlineKey.ToStr();
                Account.Text = model1.V_Mid.ToStr();
                txtAlipayAccount.Text = model1.Seller_email;
            }
            else if (PayType == "alipayinstant")
            {
                trAccount.InnerText = "合作身份者ID：".ToLang();
                trMd5Key.InnerText = "安全检验码：".ToLang();
                trurl1.Visible = trurl2.Visible = trAlipay.Visible = true;

                PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
                txtNotifyUrl.Text = model.Notify_url;
                txtReturnUrl.Text = model.ReturnURL;
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
                txtAlipayAccount.Text = model.Seller_email;
            }
            else if (PayType == "99bill")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model._99Bill model = new PayInterface.Model._99Bill().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "chinabank")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.ChinaBank model = new PayInterface.Model.ChinaBank().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "chinapay")
            {
                tr2.Visible = false;
                trAccount.InnerText = "商户号：".ToLang();
                PayInterface.Model.ChinaPay model = new PayInterface.Model.ChinaPay().Insten();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "cncard")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.CnCard model = new PayInterface.Model.CnCard().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "ipay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.IPay model = new PayInterface.Model.IPay().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "ips")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.Ips model = new PayInterface.Model.Ips().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "tenpay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.TenPay model = new PayInterface.Model.TenPay().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
            }
            else if (PayType == "xpay")
            {
                trAccount.InnerText = "商户号：".ToLang();
                trMd5Key.InnerText = "私钥：".ToLang();
                PayInterface.Model.Xpay model = new PayInterface.Model.Xpay().Insten();
                Md5Key.Text = model.PayOnlineKey.ToStr();
                Account.Text = model.V_Mid.ToStr();
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

        foreach (ListItem li in ddlPaymentMode.Items)
        {
            li.Text = li.ToLang();
        }
    }
   
    /// <summary>
    /// 点击保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Command(object sender, CommandEventArgs e)
    {
        //判断是否重复提交
        KillRefresh();

        if (txtDescn.Text.Trim().Length > 255)
        {
            Alert("详细介绍长度限制在255个字符以内，不能出现“&lt;”、“&gt;”符号".ToLang());
            return;
        }

        UpdatePayTypeListXml();//更新支付方式列表 的xml文件

        string PayType = ddlPaymentMode.Text;

        if (PayType == "alipay")
        {
            PayInterface.Model.Alipay model1 = new PayInterface.Model.Alipay().Insten();
            model1.PayOnlineKey = Md5Key.Text.Trim();
            model1.V_Mid = Account.Text.Trim();
            model1.Seller_email = txtAlipayAccount.Text.Trim();
            model1.ReturnURL = txtReturnUrl.Text.Trim();
            model1.Notify_url = txtNotifyUrl.Text.Trim();

            XmlUtil.SerializerObject(Tools.GetAppPath() + "alipay.config", typeof(PayInterface.Model.Alipay), model1);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{1}】，通知Url【{0}】，返回Url【{2}】，卖家Email【{3}】".FormatWith(model1.Notify_url, Name.Text.Trim(), model1.ReturnURL, model1.Seller_email));
            
        }
        else if (PayType == "alipayinstant")
        {
            PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            model.Seller_email = txtAlipayAccount.Text.Trim();
            model.ReturnURL = txtReturnUrl.Text.Trim();
            model.Notify_url = txtNotifyUrl.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "alipayinstant.config", typeof(PayInterface.Model.AlipayInstant), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{1}】，通知Url【{0}】，返回Url【{2}】，卖家Email【{3}】".FormatWith(model.Notify_url, Name.Text.Trim(), model.ReturnURL, model.Seller_email));

        }
        else if (PayType == "99bill")
        {
            PayInterface.Model._99Bill model = new PayInterface.Model._99Bill().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "99Bill.config", typeof(PayInterface.Model._99Bill), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{1}】，通知Url【{0}】，".FormatWith(model.Notify_url,Name.Text.Trim()));
        }
        else if (PayType == "chinabank")
        {
            PayInterface.Model.ChinaBank model = new PayInterface.Model.ChinaBank().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "ChinaBank.config", typeof(PayInterface.Model.ChinaBank), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith(Name.Text.Trim()));
        }
        else if (PayType == "chinapay")
        {
            PayInterface.Model.ChinaPay model = new PayInterface.Model.ChinaPay().Insten();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "ChinaPay.config", typeof(PayInterface.Model.ChinaPay), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith( Name.Text.Trim()));
        }
        else if (PayType == "cncard")
        {
            PayInterface.Model.CnCard model = new PayInterface.Model.CnCard().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "CnCard.config", typeof(PayInterface.Model.CnCard), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith(Name.Text.Trim()));
        }
        else if (PayType == "ipay")
        {
            PayInterface.Model.IPay model = new PayInterface.Model.IPay().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "IPay.config", typeof(PayInterface.Model.IPay), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith(Name.Text.Trim()));
        }
        else if (PayType == "ips")
        {
            PayInterface.Model.Ips model = new PayInterface.Model.Ips().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "Ips.config", typeof(PayInterface.Model.Ips), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith( Name.Text.Trim()));
        }
        else if (PayType == "tenpay")
        {
            PayInterface.Model.TenPay model = new PayInterface.Model.TenPay().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "TenPay.config", typeof(PayInterface.Model.TenPay), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付【{0}】".FormatWith( Name.Text.Trim()));
        }
        else if (PayType == "xpay")
        {
            PayInterface.Model.Xpay model = new PayInterface.Model.Xpay().Insten();
            model.PayOnlineKey = Md5Key.Text.Trim();
            model.V_Mid = Account.Text.Trim();
            XmlUtil.SerializerObject(Tools.GetAppPath() + "Xpay.config", typeof(PayInterface.Model.Xpay), model);

            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付方式【{0}】".FormatWith(Name.Text.Trim()));
        }
        else  //只是为了记录 货到付款 操作日志
        {
            //记录操作日志
            ServiceFactory.OperateLogService.Save("修改支付方式【{0}】".FormatWith(Name.Text.Trim()));
        }
        Alert("更新成功".ToLang(), SysPath + "Plugin/payment/paymentlist.aspx");
    }

    /// <summary>
    /// 更新支付方式列表 的xml文件
    /// </summary>
    private void UpdatePayTypeListXml()
    {
        string XmlPath = HttpContext.Current.Server.MapPath(PayInterface.Common.Tools.GetAppPath() + "PayTypeList.config");

        XmlDocument doc = new XmlDocument();
        doc.XmlResolver = null;
        doc.Load(XmlPath);

        XmlNode root = doc.DocumentElement;

        XmlNodeList nodeList = root.SelectNodes("descendant::item[id='" + ItemId.ToStr() + "']");//查找节点


        nodeList[0].ChildNodes[1].InnerText = Name.Text.Trim().ToStr();
        if (CheckBox1.Checked)
        {
            nodeList[0].ChildNodes[2].InnerText = "1";
        }
        else
        {
            nodeList[0].ChildNodes[2].InnerText = "0";
        }

        nodeList[0].ChildNodes[5].InnerText = txtDescn.Text.Trim().ToStr();
        //   nodeList[0].ChildNodes[3].InnerText = PayRate.Text.Trim().ToString();

        doc.Save(HttpContext.Current.Server.MapPath(PayInterface.Common.Tools.GetAppPath() + "PayTypeList.config"));//更新xml文件

    }
}