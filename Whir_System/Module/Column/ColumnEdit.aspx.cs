/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：column_edit.aspx.cs
 * 文件描述：栏目的添加和编辑页面
 *          
 *
 *          1. 单条添加栏目, 直接Insert到数据库, 同时根据这个栏目的模型ID为这个栏目创建Form字段
 *          2. 批量添加栏目, 根据多行文本框输入的内容, 进行解析层级结构(并预览), Save到数据库中;
 *              注: 因为不知道批量添加的栏目模型, 所以在批量添加时不为这些栏目创建Form字段
 *          3. 高级设置的模板选择, 打开template_select.aspx页面, 返回选中的模板文件相对与该站点的路径
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;

public partial class Whir_System_Module_Column_ColumnEdit : SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目ID
    /// </summary>
    protected int ColumnId { get { return RequestUtil.Instance.GetQueryInt("ColumnId", 0); } }

    /// <summary>
    /// 当前父栏目ID
    /// </summary>
    protected int ParentId { get { return RequestUtil.Instance.GetQueryInt("ParentId", 0); } }

    /// <summary>
    /// 所有功能模型，用于绑定下拉框
    /// </summary>
    protected List<Model> AllModel { get; set; }

    /// <summary>
    /// 所有工作流，用户绑定下拉框
    /// </summary>
    protected List<WorkFlow> AllWorkFlow { get; set; }

    /// <summary>
    /// 所有栏目，用户绑定下拉框
    /// </summary>
    protected List<Column> AllColumn { get; set; }
    protected IList<Column> ColumnTreeList { get; set; }

    protected Column CurrentColumn { get; set; }


    /// <summary>
    /// 可上传文件后缀名, 以英文逗号分隔开
    /// </summary>
    protected string EnableExtensionNames { get; private set; }

    /// <summary>
    /// 可上传文件后缀名, 'jpg','png','gif','bmp'
    /// </summary>
    protected string AllowPicType { get; private set; }

    /// <summary>
    /// 弹窗选文件只显示指定类型文件，格式： ".jpg,.png,.gif,.bmp"
    /// </summary>
    protected string AcceptPicType { get; private set; }

    protected SystemConfig SystemConfig { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeActionPermission(IsRoleHaveColumnRes("栏目修改", ColumnId, -1));
        SystemConfig = ConfigHelper.GetSystemConfig();
        AllWorkFlow = ServiceFactory.WorkFlowService.GetList().OrderBy(p => p.WorkFlowName).ToList();
        AllModel = ServiceFactory.ModelService.GetList().OrderBy(p => p.ModelName).ToList();
        AllColumn = ServiceFactory.ColumnService.GetListAllColumn(CurrentSiteId)
            .OrderBy(p => p.Sort)
            .ThenBy(p => p.CreateDate)
            .ToList();
        ColumnTreeList = ServiceFactory.ColumnService.GetList(0, CurrentSiteId, 0, 0, ColumnId);
        //GetNodeColumns(0, 1);

        CurrentColumn = AllColumn.FirstOrDefault(p => p.ColumnId == ColumnId)
                        ?? ModelFactory<Column>.Insten();

        //上传控件参数
        UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();
        foreach (string extension in uploadConfig.AllowPicType.Split('|'))
        {
            AllowPicType += "'" + extension + "'" + ",";
            EnableExtensionNames += extension + ",";
            AcceptPicType += "." + extension + ",";
        }
        AllowPicType = AllowPicType.TrimEnd(',');
        AcceptPicType = AcceptPicType.TrimEnd(',');
    }

    /// <summary>
    /// 获取下级栏目 暂时不使用
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private List<Column> GetNodeColumns(int parentId, int level)
    {
        var columns = new List<Column>();
        foreach (var column in AllColumn.Where(p => p.ParentId == parentId))
        {
            int fix = column.LevelNum;
            while (fix-- > 0)
            {
                column.ColumnName = " " + column.ColumnName;
            }
            columns.Add(column);
            columns.AddRange(GetNodeColumns(column.ColumnId, level + 1));
        }
        return columns;
    }
}