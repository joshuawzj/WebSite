/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：TemplateManage.aspx.cs
 * 文件描述：编辑器模板管理
 *
 */


using System;
using System.IO;
using Whir.Config;
using Whir.Config.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Linq;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Plugin_Template_TemplateManage : Whir.ezEIP.Web.SysManagePageBase
{
    public class TemplateData
    {
        public string Name { get; set; }
        public string Html { get; set; }
    }

    public List<TemplateData> eWebEditorList = new List<TemplateData>();
    public List<TemplateData> kindEditorList = new List<TemplateData>();
    public List<TemplateData> uEditorList = new List<TemplateData>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("381"));
            GetEditorList();
        }
    }

    public void GetEditorList()
    { 
        #region eWebEditor
        string path = AppName + "Editor/ewebeditor/template/template.js";
        if (File.Exists(this.Server.MapPath(path)))
        {
            StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            str = str.Substring(str.IndexOf("config.Template"), str.Length - str.IndexOf("config.Template"));
            str = str.Replace("config.Template =", "").TrimEnd(';').Trim();
            List<Object> objList = ListDataFromJSON(str);
            foreach (var obj in objList)
            {
                object[] subObjList = (object[])obj;
                eWebEditorList.Add(new TemplateData() { Name = subObjList[2].ToString().Trim(), Html = subObjList[1].ToString().Trim() });

            }
            if (eWebEditorList.Count > 0)
            {
                foreach (var item in eWebEditorList)
                {
                    try
                    {
                        path = AppName + "Editor/ewebeditor/template/" + item.Html;
                        reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
                        item.Html = reader.ReadToEnd();
                        reader.Close();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            reader.Close();
            reader.Dispose();
        }
        #endregion

        #region kindEditor
        path = AppName + "Editor/kindEditor/lang/zh_CN.js";
        if (File.Exists(this.Server.MapPath(path)))
        {
            StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            str = str.Substring(str.IndexOf("template.fileList"), str.IndexOf("'zh-cn');") - str.IndexOf("template.fileList") - 1);
            str = str.Replace(" ", "").Replace("template.fileList':", "").Replace("},", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Trim(',').Trim();

            Dictionary<string, string> dicList = TablesDataFromJSON(str);
            foreach (var ss in dicList)
            {
                kindEditorList.Add(new TemplateData() { Name = ss.Value, Html = ss.Key });
            }
            if (kindEditorList.Count > 0)
            {
                foreach (var item in kindEditorList)
                {
                    try
                    {
                        path = AppName + "Editor/kindEditor/plugins/template/html/" + item.Html;
                        reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
                        item.Html = reader.ReadToEnd();
                        reader.Close();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            reader.Close();
            reader.Dispose();
        }
        #endregion

        #region 百度编辑器
        path = AppName + "Editor/ueditor/dialogs/template/config.js";
        if (File.Exists(this.Server.MapPath(path)))
        {
            StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            str = str.Substring(str.IndexOf("var templates"), str.Length - str.IndexOf("var templates"));
            str = str.Replace("var templates =", "").TrimEnd(';').Trim();

            List<Object> objList = ListDataFromJSON(str);
            foreach (var obj in objList)
            {
                Dictionary<string, object> subObjList = (Dictionary<string, object>)obj;
                var data = new TemplateData();
                foreach (var subObj in subObjList)
                {
                    if (subObj.Key == "title")
                        data.Name = subObj.Value.ToString();
                    if (subObj.Key == "html")
                        data.Html = subObj.Value.ToString();

                }
                uEditorList.Add(data);
            }
            reader.Close();
            reader.Dispose();
        }
        #endregion

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsCurrentRoleMenuRes("382"))
        {
            string tempscript = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", tempscript);
            return;
        }
        GetEditorList();
        string txtName = Request["txtName"];
        string editor = Request["editor"];
        string txtHTML = Request["txtHTML"];
        string path = "";
        string str = "";

        #region eWebEditor
        if (editor.Contains("1"))
        {
            if (eWebEditorList.Where(p => p.Name == txtName).Count() == 0)
            {
                eWebEditorList.Add(new TemplateData() { Name = txtName, Html = txtHTML });
            }
            str = "config.Template = [";
            foreach (var data in eWebEditorList)
            {
                if (data.Name == txtName)
                {
                    data.Html = txtHTML;
                    str += "[\"600\", \"" + data.Name + ".html\", \"" + txtName + "\"],";
                    path = AppName + "Editor/ewebeditor/template/" + data.Name + ".html";
                    File.WriteAllText(this.Server.MapPath(path), txtHTML, System.Text.Encoding.UTF8);
                }
                else
                    str += "[\"600\", \"" + data.Name + ".html\" ,\"" + data.Name + "\"],";
            }
            str = str.TrimEnd(',');
            str += "];";

            path = AppName + "Editor/ewebeditor/template/template.js";
            File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);
        }
        #endregion

        #region kindEditor
        if (editor.Contains("2"))
        {
            if (kindEditorList.Where(p => p.Name == txtName).Count() == 0)
            {
                kindEditorList.Add(new TemplateData() { Name = txtName, Html = txtHTML });
            }

            path = AppName + "Editor/kindEditor/lang/zh_CN.js";
            StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
            str = reader.ReadToEnd();
            reader.Close();
            str = str.Substring(0, str.IndexOf("template.fileList") - 1);
            str += "'template.fileList': {";
            foreach (var data in kindEditorList)
            {
                if (data.Name == txtName)
                {
                    data.Html = txtHTML;
                    str += "'" + data.Name + ".html':'" + txtName + "',";
                    path = AppName + "Editor/kindEditor/plugins/template/html/" + data.Name + ".html";
                    File.WriteAllText(this.Server.MapPath(path), txtHTML, System.Text.Encoding.UTF8);
                }
                else
                    str += "'" + data.Name + ".html': '" + data.Name + "',";
            }
            str = str.TrimEnd(',');
            str += "   }}, 'zh-cn');  KindEditor.options.langType = 'zh_CN';";

            path = AppName + "Editor/kindEditor/lang/zh_CN.js";
            File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);

        }
        #endregion

        #region 百度编辑器
        if (editor.Contains("3"))
        {
            txtHTML = txtHTML.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim().Replace("'", "\"");
            if (uEditorList.Where(p => p.Name == txtName).Count() == 0)
            {
                uEditorList.Add(new TemplateData() { Name = txtName, Html = txtHTML });
            }
            path = AppName + "Editor/ueditor/dialogs/template/config.js";
            StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
            str = reader.ReadToEnd();
            reader.Close();
            str = str.Substring(0, str.IndexOf("var templates = ["));
            str += "var templates = [";
            foreach (var data in uEditorList)
            {
                if (data.Name == txtName)
                {
                    data.Html = txtHTML;
                }
                str += "{ 'pre':'pre" + (uEditorList.IndexOf(data) + 1) % 5 + ".png',";
                str += "'title':'" + data.Name + "',";
                str += "'preHtml':'" + data.Html + "',";
                str += "'html':'" + data.Html + "'},";
            }
            str = str.TrimEnd(',');
            str += "];";

            File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);
        }
        #endregion

        string script = "<script language=\"javascript\" > window.parent.whir.toastr.success( '操作成功'); </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "", script);

    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (!IsCurrentRoleMenuRes("383"))
        {
            string tempscript = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", tempscript);
            return;
        }
        GetEditorList();
        string txtName = Request["txtName"];
        string editor = Request["editor"];
        string path = "";
        string str = "";

        #region eWebEditor
        if (editor.Contains("1"))
        {
            if (eWebEditorList.Where(p => p.Name == txtName).Count() > 0)
            {
                eWebEditorList = eWebEditorList.Where(p => p.Name != txtName).ToList();
                str = "config.Template = [";
                foreach (var data in eWebEditorList)
                {
                    if (data.Name == txtName)
                        continue;
                    else
                        str += "[\"600\", \"" + data.Name + ".html\" ,\"" + data.Name + "\"],";
                }
                str = str.TrimEnd(',');
                str += "];";

                path = AppName + "Editor/ewebeditor/template/template.js";
                File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);
            }
        }
        #endregion

        #region kindEditor
        if (editor.Contains("2"))
        {
            if (kindEditorList.Where(p => p.Name == txtName).Count() > 0)
            {
                kindEditorList = kindEditorList.Where(p => p.Name != txtName).ToList();
                path = AppName + "Editor/kindEditor/lang/zh_CN.js";
                StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
                str = reader.ReadToEnd();
                reader.Close();
                str = str.Substring(0, str.IndexOf("template.fileList") - 1);
                str += "'template.fileList': {";
                foreach (var data in kindEditorList)
                {
                    if (data.Name == txtName)
                        continue;
                    else
                        str += "'" + data.Name + ".html': '" + data.Name + "',";
                }
                str = str.TrimEnd(',');
                str += "   }}, 'zh-cn');";

                path = AppName + "Editor/kindEditor/lang/zh_CN.js";
                File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);
            }
        }
        #endregion

        #region 百度编辑器
        if (editor.Contains("3"))
        {
            if (uEditorList.Where(p => p.Name == txtName).Count() > 0)
            {
                uEditorList = uEditorList.Where(p => p.Name != txtName).ToList();
                path = AppName + "Editor/ueditor/dialogs/template/config.js";
                StreamReader reader = new StreamReader(this.Server.MapPath(path), System.Text.Encoding.UTF8);
                str = reader.ReadToEnd();
                reader.Close();
                str = str.Substring(0, str.IndexOf("var templates = ["));
                str += "var templates = [";
                foreach (var data in uEditorList)
                {
                    str += "{ 'pre':'pre" + uEditorList.IndexOf(data) + ".png',";
                    str += "'title':'" + data.Name + "',";
                    str += "'preHtml':'" + data.Html + "',";
                    str += "'html':'" + data.Html + "'},";
                }
                str = str.TrimEnd(',');
                str += "];";

                File.WriteAllText(this.Server.MapPath(path), str, System.Text.Encoding.UTF8);
            }
        }
        #endregion


        string script = "<script language=\"javascript\" > window.parent.whir.toastr.success( '操作成功'); </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }

    public static Dictionary<string, string> TablesDataFromJSON(string jsonText)
    {
        return JSONToObject<Dictionary<string, string>>(jsonText);
    }

    public static List<Object> ListDataFromJSON(string jsonText)
    {
        return JSONToObject<List<Object>>(jsonText);
    }

    /// <summary> 
    /// JSON文本转对象,泛型方法 
    /// </summary> 
    /// <typeparam name="T">类型</typeparam> 
    /// <param name="jsonText">JSON文本</param> 
    /// <returns>指定类型的对象</returns> 
    public static T JSONToObject<T>(string jsonText)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        try
        {
            return jss.Deserialize<T>(jsonText);
        }
        catch (Exception ex)
        {
            throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
        }
    }

}