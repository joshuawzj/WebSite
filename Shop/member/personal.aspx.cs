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
using System.Web.UI.WebControls;
using System.Data;

using Whir.Repository;
using Whir.Framework;
using Whir.Service;
using Whir.Domain;

public partial class Shop_member_personal : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WebUser.IsLogin("shop/member/login.aspx");
            BandInfo();
        }
    }

    /// <summary>
    /// 绑定会员信息
    /// </summary>
    private void BandInfo()
    {
        string SQL = "SELECT * FROM Whir_Mem_Member WHERE Whir_Mem_Member_PID=@0";
        string dd = WebUser.GetUserValue("Whir_Mem_Member_PID");
        DataTable table = DbHelper.CurrentDb.Query(SQL, WebUser.GetUserValue("Whir_Mem_Member_PID")).Tables[0];
        if (table.Rows.Count == 0)
        {
            return;
        }
        DataRow row = table.Rows[0];

        ltLoginName.Text = row["LoginName"].ToStr();
        ltEmail.Text = row["Email"].ToStr();
        txtNickName.Text = row["NickName"].ToStr();
        rblSex.SelectedValue = row["Sex"].ToStr();
        txtBridth.Text = row["BrithDate"].ToDateTime().ToString("yyyy-MM-dd");
        txtTakeName.Text = row["TakeName"].ToStr();

        BandRegionDropDownList(ddlProv, 0);
        ddlCity.Items.Insert(0, new ListItem("==请选择==", ""));
        ddlArea.Items.Insert(0, new ListItem("==请选择==", ""));
        BandRegion(row["TakeRegion"].ToInt());

        txtAddress.Text = row["TakeAddress"].ToStr();
        txtMobile.Text = row["Mobile"].ToStr();
        txtTel.Text = row["TakeTel"].ToStr();
        txtTakeEmail.Text = row["TakeEmail"].ToStr();
        txtPostCode.Text = row["TakePostcode"].ToStr();
    }

    /// <summary>
    /// 绑定地区
    /// </summary>
    /// <param name="id"></param>
    private void BandRegion(int id)
    {
        var level3 = ServiceFactory.AreaService.SingleOrDefault<Area>(id);
        if (level3 != null)
        {
            if (level3.Pid != 0)
            {
                var level2 = ServiceFactory.AreaService.SingleOrDefault<Area>(level3.Pid);
                if (level2 != null)
                {
                    if (level2.Pid != 0)//三级
                    {
                        var level1 = ServiceFactory.AreaService.SingleOrDefault<Area>(level2.Pid);
                        if (level1 != null)
                        {
                            ddlProv.SelectedValue = level1.Id == 0 ? "" : level1.Id.ToStr();
                            ddlProv_SelectedIndexChanged(null, null);
                            ddlCity.SelectedValue = level2.Id == 0 ? "" : level2.Id.ToStr();
                            ddlCity_SelectedIndexChanged(null, null);
                            ddlArea.SelectedValue = level3.Id == 0 ? "" : level3.Id.ToStr();
                        }
                    }
                    else//二级
                    {
                        ddlProv.SelectedValue = level3.Pid == 0 ? "" : level3.Pid.ToStr();
                        ddlProv_SelectedIndexChanged(null, null);
                        ddlCity.SelectedValue = level3.Id == 0 ? "" : level3.Id.ToStr();
                        ddlArea.Visible = false;
                    }
                }
            }
            else//一级
            {
                ddlProv.SelectedValue = id == 0 ? "" : id.ToStr();
                ddlCity.Visible = ddlArea.Visible = false;
            }
        }
    }

    #region 地区事件
    /// <summary>
    /// 绑定地区，公用下拉列表
    /// </summary>
    /// <param name="pid">父ID</param>
    private void BandRegionDropDownList(Whir.Framework.DropDownList ddl, int pid)
    {
        var list = ServiceFactory.AreaService.GetAreaByPid(pid);
        ddl.DataSource = list;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("==请选择==", ""));
        if (list.Count == 0)
        {
            if (ddl.Equals(ddlCity))
            {
                ddlCity.Visible = ddlArea.Visible = false;
            }
            else if (ddl.Equals(ddlArea))
            {
                ddlArea.Visible = false;
            }
        }
        else
        {
            if (ddl.Equals(ddlCity))
            {
                ddlCity.Visible = ddlArea.Visible = true;
                ddlArea.Items.Clear();
                ddlArea.Items.Insert(0, new ListItem("==请选择==", ""));
            }
            else if (ddl.Equals(ddlArea))
            {
                ddlArea.Visible = true;
            }
        }
    }

    protected void ddlProv_SelectedIndexChanged(object sender, EventArgs e)
    {
        int pid = ddlProv.SelectedValue.ToInt();
        if (pid != 0)
        {
            BandRegionDropDownList(ddlCity, pid);
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        int pid = ddlCity.SelectedValue.ToInt();
        if (pid != 0)
        {
            BandRegionDropDownList(ddlArea, pid);
        }
    }

    #endregion 地区事件


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string SQL = "Update Whir_Mem_Member Set NickName=@0,Sex=@1,Brithdate=@2,TakeName=@3,TakeRegion=@4,TakeAddress=@5,Mobile=@6,TakeTel=@7,TakeEmail=@8,TakePostcode=@9 WHERE Whir_Mem_Member_PID=@10";
        string nickName = txtNickName.Text.Trim();
        string sex = rblSex.SelectedValue;
        DateTime brithdate = txtBridth.Text.ToDateTime();
        string takeName = txtTakeName.Text.Trim();
        string takeRegion = "";
        #region 获取收货地区下拉列表值
        if (ddlArea.Visible)
        {
            takeRegion = ddlArea.SelectedValue;
        }
        else if (ddlCity.Visible)
        {
            takeRegion = ddlCity.SelectedValue;
        }
        else
        {
            takeRegion = ddlProv.SelectedValue;
        }
        #endregion 获取收货地区下拉列表值

        string takeAddress = txtAddress.Text.Trim();
        string mobile = txtMobile.Text.Trim();
        string takeTel = txtTel.Text.Trim();
        string takeEmail = txtTakeEmail.Text.Trim();
        string takePostCode = txtPostCode.Text.Trim();

        int id = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt();

        DbHelper.CurrentDb.Execute(SQL, nickName, sex, brithdate, takeName, takeRegion, takeAddress, mobile, takeTel, takeEmail, takePostCode, id);

        string script = "<script language=\"javascript\" defer=\"defer\">alert('修改成功');</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}