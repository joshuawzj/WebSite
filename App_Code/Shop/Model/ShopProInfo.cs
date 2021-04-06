/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopProInfo.cs
 * 文件描述：Whir_Shop_ProInfo实体对象
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
    [TableName("Whir_Shop_ProInfo")]
    [PrimaryKey("ProID", sequenceName = "seq_ezEIP")]
    public class ShopProInfo : DomainBase
    {


        /// <summary>
        ///主键ID
        /// <summary>
        public int ProID { get; set; }

        /// <summary>
        ///商品编号
        /// <summary>
        public string ProNO { get; set; }

        /// <summary>
        ///商品名称
        /// <summary>
        public string ProName { get; set; }

        /// <summary>
        ///商品分类主键ID
        /// <summary>
        public int CategoryID { get; set; }

        /// <summary>
        ///可购状态
        /// <summary>
        public bool IsAllowBuy { get; set; }

        /// <summary>
        /// 商品详细描述
        /// </summary>
        public string ProDesc { get; set; }

        /// <summary>
        ///SEO标题
        /// <summary>
        public string MetaTitle { get; set; }

        /// <summary>
        ///SEO关键字
        /// <summary>
        public string MetaKeyword { get; set; }

        /// <summary>
        ///SEO详细描述
        /// <summary>
        public string MetaDescription { get; set; }

        /// <summary>
        ///价格
        /// <summary>
        public decimal CostAmount { get; set; }

        /// <summary>
        ///商品主图片
        /// <summary>
        public string ProImg { get; set; }

        /// <summary>
        ///商品图片,以星号分割多张图片
        /// <summary>
        public string Images { get; set; }

        /// <summary>
        ///搜选项,对应多个搜选项表的主键ID，多个以英文逗号分隔开
        /// <summary>
        public string SearchValueIDs { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        [ResultColumn]
        public string CategoryName { get; set; }

        /// <summary>
        /// 属性商品最低价格
        /// </summary>
        [ResultColumn]
        public decimal AttrMinPrice { get; set; }

        /// <summary>
        /// 属性商品最高价格
        /// </summary>
        [ResultColumn]
        public decimal AttrMaxPrice { get; set; }

        /// <summary>
        /// 属性商品最高价格
        /// </summary>
        [ResultColumn]
        public int AttrCount { get; set; }

        /// <summary>
        ///属性ID集合，多个以英文逗号分隔开
        /// </summary>
        public string AttrIDs { get; set; }
    }
}
