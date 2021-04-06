using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 微信用户持久化类
/// </summary>
public class WxUserRepository {


    /// <summary>
    /// 保存微信用户
    /// </summary>
    /// <param name="user">用户信息</param>
    public static void Add(WxUser user) {
        Whir.Repository.DbHelper.CurrentDb.Insert(user);
    }

    /// <summary>
    /// 更新微信用户
    /// </summary>
    /// <param name="user">用户信息</param>
    public static void Update(WxUser user) {
        Whir.Repository.DbHelper.CurrentDb.Update(user);
    }

    /// <summary>
    /// 获取微信用户
    /// </summary>
    /// <param name="openid">用户的openid</param>
    /// <returns></returns>
    public static WxUser Find(string openid) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxUser>("WHERE openid = @0", openid);
    }

    /// <summary>
    /// 删除微信用户
    /// </summary>
    /// <param name="openid">用户的openid</param>
    public static void Remove(string openid) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Users WHERE openid = @0", openid);
    }

    /// <summary>
    /// 获取用户的主键
    /// </summary>
    /// <param name="openid">用户的openid</param>
    /// <returns></returns>
    public static int FindPrimaryKey(string openid) {
        object r = Whir.Repository.DbHelper.CurrentDb.ExecuteScalar<object>("SELECT TOP 1 Whir_Wx_UsersId FROM Whir_Wx_Users WHERE openid = @0", openid);
        return r.ToInt32(0);
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxUser> Page(string appid, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxUser>(page, pageSize, "WHERE appid = @0 ORDER BY subscribe_time DESC", appid);
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="tagid">所属标签</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxUser> Page(string appid,int tagid, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxUser>(page, pageSize, "WHERE appid = @0 AND (tagid_list LIKE '{0},%' OR tagid_list LIKE '%,{0},%' OR tagid_list LIKE '%,{0}' OR tagid_list = '{0}') ORDER BY subscribe_time DESC".FormatWith(tagid), appid);
    }

    /// <summary>
    /// 将微信接口获取的信息映射到数据库实体
    /// </summary>
    /// <param name="result">微信接口返回信息</param>
    /// <returns></returns>
    public static WxUser Map(Senparc.Weixin.MP.Entities.WxUserJsonResult result) {
        return new WxUser() {
            appid = result.appid,
            city = result.city,
            country = result.country,
            groupid = result.groupid,
            headimgurl = result.headimgurl,
            language = result.language,
            nickname = result.nickname,
            openid = result.openid,
            province = result.province,
            remark = result.remark,
            sex = result.sex,
            subscribe = result.subscribe,
            subscribe_time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(result.subscribe_time),
            tagid_list = string.Join(",", result.tagid_list),
            unionid = result.unionid
        };
    }
}