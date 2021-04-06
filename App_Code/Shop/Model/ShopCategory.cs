/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopCategory.cs
 * 文件描述：Whir_Shop_Category实体对象
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
    [TableName("Whir_Shop_Category")]
    [PrimaryKey("CategoryID", sequenceName = "seq_ezEIP")]
    public class ShopCategory  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int CategoryID { get; set;}

		/// <summary>
		///上级类目ID
		/// <summary>
		public int ParentID { get; set;}

		/// <summary>
		///上级路径
		/// <summary>
		public string ParentPath { get; set;}

		/// <summary>
		///类目名称
		/// <summary>
		public string CategoryName { get; set;}

        /// <summary>
		///类目图片
		/// <summary>
		public string CategoryImages { get; set; }

        /// <summary>
        ///SEO标题
        /// <summary>
        public string MetaTitle { get; set;}

		/// <summary>
		///SEO关键字
		/// <summary>
		public string MetaKeyword { get; set;}

		/// <summary>
		///SEO描述
		/// <summary>
		public string MetaDescription { get; set;}
    }
}
