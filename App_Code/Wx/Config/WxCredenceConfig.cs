using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 公众号信息配置
/// </summary>
public class WxCredenceConfig {

    /// <summary>
    /// 凭证集合
    /// </summary>
    public List<WxCredence> CredenceSet { get; set; }

}

public class WxCredence {

    /// <summary>
    /// 原始ID
    /// </summary>
    public string OriginalId { get; set; }

    /// <summary>
    /// 微信账户
    /// </summary>
    public string WxAccount { get; set; }

    /// <summary>
    /// 验证账户
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 验证密钥
    /// </summary>
    public string AppSecret { get; set; }

    /// <summary>
    /// 账户名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 自定义令牌
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 访问令牌
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 令牌过期时间
    /// </summary>
    public DateTime TokenExpired { get; set; }

}