/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopMemberInfo.cs
 * 文件描述：Whir_Shop_MemberInfo实体对象
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
    [TableName("Whir_Shop_MemberInfo")]
    [PrimaryKey("MemberID", sequenceName = "seq_ezEIP")]
    public class ShopMemberInfo  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int MemberID { get; set;}

		/// <summary>
		///登录(用户)名
		/// <summary>
		public string LoginName { get; set;}

		/// <summary>
		///密码
		/// <summary>
		public string Password { get; set;}

		/// <summary>
		///电子邮箱
		/// <summary>
		public string Email { get; set;}

		/// <summary>
		///性别
		/// <summary>
		public string Sex { get; set;}

		/// <summary>
		///出生日期
		/// <summary>
		public DateTime Birthday { get; set;}

		/// <summary>
		///昵称
		/// <summary>
		public string NickName { get; set;}

		/// <summary>
		///收货人
		/// <summary>
		public string Consignee { get; set;}

		/// <summary>
		///收货地址
		/// <summary>
		public string Address { get; set;}

		/// <summary>
		///电话
		/// <summary>
		public string Tel { get; set;}

		/// <summary>
		///手机
		/// <summary>
		public string Mobile { get; set;}

		/// <summary>
		///邮政编码
		/// <summary>
		public string Postcode { get; set;}

		/// <summary>
		///收货电子邮箱
		/// <summary>
		public string ConEmail { get; set;}
    }
}
