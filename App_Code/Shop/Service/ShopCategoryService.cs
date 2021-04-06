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
//非系统
using Shop.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Repository;
/// <summary>
///ShopCategoryService 的摘要说明
/// </summary>
public class ShopCategoryService : DbBase<ShopCategoryService>
{
    #region 根据单一模式构建类的对象
    private ShopCategoryService() { }  //私有构造函数

    private static ShopCategoryService _object = null; //静态变量
    /// <summary>
    /// 提供类的实例属性
    /// </summary>
    public static ShopCategoryService Instance
    {
        get
        {
            lock (typeof(ShopCategoryService))
            {
                if (_object == null)
                {
                    _object = new ShopCategoryService();
                }

                return _object;
            }
        }
    }

    #endregion
    /// <summary>
    /// 获取非删除的自定义属性信息列表
    /// </summary>
    /// <param name="lcid">排除的类目ID</param>
    /// <returns></returns>
    public List<DataRow> GetAllCategoryList(int lcid)
    {
        return getCategoryRow(0, "├─",lcid);
    }


    /// <summary>
    /// 修改栏目的排序
    /// </summary>
    /// <param name="CategoryID">主键ID</param>
    /// <param name="sort">排序号</param>
    /// <returns></returns>
    public bool ModifyCategorySort(int CategoryID, long sort)
    {
        var result = base.Update<ShopCategory>("SET Sort=@0 WHERE CategoryID=@1", sort, CategoryID);
        return result > 0;
    }

    /// <summary>
    /// 获取自定义属性信息
    /// </summary>
    /// <param name="CategoryID">主键ID</param>
    /// <returns></returns>
    public ShopCategory GetCategoryByID(int CategoryID)
    {
        var category = base.Query<ShopCategory>(" WHERE CategoryID=@0 ", CategoryID);
        return category.SingleOrDefault();
    }

    /// <summary>
    /// 判断字段名称是否存在
    /// </summary>
    /// <param name="CategoryID">ID</param>
    /// <param name="CategoryName">名称</param>
    /// <returns></returns>
    public bool IsExistByCategoryName(int CategoryID, string CategoryName)
    {
        return base.Query<ShopCategory>(" WHERE CategoryID!=@0 AND CategoryName=@1  ", CategoryID, CategoryName).Count() > 0;
    }
    /// <summary>
    /// 判断是否有下级类目
    /// </summary>
    /// <param name="CategoryID">类目ID</param>
    /// <returns></returns>
    public bool IsExistChildCategory(int CategoryID)
    {
        return GetCategoryListByParentID(CategoryID).Count > 0;
    }

    /// <summary>
    /// 根据父ID获取下级类目信息
    /// </summary>
    /// <param name="ParentID">父ID</param>
    /// <returns></returns>
    public IList<ShopCategory> GetCategoryListByParentID(int ParentID)
    {
        return base.Query<ShopCategory>(" WHERE ParentID=@0 AND IsDel=0  ORDER BY sort DESC,createdate DESC", ParentID).ToList();
    }

    /// <summary>
    /// 获取所有类目信息
    /// </summary>
    /// <returns></returns>
    public IList<ShopCategory> GetList()
    {
        return base.Query<ShopCategory>(" WHERE IsDel=0  ORDER BY sort DESC,createdate DESC").ToList();
    }
    //解析类别树的层级
    private List<DataRow> getCategoryRow(int parentID, string str,int cid)
    {
        List<DataRow> list = new List<DataRow>();
        string SQL = "SELECT * FROM Whir_Shop_Category WHERE  ParentID={0} AND CategoryID!={1}  AND IsDel=0  ORDER BY sort DESC,createdate DESC";
        SQL = SQL.FormatWith(parentID,cid);
        DataTable table = DbHelper.CurrentDb.Query(SQL, null).Tables[0];

        for (int z = 0; z < table.Rows.Count; z++)
        {
            if (z == (table.Rows.Count - 1))//当前层次最后一个栏目时
            {
                str = str.Replace("├─", "└─");
            }
            DataRow dr = table.Rows[z];

            dr["CategoryName"] = str + dr["CategoryName"];
            list.Add(dr);

            string NewStr = str;
            if (str == "")
            {
                NewStr = "　├─";
            }
            else
            {
                NewStr = (NewStr.Replace("├─", "│").Replace("└─", "　") + "　├─");
            }
            List<DataRow> ChildList = getCategoryRow(dr["CategoryID"].ToInt(), NewStr,cid).ToList();
            list.AddRange(ChildList);
        }
        return list;
    }

