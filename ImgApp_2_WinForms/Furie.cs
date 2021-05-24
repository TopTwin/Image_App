using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace ImgApp_2_WinForms
{
    /// <summary> 
    /// Преобразование Фурье 
    /// </summary> 
    public class FurieTrans
    {
        /// <summary> 
        /// Коэффициенты 
        /// </summary> 
        public List<Complex> koeffsR;
        public List<Complex> koeffsG;
        public List<Complex> koeffsB;

        /// <summary> 
        /// Количество точек, которые закодированы в данном ДПФ 
        /// </summary> 
        public double K;

        public FurieTrans()
        {
            koeffsR = new List<Complex>();
            koeffsG = new List<Complex>();
            koeffsB = new List<Complex>();
        }

        /// <summary> 
        /// Выполнить дискретное преобразование Фурье 
        /// </summary> 
        /// <param name="points">Точки контура</param> 
        /// <param name="count">Количество коэффициентов</param> 
        public void dpf(List<Point> pointsR, List<Point> pointsG, List<Point> pointsB, int count)
        {
            koeffsR.Clear();
            koeffsG.Clear();
            koeffsB.Clear();
            K = pointsR.Count;

            //Цикл вычисления коэффициентов 
            for (int u = 0; u < count; u++)
            {
                //цикл суммы 
                Complex summaR = new Complex();
                Complex summaG = new Complex();
                Complex summaB = new Complex();
                for (int k = 0; k < K; k++)
                {
                    Complex SR = new Complex(pointsR[k].X, pointsR[k].Y);
                    Complex SG = new Complex(pointsG[k].X, pointsG[k].Y);
                    Complex SB = new Complex(pointsB[k].X, pointsB[k].Y);
                    double koeff = -2 * Math.PI * u * k / K;
                    Complex e = new Complex(Math.Cos(koeff), Math.Sin(koeff));
                    summaR += (SR * e);
                    summaG += (SG * e);
                    summaB += (SB * e);
                }
                koeffsR.Add(summaR / K);
                koeffsG.Add(summaG / K);
                koeffsB.Add(summaB / K);
            }
        }

        /// <summary> 
        /// Обратное преобразование Фурье 
        /// </summary> 
        /// <returns>Точки</returns> 
        public void undpf()
        {
            List<Complex> resR = new List<Complex>();
            List<Complex> resG = new List<Complex>();
            List<Complex> resB = new List<Complex>();
            for (int k = 0; k < K; k++)
            {
                Complex summaR = new Complex();
                Complex summaG = new Complex();
                Complex summaB = new Complex();
                for (int u = 0; u < koeffsR.Count; u++)
                {
                    double koeff = 2 * Math.PI * u * k / K;
                    Complex e = new Complex(Math.Cos(koeff), Math.Sin(koeff));
                    summaR += (koeffsR[u] * e);
                    summaG += (koeffsG[u] * e);
                    summaB += (koeffsB[u] * e);
                }
                resR.Add(summaR);
                resG.Add(summaG);
                resB.Add(summaB);
            }
            koeffsR = resR;
            koeffsG = resG;
            koeffsB = resB;
        }
    }

    public class FFT
    {
        /// <summary>
        /// Вычисление поворачивающего модуля e^(-i*2*PI*k/N)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private static Complex w(int k, int N)
        {
            if (k % N == 0) return 1;
            double arg = -2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }
        /// <summary>
        /// Возвращает спектр сигнала
        /// </summary>
        /// <param name="x">Массив значений сигнала. Количество значений должно быть степенью 2</param>
        /// <returns>Массив со значениями спектра сигнала</returns>
        public static Complex[] fft(Complex[] x)
        {
            Complex[] X;
            int N = x.Length;
            if (N == 2)
            {
                X = new Complex[2];
                X[0] = x[0] + x[1];
                X[1] = x[0] - x[1];
            }
            else
            {
                Complex[] x_even = new Complex[N / 2];
                Complex[] x_odd = new Complex[N / 2];
                for (int i = 0; i < N / 2; i++)
                {
                    x_even[i] = x[2 * i];
                    x_odd[i] = x[2 * i + 1];
                }
                Complex[] X_even = fft(x_even);
                Complex[] X_odd = fft(x_odd);
                X = new Complex[N];
                for (int i = 0; i < N / 2; i++)
                {
                    X[i] = X_even[i] + w(i, N) * X_odd[i];
                    X[i + N / 2] = X_even[i] - w(i, N) * X_odd[i];
                }
            }
            return X;
        }
        /// <summary>
        /// Центровка массива значений полученных в fft (спектральная составляющая при нулевой частоте будет в центре массива)
        /// </summary>
        /// <param name="X">Массив значений полученный в fft</param>
        /// <returns></returns>
        public static Complex[] nfft(Complex[] X)
        {
            int N = X.Length;
            Complex[] X_n = new Complex[N];
            for (int i = 0; i < N / 2; i++)
            {
                X_n[i] = X[N / 2 + i];
                X_n[N / 2 + i] = X[i];
            }
            return X_n;
        }
    }

}
