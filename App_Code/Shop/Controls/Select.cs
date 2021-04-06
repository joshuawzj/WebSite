using Shop.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Whir.Controls.UI.Controls;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Repository;

namespace Shop.Controls
{

    /// <summary>
    /// Select 的摘要说明
    /// </summary>
    public class Select : Control
    {
        public ShopField shopField { get; set; }
        public Select(Column column, Form form, Field field, RegularEnum regular, ShopField shopFieldModel)
            : base(column, form, field, regular)
        {
            shopField = shopFieldModel;
        }

        public override string Render(string val)
        {
            string html = "";
            int i = 0;
            if (Field != null)
            {
                val = val.IsEmpty() ? Field.DefaultValue : val;
                string select = "";
                if (shopField == null) return "";
                string tipText = "";
                if (!Form.TipText.IsEmpty())
                    tipText = "data-toggle=\"tooltip\" data-placement=\"top\" title=\"{0}\" ".FormatWith(Form.TipText.ToLang());

                if (shopField.BindType == 2) //下拉框
                {
                    select = " selected=\"selected\" ";
                    html = string.Format("<select class=\"{0}\" name=\"{1}\" {2} ", ClassName, Field.FieldName, tipText);
                    if (!Form.IsAllowNull)
                    {
                        html += " required=\"true\" ";
                    }

                    html = html.TrimEnd(',');
                    html += "\">";

                    html += "<option value=\"\" {1}>{0}</option > ".FormatWith("==请选择==".ToLang(), select);
                    DataTable dt = DbHelper.CurrentDb.Query(shopField.BindSql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            html += "<option value=\"{0}\" {2}>{1}</option > ".FormatWith(item[shopField.BindValueField], item[shopField.BindTextField],
                                item[shopField.BindValueField].ToStr() == val ? select : "");
                        }
                    }
                    html += "</select>";
                    return html;
                }
                else if (shopField.BindType == 1) //单选
                {
                    #region 单选
                    string[] options = shopField.BindText.Split(',');
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    if (options.Length > 0)
                    {
                        foreach (var item in options)
                        {
                            string[] arr = item.Split('|');
                            dic.Add(arr[1], arr[0]);
                        }
                    }
                    html += "<ul class=\"list\">";
                    foreach (KeyValuePair<string, string> kv in dic)
                    {
                        select = "";
                        if (!Form.IsAllowNull)
                        {
                            if (i == 0)
                            {
                                select = " checked=\"checked\" ";
                            }

                        }
                        if (Form.DefaultValue.IsEmpty() && kv.Value == val)
                        {
                            select = " checked=\"checked\" ";
                        }
                        html +=
                            "<li><input type=\"radio\" id=\"{0}_{1}\" name=\"{0}\" value=\"{1}\" {3} /><label for=\"{0}_{1}\" {4}>{2}</label></li>"
                                .FormatWith(Field.FieldName, kv.Value, kv.Key, select, tipText);
                        i++;
                    }
                    html += "</ul>";
                    return html;

                    #endregion 单选
                }
                else if (shopField.BindType == 4) //多选
                {
                    #region 多选

                    html += "<ul class=\"list\">";

                    IList<ListItem> listResult = new List<ListItem>();

                    string bindSQL = shopField.BindSql;

                    bindSQL = Regex.Replace(bindSQL, "@SubjectID", "0", RegexOptions.IgnoreCase);

                    DataTable dtResult = DbHelper.CurrentDb.Query(bindSQL, null).Tables[0];
                    foreach (DataRow row in dtResult.Rows)
                    {
                        listResult.Add(
                            new ListItem(
                                row[shopField.BindTextField].ToStr(),
                                row[shopField.BindValueField].ToStr()
                            )
                        );
                    }
                     
                    foreach (var item in listResult)
                    {
                        select = "";

                        if (val.Split(',').Contains(item.Value))
                        {
                            select = " checked=\"checked\" ";
                        }
                        html +=
                            "<li><input type=\"checkbox\" id=\"{0}_{1}\" name=\"{0}\" value=\"{1}\" {3} /><label for=\"{0}_{1}\" {4} >{2}</label></li>"
                                .FormatWith(Field.FieldName, item.Value, item.Text, select, tipText);
                    }
                    html += "</ul>";
                    return html;

                    #endregion 多选
                }
            }
            return html;
        }
    }
}