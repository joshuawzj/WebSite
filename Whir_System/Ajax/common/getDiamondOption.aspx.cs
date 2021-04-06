/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：getDiamondOption.cs
 * 文件描述：类别选项选择器
 */
using System;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public partial class whir_system_ajax_common_getDiamondOption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        int pid = RequestUtil.Instance.GetQueryInt("id", 0);
        int formid = RequestUtil.Instance.GetQueryInt("formid", 0);
        //int fieldid = RequestUtil.Instance.GetQueryInt("fieldid", 0);
        //int formoptionid = RequestUtil.Instance.GetQueryInt("formoptionid", 0);
        int subjectid = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        int pageIndex = RequestUtil.Instance.GetQueryInt("index", 1);//当前索引
        int pageSize = RequestUtil.Instance.GetQueryInt("pagesize", int.MaxValue);//每页数
        string keyword = RequestUtil.Instance.GetQueryString("keyword");

        IList<ZTreeNode> list = new List<ZTreeNode>();
        FormOption optionModel = ServiceFactory.FormOptionService.GetFormOptionByFormID(formid);
      
        if (optionModel != null)
        {
            switch (optionModel.BindType)
            {
                case 1://文本
                    list = ServiceFactory.FormOptionService.GetZTreeNodeByBindText(optionModel, keyword);
                    break;
                case 2://绑定SQL语句
                    list = ServiceFactory.FormOptionService.GetZTreeNodeByBindSQL(optionModel, subjectid, pageIndex, pageSize, keyword);
                    break;
                case 3://绑定多级类别
                    list = ServiceFactory.FormOptionService.GetZTreeNodeLevelInSubject(optionModel, pid, subjectid, pageIndex, pageSize, keyword);
                    break;
                case 4://指定当前栏目
                    list = ServiceFactory.FormOptionService.GetZTreeNodeByParentID(optionModel, pid, 0, subjectid, pageIndex, pageSize, keyword);
                    break;
            }
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        Response.Write(jss.Serialize(list));
        Response.End();
    }
}