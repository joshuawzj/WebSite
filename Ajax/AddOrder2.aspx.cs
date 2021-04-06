using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class Ajax_AddOrder2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string output = "";
        //获取订阅的产品ID
        int itemid = WebUtil.Instance.GetQueryInt("id", 0);
        if (WebUser.IsLogin())
        {
            DataTable dataTable = DbHelper.CurrentDb.Query("select * from whir_U_Content where whir_U_Content_pid = @0", itemid).Tables[0];
            if (dataTable.Rows.Count > 0)
            {
				LogHelper.Log("开始下单 : ");

                //请求地址
                string url = "https://api.pay.yungouos.com/api/pay/merge/nativePay"; 
                //支付密钥 登录yungouos.com-》聚合支付-》商户管理 支付密钥 获取
                string key = "6802F0E8E0BE412BB37D8A0AD35CCAD6"; 
                //会员ID
                int memberId = new FrontBasePage().GetUserId(); 
                //订单号
                string out_trade_no = System.DateTime.Now.ToString("yyMMddHHmmss") + Rand.Instance.Number(4, true);
                //支付金额
                decimal total_fee = dataTable.Rows[0]["price2"].ToDecimal();
                //聚合支付商户号
                string mch_id = "100153075261";
                //商品简单描述
                string body = "会员";//dataTable.Rows[0]["title"].ToStr().Trim();
                //返回类型（1、返回原生的支付连接需要自行生成二维码；2、直接返回付款二维码地址，页面上展示即可。不填默认1）
                string type = "2";
                //异步回调地址，用户支付成功后系统将会把支付结果发送到该地址，不填则无回调
                string notify_url = Request.Url.Scheme + "://" + Request.Url.Host + "/ajax/notify_url.aspx"; 
                //签名
                Dictionary<string, string> dics = new Dictionary<string, string>();
                dics.Add("out_trade_no", out_trade_no);
                dics.Add("total_fee", total_fee.ToStr());
                dics.Add("mch_id", mch_id);
                dics.Add("body", body);
                string stringSignTemp = getParamSrc(dics);
                stringSignTemp = stringSignTemp + "&key=" + key;
                string sign = Upper32(stringSignTemp);

				LogHelper.Log("下单签名 : " + sign);
				
				//写入订单表
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("orderNo", out_trade_no);
                dict.Add("price", total_fee);
				dict.Add("productId", dataTable.Rows[0]["whir_U_Content_pid"].ToInt32());
                dict.Add("memberid", memberId);
                dict.Add("Content", body);
				dict.Add("payStatus", 0);
                dict.Add("CreateDate", System.DateTime.Now);
                dict.Add("CreateUser", memberId);
                dict.Add("Sort", DateTime.Now.ToString("yyMMddHHmmssff"));
                dict.Add("State", 0);
                dict.Add("IsDel", 0);

                string SQL = ServiceFactory.DynamicFormService.GetInsertSql(15, 0, dict);
                object[] parms = dict.Select(p => p.Value).ToArray();

                int primaryValue = DbHelper.CurrentDb.Insert("Whir_U_Order_PId", SQL, parms).ToInt();
                if (primaryValue > 0)
                {
					//订单表插入成功调取下单接口
                    string param = "out_trade_no=" + out_trade_no + "&total_fee=" + total_fee + "&mch_id=" + mch_id + "&body=" + body + "&type=" + type + "&notify_url=" + notify_url + "&sign=" + sign;
                    string result = Post(param, url, "application/x-www-form-urlencoded");
					
					 output = SendPayPal(out_trade_no, body, total_fee);
                   
					
					
                }
                else
                {
                    output = "{\"info\":\"订单提交失败！\",\"status\":\"n\"}";
                }
            }
            else
            {
                output = "{\"info\":\"产品有误！\",\"status\":\"n\"}";
            }
        }
        else
        {
            output = "{\"info\":\"请先登录！\",\"status\":\"n\"}";
        }
        Response.Write(output);
    }
	
	
	/// <summary>
    /// PayPal支付
    /// </summary>
    /// <param name="orderno">订单号</param>
    /// <param name="subject">主题</param>
    /// <param name="price">支付金额</param>
    /// <returns></returns>
    private string SendPayPal(string orderno, string subject, decimal price)
    {
        PayPalModel model = new PayPalModel();
        model.OrderNo = orderno;
        model.Amount = price;
        model.Title = subject;
        string sHtmlText = PayPalCore.Submit(model, false);
        return sHtmlText;

    }
	

    #region post 请求
    public static string Post(string param, string url, string contentType)
    {
        System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

        string result = "";//返回结果

        HttpWebRequest request = null;
        HttpWebResponse response = null;
        Stream reqStream = null;

        try
        {
            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;

            /***************************************************************
            * 下面设置HttpWebRequest的相关属性 
            * ************************************************************/
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Timeout = 100000;

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;

            //设置POST的数据类型和长度
            request.ContentType = contentType;

            byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
            //request.ContentLength = data.Length;

            //往服务器写入数据
            reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            //获取服务端返回
            response = (HttpWebResponse)request.GetResponse();

            //获取服务端返回数据
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = sr.ReadToEnd().Trim();
            sr.Close();

        }
        catch (Exception e)
        {
            HttpContext.Current.Response.Write(e.ToString());
        }
        finally
        {
            //关闭连接和流
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
        }
        return result;
    }
    #endregion

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

}