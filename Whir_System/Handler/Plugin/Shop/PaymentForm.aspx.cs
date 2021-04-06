using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using XmlUtil = Whir.Framework.XmlUtil;

using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Language;
using Whir.Service;
using PayInterface.Common;
using Whir.Config.Models;
using Whir.Config;
using Whir.Domain;

public partial class Whir_System_Handler_Plugin_order_PaymentForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 单个启用，禁用命令
    /// </summary>
    /// <returns></returns>
    public HandlerResult PaymentOperating()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("408"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string comName = RequestUtil.Instance.GetFormString("operName");
        string id = RequestUtil.Instance.GetFormString("id");
        if (comName.Equals("start"))
        {
            string mssage = UpdatePayTypeListXml(id, 1, true, "", "");
            return new HandlerResult { Status = true, Message = mssage };
        }
        else
        {
            string mssage = UpdatePayTypeListXml(id, 0, false, "", "");
            return new HandlerResult { Status = true, Message = mssage };
        }
    }
    /// <summary>
    /// 启用、禁用命令
    /// </summary>
    /// <returns></returns>
    public HandlerResult PaymentOperatingList()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("408"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string cmd = RequestUtil.Instance.GetFormString("operName");
        string ids = RequestUtil.Instance.GetFormString("ids").Trim();
        string massage = string.Empty;//提示
        switch (cmd)
        {
            //启用
            case "start":
                massage = UpdatePayTypeListXml(ids, 0, true, "", "");
                break;
            //禁用
            case "stop":
                massage = UpdatePayTypeListXml(ids, 0, false, "", "");
                break;
        }
        return new HandlerResult { Status = true, Message = massage };
    }
    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("408"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();
        string data = dt.ToJson();
        total = dt.Rows.Count;
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
    /// <summary>
    /// 更新支付方式列表 的xml文件，更新启用的状态
    /// </summary>
    /// <param name="ids">要操作的记录</param>
    /// <param name="type">0：标识批量启用、禁用，1：单个启用禁用</param>
    /// <param name="isUse">type=0时，要写正确，type=1时不作判断</param>
    /// <returns></returns>
    private string UpdatePayTypeListXml(string ids, int type, bool isUse, string descn, string name)
    {
        if (ids.Trim() == "") return string.Empty;
        try
        {
            string XmlPath = HttpContext.Current.Server.MapPath(PayInterface.Common.Tools.GetAppPath() + "PayTypeList.config");

            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(XmlPath);

            XmlNode root = doc.DocumentElement;

            string[] Ids = ids.Split(',');

            foreach (string s in Ids)
            {
                XmlNodeList nodeList = root.SelectNodes("descendant::item[id='" + s + "']");//查找节点
                switch (type)
                {
                    //批量启用、禁用
                    case 0:
                        if (isUse)
                        {
                            nodeList[0].ChildNodes[2].InnerText = "1";
                        }
                        else
                        {
                            nodeList[0].ChildNodes[2].InnerText = "0";
                        }
                        break;
                    //单个启用禁用
                    case 1:
                        nodeList[0].ChildNodes[2].InnerText = nodeList[0].ChildNodes[2].InnerText == "1" ? "0" : "1";
                        break;
                    case 2: //更改 启用、名称、以及描述
                        nodeList[0].ChildNodes[1].InnerText = name;
                        nodeList[0].ChildNodes[2].InnerText = isUse ? "1" : "0";
                        nodeList[0].ChildNodes[5].InnerText = descn;
                        break;
                }
            }
            doc.Save(HttpContext.Current.Server.MapPath(PayInterface.Common.Tools.GetAppPath() + "PayTypeList.config"));//更新xml文件

            //操作日志
            ServiceFactory.OperateLogService.Save("修改【PayTypeList.config】");
            return "操作成功".ToLang();
        }
        catch (Exception ex)
        {
            return "操作失败".ToLang();
        }
    }



    public HandlerResult Save()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("408"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string Name = RequestUtil.Instance.GetFormString("Name");
            bool IsStart = RequestUtil.Instance.GetFormString("IsStart").ToBoolean();
            string Account = RequestUtil.Instance.GetFormString("Account");
            string Md5Key = RequestUtil.Instance.GetFormString("Md5Key");
            string AlipayAccount = RequestUtil.Instance.GetFormString("AlipayAccount");
            string ReturnUrl = RequestUtil.Instance.GetFormString("ReturnUrl");
            string NotifyUrl = RequestUtil.Instance.GetFormString("NotifyUrl");
            string PaymentMode = RequestUtil.Instance.GetFormString("PaymentMode");
            string Descn = RequestUtil.Instance.GetFormString("Descn");
            string Id = RequestUtil.Instance.GetFormString("id");


            UpdatePayTypeListXml(Id, 2, IsStart, Descn, Name);//更新支付方式列表 的xml文件

            string PayType = PaymentMode;

            if (PayType == "alipay")
            {
                PayInterface.Model.Alipay model1 = new PayInterface.Model.Alipay().Insten();
                model1.PayOnlineKey = Md5Key.Trim();
                model1.V_Mid = Account.Trim();
                model1.Seller_email = AlipayAccount.Trim();
                model1.ReturnURL = ReturnUrl.Trim();
                model1.Notify_url = NotifyUrl.Trim();

                XmlUtil.SerializerObject(Tools.GetAppPath() + "alipay.config", typeof(PayInterface.Model.Alipay), model1);

            }
            else if (PayType == "alipayinstant")
            {
                PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                model.Seller_email = AlipayAccount.Trim();
                model.ReturnURL = ReturnUrl.Trim();
                model.Notify_url = NotifyUrl.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "alipayinstant.config", typeof(PayInterface.Model.AlipayInstant), model);

            }
            else if (PayType == "99bill")
            {
                PayInterface.Model._99Bill model = new PayInterface.Model._99Bill().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "99Bill.config", typeof(PayInterface.Model._99Bill), model);

            }
            else if (PayType == "chinabank")
            {
                PayInterface.Model.ChinaBank model = new PayInterface.Model.ChinaBank().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "ChinaBank.config", typeof(PayInterface.Model.ChinaBank), model);

            }
            else if (PayType == "chinapay")
            {
                PayInterface.Model.ChinaPay model = new PayInterface.Model.ChinaPay().Insten();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "ChinaPay.config", typeof(PayInterface.Model.ChinaPay), model);

            }
            else if (PayType == "cncard")
            {
                PayInterface.Model.CnCard model = new PayInterface.Model.CnCard().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "CnCard.config", typeof(PayInterface.Model.CnCard), model);

            }
            else if (PayType == "ipay")
            {
                PayInterface.Model.IPay model = new PayInterface.Model.IPay().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "IPay.config", typeof(PayInterface.Model.IPay), model);

            }
            else if (PayType == "ips")
            {
                PayInterface.Model.Ips model = new PayInterface.Model.Ips().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "Ips.config", typeof(PayInterface.Model.Ips), model);

            }
            else if (PayType == "tenpay")
            {
                PayInterface.Model.TenPay model = new PayInterface.Model.TenPay().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "TenPay.config", typeof(PayInterface.Model.TenPay), model);

            }
            else if (PayType == "xpay")
            {
                PayInterface.Model.Xpay model = new PayInterface.Model.Xpay().Insten();
                model.PayOnlineKey = Md5Key.Trim();
                model.V_Mid = Account.Trim();
                XmlUtil.SerializerObject(Tools.GetAppPath() + "Xpay.config", typeof(PayInterface.Model.Xpay), model);

            }
            //微信支付
            else if (PayType == "wechatpay")
            {
                var type = typeof(WeChatPayConfig);
                var model = ConfigHelper.GetWeChatPayConfig() ?? ModelFactory<WeChatPayConfig>.Insten();
                model.APPID = Account;
                model.KEY = Md5Key;
                model.MCHID = AlipayAccount;
                model.APPSECRET = ReturnUrl;
                model.NOTIFY_URL = NotifyUrl;
                // model = GetPostObject(type, model) as EmailConfig;
                XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("WeChatPayConfig.config"), type, model);
            }

            ServiceFactory.OperateLogService.Save("修改支付方式【{0}】".FormatWith(Name.Trim()));

            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = true, Message = "操作失败".ToLang() };

        }
    }

}