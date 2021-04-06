/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopOrderInfo.cs
 * 文件描述：Whir_Shop_OrderInfo实体对象
 * 
 * 创建标识: 2013-01-25 10:43:43
 * 
 * 修改标识：
 */
using System;

using Whir.Repository;
using Whir.Domain;

namespace Shop.Domain
{
    [TableName("Whir_Shop_OrderInfo")]
    [PrimaryKey("OrderID", sequenceName = "seq_ezEIP")]
    public class ShopOrderInfo : DomainBase
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///购买的会员ID
        /// </summary>
        public int MemberID { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public int CourierID { get; set; }

        /// <summary>
        /// 是否已支付
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        ///订单流程状态：0.交易完成，1.未完成
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///是否已取消
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        ///发货(快递)单号
        /// </summary>
        public string ShipOrderNumber { get; set; }

        /// <summary>
        ///订单商品总金额
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        ///订单优惠金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        ///应付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 支付方式ID
        /// </summary>
        public int PaymentID { get; set; }

        /// <summary>
        ///交易完成时间
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string TakeName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string TakeMobile { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string TakeTel { get; set; }

        /// <summary>
        /// 收货人地区，结合TakeAddress一起组成具体的收货地址
        /// </summary>
        public int TakeRegion { get; set; }

        /// <summary>
        /// 收货人地址，结合TakeRegion一起组成具体的收货地址
        /// </summary>
        public string TakeAddress { get; set; }

        /// <summary>
        /// 收货人邮编
        /// </summary>
        public string TakePostcode { get; set; }

        /// <summary>
        /// 收货人Email
        /// </summary>
        public string TakeEmail { get; set; }

        /// <summary>
        /// 发票信息
        /// </summary>
        public string TakeInvoice { get; set; }

        /// <summary>
        /// 购买商品会员(外联属性，订单表中无此字段)
        /// </summary>
        [ResultColumn]
        public string LoginName { get; set; }

        /// <summary>
        /// 支付名称(外联属性，订单表中无此字段)
        /// </summary>
        [ResultColumn]
        public string PaymentName { get; set; }

        /// <summary>
        /// 订单商品(外联属性，订单表中无此字段)
        /// </summary>
        [ResultColumn]
        public string ShopOrderProducts { get; set; }
    }
}
