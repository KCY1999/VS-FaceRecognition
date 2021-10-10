using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenCvSharp.Mat pic = new OpenCvSharp.Mat();
            pic = Cv2.ImRead(openFileDialog1.FileName,
            OpenCvSharp.ImreadModes.Grayscale);
            pictureBox1.Image = pic.ToBitmap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenCvSharp.Mat src = new OpenCvSharp.Mat();
            // src = Cv2.ImRead("lena_std.bmp");
            src = Cv2.ImRead(openFileDialog1.FileName);
            pictureBox1.Image = src.ToBitmap();
            Cv2.ImShow("image", src);
            Cv2.WaitKey(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
            //===================彩色===============================
              Image<Bgr, byte> colorImage;
              colorImage = new Image<Bgr, byte>(openFileDialog1.FileName);
              imageBox1.Image = colorImage;
            */
            
            //===================灰階===============================
            Image<Gray, byte> grayImage;
            grayImage = new Image<Gray, byte>(openFileDialog1.FileName);
            imageBox1.Image = grayImage;
            
        }
    }
}
