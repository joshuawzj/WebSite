using System;
using System.Data;
using System.Linq;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.Domain;
public partial class whir_system_ajax_developer_formsort : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

        var formid1 = RequestUtil.Instance.GetQueryInt("formid1", 0);//主键
        var formid2 = RequestUtil.Instance.GetQueryInt("formid2", 0);//主键


        if (formid1 == 0 || formid2 == 0)//失败
        {
            Response.Write("");
            Response.End();
            return;
        }


        var form = ServiceFactory.FormService.Query<Form>("where formID=@0", formid1).FirstOrDefault();
        if (form == null)
        {
            Response.Write("");
            Response.End();
            return;
        }
        

        try
        {
            var list = DbHelper.CurrentDb.Query("select * from Whir_Dev_Form where ColumnId=@0 AND ModelID=@1 AND FormId!=@2 order by Sort DESC,CreateDate DESC", form.ColumnId, form.ModelId, formid1).Tables[0];
            var i = list.Rows.Count+1;
            if (formid1 == formid2)
            {
                DbHelper.CurrentDb.Execute("update Whir_Dev_Form set sort=@1 where FormId=@0", form.FormId, i--);
            }
            foreach (DataRow item in list.Rows)
            {
                DbHelper.CurrentDb.Execute("update Whir_Dev_Form set sort=@1 where FormId=@0", item["FormId"].ToInt(0), i--);
                if (item["FormId"].ToInt(0) == formid2)
                {
                    DbHelper.CurrentDb.Execute("update Whir_Dev_Form set sort=@1 where FormId=@0", form.FormId, i--);
                }
            }

            Response.Write(formid1);
            Response.End();
        }
        catch (Exception)
        {

            throw;
        }

        
    }
}