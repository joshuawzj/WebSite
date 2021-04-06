/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：ShopFieldService.cs
 * 文件描述：示例操作类
 * 
 * 创建标识: yangwb 2013-01-31
 * 
 * 修改标识：
 */
using org.in2bits.MyXls;
//非系统引用
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

/// <summary>
///ShopProInfoService 的摘要说明
/// </summary>
public class ShopProInfoService : DbBase<ShopProInfo>
{
    #region 根据单一模式构建类的对象
    private ShopProInfoService() { }  //私有构造函数

    private static ShopProInfoService _object = null; //静态变量
    /// <summary>
    /// 提供类的实例属性
    /// </summary>
    public static ShopProInfoService Instance
    {
        get
        {
            lock (typeof(ShopProInfoService))
            {
                if (_object == null)
                {
                    _object = new ShopProInfoService();
                }

                return _object;
            }
        }
    }

    #endregion

    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <param name="proId">商品主键ID</param>
    /// <returns></returns>
    public ShopProInfo GetShopProById(int proId)
    {
        
        var filed = base.Query<ShopProInfo>(" WHERE ProID=@0 ", proId);
        return filed.SingleOrDefault();
    }
    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <param name="proId">商品主键ID</param>
    /// <returns></returns>
    public DataTable GetRowShopProById(int proId)
    {
        return DbHelper.CurrentDb.Query("SELECT * FROM Whir_Shop_ProInfo WHERE ProID=@0", proId).Tables[0];
        
    }
    /// <summary>
    /// 更新商品删除状态
    /// </summary>
    /// <param name="proId">商品ID</param>
    /// <returns></returns>
    public bool DeleteShopProById(int proId, int IsDel)
    {
        return Update<ShopProInfo>(" SET IsDel=@0 WHERE ProID=@1", IsDel, proId) > 0;
    }
    /// <summary>
    /// 修改栏目的排序
    /// </summary>
    /// <param name="proId">商品ID</param>
    /// <param name="sort">排序号</param>
    /// <returns></returns>
    public bool ModifyProInfoSort(int proId, decimal sort)
    {
        var result = base.Update<ShopProInfo>("SET Sort=@0 WHERE ProID=@1", sort, proId);
        return result > 0;
    }
    /// <summary>
    /// 返回最小排序号
    /// </summary>
    /// <returns></returns>
    public decimal GetMaxSort(List<int>ids)
    {
       return  base.SingleOrDefault<decimal>("SELECT top 1 Sort FROM Whir_Shop_ProInfo Where ProId in (@0) And Isdel=0 order by sort desc",ids.ToArray() );
        
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="list">导出列表</param>
    /// <param name="strFileName">导出文件名称，包含路径但不需要后辍名</param>
    /// <param name="columnName">对应数据源的字段名,区分大小写</param>
    /// <param name="excelName">对应数据源的列名</param>
    public void ExportExcel<T>(IList<T> list, string strFileName, string[] columnName, params string[] excelName)
    {
        XlsDocument xls = new XlsDocument();
        Worksheet sheet = xls.Workbook.Worksheets.Add("Sheet1");

        org.in2bits.MyXls.XF xf = xls.NewXF();
        //xf.Font.Bold = true;

        //填充表头
        for (int i = 0; i < excelName.Length; i++)
        {
            sheet.Cells.Add(1, i + 1, excelName[i], xf);
        }

        //填充内容
        int j = 2;
        foreach (T model in list)
        {
            Type tp = typeof(T);
            for (int i = 0; i < columnName.Length; i++)
            {
                string cName = columnName[i].Split('.')[0];
                System.Reflection.PropertyInfo PColumnName = tp.GetProperty(cName);
                object Name = null;

                try
                {
                    Name = PColumnName.GetValue(model, null);

                    if (columnName[i].Split('.').Length > 1 && Name != null)
                    {
                        //当前参数是一个实体
                        cName = columnName[i].Split('.')[1].ToString();
                        Type tmodel = Name.GetType();
                        System.Reflection.PropertyInfo PColumnName2 = tmodel.GetProperty(cName);
                        Name = PColumnName2.GetValue(Name, null);
                    }

                }
                catch
                {
                    throw new Exception("检查数组是否匹配与注意字段组的大小写!");
                }

                if (Name == null)
                    Name = "";

                if (PColumnName.PropertyType.Name.Contains("Bool"))
                {
                    Name = Name.ToString().ToLower() == "true" ? "是" : "否";
                }

                //Name = PColumnName.Name;

                if (PColumnName.Name.IndexOf("Price") > 0)
                {
                    try
                    {
                        Name = string.Format("{0:F2}", decimal.Parse(Name.ToStr()));
                    }
                    catch (Exception ex) { }
                }

                sheet.Cells.Add(j, i + 1, Name.ToString(), xf);
            }
            j++;
        }

        try
        {
            //保存   
            xls.FileName = strFileName;
            xls.Save(true);
        }
        catch
        {
            throw new Exception("导出文件处于打开状态！");
        }
    }

}