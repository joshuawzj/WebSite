/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：discuzconfig.aspx.cs
 * 文件描述：论坛单点登陆配置
 */
using System;

using Whir.Config;
using Whir.Config.Models;
using Whir.Framework;

public partial class whir_system_Plugin_discuz_discuzconfig : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BandInfo();
        }
    }

    /// <summary>
    /// 绑定数据到页面
    /// </summary>
    private void BandInfo()
    {
        DiscuzXml model = GetInfo();
        if (model != null)
        {
            txtDiscuzURL.Text = model.DiscuzURL;
            txtAPIKey.Text = model.APIKey;
            txtDiscuzKey.Text = model.DiscuzKey;
            txtServerName.Text = model.ServerName;
            txtLoginName.Text = model.LoginName;
            txtDatabaseName.Text = model.DataBaseName;
        }
    }

    /// <summary>
    /// 获取配置文档里的实体信息
    /// </summary>
    /// <returns></returns>
    private DiscuzXml GetInfo()
    {
        string xmlPath = System.IO.Path.GetDirectoryName(Page.Request.PhysicalPath) + "\\discuz.config";//获取配置文件路径
        DiscuzXml model = ConfigHelper.GetDiscuzXml(xmlPath);
        return model;
    }

    //保存
    protected void linkBtnSave_Click(object sender, EventArgs e)
    {
        string xmlPath = System.IO.Path.GetDirectoryName(Page.Request.PhysicalPath) + "\\discuz.config";//获取配置文件路径
        DiscuzXml Model = ConfigHelper.GetDiscuzXml(xmlPath);
        Model.DiscuzURL = txtDiscuzURL.Text.Trim();
        Model.APIKey = txtAPIKey.Text.Trim();
        Model.DiscuzKey = txtDiscuzKey.Text.Trim();
        Model.ServerName = txtServerName.Text.Trim();
        Model.DataBaseName = txtDatabaseName.Text.Trim();

        if (string.Empty != txtLoginPassword.Text)
        {
            Model.LoginPassword = txtLoginPassword.Text;
        }

        Type type = typeof(DiscuzXml);
        XmlUtil.SerializerObject(xmlPath, type, Model);

        Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("修改第三方用户集成，数据库名称【{0}】，密钥【{1}】，论坛Url【{2}】，数据库登录账户【{3}】，服务器名称【{4}】,API【{5}】", Model.DataBaseName, Model.DiscuzKey, Model.DiscuzURL, Model.LoginName, Model.ServerName, Model.APIKey));
        Alert("保存成功", true);
    }
}