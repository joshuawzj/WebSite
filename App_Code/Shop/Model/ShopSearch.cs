/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopSearch.cs
 * 文件描述：Whir_Shop_Search实体对象
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
    [TableName("Whir_Shop_Search")]
    [PrimaryKey("SearchID", sequenceName = "seq_ezEIP")]
    public class ShopSearch  : DomainBase
    {
        
		/// <summary>
		///主键ID
		/// <summary>
		public int SearchID { get; set;}

		/// <summary>
		///搜选项名称
		/// <summary>
		public string SearchName { get; set;}
    }
}
