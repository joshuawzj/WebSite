/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopOrderInfoService.cs
 * 文件描述：订单业务操作类
 * 
 * 创建标识: liuyong 2013-02-04
 * 
 * 修改标识：
 */

using System.Data;
using Shop.Domain;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using System;
namespace Shop.Service
{
    public class ShopOrderInfoService : DbBase<ShopOrderInfo>
    {
        #region 根据单一模式构建类的对象
        private ShopOrderInfoService() { }  //私有构造函数

        private static ShopOrderInfoService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopOrderInfoService Instance
        {
            get
            {
                lock (typeof(ShopOrderInfoService))
                {
                    if (_object == null)
                    {
                        _object = new ShopOrderInfoService();
                    }

                    return _object;
                }
            }
        }

        #endregion

        /// <summary>
        /// 新增订单(返回状态0：表示成功;1:表示未登录或会员不存在;2：下单失败;3:购物车中没有商品或异常)
        /// </summary>
        /// <param name="paymentID">支付方式</param>
        /// <param name="address">地址</param>
        /// <param name="email">电子邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="tel">电话</param>
        /// <param name="name">收货人姓名</param>
        /// <param name="postcode">邮编</param>
        /// <param name="region">地区</param>
        /// <returns></returns>
        public int AddOrder(int paymentID, string address, string email, string mobile, string tel
            , string name, string postcode, int region,out int orderID)
        {
            orderID = 0;
            #region 所需会员数据
            int memberId = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt(0);
            string loginName = WebUser.GetUserValue("LoginName");

            //验证MemberId 是否有效
            if (memberId == 0 || !WebUser.IsLogin())
                return 1;
            else if (!WebUser.IsExist("Whir_Mem_Member_PID", memberId.ToStr()))
                return 1;
            #endregion

            #region 获取购物车数据
            DataSet ds = ShopCartService.Instance.GetShopCartDetailed();
            
            if (ds.Tables.Count < 3 || ds.Tables[0].Rows.Count == 0)
            {
                return 3;
            }
            DataTable cartProList = ds.Tables[0];//购物车商品
            int count = ds.Tables[1].Rows[0][0].ToInt(0);//购物车商品总数
            decimal total = ds.Tables[2].Rows[0][0].ToDecimal(0);//购物车总金额
            #endregion


            #region 添加订单基本信息,添加成功后再更新订单编号
            ShopOrderInfo orderInfo = ModelFactory<ShopOrderInfo>.Insten();
            orderInfo.MemberID = memberId;
            orderInfo.IsPaid = false;
            orderInfo.IsCancel = false;
            orderInfo.PayAmount = total;
            orderInfo.PaymentID = paymentID;
            orderInfo.ProductAmount = total;
            orderInfo.TakeAddress = address;
            orderInfo.TakeEmail = email;
            orderInfo.TakeMobile = mobile;
            orderInfo.TakeName = name;
            orderInfo.TakePostcode = postcode;
            orderInfo.TakeRegion = region;
            orderInfo.TakeTel = tel;
            orderInfo.CreateUser = loginName;
            orderInfo.Status = 1;
            this.Save(orderInfo);
            orderID=orderInfo.OrderID;
            if (orderInfo.OrderID == 0)
                return 2;
            orderInfo.OrderNo = CreateOrderNo(orderInfo.OrderID);//订单号
            this.Update(orderInfo);
            #endregion

            #region 添加订单商品明细
            ShopOrderProduct orderPro;

            foreach (DataRow dr in cartProList.Rows)
            {
                orderPro = Whir.Domain.ModelFactory<ShopOrderProduct>.Insten();
                orderPro.AttrProID = dr["AttrProID"].ToInt(0);
                orderPro.AttrProID = dr["AttrProID"].ToInt(0);
                orderPro.Count = dr["Qutity"].ToInt(0);
                orderPro.OrderID = orderInfo.OrderID;
                orderPro.ProID = dr["ProID"].ToInt(0);
                orderPro.ProName = dr["ProName"].ToStr() + " " + dr["AttrValueNames"].ToStr();
                orderPro.ProNO = dr["ProNO"].ToStr();
                orderPro.SaleAmount = dr["CostAmount"].ToDecimal(0);
                ShopOrderProductService.Instance.Save(orderPro);
            }
            ShopCartService.Instance.ClearCart();
            #endregion
            return 0;
        }

        /// <summary>
        /// 创建订单编号
        /// </summary>
        /// <returns>返回订单编号</returns>
        public string CreateOrderNo(int orderid)
        {
            string orderNo = (orderid+10000).ToStr();//订单号
            int length = orderNo.Length;

            do
            {
                orderNo = orderNo + Whir.Framework.Rand.Instance.Number(4, true);
            } while (IsExists(orderNo));
            return orderNo;
        }

        /// <summary>
        /// 订单是否存在(true:存在)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsExists(string orderNo)
        {
            return this.SingleOrDefault<ShopOrderInfo>("SELECT * FROM Whir_Shop_OrderInfo WHERE OrderNo=@0", orderNo) != null;
        }

        /// <summary>
        /// 完成订单(-1：订单异常或不存在；0：成功；1：订单未支付不能完成（不包括货到付款）；2:订单未选中支付方式或支付方式不存在，不能完成)；3:订单已
        /// </summary>
        /// <param name="orderID"></param>
        public int CompleteTheOrder(int orderID)
        {

            ShopOrderInfo shopOrderInfo = GetShopOrderInfo(orderID);
            if (shopOrderInfo == null)
                return -1;
            string paytype = null;
            
                try
                {
                    DataTable dt = PayInterface.Common.Tools.GetPayTypeList();
                    DataRow dr = dt.Select("id='" + shopOrderInfo.PaymentID.ToStr() + "'")[0];
                    paytype = dr["paytype"].ToStr().ToLower();
                }
                catch
                {
                }

                if (shopOrderInfo.PaymentID == null || string.IsNullOrEmpty(paytype))
                {
                    return 2;
                }

                if (paytype.ToLower() == "cod")
                {
                    shopOrderInfo.IsPaid = true;
                     
                }else if(shopOrderInfo.IsPaid == false)
                {
                    return 1;
                }

                shopOrderInfo.FinishDate = DateTime.Now;
                shopOrderInfo.Status = 0;
                return this.Update(shopOrderInfo) > 0 ? 0 : -1;
        }

        /// <summary>
        /// 取消订单(-1：订单异常或不存在；0：成功；1：订单已支付不能取消)
        /// </summary>
        /// <param name="orderID"></param>
        public int CancelOrder(int orderID)
        {
            
            ShopOrderInfo shopOrderInfo = GetShopOrderInfo(orderID);
            if (shopOrderInfo == null)
                return -1;

            shopOrderInfo.IsCancel = true;
           return this.Update(shopOrderInfo)>0?0:-1;
        }

        /// <summary>
        /// 根据Id获取订单实体
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ShopOrderInfo GetShopOrderInfo(int orderID)
        {
            return base.SingleOrDefault<ShopOrderInfo>(orderID);
        }

        /// <summary>
        /// 根据orderNo获取订单实体
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ShopOrderInfo GetShopOrderInfo(string orderNo)
        {
            return this.SingleOrDefault<ShopOrderInfo>("SELECT * FROM Whir_Shop_OrderInfo WHERE OrderNo=@0", orderNo);
        }
    }
}
