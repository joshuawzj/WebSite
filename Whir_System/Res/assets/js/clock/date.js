var monthNames = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
var dayNames = ["周日", "周一", "周二", "周三", "周四", "周五", "周六"];

var newDate = new Date();
newDate.setDate(newDate.getDate());
$('#Date').html(newDate.getFullYear() + "&nbsp;年&nbsp;" + monthNames[newDate.getMonth()] + "&nbsp;月&nbsp;" + newDate.getDate() + "&nbsp;日&nbsp;&nbsp;" + dayNames[newDate.getDay()]);
