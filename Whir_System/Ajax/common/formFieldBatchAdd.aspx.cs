using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;

public partial class whir_system_ajax_common_formFieldBatchAdd : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Clear();
            string columnId = HttpUtility.UrlDecode(Request.Form["columnId"]);
            string xml = HttpUtility.UrlDecode(Request.Form["xml"]);
            string result = "";
            var reg = new Regex(@"<(/*[\w]*?)>");
            xml = reg.Replace(xml, OutPutMatch); 
            var doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.LoadXml(xml);
            var nodes = doc.SelectNodes("root/item");
            var list = new List<TempField>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    string name = "";
                    string note = "";
                    string type = "";
                    XmlNode temp = node.SelectSingleNode("name");
                    if (temp != null)
                    {
                        name = temp.InnerText;
                    }

                    temp = node.SelectSingleNode("note");
                    if (temp != null)
                    {
                        note = temp.InnerText;
                    }

                    temp = node.SelectSingleNode("type");
                    if (temp != null)
                    {
                        type = temp.InnerText;
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        var field = new TempField { Name = name.Trim(), Note = note.Trim(), Type = type.ToInt() };
                        list.Add(field);
                    }
                }
            }
            result = BatchAddField(list, columnId.ToInt());

            Response.Write(result);
            Response.End();
        }
    }

    public string BatchAddField(List<TempField> fields, int columnId)
    {
        string msg;
        try
        {
            var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);
            if (column == null)
            {
                msg = ("ColumnId为{0}的栏目不存在".FormatWith(columnId));
                return msg;
            }
            int count = 0;
            foreach (TempField tempField in fields)
            {
                Field field = ModelFactory<Field>.Insten();
                Form form = ModelFactory<Form>.Insten();

                field.ModelId = column.ModelId;
                field.FieldType = tempField.Type;
                field.FieldName = tempField.Name;
                field.FieldAlias = tempField.Note;
                field.IsEnableListShow = 1;

                form.ColumnId = columnId;
                form.ModelId = field.ModelId;
                form.FieldAlias = field.FieldAlias;
                form.IsAllowNull = true;
                form.IsOnly = false;
                form.Width = 0;
                form.Height = 100;

                #region 默认值

                switch (field.FieldType)
                {
                    case 1: //单行文本
                        form.Width = 0;
                        form.Height = 100;
                        form.DefaultValue = string.Empty;
                        break;
                    case 2: //多行文本
                        form.Width = 0;
                        form.Height = 130;
                        form.DefaultValue = string.Empty;
                        break;
                    case 3: //HTML
                        form.Width = 0;
                        form.Height = 350;
                        form.DefaultValue = "";
                        break;
                    case 4:
                        break;
                    case 5: //数字
                        break;
                    case 6: //货币
                        break;
                    case 7: //日期和时间
                        form.DefaultValue = "2"; //当前时间
                        break;
                    case 8:
                        break;
                    case 9: //是/否
                        form.DefaultValue = "0"; //默认"否"
                        break;
                    case 10: //图片
                        form.Width = 100;
                        form.Height = 100;
                        break;
                    case 11: //文件
                        break;
                    case 12:
                        break;
                    case 13: //地区
                        break;
                    case 14: //密码型字段
                        break;
                }

                #endregion

                form.TipText = "";
                form.ValidateText = "";
                form.ValidateType = "";
                form.ValidateExpression = "";
          
                form.IsColor = false;
                form.IsBold = false;
                form.IsLengthCalc = false;

                Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
                if (ServiceFactory.DbService.IsExsitColumn(model.TableName, field.FieldName))
                    continue;

                int formId = ServiceFactory.FormService.AddForm(form, field);
                count = count + (formId > 0 ? 1 : 0);

            }
            msg = (count == fields.Count & count > 0) ? "ok" : "err";
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        return msg;
    }

    private static string OutPutMatch(Match match)
    {
        return match.Value.ToLower();
    }
}

/// <summary>
/// 要批量添加的字段
/// </summary>
public class TempField
{
    public string Name { get; set; }
    public string Note { get; set; }
    public int Type { get; set; }
}