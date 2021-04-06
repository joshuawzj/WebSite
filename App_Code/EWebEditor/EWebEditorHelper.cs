
using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

/// <summary>
///EWebEditorHelper 的摘要说明
/// </summary>
public class EWebEditorHelper
{
    public EWebEditorHelper()
    {
    }

    public static object sLicense;

    /// <summary>
    /// 获取内容,如License: 'License = "2:2323:2:2:1::web.gzwhir.com:c4828092ad389b22fcdd568e3dd11a06"
    /// </summary>
    /// <param name="s_Key"></param>
    /// <returns></returns>
    public static string GetConfigString(string s_Key, string s_config)
    {
        string pattern = "'" + s_Key + " = \"(.*)\"";
        MatchCollection matchs = Regex.Matches(s_config, pattern, RegexOptions.IgnoreCase);
        string str3 = "";
        foreach (Match match in matchs)
        {
            str3 = match.Groups[1].Value.ToString();
        }
        return str3;
    }

    /// <summary>
    /// 读取style和toolbar
    /// </summary>
    /// <param name="s_Key"></param>
    /// <returns></returns>
    public static object GetConfigArray(string s_Key, string s_config)
    {
        object[] objArray = null;
        string pattern = "'" + s_Key + " = \"(.*)\"";
        MatchCollection matchs = Regex.Matches(s_config, pattern, RegexOptions.IgnoreCase);
        object obj3 = 0;
        foreach (Match match in matchs)
        {
            obj3 = ObjectType.AddObj(obj3, 1);
            objArray = (object[])Utils.CopyArray((Array)objArray, new object[IntegerType.FromObject(obj3) + 1]);
            objArray[IntegerType.FromObject(obj3)] = match.Groups[1].Value.ToString();
        }
        return objArray;

    }

