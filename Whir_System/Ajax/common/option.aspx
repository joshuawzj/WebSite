<%@ Page Language="C#" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Service" %>

<script type="text/C#" runat="server">
    
    protected void Page_Load(object sender, EventArgs e)
    {

        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

        int pid = RequestUtil.Instance.GetQueryInt("id", 0);
        int formID = RequestUtil.Instance.GetQueryInt("formid", 0);
        int exceptID = RequestUtil.Instance.GetQueryInt("exceptid",0);
        int subjectID = RequestUtil.Instance.GetQueryInt("subjectid", -99999);
                
        Form form = ServiceFactory.FormService.SingleOrDefault<Form>(formID);
        FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formID);

        if (form == null || formOption == null)
        {
            Response.Write("[]");
            Response.End();
        }
        
        string json = "";
        if(formOption.BindType == 3)
            json = ServiceFactory.FormOptionService.GetJsonLevelInSubject(formOption, pid, subjectID);
        else if(formOption.BindType == 4)
            json = ServiceFactory.FormOptionService.GetJsonByParentID(formOption, pid, exceptID, subjectID);


        if (!json.IsEmpty())
        {
            Response.Write(json);
            Response.End();
        }
        else
        {
            Response.Write("[]");
            Response.End();
        }
    }
    
</script>