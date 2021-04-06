/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopCourierService.cs
 * 文件描述：商品规格服务操作类
 * 
 * 创建标识: liuyong 2013-01-30
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;

using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

namespace Shop.Service
{
    /// <summary>
    ///ShopAttrService 的摘要说明
    /// </summary>
    public class ShopCourierService : DbBase<ShopCourier>
    {
        #region 根据单一模式构建类的对象
        private ShopCourierService() { }  //私有构造函数

        private static ShopCourierService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopCourierService Instance
        {
            get
            {
                lock (typeof(ShopCourierService))
                {
                    if (_object == null)
                    {
                        _object = new ShopCourierService();
                    }

                    return _object;
                }
            }
        }
        #endregion
    }
}