/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopFieldService.cs
 * 文件描述：示例操作类
 * 
 * 创建标识: yangwb 2013-01-31
 * 
 * 修改标识：
 */
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
//非系统引用
using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

/// <summary>
///ShopAttrProService 的摘要说明
/// </summary>
public class ShopAttrProService : DbBase<ShopAttrPro>
{
    #region 根据单一模式构建类的对象
    private ShopAttrProService() { }  //私有构造函数

    private static ShopAttrProService _object = null; //静态变量
    /// <summary>
    /// 提供类的实例属性
    /// </summary>
    public static ShopAttrProService Instance
    {
        get
        {
            lock (typeof(ShopAttrProService))
            {
                if (_object == null)
                {
                    _object = new ShopAttrProService();
                }

                return _object;
            }
        }
    }

    #endregion
    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <param name="ProID">商品ID</param>
    /// <returns></returns>
    public IList<ShopAttrPro> GetShopAttrProByProID(int ProID)
    {
        return base.Query<ShopAttrPro>(" WHERE ProID=@0 ", ProID).ToList();

    }
    /// <summary>
    /// 删除商品规格信息
    /// </summary>
    /// <param name="ProID">商品ID</param>
    /// <returns></returns>
    public bool DeleteShopAttrProByProID(int ProID)
    {
        return base.Delete("WHERE ProID=@0", ProID) > 0;
    }
    /// <summary>
    /// 删除商品规格信息
    /// </summary>
    /// <param name="ProID">商品ID</param>
    /// <returns></returns>
    public bool DeleteShopAttrProByProIDNotAttrProids(int ProID, string attrproids)
    {
        return base.Delete("WHERE ProID="+ProID+" and AttrProID not in("+attrproids+")") > 0;
    }
    /// <summary>
    /// 更新规格商品中的规格名称
    /// </summary>
    /// <param name="AttrValueID">规格ID</param>
    /// <param name="NewAttrValueName">规格名称</param>
    /// <returns></returns>
    public bool UpdateAttrNames(int AttrValueID, string NewAttrValueName)
    {
        string charindex = "";
        switch (Whir.Service.CurrentDbType.CurDbType)
        {
            case EnumType.DbType.SqlServer:
                charindex = "charindex((','+cast(@2 as varchar(20))+','),(','+AttrValueIDs+','))>0 ";
                break;
            case EnumType.DbType.Oracle:
                charindex = "INSTR" + "(','||AttrValueIDs||',',','||cast(@2 as varchar(20))||',')>0";
                break;
            case EnumType.DbType.MySql:
                charindex = "INSTR" + "(CONCAT(',',cast(@2 as char),','),CONCAT(',',AttrValueIDs,','))>0";
                break;
            default:
                charindex = "charindex" + "(','+cast(@2 as varchar(20))+',',','+AttrValueIDs+',')>0";
                break;
        }
        string sql = @"update Whir_Shop_AttrPro set AttrValueNames=
                        LTRIM(
                        RTRIM(
                        replace((' '+AttrValueNames+' '),(select (' '+AttrValueName+' ') from Whir_Shop_AttrValue where AttrValueID=@0),(' '+@1+' '))
                        )
                        )
                        where " + charindex;
        return base.Execute(sql, AttrValueID, NewAttrValueName, AttrValueID) > 0;
    }
}