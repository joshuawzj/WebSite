using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;

public partial class member_success : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int userId = RequestUtil.Instance.GetQueryInt("userid", 0);
        string loginName = RequestUtil.Instance.GetQueryString("loginname");

        var item = DbHelper.CurrentDb.Query("SELECT * FROM Whir_Mem_Member WHERE Whir_Mem_Member_PID=@0 AND LoginName=@1", userId, loginName);
        if (item.Tables[0].Rows.Count > 0)
        {
            DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET AccountState=1 WHERE Whir_Mem_Member_PID=@0", userId);
            ltUserName.Text = "验证成功！欢迎您：" + loginName;
        }
        else
        {
            ltUserName.Text = "验证失败！用户：【" + loginName + "】不存在";
        }
    }
}