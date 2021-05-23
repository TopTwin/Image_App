using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgApp_2_WinForms
{
    class SpatialFiltering: IDisposable
    {
        int w, h;       //размеры прямоугольника
        int r1, r2;     //r1 - width, r2 - height
        int widthPic, heightPic;
        Bitmap picture;
        Bitmap resultPic;
        double[,] matrix;

        public SpatialFiltering()
        {
            w = 0;  h = 0;
            r1 = 0; r2 = 0;
            picture = null;
            widthPic = 0;
            heightPic = 0;
            matrix = new double[0,0];
            resultPic = null;
        }

        public SpatialFiltering(Bitmap bit, int _w, int _h, double[,] mat)
        {
            w = _w; 
            h = _h;
            r1 = (w - 1) / 2;
            r2 = (h - 1) / 2;
            picture = bit;
            widthPic = picture.Width;
            heightPic = picture.Height;
            matrix = mat;
            resultPic = null;
        }
        ~SpatialFiltering()
        {
            Dispose();
        }

        public void SetPicture(Bitmap _bit)     //устанавливает картинку
        {
            if (picture != null)
                picture.Dispose();
            picture = _bit;
        }
        public void SetMatrix(int _w, int _h, double[,] mat)    //устанавливает матрицу и ее размеры
        {
            w = _w;
            h = _h;
            r1 = (_w - 1) / 2;
            r2 = (_h - 1) / 2;
            matrix = mat;
        }
        public Bitmap ApplyFilter()     //Применение фильтра
        {
            //если размер матрицы не совпадает с фактической матрицей
            //то возвращаем null
            if (!CheckMatrix())     
                return null;

            int width = picture.Width;
            int height = picture.Height;
            byte[] bytesPic = ananas.ByteFromImage(picture);
            byte[] bytesResultPic = new byte[widthPic * heightPic * 3];
            for (int i = 0; i < height; i++)         //Цикл прохода по картинке
                for(int j = 0; j < width; j++)
                {
                    int[] lul = new int[3];
                    lul[0] = bytesPic[i * widthPic * 3 + j * 3 + 2];
                    lul[1] = bytesPic[i * widthPic * 3 + j * 3 + 1];
                    lul[2] = bytesPic[i * widthPic * 3 + j * 3];
                    byte[] pix = PixProcessing(i, j, bytesPic);    //Обрабатываем пиксель и получаем новое значение
                                                                   //pix[0] - R, pix[1] - G, pix[2] - B
                    bytesResultPic[i * widthPic * 3 + j * 3 + 2] = pix[0];
                    bytesResultPic[i * widthPic * 3 + j * 3 + 1] = pix[1];
                    bytesResultPic[i * widthPic * 3 + j * 3] = pix[2];
                }
            resultPic = ananas.ImageFromByte(bytesResultPic, picture.Width, picture.Height);
            return (Bitmap)resultPic.Clone();
        }
        byte[] PixProcessing(int y, int x, byte[] bytesPic)
        {
            //х,у - координаты пикселя
            //i,j - координаты в матрице
            double[] pix = new double[3];
            //pix[0] - R
            //pix[1] - G
            //pix[2] - B
            double buf = 0;     //для хранения промежуточного результата
            for(int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    //центр
                    if (i + y - r2 >= 0 &&
                        i + y - r2 <= heightPic &&
                        j + x - r1 >= 0 &&
                        j + x - r1 <= widthPic)
                    {
                        int newY = y + i - r2;
                        int R2;
                        if (i > r2)
                        {
                            R2 = r2 + 1;
                            newY = y + i - R2;
                        }
                        int newX = x + j - r1;
                        int R1;
                        if (j > r1)
                        {
                            R1 = r1 + 1;
                            newX = x + j - R1;
                        }
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхний левый угол
                    if ((i + y - r2) < 0 && (j + x - r1) < 0)   
                    {
                        int newX = x + (r1 - j);
                        int newY = y + (r2 - i);
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхний правый угол
                    if ((i + y - r2) < 0 && (x + j - r1) > widthPic)
                    {
                        int newX = x - (j - r1);
                        int newY = y + (r2 - i);
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижний правый угол
                    if ((i + y - r2) > heightPic && (x + j - r1) >= widthPic)
                    {
                        int newX = x - (j - r1);
                        int newY = y - (i - r2);
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижний левый угол
                    if ((i + y - r2) > heightPic && (j + x - r1) < 0)
                    {
                        int newX = x + (r1 - j);
                        int newY = y - (i - r2);
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхняя сторона
                    if ((i + y - r2) < 0)       
                    {
                        int newY = y + (r2 - i);
                        int newX = x + j - r1;
                        int R1;
                        if (j > r1)
                        {
                            R1 = r1 + 1;
                            newX = x + j - R1;
                        }
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //левая сторона
                    if ((j + x - r1) < 0)
                    {
                        int newX = x + (r1 - j);
                        int newY = y + i - r2;
                        int R2;
                        if (i > r2)
                        {
                            R2 = r2 + 1;
                            newY = y + i - R2;
                        }
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //правая сторона
                    if ((x + j - r1) > widthPic)
                    {
                        int newX = x - (j - r1);
                        int newY = y + i - r2;
                        int R2;
                        if (i > r2)
                        {
                            R2 = r2 + 1;
                            newY = y + i - R2;
                        }
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижняя сторона
                    if ((i + y - r2) > heightPic)
                    {
                        int newY = y - (i - r2);
                        int newX = x + j - r1;
                        int R1;
                        if (j > r1)
                        {
                            R1 = r1 + 1;
                            newX = x + j - R1;
                        }
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * widthPic * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                }
            byte[] tt = new byte[3];

            tt[0] = (byte)pix[0];
            tt[1] = (byte)pix[1];
            tt[2] = (byte)pix[2];

            if (pix[0] > 255)   tt[0] = 255;
            if (pix[0] < 0)     tt[0] = 0;

            if (pix[1] > 255)   tt[1] = 255;
            if (pix[1] < 0)     tt[1] = 0;

            if (pix[2] > 255)   tt[2] = 255;
            if (pix[2] < 0)     tt[2] = 0;

            return tt;

        }
        bool CheckMatrix()
        {
            if (w * h == matrix.Length)
                return true;
            else
                return false;
        }
        public void Dispose()
        {
            w = 0; h = 0;
            r1 = 0; r2 = 0;
            matrix = new double[0,0];
            picture.Dispose();
            resultPic.Dispose();
        }
    }
}
