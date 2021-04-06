/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：PageBase.cs
 * 文件描述：前台用户控件的基类
 */
using System;
using System.Text.RegularExpressions;
using System.Web;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web.HttpModules.Refresh;


namespace Shop.Common
{
    public class PageBase : System.Web.UI.Page
    {

        public static PageBase Insten
        {
            get { return new PageBase(); }
        }
        public PageBase()
        {
            this.PreRender += new EventHandler(RefreshPage_PreRender);//重复提交使用
        }

        #region 公用属性

        /// <summary>
        /// 站点根目录, 如: "/"或"/WebSite/"
        /// </summary>
        public string AppName { get { return WebUtil.Instance.AppPath(); } }

        /// <summary>
        /// 后台路径
        /// </summary>
        public string SysPath { get { return AppName + AppSettingUtil.AppSettings["SystemPath"]; } }

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public string UploadFilePath { get { return AppName + AppSettingUtil.AppSettings["UploadFilePath"]; } }

        /// <summary>
        /// 当前使用的数据库类型
        /// </summary>
        public EnumType.DbType CurrentUseDbType { get { return Whir.Service.CurrentDbType.CurDbType; } }

        public string CurrentPageUrl
        {
            get
            {
                return HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery);
            }
        }

        private string _BackPageUrl;
        /// <summary>
        /// 返回的页面地址, 主要用于内容的编辑页面, 在编辑完之后返回到列表页
        /// </summary>
        public string BackPageUrl
        {
            get
            {
                //有URL参数时, 返回URL参数; 否则返回set的值
                string queryBackPageUrl = RequestUtil.Instance.GetQueryString("BackPageUrl").ToLower();
                if (queryBackPageUrl.IsEmpty())
                    return _BackPageUrl;
                return queryBackPageUrl;
            }
            set { _BackPageUrl = value; }
        }

        /// <summary>
        /// url里有key的值，就替换为value,没有的话就追加.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildUrl(string url, string key, int value)
        {
            Regex reg = new Regex(string.Format("{0}=[^&]*", key), RegexOptions.IgnoreCase);
            Regex reg1 = new Regex("[&]{2,}", RegexOptions.IgnoreCase);
            string _url = reg.Replace(url, "");
            //_url = reg1.Replace(_url, "");
            if (_url.IndexOf("?") == -1)
                _url += string.Format("?{0}={1}", key, value);//?
            else
                _url += string.Format("&{0}={1}", key, value);//&
            _url = reg1.Replace(_url, "&");
            _url = _url.Replace("?&", "?");
            return _url;
        }

        #endregion 公用属性

        #region 页面提示方法


        public void Alert(string message,string redirectUrl)
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alertMessage", string.Format("TipMessage('{0}');location='{1}'", message, redirectUrl), true);
        }

        public void Alert(string message)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alertMessage", string.Format("TipMessage('{0}');", message), true);
        }

        #endregion

        #region 防止重复提交，暂不使用
        // 常量   

        public const string RefreshTicketCounter = "RefreshTicketCounter";

        //标志页面是否按F5进行重复刷新的标志属性   

        public bool IsPageRefresh
        {

            get
            {

                object o = HttpContext.Current.Items[RefreshAction.PageRefreshEntry];

                if (o == null)

                    return false;

                return (bool)o;

            }

        }

        //增加刷新票证的内部计数器   

        public void TrackRefreshState()
        {

            //初始化刷新计数器   

            InitRefreshState();

            //将刷新计数器加1，然后放进Session   

            int ticket = Convert.ToInt32(Session[RefreshTicketCounter]) + 1;

            Session[RefreshTicketCounter] = ticket;

        }

        //初始化刷新计数器   

        private void InitRefreshState()
        {

            if (Session[RefreshTicketCounter] == null)

                Session[RefreshTicketCounter] = 0;

        }

        // PreRender事件处理器   

        private void RefreshPage_PreRender(object sender, EventArgs e)
        {
           
            //在页面呈现之前就保存票证值到隐藏域   

           // SaveRefreshState();   出现内存溢出，暂时取消

        }

        //创建隐藏域来保存当前请求的票证值   

        private void SaveRefreshState()
        {

            //将票证计数器的值加1，然后将此值注册到当前票证隐藏域中   

            int ticket = Convert.ToInt32(Session[RefreshTicketCounter]) + 1;

            this.ClientScript.RegisterHiddenField(RefreshAction.CurrentRefreshTicketEntry, ticket.ToString());

        }

        /// <summary>
        /// 判断是否重复提交了
        /// </summary>
        public void KillRefresh()
        {
            #region 暂不使用
            //if (AppSettingUtil.AppSettings["KillRefresh"]=="1")
            //{
            //    if (this.IsPageRefresh)
            //    {
            //        Response.Redirect(Request.RawUrl);//重复提交了跳转刷新

            //    }
            //    else
            //    {
            //        this.TrackRefreshState();
            //    }
            //} 
            #endregion
        }
        #endregion

    }
}
