/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopAttrPro.cs
 * 文件描述：Whir_Shop_AttrPro实体对象
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
    [TableName("Whir_Shop_AttrPro")]
    [PrimaryKey("AttrProID", sequenceName = "seq_ezEIP")]
    public class ShopAttrPro  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int AttrProID { get; set;}

		/// <summary>
		///商品主表主键ID
		/// <summary>
		public int ProID { get; set;}

		/// <summary>
		///属性值主键ID集合,对应多个商品属性表的主键ID，多个以英文逗号分隔开
		/// <summary>
		public string AttrValueIDs { get; set;}

        /// <summary>
        ///属性值名称集合，多个以英文逗号分隔开
        /// <summary>
        public string AttrValueNames { get; set; }

		/// <summary>
		///价格
		/// <summary>
		public decimal CostAmount { get; set;}

		/// <summary>
		///商品主图片
		/// <summary>
		public string ProImg { get; set;}

		/// <summary>
		///商品图片,以星号分割多张图片
		/// <summary>
		public string Images { get; set;}

		/// <summary>
		///是否延用主商品图片
		/// <summary>
		public bool IsUseMainImage { get; set;}
    }
}
