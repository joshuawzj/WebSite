using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class Ajax_notify_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SortedDictionary<string, string> sPara = GetRequestPost();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            //支付成功回调，查询订单是否存在
            string code = Request.Form["code"];                 //支付结果 【1：成功；0：失败】
            string orderNo = Request.Form["orderNo"];           //系统订单号（YunGouOS系统内单号）
            string outTradeNo = Request.Form["outTradeNo"];     //商户订单号
            string payNo = Request.Form["payNo"];               //支付单号（第三方支付单号）
            string money = Request.Form["money"];               //支付金额 单位：元
            string time = Request.Form["time"];                 //支付成功时间
            string sign = Request.Form["sign"];                 //签名
            string payChannel = Request.Form["payChannel"];     //支付渠道（枚举值 wxpay、alipay）
            string mchId = Request.Form["mchId"];               //商户号

        
            //支付密钥 根据支付渠道 获取 key
            string key = "";
            if (payChannel == "wxpay")
            {
                key = "7845602CB5D9432795FDCE4AA00DC542";
            }
            else
            {
                key = "3F8750CF0D484757ACE403391D1DBB4E";
            }

            //生成签名
            Dictionary<string, string> dics = new Dictionary<string, string>();
            dics.Add("code", code);
            dics.Add("orderNo", orderNo);
            dics.Add("outTradeNo", outTradeNo);
            dics.Add("payNo", payNo);
            dics.Add("money", money);
            dics.Add("mchId", mchId);
            string stringSignTemp = getParamSrc(dics);
            stringSignTemp = stringSignTemp + "&key=" + key;
            string reSign = Upper32(stringSignTemp);

            LogHelper.Log("支付成功回调的新签名为 : " + reSign);


            //根据商户订单号，查询未支付未删除的订单是否存在
            DataTable dataTable = DbHelper.CurrentDb.Query("select * from whir_U_Order where orderNo = @0 and payStatus = 0 and isdel = 0", outTradeNo).Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                 
                //把回调参数重新生成签名对比回调返回是否一致
                if (sign == reSign)
                {
                    LogHelper.Log("支付成功回调签名验证通过 。");
                    //修改订单状态
                    DbHelper.CurrentDb.ExecuteScalar<int>("update whir_U_Order set TradeNo=@0,payType=@1,payTime=@2,payStatus=@3 where whir_U_Order_pid = @4", orderNo, payChannel, time, code, dataTable.Rows[0]["whir_U_Order_pid"].ToInt32());
                    //把会员时间加365天
					DateTime endTime = DbHelper.CurrentDb.ExecuteScalar<DateTime>("select endtime from Whir_Mem_Member where Whir_Mem_Member_PId=@0", dataTable.Rows[0]["memberId"].ToInt32());
                    //判断到期时间是否大于当前时间，   如果结束时间小于当前时间， 就从今天开始到1年后到期 ，否则在原来到期日期上加1年
                    if (endTime > System.DateTime.Now)
                    {
                        DbHelper.CurrentDb.ExecuteScalar<int>("update Whir_Mem_Member set endtime = CONVERT(varchar(100), DATEADD(YEAR,1,endtime), 23) where Whir_Mem_Member_PId=@0", dataTable.Rows[0]["memberId"].ToInt32());
                    }
                    else
                    {
                        DbHelper.CurrentDb.ExecuteScalar<int>("update Whir_Mem_Member set endtime = CONVERT(varchar(100), DATEADD(YEAR,1,getdate()), 23) where Whir_Mem_Member_PId=@0", dataTable.Rows[0]["memberId"].ToInt32());
                    }
					//支付成功给会员发送邮箱 
                    string email = DbHelper.CurrentDb.ExecuteScalar<string>("select Email from Whir_Mem_Member where Whir_Mem_Member_pid = @0", dataTable.Rows[0]["memberId"].ToInt32());
                    if (!string.IsNullOrEmpty(email))
                    {
						DataTable emInfo = DbHelper.CurrentDb.Query("select title,content from Whir_U_SinglePage where typeid = 18").Tables[0]; 
                        SendEmailHelper.SendEmail(email, emInfo.Rows[0]["title"].ToStr(), emInfo.Rows[0]["content"].ToStr());
                    }
					
                    Response.Write("SUCCESS");
                    Response.End();
                }
                else
                {
                    LogHelper.Log("支付成功回调签名验证失败 。 ");
                }
            }
        }
    }

    #region ASCII码从小到大排序
    public static String getParamSrc(Dictionary<string, string> paramsMap)
    {
        var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
        StringBuilder str = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in vDic)
        {
            string pkey = kv.Key;
            string pvalue = kv.Value;
            str.Append(pkey + "=" + pvalue + "&");
        }

        String result = str.ToString().Substring(0, str.ToString().Length - 1);
        return result;
    }
    #endregion

    #region MD5 加密
    /// <summary>
    /// 32位大写
    /// </summary>
    /// <returns></returns>
    public static string Upper32(string s)
    {
        s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
        return s.ToUpper();
    }

    #endregion

    #region 接受post参数
    /// <summary>
    /// 获取POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestPost()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = Request.Form;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
			LogHelper.Log("支付成功回调参数： " + requestItem[i] + " ------ " + Request.Form[requestItem[i]]);
        }
        return sArray;
    }
    #endregion
}