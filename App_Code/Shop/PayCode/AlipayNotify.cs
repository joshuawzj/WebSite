using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.AlipayWap
{
    /// <summary>
    ///     类名：AlipayNotify
    ///     功能：支付宝通知处理类
    ///     详细：处理支付宝各接口通知返回
    ///     版本：3.3
    ///     修改日期：2011-07-05
    ///     '说明：
    ///     以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    ///     该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    ///     //////////////////////注意/////////////////////////////
    ///     调试通知返回时，可查看或改写log日志的写入TXT里的数据，来检查通知返回是否正常
    /// </summary>
    public class AlipayNotify
    {
        #region 字段
        private readonly string _inputCharset = ""; //编码格式
        private readonly string _key = ""; //商户的私钥
        private readonly string _partner = ""; //合作身份者ID
        private readonly string _signType = ""; //签名方式

        //支付宝消息验证地址
        private const string HttpsVeryfyUrl = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

        #endregion

        /// <summary>
        ///     构造函数
        ///     从配置文件中初始化变量
        /// </summary>
        public AlipayNotify()
        {
            //初始化基础配置信息
            PayInterface.Model.AlipayInstant model = new PayInterface.Model.AlipayInstant().Insten();
            _partner = model.V_Mid.Trim();
            _key = model.PayOnlineKey.Trim();
            _inputCharset = "utf-8";
            _signType = "MD5";
        }

        /// <summary>
        ///     从文件读取公钥转公钥字符串
        /// </summary>
        /// <param name="path">公钥文件路径</param>
        public static string GetPublicKeyStr(string path)
        {
            var sr = new StreamReader(path);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
            pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
            pubkey = pubkey.Replace("\r", "");
            pubkey = pubkey.Replace("\n", "");
            return pubkey;
        }

        /// <summary>
        ///     验证消息是否是支付宝发出的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notifyId">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string notifyId, string sign)
        {
            //获取返回时的签名验证结果
            bool isSign = GetSignVeryfy(inputPara, sign);
            //获取是否是支付宝服务器发来的请求的验证结果
            string responseTxt = "false";
            if (!string.IsNullOrEmpty(notifyId))
            {
                responseTxt = GetResponseTxt(notifyId);
            }

            //写日志记录（若要调试，请取消下面两行注释）
            //string sWord = "responseTxt=" + responseTxt + "\n isSign=" + isSign.ToString() + "\n 返回回来的参数：" + GetPreSignStr(inputPara) + "\n ";
            //AlipayCore.LogResult(sWord);

            //判断responsetTxt是否为true，isSign是否为true
            //responsetTxt的结果不是true，与服务器设置问题、合作身份者ID、notify_id一分钟失效有关
            //isSign不是true，与安全校验码、请求时的参数格式（如：带自定义参数等）、编码格式有关
            if (responseTxt == "true" && isSign) //验证成功
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     获取返回时的签名验证结果
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="sign">对比的签名结果</param>
        /// <returns>签名验证结果</returns>
        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
        {
            //过滤空值、sign与sign_type参数
            Dictionary<string, string> para = AlipayCore.FilterPara(inputPara);

            //获取待签名字符串
            string preSignStr = AlipayCore.CreateLinkString(para);

            //获得签名验证结果
            bool isSgin = false;
            if (!string.IsNullOrEmpty(sign))
            {
                switch (_signType)
                {
                    case "MD5":
                        isSgin = AlipayMD5.Verify(preSignStr, sign, _key, _inputCharset);
                        break;
                }
            }

            return isSgin;
        }

        /// <summary>
        ///     获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notifyId">通知验证ID</param>
        /// <returns>验证结果</returns>
        private string GetResponseTxt(string notifyId)
        {
            string veryfyUrl = HttpsVeryfyUrl + "partner=" + _partner + "&notify_id=" + notifyId;

            //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
            string responseTxt = Get_Http(veryfyUrl, 120000);

            return responseTxt;
        }

        /// <summary>
        ///     获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string result = "";
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(strUrl);
                request.Timeout = timeout;
                var response = (HttpWebResponse) request.GetResponse();
                Stream myStream = response.GetResponseStream();
                if (myStream != null)
                {
                    var sr = new StreamReader(myStream, Encoding.UTF8);
                    var strBuilder = new StringBuilder();
                    while (-1 != sr.Peek())
                    {
                        strBuilder.Append(sr.ReadLine());
                    }

                    result = strBuilder.ToString();
                }
            }
            catch (Exception exp)
            {
                result = "错误：" + exp.Message;
            }
           // LogManager.Log("验证支付宝：url=" + strUrl);
            return result;
        }
    }
}