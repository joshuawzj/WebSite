using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class Whir_System_Plugin_Wx_Broadcast : WxBasePage {

    /// <summary>
    /// 标签
    /// </summary>
    protected string Tags { get; set; }

    protected void Page_Load(object sender, EventArgs e) {
        JudgePagePermission(IsCurrentRoleMenuRes("402"));
        WxCredence credence = this.CurrentCredence;
        if (credence != null) {
            ArrayList result = WxUtility.GetTags(credence.AccessToken);
            this.Tags = result.ToJson();
        }
    }
}