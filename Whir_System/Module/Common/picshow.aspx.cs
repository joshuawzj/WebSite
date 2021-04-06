/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：PicShow.aspx.cs
 * 文件描述：动态显示指定尺寸图片
 */
using System;
using System.Web.UI;
using System.IO;
using System.Drawing;

using Whir.Framework;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class whir_system_module_common_picshow : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //默认是传至根目录uploadfiles
            string fn = "";
            int width = 240;
            int height = 240;
            string color = "";

            fn = RequestUtil.Instance.GetQueryString("fn");

            string _imagepath = Server.MapPath((UploadFilePath + fn).Replace("\\", "/".Replace("//", "/")));


            Response.Clear();

            string originalFileName = _imagepath;

            width = RequestUtil.Instance.GetQueryInt("width", 100);
            height = RequestUtil.Instance.GetQueryInt("height", 100);
            color = RequestUtil.Instance.GetQueryString("color");

            color = color == "" ? "FCFCFC" : color;
            int thumbnailWidth = width;
            int thumbnailHeight = height;
            string bgColorString = color;

            ColorConverter cv = new ColorConverter();
            Color bgColor = (Color)cv.ConvertFromString("#" + bgColorString);

            System.Drawing.Image img = System.Drawing.Image.FromFile(originalFileName);

            System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;

            System.Drawing.Size newSize = this.GetNewSize(thumbnailWidth, thumbnailHeight, img.Width, img.Height);

            System.Drawing.Bitmap outBmp = new Bitmap(thumbnailWidth, thumbnailHeight);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(outBmp);

            //设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.Clear(System.Drawing.Color.FromArgb(255, 249, 255, 240));
            g.Clear(bgColor);
            g.DrawImage(img, new Rectangle((thumbnailWidth - newSize.Width) / 2,
                                            (thumbnailHeight - newSize.Height) / 2,
                                           newSize.Width, newSize.Height),
                         0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();

            if (thisFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                Response.ContentType = "image/gif";
            }
            else
            {
                Response.ContentType = "image/jpeg";
            }


            //设置压缩质量
            System.Drawing.Imaging.EncoderParameters encoderParams = new EncoderParameters();

            System.Drawing.Imaging.EncoderParameter encoderParam =
               new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            encoderParams.Param[0] = encoderParam;


            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;

            for (int fwd = 0; fwd < arrayICI.Length; fwd++)
            {
                if (arrayICI[fwd].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[fwd];
                    break;
                }
            }

            //if (jpegICI == null)
            //{
            //    outBmp.Save(Response.OutputStream, jpegICI, encoderParams);
            //}
            //else
            //{
            //    outBmp.Save(Response.OutputStream, thisFormat);
            //} 

            MemoryStream ms = new MemoryStream();
            outBmp.Save(ms, jpegICI, encoderParams);
            Response.BinaryWrite(ms.ToArray());


        }
    }
    System.Drawing.Size GetNewSize(int maxWidth, int maxHeight, int width, int height)
    {
        Double w = 0.0;
        Double h = 0.0;
        Double sw = Convert.ToDouble(width);
        Double sh = Convert.ToDouble(height);
        Double mw = Convert.ToDouble(maxWidth);
        Double mh = Convert.ToDouble(maxHeight);

        if (sw < mw && sh < mh)
        {
            w = sw;
            h = sh;
        }
        else if ((sw / sh) > (mw / mh))
        {
            w = maxWidth;
            h = (w * sh) / sw;
        }
        else
        {
            h = maxHeight;
            w = (h * sw) / sh;
        }
        return new System.Drawing.Size(Convert.ToInt32(w), Convert.ToInt32(h));
    }
}