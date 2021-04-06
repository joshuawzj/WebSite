using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Service;
using Whir.Framework;


/// <summary>
/// 公众号信息推送接收地址
/// </summary>
public partial class Whir_System_Handler_Plugin_Wx_Callback : System.Web.UI.Page {

    protected void Page_Load(object sender, EventArgs e) {

        // 验证是否是公众号推送信息
        if (WxCallbackUtility.Authorizate(this.Request)) {
            this.Process();
        }

    }

    /// <summary>
    /// 处理推送的信息
    /// </summary>
    void Process() {

        Dictionary<string, string> data = WxCallbackUtility.GetPushData(this.Request);

        string msgType = WxCallbackUtility.GetDictionaryValue(data, "MsgType"), msgId = WxCallbackUtility.GetDictionaryValue(data, "MsgId"), returnValue = string.Empty;
        if (msgId.IsEmpty()) {
            msgId = WxCallbackUtility.GetDictionaryValue(data, "FromUserName") + WxCallbackUtility.GetDictionaryValue(data, "CreateTime") + DateTime.Now.Ticks;
        }
        switch (msgType) {
            case "event": // 事件类
                returnValue = WxCallbackUtility.ProcessEvent(data);
                break;
            case "text": // 文本类
                returnValue = WxCallbackUtility.ProcessText(data);
                break;
            default:
                returnValue = Request.QueryString["echostr"];
                break;
        }
        try {
            WxMessage message = WxMessageRepository.Find(msgId);
            if (message == null) {
                WxMessageRepository.Add(new WxMessage() {
                    CreateDate = DateTime.Now,
                    FormData = data.ToJson(),
                    FromUserName = WxCallbackUtility.GetDictionaryValue(data, "FromUserName"),
                    MessageId = msgId,
                    MessageType = msgType,
                    ReplyData = returnValue,
                    ToUserName = WxCallbackUtility.GetDictionaryValue(data, "ToUserName")
                });
            }
        }
        catch (Exception ex) {
            ServiceFactory.OperateLogService.Save(ex.Message + "=>" + ex.StackTrace);
        }

        Response.Write(returnValue);
        Response.Flush();
        Response.Close();
        //ServiceFactory.OperateLogService.Save(data.ToJson() + "=>" + returnValue);
    }
}