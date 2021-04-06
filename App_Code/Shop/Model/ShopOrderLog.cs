/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopOrderLog.cs
 * 文件描述：Whir_Shop_OrderLog实体对象
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
    [TableName("Whir_Shop_OrderLog")]
    [PrimaryKey("OrderLogID", sequenceName = "seq_ezEIP")]
    public class ShopOrderLog  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int OrderLogID { get; set;}

		/// <summary>
		///订单主表主键ID
		/// <summary>
		public int OrderID { get; set;}

		/// <summary>
		///操作说明
		/// <summary>
		public string Descn { get; set;}

		/// <summary>
		///备注
		/// <summary>
		public string Remark { get; set;}
    }
}
