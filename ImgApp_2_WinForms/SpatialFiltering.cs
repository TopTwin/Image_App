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
        int r1, r2;     //Расстояния до края от центра
        Bitmap bitmap;
        byte[] bytesPic;
        double[,] matrix;

        public SpatialFiltering()
        {
            w = 0;  h = 0;
            r1 = 0; r2 = 0;
            bitmap = null;
            matrix = new double[0,0];
            bytesPic = new byte[0];
        }

        public SpatialFiltering(Bitmap bit, int _w, int _h, double[,] mat)
        {
            w = _w; 
            h = _h;
            r1 = (w - 1) / 2;
            r2 = (h - 1) / 2;
            bitmap = bit;
            bytesPic = ananas.ByteFromImage(bitmap);
            matrix = mat;
        }
        ~SpatialFiltering()
        {
            Dispose();
        }

        public void SetPicture(Bitmap _bit)     //устанавливает картинку
        {
            if (bitmap != null)
                bitmap.Dispose();
            bitmap = _bit;
            bytesPic = ananas.ByteFromImage(bitmap);
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

            int width = bitmap.Width;
            int height = bitmap.Height;

            for(int i = 0; i <= width; i++)         //Цикл прохода по картинке
                for(int j = 0; j < height; j++)
                {
                    byte[] pix;                     //Создаем массив для нового значения пикселя        
                    //PixProcessing(i, j,out pix);    //Обрабатываем пиксель и получаем новое значение
                }
            return bitmap;
        }
       //void PixProcessing(int x, int y, out byte[] pix)
       //{
       //    // BGR
       //
       //    for(int i = 0; i < w; i++)
       //        for (int j = 0; j < h; j++)
       //        {
       //            if(i + x - r1 < 0)
       //
       //        }
       //}
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
            bitmap.Dispose();
        }
    }
}
