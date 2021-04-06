using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class Ajax_AjaxData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = RequestUtil.Instance.GetFormString("action");
        switch (action)
        {
            case "zhuce":
                zhuce();
                break;
            case "sendcode":
                sendcode();
                break;
            case "modify":
                modify();
                break;
            case "sendcodeModify":
                sendcodeModify();
                break;
			case "forget":
                forget();
                break;
			case "modifyemail":
                modifyemail();
                break;
            case "sendcodeForget":
                sendcodeForget();
                break;
        }
    }

    protected void zhuce()
    {
        string weixinnumber = RequestUtil.Instance.GetFormString("weixinnumber");
        string loginname = RequestUtil.Instance.GetFormString("loginname");
        string email = RequestUtil.Instance.GetFormString("email");
        string code = RequestUtil.Instance.GetFormString("code");
        string pwd = RequestUtil.Instance.GetFormString("pwd");
        HttpCookie cookie = Request.Cookies.Get("Codes");
        if (cookie != null)
        {
            byte[] bytes = Convert.FromBase64String(cookie["Code"].ToStr());
            string yanz = Encoding.Default.GetString(bytes);
            if (code != yanz)
            {
                Response.Write("验证码不正确");
                Response.End();
                return;
            }
        }
        else
        {
            Response.Write("请发送验证码！");
            Response.End();
            return;
        }

        int coun = DbHelper.CurrentDb.ExecuteScalar<string>("select count(*) as coun from Whir_Mem_Member where Email=@0", email).ToInt(0);
        if (coun <= 0)
        {
            var member = Whir.Domain.ModelFactory<Whir_Mem_Member_1>.Insten();
            member.Microscopy = weixinnumber;
            member.LoginName = loginname;
            member.Password = TripleDESUtil.Encrypt(pwd);
            member.TypeID = "1";
            member.Email = email;
            member.LastTime = DateTime.Now;
            member.EndTime = DateTime.Now;
            int i = DbHelper.CurrentDb.Insert(member).ToInt();
            if (i > 0)
            {
                cookie.Expires = DateTime.Now.AddDays(-2); //删除cookie的时间
                Response.Write(1);
                Response.End();
            }
            else
            {
                Response.Write("注册失败！");
                Response.End();
            }
        }
        else
        {
            Response.Write("该邮箱已被注册！");
            Response.End();
            return;
        }

    }

    protected void sendcode()
    {
        string email = RequestUtil.Instance.GetFormString("email");
        Random yzm = new Random();
        string Activecode = yzm.Next(100000, 999999).ToString();
        string content = "您的邮箱验证码：" + Activecode;
        bool rel = SendEmailHelper.SendEmail(email, "邮箱验证码", content);
        if (rel)
        {
            HttpCookie cookie = new HttpCookie("Codes");
            byte[] bytes = Encoding.Default.GetBytes(Activecode);
            cookie.Values.Add("Code", Convert.ToBase64String(bytes));
            cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 1, 0, 0));
            Response.Cookies.Add(cookie);
            Response.Write("1");
            Response.End();
        }
        else
        {
            Response.Write("发送失败");
            Response.End();

        }
    }

    protected void sendcodeModify()
    {
        if (WebUser.IsLogin())
        {
            int memberId = new FrontBasePage().GetUserId();
            string email = DbHelper.CurrentDb.ExecuteScalar<string>("select Email from Whir_Mem_Member where Whir_Mem_Member_pid = @0", memberId);
            Random yzm = new Random();
            string Activecode = yzm.Next(100000, 999999).ToString();
            string content = "您的邮箱验证码：" + Activecode;
            bool rel = SendEmailHelper.SendEmail(email, "邮箱验证码", content);
            if (rel)
            {
                HttpCookie cookie = new HttpCookie("Codes");
                byte[] bytes = Encoding.Default.GetBytes(Activecode);
                cookie.Values.Add("Code", Convert.ToBase64String(bytes));
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 1, 0, 0));
                Response.Cookies.Add(cookie);
                Response.Write("1");
                Response.End();
            }
            else
            {
                Response.Write("发送失败");
                Response.End();
            }
        }
        else
        {
            Response.Write("请先登录");
            Response.End();
        }

    }
	
	 protected void modifyemail()
    {
        if (WebUser.IsLogin())
        {
            string email1 = RequestUtil.Instance.GetFormString("email1");
            string email2 = RequestUtil.Instance.GetFormString("email2");
            string email3 = RequestUtil.Instance.GetFormString("email3");
            
            if (email2 != email3)
            {
                Response.Write("确认邮箱和新邮箱不一致！");
                Response.End();
                return;
            }
			
            
          
                int memberId = new FrontBasePage().GetUserId();

                int count = DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Email=@0 WHERE Whir_Mem_Member_PID=@1", email2, memberId);
                if (count > 0)//修改邮箱
                {
                    Response.Write("1");
					Response.End();
                    return;
                }
                else
                {
                    Response.Write("修改失败！");
                    Response.End();
                    return;
                }
          


        }
        else
        {
            Response.Write("请先登录");
            Response.End();
            return;
        }

    }


    protected void modify()
    {
        if (WebUser.IsLogin())
        {
            string code = RequestUtil.Instance.GetFormString("code");
            string pwd = RequestUtil.Instance.GetFormString("pwd");
            string renewpassword = RequestUtil.Instance.GetFormString("renewpassword");
            HttpCookie cookie = Request.Cookies.Get("Codes");
            if (cookie != null)
            {
                byte[] bytes = Convert.FromBase64String(cookie["Code"].ToStr());
                string yanz = Encoding.Default.GetString(bytes);
                if (code != yanz)
                {
                    Response.Write("验证码不正确");
                    Response.End();
                    return;
                }
            }
            else
            {
                Response.Write("请发送验证码！");
                Response.End();
                return;
            }

            if (pwd != renewpassword)
            {
                Response.Write("确认新密码与新密码不一致！");
                Response.End();
                return;
            }
            else
            {
                int memberId = new FrontBasePage().GetUserId();

                int count = DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Password=@0 WHERE Whir_Mem_Member_PID=@1", TripleDESUtil.Encrypt(pwd), memberId);
                if (count > 0)//修改密码成功
                {
                    Response.Write("1");
					Response.End();
                    return;
                }
                else
                {
                    Response.Write("修改失败！");
                    Response.End();
                    return;
                }
            }


        }
        else
        {
            Response.Write("请先登录");
            Response.End();
            return;
        }

    }
	
	
	  protected void sendcodeForget()
    {
			string email = RequestUtil.Instance.GetFormString("email");
            Random yzm = new Random();
            string Activecode = yzm.Next(100000, 999999).ToString();
            string content = "您的邮箱验证码：" + Activecode;
            bool rel = SendEmailHelper.SendEmail(email, "邮箱验证码", content);
            if (rel)
            {
                HttpCookie cookie = new HttpCookie("Codes");
                byte[] bytes = Encoding.Default.GetBytes(Activecode);
                cookie.Values.Add("Code", Convert.ToBase64String(bytes));
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 1, 0, 0));
                Response.Cookies.Add(cookie);
                Response.Write("1");
                Response.End();
            }
            else
            {
                Response.Write("发送失败");
                Response.End();
            }
        

    }
	
	 protected void forget()
    {
			string loginname = RequestUtil.Instance.GetFormString("loginname");
			string email = RequestUtil.Instance.GetFormString("email");
            string code = RequestUtil.Instance.GetFormString("code");
            string pwd = RequestUtil.Instance.GetFormString("pwd");
            string renewpassword = RequestUtil.Instance.GetFormString("renewpassword");
            HttpCookie cookie = Request.Cookies.Get("Codes");
            if (cookie != null)
            {
                byte[] bytes = Convert.FromBase64String(cookie["Code"].ToStr());
                string yanz = Encoding.Default.GetString(bytes);
                if (code != yanz)
                {
                    Response.Write("验证码不正确");
                    Response.End();
                    return;
                }
            }
            else
            {
                Response.Write("请发送验证码！");
                Response.End();
                return;
            }

            if (pwd != renewpassword)
            {
                Response.Write("确认新密码与新密码不一致！");
                Response.End();
                return;
            }
            else
            {
                

                int count = DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Password=@0 WHERE loginname=@1 and email=@2", TripleDESUtil.Encrypt(pwd), loginname,email);
                if (count > 0)//修改密码成功
                {
                    Response.Write("1");
					Response.End();
                    return;
                }
                else
                {
                    Response.Write("修改失败！");
                    Response.End();
                    return;
                }
            }


        

    }
}