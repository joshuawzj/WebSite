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
    this.IsHitCount = "Y";
    this.Scroll = "N";
    this.UploadFilePath = "";
    this.URL = "";
    this.SiteId = 0;
    this.ShowAD = showADContent;
    this.Start = doStart;
}}

var delta = 0.08
var zcmsad_collection = [];

function showADContent() {{
    var content = this.ADContent;
    var str = "";
    var align = "";
    var x = "";
    var y = "";
    var AD = eval('(' + content + ')');
    if (this.ADType == "image") {{
        for (var i = 0; i < AD.Images.length; i++) {{
            str = "";
            align = "";
            if (i % 2 == 0) {{
                x = this.PaddingLeft;
                align = "right";
            }} else {{
                x = "document.documentElement.clientWidth-" + (this.PaddingLeft + this.Width);
                align = "left";
            }}
            y = this.PaddingTop;
            str += "<div id='ZCMSAD_" + this.ADID + "_" + i + "' style='left:" + x + "px;top:" + y + "px;width:" + this.Width + "px; height:" + this.Height + "px; position: absolute;z-index:888888;'>";
            str += "<a href='" + this.URL + "?SiteId=" + this.SiteId + "&ADID=" + this.ADID + "&URL=" + AD.Images[i].ImgADLinkUrl + "' target='" + ((AD.ImgADLinkTarget == "Old") ? "_self" : "_blank") + "'>";
            str += "<img title='" + AD.Images[i].ImgADAlt + "' src='" + this.UploadFilePath + AD.Images[i].ImgPath + "' width='" + this.Width + "' height='" + this.Height + "' onerror=\"this.src='{12}res/images/nofile.jpg'\"  style='border:0px;'>";
            str += "</a>";
            str += "<div style='text-align:" + align + "'><a href='#' class='aclose' onclick='javascript:document.getElementById(\"ZCMSAD_" + this.ADID + "_" + i + "\").style.display=\"none\"'>关闭</a></div>";
            str += "</div>";
            document.write(str);
            addItem("ZCMSAD_" + this.ADID + "_" + i, x, y);
        }}
    }} else if (this.ADType == "flash") {{
    document.write("&nbsp;");
        for (var i = 0; i < AD.Images.length; i++) {{
            str = "";
            align = "";
            if (i % 2 == 0) {{
                x = this.PaddingLeft;
                align = "right";
            }} else {{
                x = "document.documentElement.clientWidth-" + (this.PaddingLeft + this.Width);
                align = "left";
            }}
            y = this.PaddingTop;
            str += "<div id='ZCMSAD_" + this.ADID + "_" + i + "' style='left:" + x + "px;top:" + y + "px;width:" + this.Width + "px; height:" + this.Height + "px; position: absolute;z-index:888888;'>";
            str += "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' width='" + this.Width + "' height='" + this.Height + "' id='FlashAD_" + this.ADID + "'>";
            str += "<param name='movie' value='" + this.UploadFilePath + AD.Images[i].ImgPath + "' />";
            str += "<param name='quality' value='high' />";
            str += "<param name='wmode' value='transparent'/>";
            str += "<param name='swfversion' value='8.0.35.0' />";
            str += "<embed wmode='transparent' src='" + this.UploadFilePath + AD.Images[i].ImgPath + "' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='" + this.Width + "' height='" + this.Height + "'></embed>";
            str += "</object>";
            str += "<div style='text-align:" + align + "'><a href='#;' class='aclose' onclick='javascript:document.getElementById(\"ZCMSAD_" + this.ADID + "_" + i + "\").style.display=\"none\"'>关闭</a></div>";
            str += "</div>";
            document.write(str);
            addItem("ZCMSAD_" + this.ADID + "_" + i, x, y);
        }}
    }}
}}

function addItem(id, x, y) {{

    var newItem = {{}};
    newItem.object = document.getElementById(id);
    newItem.x = x;
    newItem.y = y;

    zcmsad_collection[zcmsad_collection.length] = newItem;
}}

function doStart(Scroll) {{
    var le = 0;
    var he = 0;
    for (var i = 0; i < zcmsad_collection.length; i++) {{
        var followObj_x = (typeof (zcmsad_collection[i].x) == 'string' ? eval(zcmsad_collection[i].x) : zcmsad_collection[i].x);
        var followObj_y = (typeof (zcmsad_collection[i].y) == 'string' ? eval(zcmsad_collection[i].y) : zcmsad_collection[i].y);
        le = le + followObj_x;
        he = he + followObj_y;
    }}
    zcmsad_collection[zcmsad_collection.length-1].object.style.left = le + "px";
    zcmsad_collection[zcmsad_collection.length-1].object.style.top = he + "px";
    setInterval("play(\"" + Scroll + "\")", 10);
}}

function play(Scroll) {{
    for (var i = 0; i < zcmsad_collection.length; i++) {{
        var followObj = zcmsad_collection[i].object;
        var followObj_x = (typeof (zcmsad_collection[i].x) == 'string' ? eval(zcmsad_collection[i].x) : zcmsad_collection[i].x);
        var followObj_y = (typeof (zcmsad_collection[i].y) == 'string' ? eval(zcmsad_collection[i].y) : zcmsad_collection[i].y);
        if (followObj.offsetLeft != (document.documentElement.scrollLeft + followObj_x)) {{
            var dx = (document.documentElement.scrollLeft + followObj_x - followObj.offsetLeft) * delta;
            dx = (dx > 0 ? 1 : -1) * Math.ceil(Math.abs(dx));
            followObj.style.left = (followObj.offsetLeft + dx) + "px";
        }} else {{
            if (Scroll == "N") {{
                continue;
            }}
        }}
        if (followObj.offsetTop != (document.documentElement.scrollTop + followObj_y)) {{
            var dy = (document.documentElement.scrollTop + followObj_y - followObj.offsetTop) * delta;
            dy = (dy > 0 ? 1 : -1) * Math.ceil(Math.abs(dy));
            followObj.style.top = (followObj.offsetTop + dy) + "px";
        }}
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
ezshopAD_{0}.Scroll = '{11}';
ezshopAD_{0}.Width = {6};
ezshopAD_{0}.Height = {7};
ezshopAD_{0}.UploadFilePath = "{8}";
ezshopAD_{0}.ShowAD();
ezshopAD_{0}.Start(ezshopAD_{0}.Scroll);
