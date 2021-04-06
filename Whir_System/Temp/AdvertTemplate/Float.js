function EzShopAD(PositionID) {{
    this.ID = PositionID;
    this.ADID = 0;
    this.ADType = "";
    this.ADName = "";
    this.ADContent = "";
    this.PaddingLeft = 0;
    this.PaddingTop = 0;
    this.Width = 0;
    this.Height = 0;
    this.IsHitCount = "N";
    this.UploadFilePath = "";

    this.Step = 1;
    this.Delay = 20;
    this.WindowHeight = 0;
    this.WindowWidth = 0;
    this.Yon = 0;
    this.Xon = 0;
    this.Pause = true;
    this.Interval = null;
    this.URL = "";
    this.SiteId = 0;

    this.ShowAD = showADContent;
    this.Start = doStart;
}}

function showADContent() {{
    var content = this.ADContent;
    var str = "<div id='ZCMSAD_" + this.ADID + "' style='left:" + this.PaddingLeft + "px;top:" + this.PaddingTop + "px;width:" + this.Width + "px; height:" + this.Height + "px; position: absolute;visibility: visible;z-index:999999;'>";
    var AD = eval('(' + content + ')');
    if (this.ADType == "image") {{
        str += "<a href='" + this.URL + "?SiteId=" + this.SiteId + "&ADID=" + this.ADID + "&URL=" + AD.Images[0].ImgADLinkUrl + "' target='" + ((AD.ImgADLinkTarget == "Old") ? "_self" : "_blank") + "'>";
        str += "<img title='" + AD.Images[0].ImgADAlt + "' src='" + this.UploadFilePath + AD.Images[0].ImgPath + "' width='" + this.Width + "' height='" + this.Height + "' onerror=\"this.src='{11}res/images/nofile.jpg'\"  style='border:0px;'>";
        str += "</a>";
    }} else if (this.ADType == "flash") {{
        document.write("&nbsp;");//至少要输出一个字，不然IE不显示
       str += "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' width='"+this.Width+"' height='"+this.Height+"' id='FlashAD_"+this.PosID+"' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0'>";
	  str += "<param name='movie' value='"+this.UploadFilePath+AD.Images[0].ImgPath+"' />"; 
      str += "<param name='quality' value='autohigh' />";
      str += "<param name='wmode' value='opaque'/>";
	  str += "<embed wmode='opaque' src='"+this.UploadFilePath+AD.Images[0].ImgPath+"' quality='autohigh' name='flashad' swliveconnect='TRUE' pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width='"+this.Width+"' height='"+this.Height+"'></embed>";
      str += "</object>";
    }}
    str += "<div style='text-align:right;'><a href='#' class='aclose' onclick='javascript:document.getElementById(\"ZCMSAD_" + this.ADID + "\").style.display=\"none\"'>关闭</a></div>";
    str += "</div>";
    document.write(str);
}}

function changePos(float) {{
    float.WindowWidth = document.documentElement.clientWidth;
    float.WindowHeight = document.documentElement.clientHeight;
    document.getElementById("ZCMSAD_" + float.ADID).style.left = (float.PaddingLeft + document.documentElement.scrollLeft) + "px";
    document.getElementById("ZCMSAD_" + float.ADID).style.top = (float.PaddingTop + document.documentElement.scrollTop) + "px";
    if (float.Yon) {{
        float.PaddingTop = float.PaddingTop + float.Step;
    }} else {{
        float.PaddingTop = float.PaddingTop - float.Step;
    }}
    if (float.PaddingTop < 0) {{
        float.Yon = 1;
        float.PaddingTop = 0;
    }}
    if (float.PaddingTop >= (float.WindowHeight - float.Height)) {{
        float.Yon = 0; float.PaddingTop = (float.WindowHeight - float.Height);
    }}
    if (float.Xon) {{
        float.PaddingLeft = float.PaddingLeft + float.Step;
    }} else {{
        float.PaddingLeft = float.PaddingLeft - float.Step;
    }}
    if (float.PaddingLeft < 0) {{
        float.Xon = 1;
        float.PaddingLeft = 0;
    }}
    if (float.PaddingLeft >= (float.WindowWidth - float.Width)) {{
        float.Xon = 0;
        float.PaddingLeft = (float.WindowWidth - float.Width);
    }}
}}

function doStart(float) {{
    return function() {{
        changePos(float);
    }}
}}

function cmsAD_{0}_pause_resume() {{
    if (ezshopAD_{0}.Pause) {{
        clearInterval(ezshopAD_{0}.Interval);
        ezshopAD_{0}.Pause = false;
    }} else {{
        ezshopAD_{0}.Interval = setInterval(ezshopAD_{0}.Start(ezshopAD_{0}), ezshopAD_{0}.Delay);
        ezshopAD_{0}.Pause = true;
    }}
}}

var ezshopAD_{0} = new EzShopAD('ezshopAD_{0}');
ezshopAD_{0}.ADID = {0};
ezshopAD_{0}.ADType = "{1}";
ezshopAD_{0}.ADName = "{2}";
ezshopAD_{0}.ADContent = "{{'Images':[{3}],'ImgADLinkTarget':'{4}','Count':'{5}','showAlt':'Y'}}";
ezshopAD_{0}.URL = "ADClick.aspx";
ezshopAD_{0}.SiteId = 206;
ezshopAD_{0}.PaddingLeft = {9};
ezshopAD_{0}.PaddingTop = {10};
ezshopAD_{0}.Width = {6};
ezshopAD_{0}.Height = {7};
ezshopAD_{0}.UploadFilePath = "{8}";
ezshopAD_{0}.ShowAD();
document.getElementById('ZCMSAD_{0}').visibility = 'visible';
ezshopAD_{0}.Interval = setInterval(ezshopAD_{0}.Start(ezshopAD_{0}), ezshopAD_{0}.Delay);