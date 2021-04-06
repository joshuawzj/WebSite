var styleStr = "<style>";
    styleStr += "a, body, select, td, b";
    styleStr += "{{";
    styleStr += "font-size: 12px;";
    styleStr += "text-decoration: none;";
    styleStr += "}}";
    styleStr += " a, pre";
    styleStr += "{{";
    styleStr += "color: #808080;";
    styleStr += "}}";
    styleStr += "a:link, a:visited";
    styleStr += "{{";
    styleStr += "   color: #555;";
    styleStr += "}}";
    styleStr += "a:hover, a:active";
    styleStr += "{{";
    styleStr += "    color: #ff4e00;";
    styleStr += "}}";
    styleStr += "img";
    styleStr += "{{";
    styleStr += "    border: 0;";
    styleStr += "}}";

    styleStr += "#WhirBox";
    styleStr += "{{";
    styleStr += "    position: relative;";
    styleStr += "}}";
    styleStr += "#WhirNumID";
    styleStr += "{{";
    styleStr += " position: absolute;";
    styleStr += " bottom: 5px;";
    styleStr += " right: 5px;";
    styleStr += "}}";
    styleStr += "#WhirNumID li";
    styleStr += " {{";
    styleStr += " list-style: none;";
    styleStr += " float: left;";
    styleStr += " width: 18px;";
    styleStr += "  height: 16px;";
    styleStr += "  filter: alpha(opacity=80);";
    styleStr += "  opacity: 0.8;";
    styleStr += "  border: 1px solid #D00000;";
    styleStr += "  background-color: #FFFFFF;";
    styleStr += "  color: #D00000;";
    styleStr += "  text-align: center;";
    styleStr += "  cursor: pointer;";
    styleStr += "  margin-right: 4px;";
    styleStr += "  padding-top: 2px;";
    styleStr += "  overflow: hidden;";
    styleStr += " }}";
    styleStr += " #WhirNumID li:hover, #WhirNumID li.active";
    styleStr += "{{";
    styleStr += "  border: 1px solid #D00000;";
    styleStr += " background-color: #FF0000;";
    styleStr += " color: #FFFFFF;";
    styleStr += " width: 22px;";
    styleStr += "  height: 18px;";
    styleStr += " font-weight: bold;";
    styleStr += " font-size: 13px;";
    styleStr += " }}";
    styleStr += " #WhirContentID li";
    styleStr += " {{";
    styleStr += "     position: relative;";
    styleStr += " }}";
    styleStr += " .mask";
    styleStr += " {{";
    styleStr += "     filter: alpha(opacity=40);";
    styleStr += "     opacity: 0.4;";
    styleStr += "    width: 100%;";
    styleStr += "    height: 35px;";
    styleStr += "    background-color: #000000;";
    styleStr += "    position: absolute;";
    styleStr += "    bottom: 0;";
    styleStr += "   left: 0;";
    styleStr += "    display: block;";
    styleStr += " }}";
    styleStr += " .comt";
    styleStr += " {{";
    styleStr += "    position: absolute;";
    styleStr += "   left: 0;";
    styleStr += "   bottom: 5px;";
    styleStr += "  font-size: 16px;";
    styleStr += "  color: #ffffff;";
    styleStr += "  font-weight: bold;";
    styleStr += "  text-indent: 10px;";
    styleStr += "  text-align: left;";
    styleStr += " }}";
    styleStr += "</style>";
    document.write(styleStr);
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
    this.URL = "";
    this.SiteId = 0;
    this.ShowAD = showADContent;
}}
function showADContent() {{
    var content = this.ADContent;
    var AD = eval('(' + content + ')');
    var str = "";
    str += '<div style="border: 1px solid rgb(0, 0, 0); width: ' + this.Width + 'px; height: ' + this.Height + 'px; overflow: hidden;clear: both; position: relative; background-color: rgb(255, 255, 255);">';
    str += "<div id='WhirBox'>";
    str += "<ul id='WhirContentID'>";
    for (var i = 0; i < AD.Images.length; i++) {{
        str += '<li><a href="' + this.URL + '?SiteId='+ this.SiteId + '&ADID=' + this.ADID + '&URL=' + AD.Images[i].ImgADLinkUrl + '" target="' + ((AD.ImgADLinkTarget == 'Old') ? '_self' : '_blank') +'">';
        str += "<img src='" + this.UploadFilePath + AD.Images[i].ImgPath + "' width='" + this.Width + "' height='" + this.Height + "' onerror=\"this.src='{10}res/images/nofile.jpg'\"/>";
        str += "</a><div class='mask'></div>";
        str += "<div class='comt'>" + AD.Images[i].ImgADAlt + "</div>";
        str += "</li>";
    }}
    str += "</ul>";
    str += "</div>";
    str += "<ul id='WhirNumID'>";
    for (var i = 0; i < AD.Images.length; i++) {{
        str += "<li>";
        str += i + 1;
        str += "</li>";
    }}
    str += "</ul>";
    str += "</div>";
    
    document.write(str);

}}

var ezshopAD_{0} = new EzShopAD('ezshopAD_{0}');
ezshopAD_{0}.ADID = {0};
ezshopAD_{0}.ADType = "{1}";
ezshopAD_{0}.ADName = "{2}";
ezshopAD_{0}.ADContent = "{{'Images':[{3}],'ImgADLinkTarget':'{4}','Count':'{5}','showAlt':'Y'}}";
ezshopAD_{0}.URL = "ADClick.aspx";
ezshopAD_{0}.SiteId = 206;
ezshopAD_{0}.Width = {6};
ezshopAD_{0}.Height = {7};
ezshopAD_{0}.UploadFilePath = "{8}";
ezshopAD_{0}.AppPath = "{9}";
ezshopAD_{0}.ShowAD();

new Marquee({{ MSClassID: "WhirBox", ContentID: "WhirContentID", TabID: "WhirNumID",
    Direction: 2, Step: 0.5, Width: {6}, Height: {7}, Timer: 20, DelayTime: 3000, WaitTime:
0, ScrollStep: {6}, SwitchType: 0, AutoStart: 1
}}) 