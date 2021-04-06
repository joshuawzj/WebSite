/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Hit.aspx.cs
 * 文件描述：点击率异步操作页面
 */

using System;
using System.Web.UI;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class label_ajax_hit : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetFormString("colid").ToInt();
        ItemId = RequestUtil.Instance.GetFormString("itemid").ToInt();
        FieldName = "hits";
        var result = 0;
        var model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model != null)
        {
            //验证FieldName参数合法性
            var isExsit = DbHelper.CurrentDb.ExecuteScalar<object>(
                    @"SELECT COUNT(1)
                        FROM   Whir_Dev_Form form
                               INNER JOIN Whir_Dev_Field field
                                    ON  field.FieldID = form.FieldID
                        WHERE  form.ColumnId = @0
                               AND field.FieldName = @1",
                    ColumnId, FieldName).ToInt() > 0;

            if (isExsit)
            {
                //查询点击率
                var sql = "SELECT {0} FROM {1} WHERE {1}_PID=@0".FormatWith(FieldName, model.TableName);
                result = DbHelper.CurrentDb.ExecuteScalar<object>(sql, ItemId).ToInt();

                //增加点击率
                sql = "UPDATE {0} SET {1}=@0 WHERE IsDel=0 AND {0}_PID=@1".FormatWith(model.TableName, FieldName);
                DbHelper.CurrentDb.Execute(sql, ++result, ItemId);
            }
        }
        //返回点击率
        Response.Clear();
        Response.Write(result);
        Response.End();
    }

    #region 属性

    /// <summary>
    ///     栏目ID
    /// </summary>
    private int ColumnId { get; set; }

    /// <summary>
    ///     主键ID
    /// </summary>
    private int ItemId { get; set; }

    /// <summary>
    ///     点击率字段
    /// </summary>
    private string FieldName { get; set; }

    #endregion
}