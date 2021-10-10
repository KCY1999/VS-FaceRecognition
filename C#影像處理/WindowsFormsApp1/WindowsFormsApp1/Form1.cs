using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                bmp = (Bitmap)Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int r, g, b, gray;
            int h, w;

            Bitmap gray1 = new Bitmap(bmp.Width, bmp.Height);

            w = bmp.Width;
            h = bmp.Height;

            for (int i=0;i<w;i++)
            {
                for(int j=0;j<h;j++)
                {
                    r = bmp.GetPixel(i, j).R;
                    g = bmp.GetPixel(i, j).G;
                    b = bmp.GetPixel(i, j).B;
                    gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
                    gray1.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            pictureBox1.Image = gray1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BitmapData bd;
            IntPtr imgPtr;
            int w,h;
            int stride;
            int widthByte;
            int skipByte;
            int[,,] rgbData;

            w = bmp.Width;
            h = bmp.Height;

            bd=bmp.LockBits(new Rectangle(0,0,w,h),ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            imgPtr = bd.Scan0;

            stride = bd.Stride;

            widthByte = w * 3;

            skipByte = stride - widthByte;

            rgbData = new int[w, h, 3];

            unsafe
            {
                byte* p = (byte*)(void*)imgPtr;
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        rgbData[i, j, 2] = p[0];
                        p++;
                        rgbData[i, j, 1] = p[0];
                        p++;
                        rgbData[i, j, 0] = p[0];
                        p++;
                    }
                    p += skipByte;

                }
            }
            unsafe
            {
                byte* p = (byte*)(void*)imgPtr;
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        double gary = (rgbData[i, j, 0] * 0.299 + rgbData[i, j, 1] *
                        0.587 + rgbData[i, j, 2] * 0.114);
                        p[0] = (byte)gary;
                        p++;
                        p[0] = (byte)gary;
                        p++;
                        p[0] = (byte)gary;
                        p++;
                    }
                    p += skipByte;
                }
            }
            bmp.UnlockBits(bd);
            Refresh();
            pictureBox2.Image = bmp;
        }
    }
}
