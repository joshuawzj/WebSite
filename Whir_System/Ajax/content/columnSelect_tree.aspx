<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>

<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>

<script type="text/C#" runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        int siteID = RequestUtil.Instance.GetQueryInt("siteid", 0);
        int currentColumnID = RequestUtil.Instance.GetQueryInt("columnid", 0);
        
        int parentID = RequestUtil.Instance.GetQueryInt("id", 0);

        string result = "[";

        Column currentColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(currentColumnID);
        if (currentColumn != null)
        {
            var listColumn = ServiceFactory.ColumnService.GetListByParentId(parentID, siteID).Where(p=>p.SiteType == 0);
                                                                                                //不是子站或者专题栏目
            foreach (Column column in listColumn)
            {
                result += "{";
                result += "id : '{0}',".FormatWith(column.ColumnId);
                result += "name : '{0}',".FormatWith(trimStartChar(column.ColumnName));
                result += "open : true";
                
                if (column.ModelId != currentColumn.ModelId)
                {
                    //不是同一个模型的栏目不允许勾选
                    result += ",nocheck:true"; 
                }

                if (currentColumnID == column.ColumnId)
                {
                    //同一个栏目不允许勾选
                    result += ",nocheck:true"; 
                }
                
                IList<Column> listChildColumn = ServiceFactory.ColumnService.GetListByParentId(column.ColumnId, siteID);
                if (listChildColumn.Count > 0)
                {
                    //是否有子节点
                    result += ",isParent:true";
                }
                result += "},";
            }
            result = result.TrimEnd(',');
        }
        result += "]";

        Response.Write(result);
        Response.End();
    }

    //去掉前面的"┝"符号
    private string trimStartChar(string columnName)
    {
        if (columnName.StartsWith("┝"))
            return trimStartChar(columnName.TrimStart('┝'));
        else
            return columnName;
    }

</script>
