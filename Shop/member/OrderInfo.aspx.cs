using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Shop.Common;
using Shop.Domain;
using Shop.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using System.Net;
using System.IO;
public partial class Shop_member_OrderInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUser.IsLogin("shop/member/login.aspx");
        if (!IsPostBack)
        {
            BindeOrderInfo();
        }
    }

    /// <summary>
    /// 绑定订单基本信息
    /// </summary>
    protected void BindeOrderInfo()
    {
        int orderID = RequestUtil.Instance.GetQueryInt("id", 0);
        ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(orderID);
        
        if (order == null)
            return;

        string payName = "";
        try
        {
            DataTable dt = PayInterface.Common.Tools.GetPayTypeList();
            DataRow dr = dt.Select("id='" + order.PaymentID + "'")[0];
            payName = dr["name"].ToStr();
        }
        catch { }


        OrderNo.Text = order.OrderNo;
        PayAmount.Text = order.PayAmount.ToString("C2");
        CreateDate.Text = order.CreateDate.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

        if (order.Status == 0)
            OrderStatus.Text = "交易完成";
        else
            OrderStatus.Text = "未完成";

        if (order.IsPaid)
            PayStatus.Text = "已支付";
        else if (order.IsCancel)
            PayStatus.Text = "未支付";
        else
            PayStatus.Text = string.Format("未支付 <a href=\"{0}Payment/Pay.aspx?id={1}\" class=\"a_fontbtn\" target=\"_blank\" >去支付</a>", AppName, orderID);



        PayTypeName.Text = payName;
        InfoPayAmount.Text = order.PayAmount.ToString("C2");
        InfoProductAmount.Text = order.ProductAmount.ToString("C2");

        TakeName.Text = order.TakeName;

        TakeRegion.Text = ServiceFactory.AreaService.GetParentsName(order.TakeRegion);

        TakeAddress.Text = order.TakeAddress;
        TakeMobile.Text = order.TakeMobile;
        TakeTel.Text = order.TakeTel;
        TakeEmail.Text = order.TakeEmail;
        TakePostcode.Text = order.TakePostcode;

        PayName.Text = payName;


        DiscountAmount.Text = order.DiscountAmount.ToString("f2");

        ShopCourier shopCourier = ShopCourierService.Instance.SingleOrDefault<ShopCourier>(order.CourierID);

        Courier.Text = shopCourier != null ? shopCourier.CourierName : "";
        ProductAmount.Text = order.PayAmount.ToString("C2");
        BindCourierLog(order.CourierID,order.ShipOrderNumber);
        BindOrderProducts(orderID);
        
    }
    /// <summary>
    /// 绑定订单商品
    /// </summary>
    /// <param name="orderID"></param>
    protected void BindOrderProducts(int orderID)
    {
        DataTable dt = ShopOrderProductService.Instance.GetShopOrderProductsByOrderID(orderID);

        rptOrderPro.DataSource = dt;
        rptOrderPro.DataBind();

        rptSettleAccounts.DataSource = dt;
        rptSettleAccounts.DataBind();
    
    }

    /// <summary>
    /// 物流跟踪
    /// </summary>
    /// <param name="courierID"></param>
    /// <param name="shipOrderNumber"></param>
    protected void BindCourierLog(int courierID, string shipOrderNumber)
    {
        //测试用 http://api.kuaidi100.com/api?id=3f69c1a3b2a0c305&com=shunfeng&nu=591447444242&show=2&muti=&order=asc

       

       ShopCourier shopCourier=ShopCourierService.Instance.SingleOrDefault<ShopCourier>(courierID);
       if (shopCourier == null)
       {
           litCourier.Text = "<div id=errordiv style=width:500px;border:#fe8d1d 1px solid;padding:20px;background:#FFFAE2;><p style=line-height:28px;margin:0px;padding:0px;color:#F21818;>未设置配送物流</div>";
           return;
       }
       string com = shopCourier.Com;//快递公司代码

       com = "shunfeng";
       shipOrderNumber = "591447444242";
       /*
        快递接口查询参数说明参考 http://www.kuaidi100.com/openapi/api_2_02.shtml?typeid=2
         
        show:返回类型： 
0：返回json字符串， 
1：返回xml对象， 
2：返回html对象， 
3：返回text文本。 
如果不填，默认返回json字符串。
        */
       string show = "2";
       string url = string.Format("http://api.kuaidi100.com/api?id=3f69c1a3b2a0c305&com={0}&nu={1}&show={2}&muti=&order=asc", com, shipOrderNumber, show);

        //Create a request for the URL.         
        WebRequest request = WebRequest.Create(url);

        // Get the response.
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        if (response.StatusCode != HttpStatusCode.OK) return;

        // Get the stream containing content returned by the server.
        Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.

        try
        {
            string content = reader.ReadToEnd();
            litCourier.Text = content;
            //DataSet ds = new DataSet();
            //ds.ReadXml(reader);
            //rptCourier.DataSource = ds;
            //rptCourier.DataBind();
        }
        catch { 
        
        }
   
    }
}