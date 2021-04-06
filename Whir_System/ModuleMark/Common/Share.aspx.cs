using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Whir.Framework;
using Whir.Repository;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Service;
using System.Data;

public partial class Whir_System_ModuleMark_Common_Share : System.Web.UI.Page {
    #region 属性

    /// <summary>
    /// 栏目Id
    /// </summary>
    protected int ColumnId = RequestUtil.Instance.GetQueryInt("columnId", 0);

    /// <summary>
    /// 子站在Id
    /// </summary>
    protected int SubjectId = RequestUtil.Instance.GetQueryInt("subjectId", 0);

    /// <summary>
    /// 详细页Id
    /// </summary>
    protected int ItemId = RequestUtil.Instance.GetQueryInt("itemId", 0);

    /// <summary>
    /// 分享实体
    /// </summary>
    protected Share Share { get; set; }
    #endregion
    protected void Page_Load(object sender, EventArgs e) {
        Share = ServiceFactory.ShareService.GetShare(ColumnId, SubjectId, ItemId);
    }
}