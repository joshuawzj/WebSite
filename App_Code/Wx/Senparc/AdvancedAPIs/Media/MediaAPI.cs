/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：MediaAPI.cs
    文件功能描述：素材管理接口（原多媒体文件接口）
    
    
    创建标识：Senparc - 20150211
    
    修改标识：Senparc - 20150303
    修改描述：整理接口
 
    修改标识：Senparc - 20150312
    修改描述：开放代理请求超时时间
 
    修改标识：Senparc - 20150321
    修改描述：变更为素材管理接口
 
    修改标识：Senparc - 20150401
    修改描述：上传临时图文消息接口
 
    修改标识：Senparc - 20150407
    修改描述：上传永久视频接口修改
----------------------------------------------------------------*/

/*
    接口详见：http://mp.weixin.qq.com/wiki/index.php?title=%E4%B8%8A%E4%BC%A0%E4%B8%8B%E8%BD%BD%E5%A4%9A%E5%AA%92%E4%BD%93%E6%96%87%E4%BB%B6
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.AdvancedAPIs.Media.MediaJson;

namespace Senparc.Weixin.MP.AdvancedAPIs.Media
{
    /// <summary>
    /// 素材管理接口（原多媒体文件接口）
    /// </summary>
    public static class MediaApi
    {

        /// <summary>
        /// 新增永久视频素材
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="file">文件路径</param>
        /// <param name="title"></param>
        /// <param name="introduction"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static UploadForeverMediaResult UploadForeverVideo(string accessToken, string file, string title, string introduction, int timeOut = 40000)
        {
            //return ApiHandlerWapper.TryCommonApi(accessToken =>
            //{
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}", accessToken);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = file;
            fileDictionary["description"] = string.Format("{{\"title\":\"{0}\", \"introduction\":\"{1}\"}}", title, introduction);

            return Senparc.Weixin.MP.Utilities.HttpUtility.Post.PostFileGetJson<UploadForeverMediaResult>(url, null, fileDictionary, null, timeOut: timeOut);

            //}, accessTokenOrAppId);
        }

    }
}