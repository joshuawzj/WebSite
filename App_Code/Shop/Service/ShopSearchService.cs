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
    ///ShopSearchService 的摘要说明
    /// </summary>
    public class ShopSearchService : DbBase<ShopSearch>
    {
        #region 根据单一模式构建类的对象
        private ShopSearchService() { }  //私有构造函数

        private static ShopSearchService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopSearchService Instance
        {
            get
            {
                lock (typeof(ShopSearchService))
                {
                    if (_object == null)
                    {
                        _object = new ShopSearchService();
                    }

                    return _object;
                }
            }
        }
        #endregion

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="searchID">主键</param>
        /// <param name="sort">排序值</param>
        /// <returns></returns>
        public void ModifySort(int searchID, long sort)
        {
            base.Update<ShopSearch>("SET Sort=@0 WHERE SearchID=@1", sort, searchID);
        }
        /// <summary>
        /// 获取非删除的搜选项
        /// </summary>
        /// <returns></returns>
        public IList<ShopSearch> GetAllShopSearchList()
        {
            return base.Query<ShopSearch>(" WHERE IsDel=0 ORDER BY Sort DESC,UpdateDate DESC ").ToList();
        }
    }
}