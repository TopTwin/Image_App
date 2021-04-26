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
        private Bitmap workingImage = null;
        private Bitmap secondImage = null;
        private Bitmap result_image = null;
        private List<Bitmap> images = null;
        private bool choice_R = false;
        private bool choice_G = false;
        private bool choice_B = false;
        public Form1()
        {
            InitializeComponent();
            workingImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            result_image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            secondImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            images = new List<Bitmap>();

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            pictureBox1.Image = workingImage;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
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
                    workingImage = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(workingImage);                              //добавляем картинку в память
                    pictureBox1.Image = workingImage;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    workingImage = new Bitmap(workingImage, new Size(pictureBox2.Width, pictureBox2.Height));
                    pictureBox2.Image = workingImage;                      //отображаем ее в основном окне
                    workingImage = (Bitmap)images[0].Clone();
                    if (panel1.Visible == true)
                        GisDraw();
                }
                else                               //если есть место для второй картинки, заполняем его
                {
                    workingImage = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(workingImage);                              //добавляем в память новую картинку
                    pictureBox1.Image = workingImage;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    workingImage = new Bitmap(workingImage, new Size(pictureBox3.Width, pictureBox3.Height));
                    pictureBox3.Image = workingImage;                      //отображаем ее в основном окне
                    workingImage = (Bitmap)images[1].Clone();
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
                        workingImage = (Bitmap)images[0].Clone();          //берем за основу большую картинку
                        secondImage = new Bitmap(images[1]);           //в доп переменную кладем вторую
                    }
                    else
                    {
                        workingImage = (Bitmap)images[1].Clone();          //тоже самое что чуть выше
                        secondImage = new Bitmap(images[0]);
                    }
                    //увеличим меньшую картинку до размеров большой
                    secondImage = new Bitmap(secondImage, new Size(workingImage.Width, workingImage.Height));
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
                if (workingImage != null)
                    workingImage.Dispose();
                workingImage = (Bitmap)images[0].Clone();

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
                if (workingImage != null)
                    workingImage.Dispose();
                workingImage = (Bitmap)images[1].Clone();

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
                if (workingImage != null)
                    workingImage.Dispose();
                pictureBox1.Image = result_image;
                workingImage = (Bitmap)result_image.Clone();
            }

            if (panel1.Visible == true)
            {
                GisDraw();
                pictureBox5.Refresh();
            }
        }
        private void ArithmeticMeanPix()
        {

            for (int i = 0; i < workingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Width; j++)
                {
                    var pix1 = workingImage.GetPixel(j, i);        //получаем пиксили картинок
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
                    workingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
        }
        private void SummPix()
        {
            for (int i = 0; i < workingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Width; j++)
                {
                    var pix1 = workingImage.GetPixel(j, i);        //получаем пиксили картинок
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
                    workingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
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
            for (int i = 0; i < workingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Width; j++)
                {
                    var pix1 = workingImage.GetPixel(j, i);        //получаем пиксили картинок

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
                    workingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
        }
        private void MaxPix()
        {
            for (int i = 0; i < workingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Width; j++)
                {
                    var pix1 = workingImage.GetPixel(j, i);        //получаем пиксили картинок
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
                    workingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
        }
        private void MinPix()
        {
            for (int i = 0; i < workingImage.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Width; j++)
                {
                    var pix1 = workingImage.GetPixel(j, i);        //получаем пиксили картинок
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
                    workingImage.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
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
            for (int i = 0; i < workingImage.Width; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < workingImage.Height; j++)
                {
                    var pix1 = workingImage.GetPixel(i, j);        //получаем пиксили картинок

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
            LineInterpolation lineInterpolation = new LineInterpolation(n, X, Y);
            int R = 0;
            int G = 0;
            int B = 0;
            for (int i = 0; i < workingImage.Width; i++)         //Перебираем пиксели
                for (int j = 0; j < workingImage.Height; j++)
                {
                    var pix = workingImage.GetPixel(i, j);      //Считываем пиксель
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
                    workingImage.SetPixel(i, j, pix);
                }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
            
            if(pictureBox6.Visible == true)
            {
                PaintGraphic();
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
                PaintGraphic();
            }
            else
            {
                pictureBox6.Visible = false;
            }
        }

        private void PaintGraphic()
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

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox7.Image != null)
                pictureBox7.Image.Dispose();
           
            pictureBox7.Image = new Bitmap(pictureBox7.Width, pictureBox7.Height);
           
            pictureBox7.MouseClick += new MouseEventHandler(lulJS);
           
            Graphics grap = Graphics.FromImage(pictureBox7.Image);
            Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);
            
            grap.DrawLine(pp, 0, 255, 255, 0);
            grap.DrawRectangle(pp, 0, 255, 3, 3);
            grap.DrawRectangle(pp, 255, 0, 3, 3);
            
            grap.Dispose();
            pp.Dispose();
        }


        private void lulJS(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Graphics grap = Graphics.FromImage(pictureBox7.Image);
            Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);

            Point point = e.Location;
            grap.DrawRectangle(pp, point.X, point.Y, 3, 3);

            pictureBox7.Refresh();

            grap.Dispose();
            pp.Dispose();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
            else panel3.Visible = true;

            if(comboBox2.SelectedIndex == 1 || 
               comboBox2.SelectedIndex == 2)
            {
                comboBox2.Size = new Size(132, 80);
                comboBox2.Location = new Point(16, 6);
                button11.Location = new Point(29, 42);
            }
            else
            {

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int n = comboBox2.SelectedIndex;
            switch(n)
            {
                case 0:     //Критерий Гаврилова
                    {
                        Gavrilov();
                        break;
                    }
                case 1:     //Критерий Отсу
                    {
                        Otsu();
                        break;
                    }
                case 2:     //Критерий Ниблека
                    {

                        break;
                    }
                case 3:     //Критерий Сауволы
                    {

                        break;
                    }
                case 4:     //Критерий Кристиана Вульфа
                    {

                        break;
                    }
                case 5:     //Критерий Брэдли-Рота
                    {

                        break;
                    }
            }
                
        }
        private void Otsu()
        {
            int w = workingImage.Width;
            int h = workingImage.Height;
            int[] N = new int[256];
            int all_pixel_count = w * h;
            int all_intensity_sum = 0; int k = 0;
            int[] tmas = new int[w * h + 1];
            for (int i = 0; i < h; ++i)
            {
                for (int j = 0; j < w; ++j)
                {
                    var pix1 = workingImage.GetPixel(j, i);
                    int r1 = pix1.R;
                    int g1 = pix1.G;
                    int b1 = pix1.B;
                    all_intensity_sum += (r1 + g1 + b1) / 3;
                    tmas[k] = (r1 + g1 + b1) / 3; k++;
                    N[(r1 + g1 + b1) / 3]++;
                }
            }


            int best_thresh = 0;
            double best_sigma = 0.0;

            int first_class_pixel_count = 0;
            int first_class_intensity_sum = 0;

            // Перебираем границу между классами
            // thresh < INTENSITY_LAYER_NUMBER - 1, т.к. при 255 в ноль уходит знаменатель внутри for
            for (int thresh = 0; thresh < 255; ++thresh)
            {
                first_class_pixel_count += N[thresh];
                first_class_intensity_sum += thresh * N[thresh];

                double first_class_prob = first_class_pixel_count / (double)all_pixel_count;
                double second_class_prob = 1.0 - first_class_prob;

                double first_class_mean = first_class_intensity_sum / (double)first_class_pixel_count;
                double second_class_mean = (all_intensity_sum - first_class_intensity_sum)
                    / (double)(all_pixel_count - first_class_pixel_count);

                double mean_delta = first_class_mean - second_class_mean;

                double sigma = first_class_prob * second_class_prob * mean_delta * mean_delta;

                if (sigma > best_sigma)
                {
                    best_sigma = sigma;
                    best_thresh = thresh;
                }
            }
            k = 0;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Color pix;
                    if (tmas[k] <= best_thresh)
                    {
                        pix = Color.FromArgb(0, 0, 0);
                    }
                    else
                    {
                        pix = Color.FromArgb(255, 255, 255);
                    }
                    workingImage.SetPixel(j, i, pix); k++;
                }
            }

            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();

            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox4.Refresh();
        }
        private void Gavrilov()
        {
            double mas = new double();
            double[,] lul = new double[workingImage.Width,workingImage.Height];

            for(int i = 0; i < workingImage.Width; i++)
                for(int j = 0; j < workingImage.Height; j++)
                {
                    var pix = workingImage.GetPixel(i, j);      //Считываем пиксель
                    lul[i, j] = (pix.R + pix.B + pix.G) / 3;
                    mas += (pix.R + pix.B + pix.G) / 3;        //Считаем его яркость
                }
            mas = (int)(mas / (workingImage.Width * workingImage.Height));

            for (int i = 0; i < workingImage.Width; i++)
                for (int j = 0; j < workingImage.Height; j++)
                {
                    if (lul[i, j] >= mas)
                        workingImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        workingImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }

            
            pictureBox1.Image = (Bitmap)workingImage;
            pictureBox1.Refresh();
           
            result_image = (Bitmap)workingImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox4.Refresh();
        }
    }
}
