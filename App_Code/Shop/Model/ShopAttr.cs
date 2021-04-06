/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopAttr.cs
 * 文件描述：Whir_Shop_Attr实体对象
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
    [TableName("Whir_Shop_Attr")]
    [PrimaryKey("AttrID", sequenceName = "seq_ezEIP")]
    public class ShopAttr  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int AttrID { get; set;}

		/// <summary>
		///属性规格名称
		/// <summary>
		public string SearchName { get; set;}

		/// <summary>
		///是否开启图标显示
		/// <summary>
		public bool IsShowImage { get; set;}
    }
}
