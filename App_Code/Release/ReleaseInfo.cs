using System;
using Whir.Repository;

/// <summary>
///     静态化文件发布实体类
/// </summary>
[TableName("Whir_Dev_Release")]
[PrimaryKey("ReleaseId")]
[Serializable]
public class ReleaseInfo 
{
    public ReleaseInfo()
    {

    }

    /// <summary>
    ///     ReleaseId
    /// </summary>
    public int ReleaseId { get; set; }

    /// <summary>
    ///     站点id
    /// </summary>
    public int SiteId { get; set; }
    /// <summary>
    ///     栏目id
    /// </summary>
    public int ColumnId { get; set; }
    /// <summary>
    ///     子站、专题id
    /// </summary>
    public int SubjectId { get; set; }
    /// <summary>
    ///     数据id
    /// </summary>
    public int ItemId { get; set; }
    /// <summary>
    ///     发布网址
    /// </summary>
    public string ReleaseUrl { get; set; }
    /// <summary>
    ///     aspx网址
    /// </summary>
    public string AspxUrl { get; set; }
    /// <summary>
    ///     是否成功发布
    /// </summary>
    public bool IsSuccess { get; set; }




}