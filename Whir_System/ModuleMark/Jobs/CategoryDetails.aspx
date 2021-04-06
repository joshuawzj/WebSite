<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="categorydetails.aspx.cs" Inherits="whir_system_ModuleMark_jobs_categorydetails" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="box_common" valign="top">
                    <div class="All_table">
                        <whir:DetailsForm ID="detailsForm1" runat="server" FormType="Left"></whir:DetailsForm>
                        <div class="button_submit_div">
                            <a class="aBack" href='javascript:history.back();'><b>
                                <%="返回".ToLang()%></b></a>
                        </div>
                    </div>
                </td>
                <td width="5">
                </td>
                <td class="box_common" valign="top" width="220">
                    <div class="article_txt">
                        <whir:DetailsForm ID="detailsForm2" runat="server" FormType="Right"></whir:DetailsForm>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
