/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：paymentlist.aspx.cs
 * 文件描述：支付方式管理
 * 
 * 创建标识: liuyong 2012-08-21
 * 
 * 修改标识：
 */
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Language;
using Whir.Service;

public partial class whir_system_Plugin_payment_paymentlist : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BandList();
            //提示选择
            AlertIsCheck(lbStart, "whir.checkbox.isSelect('cb_Position')");
            AlertIsCheck(lbStop, "whir.checkbox.isSelect('cb_Position')");
        }
    }
    #region 绑定数据方法
    /// <summary>
    /// 绑定列表
    /// </summary>
    private void BandList()
    {
       DataTable dt= PayInterface.Common.Tools.GetPayTypeList();
       rpPayment.DataSource = dt;
        rpPayment.DataBind();

        ltNoRecord.Text =dt.Rows.Count > 0 ? "" : "找不到记录".ToLang();
    }

    /// <summary>
    /// 判断有没有选择
    /// </summary>
    /// <param name="linkButton">LinkButton控件</param>
    /// <param name="condition">判断条件</param>
    public void AlertIsCheck(LinkButton linkButton, string condition)
    {
        if (linkButton == null) return;
        string script = @"if({0}==false){{
                                    TipMessage('{1}'); 
                                    return false; 
                                }}".FormatWith(condition, "请选择".ToLang());
        script = Regex.Replace(script, @"\s+", " ");
        linkButton.Attributes.Add("onclick", script);
    }
    #endregion

    #region 页面事件

    /// <summary>
    /// 启用、禁用命令
    /// </summary>
    protected void Link_Command(object sender, CommandEventArgs e)
    {
        string cmd = e.CommandName;
        string ids = Request["cb_Position"].ToStr().Trim();
        string massage = string.Empty;//提示
        switch (cmd)
        {
            //启用
            case "start":
                massage = UpdatePayTypeListXml(ids, 0, true);
                break;
            //禁用
            case "stop":
                massage = UpdatePayTypeListXml(ids, 0, false);
                break;
        }
        Alert(massage);
    }

    /// <summary>
    /// 单个启用，禁用命令
    /// </summary>
    protected void rpPayment_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string comName = e.CommandName;
        string id = e.CommandArgument.ToStr();
        if (comName.Equals("start"))
        {
            string mssage = UpdatePayTypeListXml(id, 1, true);
            Alert(mssage);
        }
    }
    #endregion

    #region XML操作方法

    /// <summary>
    /// 更新支付方式列表 的xml文件，更新启用的状态
    /// </summary>
    /// <param name="ids">要操作的记录</param>
    /// <param name="type">0：标识批量启用、禁用，1：单个启用禁用</param>
    /// <param name="isUse">type=0时，要写正确，type=1时不作判断</param>
    /// <returns></returns>
    private string UpdatePayTypeListXml(string ids, int type, bool isUse)
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
                }
            }
            doc.Save(HttpContext.Current.Server.MapPath(PayInterface.Common.Tools.GetAppPath() + "PayTypeList.config"));//更新xml文件

            //操作日志
            ServiceFactory.OperateLogService.Save("修改【PayTypeList.config】");

            BandList();
            return "操作成功".ToLang();
        }
        catch (Exception ex)
        {
            return "操作失败".ToLang();
        }
    }
    #endregion

    
}