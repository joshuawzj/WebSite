/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：EWebEditorUpload.aspx.cs
 * 文件描述：EWebEditor编辑器上传文件要管理员登录后才能浏览已上传文件
 */
using System;
using Whir.ezEIP.Web;

namespace EWebEditor
{
    public class EWebEditorBrowse : eWebEditorServer.browse_aspx
    {
        /// <summary>
        /// 管理页面登录验证.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //要管理员登录后才能浏览
            if (!SysManagePageBase.IsLoginUser)
            {
                Response.Write("非法上传文件！请登录后台使用上传功能！");
                Response.End(); 
            }
        }
    }
}
