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

public partial class Whir_System_ModuleMark_Common_Label : System.Web.UI.Page {
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
    /// 页面Html
    /// </summary>
    protected string Html { get; set; }
    #endregion
    protected void Page_Load(object sender, EventArgs e) {
        const string label = "<wtl:{0} {1}>&#10;{2}</wtl:{0}>";
        string strLabel = "";
        string param = "";
        string labelName = "";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=\"form-group row\">");
        sb.Append("    <div class=\"col-md-2  text-right\">{0}：</div>");
        sb.Append("    <div class=\"col-md-10 \">");
        sb.Append("        <textarea class=\"form-control\" rows=\"8\" width=\"100%\">{1}</textarea>");
        sb.Append("    </div>");
        sb.Append("</div>");
        var model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model != null) {
            switch (model.ModuleMark) {
                case "SinglePage_v0.0.01"://单篇置标
                case "SubsiteSinglePage_v0.0.01":
                    labelName = "InforArea";
                    param = " columnid=\"" + ColumnId + "\"";
                    if (SubjectId > 0) {
                        param = " sql=\"" + getSql(model) + "\""; 
                    }
                    strLabel = label.FormatWith(labelName, param, getFields());
                    Html += sb.ToStr().FormatWith("单篇", strLabel);
                    break;
                default:
                    if (ItemId > 0) {//详细页
                        labelName = "InforArea";
                        param = " columnid=\"" + ColumnId + "\" itemid=\"" + ItemId + "\" ";
                        strLabel = label.FormatWith(labelName, param, getFields());
                        Html += sb.ToStr().FormatWith("详细页", strLabel);
                    }
                    else if (model.ModuleMark.IndexOf("_Category_v0.0.01") > -1) {//类别
                        labelName = "Category";
                        Column column = DbHelper.CurrentDb.SingleOrDefault<Column>(ColumnId);
                        if (column != null) {
                            param = " columnid=\"" + column.MarkParentId + "\" ";
                            if (SubjectId > 0) {
                                param += " where=\"SubjectId={0}\" where0=\"{@subjectId,true,*}\"";
                            }
                        }
                        strLabel = label.FormatWith(labelName, param, getFields());
                        Html += sb.ToStr().FormatWith("栏目类别", strLabel);
                    }
                    else {
                        labelName = "list";
                        string fidlds = getFields();
                        //指定栏目-前10条
                        param = " columnid=\"" + ColumnId + "\" count=\"10\"";
                        if (SubjectId > 0) {
                            param += " where=\"SubjectId={0}\" where0=\"{@subjectId,true,*}\"";
                        }
                        strLabel = label.FormatWith(labelName, param, fidlds);
                        Html += sb.ToStr().FormatWith("列表-指定栏目", strLabel);
                        //栏目模板-头条
                        param = " topcount=\"1\" ";
                        if (SubjectId > 0) {
                            param += " where=\"SubjectId={0}\" where0=\"{@subjectId,true,*}\"";
                        }
                        strLabel = label.FormatWith(labelName, param, "<top>&#10;" + fidlds + "</top>&#10;" + fidlds);
                        Html += sb.ToStr().FormatWith("列表-头条", strLabel);
                        //栏目模板-类别
                        param = " categoryid=\"{@lcid,true,*}\" ";
                        if (SubjectId > 0) {
                            param += " where=\"SubjectId={0}\" where0=\"{@subjectId,true,*}\"";
                        }
                        strLabel = label.FormatWith(labelName, param, fidlds);
                        Html += sb.ToStr().FormatWith("列表-类别", strLabel);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 获取字段别名以及字段名称
    /// </summary>
    /// <returns></returns>
    private string getFields() {
        IList<Form> listForm = ServiceFactory.FormService.GetMainListByColumnId(ColumnId);
        string result = "";
        foreach (Form form in listForm) {
            var field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);
            if (field != null) {
                result += form.FieldAlias + "：{$" + field.FieldName + "}&#10;";
            }
        }
        return result;
    }

    //获取置标Sql语句
    private string getSql(Model model) {
        string sql = "";
        Column column = DbHelper.CurrentDb.SingleOrDefault<Column>(ColumnId);
        if (SubjectId > 0) {
            if (column != null && column.WorkFlow > 0) {
                sql = string.Format("SELECT Top 1 State FROM {0} WHERE TypeId={1} AND SubjectId={2} ORDER BY UpdateDate DESC", model.TableName, ColumnId, SubjectId);
                if (DbHelper.CurrentDb.SingleOrDefault<int>(sql).ToInt() == -1) //现在在使用的单篇 审核通过 否则找历史记录最新审核通过的记录
                    sql = string.Format("SELECT Top 1 * FROM {0} WHERE TypeId={1} AND State=-1 AND SubjectId={2} ORDER BY UpdateDate DESC", model.TableName, ColumnId, SubjectId);
                else
                    sql = string.Format("SELECT Top 1 * FROM {0} WHERE TypeId={1} AND State=-1 AND SubjectId={2} ORDER BY UpdateDate DESC", model.TableName + "_Bak", ColumnId, SubjectId);
            }
            else
                sql = string.Format("SELECT Top 1 * FROM {0} WHERE TypeId={1} AND SubjectId={2} ORDER BY Sort DESC", model.TableName, ColumnId, SubjectId);

        }
        return sql;
    }
}