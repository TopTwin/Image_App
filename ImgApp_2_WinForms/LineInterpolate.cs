using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgApp_2_WinForms
{

    class LineInterpolation
    {
        double[] X;
        double[] Y;
        int N;
        public LineInterpolation(int n, double[] x, double[] y)
        {
            X = x;
            Y = y;
            N = n;
        }
        public double Interpolate(double x)
        {
            int i = SearchInterval(x);
            if (i == 0)
                return 0;
            double a = Calculation_A(i);
            double b = Calculation_B(i, a);
            return Calculation_Y(a, b, x);
        }

        private double Calculation_Y(double a,double b,double x)
        {
            return (a * x + b);
        }
        private int SearchInterval(double x)
        {
            int m = 0;
            if (x < X[0] || x > X[N-1])
                return 0;
            for (int i = 1; i < N; i++)
            {
                if (x >= X[i - 1] && x <= X[i])
                    m = i;
            }
            return m;
        }

        private double Calculation_A(int i)
        {
            return (Y[i] - Y[i - 1]) / (X[i] - X[i - 1]);
        }

        private double Calculation_B(int i,double a)
        {
            return (Y[i - 1] - a * X[i - 1]);
        }
    }
}
