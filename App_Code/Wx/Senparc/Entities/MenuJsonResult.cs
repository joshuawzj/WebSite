/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
  
    文件名：WxJsonResult.cs
    文件功能描述：JSON返回结果
    
    
    创建标识：Senparc - 20150319
----------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Entities {

    /// <summary>
    /// JSON返回结果（自定义菜单）
    /// </summary>
    public class MenuJsonResult : WxJsonResult {

        /// <summary>
        /// 普通菜单
        /// </summary>
        public Menu menu { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public class Menu {

            /// <summary>
            /// 按钮集合
            /// </summary>
            public List<Button> button { get; set; }

            /// <summary>
            /// 菜单编号
            /// </summary>
            public string menuid { get; set; }

            /// <summary>
            /// 按钮
            /// </summary>
            public class Button {

                /// <summary>
                /// 事件类型
                /// </summary>
                public string type { get; set; }

                /// <summary>
                /// 菜单名称
                /// </summary>
                public string name { get; set; }

                /// <summary>
                /// 事件参数
                /// </summary>
                public string key { get; set; }

                /// <summary>
                /// 跳转地址
                /// </summary>
                public string url { get; set; }

                /// <summary>
                /// 子菜单
                /// </summary>
                public List<Button> sub_button { get; set; }

                /// <summary>
                /// 媒体编号
                /// </summary>
                public string media_id { get; set; }

                /// <summary>
                /// 自定义标识（只供当前站点使用）
                /// </summary>
                public string user_type { get; set; }

                /// <summary>
                /// 自定义数据（只供当前站点使用）
                /// </summary>
                public object data { get; set; }
            }
        }

        /// <summary>
        /// 个性化菜单
        /// </summary>
        public class Condition {

            /// <summary>
            /// 按钮集合
            /// </summary>
            public List<Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button> button { get; set; }

            /// <summary>
            /// 匹配规则
            /// </summary>
            public object matchrule { get; set; }

            /// <summary>
            /// 菜单编号
            /// </summary>
            public string menuid { get; set; }
        }
    }
}