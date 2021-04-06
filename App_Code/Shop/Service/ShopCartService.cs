/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopCartService.cs
 * 文件描述：购物车服务操作类
 * 
 * 创建标识: lurong 2013-2-18
 * 
 * 修改标识：
 */
using System;
using System.Data;
using System.Text;

using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.Domain;

namespace Shop.Service
{

    /// <summary>
    /// 购物车操作
    /// </summary>
    public class ShopCartService:DbBase<ShopCart>
    {
        #region 根据单一模式构建购物车实体类
        private ShopCartService(){ }//私有构造函数
        private static ShopCartService _boject = null;//静态变量


        /// <summary>
        /// 购物车实例
        /// </summary>
        public static ShopCartService Instance
        {
            get {
                if (_boject == null)
                    _boject = new ShopCartService();
                return _boject;
            }

        }
        #endregion

         #region 扩展方法
        /// <summary>
        /// 获取购物车中的商品信息(返回三张表，0：是返回购物车详细信息；1：返回商品总数；2：返回商品价格总记)
        /// </summary>
        /// <param name="UniqueID">购买者标识（未登陆前记录全局唯一标识）</param>
        /// <returns></returns>
        public DataSet GetShopCartDetailed()
        {
            DataSet ds = new DataSet();
          
            StringBuilder SQL = new StringBuilder();
            DataTable[] tempTable = { new DataTable("list"), new DataTable("QutityTable"), new DataTable("CostAmountTable") };
            #region 商品列表
            SQL.Append("SELECT cart.CartID,cart.UniqueID,cart.ProID,cart.AttrProID,cart.Qutity,cart.CreateDate");
            SQL.Append(",pro.ProNO,pro.ProName,attrPro.AttrValueIDs,attrPro.AttrValueNames,pro.IsAllowBuy, pro.IsDel AS proIsDel, attrPro.IsDel AS attrIsDel");
            SQL.Append(",CASE WHEN cart.AttrProID>0 AND attrPro.IsUseMainImage=0 THEN attrPro.ProImg ELSE pro.ProImg END AS ProImg");
            SQL.Append(",CASE WHEN cart.AttrProID>0 THEN attrPro.CostAmount ELSE pro.CostAmount END AS CostAmount");
            SQL.Append(",CASE WHEN cart.AttrProID>0 AND pro.IsAllowBuy=1 AND pro.IsDel=0 AND attrPro.IsDel=0  THEN 1 WHEN pro.IsAllowBuy=1 AND pro.IsDel=0 THEN 1 ELSE 0 END AS IsBuy");
            SQL.Append(" FROM Whir_Shop_Cart cart");
            SQL.Append(" INNER JOIN Whir_Shop_ProInfo pro ON cart.ProID=pro.ProID");
            SQL.Append(" LEFT JOIN Whir_Shop_AttrPro attrPro ON cart.AttrProID=attrPro.AttrProID");
            SQL.Append(" WHERE UniqueID=@0");
            SQL.AppendLine("");

           
            DataSet temp = DbHelper.CurrentDb.Query(SQL.ToStr(), GetMemberIdForCart());
            if (temp != null && temp.Tables.Count > 0)
            {
               
                tempTable[0] = temp.Tables[0].Copy();
                tempTable[0].TableName = "list";
            }
              

            #endregion 
           

            #region 商品总数
            SQL = new StringBuilder();
            SQL.Append("SELECT SUM(Qutity) AS Qutity FROM Whir_Shop_Cart cart");
            SQL.Append(" INNER JOIN Whir_Shop_ProInfo pro ON cart.ProID=pro.ProID");
            SQL.Append(" LEFT JOIN Whir_Shop_AttrPro attrPro ON cart.AttrProID=attrPro.AttrProID");
            SQL.Append(" WHERE UniqueID=@0");
            SQL.AppendLine("");
            temp = DbHelper.CurrentDb.Query(SQL.ToStr(), GetMemberIdForCart());
            if (temp != null && temp.Tables.Count > 0)
            {
                tempTable[1] = temp.Tables[0].Copy();
                tempTable[1].TableName = "QutityTable";
            }
            #endregion


            #region 商品价格总记
            SQL = new StringBuilder();
            SQL.Append("SELECT SUM(CostAmount*Qutity) AS CostAmount FROM (SELECT cart.Qutity,CASE WHEN cart.AttrProID>0 THEN attrPro.CostAmount ELSE pro.CostAmount END AS CostAmount  FROM Whir_Shop_Cart cart");
            SQL.Append(" INNER JOIN Whir_Shop_ProInfo pro ON cart.ProID=pro.ProID");
            SQL.Append(" LEFT JOIN Whir_Shop_AttrPro attrPro ON cart.AttrProID=attrPro.AttrProID");
            SQL.Append(" WHERE UniqueID=@0) A");
            SQL.Append("");
            temp = DbHelper.CurrentDb.Query(SQL.ToStr(), GetMemberIdForCart());
            if (temp != null && temp.Tables.Count > 0)
            {
                tempTable[2] = temp.Tables[0].Copy();
                tempTable[2].TableName = "CostAmountTable";
            }
            #endregion


            ds.Tables.AddRange(tempTable);
            return ds;
        }

       

