
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using Shop.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
/// <summary>
///ShopFieldService 的摘要说明
/// </summary>
public class ShopFieldService : DbBase<ShopField>
{
    #region 根据单一模式构建类的对象
    private ShopFieldService() { }  //私有构造函数

    private static ShopFieldService _object; //静态变量
    /// <summary>
    /// 提供类的实例属性
    /// </summary>
    public static ShopFieldService Instance
    {
        get
        {
            lock (typeof(ShopFieldService))
            {
                return _object ?? (_object = new ShopFieldService());
            }
        }
    }

    #endregion

    /// <summary>
    /// 修改栏目的排序
    /// </summary>
    /// <param name="filedId">自定义属性主键ID</param>
    /// <param name="sort">排序号</param>
    /// <returns></returns>
    public bool ModifyFiledSort(int filedId, long sort)
    {
        var result = base.Update<ShopField>("SET Sort=@0 WHERE FieldID=@1", sort, filedId);
        return result > 0;
    }

    /// <summary>
    /// 获取自定义属性信息
    /// </summary>
    /// <param name="filedId">自定义属性主键ID</param>
    /// <returns></returns>
    public ShopField GetShopFileById(int filedId)
    {
        var filed = base.Query<ShopField>(" WHERE FieldID=@0 ", filedId);
        return filed.SingleOrDefault();
    }
    /// <summary>
    /// 获取非删除的自定义属性信息列表
    /// </summary>
    /// <returns></returns>
    public IList<ShopField> GetAllShopFileList()
    {
        return base.Query<ShopField>(" WHERE IsDel=0 ORDER BY Sort ASC,FieldID DESC ").ToList();
    }
    /// <summary>
    /// 获取商品引用的自定义属性信息列表
    /// </summary>
    /// <returns></returns>
    public IList<ShopField> GetProShopFileList()
    {
        return base.Query<ShopField>(" WHERE IsDel=0 AND IsUsing=1 ORDER BY Sort ASC,FieldID DESC ").ToList();
    }
    /// <summary>
    /// 判断字段名称是否存在
    /// </summary>
    /// <param name="fieldId">字段ID</param>
    /// <param name="fieldName">字段名称</param>
    /// <returns></returns>
    public bool IsExistByFieldName(int fieldId, string fieldName)
    {
        return Query<ShopField>(" WHERE FieldID!=@0 AND FieldName=@1 ", fieldId, fieldName).Any();
    }

    /// <summary>
    /// 获取公用提交页面中,选项字段的数据源
    /// </summary>
    /// <param name="form"></param>
    /// <param name="exceptCategoryId">单独分类或者多级分类中, 要排除掉的主键ID</param>
    /// <param name="subjectId">所属的子站ID</param>
    /// <returns></returns>
    public IList<ListItem> GetOptionsInSubject( ShopField form,  int exceptCategoryId, int subjectId)
    {
        IList<ListItem> listResult = new List<ListItem>();

        if (form.BindType == 1)//绑定文本
        {
            string bindText = form.BindText;
            if (!string.IsNullOrEmpty(bindText))
            {
                foreach (string option in bindText.Split(','))
                {
                    if (option.Contains("|"))
                    {
                        string key = option.Split('|')[0];
                        string value = option.Split('|')[1];
                        listResult.Add(new ListItem(value, key));
                    }
                }
            }
        }
        else if (form.BindType == 2)//绑定SQL
        {
            string bindSql = form.BindSql;

            bindSql = Regex.Replace(bindSql, "@SubjectID", subjectId.ToStr(), RegexOptions.IgnoreCase);

            DataTable dtResult = DbHelper.CurrentDb.Query(bindSql, null).Tables[0];
            foreach (DataRow row in dtResult.Rows)
            {
                listResult.Add(
                    new ListItem(
                        row[form.BindTextField].ToStr(),
                        row[form.BindValueField].ToStr()
                    )
                );
            }
        }
        else if (form.BindType == 3)//绑定多级类别
        {
            
        }
       
        return listResult;
    }
}