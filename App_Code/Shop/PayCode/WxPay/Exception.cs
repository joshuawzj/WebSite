using System;
using System.Collections.Generic;
using System.Web;


public class WxPayException : Exception
{
    public WxPayException(string msg)
        : base(msg)
    {

    }
}
