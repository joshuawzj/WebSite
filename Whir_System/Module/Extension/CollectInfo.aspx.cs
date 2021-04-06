using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Repository;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Framework;

public partial class whir_system_module_extension_CollectInfo : SysManagePageBase
{
    protected int CollectId { get; set; }
    protected Collect Collection { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("354"));
        CollectId = RequestUtil.Instance.GetQueryInt("collectid", 0);
        Collection = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId) ?? ModelFactory<Collect>.Insten();
        if (Collection != null)
        {
            var table = DbHelper.CurrentDb.Query(
                            @"SELECT form.FormId,field.FieldName,form.FieldAlias,field.FieldType,form.DefaultValue,form.Sort FROM Whir_Dev_Form form INNER JOIN Whir_Dev_Field field ON form.FieldID=field.FieldID
                    WHERE form.IsDel=0 AND ColumnId=@0 AND field.IsHidden=0 AND field.FieldName!='RedirectUrl' AND field.fieldName!='EnableRedirectUrl' And TypeName!='ntext' ORDER BY Sort Desc",
                            Collection.ColumnId).Tables[0];
            dropFilterField.DataTextField = "FieldAlias";
            dropFilterField.DataValueField = "FieldName";
            dropFilterField.DataSource = table;
            dropFilterField.DataBind();
        }

    }
}