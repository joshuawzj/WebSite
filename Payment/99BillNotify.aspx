<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
 
        string ReturnURL =  PayInterface.Common.Tools.GetWebUrl() + "/payment/99BillReturn.aspx";
        PayInterface._99Bill.GetNotify(ReturnURL);
     
    }
</script>

