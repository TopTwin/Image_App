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
        Bitmap picture;
        Bitmap resultPic;
        double[,] matrix;

        public SpatialFiltering()
        {
            w = 0;  h = 0;
            r1 = 0; r2 = 0;
            picture = null;
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
            for (int i = 0; i < width; i++)         //Цикл прохода по картинке
                for(int j = 0; j < height; j++)
                {
                    int[] lul = new int[3];
                    lul[0] = bytesPic[i * 3 + j * 3 + 2];
                    lul[1] = bytesPic[i * 3 + j * 3 + 1];
                    lul[2] = bytesPic[i * 3 + j * 3];
                    byte[] pix = PixProcessing(j, i, bytesPic);    //Обрабатываем пиксель и получаем новое значение
                    //pix[0] - R, pix[1] - G, pix[2] - B
                    bytesPic[i * 3 + j * 3 + 2] = pix[0];
                    bytesPic[i * 3 + j * 3 + 1] = pix[1];
                    bytesPic[i * 3 + j * 3] = pix[2];
                }
            resultPic = ananas.ImageFromByte(bytesPic, picture.Width, picture.Height);
            return resultPic;
        }
        byte[] PixProcessing(int x, int y, byte[] bytesPic)
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
                        i + y - r2 < picture.Height &&
                        j + x - r1 >= 0 &&
                        j + x - r1 < picture.Height)
                    {
                        int newY = y + i - r2;
                        int newX = x + j - r1;
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхний левый угол
                    if ((i + y - r2) < 0 && (j + x - r1) < 0)   
                    {
                        int newX = x + (r1 - j);
                        int newY = y + (r2 - i);
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхний правый угол
                    if ((i + y - r2) < 0 && (x + j - r1) >= picture.Width)
                    {
                        int newX = x - (j - r1);
                        int newY = y + (r2 - i);
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижний правый угол
                    if ((i + y - r2) >= picture.Height && (x + j - r1) >= picture.Width)
                    {
                        int newX = x - (j - r1);
                        int newY = y - (i - r2);
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижний левый угол
                    if ((i + y - r2) >= picture.Height && (j + x - r1) < 0)
                    {
                        int newX = x + (r1 - j);
                        int newY = y - (i - r2);
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //верхняя сторона
                    if ((i + y - r2) < 0)       
                    {
                        int newY = y + (r2 - i);
                        int newX = x + j - r1;
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //левая сторона
                    if ((j + x - r1) < 0)
                    {
                        int newX = x + (r1 - j);
                        int newY = y + i - r2;
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //правая сторона
                    if ((x + j - r1) >= picture.Width)
                    {
                        int newX = x - (j - r1);
                        int newY = y + i - r2;
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                    //нижняя сторона
                    if ((i + y - r2) >= picture.Height)
                    {
                        int newY = y - (i - r2);
                        int newX = x + j - r1;
                        buf = bytesPic[newX * 3 + newY * 3 + 2] * matrix[i, j];
                        pix[0] += buf;
                        buf = bytesPic[newX * 3 + newY * 3 + 1] * matrix[i, j];
                        pix[1] += buf;
                        buf = bytesPic[newX * 3 + newY * 3] * matrix[i, j];
                        pix[2] += buf;
                        continue;
                    }
                }
            byte[] tt = new byte[3];

            if (pix[0] > 255)   tt[0] = 255;
            else                tt[0] = (byte)pix[0];

            if (pix[1] > 255)   tt[1] = 255;
            else                tt[1] = (byte)pix[1];

            if (pix[2] > 255)   tt[2] = 255;
            else                tt[2] = (byte)pix[2];

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
