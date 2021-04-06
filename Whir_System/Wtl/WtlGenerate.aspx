<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WtlGenerate.aspx.cs" Inherits="whir_system_wtl_WtlGenerate" %>

<%@ Import Namespace="System.IO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="<%=SysPath%>res/css/css_whir.css" rel="stylesheet" />
    <link href="<%=SysPath%>res/css/blue.css" rel="stylesheet" />
    <script type="text/javascript">
        window.globalObj = {
            basePath: "<%=AppName%>"
        }
    </script>


    <script src="<%=AppName%>res/js/jquery-3.3.1.min.js"></script>
    <script src="<%=AppName%>res/js/JSON-js-master/json2.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtl.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtlHelp.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtl.generateHelp.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtl.columnHelp.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtl.fieldHelp.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtl.tabHelp.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtlGlobal.js"></script>
    <script src="<%=AppName%>res/js/wtl/whir.wtlInit.js"></script>

    <style type="text/css">
        body { overflow-x: hidden; }
        .ops_table { display: none; }
        .cover { z-index: -1; }
        #ops_table { display: table; }
    </style>
</head>
<body>
    <div class="frame-view">
        <div class="contain">
            <iframe src="" id="wtl_iframe" scrolling="auto" frameborder="0"></iframe>
            <iframe class="frame-hidden" src="" id="wtl_generate_iframe" scrolling="auto" frameborder="0" style="display: none;"></iframe>
        </div>
        <div class="cover" onclick="closeConfig()"></div>
        <a class="open-template" href="javascript:openTemplate();">&nbsp;</a>
        <a class="open-config" href="javascript:whir.wtlGlobal.openConfig();">&nbsp;</a>
    </div>
    <div class="template-view">
        <div class="contain">
            <div class="tab_common">
                <h5>
                    <a><span class="show"><b>选择页面</b></span></a>
                    <a class="close-template" href="javascript:closeTemplate();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <ul id="template_ul">
                <% string directoryPath = HttpContext.Current.Server.MapPath("/" + AppName + "cn/wtl");  %>
                <% if (Directory.Exists(directoryPath)) %>
                <% { %>
                <% foreach (var fName in Directory.GetFiles(directoryPath)) %>
                <% { %>
                <li class="template" data-url="<%= fName.Replace(directoryPath + "\\","") %>"><%= fName.Replace(directoryPath + "\\","") %></li>
                <% } %>
                <% } %>
            </ul>
        </div>
    </div>
    <div class="config-view" id="div_right">
        <div class="contain" id="default_ops">
            <div id="tab_div" class="tab_common">
                <h5>
                    <a><span class="show"><b>基本配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="ops_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="120px" class="item">栏目：</td>
                            <td>
                                <select id="select_coulumn">
                                    <option value="0">选择栏目</option>
                                    <% foreach (var columnItem in Columns) %>
                                    <%
                                       { %>
                                    <option value="<%= columnItem.ColumnId %>"><%= columnItem.ColumnName %></option>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td width="120px" class="item">是否多栏目共用：</td>
                            <td>
                                <input type="checkbox" id="cbNotBindColumn" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="contain" id="column_ops">
            <div id="tabs_column" class="tab_common">
                <h5>
                    <a><span class="show"><b>一级栏目配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="column_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                    </table>
                </div>
            </div>
        </div>
        <div class="contain" id="pagper_ops">
            <div id="tabs_pagper" class="tab_common">
                <h5>
                    <a><span class="show"><b>基本配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="pagper_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="item">所属列表
                            </td>
                            <td>
                                <select class="wtllist_select"></select>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="contain" id="form_ops">
            <div id="tabs_form" class="tab_common">
                <h5>
                    <a><span class="show"><b>基本配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="form_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="item">表单代码
                            </td>
                            <td>
                                <textarea id="form_code" rows="20" cols="90"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">所属表单
                            </td>
                            <td>
                                <select class="form_select"></select>
                                <a href="javascript:void(0)" class="form_relod">刷新</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="contain" id="location_ops">
            <div id="tabs_location" class="tab_common">
                <h5>
                    <a><span class="show"><b>基本配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="location_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="item">栏目
                            </td>
                            <td>
                                <select name="location_column">
                                    <option value="0">选择栏目</option>
                                    <% foreach (var columnItem in Columns) %>
                                    <%
                                       { %>
                                    <option value="<%= columnItem.ColumnId %>"><%= columnItem.ColumnName %></option>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">是否开启栏目别名
                            </td>
                            <td>
                                <input type="radio" name="location_IsShowColumnNameStage" value="1" />是
                                <input type="radio" name="location_IsShowColumnNameStage" value="0" checked="checked" />否
                            </td>
                        </tr>
                        <tr>
                            <td class="item">首页名字
                            </td>
                            <td>
                                <input type="text" name="location_DefaultValue" value="首页" />
                            </td>
                        </tr>
                        <tr>
                            <td class="item">分隔符
                            </td>
                            <td>
                                <input type="text" name="location_Separator" value=">" />
                            </td>
                        </tr>
                        <tr>
                            <td class="item">是否启用显示单独类别
                            </td>
                            <td>
                                <input type="radio" name="location_IsCategory" value="1" />是
                                <input type="radio" name="location_IsCategory" value="0" checked="checked" />否
                            </td>
                        </tr>
                        <tr>
                            <td class="item">类别参数名称
                            </td>
                            <td>
                                <input type="text" name="location_CategoryParam" value="lcid" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="contain" id="survey_ops">
            <div id="tabs_survey" class="tab_common">
                <h5>
                    <a><span class="show"><b>基本配置</b></span></a>
                    <a class="close-config" href="javascript:closeConfig();">&nbsp;</a>
                </h5>
            </div>
            <div class="clear"></div>
            <div>
                <div class="All_table">
                    <table id="survey_table" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="item">栏目
                            </td>
                            <td>
                                <select name="survey_column">
                                    <option value="0">选择栏目</option>
                                    <% foreach (var columnItem in Columns) %>
                                    <%
                                       { %>
                                    <option value="<%= columnItem.ColumnId %>"><%= columnItem.ColumnName %></option>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
        <div class="button_submit_div">
            <a href="javascript:void(0)" class="aLink generate_btn">
                <em>
                    <img src="../res/images/button_submit_icon_6.gif" /></em>
                <b>生成</b>
            </a>
            <a href="javascript:void(0)" class="aLink save_btn">
                <em>
                    <img src="../res/images/button_submit_icon_6.gif" /></em>
                <b>保存</b>
            </a>
        </div>
    </div>
    <script src="../../res/js/template/template.js"></script>
    <script src="../../res/js/template/templatehelper.js"></script>
    <script id="columnSon" type="text/html">
        <table class="ops_table" width="100%" border="0" cellpadding="0" cellspacing="0" data-index="{{index}}">
            <tr>
                <td class="item">类型</td>
                <td>
                    <input type="radio" name="columnSonType_{{index}}" class="columnSonType" data-index="{{index}}" value="column" checked="checked" />绑定子栏目
                    <input type="radio" name="columnSonType_{{index}}" class="columnSonType" data-index="{{index}}" value="other" />其他
                </td>
            </tr>
            <tr>
                <td class="item">配置</td>
                <td>
                    <div id="columnSon_{{index}}_select" class="columnSon_select">
                        <select data-index="{{index}}"></select>
                    </div>
                    <div id="otherSon_{{index}}_input" class="otherSon_input" style="display: none;">
                        <div array-index="0">
                            链接：<input data-index="{{index}}" type="text" name="url" /><br />
                            文本：<input data-index="{{index}}" type="text" name="text" />&nbsp;
                            <a href="javascript:void(0)" class="otherSon_add_a">新增</a>&nbsp;<a href="javascript:void(0)" class="otherSon_del_a" style="display: none;">删除</a>
                        </div>
                    </div>
                </td>
            </tr>
            {{if isSon == true}}
            <tr>
                <td class="item">子栏目</td>
                <td>
                    <input type="radio" name="sonColumn_{{index}}" class="sonColumn" data-index="{{index}}" value="1" />是
                    <input type="radio" name="sonColumn_{{index}}" class="sonColumn" data-index="{{index}}" value="0" checked="checked" />否
                </td>
            </tr>
            {{/if}}
        </table>
    </script>
    <script id="columnTr" type="text/html">
        <tr>
            <td width="120px" class="item" rowspan='{{if isSon == true}}3{{else}}2{{/if}}' oldrowspan="{{if isSon == true}}3{{else}}2{{/if}}" data-index="{{index}}">{{indexName}}：</td>
            <td class="item">类型</td>
            <td>
                <input type="radio" name="columnType_{{index}}" class="columnType" data-index="{{index}}" value="column" checked="checked" />绑定栏目
                <input type="radio" name="columnType_{{index}}" class="columnType" data-index="{{index}}" value="index" />首页
                <input type="radio" name="columnType_{{index}}" class="columnType" data-index="{{index}}" value="other" />其他
            </td>
        </tr>
        <tr ops_tr_index="{{index}}">
            <td class="item">配置</td>
            <td>
                <div id="column_{{index}}_select" class="column_select">
                    <select data-index="{{index}}"></select><span class="tips">默认为当前栏目，需要链接的栏目，将会出错</span>
                </div>
                <div id="other_{{index}}_input" class="other_input" style="display: none;">
                    链接：<input data-index="{{index}}" type="text" name="url" /><br />
                    文本：<input data-index="{{index}}" type="text" name="text" />
                </div>
            </td>
        </tr>
        {{if isSon == true}}
        <tr>
            <td class="item">子栏目</td>
            <td>
                <input type="radio" name="sonColumn_{{index}}" class="sonColumn" data-index="{{index}}" value="1" />是
                <input type="radio" name="sonColumn_{{index}}" class="sonColumn" data-index="{{index}}" value="0" checked="checked" />否
            </td>
        </tr>
        {{/if}}
    </script>
    <script id="opsTable" type="text/html">
        <table class="ops_table" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="item">页面可配置字段</td>
                <td class="item">操作说明</td>
                <td>操作</td>
            </tr>
            {{each list as value}}
            {{if value.type == "图片链接"}}
                <tr>
                    <td class="item">{{value.name}}</td>
                    <td class="item">选择字段：
                    </td>
                    <td>
                        <select class="field_select" name="{{value.id}}"></select>
                    </td>
                </tr>
            {{else if value.type == "A链接"}}
                <tr id="radio_a_type_{{value.id}}_tr">
                    <td class="item" rowspan="2">{{value.name}}</td>
                    <td class="item">链接类型：
                    </td>
                    <td>
                        <input type="radio" class="radio_a_type" name="{{value.id}}" value="系统生成" checked="checked" />系统生成
                        <input type="radio" class="radio_a_type" name="{{value.id}}" value="选择字段" />选择字段
                        <input type="radio" class="radio_a_type" name="{{value.id}}" value="固定地址" />固定地址
                        <input type="radio" class="radio_a_type" name="{{value.id}}" value="首页链接" />首页链接
                    </td>
                </tr>
            <tr id="a_{{value.id}}_type_parm_tr">
                <td class="item">其他参数：
                </td>
                <td>
                    <input type="text" class="parm_text" name="{{value.id}}" />
                    <span class="tips">生成url的其他参数，可为空。例：cid=5</span>
                </td>
            </tr>
            <tr id="a_{{value.id}}_type_field_tr" style="display: none;">
                <td class="item">选择字段：
                </td>
                <td>
                    <select class="field_select" name="{{value.id}}"></select>
                </td>
            </tr>

            <tr id="a_{{value.id}}_type_text_tr" style="display: none;">
                <td class="item">输入地址：
                </td>
                <td>
                    <input type="text" class="fixed_url" name="{{value.id}}" />
                </td>
            </tr>
            {{else if value.type == "文本字段"}}
                <tr>
                    <td class="item" rowspan="2">{{value.name}}</td>
                    <td class="item">选择字段：
                    </td>
                    <td>
                        <select class="field_select" name="{{value.id}}"></select>
                    </td>
                </tr>
            <tr>
                <td class="item">截取字符数：
                </td>
                <td>
                    <input type="text" class="substring_number" pattern="\d+" name="{{value.id}}" style="width: 50px;" />
                    <span class="tips">页面字符截取数量，不填为不截取</span>
                </td>
            </tr>
            {{else if value.type == "日期时间"}}
                <tr>
                    <td class="item">{{value.name}}</td>
                    <td class="item">选择字段：
                    </td>
                    <td>
                        <select class="field_select" name="{{value.id}}">
                        </select>
                    </td>
                </tr>
            {{else if value.type == "固定文本"}}
                <tr>
                    <td class="item">{{value.name}}</td>
                    <td class="item">替换内容：
                    </td>
                    <td>
                        <input type="text" class="fixed_text" name="{{value.id}}" value="{{value.defaultVal}}" />
                        <span class="tips">可替换的文本，为空使用默认文本</span>
                    </td>
                </tr>
            {{else if value.type == "点击率"}}
                 <tr>
                     <td class="item">点击率</td>
                     <td class="item">选择字段：
                     </td>
                     <td>
                         <select class="field_select" name="{{value.id}}">
                         </select>
                         <span class="tips">默认内容Hits 字段</span>
                     </td>
                 </tr>
            {{/if}}

            {{/each}}
        </table>
    </script>
    <script type="text/javascript">


        function autoHeight() {
            var winHeight = $(window).height();
            $(".frame-view iframe").height(winHeight - 30);
            $(".frame-view .cover").height(winHeight);
            $(".template-view").height(winHeight);
            $(".config-view").height(winHeight);
        }
        autoHeight();
        window.onresize = function () { autoHeight(); }

        $("#wtl_iframe").WtlGenerate();

        function closeConfig() {
            $(".frame-view .cover").fadeOut();
            $(".template-view").fadeOut();
            $(".config-view").animate({ right: "-50%" });
        }
        function openTemplate() {
            $(".frame-view .cover").fadeIn();
            $(".template-view").fadeIn();
            $(".template-view").animate({ left: "0px" });
        }
        function closeTemplate() {
            $(".frame-view .cover").fadeOut();
            $(".template-view").animate({ left: "-300px" });
        }

    </script>
</body>
</html>
