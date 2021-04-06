using System;
using Whir.Repository;

/// <summary>
///     静态化文件发布实体类
/// </summary>
[TableName("Whir_Dev_Parameter")]
[PrimaryKey("ParameterId")]
[Serializable]
public class ReleaseParameter
{
    public ReleaseParameter()
    {

    }

    /// <summary>
    ///     ParameterId
    /// </summary>
    public int ParameterId { get; set; }
    /// <summary>
    ///     栏目id
    /// </summary>
    public int ColumnId { get; set; }
     
    /// <summary>
    ///     是否开启分页
    /// </summary>
    public bool IsPage { get; set; }
    /// <summary>
    ///     分页数
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     模板类型 1-首页；2-列表页；3-详情页
    /// </summary>
    public int TempType { get; set; }

    /// <summary>
    ///     参数名称
    /// </summary>
    public string ParameterName { get; set; }
    /// <summary>
    ///     参数类型
    ///     1:字符串
    ///     2:数字
    ///     3:货币
    ///     4:日期和时间
    ///     5:是/否
    /// </summary>
    public string ParameterType { get; set; }
    /// <summary>
    ///     参数来源
    ///     1:自定义
    ///     2:指定栏目类别
    ///     3:绑定Sql
    ///     4:数值范围
    ///     5:日期范围
    /// </summary>
    public string ParameterSource { get; set; }
    /// <summary>
    ///     参数绑定
    /// </summary>
    public string ParameterBind { get; set; }




}