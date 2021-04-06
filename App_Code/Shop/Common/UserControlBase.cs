/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：UserControlBase.cs
 * 文件描述：前台用户控件的基类
 * 
 * 创建标识: lurong 2012-02-19 09:12 
 * 
 * 修改标识：
 */

 
using Whir.Framework;
using Whir.Service;

namespace Shop.Common
{
    public class UserControlBase : System.Web.UI.UserControl
    {
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

        #endregion 公用属性
    }
}
