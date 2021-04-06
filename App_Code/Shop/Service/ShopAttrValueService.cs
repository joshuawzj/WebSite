/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopAttrValueService.cs
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
    public class ShopAttrValueService : DbBase<ShopAttrValue>
    {
        #region 根据单一模式构建类的对象
        private ShopAttrValueService() { }  //私有构造函数

        private static ShopAttrValueService _object = null; //静态变量
        /// <summary>
        /// 提供类的实例属性
        /// </summary>
        public static ShopAttrValueService Instance
        {
            get
            {
                lock (typeof(ShopAttrValueService))
                {
                    if (_object == null)
                    {
                        _object = new ShopAttrValueService();
                    }

                    return _object;
                }
            }
        }
        #endregion
        /// <summary>
        /// 获取自定义属性信息
        /// </summary>
        /// <param name="AttrID">外键ID</param>
        /// <returns></returns>
        public IList<ShopAttrValue> GetAttrValueByAttrID(int AttrID)
        {
            return base.Query<ShopAttrValue>(" WHERE AttrID=@0 AND IsDel=0 ORDER BY Sort DESC,UpdateDate DESC", AttrID).ToList();

        }
        /// <summary>
        /// 获取自定义属性信息
        /// </summary>
        /// <param name="AttrValueIDs">主键ID集合</param>
        /// <returns></returns>
        public IList<ShopAttrValue> GetAttrValueByAttrValueIDs(string AttrValueIDs)
        {
            return base.Query<ShopAttrValue>(" WHERE IsDel=0 AND AttrValueID IN(" + AttrValueIDs + ") ORDER BY Sort DESC,UpdateDate DESC").ToList();
        }
        /// <summary>
        /// 主键ID集合排序由大到小
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns>主键ID集合排序由大到小</returns>
        public string OrderByIDs(string ids)
        {
            string[] list = ids.Split(',');
            string rslt = "";
            for (int i = list.Length; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (Convert.ToInt32(list[j]) > Convert.ToInt32(list[j + 1]))
                    {
                        int temp = 0;
                        temp = Convert.ToInt32(list[j]);
                        list[j] = list[j + 1];
                        list[j + 1] = temp.ToString();
                    }
                }
            }
            for (int z = list.Length; z > 0; z--)
            {
                rslt += list[z - 1].ToString() + ",";
            }
            if (rslt.Length > 0)
            {
                rslt = rslt.Substring(0, rslt.Length - 1);
            }
            return rslt;
        }
        /// <summary>
        /// 判断属性值是否被属性商品引用
        /// </summary>
        /// <param name="AttrValueID">属性值ID</param>
        /// <returns></returns>
        public bool AttrValueIsExistPro(int AttrValueID)
        {
            string charindex = "";
            switch (Whir.Service.CurrentDbType.CurDbType)
            {
                case  EnumType.DbType.SqlServer:
                    charindex = "charindex" + "(','+cast(" + AttrValueID + " as varchar(20))+',',','+AttrValueIDs+',')>0";
                    break;
                case EnumType.DbType.Oracle:
                    charindex = "INSTR" + "(','||AttrValueIDs||',',','||cast(" + AttrValueID + " as varchar(20))||',')>0";
                    break;
                case EnumType.DbType.MySql:
                    charindex = "INSTR" + "(CONCAT(',',AttrValueIDs,','),CONCAT(',',cast(" + AttrValueID + " as char),','))>0";
                    break;
                default :
                    charindex = "charindex" + "(','+cast(" + AttrValueID + " as varchar(20))+',',','+AttrValueIDs+',')>0";
                    break;
            }
            string sql = "select * from Whir_Shop_AttrPro where " + charindex ;
            return base.Query<ShopAttrPro>(sql).Count() > 0;
        }
    }
}