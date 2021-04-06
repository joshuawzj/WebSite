using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whir.ezEIP.Web;

/// <summary>
/// 公众号相关基础页
/// </summary>
public class WxBasePage : Whir.ezEIP.Web.SysManagePageBase {

    WxCredence _CurrentCredence;

    /// <summary>
    /// 获取当前公众号
    /// </summary>
    protected WxCredence CurrentCredence {
        get {
            if (this._CurrentCredence == null) {
                this._CurrentCredence = WxUtility.GetCurrentCredence();
            }
            else if (_CurrentCredence.TokenExpired == null || _CurrentCredence.TokenExpired <= DateTime.Now)
            {
                this._CurrentCredence = WxUtility.GetCurrentCredence();
            }
            return this._CurrentCredence;
        }
    }

}