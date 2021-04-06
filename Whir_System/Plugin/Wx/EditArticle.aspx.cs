using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class Whir_System_Plugin_Wx_EditArticle : WxBasePage {

    /// <summary>
    /// 文章编号
    /// </summary>
    protected string MediaId {
        get {
            return RequestUtil.Instance.GetQueryString("media").RemoveHtml();
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        JudgePagePermission(IsCurrentRoleMenuRes("394"));
    }
}