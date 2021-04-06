using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml;
using Whir.Framework;

/// <summary>
/// 公众号推送相关功能类
/// </summary>
public static class WxCallbackUtility
{

    /// <summary>
    /// 获取字典指定键的值
    /// </summary>
    /// <param name="dic">字典</param>
    /// <param name="key">键名</param>
    /// <returns></returns>
    public static string GetDictionaryValue(this Dictionary<string, string> dic, string key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key].ToStr();
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取微信推送的信息
    /// </summary>
    /// <param name="request">请求实例</param>
    /// <returns></returns>
    public static Dictionary<string, string> GetPushData(System.Web.HttpRequest request)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.XmlResolver = null;
            doc.Load(request.InputStream);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (XmlNode node in doc.SelectNodes("xml/*"))
            {
                dic[node.Name] = node.InnerText;
            }
            return dic;
        }
        catch (Exception ex)
        {
            LogHelper.Log(ex);
        }
        return new Dictionary<string, string>();

    }

    /// <summary>
    /// 获取URL参数集合
    /// </summary>
    /// <param name="request">请求实例</param>
    /// <returns></returns>
    public static Dictionary<string, string> GetQueryStrings(System.Web.HttpRequest request)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (var item in request.QueryString.AllKeys)
        {
            dic[item] = request.QueryString[item].ToStr();
        }
        return dic;
    }

    /// <summary>
    /// 验证信息
    /// </summary>
    /// <param name="request">请求实例</param>
    /// <returns></returns>
    public static bool Authorizate(System.Web.HttpRequest request)
    {
        Dictionary<string, string> dic = GetQueryStrings(request);
        if (dic.ContainsKey("token") && dic.ContainsKey("signature") && dic.ContainsKey("timestamp") && dic.ContainsKey("nonce"))
        {
            string[] list = { dic["token"], dic["timestamp"], dic["nonce"] };
            Array.Sort(list);　　 //字典排序 
            string data = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join("", list), "SHA1").ToLower();
            return data == dic["signature"];
        }
        return false;
    }

    /// <summary>
    /// 获取回复内容
    /// </summary>
    /// <param name="reply">回复信息</param>
    /// <param name="wxAccount">公众号对应微信号</param>
    /// <param name="toUser">接收人openid</param>
    /// <returns></returns>
    static string GetReturnValue(WxReply reply, string wxAccount, string toUser)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<xml>");
        builder.AppendLine("    <ToUserName><![CDATA[{0}]]></ToUserName>".FormatWith(toUser));
        builder.AppendLine("    <FromUserName><![CDATA[{0}]]></FromUserName>".FormatWith(wxAccount));
        builder.AppendLine("    <CreateTime>{0}</CreateTime>".FormatWith(Convert.ToInt32((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds)));
        builder.AppendLine("    <MsgType><![CDATA[{0}]]></MsgType>".FormatWith(reply.ReplyDataType == Senparc.Weixin.MP.ReplyDataType.article ? "news" : reply.ReplyDataType.ToStr()));
        switch (reply.ReplyDataType)
        {
            case Senparc.Weixin.MP.ReplyDataType.article:
                List<WxArticle> articles = WxArticleRepository.Find(reply.MessageDetail);
                builder.AppendLine("    <ArticleCount>{0}</ArticleCount>".FormatWith(articles.Count));
                builder.AppendLine("    <Articles>");
                articles.ForEach(m =>
                {
                    builder.AppendLine("    <item>");
                    builder.AppendLine("        <Title><![CDATA[{0}]]></Title>".FormatWith(m.Title));
                    builder.AppendLine("        <Description><![CDATA[{0}]]></Description>".FormatWith(m.Summary));
                    builder.AppendLine("        <PicUrl><![CDATA[{0}]]></PicUrl>".FormatWith(m.ThumbMediaWebURL));
                    builder.AppendLine("        <Url><![CDATA[{0}]]></Url>".FormatWith(m.ViewURL));
                    builder.AppendLine("    </item>");
                });
                builder.AppendLine("    </Articles>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.image:
                builder.AppendLine("    <Image>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                builder.AppendLine("    </Image>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.text:
                builder.AppendLine("    <Content><![CDATA[{0}]]></Content>".FormatWith(reply.MessageDetail));
                break;
            case Senparc.Weixin.MP.ReplyDataType.video:
                WxSampleMedia media = WxSampleMediaRepository.Find(reply.MessageDetail);
                builder.AppendLine("    <Video>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                if (media != null)
                {
                    builder.AppendLine("        <Title><![CDATA[{0}]]></Title>".FormatWith(media.Title));
                    builder.AppendLine("        <Description><![CDATA[{0}]]></Description>".FormatWith(media.Introduction));
                }
                builder.AppendLine("    </Video>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.voice:
                builder.AppendLine("    <Voice>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                builder.AppendLine("    </Voice>");
                break;
        }
        builder.AppendLine("</xml>");
        return builder.ToStr();
    }


    /// <summary>
    /// 获取回复内容
    /// </summary>
    /// <param name="reply">回复信息</param>
    /// <param name="wxAccount">公众号对应微信号</param>
    /// <param name="toUser">接收人openid</param>
    /// <returns></returns>
    static string GetReturnValue(WxReply reply, string wxAccount, string toUser, string msgId)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<xml>");
        builder.AppendLine("    <ToUserName><![CDATA[{0}]]></ToUserName>".FormatWith(toUser));
        builder.AppendLine("    <FromUserName><![CDATA[{0}]]></FromUserName>".FormatWith(wxAccount));
        builder.AppendLine("    <CreateTime>{0}</CreateTime>".FormatWith(Convert.ToInt32((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds)));
        builder.AppendLine("    <MsgType><![CDATA[{0}]]></MsgType>".FormatWith(reply.ReplyDataType == Senparc.Weixin.MP.ReplyDataType.article ? "news" : reply.ReplyDataType.ToStr()));
        switch (reply.ReplyDataType)
        {
            case Senparc.Weixin.MP.ReplyDataType.article:
                List<WxArticle> articles = WxArticleRepository.Find(reply.MessageDetail);
                builder.AppendLine("    <ArticleCount>{0}</ArticleCount>".FormatWith(articles.Count));
                builder.AppendLine("    <Articles>");
                articles.ForEach(m =>
                {
                    builder.AppendLine("    <item>");
                    builder.AppendLine("        <Title><![CDATA[{0}]]></Title>".FormatWith(m.Title));
                    builder.AppendLine("        <Description><![CDATA[{0}]]></Description>".FormatWith(m.Summary));
                    builder.AppendLine("        <PicUrl><![CDATA[{0}]]></PicUrl>".FormatWith(m.ThumbMediaWebURL));
                    builder.AppendLine("        <Url><![CDATA[{0}]]></Url>".FormatWith(m.ViewURL));
                    builder.AppendLine("    </item>");
                });
                builder.AppendLine("    </Articles>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.image:
                builder.AppendLine("    <Image>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                builder.AppendLine("    </Image>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.text:
                builder.AppendLine("    <Content><![CDATA[{0}]]></Content>".FormatWith(reply.MessageDetail));
                break;
            case Senparc.Weixin.MP.ReplyDataType.video:
                WxSampleMedia media = WxSampleMediaRepository.Find(reply.MessageDetail);
                builder.AppendLine("    <Video>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                if (media != null)
                {
                    builder.AppendLine("        <Title><![CDATA[{0}]]></Title>".FormatWith(media.Title));
                    builder.AppendLine("        <Description><![CDATA[{0}]]></Description>".FormatWith(media.Introduction));
                }
                builder.AppendLine("    </Video>");
                break;
            case Senparc.Weixin.MP.ReplyDataType.voice:
                builder.AppendLine("    <Voice>");
                builder.AppendLine("        <MediaId><![CDATA[{0}]]></MediaId>".FormatWith(reply.MessageDetail));
                builder.AppendLine("    </Voice>");
                break;
        }
        builder.AppendLine("<MsgId>{0}</MsgId>".FormatWith(msgId));
        builder.AppendLine("</xml>");
        return builder.ToStr();
    }

    /// <summary>
    /// 同步用户信息
    /// </summary>
    /// <param name="appId">公众号账户</param>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="openid">用户编号</param>
    public static void SyncUser(string appId, string accessToken, string openid)
    {
        var factory = new System.Threading.Tasks.TaskFactory();
        var task = factory.StartNew(() =>
        {
            WxUserJsonResult user = WxUtility.GetUser(accessToken, openid);
            if (user != null)
            {
                WxUser wx = WxUserRepository.Map(user);
                int userId = WxUserRepository.FindPrimaryKey(wx.openid);
                if (userId > 0)
                {
                    // 更新
                    wx.Whir_Wx_UsersId = userId;
                    wx.appid = appId;
                    WxUserRepository.Update(wx);
                }
                else
                {
                    // 添加
                    wx.appid = appId;
                    WxUserRepository.Add(wx);
                }
            }
        });

    }

    #region 消息处理

    /// <summary>
    /// 处理事件类型
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ProcessEvent(Dictionary<string, string> data)
    {
        if ("event".Equals(data.GetDictionaryValue("MsgType")))
        {
            string eventType = data.GetDictionaryValue("Event"), eventKey = data.GetDictionaryValue("EventKey"), returnValue = string.Empty;
            string fromUser = data.GetDictionaryValue("FromUserName"), toUser = data.GetDictionaryValue("ToUserName");

            WxCredence credence = WxConfigRepository.GetConfig().CredenceSet.Find(m => m.OriginalId == toUser) ?? new WxCredence();
            switch (eventType)
            {
                case "subscribe":
                    WxReply reply = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Subscribe).FirstOrDefault();
                    if (reply != null)
                    {
                        returnValue = GetReturnValue(reply, toUser, fromUser);
                    }
                    else
                    {
                        returnValue = GetReturnValue(new WxReply()
                        {
                            ReplyDataType = Senparc.Weixin.MP.ReplyDataType.text,
                            MessageDetail = "感谢您的关注~！"
                        }, toUser, fromUser);
                    }
                    SyncUser(credence.AppId, credence.AccessToken, fromUser);
                    break;
                case "unsubscribe":
                    // 删除关注用户
                    WxUserRepository.Remove(fromUser);
                    break;
                case "SCAN":
                    if (eventKey == "1")
                    {
                        string ticke = data.GetDictionaryValue("Ticket");
                        WxQrcode code = WxQrcodeRepository.Find(ticke);
                        if (code != null)
                        {
                            WxReply codeReply = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Keyword, code.ExtraData).FirstOrDefault();
                            if (codeReply == null)
                            {
                                codeReply = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Default).FirstOrDefault();
                                if (codeReply == null)
                                {
                                    codeReply = new WxReply()
                                    {
                                        ReplyDataType = Senparc.Weixin.MP.ReplyDataType.text,
                                        MessageDetail = "感谢您的关注~！!"
                                    };
                                }
                            }
                            return GetReturnValue(codeReply, toUser, fromUser);
                        }
                    }
                    break;
            }
            return returnValue;
        }
        return string.Empty;
    }

    /// <summary>
    /// 处理文本类型
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ProcessText(Dictionary<string, string> data)
    {
        if ("text".Equals(data.GetDictionaryValue("MsgType")))
        {

            string content = data.GetDictionaryValue("Content");
            string fromUser = data.GetDictionaryValue("FromUserName"), toUser = data.GetDictionaryValue("ToUserName"), MsgId = data.GetDictionaryValue("MsgId");

            WxCredence credence = WxConfigRepository.GetConfig().CredenceSet.Find(m => m.OriginalId == toUser) ?? new WxCredence();

            WxReply reply = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Keyword, content).FirstOrDefault();
            if (reply == null)
            {
                reply = WxReplyRepository.Find(credence.AppId, Senparc.Weixin.MP.ReplyType.Default).FirstOrDefault();
                if (reply == null)
                {
                    reply = new WxReply()
                    {
                        ReplyDataType = Senparc.Weixin.MP.ReplyDataType.text,
                        MessageDetail = "感谢您的关注~！!"
                    };
                }
            }
            return GetReturnValue(reply, toUser, fromUser);
        }
        return string.Empty;
    }

    #endregion
}