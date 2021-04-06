using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Repository;
using System.Collections.Specialized;
using Whir.Service;

public partial class Paypal_Return_Notify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SortedDictionary<string, string> sPara = GetRequestPost();
        PayPalLog.Info(this.GetType().ToString(), "返回参数：" + Request.Form.ToString());
        if (sPara.Count > 0)
        {
            string out_trade_no = Request.Form["invoice"].ToStr();  //原始订单号
            string trade_status = Request.Form["payment_status"].ToStr();  //支付状态
            string trade_no = Request.Form["txn_id"].ToStr();    //第三方支付交易号
            string seller = Request.Form["business"].ToStr();
            // && seller == PayPalConfig.sellerEmail

            if (trade_status == "Completed")
            {
                try
                {
                    PayPalLog.Info(this.GetType().ToString(), "进来了-------------------------------------：");
                    DataTable dataTable = DbHelper.CurrentDb.Query("select * from whir_U_Order where orderNo = @0 and payStatus = 0 and isdel = 0", out_trade_no).Tables[0];
                    //支付成功处理逻辑
                    LogHelper.Log("支付成功回调签名验证通过 。");
                    //修改订单状态
                    DbHelper.CurrentDb.ExecuteScalar<int>("update whir_U_Order set TradeNo=@0,payType=@1,payTime=@2,payStatus=@3 where whir_U_Order_pid = @4", out_trade_no, "paypal", DateTime.Now, 1, dataTable.Rows[0]["whir_U_Order_pid"].ToInt32());
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

                
                }
                catch (Exception ex)
                {
                    PayPalLog.Info(this.GetType().ToString(), "报错了---：" + ex.Message);
                }

            }

        }
    }

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