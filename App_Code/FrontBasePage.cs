using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP;
using Whir.Repository;

/// <summary>
///前台发布页面基类
/// </summary>
public class FrontBasePage : BasePage
{
    /// <summary>
    /// 转换为大写(测试方法)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string ToUper(object input)
    {
        return input.ToStr().ToUpper();
    }

    /// <summary>
    /// 取得当前用户名 
    /// </summary> 
    /// <returns>取得当前用户名</returns>
    public string GetUserName()
    {
        if (WebUser.IsLogin())
        {
            return WebUser.GetUserValue("LoginName");
        }
        return "匿名用户";
    }
    /// <summary>
    /// 获取当前用户id
    /// </summary>
    /// <returns></returns>
    public int GetUserId()
    {
        if (WebUser.IsLogin())
        {
            return WebUser.GetUserValue("Whir_Mem_Member_PId").ToInt();
        }
        return 0;
    }

    #region 取得栏目信息

    /// <summary>
    /// 取得栏目信息
    /// </summary>
    /// <param name="colunmId"></param> 
    /// <returns></returns>
    public string GetColumnInfo(object colunmId)
    {
        return GetColumnInfo(colunmId, "ColumnName");
    }

    /// <summary>
    /// 取得栏目信息
    /// </summary>
    /// <param name="colunmId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetColumnInfo(object colunmId, object name)
    {
        var result = "";
        var fieldName = name == null ? "ColumnName" : name.ToStr();
        var id = colunmId.ToInt32(0);
        if (id > 0)
        {
            var column = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(id);
            if (column != null)
            {
                var type = column.GetType();
                string fieldBuilder = "";
                PropertyInfo[] fields = type.GetProperties();//获取指定对象的所有公共属性
                foreach (PropertyInfo field in fields)
                {
                    if (String.Compare(field.Name.ToStr(), fieldName.ToStr(), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return field.GetValue(column, null).ToStr();
                    }
                    fieldBuilder += field.Name + ",";
                }
                return "栏目不存在该字段，请输入：{0}内字段".FormatWith(fieldBuilder.TrimEnd(','));
            }
            result = "栏目不存在";
        }
        else
        {
            result = "栏目ID格式错误";
        }
        return result;
    }
    #endregion

    #region 获取会员相关信息

    /// <summary>
    /// 获取会员注册协议
    /// </summary>
    /// <returns></returns>
    public static string GetRegisterAgreement()
    {
        return WebUser.GetMemberRegisterAgreement().Replace("\n", "<br/>");
    }
    #endregion

    /// <summary>
    ///  获取 in 方法的sql语句
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetSql(object columnId, object id)
    {
        string str = "";
        if (id.ToStr().IsEmpty())
            str = "select  * from Whir_Sit_SiteInfo where 1=2";//返回一个查不到数据的SQL语句,这里是固定的
        else
        {
            if (id.ToStr().Contains(","))  //用于in 一些类似（3,12,15）id组合
            {
                var ids = id.ToStr().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.ToInt());
                if (ids.Count() > 0)
                {
                    id = string.Join(",", ids.ToArray());
                    //和list置标里面的sql写法一样
                    str = "select * from Whir_U_product where TypeId={1} and Whir_U_product_PId in ({0}) order by Sort desc,CreateDate desc"
                                     .FormatWith(id.ToStr(), columnId.ToStr());
                }
                else
                    str = "select * from Whir_Sit_SiteInfo where 1=2";//返回一个查不到数据的SQL语句,这里是固定的

            }
            else  //用于in 单个id值 sql语句和list的一致，方便修改
                str = "select * from Whir_U_product where TypeId={1} and Whir_U_product_PId in ({0}) order by Sort desc,CreateDate desc".FormatWith(id.ToStr(), columnId.ToStr());
        }

        return str;
    }

	
	
	  //检查用户是否到期
    public int CheckTime()
    {
        int memberid = GetUserId();
        DateTime endtime = DbHelper.CurrentDb.ExecuteScalar<DateTime>("select endtime from Whir_Mem_Member where Whir_Mem_Member_pid = @0", memberid);

        DateTime Convert_dt1 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", endtime.Year, endtime.Month, endtime.Day));
        DateTime Convert_dt2 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day));

        int Days = (Convert_dt1 - Convert_dt2).Days.ToInt32();

		
        if (Days > 0)
        {
			//没过期
            return 1;
        }
        else
        {
            return 0;
        }
    }

}