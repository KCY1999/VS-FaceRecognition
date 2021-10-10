using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace 影像
{
    public struct COO
    {
        int x1, x2, y1, y2;
    }

    unsafe public class cc
    {
        int BACKGROUND_COLOR = 255;//背景色的預設值
        int FOREGROUND_COLOR = 0;//前景色的預設值
        int MAX_LABEL_LIMIT = 30000;//允許一張影像中的label的最大數量, 請勿任意更改.
                                    //若要更改,需搭配更改 LabelMatrix, ConnectSet,
                                    //Buf, F 這些變數的資料型態, 以及相關的functions.
        int DEFAULT_MAX_LABEL = 1000;//預設一張影像中最多可包含的label數目, 不得大於 MAX_LABEL_LIMIT
        int REMOVAL_SIZE = 0;//預設的雜訊大小=0, 即不處理雜訊.         
        int MAX_X = 9999999;
        int MAX_Y = 9999999;


        int NewLabel; // 指向目前新增的label編號, 即component的總數
        int Width, Height, TotalWidth; //影像的寬與高, TotalWidth=Width*3 (R,G,B三個chennels)
        int MaxLabel;      //一張影像中最多可包含的label數目,可由 ConntdCompnt(),
                           //及 Initial()設定. 請視狀況而設定, 此值越大會浪費記憶體空間,
                           //及拖慢執行的速度.
        int up, left, lx;
        int RemovalSize;   // 雜訊的大小, 可由SetRemovalSize()設定.
        int x, y, x1, i, j, k, y1;
        Byte* ptr;
        Byte* Upptr;
        Bitmap Show; // 指向欲輸出結果的影像的指標, 可由SetShow()設定.
        int[,] LabelMatrix=null;  // 儲存影像的labels. label的編號不一定有順序性
        int[] ConnectSet;   //儲存每個找到的label的size，有可能某個label的size=0
        char[,] Buf;
        char[] F;
        COO* CompntPos; //用來儲存每個元件的座標
        int CompntNum;
        char ERRORMSG;

        //-----------------------------------------------------------------------------

        public void ConntdCompnt()
        {
            NewLabel = 0;
            ERRORMSG = (char)0;
            Width = 0;
            Height = 0;
            MaxLabel = DEFAULT_MAX_LABEL;
            LabelMatrix = null;
            ConnectSet = null;
            Buf = null;
            F = null;
            Show = null;
            CompntPos = null;
            CompntNum = 0;

            RemovalSize = REMOVAL_SIZE;
        }

        public void ConntdCompnt(int w, int h, int num, int size)
        {
            NewLabel = 0;
            ERRORMSG = (char)0;
            MaxLabel = DEFAULT_MAX_LABEL;
            LabelMatrix = null;
            ConnectSet = null;
            Buf = null;
            F = null;
            CompntPos = null;
            CompntNum = 0;

            Show = null;
            SetRemovalSize(size);
            Initial(w, h, null, num);
        }

        public void SetRemovalSize(int size)
        {
            if (size < 1)
                return;

            RemovalSize = size;

        }

        public bool Initial(int w, int h, Bitmap bmp, int num)
        {

            DeleteMatrix();
            DeleteConnectSet();
            DeleteBuf();
            DeleteCompntPos();


            NewLabel = 0;
            ERRORMSG = (char)0;
            Width = w;
            Height = h;
            TotalWidth = Width * 3;

            if (num >= 0)
                if (SetMaxLabel(num) == false)
                {
                    ERRORMSG = (char)1;
                    return false;
                }

            if (AllocateLabelMatrix() == false)
            {
                ERRORMSG = (char)1;
                return false;
            }

            SetShow(bmp);
            return true;
        }

        public bool SetMaxLabel(int num)
        {
            if (num > MAX_LABEL_LIMIT)
                return false;
            else
                MaxLabel = num;

            return true;

        }

        public bool AllocateLabelMatrix()
        {

            if (LabelMatrix != null)
            {
                DeleteMatrix();
                DeleteConnectSet();
                DeleteBuf();
                DeleteCompntPos();

            }

            LabelMatrix = new int[Height + 2, Width + 2];

            for (int i = 0; i < Height + 2; ++i)
            {
                for (int j = 0; j < Width + 2; j++)  // initialization
                {
                    if (LabelMatrix[i,j] == null)
                    {
                        LabelMatrix = null;
                        return false;
                    }
                    LabelMatrix[i, j] = 0;
                }
                   
            }
            //----------------------------------------------------
            if (AllocateSet() == false)
            {
                DeleteMatrix();
                return false;
            }

            if (AllocateBuf() == false)
            {
                DeleteMatrix();
                DeleteConnectSet();
                return false;
            }

            return true;

        }

        public bool AllocateSet()
        {
            if (ConnectSet != null)
                return false;

            ConnectSet = new int[MaxLabel + 1];

            F = new char[MaxLabel + 1];

            InitialSet();

            return true;
        }

        public void InitialSet()
        {
            //ConnectSet的大小為使用者預留的，通常會比真正找到的label數還要多很多
            //由於元件最後的編號，不一定是有順序的，例如可能最後找到三個物體，
            //但其編號為1, 4, 10。因此，在ConnectSet中有物體的編號位置，才會有值。

            for (i = 0; i < MaxLabel + 1; i++)
            {
                ConnectSet[i] = i;
                F[i] = (char)1;
            }
        }

        public bool AllocateBuf()
        {
            Buf = new char[Height + 2, Width + 2];

            for (int i = 0; i < Height + 2; ++i)
            {
                for (int j = 0; j < Width + 2; j++)  // initialization
                {
                    if (Buf[i,j] == null)
                    {
                        Buf = null;
                        return false;
                    }
                    Buf[i, j] = (char)BACKGROUND_COLOR;
                }
                    
            }
            return true;
        }

        public void SetShow(Bitmap Bmp)
        {
            Show = Bmp;
        }

        public void Connection(Bitmap Bmp)
        {
            FirstScan(Bmp);
            SecondScan();
            LabelIgnoredComponents();
        }

        public void FirstScan(Bitmap Bmp)
        {
            NewLabel = 0;
            CompntNum = 0;
            InitialMatrix();
            InitialSet();
            InitialBuf(Bmp);

            for (y = 1; y <= Height; y++)
            {
                for (x = 1; x <= Width; x++)
                {
                    if (Buf[y, x] == FOREGROUND_COLOR)
                    {
                        up = Buf[y - 1, x];
                        left = Buf[y, x - 1];

                        if (up == BACKGROUND_COLOR && left == BACKGROUND_COLOR)
                        {
                            NewLabel++;
                            lx = NewLabel;
                        }
                        else
                          if ((LabelMatrix[y - 1, x] != LabelMatrix[y, x - 1]) &&
                              (up != BACKGROUND_COLOR) && (left != BACKGROUND_COLOR))
                        {
                            if (F[ConnectSet[LabelMatrix[y, x - 1]]] == 1)
                            {
                                ConnectSet[LabelMatrix[y, x - 1]] = ConnectSet[LabelMatrix[y - 1, x]];
                                F[ConnectSet[LabelMatrix[y - 1, x]]] = (char)0;
                            }
                            else
                              if (F[ConnectSet[LabelMatrix[y - 1, x]]] == 1)
                            {
                                ConnectSet[LabelMatrix[y - 1, x]] = ConnectSet[LabelMatrix[y, x - 1]];
                                F[ConnectSet[LabelMatrix[y, x - 1]]] = (char)0;
                            }
                            else
                                for (k = 0; k < NewLabel; k++)
                                    if (ConnectSet[k] == ConnectSet[LabelMatrix[y - 1, x]])
                                        ConnectSet[k] = ConnectSet[LabelMatrix[y, x - 1]];

                            lx = LabelMatrix[y, x - 1];
                        }
                        else
                            if (left != BACKGROUND_COLOR)
                            lx = LabelMatrix[y, x - 1];
                        else
                              if (up != BACKGROUND_COLOR)
                            lx = LabelMatrix[y - 1, x];

                        LabelMatrix[y, x] = lx;
                    }
                }// end for x
            } // end for y
        }

        public void InitialMatrix()
        {
            // LabelMatrix的w和h比真實的image的w和h個多出2個byte
            //而圖像資料會擺在LabelMatrix的[1,1]開始處。即會留下一個寬為1 byte的框，
            //使得背景的編號永遠為0，而物體的編號則從1開始。
            for (i = 0; i < Height + 2; i++)
                for (j = 0; j < Width + 2; j++)
                    LabelMatrix[i, j] = 0;

        }

        public void InitialBuf(Bitmap Bmp)
        {
            BitmapData dstData = Bmp.LockBits(new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* data = (byte*)(dstData.Scan0);

            int step = dstData.Stride;
            for (y = 0, y1 = 1; y < Height; y++, y1++)
            {
                for (x = 0, x1 = 1; x < TotalWidth; x += 3, x1++)
                {
                    Buf[y1, x1] = (char)data[y * step + x];
                }
            }
            Bmp.UnlockBits(dstData);
        }
        public void SecondScan()
        {
            for (y = 1; y <= Height; y++)
                for (x = 1; x <= Width; x++)
                    if (LabelMatrix[y, x] != 0)
                        LabelMatrix[y, x] = ConnectSet[LabelMatrix[y, x]];
        }
        public void LabelIgnoredComponents()
        {
            for (i = 0; i < NewLabel + 1; i++)
            {
                F[i] = (char)0; 
                ConnectSet[i] = 0;
            }

            for (y = 1; y < Height; y++)
                for (x = 1; x < Width; x++) 
                    ConnectSet[LabelMatrix[y, x]]++;

            for (i = 0; i < NewLabel + 1; i++)
                if (ConnectSet[i] < RemovalSize)
                    F[i] = (char)1;  
        }
        public void ShowComponents()
        {
            BitmapData dstData = Show.LockBits(new Rectangle(0, 0, Show.Width, Show.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            ptr = (byte*)(dstData.Scan0);
            int step = dstData.Stride;

            for (y = 0, y1 = 1; y < Height; y++, y1++)
            {
                for (x = 0, x1 = 1; x < TotalWidth; x += 3, x1++)
                {
                    if (LabelMatrix[y1, x1] != 0 && F[LabelMatrix[y1, x1]] == 0)
                    {
                       // ptr[y * step + x] = (byte)((ConnectSet[LabelMatrix[y1, x1]] * 624) & 0xff);
                       // ptr[y * step + x + 1] = (byte)((ConnectSet[LabelMatrix[y1, x1]] * 371) & 0xff);
                        //ptr[y * step + x + 2] = (byte)((ConnectSet[LabelMatrix[y1, x1]] * 923) & 0xff);

                        ptr[y * step + x] = (byte)FOREGROUND_COLOR;
                        ptr[y * step + x + 1] = (byte)FOREGROUND_COLOR;
                        ptr[y * step + x + 2] = (byte)FOREGROUND_COLOR;
                    }
                    else
                    {
                        ptr[y * step + x] = (byte)BACKGROUND_COLOR;
                        ptr[y * step + x + 1] = (byte)BACKGROUND_COLOR;
                        ptr[y * step + x + 2] = (byte)BACKGROUND_COLOR;
                    }
                }
            }
            Show.UnlockBits(dstData);
        }

        public void DeleteMatrix()
        {
            if (LabelMatrix == null)
                return;

            LabelMatrix = null;
            GC.Collect();
        }

        public void DeleteConnectSet()
        {
            if (ConnectSet == null)
                return;

            ConnectSet = null;
            F = null;
            GC.Collect();
        }

        public void DeleteBuf()
        {
            if (Buf == null)
                return;

            Buf = null;
            GC.Collect();
        }

        public void DeleteCompntPos()
        {
            if (CompntPos == null)
                return;

            CompntPos = null;
            GC.Collect();

        }

    }
}
