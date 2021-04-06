/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopCourier.cs
 * 文件描述：Whir_Shop_Courier实体对象
 * 
 * 创建标识: 2013-01-25 10:43:42
 * 
 * 修改标识：
 */
using System;

using Whir.Repository;
using Whir.Domain;

namespace Shop.Domain
{
    [TableName("Whir_Shop_Courier")]
    [PrimaryKey("CourierID", sequenceName = "seq_ezEIP")]
    public class ShopCourier  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int CourierID { get; set;}

		/// <summary>
		///物流名称
		/// </summary>
		public string CourierName { get; set;}

		/// <summary>
		///快递100接口
		/// </summary>
		public string Interface { get; set;}

        /// <summary>
        ///快递公司代码
        /// </summary>
        public string Com { get; set; }
    }
}
