using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Whir.ezEIP.Web.HttpHandlers
{
    public class CheckCodeHandler2 : IHttpHandler, IRequiresSessionState
    {
        public static string CheckCode_Key = "Whir_ezEIP5_CheckCode_Handler";

        public static int CheckCode_Length = 4;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetNoStore();
            string text = new Random().Next(1000, 9999).ToString();
            context.Session[CheckCodeHandler2.CheckCode_Key] = text;
            this.GenerateCheckCode(text);
        }

        private void GenerateCheckCode(string checkCode)
        {
            int width = checkCode.Length * 20;
            Bitmap bitmap = new Bitmap(width, 38);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Color[] array = new Color[]
            {
                Color.Black,
                Color.Red,
                Color.DarkBlue,
                Color.Green,
                Color.Orange,
                Color.Brown,
                Color.DarkCyan,
                Color.Purple
            };
            string[] array2 = new string[]
            {
                "Verdana",
                "Microsoft Sans Serif",
                "Comic Sans MS",
                "Arial",
                "宋体"
            };
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(new Pen(Color.LightGray, 0f), x, y, 1, 1);
            }
            for (int j = 0; j < checkCode.Length; j++)
            {
                int num = random.Next(7);
                int num2 = random.Next(5);
                Font font = new Font(array2[num2], 22f, FontStyle.Bold);
                Brush brush = new SolidBrush(array[num]);
                int num3 = 4;
                bool flag = (j + 1) % 2 == 0;
                if (flag)
                {
                    num3 = 2;
                }
                graphics.DrawString(checkCode.Substring(j, 1), font, brush, (float)(3 + j * 12), (float)num3);
            }
            graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            graphics.Dispose();
            bitmap.Dispose();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Jpeg";
            HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
            memoryStream.Dispose();
        }

        private string RndNum(int vcodeNum)
        {
            string[] array = "0,1,2,3,4,5,6,7,8,9".Split(new char[]
            {
                ','
            });
            string text = "";
            int num = -1;
            Random random = new Random();
            string result;
            for (int i = 1; i < vcodeNum + 1; i++)
            {
                bool flag = num != -1;
                if (flag)
                {
                    random = new Random(i * num * (int)DateTime.Now.Ticks);
                }
                int num2 = random.Next(array.Length);
                bool flag2 = num != -1 && num == num2;
                if (flag2)
                {
                    result = this.RndNum(vcodeNum);
                    return result;
                }
                num = num2;
                text += array[num2];
            }
            result = text;
            return result;
        }
    }
}
