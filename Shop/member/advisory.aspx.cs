/*
 * Copyright © 2009-2013 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 * 
 * 创建标识: yangwb 2012-02-19
 * 
 * 修改标识：
 */
using System;
using System.Web;
using System.Web.UI;

using Whir.Framework;
using Whir.Service;
using Whir.Repository;
using Shop.Service;
public partial class Shop_member_advisory : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //WebUser.IsLogin("shop/member/login.aspx");
            bindConsult();// 绑定我的咨询列表
        }
    }
    /// <summary>
    /// 绑定我的咨询列表
    /// </summary>
    private void bindConsult()
    {
        string SQL = @"select c.*,p.ProImg,p.Images,p.ProName,p.ProNO,m.LoginName from Whir_Shop_Consult c
                        inner join Whir_Shop_ProInfo p on c.ProID=p.ProID
                        inner join Whir_Mem_Member m on m.Whir_Mem_Member_PID=c.MemberID
                        where c.MemberID=" + WebUser.GetUserValue("Whir_Mem_Member_PID") + " order by c.ConsultID desc ";

        var list = ShopConsultService.Instance.Page(pager1.PageIndex, pager1.PageSize, SQL);
        pager1.RecordsTotal = list.TotalItems.ToInt();
        rptProList.DataSource = list.Items;
        rptProList.DataBind();
    }
}