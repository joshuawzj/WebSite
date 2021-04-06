using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using Whir.Framework;
using Whir.Repository;
using Whir.Service;

/// <summary>
///WebUser 的摘要说明:“会员管理”的操作类
/// </summary>
public class WebUser
{
    public static int CookiesSaveTime = 2;//cookies保存有效时间，默认为2小时
    public WebUser()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    ///  判断是否已登陆，未登录则跳转到指定地址
    /// </summary>
    /// <param name="redirectUrl">没登录跳转的地址.只需按根目录传路径，如想跳到http://www.XX.com/member/login.aspx,则该参数值传"member/login.aspx"进来即可</param>
    public static void IsLogin(string redirectUrl)
    {

        HttpCookie cookie = CookieUtil.Instance.GetCookie("WHIR_USERINFOR");
        if (cookie == null)
        {
            HttpContext.Current.Response.Write("<script>alert('您尚未登录或登录超时！');window.location.href='" + WebUtil.Instance.AppPath() + redirectUrl + "';</script>");
            HttpContext.Current.Response.End();
        }
    }

    /// <summary>
    /// 判断是否已登陆
    /// </summary>
    /// <returns>返回true/false</returns>
    public static bool IsLogin()
    {
        HttpCookie cookie = CookieUtil.Instance.GetCookie("WHIR_USERINFOR");
        if (cookie == null)
        {
            return false;
        }
        else
        {
            //校验由会员id、会员名称、cookie有效期 md5加密后的字符串

            var cookieCode = CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", "CookieCode");
            var memberId = CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", "Whir_Mem_Member_Pid");
            var loginName = CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", "LoginName");
            var cookiExpires = CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", "CookiExpires");
            if (cookieCode == (memberId + HttpUtility.UrlDecode(loginName) + cookiExpires).ToMD5())
                return true;
            else
                return false;

        }
    }

    /// <summary>
    /// 用户登陆。返回0：用户名或密码错误；1:帐号未审核；2：登录成功；3:回收站
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="saveHours">cookies有效时间，以小时为单位</param>
    /// <param name="useState">是否需要判断是否审核</param>
    /// <returns></returns>
    public static string Login(string userName, string password, int saveHours, bool useState)
    {
        password = TripleDESUtil.Encrypt(password.Trim()); //对密码加密
        DataTable table = DbHelper.CurrentDb.Query("SELECT Whir_Mem_Member_Pid,LoginName,ThisTime,LastTime,IsDel,State,'' CookieCode FROM Whir_Mem_Member WHERE  LoginName=@0 AND Password=@1", userName, password).Tables[0];
        if (table.Rows.Count > 0)
        {
            if (table.Rows[0]["IsDel"].ToBoolean())
            {
                return "3";
            }
            //需要判断审核
            if (useState)
            {
                //-1代表通过审核
                if (table.Rows[0]["State"].ToString() != "-1")
                {
                    return "1";//未审核
                }
            }
            HttpCookie Cookie = new HttpCookie("WHIR_USERINFOR");
            Cookie.Expires = DateTime.Now.AddHours(saveHours);
            foreach (DataColumn dc in table.Columns)
            {
                if (dc.ColumnName.ToLower() == "CookieCode".ToLower())  //由会员id、会员名称、cookie有效期 md5加密后的字符串
                {
                    var cookiExpires = Cookie.Expires.ToFileTime().ToStr();
                    var cookieCode = (table.Rows[0]["Whir_Mem_Member_Pid"].ToStr() + table.Rows[0]["LoginName"].ToStr() + cookiExpires).ToMD5();
                    Cookie.Values.Add(dc.ColumnName.ToString().ToLower(), cookieCode);
                    Cookie.Values.Add("CookiExpires", cookiExpires);
                }
                else                                                        //把所有字段转小写作cookies名
                    Cookie.Values.Add(dc.ColumnName.ToString().ToLower(), HttpContext.Current.Server.UrlEncode(table.Rows[0][dc.ColumnName].ToStr()));
            }

            CookieUtil.Instance.AddCookie(Cookie);

            if (table.Rows[0]["ThisTime"].ToStr() == "")
            {
                int i = DbHelper.CurrentDb.Update<Whir_Mem_Member_1>("set LastTime=@0 where Whir_Mem_Member_PID=@1", DateTime.Now, table.Rows[0]["Whir_Mem_Member_PID"].ToInt());
            }
            else
            {
                int i = DbHelper.CurrentDb.Update<Whir_Mem_Member_1>("set LastTime=@0 where Whir_Mem_Member_PID=@1", table.Rows[0]["ThisTime"].ToDateTime(), table.Rows[0]["Whir_Mem_Member_PID"].ToInt());
            }

            int a = DbHelper.CurrentDb.Update<Whir_Mem_Member_1>("set ThisTime=@0 where Whir_Mem_Member_PID=@1", DateTime.Now, table.Rows[0]["Whir_Mem_Member_PID"].ToInt());

            return "2";//成功登录
        }
        else
        {
            return "0";//用户名或密码错误
        }
    }

