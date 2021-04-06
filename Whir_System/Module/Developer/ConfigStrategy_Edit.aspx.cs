/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：configstrategy_edit.aspx.cs
 * 文件描述：配置新增节点页面
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_module_developer_configstrategy_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 字段，配置集合
    /// </summary>
    private IList<ConfigSetting> list;

    #region 属性
    /// <summary>
    /// 配置设置Id
    /// </summary>
    public int ConfigSettingId { get; set; }

    /// <summary>
    /// 配置策略Id
    /// </summary>
    public int ConfigStrategyId { get; set; }
    
    /// <summary>
    /// 左边的配置树，选中的Id
    /// </summary>
    public string ShowSelectedId { get; set; }

    /// <summary>
    /// 配置的Id
    /// </summary>
    protected int Id { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        ConfigSettingId = RequestUtil.Instance.GetQueryInt("configsettingid", 0);
        ConfigStrategyId = RequestUtil.Instance.GetQueryInt("configstrategyid", 0);

        Id = RequestUtil.Instance.GetQueryInt("id", 2);

        ShowSelectedId = WebUtil.Instance.GetQueryString("showselectedid");

        if (!IsPostBack)
        {
            if (ConfigSettingId > 0)
            {
                //更新状态
                PageMode = EnumPageMode.Update;
                lbtnSubmitContinue.Visible = false;
                hlkEdit.Text = "<b>编辑节点</b>";
                BindData();

                txtKeyName.Enabled = false;
                lbtnGetNewValue.Visible = false;
                lbtnGetNewValue.Visible = false;

                spInfo.Visible = false;
            }
            else
            {
                //添加
                //绑定配置名称
                BindConfigName();
                ConfigSettingId = Id;

                txtKeyName.Enabled = true;
                lbtnGetNewValue.Visible = true;
                lbtnSubmitContinue.Visible = true;

                spInfo.Visible = true;
            }
        }
        else
        { 
            //回传,只有添加才回传，而不是编辑，不再使用了ConfigSettingId
            //ConfigSettingId = Id;  //这个可以解决添加，但是编辑有问题

            ConfigSettingId = Id > ConfigSettingId ? Id : ConfigSettingId;
        } 
    }

    /// <summary>
    /// 绑定当前编辑节点
    /// </summary>
    private void BindData()
    {
        try
        {
            ConfigStrategy model = ServiceFactory.ConfigStrategyService.SingleOrDefault<ConfigStrategy>(ConfigStrategyId);

            /*
            ddConfigs.SelectedItem.Text = model.ConfigStrategyName; 
            ddConfigs.SelectedItem.Value = model.ConfigSettingId.ToStr();
             */

            #region  绑定配置名称

             //获取 Xml中设置的配置
            list = ServiceFactory.ConfigStrategyService.ReadXml();

            //去掉父节点的集合
            list = ServiceFactory.ConfigStrategyService.RemoveParent(list);

            //判断
            if (model != null && list.Count > 0)
            {
                ConfigSetting configSetting = null;
                foreach (var item in list)
                {
                    if (item.ConfigId.Equals(model.ConfigSettingId))
                    {
                        configSetting = item;
                        break;
                    }
                }

                if (configSetting != null)
                {
                    //ddConfigs.DataSource = list;
                    //ddConfigs.DataTextField = "ConfigDescription";
                    //ddConfigs.DataValueField = "ConfigId";

                    ddConfigs.SelectedItem.Text = configSetting.ConfigDescription;
                    ddConfigs.SelectedItem.Value = configSetting.ConfigId.ToStr();

                }
            }

                   

            #endregion 绑定配置名称


            ddConfigs.Enabled = false;

            txtKeyName.Text = model.ConfigKey;
            //禁用 编辑 Key ,如果要禁用掉Key，那么也必须禁用 配置名称
            //txtKeyName.Enabled = false;

            if (model.ConfigStrategyName.Equals("WebConfig", StringComparison.OrdinalIgnoreCase))
            {
                txtKeyValue.Text = ServiceFactory.ConfigStrategyService.GetValueWebConfigFromXml(model.ConfigKey, model.ConfigStrategyName);
            }
            else
            {
                txtKeyValue.Text = ServiceFactory.ConfigStrategyService.GetValueNoWebConfigFromXml(model.ConfigKey, model.ConfigStrategyName);
            }
            txtDescription.Text = model.ConfigDescription;

            /*
            //获取 Xml中设置的配置
            list = ServiceFactory.ConfigStrategyService.ReadXml();

            //去掉父节点的集合
            list = ServiceFactory.ConfigStrategyService.RemoveParent(list);

            //判断
            if (model != null && list.Count>0)
            {
                ConfigSetting configSetting =null;
                foreach (var item in list)
                {
                    if (item.ConfigId.Equals(model.ConfigSettingId))
                    {
                        configSetting = item;
                        break;
                    }
                }

                if (configSetting != null)
                {
                    ddConfigs.DataSource = list;
                    ddConfigs.DataTextField = "ConfigDescription";
                    ddConfigs.DataValueField = "ConfigId";
                    
                    //出现不能问题，默认选中当前编辑的项，当时在框架中，SelecedIndex 为 0 的就必须重新选择，选中，同时在下拉列表框中，依然存在当前编辑的项。这样可以告诉用户当前编辑的是哪一项。更加人性化。
                  //  ddConfigs.SelectedItem.Text="==当前编辑："+configSetting.ConfigDescription+"，请选择==";  //把第一个项的Text修改为当前编辑的项的Text

                    //ddConfigs.DataBind();

                    ddConfigs.SelectedItem.Text = configSetting.ConfigDescription;
                    ddConfigs.SelectedItem.Value = configSetting.ConfigId.ToStr();

                    ddConfigs.Enabled = false;

                    txtKeyName.Text = model.ConfigKey;
                    //禁用 编辑 Key ,如果要禁用掉Key，那么也必须禁用 配置名称
                    //txtKeyName.Enabled = false;
                    txtKeyValue.Text = model.ConfigValue;
                    txtDescription.Text = model.ConfigDescription;
                }
            }*/


        }
        catch (Exception ex)
        {
            ErrorAlert("操作失败：".ToLang() + ex.Message);
        }
    }

    /// <summary>
    /// 绑定配置名称
    /// </summary>
    private void BindConfigName()
    {
        //获取 Xml中设置的配置
        list = ServiceFactory.ConfigStrategyService.ReadXml();
        ddConfigs.DataSource = ServiceFactory.ConfigStrategyService.RemoveParent(list);
        ddConfigs.DataTextField = "ConfigDescription";
        ddConfigs.DataValueField = "ConfigId";
        ddConfigs.DataBind();
    }

    protected void Save_Command(object sender, CommandEventArgs e)
    {
        //获取 Xml中设置的配置
        list = ServiceFactory.ConfigStrategyService.ReadXml();
        string cmdArgs = e.CommandArgument.ToStr();

        //判断
        if (list.Count > 0) 
        {
            //去掉父节点的集合
            list = ServiceFactory.ConfigStrategyService.RemoveParent(list);
                switch (PageMode)
                {
                    //插入
                    case EnumPageMode.Insert:
                        try
                        {
                            ConfigStrategy model = ModelFactory<ConfigStrategy>.Insten();
                            ConfigSetting configSetting = GetInputData(model);
                            model.ConfigStrategyName = configSetting.ConfigName;
                            model.ParentId = configSetting.ParentId;

                            //必须判断数据库表中是否已经存在当前的 
                            if (IsExistKeyAtDB(model))
                            {
                                Alert(string.Format("数据库中已经存在 {0} 配置文件中的键：{1} ",model.ConfigStrategyName,model.ConfigKey));
                                return;
                            }

                            if (IsExecuteOk(model, configSetting))
                            {
                                ServiceFactory.ConfigStrategyService.Insert(model);
                                //操作日志
                                ServiceFactory.OperateLogService.Save("插入【{0}】".FormatWith(model.ConfigStrategyName));
                                switch (cmdArgs)
                                {
                                    case "Save":
                                        Alert("操作成功", SysPath + "module/developer/configstrategy.aspx?id={0}&showselectedid={1}".FormatWith(Id,ShowSelectedId));
                                        break;
                                    case "SaveContinue":
                                        Alert("操作成功", true);
                                        break;

                                } 
                            }
                            else
                            {
                                Alert(string.Format("配置文件 {0} 中不存在键：{1} ", configSetting.ConfigName, model.ConfigKey));
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorAlert("添加失败");
                        }
                        break;
                    case EnumPageMode.Update:
                        try
                        {
                            ConfigStrategy model = ServiceFactory.ConfigStrategyService.SingleOrDefault<ConfigStrategy>(ConfigStrategyId);
                            ConfigSetting configSetting = GetInputData(model);


                            if (IsExecuteOk(model, configSetting))
                            {
                                bool isSuccess=ServiceFactory.ConfigStrategyService.Update(model)>0;
                                
                                //更新 
                                if (isSuccess)
                                {
                                    //操作日志
                                    ServiceFactory.OperateLogService.Save("修改【{0}】".FormatWith(model.ConfigStrategyName));
                                    switch (cmdArgs)
                                    {
                                        case "Save":

                                            Alert("操作成功", SysPath + "module/developer/configstrategy.aspx?id={0}&showselectedid={1}".FormatWith(ConfigSettingId, ShowSelectedId));
                                         
                                            break;
                                        case "SaveContinue":
                                            Alert("操作成功", true);
                                            break;

                                    }
                                }
                                else
                                {
                                    ErrorAlert("编辑失败");
                                }
                            }
                            else
                            {
                                ErrorAlert(string.Format("配置文件 {0} 中不存在键：{1} ", configSetting.ConfigName, model.ConfigKey));
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorAlert("编辑失败：" + ex.Message);
                        }
                        break;
                }
            }
    }

    /// <summary>
    /// 获取用户输入的数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private ConfigSetting GetInputData(ConfigStrategy model)
    {
        //获取用户输入的数据
        model.ConfigSettingId = ddConfigs.Text.ToInt();
        model.ConfigKey = txtKeyName.Text.Trim();
        model.ConfigValue = txtKeyValue.Text.Trim();
        model.ConfigDescription = txtDescription.Text.Trim();

        ConfigSetting configSetting = null;
        foreach (var item in list)
        {
            if (item.ConfigId.Equals(model.ConfigSettingId))
            {
                configSetting = item;
                break;
            }
        }
        return configSetting;
    }

    /// <summary>
    /// 是否执行插入、编辑成功
    /// </summary>
    /// <param name="model">配置策略实体对象</param>
    /// <param name="configSetting">配置实体对象</param>
    /// <returns></returns>
    private static bool IsExecuteOk(ConfigStrategy model, ConfigSetting configSetting)
    {
        //如果当前是 web.config
        if (configSetting.ConfigName.Equals("WebConfig"))
        {
            if (ServiceFactory.ConfigStrategyService.ReadXmlOfWebConfig(model.ConfigKey, model.ConfigValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //非web.config 新增操作
            if (ServiceFactory.ConfigStrategyService.ReadXmlOfNoWebConfig(configSetting.ConfigName, model.ConfigKey, model.ConfigValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
    /// <summary>
    /// 判断数据库中是否存在当前配置文件中的Key
    /// </summary>
    /// <param name="model">实体对象</param>
    /// <returns></returns>
    private bool IsExistKeyAtDB(ConfigStrategy model)
    {
        //必须同时以 model.ConfigKey; 和 model.ConfigStrategyName 来判断，因为可能是不同的配置文件中的Key相同了
        IList<ConfigStrategy> list = ServiceFactory.ConfigStrategyService.GetList().ToList<ConfigStrategy>();
       //判断
        if (list.Count > 0)
        { 
            //遍历
            foreach (var item in list)
            {
                if (item.ConfigKey.Equals(model.ConfigKey) && item.ConfigStrategyName.Equals(model.ConfigStrategyName))
                {
                    //数据库中已经存在当前配置文件的Key
                    return true ;  
                }
            }
        }
        return false;
    }
    protected void lbtnGetNewValue_Click(object sender, EventArgs e)
    {
        //tdKeyOne.Visible = false;
        //tdKeyTwo.Visible = false;
      string settingId=  ddConfigs.SelectedValue;
      string key = txtKeyName.Text.Trim();

      ConfigStrategy model = ServiceFactory.ConfigStrategyService.Query<ConfigStrategy>("WHERE  IsDel=0 AND ConfigSettingId={0}".FormatWith(settingId)).FirstOrDefault<ConfigStrategy>();

      string value = "";
      if (model.ConfigStrategyName.Equals("webconfig", StringComparison.OrdinalIgnoreCase))
      {
        value=  ServiceFactory.ConfigStrategyService.GetValueWebConfigFromXml(key, model.ConfigStrategyName);
      }
      else
      {
        value=  ServiceFactory.ConfigStrategyService.GetValueNoWebConfigFromXml(key, model.ConfigStrategyName);
      }
      if (string.IsNullOrEmpty(value))
      {
          Alert("当前键名{0}在配置文件{1}中不存在".FormatWith(key, model.ConfigStrategyName));
      }
      else
      {
          //tdKeyOne.Visible = true;
          //tdKeyTwo.Visible = true;
          txtKeyValue.Text = value;
      }
    }
}