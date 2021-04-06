/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopOrderProductService.cs
 * 文件描述：订单商品服务操作类
 * 
 * 创建标识: liuyong 2013-02-04
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

namespace Shop.Service
{
    /// <summary>
    ///ShopOrderProductService 的摘要说明
    /// </summary>
    public class ShopOrderProductService : DbBase<ShopOrderProduct>
    {
        #region 根据单一模式构建类的对象
        private ShopOrderProductService() { }  //私有构造函数

        private static ShopOrderProductService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopOrderProductService Instance
        {
            get
            {
                lock (typeof(ShopOrderProductService))
                {
                    if (_object == null)
                    {
                        _object = new ShopOrderProductService();
                    }

                    return _object;
                }
            }
        }

        public DataTable GetShopOrderProductsByOrderID(int orderID)
        {
            if (orderID == 0)
                return null;
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT oPro.*");
            SQL.Append(",CASE WHEN oPro.AttrProID>0 AND attrPro.IsUseMainImage=0 THEN attrPro.ProImg ELSE pro.ProImg END AS ProImg");
            SQL.Append(",oinfo.ProductAmount,oinfo.PaymentID,oinfo.IsPaid,oinfo.TakeName");
            SQL.Append(",(SELECT COUNT(*) FROM Whir_Shop_OrderProduct WHERE IsDel=0 AND oPro.OrderID=OrderID) AS ProLength");
            SQL.Append(",oinfo.IsCancel,attrPro.AttrValueIDs");
            SQL.Append(" FROM Whir_Shop_OrderProduct oPro");
            SQL.Append(" INNER JOIN Whir_Shop_OrderInfo oinfo ON oPro.OrderID=oinfo.OrderID");
            SQL.Append(" INNER JOIN Whir_Shop_ProInfo pro ON oPro.ProID=pro.ProID");
            SQL.Append(" LEFT JOIN Whir_Shop_AttrPro attrPro ON oPro.AttrProID=attrPro.AttrProID");
            SQL.Append(" WHERE oPro.IsDel=0 AND oPro.OrderID=@0 ORDER BY oPro.CreateDate ASC");


            DataSet ds = DbHelper.CurrentDb.Query(SQL.ToStr(), orderID);
            if (ds == null || ds.Tables.Count == 0)
                return null;
            return ds.Tables[0];
        }
        #endregion

    }
}