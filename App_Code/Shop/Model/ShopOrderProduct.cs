/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Whir_Shop_OrderProduct.cs
 * 文件描述：Whir_Shop_OrderProduct实体对象
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
    [TableName("Whir_Shop_OrderProduct")]
    [PrimaryKey("OrderProID", sequenceName = "seq_ezEIP")]
    public class ShopOrderProduct : DomainBase
    {
        /// <summary>
        ///主键ID
        /// </summary>
        public int OrderProID { get; set; }

        /// <summary>
        ///订单主表主键ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        ///商品主表主键ID
        ///</summary>
        public int ProID { get; set; }

        /// <summary>
        ///属性商品表的主键ID
        /// </summary>
        public int AttrProID { get; set; }

        /// <summary>
        ///购买时商品编号
        /// </summary>
        public string ProNO { get; set; }

        /// <summary>
        ///购买时商品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        ///购买时售价
        /// </summary>
        public decimal SaleAmount { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
    }
}
