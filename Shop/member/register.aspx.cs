/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 * 
 * 创建标识: liuyong 2012-02-07
 * 
 * 修改标识：
 */
using System;
using System.Web.UI;

using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.ezEIP.Web.HttpHandlers;
using System.Globalization;

public partial class Shop_member_register : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltAgreement.Text = WebUser.GetMemberRegisterAgreement();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string msg = "";//提示
        bool isSuccess = false;//是否注册成功
        if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
        {
            msg = "验证码无效";
        }
        else
        {
            string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
            string codeStr = txtCode.Text.Trim();

            if (!checkCode.Equals(codeStr, StringComparison.OrdinalIgnoreCase))
            {
                msg = "验证码不正确";
            }
            else
            {
                string loginName = txtUserName.Text.Trim();
                //判断用户名
                string SQL = "SELECT COUNT(*) FROM Whir_Mem_Member WHERE LoginName=@0 ";
                int count = DbHelper.CurrentDb.ExecuteScalar<int>(SQL, loginName);
                if (count > 0)
                {
                    msg = "用户名[{0}]已被注册".FormatWith(loginName);
                }
                else
                {
                    //ColumnID=1表示会员系统
                    int workFlow = DbHelper.CurrentDb.ExecuteScalar<int>("SELECT WorkFlow From Whir_Dev_Column WHERE ColumnID=@0 and IsDel=0", 1);
                    int state = 0;
                    if (workFlow > 0)//开启工作流
                    {
                        state = 0;//0代表工作流中的第一个节点
                    }
                    else//没有开启工作流
                    {
                        state = -1;//-1代表通过审核
                    }

                    string pwd = TripleDESUtil.Encrypt(txtNewPassWord.Text);
                    string email = txtEmail.Text.Trim();
                    string Sql = "";
                    if (CurrentDbType.CurDbType == EnumType.DbType.Oracle)
                    {
                        Sql = "INSERT INTO WHIR_MEM_MEMBER (WHIR_MEM_MEMBER_PID,LoginName,Password,Email,State,Sort,IsDel,CreateDate,CreateUser,TypeID)VALUES(seq_ezEIP.NEXTVAL,@0,@1,@2,@3,@4,@5,@6,@7,1)";
                    }
                    else
                    {
                        Sql="INSERT INTO WHIR_MEM_MEMBER (LoginName,Password,Email,State,Sort,IsDel,CreateDate,CreateUser,TypeID)VALUES(@0,@1,@2,@3,@4,@5,@6,@7,1)";
                    }

                    DbHelper.CurrentDb.Execute(Sql, loginName, pwd, email, state,Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo)), false, DateTime.Now, loginName);

                    if (workFlow > 0)
                    {
                        int userId = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT  WHIR_MEM_MEMBER_PID FROM WHIR_MEM_MEMBER WHERE LoginName=@0", loginName).ToInt();
                        string emailMessage = ServiceFactory.MemberService.MemberAuthentication(userId);
                        isSuccess = SendEmailHelper.SendEmail(email, "邮箱验证邮件", emailMessage);
                        if (isSuccess)
                        {
                            msg = "恭喜您！注册用户成功，验证邮件已发送到您的邮箱，请注意查收！提示：帐号需要审核通过后才能登录哦！";
                        }
                        else
                        {
                            msg = "恭喜您！注册用户成功。提示：帐号需要审核通过后才能登录哦！";
                        }
                    }
                    else
                    {
                        isSuccess = true;
                        msg = "恭喜您！注册用户成功。";
                    }
                }
            }
        }
        string script = "<script language=\"javascript\" defer=\"defer\">alert('{0}');{1}</script>".FormatWith(msg, isSuccess ? "location.href='login.aspx';" : "$('#" + txtCode.ClientID + "').val('');");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}