<%@ WebHandler Language="C#" Class="AddCart" %>
/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：AddCart.ashx
 * 文件描述：异步添加购物车
 * 
 * 创建标识: lurong 2013-2-18
 * 
 * 修改标识：
 */
using System;
using System.Web;
using Whir.Framework;
using Shop.Service;

public class AddCart : IHttpHandler {
    /// <summary>
    /// 返回1表成功
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        int proID = RequestUtil.Instance.GetQueryInt("proid", 0);
        int attrProID = RequestUtil.Instance.GetQueryInt("attrproid", 0);
        int qutity = RequestUtil.Instance.GetQueryInt("qutity", 0);
        int cartId=RequestUtil.Instance.GetQueryInt("cartid", 0);

        bool isCumulative=RequestUtil.Instance.GetQueryString("isCumulative").ToBoolean(true);
        
        try
        {

            if(cartId==0)
            {
                ShopCartService.Instance.AddCart(proID, attrProID, qutity, isCumulative);
                context.Response.Write("1");
            }else
            {
                ShopCartService.Instance.AddCart(cartId, qutity);
                context.Response.Write("1");
            }
        }
        catch(Exception ex) {
            context.Response.Write("err:"+ex.ToStr());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}