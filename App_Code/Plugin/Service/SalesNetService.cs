/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：SalesNetService.cs
 * 文件描述：销售网络服务操作类
 */

using Whir.Service;
using Plu.Domain;

namespace Plu.Service
{
    /// <summary>
    ///ShopAttrService 的摘要说明
    /// </summary>
    public class SalesNetService : DbBase<SalesNet>
    {
        #region 根据单一模式构建类的对象
        private SalesNetService() { }  //私有构造函数

        private static SalesNetService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static SalesNetService Instance
        {
            get
            {
                lock (typeof(SalesNetService))
                {
                    if (_object == null)
                    {
                        _object = new SalesNetService();
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
            base.Update<SalesNet>("SET Sort=@0 WHERE PID=@1", sort, attrId);
        }
    }
}