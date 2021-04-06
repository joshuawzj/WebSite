/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：translater.aspx.cs
* 文件描述：中英文语言翻译类。 
*/

using System;
using System.Globalization;
using System.Text;
using System.Web;
using Whir.Framework;
using Whir.ezEIP.Web;

public partial class whir_system_ajax_common_translater : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Clear();
            string source = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("source"));
            string from = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("from"));
            string to = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("to"));
            bool camelCase = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("camelCase")).ToBoolean();

            var fromLanguage = TranslateLanguage.Chinese;
            var toLanguage = TranslateLanguage.English;

            switch (from.ToLower())
            {
                case "zh-cn":
                    fromLanguage = TranslateLanguage.Chinese;
                    break;
                case "zh-tw":
                    fromLanguage = TranslateLanguage.ChineseTraditional;
                    break;
                case "en":
                    fromLanguage = TranslateLanguage.English;
                    break;
            }
            switch (to.ToLower())
            {
                case "zh-cn":
                    toLanguage = TranslateLanguage.Chinese;
                    break;
                case "zh-tw":
                    toLanguage = TranslateLanguage.ChineseTraditional;
                    break;
                case "en":
                    toLanguage = TranslateLanguage.English;
                    break;
            }
            string result = Translater.Translate(source, fromLanguage, toLanguage, TranslateType.MircsoftBing).Replace(".", "");

            if (camelCase)
            {
                var words = new StringBuilder();
                foreach (string word in result.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    words.Append(CamelCase(word).Trim());
                }
                result = words.ToStr();
            }
            Response.Write(result);

            Response.End();
        }
    }

    public string CamelCase(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            input = input.ToCharArray()[0].ToString(CultureInfo.InvariantCulture).ToUpper() + input.Substring(1);
            return input;
        }
        return string.Empty;
    }
}