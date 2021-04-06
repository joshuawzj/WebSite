/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopAttrService.cs
 * 文件描述：商品规格服务操作类
 * 
 * 创建标识: liuyong 2013-01-30
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

namespace Shop.Service
{
    /// <summary>
    ///ShopAttrService 的摘要说明
    /// </summary>
    public class ShopAttrService : DbBase<ShopAttr>
    {
        #region 根据单一模式构建类的对象
        private ShopAttrService() { }  //私有构造函数

        private static ShopAttrService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopAttrService Instance
        {
            get
            {
                lock (typeof(ShopAttrService))
                {
                    if (_object == null)
                    {
                        _object = new ShopAttrService();
                    }

                    return _object;
                }
            }
        }
        #endregion

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="attrId">主键</param>
        /// <param name="sort">排序值</param>
        /// <returns></returns>
        public void ModifySort(int attrId, long sort)
        {
            base.Update<ShopAttr>("SET Sort=@0 WHERE AttrID=@1", sort, attrId);
        }
        /// <summary>
        /// 获取非删除的规格
        /// </summary>
        /// <returns></returns>
        public IList<ShopAttr> GetAllShopAttrList()
        {
            return base.Query<ShopAttr>(" WHERE IsDel=0 ORDER BY Sort DESC,UpdateDate DESC ").ToList();
        }
        /// <summary>
        /// 获取商品相关的规格
        /// </summary>
        /// <returns></returns>
        public IList<ShopAttr> GetShopAttrListByAttrValueIDs(string AttrValueIDs)
        {
            return base.Query<ShopAttr>(@" select * from Whir_Shop_Attr where AttrID 
            in (select AttrID from Whir_Shop_AttrValue where AttrValueID in (" + AttrValueIDs + "))  ORDER BY Sort DESC,UpdateDate DESC").ToList();
        }
    }
}