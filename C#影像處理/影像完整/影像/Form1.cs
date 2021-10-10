using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;

using OpenCvSharp;
using OpenCvSharp.Extensions;



namespace 影像
{
    public partial class Form1 : Form
    {
        Bitmap bmp,garybmp;  //原圖的bmp

        Image<Bgr,  byte> colorImage;
        Image<Gray, byte> grayImage;
        Image<Hsv,  byte> hsvImage;

        Image<Gray, byte> thresholdImage;

        Image<Gray, Byte> img_1;
        Image<Gray, Byte> img_2;

        int[] h;
        int[] w;

        int mxh = 360, mih = 0;
        int mxs = 255, mis = 0;
        int mxv = 255, miv = 0;


        int mi1 = 999, mi2 = 999, y1 = 0, y2 = 0;
        int mi3 = 999, mi4 = 999, mi5 = 999, x1 = 0, x2 = 0, x3 = 0;
        int eyex = 25, eyey = 15;
        int cox = 30, coy = 15;

        cc CC = new cc();

        Bitmap b2, b3, b4;
        Image<Gray, byte> image2, imge3, imge4;
        Bitmap bmp1,bmp2,bmp3,bmp4;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            colorImage = null;
            grayImage = null;
            hsvImage = null;
            bmp = null;
            garybmp = null;
            thresholdImage = null;
            img_1 = null;img_2 = null;
            h = null; w = null;
            b2 = null;b3 = null;b4 = null;
            image2 = null; imge3 = null; imge4 = null;
            bmp1 = null; bmp2 = null; bmp3 = null; bmp4 = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {  
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(colorImage!=null)
                {
                    colorImage = null;
                    grayImage = null;
                    hsvImage = null;
                    bmp = null;
                    garybmp = null;
                    thresholdImage = null;
                    img_1 = null; img_2 = null;
                    h = null; w = null;
                    b2 = null; b3 = null; b4 = null;
                    image2 = null; imge3 = null; imge4 = null;
                    bmp1 = null; bmp2 = null; bmp3 = null; bmp4 = null;
                    GC.Collect();
                }
                colorImage = new Image<Bgr, byte>(openFileDialog1.FileName);
                grayImage = new Image<Gray, byte>(colorImage.Bitmap);
                hsvImage = new Image<Hsv, byte>(colorImage.Bitmap);
                bmp = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                //CvInvoke.CvtColor(colorImage, hsvImage,ColorConversion.Bgr2Hsv);
                imageBox17.Width = bmp.Width;
                imageBox17.Height = bmp.Height;
                imageBox17.Image = colorImage;

                pictureBox1.Width = bmp.Width;
                pictureBox1.Height = bmp.Height;
                pictureBox1.Image = bmp;


                imageBox1.Width = bmp.Width;
                imageBox1.Height = bmp.Height;

                imageBox2.Width = bmp.Width;
                imageBox2.Height = bmp.Height;

                imageBox3.Width = bmp.Width;
                imageBox3.Height = bmp.Height;

                imageBox4.Width = bmp.Width;
                imageBox4.Height = bmp.Height;

                imageBox5.Width = bmp.Width;
                imageBox5.Height = bmp.Height;

                imageBox6.Width = bmp.Width;
                imageBox6.Height = bmp.Height;

                imageBox7.Width = bmp.Width;
                imageBox7.Height = bmp.Height;

                imageBox8.Width = bmp.Width;
                imageBox8.Height = bmp.Height;

                imageBox12.Width = bmp.Width;
                imageBox12.Height = bmp.Height;

                imageBox13.Width = 100;
                imageBox13.Height = bmp.Height;
                imageBox13.Top = imageBox12.Top;
                imageBox13.Left = imageBox12.Left + imageBox12.Width + 10;

                pictureBox2.Width = 100;
                pictureBox2.Height = bmp.Height;
                pictureBox2.Top = imageBox12.Top;
                pictureBox2.Left = imageBox12.Left + imageBox12.Width + 10;

                textBox1.Top= imageBox12.Top;
                textBox1.Left = pictureBox2.Left + pictureBox2.Width + 10;

                imageBox14.Width = bmp.Width;
                imageBox14.Height = bmp.Height;

                imageBox15.Width = bmp.Width;
                imageBox15.Height = 100;
                imageBox15.Top = imageBox14.Top+imageBox14.Height+10;
                imageBox15.Left = imageBox14.Left;

                pictureBox3.Width = bmp.Width;
                pictureBox3.Height = 100;
                pictureBox3.Top = imageBox14.Top + imageBox14.Height + 10;
                pictureBox3.Left = imageBox14.Left;

                textBox2.Top = imageBox14.Top;
                textBox2.Left = imageBox14.Left+ imageBox14.Width+10;

                imageBox16.Width = bmp.Width;
                imageBox16.Height = bmp.Height;

                imageBox18.Width = bmp.Width;
                imageBox18.Height = bmp.Height;

                pictureBox4.Width = 100;
                pictureBox4.Height = bmp.Height;
                pictureBox4.Top = imageBox18.Top;
                pictureBox4.Left = imageBox18.Left + imageBox18.Width + 10;

                pictureBox5.Width = bmp.Width;
                pictureBox5.Height = 100;
                pictureBox5.Top = imageBox18.Top + imageBox18.Height + 10;
                pictureBox5.Left = imageBox18.Left;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grayImage = new Image<Gray, byte>(colorImage.Bitmap);
            imageBox1.Image = grayImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageBox3.Image = grayImage.SmoothBlur(3, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageBox4.Image = grayImage.SmoothMedian(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageBox5.Image = colorImage.SmoothBlur(3, 3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageBox6.Image = colorImage.SmoothMedian(3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //imageBox7.Image = Sharpen(grayImage);

            float[,] matrixKernel = new float[3, 3] {
                { -1,-1, -1 },
                {-1, 9,-1 },
                { -1,-1, -1 }};
            ConvolutionKernelF matrix = new ConvolutionKernelF(matrixKernel);
            Image<Gray, float> result = grayImage.Convolution(matrix);
            Image<Gray, Byte> grayResult = result.ConvertScale<byte>(1, 0);
            imageBox7.Image = grayResult;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float[,] matrixKernel = new float[3, 3] {
                { -1,-1, -1 },
                {-1, 9,-1 },
                { -1,-1, -1 }};
            ConvolutionKernelF matrix = new ConvolutionKernelF(matrixKernel);
            Image<Bgr, float> result = colorImage.Convolution(matrix);
            Image<Bgr, Byte> BGRResult = result.ConvertScale<byte>(1, 0);
            imageBox8.Image = BGRResult;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img_1 = new Image<Gray, Byte>(openFileDialog1.FileName);
                imageBox9.Image = img_1;

                imageBox11.Width = img_1.Width;
                imageBox11.Height = img_1.Height;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img_2 = new Image<Gray, Byte>(openFileDialog1.FileName);
                imageBox10.Image = img_2;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> result = new Image<Gray, Byte>(CvInvoke.cvGetSize(img_2));

            result = img_1.AbsDiff(img_2);
            //imageBox11.Image = result;

            int val=50;
            Image<Gray, byte> thresholdImage1;

            Gray thresholdValue = new Gray(val);

            thresholdImage1 = result.ThresholdBinaryInv(thresholdValue, new Gray(255));
            imageBox11.Image = thresholdImage1;

        }

        private void button12_Click(object sender, EventArgs e)
        {

            Bitmap bmpp;
            Graphics g;
            float s;
            int c;

            Pen p = new Pen(Brushes.Black);

            h =new int[imageBox2.Height];
            h=ToProjection_W(imageBox2.Image.Bitmap);
            bmpp = new Bitmap(imageBox13.Width,imageBox13.Height);
            g = Graphics.FromImage(bmpp);
            s = (float)imageBox12.Width / (float)imageBox13.Width;
           

            for (int i=0;i<h.Length;i++)
            {
                c = (int)(h[i] / s); 
                textBox1.AppendText(h[i].ToString() + "\n\r");
                g.DrawLine(p,imageBox13.Width-c, i, imageBox13.Width, i);
            }
            pictureBox2.Image = bmpp;
           // Image<Bgr, Byte> image = new Image<Bgr, Byte>(bmpp);
            //imageBox13.Image = image;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap bmpp;
            Graphics g;
            float s;
            int c;

            Pen p = new Pen(Brushes.Black);

            w = new int[imageBox2.Width];
            w = ToProjection_H(imageBox2.Image.Bitmap);
            bmpp = new Bitmap(imageBox14.Width, imageBox14.Height);
            g = Graphics.FromImage(bmpp);
            s = (float)imageBox14.Height / (float)imageBox15.Height;

            for (int i = 0; i < w.Length; i++)
            {
                c = (int)(w[i] / s);
                textBox2.AppendText(w[i].ToString() + "\n\r");
                g.DrawLine(p,i,imageBox15.Height-c,i,imageBox15.Height);
            }
            pictureBox3.Image = bmpp;

        }

        private void button14_Click(object sender, EventArgs e)
        {
            //imageBox16.Image = grayImage.Sobel(0, 1, 3).Add(grayImage.Sobel(1, 0, 3)).AbsDiff(new Gray(0.0));
            imageBox16.Image = grayImage.Sobel(1, 1, 9);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Colorr(imageBox18.Image.Bitmap, imageBox18.Image.Bitmap);
            imageBox18.Refresh();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img = new Image<Gray, byte>(imageBox18.Image.Bitmap);

            Gray v = new Gray(235);

            img = img.ThresholdBinary(v, new Gray(255));
            imageBox18.Image = img;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Bitmap bmpp;
            Graphics g;
            float s;
            int c;
            Pen p = new Pen(Brushes.Black);

            h = new int[imageBox18.Height];
            h = ToProjection_W(imageBox18.Image.Bitmap);
            bmpp = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            g = Graphics.FromImage(bmpp);
            s = (float)imageBox18.Width / (float)pictureBox4.Width;


            for (int i = 0; i < h.Length; i++)
            {
                c = (int)(h[i] / s);
                g.DrawLine(p, pictureBox4.Width - c, i, pictureBox4.Width, i);
            }
            pictureBox4.Image = bmpp;

            //-----------------------------------------------------------------------

            p = new Pen(Brushes.Black);

            w = new int[imageBox18.Width];
            w = ToProjection_H(imageBox18.Image.Bitmap);
            bmpp = new Bitmap(pictureBox5.Width, pictureBox5.Height);
            g = Graphics.FromImage(bmpp);
            s = (float)imageBox18.Height / (float)pictureBox5.Height;

            for (int i = 0; i < w.Length; i++)
            {
                c = (int)(w[i] / s);
                g.DrawLine(p, i, pictureBox5.Height - c, i, pictureBox5.Height);
            }
            pictureBox5.Image = bmpp;

        }

        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap bmpp;
            Graphics g;
            int a, b;
            Pen p = new Pen(Brushes.Red);

            bmpp = new Bitmap(imageBox18.Image.Bitmap);
            g = Graphics.FromImage(bmpp);

            a = imageBox18.Width / 10;
            b = imageBox18.Height / 10;

            for (int i = 0; i < imageBox18.Width; i += a)
            {
                g.DrawLine(p, i, 0, i, imageBox18.Height);
            }


            for (int i = 0; i < imageBox18.Height; i += b)
            {
                g.DrawLine(p, 0, i, imageBox18.Width, i);
            }
            Image<Bgr, byte> image = new Image<Bgr, byte>(bmpp);
            imageBox18.Image = image;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int v = 7;
            re(imageBox18.Image.Bitmap, v);
            imageBox18.Refresh();
        }

        private unsafe void button20_Click(object sender, EventArgs e)
        {
            int mx_x = 0, mx_y = 0, mi_x = 999, mi_y = 999;

            int iw = imageBox18.Image.Bitmap.Width;
            int ih = imageBox18.Image.Bitmap.Height;
            int w = imageBox18.Image.Bitmap.Width * 3;

            BitmapData dstData = imageBox18.Image.Bitmap.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);

            int step = dstData.Stride;

            for (int i = 0; i < dstData.Height; i++)
            {
                for (int j = 0; j < w; j += 3)
                {
                    if (data[i * step + j] == 0 && data[i * step + j + 1] == 0 && data[i * step + j + 2] == 0)
                    {
                        if (j <= mi_x)
                            mi_x = j;
                        if (j >= mx_x)
                            mx_x = j;
                        if (i <= mi_y)
                            mi_y = i;
                        if (i >= mx_y)
                            mx_y = i;
                    }
                }

            }
            //imageBox18.Image.Bitmap.UnlockBits(dstData);

            mx_x /= 3;
            mi_x /= 3;

            //------------------------------------------------------

            Bitmap bmpp;
            Graphics g;
            Pen p = new Pen(Brushes.Red);

            bmpp = new Bitmap(imageBox18.Image.Bitmap);
            g = Graphics.FromImage(bmpp);

            g.DrawRectangle(p, new Rectangle(mi_x,mi_y,mx_x-mi_x,mx_y-mi_y));

            Image<Bgr, byte> image = new Image<Bgr, byte>(bmpp);
            imageBox18.Image = image;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int  a, b;

            a = imageBox18.Height / 10;
            b = imageBox18.Width / 10;

            for (int i = 0; i < imageBox18.Height; i++)
            {
                if (i >= a * 3 && i <= a * 4)
                {
                    if (h[i] < mi1)
                    {
                        mi1 = h[i];
                        y1 = i;
                    }
                }

                if (i >= a * 5 && i <= a * 6)
                {
                    if (h[i] < mi2)
                    {
                        mi2 = h[i];
                        y2 = i;
                    }
                }
            }

            for (int i = 0; i < imageBox18.Width; i++)
            {
                if (i >= b * 3 && i <= b * 5)
                {
                    if (w[i] < mi3)
                    {
                        mi3 = w[i];
                        x1 = i;
                    }
                }

                if (i >= b * 6 && i <= b * 8)
                {
                    if (w[i] < mi4)
                    {
                        mi4 = w[i];
                        x2 = i;
                    }
                }

                if (i >= b * 4 && i <= b * 7)
                {
                    if (w[i] < mi5)
                    {
                        mi5 = w[i];
                        x3 = i;
                    }
                }
            }
            //-------------------------------------------

            Bitmap bmpp;
            Graphics g;
            Pen p = new Pen(Brushes.Yellow);

            bmpp = new Bitmap(imageBox18.Image.Bitmap);
            g = Graphics.FromImage(bmpp);

            g.DrawRectangle(p, new Rectangle(x1 - eyex, y1 - eyey, 2*eyex, 2*eyey));
            g.DrawRectangle(p, new Rectangle(x2-eyex, y1-eyey, 2 * eyex, 2 * eyey));
            g.DrawRectangle(p, new Rectangle(x3-cox, y2-coy, 2*cox, 2*coy));

            Image<Bgr, byte> image = new Image<Bgr, byte>(bmpp);
            imageBox18.Image = image;
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp1 = new Bitmap(openFileDialog1.FileName);
                pictureBox6.Image = bmp1;

                pictureBox9.Width = bmp1.Width;
                pictureBox9.Height = bmp1.Height;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp2 = new Bitmap(openFileDialog1.FileName);
                pictureBox7.Image = bmp2;

                bmp3 = new Bitmap(bmp2.Width, bmp2.Height, PixelFormat.Format24bppRgb);
                bmp4 = new Bitmap(bmp2.Width, bmp2.Height, PixelFormat.Format24bppRgb);

            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            mask_img(bmp2, bmp3);
            pictureBox8.Image = bmp3;
            mask(bmp1, bmp2, bmp3, bmp4);
            pictureBox9.Image = bmp4;
            pictureBox9.Refresh();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int val;
         

            val = trackBar1.Value;   //取拉桿值
            label1.Text = val.ToString();

            Gray thresholdValue = new Gray(val);
            if (radioButton1.Checked == true)
            {
                thresholdImage = grayImage.ThresholdBinaryInv(thresholdValue, new Gray(255));
                imageBox2.Image = thresholdImage;
                imageBox12.Image = thresholdImage;
                imageBox14.Image = thresholdImage;
            }
            if (radioButton2.Checked == true)
            {
                thresholdImage = grayImage.ThresholdBinary(thresholdValue, new Gray(255));
                imageBox2.Image = thresholdImage;
                imageBox12.Image = thresholdImage;
                imageBox14.Image = thresholdImage;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar2.Value;
            mih = v;
            label5.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar3.Value;
            mxh = v;
            label6.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            eye(bmp, imageBox18.Image.Bitmap);
            imageBox18.Refresh();
        }

        private void button23_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(c1.ab().ToString());
        }

        private void button27_Click(object sender, EventArgs e)
        {
            //指定輸出影像
            b2 = new Bitmap(bmp.Width, bmp.Height);
            CC.Initial(b2.Width, b2.Height, b2, 20000);
            CC.SetRemovalSize(20);

            //指定來源影像，並開始計算
            CC.Connection(imageBox18.Image.Bitmap);

            //可以不用此行
            CC.ShowComponents();

            image2 = new Image<Gray, byte>(b2);
           // imageBox18.Image = image2;

            image2 = image2.Dilate(1);
           // imageBox18.Image = image2;
            //imageBox18.Refresh();

            image2 = image2.Erode(1);
            imageBox18.Image = image2;
           // imageBox18.Refresh();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar5.Value;
            mis = v;
            label7.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar4.Value;
            mxs = v;
            label8.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar7.Value;
            miv = v;
            label9.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            int v;
            v = trackBar6.Value;
            mxv = v;
            label10.Text = v.ToString();
            hsv(bmp, imageBox17.Image.Bitmap);
            imageBox17.Refresh();
            imageBox18.Image = imageBox17.Image;
            imageBox2.Image = imageBox17.Image;
        }

        public unsafe int[] ToProjection_W(Bitmap srcImg)
        {
            int iw = srcImg.Width;
            int ih = srcImg.Height;

            int[] warray = new int[srcImg.Height];

            BitmapData dstData = srcImg.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            int step = dstData.Stride;

            for (int i = 0; i < dstData.Height; i++)
            {
                int sum = 0;
                for (int j = 0; j < dstData.Width; j++)
                {
                    if (data[i * step + j] > 128)
                        sum = sum + 0;
                    else
                        sum = sum + 1;
                }

                warray[i] = sum;
            }


            srcImg.UnlockBits(dstData);


            return warray;
        }

        public unsafe int[] ToProjection_H(Bitmap srcImg)
        {
            int iw = srcImg.Width;
            int ih = srcImg.Height;

            int[] harray = new int[srcImg.Width];

            BitmapData dstData = srcImg.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            int step = dstData.Stride;

            for (int i = 0; i < dstData.Width; i++)
            {
                int sum = 0;
                for (int j = 0; j < dstData.Height; j++)
                {
                    if (data[j * step + i] == 0)
                        sum += 1;
                }

                harray[i] = sum;
            }

            srcImg.UnlockBits(dstData);

            return harray;
        }

        private Image<Gray, Byte> Sharpen(Image<Gray, Byte> image)
        {
            Image<Gray, Byte> result = image.CopyBlank(); //copy a blank image

            MIplImage MIpImg = (MIplImage)System.Runtime.InteropServices.Marshal.PtrToStructure(image.Ptr, typeof(MIplImage));
            MIplImage MIpImgResult = (MIplImage)System.Runtime.InteropServices.Marshal.PtrToStructure(result.Ptr, typeof(MIplImage));

            int imageHeight = MIpImg.Height;
            int imageWidth = MIpImg.WidthStep;
            unsafe
            {
                for (int height = 1; height < imageHeight - 1; height++)
                {
                    //current_pixel line
                    byte* currentPixel = (byte*)MIpImg.ImageData + imageWidth * height;
                    //up_pixel line
                    byte* uplinePixel = currentPixel - MIpImg.WidthStep;
                    //down_pixel line
                    byte* downlinePixel = currentPixel + MIpImg.WidthStep;
                    //result current_pixel line
                    byte* resultPixel = (byte*)MIpImgResult.ImageData + imageWidth * height;

                    for (int width = 1; width < imageWidth - 1; width++)
                    {
                        //5*current_pixel-left_pixel-right_pixel-up_pixel-down_pixel
                        int sharpValue = 5 * currentPixel[width] - currentPixel[width - 1]
                                            - currentPixel[width + 1] - uplinePixel[width]
                                            - downlinePixel[width];

                        if (sharpValue < 0) sharpValue = 0;     //Gray level 0~255
                        if (sharpValue > 255) sharpValue = 255; //Gray level 0~255

                        resultPixel[width] = (byte)sharpValue;
                    }
                }
            }

            return result;
        }

        public unsafe void hsv(Bitmap sbmp, Bitmap tbmp)
        {
            int hh, ss, vv;
            int iw = sbmp.Width;
            int ih = sbmp.Height;

            int w = sbmp.Width * 3;

            BitmapData dstData = sbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = tbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            byte* data1 = (byte*)(dstData1.Scan0);

            int step = dstData.Stride;
            int step1 = dstData1.Stride;

            for (int i = 0; i < dstData.Height; i++)
            {
                for (int j = 0; j < w; j+=3)
                {
                    RGBToHSV(data[i * step + j + 2], data[i * step + j + 1], data[i * step + j], &hh, &ss, &vv);
                    if (hh >= mih && hh <= mxh &&
                        ss >= mis && ss <= mxs &&
                        vv >= miv && vv <= mxv)
                    {
                        data1[i * step1 + j] = data[i * step + j];
                        data1[i * step1 + j + 1] = data[i * step + j + 1];
                        data1[i * step1 + j + 2] = data[i * step + j + 2];
                    }
                    else
                    {
                        data1[i * step1 + j] = 0;
                        data1[i * step1 + j + 1] = 0;
                        data1[i * step1 + j + 2] = 0;
                    }
                }

            }
            sbmp.UnlockBits(dstData);
            tbmp.UnlockBits(dstData1);
        }

        public unsafe void Colorr(Bitmap sbmp, Bitmap tbmp)
        {
            int hh, ss, vv;

            int iw = sbmp.Width;
            int ih = sbmp.Height;

            int w = sbmp.Width * 3;

            BitmapData dstData = sbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = tbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            byte* data1 = (byte*)(dstData1.Scan0);

            int step = dstData.Stride;
            int step1 = dstData1.Stride;

            for (int i = 0; i < dstData.Height; i++)
            {
                for (int j = 0; j < w; j+=3)
                {
                    RGBToHSV(data[i * step + j + 2], data[i * step + j + 1], data[i * step + j], &hh, &ss, &vv);
                    if (hh >= 20 && hh <= 40 && ss >= 0 && ss <= 255 && vv >= 0 && vv <= 255)
                    {
                        data1[i * step1 + j] = data[i * step + j];
                        data1[i * step1 + j + 1] = data[i * step + j + 1];
                        data1[i * step1 + j + 2] = data[i * step + j + 2];
                    }
                    else
                    {
                        data1[i * step1 + j] = 255;
                        data1[i * step1 + j + 1] = 255;
                        data1[i * step1 + j + 2] = 255;
                    }
                }

            }
            sbmp.UnlockBits(dstData);
            tbmp.UnlockBits(dstData1);
        }

        int min(int a, int b)
        {
            if (a >= b)
            {
                return b;
            }
            else
            {
                return a;
            }
        }

        int max(int a, int b)
        {
            if (a >= b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public unsafe void RGBToHSV(int r, int g, int b, int* ch, int* cs, int*cv)
        {
            float temp, mi, delta;
            int inter, c;
            float h=0, s, v;

            mi = min(min(r, g), b);
            v = max(max(r, g), b);
            delta = v - mi;

            if (delta == 0)
                h = 0;
            else
            {
                if (r == v)
                    h = 60.0f*(g - b) / delta;
                if (g == v)
                    h = 120.0f + 60.0f * (b - r) / delta;
                if (b == v)
                    h = 240.0f + 60.0f * (r - g) / delta;
                if (h < 0.0f)
                    h = h + 360.0f;
            }

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            s *= 255.0f;

            *ch = (int)h;
            *cs = (int)s;
            *cv = (int)v;
        }

        public unsafe void re(Bitmap sbmp,int v)
        {
            int hh, ss, vv;
            int iw = sbmp.Width;
            int ih = sbmp.Height;

            int w = sbmp.Width * 3;
            v = (imageBox18.Height / 10) * v;

            BitmapData dstData = sbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);

            int step = dstData.Stride;

            for (int i = 0; i < dstData.Height; i++)
            {
                for (int j = 0; j < w; j += 3)
                {
                    if (i>=v)
                    {
                        data[i * step + j] = 255;
                        data[i * step + j + 1] = 255;
                        data[i * step + j + 2] = 255;
                    }
                }

            }
            sbmp.UnlockBits(dstData);
        }

        public unsafe void eye(Bitmap sbmp, Bitmap tbmp)
        {
            int iw = sbmp.Width;
            int ih = sbmp.Height;
            int w = sbmp.Width * 3;

            BitmapData dstData = sbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = tbmp.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            byte* data1 = (byte*)(dstData1.Scan0);

            int step = dstData.Stride;
            int step1 = dstData1.Stride;

            for (int y = 0; y < dstData.Height; y++)
            {
                for (int x = 0; x< w; x += 3)
                {
                    if ((x >= 3 * (x1 - eyex) && x <= 3 * (x1 + eyex) && y >= y1 - eyey && y <= y1 + eyey) ||
                        (x >= 3 * (x2 - eyex) && x <= 3 * (x2 + eyex) && y >= y1 - eyey && y <= y1 + eyey) ||
                        (x >= 3 * (x3 - cox) && x <= 3 * (x3 + cox) && y >= y2 - coy && y <= y2 + coy))
                    {
                        if(data1[y * step1 + x]!=0)
                        {
                            data1[y * step1 + x] = data[y * step + x];
                            data1[y * step1 + x + 1] = data[y * step + x + 1];
                            data1[y * step1 + x + 2] = data[y * step + x + 2];
                        }
                        else
                        {
                            data1[y * step1 + x] = 255;
                            data1[y * step1 + x + 1] = 255;
                            data1[y * step1 + x + 2] = 255;
                        }

                    }
                    else
                    {
                        data1[y * step1 + x] = 255;
                        data1[y * step1 + x + 1] = 255;
                        data1[y * step1 + x + 2] = 255;
                    }
                }

            }
            sbmp.UnlockBits(dstData);
            tbmp.UnlockBits(dstData1);
        }

        public unsafe void mask_img(Bitmap sbmp1, Bitmap tbmp1)
        {
            int iw = sbmp1.Width;
            int ih = sbmp1.Height;
            int w = sbmp1.Width * 3;

            BitmapData dstData = sbmp1.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = tbmp1.LockBits(new Rectangle(0, 0, iw, ih), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);
            byte* data1 = (byte*)(dstData1.Scan0);

            int step = dstData.Stride;
            int step1 = dstData1.Stride;

            for (int y = 0; y < dstData.Height; y++)
            {
                for (int x = 0; x < w; x += 3)
                {
                    if (data[y * step + x] == 255 && data[y * step + x + 1] == 255 && data[y * step + x + 2] == 255)
                    {
                        data1[y * step1 + x] = 255;
                        data1[y * step1 + x+1] = 255;
                        data1[y * step1 + x+2] = 255;
                    }
                    else
                    {
                        data1[y * step1 + x] = 0;
                        data1[y * step1 + x + 1] = 0;
                        data1[y * step1 + x + 2] = 0;
                    }
                }

            }
            sbmp1.UnlockBits(dstData);
            tbmp1.UnlockBits(dstData1);
        }

        public unsafe void mask(Bitmap sbmp1, Bitmap sbmp2, Bitmap sbmp3, Bitmap tbmp1)
        {
            int iw = sbmp1.Width;
            int ih = sbmp1.Height;
            int w = sbmp1.Width * 3;

            BitmapData dstData  = sbmp1.LockBits(new Rectangle(0, 0, sbmp1.Width, sbmp1.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = sbmp2.LockBits(new Rectangle(0, 0, sbmp2.Width, sbmp2.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData2 = sbmp3.LockBits(new Rectangle(0, 0, sbmp3.Width, sbmp3.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstData3 = tbmp1.LockBits(new Rectangle(0, 0, tbmp1.Width, tbmp1.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* data  = (byte*)(dstData.Scan0);
            byte* data1 = (byte*)(dstData1.Scan0);
            byte* data2 = (byte*)(dstData2.Scan0);
            byte* data3 = (byte*)(dstData3.Scan0);

            int step  =  dstData.Stride;
            int step1 = dstData1.Stride;
            int step2 = dstData2.Stride;
            int step3 = dstData3.Stride;

            for (int y = 0; y < dstData.Height; y++)
            {
                for (int x = 0; x < w; x += 3)
                {

                    if (data2[y * step2 + x] == 0 && data2[y * step2 + x+1] == 0 && data2[y * step2 + x+2] == 0)
                    {
                        data3[y * step3 + x] = data1[y * step1 + x ];
                        data3[y * step3 + x+1] = data1[y * step1 + x + 1];
                        data3[y * step3 + x+2] = data1[y * step1 + x + 2];
                    }
                    else
                    {
                        data3[y * step3 + x ] = data[y * step + x ];
                        data3[y * step3 + x + 1] = data[y * step + x+1];
                        data3[y * step3 + x + 2] = data[y * step + x+2];
                    }                 
                }
            }
            sbmp1.UnlockBits(dstData);
            sbmp2.UnlockBits(dstData1);
            sbmp3.UnlockBits(dstData2);
            tbmp1.UnlockBits(dstData3);
        }
    }
    
}
