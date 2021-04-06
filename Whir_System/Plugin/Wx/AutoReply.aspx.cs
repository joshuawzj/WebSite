using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.MP;
using Whir.Framework;

public partial class Whir_System_Plugin_Wx_AutoReply : WxBasePage {

    /// <summary>
    /// 数据
    /// </summary>
    protected WxReply ViewData { get; set; }

    /// <summary>
    /// 数据JSON格式字符串
    /// </summary>
    protected string DataJSON { get; set; }

    protected void Page_Load(object sender, EventArgs e) {
        JudgePagePermission(IsCurrentRoleMenuRes("398"));
        WxCredence credence = this.CurrentCredence;
        if (credence != null) {
            this.ViewData = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Default).FirstOrDefault();
        }
        if (this.ViewData == null) {
            this.ViewData = new WxReply();
        }
        if (this.ViewData.Whir_Wx_ReplyId > 0) {
            this.DataJSON = WxReplyRepository.RepairData(this.ViewData).ToJson();
        }
    }

    
}