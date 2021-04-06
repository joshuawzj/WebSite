/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopSearchValueService.cs
 * 文件描述：商品规格值服务操作类
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
    ///ShopAttrValueService 的摘要说明
    /// </summary>
    public class ShopSearchValueService : DbBase<ShopSearchValue>
    {
        #region 根据单一模式构建类的对象
        private ShopSearchValueService() { }  //私有构造函数

        private static ShopSearchValueService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopSearchValueService Instance
        {
            get
            {
                lock (typeof(ShopSearchValueService))
                {
                    if (_object == null)
                    {
                        _object = new ShopSearchValueService();
                    }

                    return _object;
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取自定义属性信息
        /// </summary>
        /// <param name="SearchID">外键ID</param>
        /// <returns></returns>
        public IList<ShopSearchValue> GetSearchValueBySearchID(int SearchID)
        {
            return base.Query<ShopSearchValue>(" WHERE SearchID=@0 AND IsDel=0 ", SearchID).ToList();
           
        }
    }
}