        /// <summary>
        /// 获取购物车车主标识
        /// </summary>
        /// <returns></returns>
        public string GetMemberIdForCart()
        {
            string uid = WebUser.GetUserValue("Whir_Mem_Member_PID");
            StringBuilder SQL = new StringBuilder();
            if (string.IsNullOrEmpty(uid))
            {
                if (CookieUtil.Instance.GetCookie("tempUser") == null)
                {
                    CookieUtil.Instance.SetCookie("tempUser", "MemberID", Guid.NewGuid().ToStr(), DateTime.Now.AddDays(5));
                }
                uid = CookieUtil.Instance.GetCookieValue("tempUser", "MemberID");
            }
            else
            {

                if (!string.IsNullOrEmpty(CookieUtil.Instance.GetCookieValue("tempUser", "MemberID")))
                {
                    SQL.Append("UPDATE Whir_Shop_Cart SET UniqueID=@0 WHERE UniqueID=@1");
                    DbHelper.CurrentDb.Query(SQL.ToStr(), uid, CookieUtil.Instance.GetCookieValue("tempUser", "MemberID"));
                    CookieUtil.Instance.RemoveCookie("tempUser");
                }
            }
            //SQL = new StringBuilder();
            //SQL.Append("UPDATE Whir_Shop_Cart SET CreateDate=@0 WHERE UniqueID=@1");
            //DbHelper.CurrentDb.Query(SQL.ToStr(),DateTime.Now, uid);
            return uid;
        }

        /// <summary>
        /// 往购物车添加商品(0：表示正常,1:表示主商品不存在,2:表示属性商品不存在)
        /// </summary>
        /// <param name="proID">主商品ID</param>
        /// <param name="attrProID">规格商品ID</param>
        /// <param name="qutity">商品数量，为0时将从购物车中移出此商品</param>
        /// <param name="isCumulative">是否，对商品数量进行累加</param>
        /// <returns></returns>
        public int AddCart(int proID, int attrProID, int qutity, bool isCumulative)
        {
            string uniqueID=GetMemberIdForCart();
            StringBuilder SQL = new StringBuilder();

            #region 判断主商品是否可购买
            SQL.Append("SELECT * FROM Whir_Shop_ProInfo WHERE ProID=@0 AND IsDel=0 AND IsAllowBuy=1");
            if (DbHelper.CurrentDb.SingleOrDefault<ShopProInfo>(SQL.ToStr(), proID, attrProID) == null)
                return 1;
            #endregion
            #region 判断属性商品是否可购买
            if (attrProID > 0)
            {
                SQL = new StringBuilder();
                SQL.Append("SELECT * FROM Whir_Shop_AttrPro WHERE ProID=@0 AND AttrProID=@1 AND IsDel=0 ");
                if (DbHelper.CurrentDb.SingleOrDefault<ShopProInfo>(SQL.ToStr(), proID, attrProID) == null)
                    return 1;
            }
            #endregion
            SQL = new StringBuilder();
            SQL.Append("SELECT * FROM Whir_Shop_Cart WHERE ProID=@0 AND AttrProID=@1 AND UniqueID=@2");

            ShopCart shopCart = DbHelper.CurrentDb.SingleOrDefault<ShopCart>(SQL.ToStr(), proID, attrProID, uniqueID);
            if (shopCart == null || shopCart.CartID == 0)
            {
                shopCart = ModelFactory<ShopCart>.Insten();
                shopCart.ProID = proID;
                shopCart.AttrProID = attrProID;
                shopCart.UniqueID = uniqueID;
                if (qutity == 0)
                    return 0;
            }

            if (isCumulative)
                shopCart.Qutity += qutity;
            else
                shopCart.Qutity = qutity;

            if (shopCart.Qutity > 0)
            {
                if (shopCart.CartID == 0)
                    Save(shopCart);
                else
                    Update(shopCart);
            }
            else
                Delete(shopCart);
            return 0;
        }


        /// <summary>
        /// 往购物车添加商品 (返回 0:表示成功;1：表示cartId匹配的数据不存在
        /// </summary>
        /// <param name="proID">主商品ID</param>
        /// <param name="attrProID">规格商品ID</param>
        /// <param name="qutity">商品数量，为0时将从购物车中移出此商品</param>
        /// <param name="isCumulative">是否，对商品数量进行累加</param>
        public int AddCart(int cartId, int qutity)
        {
            string uniqueID = GetMemberIdForCart();
            StringBuilder SQL = new StringBuilder();

            SQL.Append("SELECT * FROM Whir_Shop_Cart WHERE CartID=@0");

            ShopCart shopCart = DbHelper.CurrentDb.Single<ShopCart>(SQL.ToStr(), cartId);
            if (shopCart == null || shopCart.CartID == 0)
            {
                    return 1;
            }

            shopCart.Qutity = qutity;

            if (shopCart.Qutity > 0)
                    Update(shopCart);
            else
                Delete(shopCart);
            return 0;
        }

        /// <summary>
        /// 往购物车添加商品
        /// </summary>
        /// <param name="proID">主商品ID</param>
        /// <param name="attrProID">规格商品ID</param>
        /// <param name="qutity">商品数量，已经累加方式加入购物车，为0时将从购物车中移出此商品</param>
        public int AddCart(int proID, int attrProID, int qutity)
        {
           return AddCart(proID,attrProID,qutity,true);
        }

        /// <summary>
        /// 移出商品
        /// </summary>
        /// <param name="cartID"></param>
        /// <returns></returns>
        public int RemovePro(int cartID)
        {
            try { }
            catch { }
            return DbHelper.CurrentDb.Delete<ShopCart>(cartID);
        }

        /// <summary>
        /// 清除购物车中的商品
        /// </summary>
        /// <returns></returns>
        public int ClearCart()
        {
            return DbHelper.CurrentDb.Delete<ShopCart>("WHERE UniqueID=@0", GetMemberIdForCart());
        }

        /// <summary>
        /// 购物车垃圾清理
        /// </summary>
        public void GCCart()
        {
            DateTime vilTime = DateTime.Now.AddDays(-3);//清除n天前的数据
            DbHelper.CurrentDb.Delete<ShopCart>("WHERE CreateDate<=CAST(@0 AS DATETIME)",vilTime);
        }

        #endregion
    }
}