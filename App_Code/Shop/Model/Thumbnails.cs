/*
* Copyright(c) 2009-2010 万户网络技术有限公司
* 文 件 名：Thumbnails.cs
* 文件描述：批量上传类
* 
* 创建标识: 
* 
* 修改标识：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///Thumbnails 的摘要说明
/// </summary>
public class Thumbnails
{
    public Thumbnails(string id, byte[] data)
    {
        this.ID = id;
        this.Data = data;
    }

    public Thumbnails(string id, string fileName, string format, byte[] data)
    {
        this.ID = id;
        this.FileName = fileName;
        this.Format = format;
        this.Data = data;
    }


    private string id;
    public string ID
    {
        get
        {
            return this.id;
        }
        set
        {
            this.id = value;
        }
    }

    private byte[] thumbnail_data;
    public byte[] Data
    {
        get
        {
            return this.thumbnail_data;
        }
        set
        {
            this.thumbnail_data = value;
        }
    }


    private string fileName;
    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }

    private string format;
    public string Format
    {
        get
        {
            return format;
        }
        set
        {
            format = value;
        }
    }
}
