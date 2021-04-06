

if (!window.whir) window.whir = {}

var tagTrigger;
var tagValue;
var tagShow;

whir.content = {

    createToColumn: function (openTitle, openUrl, tagTriggerID, tagValueID, tagShowID) {
        tagTrigger = $("#" + tagTriggerID);
        tagValue = $("#" + tagValueID);
        tagShow = $("#" + tagShowID);
       
        tagTrigger.click(function () {
            if (openUrl.indexOf("&callback=getSelectedColumn") < 0) {
                openUrl += "&callback=getSelectedColumn";
            }
            whir.dialog.frame(openTitle, openUrl, null, 800, 500);
            $("#" + tagShowID).show();
        });
    }
};

//选择栏目的返回值
function getSelectedColumn(json) {
    json = eval("(" + json + ")");
    var columnId = "";
    var columnHtml = "";
    $(json).each(function (idx, item) {
        var id = item.id;
        var name = item.name;
        var subjectID = item.subjectID;

        columnId += id + ",";
        var cId = id.split('|')[0];
        var sId = id.split('|')[1];
        columnHtml += '<em id="emColumn' + cId + '_' + sId + '" class="btn btn-white aPart">';
        columnHtml += '<b>' + name + '[' + cId + '][' + sId + ']</b>';
        columnHtml += '<a class="close" onclick="removeColumn(' + cId + ',' + sId + ');"></a>';
        columnHtml += '</em>';
    });
    if (columnId.length > 0) 
        columnId = columnId.substring(0, columnId.length - 1);

    tagValue.val(columnId);
    if (tagShow.attr("id") == "spanColumn") {
        tagShow.css("display", "block");
    }
    tagShow.html(columnHtml);
    $('#tagTrigger').tooltip("show");
    whir.dialog.remove();
}
//移除选中的栏目
function removeColumn(id, subjectId) {
    
    $('#emColumn' + id + '_' + subjectId).remove();

    var ids = tagValue.val();
    ids = ids.split(',');

    var idAfterRemove = "";
    $(ids).each(function (idx, item) {
        var thisID = item.split('|')[0];
        var thisSubjectID = item.split('|')[1];
        if (thisID == id && thisSubjectID == subjectId) {
            ;
        } else {
            idAfterRemove += thisID + "|" + thisSubjectID + ",";
        }
    });
    if (idAfterRemove.length > 0)
        idAfterRemove = idAfterRemove.substring(0, idAfterRemove.length - 1);
    tagValue.val(idAfterRemove);
}