    /// <summary>
    /// 获取商品相关的类别信息
    /// </summary>
    /// <param name="ProID">商品ID</param>
    /// <returns></returns>
    public IList<ShopCategory> GetCategoryListByProID(int ProID)
    {
        string charindex = "";
        switch (Whir.Service.CurrentDbType.CurDbType)
        {
            case EnumType.DbType.SqlServer:
                charindex = @"charindex((','+cast(CategoryID as varchar(20))+','),
                        (select (','+ParentPath+','+cast(CategoryID as varchar(20))+',') from Whir_Shop_Category 
                        where CategoryID in (select CategoryID from Whir_Shop_ProInfo where proid=" + ProID + ")))>0";
                break;
            case EnumType.DbType.Oracle:
                charindex = @"INSTR(
                        (select (','||ParentPath||','||cast(CategoryID as varchar(20))||',') from Whir_Shop_Category 
                        where CategoryID in (select CategoryID from Whir_Shop_ProInfo where proid=" + ProID + ")),(','||cast(CategoryID as varchar(20))||','))>0";
                break;
            case EnumType.DbType.MySql:
                charindex = @"INSTR(
                        (select CONCAT(',',ParentPath,',',cast(CategoryID as char),',') from Whir_Shop_Category 
                        where CategoryID in (select CategoryID from Whir_Shop_ProInfo where proid=" + ProID + ")),CONCAT(',',cast(CategoryID as char),','))>0";
                break;
            default:
                charindex = @"charindex((','+cast(CategoryID as varchar(20))+','),
                        (select (','+ParentPath+','+cast(CategoryID as varchar(20))+',') from Whir_Shop_Category 
                        where CategoryID in (select CategoryID from Whir_Shop_ProInfo where proid=" + ProID + ")))>0";
                break;
        }
        string SQL = @"select * from Whir_Shop_Category where " + charindex + " order by ParentPath";
        return base.Query<ShopCategory>(SQL).ToList();
    }

    /// <summary>
    /// 获取商品相关的类别信息
    /// </summary>
    /// <param name="ProID">商品ID</param>
    /// <returns></returns>
    public IList<ShopCategory> GetCategoryListByCategoryID(int CategoryID)
    {
        string charindex = "";
        switch (Whir.Service.CurrentDbType.CurDbType)
        {
            case EnumType.DbType.SqlServer:
                charindex = @"charindex(','+cast(CategoryID as varchar(20))+',' ,
                        (select ','+ParentPath+','  +cast(" + CategoryID + " as varchar(20))+',' from Whir_Shop_Category where CategoryID=" + CategoryID + "))>0";
                break;
            case EnumType.DbType.Oracle:
                charindex = @"INSTR((select ','||ParentPath||','  ||cast(" + CategoryID + " as varchar(20))||',' from Whir_Shop_Category where CategoryID=" + CategoryID + "),','||cast(CategoryID as varchar(20))||',')>0";

                break;
            case EnumType.DbType.MySql:
                charindex = @"INSTR(
                        (select CONCAT(',',ParentPath,',',cast(" + CategoryID + " as char),',') from Whir_Shop_Category where CategoryID=" + CategoryID + "),CONCAT(',',cast(CategoryID as char),','))>0";

                break;
            default:
                charindex = @"charindex(','+cast(CategoryID as varchar(20))+',' ,
                        (select ','+ParentPath+','  +cast(" + CategoryID + " as varchar(20))+',' from Whir_Shop_Category where CategoryID=" + CategoryID + "))>0";
                break;
        }
        string SQL = @"select * from Whir_Shop_Category
                        where " + charindex + " order by ParentPath";
        return base.Query<ShopCategory>(SQL).ToList();
    }
}