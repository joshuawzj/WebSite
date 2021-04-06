using System;
using System.Data;
using System.Linq;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.Domain;
public partial class whir_system_ajax_developer_attSort : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

        var itemid1 = RequestUtil.Instance.GetQueryInt("itemid1", 0);//主键
        var itemid2 = RequestUtil.Instance.GetQueryInt("itemid2", 0);//主键


        if (itemid1 == 0 || itemid2 == 0)//失败
        {
            Response.Write("");
            Response.End();
            return;
        }


        var attached = ServiceFactory.AttachedService.Query<Attached>("where AttachedID=@0", itemid1).FirstOrDefault();
        if (attached == null)
        {
            Response.Write("");
            Response.End();
            return;
        }
        

        try
        {
            var list = DbHelper.CurrentDb.Query("select * from Whir_Cnt_Attached order by Sort DESC,CreateDate DESC").Tables[0];
            var i = list.Rows.Count+1;
            if (itemid1 == itemid2)
            {
                DbHelper.CurrentDb.Execute("update Whir_Cnt_Attached set sort=@1 where AttachedID=@0", attached.AttachedId, i--);
            }
            foreach (DataRow item in list.Rows)
            {
                DbHelper.CurrentDb.Execute("update Whir_Cnt_Attached set sort=@1 where AttachedID=@0", item["AttachedID"].ToInt(0), i--);
                if (item["AttachedID"].ToInt(0) == itemid2)
                {
                    DbHelper.CurrentDb.Execute("update Whir_Cnt_Attached set sort=@1 where AttachedID=@0", attached.AttachedId, i--);
                }
            }

            Response.Write(itemid1);
            Response.End();
        }
        catch (Exception)
        {

            throw;
        }

        
    }
}