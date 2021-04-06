/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopConsultService.cs
 * 文件描述：商品咨询服务操作类
 * 
 * 创建标识: liuyong 2013-02-01
 * 
 * 修改标识：
 */

using System.Linq;
//非系统引用
using Shop.Domain;
using Whir.Service;

namespace Shop.Service
{
    /// <summary>
    ///ShopConsultService 的摘要说明
    /// </summary>
    public class ShopConsultService : DbBase<ShopConsult>
    {
        #region 根据单一模式构建类的对象
        private ShopConsultService() { }  //私有构造函数

        private static ShopConsultService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopConsultService Instance
        {
            get
            {
                lock (typeof(ShopConsultService))
                {
                    if (_object == null)
                    {
                        _object = new ShopConsultService();
                    }

                    return _object;
                }
            }
        } 
        #endregion

        /// <summary>
        /// 获取咨询信息
        /// </summary>
        /// <param name="consultId"></param>
        /// <returns></returns>
        public ShopConsult GetShopConsultById(int consultId)
        {

            var filed = base.Query<ShopConsult>(" WHERE ConsultID=@0 ", consultId);
            return filed.SingleOrDefault();
        }
    }
}