using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Repository;

public partial class Whir_System_Module_Area_Area_Edit : SysManagePageBase
{
    /// <summary>
    /// 当前编辑ID号
    /// </summary>
    protected int AreaId { get; set; }
    /// <summary>
    /// 父级类别ID号
    /// </summary>
    protected int Pid { get; set; }
    protected List<Area> Alist = new List<Area>();
    protected Area CurrentArea { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("256"));
        AreaId = RequestUtil.Instance.GetQueryInt("id", 0);
        Pid = RequestUtil.Instance.GetQueryInt("pid", 0);
        if (!IsPostBack)
        {
            BindParentArea();
            CurrentArea =
                     DbHelper.CurrentDb.Query<Area>("SELECT * FROM  dbo.Whir_Cmn_Area WHERE IsDel=0 AND Id=@0", AreaId)
                         .FirstOrDefault() ?? ModelFactory<Area>.Insten();
           
        }
    }

    //绑定上级栏目
    private void BindParentArea()
    {
        List<Area> list = DbHelper.CurrentDb.Query<Area>("SELECT  * FROM  dbo.Whir_Cmn_Area WHERE IsDel=0").ToList();
        GetList(list, 0);
    }
  
    
    /// <summary>
    /// 递归组装数据按照层级关系排列出来
    /// </summary>
    /// <param name="list">数据</param>
    /// <param name="parentId">父级类别ID号</param>
    private void GetList(List<Area> list,int parentId)
    {
        List<Area> newlist = list.Where(c => c.Pid == parentId).ToList();
        foreach (Area item in newlist)
        {
            if (parentId != 0)
            {
                int count = DbHelper.CurrentDb.ExecuteScalar<int>("SELECT  COUNT(*) FROM  dbo.Whir_Cmn_Area WHERE IsDel=0 AND Pid=@0",item.Id);
                if (count > 0)
                {
                    item.Name = "　└─" + item.Name;
                }
                else
                {
                    item.Name = "　　　└─" + item.Name;
                }
            }
            
            Alist.Add(item);
            GetList(list, item.Id);
        }
    }

    /// <summary>
    /// 获取父类别路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetParentPath(int id)
    {
        string parentPath = string.Empty;
        Area model = DbHelper.CurrentDb.Query<Area>("SELECT  * FROM  dbo.Whir_Cmn_Area WHERE IsDel=0 AND Id=@0", id).FirstOrDefault();
        if (model != null) parentPath += model.ParentPath + model.Id + ",";
        return parentPath;
        
    }
}