    /// <summary>
    /// 获取内容,如License: 2:2323:2:2:1::web.gzwhir.com:c4828092ad389b22fcdd568e3dd11a06
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static object InHTML(object str)
    {
        object objectValue = RuntimeHelpers.GetObjectValue(str);
        object obj2 = "";
        if (!Information.IsDBNull(RuntimeHelpers.GetObjectValue(objectValue)))
        {
            obj2 = RuntimeHelpers.GetObjectValue(Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(objectValue), "&", "&amp;", 1, -1, CompareMethod.Binary)), "<", "&lt;", 1, -1, CompareMethod.Binary)), ">", "&gt;", 1, -1, CompareMethod.Binary)), "\"", "&quot;", 1, -1, CompareMethod.Binary));
        }
        return obj2;
    }
    
    /// <summary>
    /// 读取config.aspx文件
    /// </summary>
    /// <param name="s_FileName">配置文件config.aspx的存放路径</param>
    /// <returns></returns>
    public static string ReadFile(object s_FileName)
    {
        object[] args = new object[] { RuntimeHelpers.GetObjectValue(s_FileName) };
        bool[] copyBack = new bool[] { true };
        if (copyBack[0])
        {
            s_FileName = RuntimeHelpers.GetObjectValue(args[0]);
        }
        StreamReader reader = File.OpenText(StringType.FromObject(LateBinding.LateGet(HttpContext.Current.Server, null, "MapPath", args, null, copyBack)));
        string str2 = reader.ReadToEnd();
        reader.Close();
        reader = null;
        return str2;
    }

    /// <summary>
    /// 修改License
    /// </summary>
    /// <param name="strNewLicense">新的License字符串</param>
    /// <param name="s_config"></param>
    /// <param name="s_FileName">配置文件config.aspx的存放路径</param>
    public static void ModifyLicense(string strNewLicense, string s_config, string s_FileName)
    {
        string pattern = "'License = \"(.*)\"";
        string replacement = "'License = \"" + strNewLicense + "\"";
        string str = Regex.Replace(s_config, pattern, replacement, RegexOptions.IgnoreCase);
        WriteConfig(s_FileName, str);
    }

    /// <summary>
    /// 写入config文件
    /// </summary>
    /// <param name="s_FileName"></param>
    /// <param name="s_config"></param>
    public static void WriteConfig(string s_FileName, string s_config)
    {
        File.WriteAllText(s_FileName, s_config);
    }

    /// <summary>
    /// 获取编辑器的Style配置,以数组方式获取,数组中每个项代表一个配置节点
    /// </summary>
    /// <param name="s_FileName">配置文件路径</param>
    /// <param name="styleName">要获取的配置名, 如:light</param>
    /// <returns></returns>
    public static string[] GetStyleConfig(string s_FileName, string styleName)
    {
        string str = ReadFile(s_FileName);
        object[] styleFromConfig = GetConfigArray("Style", str) as object[];//获取所有样式

        string modifyStyleName = styleName;//要修改的样式
        string modifyStyleContent = string.Empty;
        foreach (object obj in styleFromConfig)
        {
            if (obj == null) continue;
            if (obj.ToString().IndexOf(modifyStyleName) == 0)
            {
                modifyStyleContent = obj.ToString();//获取到使用的样式,跳出
                break;
            }
        }

        string[] styleConfig = modifyStyleContent.Split(new string[] { "|||" }, StringSplitOptions.None);
        return styleConfig;
    }

    /// <summary>
    /// 修改样式配置
    /// </summary>
    /// <param name="s_FileName">配置文件路径</param>
    /// <param name="objStyleConfig">新的配置</param>
    public static void ModifyStyle(string s_FileName,string mapPath, object[] objOldStyleConfig ,object[] objNewStyleConfig)
    {
        int i = 1;
        string newConfig = string.Empty;
        foreach (object config in objNewStyleConfig)
        {
            newConfig += config;
            if (i < objNewStyleConfig.Length)
            {
                newConfig += "|||";
                i++;
            }
        }

        i = 1;
        string oldConfig = string.Empty;
        foreach (object config in objOldStyleConfig)
        {
            oldConfig += config;
            if (i < objNewStyleConfig.Length)
            {
                oldConfig += "|||";
                i++;
            }
        }

        string str = ReadFile(s_FileName);
        str = str.Replace(oldConfig, newConfig);

        WriteConfig(mapPath, str);
    }

    #region 使用实例
    ///修改配置:
    ///string s_FileName = "../../../editor/eWebEditor/aspx/config.aspx";
    ///string styleName = "light";
    ///string[] newStyleConfig;
    ///string[] oldStyleConfig;
    ///oldStyleConfig = EWebEditorHelper.GetStyleConfig(s_FileName, styleName);
    ///newStyleConfig = EWebEditorHelper.GetStyleConfig(s_FileName, styleName);
    ///if (rbShuiyinStyle1.Checked && chkIsEnable.Checked)//使用文字水印
    ///{
    ///    newStyleConfig[32] = "1";//文字水印使用状态 - 使用
    ///    newStyleConfig[52] = "0";//图片水印使用状态 - 不使用

    ///    newStyleConfig[33] = txtFontText.Text.Trim();//文字水印的文本内容
    ///    newStyleConfig[36] = ddlFontList.SelectedItem.Text;//文字水印字体            
    ///    newStyleConfig[35] = txtFontSize.Text.Trim();//文字水印的字体大小
    ///    //水印位置
    ///    if (RadioButton1.Checked) newStyleConfig[47] = "1";//左上
    ///    else if (RadioButton2.Checked) newStyleConfig[47] = "4";//中上
    ///    else if (RadioButton3.Checked) newStyleConfig[47] = "7";//右上
    ///    else if (RadioButton4.Checked) newStyleConfig[47] = "2";//左中
    ///    else if (RadioButton5.Checked) newStyleConfig[47] = "5";//中中
    ///    else if (RadioButton6.Checked) newStyleConfig[47] = "8";//右中
    ///    else if (RadioButton7.Checked) newStyleConfig[47] = "3";//左下
    ///    else if (RadioButton8.Checked) newStyleConfig[47] = "6";//中下
    ///    else if (RadioButton9.Checked) newStyleConfig[47] = "9";//右下
    ///}
    ///string mapPath = Server.MapPath("~/editor/eWebEditor/aspx/config.aspx");
    ///EWebEditorHelper.ModifyStyle(s_FileName,mapPath, oldStyleConfig, newStyleConfig);
    ///Alert("修改成功");
    #endregion
}
