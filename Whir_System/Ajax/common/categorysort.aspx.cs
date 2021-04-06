using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.ezEIP.Web;

public partial class whir_system_ajax_common_categorysort : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Clear();
            string sort = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("sort"));
            string columnId = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("columnId"));
            string itemId = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("itemId"));
            var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt32());
            if (column != null)
            {
                var model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
                if (model != null)
                {
                    var tableName = model.TableName;
                    if (!tableName.IsEmpty())
                    {
                        var sql = "UPDATE {0} SET Sort={1} WHERE {0}_PID={2}".FormatWith(tableName, sort.ToInt64(), itemId.ToInt32());
                        DbHelper.CurrentDb.Execute(sql);
                        Response.Write("ok");
                    }
                }
            }
            Response.End();
        }
    }
}