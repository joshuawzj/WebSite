/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopField.cs
 * 文件描述：Whir_Shop_Field实体对象
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
    [TableName("Whir_Shop_Field")]
    [PrimaryKey("FieldID", sequenceName = "seq_ezEIP")]
    public class ShopField  : DomainBase
    {
        

		/// <summary>
		///主键ID
        /// </summary>
		public int FieldID { get; set;}

		/// <summary>
		///字段名
        /// </summary>
		public string FieldName { get; set;}

		/// <summary>
		///字段别名
        /// </summary>
		public string FieldAlias { get; set;}

		/// <summary>
		/// 字段类型
		/// </summary>
		public string FieldType { get; set;}

		/// <summary>
		///是否为空
		/// </summary>
		public bool IsAllowNull { get; set;}

		/// <summary>
		///展示形式(1单行文本框，2单选按钮，3多选按钮，4多行文本框，5下拉框，6HTML编辑器，7文件上传)
        /// </summary>
		public int ShowType { get; set;}

		/// <summary>
		///验证的正则表达式
        /// </summary>
		public string ValidateExpression { get; set;}

		/// <summary>
		///验证类型
        /// </summary>
		public int ValidateType { get; set;}

		/// <summary>
		///输入时提示文字
        /// </summary>
		public string TipText { get; set;}

		/// <summary>
		///验证不通过时提示文字
		/// </summary>
		public string ValidateText { get; set;}

		/// <summary>
        ///绑定的类型,1:绑定文本(单选),2:绑定SQL语句,3:绑定多级类别,4:绑定文本(多选)
        /// </summary>
		public int BindType { get; set;}

		/// <summary>
		///BindType=1时, 绑定的文本
        /// </summary>
		public string BindText { get; set;}

		/// <summary>
		///BindType=2时, 绑定的SQL语句
        /// </summary>
		public string BindSql { get; set;}

		/// <summary>
		///BindType=3时, 绑定的表
        /// </summary>
		public string BindTable { get; set;}

		/// <summary>
		///BindType=3时, 指定的表的字段的值(如:栏目ID)
		/// </summary>
		public int BindKeyID { get; set;}

		/// <summary>
		///BindType=3时, 绑定值的字段
        /// </summary>
		public string BindValueField { get; set;}

		/// <summary>
		///BindType=3时, 绑定文本的字段
		/// </summary>
		public string BindTextField { get; set;}

		/// <summary>
		///SelectedType=2或者SelectedType=3时, 每行显示的数量
        /// </summary>
		public int RepeatColumn { get; set;}

		/// <summary>
		///默认值
        /// </summary>
		public string DefaultValue { get; set;}

		/// <summary>
		///是否启用
		/// </summary>
		public bool IsUsing { get; set;}
    }
}
