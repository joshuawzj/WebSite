﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payment_WxPayNotify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NativeNotify nativeNatify = new NativeNotify(this);
        nativeNatify.ProcessNotify();


    }
}