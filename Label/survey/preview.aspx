<%@ Page Language="C#" AutoEventWireup="true" CodeFile="preview.aspx.cs" Inherits="label_survey_preview" %>

<form id="form1" runat="server">
<asp:panel runat="server" id="panel1">
    <div id="tbQuestions">
        <asp:Repeater ID="rptQuestions" runat="server" onitemdatabound="rptQuestions_ItemDataBound">
            <ItemTemplate>
                    <ul>
                        <li>
                        <%# Container.ItemIndex + 1 + "． "%>
                        <%# Eval("Name") %>
                    </li>
                    <li>
                      <whir:AnswerForm ID="Answerform" runat="server">
                                </whir:AnswerForm>
                        <asp:HiddenField ID="hdQuestionType" Value='<%# Eval("QuestionType") %>' runat="server" />
                    </li>
                </ul>
            </ItemTemplate>
        </asp:Repeater>
        <div class="btn_box">
            <input id="btnSave" type="button" value='<%=SubmitText %>' class="btn" onclick="Add();"/>
        </div>
    </div>
</asp:panel>
<asp:literal id="ltlSurvey" runat="server"></asp:literal>
<script type="text/javascript">
    // 验证器显示样式修改(验证对象为多选/单选列表时将其附加至Table单元格内.)
//    $("span[name=whirValidator]").each(function () {
//        if ($(this).prev().attr("nodeName").toLowerCase() == "table") {
//            var tdLength = $(this).prev().find("td").length;
//            $(this).appendTo($($(this).prev().find("td")[tdLength - 1]));
//        }
//    });

    function Add() {
        var ids = "";
        var content = "";
        $(":checkbox:checked").each(function() {
            ids += $(this).val() + ",";
        });
        $(":radio:checked").each(function() {
            ids += $(this).val() + ",";
        });
        var isPass = "1";
        $("#tbQuestions .answer").each(function(index) {
            content += (index + 1) + ".";
            $(this).find("input:checked").each(function() {

                content += $(this).next("label").eq(0).text() + ",";
            });
            content += "|";

            if ($(this).find("input:checked").length <= 0) {
                isPass = "0";
            }
        });
        if (isPass == "0") {
            alert("<%=UnSelectAllTips %>");
            return false;
        }
        $.get("<%=AppName %>label/survey/SubmitSurvey.aspx?dtt="+new Date(), { columnid:<%=SurveyColumnId %>,ids: ids ,topicid:<%=TopicId %>,content: content,dt:new Date()},
         function (data) {
             if (data == "0") {
                 alert("<%=FailedTips %>");
             }
             if (data == "1") {
                 alert("<%=SuccessfulTips %>");
                 //跳转页面
                 if("<%=SuccessUrl %>".length>0)
                 {
                   location.href = "<%=SuccessUrl %>?columnid=<%=SurveyColumnId %>";
                 }
                 else
                 {
                    location.href = location.href;
                 }
             }
             if (data == "2") {
                 alert("<%=IpRepeatTips %>");
             }
         });
    }
</script>
</form>
