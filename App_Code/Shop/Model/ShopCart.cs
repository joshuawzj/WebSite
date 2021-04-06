/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopCart.cs
 * 文件描述：Whir_Shop_Cart实体对象
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
    [TableName("Whir_Shop_Cart")]
    [PrimaryKey("CartID", sequenceName = "seq_ezEIP")]
    public class ShopCart  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int CartID { get; set;}

		/// <summary>
		///购买的会员ID：对应会员表的主键；未登录购买：记录上下文标识
		/// <summary>
		public string UniqueID { get; set;}

		/// <summary>
		///商品主表主键ID
		/// <summary>
		public int ProID { get; set;}

		/// <summary>
		///属性商品表主键ID
		/// <summary>
		public int AttrProID { get; set;}

		/// <summary>
		///购买数量
		/// <summary>
		public int Qutity { get; set;}
    }
}
