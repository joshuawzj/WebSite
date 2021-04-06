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

using Whir.Framework;
using Whir.Repository;
using System.Web;

public partial class Shop_member_emailsuccess : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string loginName = RequestUtil.Instance.GetQueryString("loginname");
        string email = RequestUtil.Instance.GetQueryString("email");
        string randNum = RequestUtil.Instance.GetQueryString("r");
        string msg = "";
        bool isSuccess = false;
        
        int count=DbHelper.CurrentDb.ExecuteScalar<int>("SELECT COUNT(*) FROM Whir_Mem_Member WHERE Email=@0", email);
        if (count <= 0)
        {
            var item = DbHelper.CurrentDb.Query("SELECT * FROM Whir_Mem_Member WHERE LoginName=@0", loginName);
            if (item.Tables[0].Rows.Count > 0)
            {
                if (item.Tables[0].Rows[0]["RandomNum"].ToStr().Equals(randNum))
                {
                    DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Email=@0, RandomNum=@1 WHERE LoginName=@2", email, Rand.Instance.Str(16), loginName);
                    msg = "更改安全邮箱成功！";
                    isSuccess = true;
                }
                else
                {
                    msg = "警告！此为非法验证，验证失败！";
                }
            }
            else
            {
                msg = "验证失败！用户：【" + loginName + "】不存在";
            }
        }
        else
        {
            msg = "更改邮箱失败，原因：邮箱【{0}】已被占用";
        }
        string script = "<script language=\"javascript\" defer=\"defer\">alert('{0}');location.href='loginout.aspx';</script>".FormatWith(
            msg);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}