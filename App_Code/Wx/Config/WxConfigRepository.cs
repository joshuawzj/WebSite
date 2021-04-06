using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whir.Config;
using Whir.Framework;

/// <summary>
/// 微信凭证信息持久化类
/// </summary>
public class WxConfigRepository {

    static WxCredenceConfig _WxCredenceConfig;

    static string CONFIG_PATH = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath") + "Plugin/Wx/Config/WxCredenceConfig.config";

    /// <summary>
    /// 获取公众号信息配置
    /// </summary>
    /// <returns></returns>
    public static WxCredenceConfig GetConfig() {
        if (_WxCredenceConfig == null) {
            Type type = typeof(WxCredenceConfig);
            WxCredenceConfig config = (WxCredenceConfig)XmlUtil.DeserializeObject(CONFIG_PATH, type);
            _WxCredenceConfig = config;
        }
        return _WxCredenceConfig;
    }

    /// <summary>
    /// 根据验证账户获取公众号信息
    /// </summary>
    /// <param name="appId">验证账户</param>
    /// <returns></returns>
    public static WxCredence GetCredence(string appId) {
        WxCredenceConfig config = GetConfig();
        if (config != null && config.CredenceSet != null) {
            return config.CredenceSet.Find(w => w.AppId == appId);
        }
        return null;
    }

    /// <summary>
    /// 新增信息
    /// </summary>
    /// <param name="credence">公众号信息</param>
    public static void AddCredence(WxCredence credence) {
        WxCredenceConfig config = GetConfig();
        if (config != null) {
            if (config.CredenceSet == null) {
                config.CredenceSet = new List<WxCredence>();
            }
            config.CredenceSet.Add(credence);
        }
        Serialize();
    }

    /// <summary>
    /// 更新信息
    /// </summary>
    /// <param name="credence">公众号信息</param>
    public static void UpdateCredence(WxCredence credence) {
        WxCredenceConfig config = GetConfig();
        if (config != null ) {
            if (config.CredenceSet == null) {
                config.CredenceSet = new List<WxCredence>();
            }
            WxCredence o = config.CredenceSet.Find(w => w.AppId == credence.AppId);
            if (o != null) {
                o = credence;
            }
            Serialize();
        }
    }

    /// <summary>
    /// 删除公众号信息
    /// </summary>
    /// <param name="appId"></param>
    public static void RemoveCredence(string[] appIds) {
        WxCredenceConfig config = GetConfig();
        if (config != null) {
            if (config.CredenceSet == null) {
                config.CredenceSet = new List<WxCredence>();
            }
            foreach (var app in appIds) {
                config.CredenceSet.RemoveAll(w => w.AppId == app);
            }
            Serialize();
        }
    }

    /// <summary>
    /// 持久化到文件
    /// </summary>
   static void Serialize() {
       XmlUtil.SerializerObject(CONFIG_PATH, typeof(WxCredenceConfig), _WxCredenceConfig);
    }
}