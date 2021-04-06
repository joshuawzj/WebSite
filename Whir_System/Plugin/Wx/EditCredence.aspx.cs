using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Whir_System_Plugin_Wx_EditCredence : WxBasePage {

    /// <summary>
    /// 公众号信息
    /// </summary>
    protected WxCredence Credence { get; set; }

    protected void Page_Load(object sender, EventArgs e) {
        JudgePagePermission(IsCurrentRoleMenuRes("392"));
        this.Credence = WxConfigRepository.GetCredence(RequestString("appid")) ?? new WxCredence();
    }
}