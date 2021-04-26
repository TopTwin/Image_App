using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgApp_2_WinForms
{
    class GradationGraphic: System.Windows.Forms.PictureBox
    {
        List<double> points_X;
        List<double> points_Y;
        int n;
        public GradationGraphic()
        {
            points_X = new List<double>();
            points_Y = new List<double>();
            n = points_X.Count();
        }
        private void AddPoint(object sender, MouseEventArgs e)
        {
            float[] pp = new float[2] { e.Location.X, e.Location.Y };
            points_X.Add(pp[0]);
            points_Y.Add(pp[1]);
            n = points_X.Count();
            points_X.Sort();
            points_Y.Sort();
        }
        private void PaintGraphic(object sender, MouseEventArgs e)
        {
            double[] X = ListToDouble(points_X);
            double[] Y = ListToDouble(points_Y);
            
            LineInterpolation lineInterpolation = new LineInterpolation(n, X, Y);
          
            Graphics grap = Graphics.FromImage(this.Image);
            Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);
            for (int i = 0; i < n - 1; i++)
            {
                grap.DrawLine(pp, (float)X[i], (float)(255 - Y[i]), (float)X[i + 1], (float)(255 - Y[i + 1]));
                grap.DrawRectangle(pp, (float)X[i], (float)(255 - Y[i]), 3, 3);
            }
            grap.DrawRectangle(pp, (float)X[n - 1], (float)(255 - Y[n - 1]), 3, 3);

            grap.Dispose();
            pp.Dispose();
        }
        private double[] ListToDouble(List<double> vs)
        {
            if (vs == null)
                return new double[0];

            double[] dd = new double[vs.Count()];
            for(int i = 0; i < vs.Count(); i++)
            {
                dd[i] = vs[i];
            }
            return dd;
        }
        public void Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AddPoint(sender, e);
                PaintGraphic(sender, e);
            }
        }
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

    }
}
