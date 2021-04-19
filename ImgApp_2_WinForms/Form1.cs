using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgApp_2_WinForms
{
    public partial class Form1 : Form
    {
        private Bitmap WorkingImage = null;
        private Bitmap secondImage = null;
        private Bitmap result_image = null;
        private List<Bitmap> images = null;
        private bool choice_R = false;
        private bool choice_G = false;
        private bool choice_B = false;
        public Form1()
        {
            InitializeComponent();
            WorkingImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            result_image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            secondImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            images = new List<Bitmap>();

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            pictureBox1.Image = WorkingImage;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null && pictureBox3.Image != null)      //проверка на максимальное количество картинок
            {
                MessageBox.Show(
                    "Открыто максимальное количество картинок"
                    );
                return;
            }
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox2.Image == null)       //если есть место для первой картинки, заполняем его
                {
                    WorkingImage = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(WorkingImage);                              //добавляем картинку в память
                    pictureBox1.Image = WorkingImage;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    WorkingImage = new Bitmap(WorkingImage, new Size(pictureBox2.Width, pictureBox2.Height));
                    pictureBox2.Image = WorkingImage;                      //отображаем ее в основном окне
                    WorkingImage = (Bitmap)images[0].Clone();
                    if (panel1.Visible == true)
                        GisDraw();
                }
                else                               //если есть место для второй картинки, заполняем его
                {
                    WorkingImage = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(WorkingImage);                              //добавляем в память новую картинку
                    pictureBox1.Image = WorkingImage;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    WorkingImage = new Bitmap(WorkingImage, new Size(pictureBox3.Width, pictureBox3.Height));
                    pictureBox3.Image = WorkingImage;                      //отображаем ее в основном окне
                    WorkingImage = (Bitmap)images[1].Clone();
                    if (panel1.Visible == true)
                        GisDraw();
                }
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileFialog = new SaveFileDialog();
            saveFileFialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveFileFialog.RestoreDirectory = true;

            if (saveFileFialog.ShowDialog() == DialogResult.OK)
            {
                if (result_image != null)
                {
                    result_image.Save(saveFileFialog.FileName);
                }
            }
        }
        private void Draw_Click(object sender, EventArgs e)
        {
            if (images.Count == 2)
            {
                var time = DateTime.Now;

                Size size_1 = images[0].Size;               //размеры картинок
                Size size_2 = images[1].Size;

                if (comboBox1.SelectedIndex < 4)
                {
                    if (size_1.Width * size_1.Height > size_2.Width * size_2.Height)    //ищем наибольшую картинку
                    {
                        WorkingImage = (Bitmap)images[0].Clone();          //берем за основу большую картинку
                        secondImage = new Bitmap(images[1]);           //в доп переменную кладем вторую
                    }
                    else
                    {
                        WorkingImage = (Bitmap)images[1].Clone();          //тоже самое что чуть выше
                        secondImage = new Bitmap(images[0]);
                    }
                    //увеличим меньшую картинку до размеров большой
                    secondImage = new Bitmap(secondImage, new Size(WorkingImage.Width, WorkingImage.Height));
                }

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        SummPix();
                        break;
                    case 1:
                        ArithmeticMeanPix();
                        break;
                    case 2:
                        MinPix();
                        break;
                    case 3:
                        MaxPix();
                        break;
                    case 4:
                        CompositionPix();
                        break;
                }
                if (panel1.Visible == true)
                    GisDraw();
                secondImage.Dispose();
                var time2 = DateTime.Now;
                Console.WriteLine(Math.Round((time2 - time).TotalMilliseconds) + "мс");
            }
        }
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (images.Count >= 1)
            {
                pictureBox1.Image = images[0];
                if (WorkingImage != null)
                    WorkingImage.Dispose();
                WorkingImage = (Bitmap)images[0].Clone();

                if (panel1.Visible == true)
                {
                    GisDraw();
                    pictureBox5.Refresh();
                }
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (images.Count >= 2)
            {
                pictureBox1.Image = images[1];
                if (WorkingImage != null)
                    WorkingImage.Dispose();
                WorkingImage = (Bitmap)images[1].Clone();

                if (panel1.Visible == true)
                {
                    GisDraw();
                    pictureBox5.Refresh();
                }
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (result_image != null)
            {
                if (WorkingImage != null)
                    WorkingImage.Dispose();
                pictureBox1.Image = result_image;
                WorkingImage = (Bitmap)result_image.Clone();
            }

            if (panel1.Visible == true)
            {
                GisDraw();
                pictureBox5.Refresh();
            }
        }
        private void ArithmeticMeanPix()
        {

            for (int i = 0; i < WorkingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Width; j++)
                {
                    var pix1 = WorkingImage.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = secondImage.GetPixel(j, i);

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        R = Clamp((pix1.R + pix2.R) / 2, 0, 255);
                    if (choice_G)
                        G = Clamp((pix1.G + pix2.G) / 2, 0, 255);
                    if (choice_B)
                        B = Clamp((pix1.B + pix2.B) / 2, 0, 255);

                    pix1 = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();
        }
        private void SummPix()
        {
            for (int i = 0; i < WorkingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Width; j++)
                {
                    var pix1 = WorkingImage.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = secondImage.GetPixel(j, i);

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        R = Clamp((pix1.R + pix2.R), 0, 255);
                    if (choice_G)
                        G = Clamp((pix1.G + pix2.G), 0, 255);
                    if (choice_B)
                        B = Clamp((pix1.B + pix2.B), 0, 255);

                    pix1 = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();
        }
        private void CompositionPix()
        {
            double koef = 1;
            try
            {
                koef = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Неверный формат",
                    "Ошибка"
                    );
                return;
            }
            for (int i = 0; i < WorkingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Width; j++)
                {
                    var pix1 = WorkingImage.GetPixel(j, i);        //получаем пиксили картинок

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        R = Clamp((int)(pix1.R * koef), 0, 255);
                    if (choice_G)
                        G = Clamp((int)(pix1.G * koef), 0, 255);
                    if (choice_B)
                        B = Clamp((int)(pix1.B * koef), 0, 255);

                    pix1 = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();
        }
        private void MaxPix()
        {
            for (int i = 0; i < WorkingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Width; j++)
                {
                    var pix1 = WorkingImage.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = secondImage.GetPixel(j, i);

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        if ((pix1.R > pix2.R))
                            R = Clamp((pix1.R), 0, 255);
                        else
                            R = Clamp((pix2.R), 0, 255);

                    if (choice_G)
                        if ((pix1.G > pix2.G))
                            G = Clamp((pix1.G), 0, 255);
                        else
                            G = Clamp((pix2.G), 0, 255);

                    if (choice_B)
                        if (pix1.B > pix2.B)
                            B = Clamp((pix1.B), 0, 255);
                        else
                            B = Clamp((pix2.B), 0, 255);

                    pix1 = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();
        }
        private void MinPix()
        {
            for (int i = 0; i < WorkingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Width; j++)
                {
                    var pix1 = WorkingImage.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = secondImage.GetPixel(j, i);

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        if ((pix1.R < pix2.R))
                            R = Clamp((pix1.R), 0, 255);
                        else
                            R = Clamp((pix2.R), 0, 255);

                    if (choice_G)
                        if ((pix1.G < pix2.G))
                            G = Clamp((pix1.G), 0, 255);
                        else
                            G = Clamp((pix2.G), 0, 255);

                    if (choice_B)
                        if (pix1.B < pix2.B)
                            B = Clamp((pix1.B), 0, 255);
                        else
                            B = Clamp((pix2.B), 0, 255);

                    pix1 = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();
        }
        private void R_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                choice_R = true;
                choice_G = false;
                choice_B = false;
            }
        }

        private void RGB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                choice_R = true;
                choice_G = true;
                choice_B = true;
            }
        }

        private void G_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                choice_R = false;
                choice_G = true;
                choice_B = false;
            }
        }

        private void B_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                choice_R = false;
                choice_G = false;
                choice_B = true;
            }
        }

        private void RG_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                choice_R = true;
                choice_G = true;
                choice_B = false;
            }
        }

        private void RB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                choice_R = true;
                choice_G = false;
                choice_B = true;
            }
        }

        private void GB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                choice_R = false;
                choice_G = true;
                choice_B = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 4)
                textBox1.Visible = true;
            else
                textBox1.Visible = false;
        }

        private void Gistogramm(object sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true;
                GisDraw();
            }
            else panel1.Visible = false;
        }

        private void GisDraw()
        {
            if (pictureBox5.Image != null)
                pictureBox5.Image.Dispose();

            pictureBox5.Image = new Bitmap(pictureBox5.Width, pictureBox5.Height);

            int[] N = new int[256];
            for (int i = 0; i < WorkingImage.Width; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < WorkingImage.Height; j++)
                {
                    var pix1 = WorkingImage.GetPixel(i, j);        //получаем пиксили картинок

                    var c = (pix1.R + pix1.B + pix1.G) / 3;
                    N[c]++;
                }
            }
            int max = N.Max();
            float k = (float)pictureBox5.Height / max;
            Graphics grap = Graphics.FromImage(pictureBox5.Image);
            float[] a = new float[2];
            float[] b = new float[2];
            Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);

            for (int i = 0; i <= 255; i++)
            {
                a[0] = i; a[1] = pictureBox5.Height - 1;
                b[0] = i; b[1] = pictureBox5.Height - 1 - N[i] * k;
                grap.DrawLine(pp, a[0], a[1], b[0], b[1]);
            }

            grap.Dispose();
            pp.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }
            else panel2.Visible = false;
            pictureBox6.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.RowCount;     //Количество точек
            double[] X = new double[n];         //массивы значений Х и У
            double[] Y = new double[n];
            for (int i = 0; i < n; i++)          //Считываем значения Х и У
            {
                X[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                Y[i] = Convert.ToDouble(dataGridView1[1, i].Value);
            }
            LineInterpolation lineInterpolation = new LineInterpolation(n, X, Y);     //Создаем сплайн
            int R = 0;
            int G = 0;
            int B = 0;
            for (int i = 0; i < WorkingImage.Width; i++)         //Перебираем пиксели
                for (int j = 0; j < WorkingImage.Height; j++)
                {
                    var pix = WorkingImage.GetPixel(i, j);      //Считываем пиксель
                    double c = (pix.R + pix.B + pix.G) / 3;        //Считаем его яркость
                    double sum = pix.R + pix.B + pix.G;
                    double k = lineInterpolation.Interpolate(c);    //Производим градационные преобразования
                    if (c != 0)
                    {
                        k = k / c;
                        R = (int)Clamp((pix.R * k), 0, 255);                          //Считаем новые значения цветов пикселя
                        G = (int)Clamp((pix.G * k), 0, 255);
                        B = (int)Clamp((pix.B * k), 0, 255);
                    }
                    else
                    {
                        R = (int)Clamp((k), 0, 255);                          //Считаем новые значения цветов пикселя
                        G = (int)Clamp((k), 0, 255);
                        B = (int)Clamp((k), 0, 255);
                    }
                    pix = Color.FromArgb(R, G, B);
                    WorkingImage.SetPixel(i, j, pix);
                }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)WorkingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = WorkingImage;
            pictureBox1.Refresh();

            if(pictureBox6.Visible == true)
            {
                PaitGraphic();
                pictureBox6.Refresh();
            }

            if(panel1.Visible == true)
            {
                GisDraw();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            textBox2.Text = "";
            if (pictureBox6.Visible == true)
            {
                if(pictureBox6.Image != null)
                    pictureBox6.Image.Dispose();
                pictureBox6.Image = new Bitmap(pictureBox6.Width, pictureBox6.Height);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var str = textBox2.Text;
            int n = 0;
            try
            {
                n = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                MessageBox.Show("Введите корректное число");
                return;
            }
            for (int i = 0; i < n; i++)
                dataGridView1.Rows.Add();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (pictureBox6.Visible == false)
            {
                pictureBox6.Visible = true;
                PaitGraphic();
            }
            else
            {
                pictureBox6.Visible = false;
            }
        }

        private void PaitGraphic()
        {
            if (pictureBox6.Image != null)
                pictureBox6.Image.Dispose();

            pictureBox6.Image = new Bitmap(pictureBox6.Width, pictureBox6.Height);

            int n = dataGridView1.RowCount;     //Количество точек
            double[] X = new double[n];         //массивы значений Х и У
            double[] Y = new double[n];
            for (int i = 0; i < n; i++)          //Считываем значения Х и У
            {
                X[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                Y[i] = Convert.ToDouble(dataGridView1[1, i].Value);
            }

            var str = textBox2.Text;
            n = 0;
            try
            {
                n = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                return;
            }
            
            Graphics grap = Graphics.FromImage(pictureBox6.Image);
            Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);
            for (int i = 0; i < n-1; i++)
            {
                grap.DrawLine(pp, (float)X[i], 255 - (float)Y[i], (float)X[i+1], 255 - (float)Y[i+1]);
                grap.DrawRectangle(pp, (float)X[i], 255 - (float)Y[i], 3, 3);
            }
            grap.DrawRectangle(pp, (float)X[n - 1], 255 - (float)Y[n - 1], 3, 3);

            grap.Dispose();
            pp.Dispose();
        }
    }
}