    /// <summary>
    /// 根据用户名更新其所有cookies值
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="saveHours">登录的有效时间，小时为单位</param>
    public static void RefreshUserCookies(string userName, int saveHours)
    {
        DataSet ds = DbHelper.CurrentDb.Query("SELECT Whir_Mem_Member_Pid,LoginName,IsDel,State,'' CookieCode FROM Whir_Mem_Member WHERE  LoginName=@0", userName);
        if (ds.Tables[0].Rows.Count > 0)
        {
            HttpCookie Cookie = new HttpCookie("WHIR_USERINFOR");
            Cookie.Expires = DateTime.Now.AddHours(saveHours);

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                if (ds.Tables[0].Rows[0][dc.ColumnName].ToStr() == "CookieCode")  //由会员id、会员名称、cookie有效期 md5加密后的字符串
                { 
                    var cookiExpires = Cookie.Expires.ToFileTime().ToStr();
                    var cookieCode = (ds.Tables[0].Rows[0]["Whir_Mem_Member_Pid"].ToStr() + ds.Tables[0].Rows[0]["LoginName"].ToStr() + cookiExpires).ToMD5();
                    Cookie.Values.Add(dc.ColumnName.ToString().ToLower(), cookieCode);
                    Cookie.Values.Add("CookiExpires", cookiExpires);
                }
                else                                                        //把所有字段转小写作cookies名
                    Cookie.Values.Add(dc.ColumnName.ToString().ToLower(), HttpContext.Current.Server.UrlEncode(ds.Tables[0].Rows[0][dc.ColumnName].ToStr()));
            }
            CookieUtil.Instance.AddCookie(Cookie);
        }
    }

    /// <summary>
    /// 返回登陆用户的某个字段值，不存在则返回空值,不判断是否登录 
    /// </summary>
    /// <param name="fieldName">用户表的字段名</param>
    /// <returns></returns>
    public static string GetUserValue(string fieldName)
    {
        return GetUserValue(fieldName, "");
    }

    /// <summary>
    /// 返回登陆用户的某个字段值，不存在则返回空值，会先判断是否已登录。
    /// </summary>
    /// <param name="fieldName">用户表的字段名</param>
    /// <param name="redirectUrl">没登录跳转的地址.只需按根目录传路径，如想跳到http://www.XX.com/member/login.aspx,则该参数值传"member/login.aspx"进来即可</param>
    /// <returns></returns>
    public static string GetUserValue(string fieldName, string redirectUrl)
    {
        if (redirectUrl != "")
        {
            IsLogin(redirectUrl);
        }
        if (CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", fieldName.ToLower()) != null)
        {
            return HttpContext.Current.Server.UrlDecode(CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", fieldName.ToLower()).ToString());
        }
        else
        {
            return "";
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetUserJson()
    {
        IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
        if (IsLogin())
        {
            int userId = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt();
            DataTable tableMember = DbHelper.CurrentDb.Query("SELECT * FROM Whir_Mem_Member WHERE  Whir_Mem_Member_PID=@0", userId).Tables[0];
            DataTable tableField = DbHelper.CurrentDb.Query(@"SELECT FieldId,FieldName,FieldType FROM WHIR_DEV_FIELD
            WHERE FieldId IN(SELECT FieldId FROM WHIR_DEV_FORM WHERE ColumnId=1)").Tables[0];

            if (tableMember.Rows.Count > 0 && tableField.Rows.Count > 0)
            {
                foreach (DataRow row in tableField.Rows)
                {
                    int fieldId = row[0].ToInt();
                    if (row["FieldType"].ToInt() == (int)FieldType.DateTime)//时间
                    {
                        string dateFormat = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT DATEFORMAT FROM WHIR_DEV_FORMDATE WHERE FormId IN(SELECT formid FROM whir_dev_form WHERE fieldid=@0)", fieldId).ToStr();
                        if (!dateFormat.IsEmpty())
                        {
                            DateTime date = tableMember.Rows[0][row["FieldName"].ToStr()].ToDateTime();
                            if (date > "1900-01-01".ToDateTime())
                            {
                                KeyValuePair<int, string> item = new KeyValuePair<int, string>(
                                fieldId,
                                date.ToString(dateFormat.ToDateTimeFormat())
                                );
                                list.Add(item);
                            }
                        }
                    }
                    else
                    {
                        KeyValuePair<int, string> item = new KeyValuePair<int, string>(
                            fieldId,
                            tableMember.Rows[0][row["FieldName"].ToStr()].ToStr()
                            );
                        list.Add(item);
                    }
                }
            }
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(list);
    }

    /// <summary>
    ///退出当前的登陆
    /// </summary>
    public static void LoginOut()
    {
        CookieUtil.Instance.SetCookie("WHIR_USERINFOR", DateTime.Now.AddDays(-3));
    }

    /// <summary>
    /// 更改当前用户的cookies值,如果未登录则返回false
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="value"></param>
    public static bool UpdateUserCookiesValue(string fieldName, string value)
    {
        if (IsLogin())
        {
            if (CookieUtil.Instance.GetCookieValue("WHIR_USERINFOR", fieldName) != null)
            {
                CookieUtil.Instance.SetCookie("WHIR_USERINFOR", fieldName, value);
            }

            return true;

        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 是否存在用户名
    /// </summary>
    /// <returns></returns>
    public static bool IsExistLoginName(string loginName)
    {
        return IsExist("LoginName", loginName);
    }

    /// <summary>
    /// 根据字段判断是否存在记录，主要用于验证用户名、邮箱地址
    /// </summary>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool IsExist(string fieldName, string value)
    {
        string sql = "SELECT top 1 * FROM Whir_Mem_Member";
        var dt = DbHelper.CurrentDb.Query(sql).Tables[0];
        if (dt != null && dt.Columns.Contains(fieldName))
        {
            sql = "SELECT COUNT(1) FROM Whir_Mem_Member WHERE {0}=@0".FormatWith(fieldName);
            int count = DbHelper.CurrentDb.ExecuteScalar<int>(sql, value);
            return count > 0;
        }
        else
            return false;
    }

    /// <summary>
    /// 更改密码
    /// </summary>
    /// <param name="oldPassword">原密码</param>
    /// <param name="newPassword">新密码</param>
    /// <returns>0:修改密码失败 1:原密码不正确 2:修改密码成功</returns>
    public static string ChangPassword(string oldPassword, string newPassword)
    {
        int userId = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt();//主键
        string state = "0";
        //获取记录
        int record = DbHelper.CurrentDb.ExecuteScalar<int>(
            "SELECT Count(1) From Whir_Mem_Member WHERE Whir_Mem_Member_PID=@0 AND Password=@1",
            userId,
            TripleDESUtil.Encrypt(oldPassword));
        if (record != 1)
        {
            state = "1";
        }
        else
        {
            newPassword = TripleDESUtil.Encrypt(newPassword);//加密
            int count = DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Password=@0 WHERE Whir_Mem_Member_PID=@1", newPassword, userId);
            if (count > 0)//修改密码成功
            {
                state = "2";
                //UpdateUserCookiesValue("password", newPassword);
            }
            else
            {
                state = "0";
            }
        }
        return state;
    }

    /// <summary>
    /// 向会员发送邮箱验证邮件
    /// </summary>
    /// <param name="userId">会员ID</param>
    /// <param name="email">会员邮箱地址</param>
    /// <returns>true,false</returns>
    public static bool SendVerifyEmailToUser(int userId, string email)
    {
        string emailMessage = ServiceFactory.MemberService.MemberAuthentication(userId);
        return SendEmailHelper.SendEmail(email, "邮箱验证邮件", emailMessage);
    }

    /// <summary>
    /// 向会员发送找回密码邮件
    /// </summary>
    /// <param name="userId">会员ID</param>
    /// <param name="email">会员邮箱地址</param>
    /// <returns>turn,false</returns>
    public static bool SendRetakePwd(int userId, string email)
    {
        string emailMessage = ServiceFactory.MemberService.MemberRetakePassword(userId);
        return SendEmailHelper.SendEmail(email, "密码找回邮件", emailMessage);
    }

    /// <summary>
    /// 获取会员注册协议
    /// </summary>
    /// <returns>会员注册协议</returns>
    public static string GetMemberRegisterAgreement()
    {
        return ServiceFactory.MemberService.MemberRegisterContent();
    }
}