/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：label_BuildAreaJs.cs
 * 文件描述：地区js生成、下载页面
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Whir.Domain;
using Whir.Framework;
using System.Text;
using Whir.Repository;
using System.Web.UI;

public partial class label_BuildAreaJs : Page
{
    StringBuilder sbThree = new StringBuilder();
    bool lang = true;//true：中文，false：英文
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedIndex != 0)//中文
        {
            lang = false;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("var optionText='{0}';\r\n", lang ? "请选择" : "Please select");
        sb.Append("var area_array=[];\r\n");
        sb.Append("var sub_array=[];\r\n");
        sb.Append("var l_arr=[];\r\n");
        sb.Append("var sub_arr = [];\r\n");
        sb.AppendFormat("area_array[0] = \"{0}\";\r\n", lang ? "请选择" : "Please select");

        string area = GetLevelOne();
        string js = sb.ToString() + area + sbThree.ToString();
        //以字符流的形式下载文件
        byte[] bytes = ASCIIEncoding.UTF8.GetBytes(js);
        Response.ContentType = "application/octet-stream";
        //通知浏览器下载文件而不是打开
        Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode("AreaData_min_" + (lang ? "cn.js" : "en.js"), System.Text.Encoding.UTF8));
        if (bytes.Length > 0)
        {
            Response.BinaryWrite(bytes);
        }
        Response.Flush();
        Response.End();
    }

    /// <summary>
    /// 二级
    /// </summary>
    /// <returns></returns>
    private string GetLevelOne()
    {
        //        area_array[12]="天津市";
        //sub_array[12]=[];
        //sub_array[12][0]="请选择";
        StringBuilder sb = new StringBuilder();
        string SQL = "WHERE Pid=0 AND IsDel=0";
        var list = DbHelper.CurrentDb.Query<Area>(SQL);
        IList<Area> listArea = list.ToList();
        for (int i = 1; i <= listArea.Count; i++)
        {
            sb.AppendFormat("area_array[{0}]=\"{1}\";\r\n", listArea[i - 1].Id, lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
            sb.AppendFormat("sub_array[{0}]=[];\r\n", listArea[i - 1].Id);
            sb.AppendFormat("sub_array[{0}][0]=\"{1}\";\r\n", listArea[i - 1].Id, lang ? "请选择" : "Please select");
            string strLevelTwo = GetLeveTwo(listArea[i - 1].Id, listArea[i - 1].Id);
            sb.Append(strLevelTwo);//二级
        }
        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="levelOneIndex"></param>
    /// <returns></returns>
    private string GetLeveTwo(int pid, int levelOneIndex)
    {
        //sub_array[33][3301] = "杭州市";
        //sub_array[33][3302] = "宁波市";
        StringBuilder sb = new StringBuilder();
        string SQL = "WHERE Pid=@0 AND IsDel=0";
        var list = DbHelper.CurrentDb.Query<Area>(SQL, pid);
        IList<Area> listArea = list.ToList();
        for (int i = 1; i <= listArea.Count; i++)
        {
            sb.AppendFormat("sub_array[{0}][{1}] = \"{2}\";\r\n", levelOneIndex, listArea[i - 1].Id, lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
            GetLeveThree(listArea[i - 1].Id, listArea[i - 1].Id, lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
            //sb.AppendFormat("sub_array[{0}][{1}] = \"{2}\";\r\n", levelOneIndex, levelOneIndex.ToString() + i.ToString().PadLeft(2, '0'), lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
            //GetLeveThree(listArea[i - 1].Id, (levelOneIndex.ToString() + i.ToString().PadLeft(2, '0')).ToInt(), lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 三级
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="leveTwoIndex"></param>
    /// <param name="levelTwoName"></param>
    private void GetLeveThree(int pid, int leveTwoIndex, string levelTwoName)
    {
        //l_arr[4401]="广州市";
        //sub_arr[4401]=[];
        //sub_arr[4401][0]="请选择";
        //sub_arr[4401][440103]="荔湾区";
        //sub_arr[4401][440104]="越秀区";
        sbThree.AppendFormat("l_arr[{0}]=\"{1}\";\r\n", leveTwoIndex, levelTwoName);
        sbThree.AppendFormat("sub_arr[{0}]=[];\r\n", leveTwoIndex);
        sbThree.AppendFormat("sub_arr[{0}][0]=\"{1}\";\r\n", leveTwoIndex, lang ? "请选择" : "Please select");
        string SQL = "WHERE Pid=@0 AND IsDel=0";
        var list = DbHelper.CurrentDb.Query<Area>(SQL, pid);
        IList<Area> listArea = list.ToList();
        for (int i = 1; i <= listArea.Count; i++)
        {
            sbThree.AppendFormat("sub_arr[{0}][{1}]=\"{2}\";\r\n", leveTwoIndex,  listArea[i - 1].Id.ToStr(), lang ? listArea[i - 1].Name : listArea[i - 1].EnName);
        }
    }
}