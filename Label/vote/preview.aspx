<%@ Page Language="C#" AutoEventWireup="true" CodeFile="preview.aspx.cs" Inherits="label_vote_preview" %>

<asp:panel runat="server" id="panel1">
        <form id="form1" runat="server">
            <table width="100%" border="0" cellspacing="0" cellpadding="3" id="tdvote">
                <tr>
                    <td>
                        <asp:Literal ID="ltlVoteTitle" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 20px;">
                        <whir:answerform id="AnswerForm1" runat="server"></whir:answerform>
                    </td>
                </tr>
                <tr runat="server" id="tr_btn">
                    <td>
                    <div class="btn_box">
                        <input id="btnSave" type="button" value='<%=SubmitText %>' class="btn" onclick="AddVote();"/>
                        </div>
                    </td>
                </tr>
            </table>
            </form>
        </asp:panel>
<asp:literal id="ltlTip" runat="server"></asp:literal>
<script>
    var sortOrder = 1;
    $("#tdvote tr").each(function () {
        if ($(this).find("span").not("[name=whirValidator]").length) {
            $(this).find("span").not("[name=whirValidator]")[0].innerText = sortOrder + "． ";
            sortOrder = sortOrder + 1;
        }
    });

    // 验证器显示样式修改(验证对象为多选/单选列表时将其附加至Table单元格内.)
    $("span[name=whirValidator]").each(function () {
        if ($(this).prev().attr("nodeName").toLowerCase() == "table") {
            var tdLength = $(this).prev().find("td").length;
            $(this).appendTo($($(this).prev().find("td")[tdLength - 1]));
        }
    });

    function AddVote() {
        var ids = "";
        var content = "";
        $(":checkbox:checked").each(function() {
            ids += $(this).val() + ",";
        });
        $(":radio:checked").each(function() {
            ids += $(this).val() + ",";
        });
        var isPass = "1";
        $("#tdvote .answer").each(function(index) {
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
        $.get("<%=AppName %>label/vote/submitvote.aspx?dtt="+new Date(), { columnid:<%=ColumnId %>,ids: ids ,topicid:<%=VoteID %>,content: content,dt:new Date()},
         function (data) {
             if (data == "0") {
                 alert("<%=FailedTips %>");
             }
             if (data == "1") {
                 alert("<%=SuccessfulTips %>");
                 //跳转页面
                 if("<%=SuccessUrl %>".length>0)
                 {
                    location.href = "<%=SuccessUrl %>?columnid=<%=ColumnId %>";
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
