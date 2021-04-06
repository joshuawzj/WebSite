/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopAttrValue.cs
 * 文件描述：Whir_Shop_AttrValue实体对象
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
    [TableName("Whir_Shop_AttrValue")]
    [PrimaryKey("AttrValueID", sequenceName = "seq_ezEIP")]
    public class ShopAttrValue  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int AttrValueID { get; set;}

        /// <summary>
        /// 外键，规格组ID
        /// </summary>
        public int AttrID { get; set; }

		/// <summary>
		///属性值名称
		/// <summary>
		public string AttrValueName { get; set;}

		/// <summary>
		///图标
		/// <summary>
		public string ShowImage { get; set;}

        /// <summary>
        /// 记录数
        /// </summary>
        [ResultColumn]
        public int TotalCount { get; set; }
    }
}
