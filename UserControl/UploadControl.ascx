<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UploadControl.ascx.cs"
    Inherits="UploadControl" %>

<link rel="stylesheet" type="text/css" href="<%= GetWebUrl("Res/js/webuploader/webuploader.css") %>">

<script src="<%= GetWebUrl("Res/js/webuploader/webuploader.min.js") %>"></script>
<script src="<%= GetWebUrl("Res/js/webuploader/uploadRun.js") %>"></script>

<div id="uploader" class="wu-example">
    <div class="queueList">
        <div class="placeholder">
            <div id="filePicker"></div>
            <p>拖拽文件到这里 …</p>
        </div>
    </div>
    <div class="statusBar" style="display: none;">
        <div class="progress">
            <span class="text">0%</span>
            <span class="percentage"></span>
        </div>
        <div class="info"></div>
        <div class="btns">
            <div id="filePicker2"></div>
            <div class="uploadBtn">开始上传</div>
            <input type="hidden" id="ImageUrl" name="ImageUrl" />
        </div>
    </div>
</div>

<script>
    var opt = {
        fileId: "#uploader",
        filePicker: "#filePicker",
        filePicker2: "#filePicker2",
        fileUrl: "#ImageUrl",
        fileExt: "gif,jpg,jpeg,bmp,png",
        size: 10 * 1024,//10KB 
        //lang:"en",//英文
        isMulti: true,
        isPic: true
    };
    $('#uploader').UploadRun(opt);
</script>
