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
using System.Drawing.Imaging;
//using System.Numerics;
using AForge.Math;

namespace ImgApp_2_WinForms
{
    public partial class Form1 : Form
    {
        private Bitmap mainImage = null;
        private Bitmap secondImage = null;
        private Bitmap result_image = null;
        private WorkingPictures workingPictures;
        private List<Bitmap> images = null;
        private Size sizeLul = new Size(90, 80);    //размер маленьких окошек
        private bool choice_R = false;
        private bool choice_G = false;
        private bool choice_B = false;
        public Form1()
        {
            InitializeComponent();
            mainImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            result_image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            secondImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            images = new List<Bitmap>();

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            workingPictures = new WorkingPictures(panel4, sizeLul);
            pictureBox1.Image = mainImage;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            SetInitLocationElements();
           // this.Size = new Size(1300,850);
        }
        private void SetInitLocationElements()
        {
            //955; 401
            panel5.Location = new Point(679, 279);
            //306; 65
            panel3.Location = new Point(306, 65);
            //140; 65
            panel2.Location = new Point(289, 65);
            //25; 65
            panel1.Location = new Point(25, 65);
            //449; 62
            panel6.Location = new Point(453, 60);
           // comboBox1.Location = new Point(1014, 495);
           // textBox1.Location = new Point(1014, 533);
           // button3.Location = new Point(1010, 569);
           // pictureBox4.Location = new Point(1014, 614);
           // button4.Location = new Point(1014, 764);
            
        }

        private void Open_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mainImage = new Bitmap(openFileDialog.FileName);    //считываем картинку
                workingPictures.AddNewPictureAndCheckBox(mainImage);    //создаем для картинки пикчрбокс
                pictureBox1.Image = (Bitmap)mainImage.Clone();                      //отображаем ее в основном окне
                workingPictures.SetElementsOnPanel();
                if (panel1.Visible == true)
                    GisDraw();
            }
            //if (pictureBox2.Image != null && pictureBox3.Image != null)      //проверка на максимальное количество картинок
            //{
            //    MessageBox.Show(
            //        "Открыто максимальное количество картинок"
            //        );
            //    return;
            //}
            //using OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            //openFileDialog.RestoreDirectory = true;
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    if (pictureBox2.Image == null)       //если есть место для первой картинки, заполняем его
            //    {
            //        InitPicture(openFileDialog, pictureBox2);
            //        if (panel1.Visible == true)
            //            GisDraw();
            //    }
            //    else                               //если есть место для второй картинки, заполняем его
            //    {
            //        InitPicture(openFileDialog, pictureBox3);
            //        if (panel1.Visible == true)
            //            GisDraw();
            //    }
            //}
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
            var time = DateTime.Now;
            
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    SummPix();
                    break;
                case 1:
                    ArithmeticMidlePix();
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
            System.GC.Collect();
            if (panel1.Visible == true)
                GisDraw();
            var time2 = DateTime.Now;
            Console.WriteLine(Math.Round((time2 - time).TotalMilliseconds) + "мс");
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
                if (mainImage != null)
                    mainImage.Dispose();
                mainImage = (Bitmap)images[0].Clone();

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
                if (mainImage != null)
                    mainImage.Dispose();
                mainImage = (Bitmap)images[1].Clone();

                if (panel1.Visible == true)
                {
                    GisDraw();
                    pictureBox5.Refresh();
                }
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (Bitmap)pictureBox4.Image.Clone();
            mainImage = (Bitmap)result_image.Clone();
            if (panel1.Visible == true)
            {
                GisDraw();
                pictureBox5.Refresh();
            }
        }
        private Size MaxSizeOfPictures(List<PictureBox> _pictureBoxes)
        {
            Size _size = new Size(0, 0);

            foreach (var pb in _pictureBoxes)
            {
                if (pb.Image.Width * pb.Image.Height >
                   _size.Width * _size.Height)
                    _size = pb.Image.Size;
            }

            return _size;
        }
        private void SetPictureBox1And4FromMainImage()
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = (Bitmap)mainImage.Clone();
            pictureBox1.Refresh();

            pictureBox4.Image = (Bitmap)mainImage.Clone();
            pictureBox4.Refresh();
        }

        #region pix_Operations

        private void ArithmeticMidlePix()
        {
            List<int> numbersCheckedBoxes = workingPictures.GetNumbersCheckedBoxes();
            if (numbersCheckedBoxes.Count == 0)
            {
                MessageBox.Show("Выберите две картинки");
            }
            else
            {
                List<PictureBox> pb = workingPictures.GetCheckedPictures();
                Size _size = MaxSizeOfPictures(pb);

                int[] allBytes = new int[_size.Width * _size.Height * 3];

                for (int k = 0; k < pb.Count; k++)
                {
                    Bitmap pic = new Bitmap((Bitmap)pb[k].Image, _size);
                    byte[] bytePic = ananas.ByteFromImage(pic);
                    for (int i = 0; i < _size.Width; i++)
                    {
                        for(int j = 0; j < _size.Height; j++)
                        {
                            allBytes[i * 3 + j * _size.Width * 3] += bytePic[i * 3 + j * _size.Width * 3];
                            allBytes[i * 3 + 1 + j * _size.Width * 3] += bytePic[i * 3 + 1 + j * _size.Width * 3];
                            allBytes[i * 3 + 2 + j * _size.Width * 3] += bytePic[i * 3 + 2 + j * _size.Width * 3];
                        }
                    }
                    pic.Dispose();
                }
                if (mainImage != null)
                    mainImage.Dispose();
                mainImage = new Bitmap(_size.Width, _size.Height);

                byte[] retultBytes = new byte[_size.Width * _size.Height * 3];
                for (int i = 0; i < _size.Width; i++)       
                {
                    for (int j = 0; j < _size.Height; j++)
                    {
                        allBytes[i * 3 + j * _size.Width * 3] /= pb.Count;
                        allBytes[i * 3 + 1 + j * _size.Width * 3] /= pb.Count;
                        allBytes[i * 3 + 2 + j * _size.Width * 3] /= pb.Count;

                        if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                            retultBytes[i * 3 + 2 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + 2 + j * _size.Width * 3]), 0, 255);
                        if (choice_G)
                            retultBytes[i * 3 + 1 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + 1 + j * _size.Width * 3]), 0, 255);
                        if (choice_B)
                            retultBytes[i * 3 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + j * _size.Width * 3]), 0, 255);
                    }
                }
                mainImage = ananas.ImageFromByte(retultBytes, _size.Width, _size.Height);
                SetPictureBox1And4FromMainImage();
            }
        }
        private void SummPix()
        {
            List<int> numbersCheckedBoxes = workingPictures.GetNumbersCheckedBoxes();
            if (numbersCheckedBoxes.Count == 0)
            {
                MessageBox.Show("Выберите две картинки");
            }
            else
            {
                List<PictureBox> pb = workingPictures.GetCheckedPictures();
                Size _size = MaxSizeOfPictures(pb);

                int[] allBytes = new int[_size.Width * _size.Height * 3];

                for (int k = 0; k < pb.Count; k++)
                {
                    Bitmap pic = new Bitmap((Bitmap)pb[k].Image.Clone(), _size);
                    byte[] bytePic = ananas.ByteFromImage(pic);
                    for (int i = 0; i < _size.Width; i++)
                    {
                        for (int j = 0; j < _size.Height; j++)
                        {
                            allBytes[i * 3 + j * _size.Width * 3] += bytePic[i * 3 + j * _size.Width * 3];
                            allBytes[i * 3 + 1 + j * _size.Width * 3] += bytePic[i * 3 + 1 + j * _size.Width * 3];
                            allBytes[i * 3 + 2 + j * _size.Width * 3] += bytePic[i * 3 + 2 + j * _size.Width * 3];
                        }
                    }
                    pic.Dispose();
                }
                
                if (mainImage != null)
                    mainImage.Dispose();
                mainImage = new Bitmap(_size.Width, _size.Height);

                byte[] retultBytes = new byte[_size.Width * _size.Height * 3];

                for (int i = 0; i < _size.Width; i++)
                {
                    for (int j = 0; j < _size.Height; j++)
                    {
                        if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                            retultBytes[i * 3 + 2 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + 2 + j * _size.Width * 3]), 0, 255);
                        if (choice_G)
                            retultBytes[i * 3 + 1 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + 1 + j * _size.Width * 3]), 0, 255);
                        if (choice_B)
                            retultBytes[i * 3 + j * _size.Width * 3] = (byte)Clamp((allBytes[i * 3 + j * _size.Width * 3]), 0, 255);
                    }
                }
                mainImage = ananas.ImageFromByte(retultBytes, _size.Width, _size.Height);
                SetPictureBox1And4FromMainImage();
            }
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

            byte[] retultBytes = ananas.ByteFromImage(mainImage);

            for (int i = 0; i < mainImage.Width; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < mainImage.Height; j++)
                {
                    double lul1 = koef * (retultBytes[i * 3 + 2 + j * mainImage.Width * 3]);
                    double lul2 = koef * (retultBytes[i * 3 + 1 + j * mainImage.Width * 3]);
                    double lul3 = koef * (retultBytes[i * 3 + j * mainImage.Width * 3]);


                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        retultBytes[i * 3 + 2 + j * mainImage.Width * 3] = (byte)Clamp(lul1, 0, 255);
                    if (choice_G)
                        retultBytes[i * 3 + 1 + j * mainImage.Width * 3] = (byte)Clamp(lul2, 0, 255);
                    if (choice_B)
                        retultBytes[i * 3 + j * mainImage.Width * 3] = (byte)Clamp(lul3, 0, 255);

                }
            }
            mainImage = ananas.ImageFromByte(retultBytes, mainImage.Width, mainImage.Height);
            SetPictureBox1And4FromMainImage();
        }
        private void MaxPix()
        {
            List<int> numbersCheckedBoxes = workingPictures.GetNumbersCheckedBoxes();
            if (numbersCheckedBoxes.Count == 0)
            {
                MessageBox.Show("Выберите две картинки");
            }
            else
            {
                List<PictureBox> pb = workingPictures.GetCheckedPictures();
                Size _size = MaxSizeOfPictures(pb);

                byte[] resultBytes = new byte[_size.Width * _size.Height * 3];
                
                for (int k = 0; k < pb.Count; k++)
                {
                    Bitmap pic = new Bitmap((Bitmap)pb[k].Image.Clone(), _size);
                    byte[] bytePic = ananas.ByteFromImage(pic);
                    for (int i = 0; i < _size.Width; i++)
                    {
                        for (int j = 0; j < _size.Height; j++)
                        {
                            if (k == 0)
                            {
                                resultBytes[i * 3 + 2 + j * _size.Width * 3] = bytePic[i * 3 + 2 + j * _size.Width * 3];
                                resultBytes[i * 3 + 1 + j * _size.Width * 3] = bytePic[i * 3 + 1 + j * _size.Width * 3];
                                resultBytes[i * 3 + j * _size.Width * 3] = bytePic[i * 3 + j * _size.Width * 3];
                            }
                            else
                            {
                                int abc = resultBytes[i * 3 + 2 + j * _size.Width * 3] +
                                          resultBytes[i * 3 + 1 + j * _size.Width * 3] +
                                          resultBytes[i * 3 + j * _size.Width * 3];
                                int abc2 = bytePic[i * 3 + 2 + j * _size.Width * 3] +
                                           bytePic[i * 3 + 1 + j * _size.Width * 3] +
                                           bytePic[i * 3 + j * _size.Width * 3];

                                if (abc < abc2)
                                {
                                    resultBytes[i * 3 + 2 + j * _size.Width * 3] = bytePic[i * 3 + 2 + j * _size.Width * 3];
                                    resultBytes[i * 3 + 1 + j * _size.Width * 3] = bytePic[i * 3 + 1 + j * _size.Width * 3];
                                    resultBytes[i * 3 + j * _size.Width * 3] = bytePic[i * 3 + j * _size.Width * 3];
                                }
                            }
                        }
                    }
                    pic.Dispose();
                }

                mainImage = ananas.ImageFromByte(resultBytes, _size.Width, _size.Height);
                SetPictureBox1And4FromMainImage();
            }
        }
        private void MinPix()
        {
            List<int> numbersCheckedBoxes = workingPictures.GetNumbersCheckedBoxes();
            if (numbersCheckedBoxes.Count == 0)
            {
                MessageBox.Show("Выберите две картинки");
            }
            else
            {
                List<PictureBox> pb = workingPictures.GetCheckedPictures();
                Size _size = MaxSizeOfPictures(pb);

                byte[] resultBytes = new byte[_size.Width * _size.Height * 3];

                for (int k = 0; k < pb.Count; k++)
                {
                    Bitmap pic = new Bitmap((Bitmap)pb[k].Image.Clone(), _size);
                    byte[] bytePic = ananas.ByteFromImage(pic);
                    for (int i = 0; i < _size.Width; i++)
                    {
                        for (int j = 0; j < _size.Height; j++)
                        {
                            if (k == 0)
                            {
                                resultBytes[i * 3 + 2 + j * _size.Width * 3] = bytePic[i * 3 + 2 + j * _size.Width * 3];
                                resultBytes[i * 3 + 1 + j * _size.Width * 3] = bytePic[i * 3 + 1 + j * _size.Width * 3];
                                resultBytes[i * 3 + j * _size.Width * 3] = bytePic[i * 3 + j * _size.Width * 3];
                            }
                            else
                            {
                                int abc = resultBytes[i * 3 + 2 + j * _size.Width * 3] +
                                          resultBytes[i * 3 + 1 + j * _size.Width * 3] +
                                          resultBytes[i * 3 + j * _size.Width * 3];
                                int abc2 = bytePic[i * 3 + 2 + j * _size.Width * 3] +
                                           bytePic[i * 3 + 1 + j * _size.Width * 3] +
                                           bytePic[i * 3 + j * _size.Width * 3];
                                
                                if(abc > abc2)
                                {
                                    resultBytes[i * 3 + 2 + j * _size.Width * 3] = bytePic[i * 3 + 2 + j * _size.Width * 3];
                                    resultBytes[i * 3 + 1 + j * _size.Width * 3] = bytePic[i * 3 + 1 + j * _size.Width * 3];
                                    resultBytes[i * 3 + j * _size.Width * 3] = bytePic[i * 3 + j * _size.Width * 3];
                                }
                            }
                        }
                    }
                    pic.Dispose();
                }

                mainImage = ananas.ImageFromByte(resultBytes, _size.Width, _size.Height);
                SetPictureBox1And4FromMainImage();
            }
        }

        #endregion
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //971; 458

            if (comboBox1.SelectedIndex == 4)
            {
                textBox1.Visible = true;
                //button3.Location = new Point(971, 498);
                //button4.Location = new Point(973, 643);
                //pictureBox4.Location = new Point(971, 536);

            }
            else
            {
                textBox1.Visible = false;

                //button3.Location = new Point(971, 498 - 35);
                //button4.Location = new Point(973, 643 - 35);
                //pictureBox4.Location = new Point(971, 536 - 35);
            }
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
            byte[] picBytes = ananas.ByteFromImage(mainImage);
            int[] N = new int[256];
            for (int i = 0; i < mainImage.Width; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < mainImage.Height; j++)
                {
                    //получаем пиксили картинок
                    var c = (picBytes[i * 3 + 2 + j * mainImage.Width * 3] +
                             picBytes[i * 3 + 1 + j * mainImage.Width * 3] +
                             picBytes[i * 3 + j * mainImage.Width * 3])
                             / 3;
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
            for (int i = 0; i < mainImage.Width; i++)         //Перебираем пиксели
                for (int j = 0; j < mainImage.Height; j++)
                {
                    var pix = mainImage.GetPixel(i, j);      //Считываем пиксель
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
                    mainImage.SetPixel(i, j, pix);
                }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)mainImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = mainImage;
            pictureBox1.Refresh();

            if (pictureBox6.Visible == true)
            {
                PaintGraphic();
                pictureBox6.Refresh();
            }

            if (panel1.Visible == true)
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
                if (pictureBox6.Image != null)
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
            for (int i = 0; i < n - 1; i++)
            {
                grap.DrawLine(pp, (float)X[i], 255 - (float)Y[i], (float)X[i + 1], 255 - (float)Y[i + 1]);
                grap.DrawRectangle(pp, (float)X[i], 255 - (float)Y[i], 3, 3);
            }
            grap.DrawRectangle(pp, (float)X[n - 1], 255 - (float)Y[n - 1], 3, 3);

            grap.Dispose();
            pp.Dispose();
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            // if (pictureBox7.Image != null)
            //     pictureBox7.Image.Dispose();
            //
            // pictureBox7.Image = new Bitmap(pictureBox7.Width, pictureBox7.Height);
            //
            // pictureBox7.MouseClick += new MouseEventHandler(lulJS);
            //
            // Graphics grap = Graphics.FromImage(pictureBox7.Image);
            // Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);
            // 
            // grap.DrawLine(pp, 0, 255, 255, 0);
            // grap.DrawRectangle(pp, 0, 255, 3, 3);
            // grap.DrawRectangle(pp, 255, 0, 3, 3);
            // 
            // grap.Dispose();
            // pp.Dispose();
        }

        private void lulJS(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Graphics grap = Graphics.FromImage(pictureBox7.Image);
            //Pen pp = new Pen(Color.FromArgb(0, 0, 0), 1);
            //
            //Point point = e.Location;
            //grap.DrawRectangle(pp, point.X, point.Y, 3, 3);
            //
            //pictureBox7.Refresh();
            //
            //grap.Dispose();
            //pp.Dispose();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
            else panel3.Visible = true;

        }

        private void button11_Click(object sender, EventArgs e)
        {
            int n = comboBox2.SelectedIndex;
            switch (n)
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
                        Nublek_Sauvola_Woolf(1);
                        break;
                    }
                case 3:     //Критерий Сауволы
                    {
                        Nublek_Sauvola_Woolf(2);
                        break;
                    }
                case 4:     //Критерий Кристиана Вульфа
                    {
                        Nublek_Sauvola_Woolf(3);
                        break;
                    }
                case 5:     //Критерий Брэдли-Рота
                    {
                        Bradly_Rot();
                        break;
                    }
            }
        }
        
        #region Binarizacia

        private void Otsu()
        {
            int w = mainImage.Width;
            int h = mainImage.Height;
            int[] N = new int[256];
            int all_pixel_count = w * h;
            int all_intensity_sum = 0; int k = 0;
            int[] tmas = new int[w * h + 1];
            byte[] bytePic = ananas.ByteFromImage(mainImage);
            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    //var pix1 = mainImage.GetPixel(j, i);
                    int r1 = bytePic[i * 3 + 2 + j * w * 3];
                    int g1 = bytePic[i * 3 + 1 + j * w * 3];
                    int b1 = bytePic[i * 3 + j * w * 3];
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
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (tmas[k] <= best_thresh)
                    {
                        bytePic[i * 3 + 2 + j * w * 3] = 0;
                        bytePic[i * 3 + 1 + j * w * 3] = 0;
                        bytePic[i * 3 + j * w * 3] = 0;
                    }
                    else
                    {
                        bytePic[i * 3 + 2 + j * w * 3] = 255;
                        bytePic[i * 3 + 1 + j * w * 3] = 255;
                        bytePic[i * 3 + j * w * 3] = 255;
                    }
                    k++;
                }
            }
            mainImage = ananas.ImageFromByte(bytePic, w, h);
            pictureBox1.Image = mainImage;
            pictureBox1.Refresh();

            result_image = (Bitmap)mainImage.Clone();
            pictureBox4.Image = result_image;
            pictureBox4.Refresh();
        }
        private void Gavrilov()
        {
            int w = mainImage.Width;
            int h = mainImage.Height;
            double mas = new double();
            double[,] lul = new double[w, h];
            byte[] bytePic = ananas.ByteFromImage(mainImage);
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    int r1 = bytePic[i * 3 + 2 + j * w * 3];
                    int g1 = bytePic[i * 3 + 1 + j * w * 3];
                    int b1 = bytePic[i * 3 + j * w * 3];
                    
                    lul[i, j] = (r1 + g1 + b1) / 3;
                    mas += (r1 + g1 + b1) / 3;        //Считаем его яркость
                }
            mas = (int)(mas / (w * h));

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (lul[i, j] >= mas)
                    {
                        bytePic[i * 3 + 2 + j * w * 3] = 255;
                        bytePic[i * 3 + 1 + j * w * 3] = 255;
                        bytePic[i * 3 + j * w * 3] = 255;
                    }
                        //mainImage.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                    {
                        bytePic[i * 3 + 2 + j * w * 3] = 0;
                        bytePic[i * 3 + 1 + j * w * 3] = 0;
                        bytePic[i * 3 + j * w * 3] = 0;
                    }
                        //mainImage.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            mainImage = ananas.ImageFromByte(bytePic, w, h);
            SetPictureBox1And4FromMainImage();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (panel5.Visible == false)
                panel5.Visible = true;
            else panel5.Visible = false;
        }

        private void Nublek_Sauvola_Woolf(int num)
        {
            int a = int.Parse(textBox4.Text);
            int del = a * a;
            double t = 0;
            int min = 256;
            int w = mainImage.Width;
            int h = mainImage.Height;
            int[] tmas = new int[h * w + 1];
            int[,] pmas = new int[w + 1, h + 1];
            double D, M2 = 0, maxo = 0;
            int k = 0;
            double[] M = new double[w * h + 1];
            double[] o = new double[w * h + 1];
            //Pix(pmas);
            byte[] bytePic = ananas.ByteFromImage(mainImage);
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    int r1 = bytePic[j * 3 + i * w * 3 + 2];
                    int g1 = bytePic[j * 3 + i * w * 3 + 1];
                    int b1 = bytePic[j * 3 + i * w * 3];

                    pmas[j, i] = (r1 + g1 + b1) / 3;     //Считаем его яркость
                }
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int ia = i - a / 2, 
                        ja = j - a / 2, 
                        i_a = i + a / 2, 
                        j_a = j + a / 2, 
                        ja1;
                    if (ia <= 0)    ia = 0;
                    if (i_a >= h)   i_a = h - 1;
                    if (ja <= 0)    ja = 0;
                    if (j_a >= w)   j_a = w - 1;
                    ja1 = ja;
                    int count = 0;
                    while (ia <= i_a)
                    {
                        while (ja <= j_a)
                        {
                            int p = pmas[ja, ia];
                            if (ia == i & ja == j) tmas[k] = p;
                            M[k] += p;
                            M2 += p * p;
                            if (p < min) min = p;
                            count++;
                            ja++;
                        }
                        ia++;
                        ja = ja1;
                    }
                    M[k] /= count;
                    M2 /= count;
                    D = M2 - M[k] * M[k];
                    o[k] = Math.Sqrt(D);
                    double sensitivity = double.Parse(textBox3.Text);
                    switch (num)
                    {
                        case 1:
                            {
                                t = (M[k] + sensitivity * o[k]);
                                break;
                            }
                        case 2:
                            {
                                t = (M[k] * (1 + sensitivity * (o[k] / 128 - 1)));
                                break;
                            }

                    }
                    if (o[k] > maxo) maxo = o[k];
                    if (num != 3)
                    {
                        //Color pix;
                        if (tmas[k] <= t)
                        {
                            //pix = Color.FromArgb(0, 0, 0);
                            bytePic[j * 3 + i * w * 3 + 2] = 0;
                            bytePic[j * 3 + i * w * 3 + 1] = 0;
                            bytePic[j * 3 + i * w * 3] = 0;
                        }
                        else
                        {
                            bytePic[j * 3 + i * w * 3 + 2] = 255;
                            bytePic[j * 3 + i * w * 3 + 1] = 255;
                            bytePic[j * 3 + i * w * 3] = 255;
                        }
                        //mainImage.SetPixel(j, i, pix);
                    }
                    k++;
                }
            }
            if (num == 3)
            {
                k = 0;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        t = (1 - 0.5) * M[k] + 0.5 * min + 0.5 * o[k] / maxo * (M[k] - min);
                        //Color pix;
                        if (tmas[k] <= t)
                        {
                            //pix = Color.FromArgb(0, 0, 0);
                            bytePic[j * 3 + i * w * 3 + 2] = 0;
                            bytePic[j * 3 + i * w * 3 + 1] = 0;
                            bytePic[j * 3 + i * w * 3] = 0;
                        }
                        else
                        {
                            bytePic[j * 3 + i * w * 3 + 2] = 255;
                            bytePic[j * 3 + i * w * 3 + 1] = 255;
                            bytePic[j * 3 + i * w * 3] = 255;
                            //pix = Color.FromArgb(255, 255, 255);
                        }
                        //mainImage.SetPixel(j, i, pix);
                        k++;
                    }
                }
            }
            mainImage = ananas.ImageFromByte(bytePic, w, h);
            SetPictureBox1And4FromMainImage();
            //Grey();
        }
        private void Bradly_Rot()
        {
            int w = mainImage.Width;
            int h = mainImage.Height;
            int[] tmas = new int[h * w + 1];
            int[,] pmas = new int[w + 1, h + 1]; int[,] S = new int[w + 1, h + 1];
            //Pix(pmas);
            byte[] bytePic = ananas.ByteFromImage(mainImage);
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    int r1 = bytePic[j * 3 + i * w * 3 + 2];
                    int g1 = bytePic[j * 3 + i * w * 3 + 1];
                    int b1 = bytePic[j * 3 + i * w * 3];

                    pmas[j, i] = (r1 + g1 + b1) / 3;     //Считаем его яркость
                }

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    S[j, i] += pmas[j, i];
                    if (j != 0 & i != 0) S[j, i] += S[j - 1, i] + S[j, i - 1] - S[j - 1, i - 1];
                    if (j == 0 & i != 0) S[j, i] += S[j, i - 1];
                    if (j != 0 & i == 0) S[j, i] += S[j - 1, i];
                }
            }
            int a = int.Parse(textBox4.Text);
            double k = double.Parse(textBox3.Text);
            //Pix(pmas);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int ia = i - a / 2, 
                        ja = j - a / 2, 
                        i_a = i + a / 2, 
                        j_a = j + a / 2; 
                    int x1, x2, y1, y2;
                    if (ia <= 0) ia = 0;
                    if (i_a >= h) i_a = h - 1;
                    if (ja <= 0) ja = 0;
                    if (j_a >= w) j_a = w - 1;
                    x1 = ja; x2 = j_a; y1 = ia; y2 = i_a;
                    int Sum = 0;
                    if (x1 != 0 & y1 != 0) Sum = S[x2, y2] + S[x1 - 1, y1 - 1] - S[x1 - 1, y2] - S[x2, y1 - 1];
                    if (x1 == 0 & y1 != 0) Sum = S[x2, y2] - S[x2, y1 - 1];
                    if (x1 != 0 & y1 == 0) Sum = S[x2, y2] - S[x1 - 1, y2];

                    //Color pix;
                    if (pmas[j, i] * a * a < Sum * (1 - k))
                    {
                        //pix = Color.FromArgb(0, 0, 0);
                        bytePic[j * 3 + i * w * 3 + 2] = 0;
                        bytePic[j * 3 + i * w * 3 + 1] = 0;
                        bytePic[j * 3 + i * w * 3] = 0;
                    }
                    else
                    {
                        //pix = Color.FromArgb(255, 255, 255);
                        bytePic[j * 3 + i * w * 3 + 2] = 255;
                        bytePic[j * 3 + i * w * 3 + 1] = 255;
                        bytePic[j * 3 + i * w * 3] = 255;
                    }
                    //workingImage.SetPixel(j, i, pix);
                }
            }
            mainImage = ananas.ImageFromByte(bytePic, w, h);
            SetPictureBox1And4FromMainImage();
            //Grey();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0 ||
               comboBox2.SelectedIndex == 1)
            {
                panel3.Size = new Size(128, 80);
                comboBox2.Size = new Size(100, 80);

                comboBox2.Location = new Point(14, 6);
                button11.Location = new Point(25, 40);

                label2.Visible = false;
                label3.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
            }
            else
            {
                //Size 195; 154
                //Location comboBox 43; 10
                //Location button 59; 118
                panel3.Size = new Size(195, 154);

                label2.Visible = true;
                label3.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;

                label3.Location = new Point(10, 50);
                label2.Location = new Point(10, 82);
                textBox3.Location = new Point(120, 48);
                textBox4.Location = new Point(120, 78);
                comboBox2.Location = new Point(43, 10);
                button11.Location = new Point(59, 118);

                switch (comboBox2.SelectedIndex)
                {
                    case 2:
                        {
                            textBox3.Text = "-0,2";
                            textBox4.Text = "20";
                            break;
                        }
                    case 3:
                        {
                            textBox3.Text = "0,2";
                            textBox4.Text = "20";
                            break;
                        }
                    case 4:
                        {
                            label3.Visible = false;
                            textBox4.Text = "20";
                            textBox3.Visible = false;
                            break;
                        }
                    case 5:
                        {
                            textBox3.Text = "0,15";
                            textBox4.Text = "20";
                            break;
                        }
                    default:
                        {
                            textBox3.Text = "";
                            textBox4.Text = "";
                            break;
                        }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                choice_R = true;
                choice_G = true;
                choice_B = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                choice_R = true;
                choice_G = false;
                choice_B = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                choice_R = false;
                choice_G = true;
                choice_B = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                choice_R = false;
                choice_G = false;
                choice_B = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                choice_R = true;
                choice_G = true;
                choice_B = false;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                choice_R = true;
                choice_G = false;
                choice_B = true;
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                choice_R = false;
                choice_G = true;
                choice_B = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (panel6.Visible == false)
                panel6.Visible = true;
            else
                panel6.Visible = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int o = int.Parse(textBox6.Text);
            int w = int.Parse(textBox5.Text);
            int h = int.Parse(textBox7.Text);

            double[,] mat = GetMatrix(o, w, h);
            SpatialFiltering spatialFiltering = new SpatialFiltering((Bitmap)mainImage.Clone(), w, h, mat);
            double[,] test =
            {
                {-1 , -1, -1 },
                {-1 , 9,  -1 },
                {-1 , -1, -1 }
            };

            //spatialFiltering.SetMatrix(3,3,test);
            mainImage = (Bitmap)spatialFiltering.ApplyFilter().Clone();
            //spatialFiltering.SetPicture(mainImage);
            SetPictureBox1And4FromMainImage();
            spatialFiltering.Dispose();
        }
        double[,] GetMatrix(int o, int  w, int h)
        {
            double[,] mat = new double[h, w]; 
            int r1 = (w - 1) / 2;
            int r2 = (h - 1) / 2;
            double s = 0;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double g = 1.0 / (2.0 * Math.PI * o * o) * Math.Exp(-1.0 * ((i - r2) * (i - r2) + (j - r1) * (j - r1)) / (2.0 * o * o));
                    s += g;
                    mat[i, j] = g;
                    Console.Write(g + "    ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Sum: " + s);
            return mat;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button13_Click_DeleteCheckedImages(object sender, EventArgs e)
        {
            workingPictures.DeleteCheckedImages();
            workingPictures.SetElementsOnPanel();
            panel4.Refresh();
        }


        private void button16_Click_1(object sender, EventArgs e)
        {
            panel7.Visible = true;
            panel6.Visible = false;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int w = int.Parse(textBox8.Text);
            int h = int.Parse(textBox9.Text);

            
            for(int j = 0; j < w; j++)
            {
                dataGridView2.Columns.Add("name" + j, j.ToString());
            }
            for (int i = 0; i < h; i++)
                dataGridView2.Rows.Add();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            textBox8.Text = "";
            textBox9.Text = "";

            panel7.Visible = false;
            panel6.Visible = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int w = int.Parse(textBox8.Text);
            int h = int.Parse(textBox9.Text);
            double[,] mat = new double[h, w];

            for(int i = 0; i < h; i++)
                for(int j = 0; j < w; j++)
                {
                    mat[i, j] = Convert.ToDouble(dataGridView2[j, i].Value);
                }
            SpatialFiltering spatialFiltering = new SpatialFiltering((Bitmap)mainImage.Clone(), w, h, mat);
            
            mainImage = (Bitmap)spatialFiltering.ApplyFilter().Clone();
            //spatialFiltering.SetPicture(mainImage);
            SetPictureBox1And4FromMainImage();
            spatialFiltering.Dispose();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Bitmap test = new Bitmap(mainImage, 256, 512);
            byte[] lul = ananas.ByteFromImage(test);
            int w = test.Width;
            int h = test.Height;

            Complex[] pointsR = new Complex[w];
            Complex[] pointsG = new Complex[w];
            Complex[] pointsB = new Complex[w];

            Complex[,] MorepointsR = new Complex[h,w];
            Complex[,] MorepointsG = new Complex[h,w];
            Complex[,] MorepointsB = new Complex[h,w];

            for(int i = 0; i < h; i++)
                for(int j = 0; j < w; j++)
                {
                    MorepointsR[i, j] = new Complex(lul[i * w * 3 + j * 3 + 2] * Math.Pow((-1), i + j),0);
                    MorepointsG[i, j] = new Complex(lul[i * w * 3 + j * 3 + 1] * Math.Pow((-1), i + j),0);
                    MorepointsB[i, j] = new Complex(lul[i * w * 3 + j * 3]* Math.Pow((-1), i + j),0);
                }
           
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    pointsR[j] = MorepointsR[i, j];
                    pointsG[j] = MorepointsG[i, j];
                    pointsB[j] = MorepointsB[i, j];
                }
                FourierTransform.FFT(pointsR,FourierTransform.Direction.Backward);
                FourierTransform.FFT(pointsG,FourierTransform.Direction.Backward);
                FourierTransform.FFT(pointsB,FourierTransform.Direction.Backward);

                for(int j = 0; j < w; j++)
                {
                    MorepointsR[i, j] = pointsR[j];
                    MorepointsG[i, j] = pointsG[j];
                    MorepointsB[i, j] = pointsB[j];
                }
            }
            
            pointsR = new Complex[h];
            pointsG = new Complex[h];
            pointsB = new Complex[h];

            for (int j = 0; j < w; j++)
            {
                for(int i = 0; i < h; i++)
                {
                    pointsR[i] = MorepointsR[i, j];
                    pointsG[i] = MorepointsG[i, j];
                    pointsB[i] = MorepointsB[i, j];
                }
                FourierTransform.FFT(pointsR, FourierTransform.Direction.Backward);
                FourierTransform.FFT(pointsG, FourierTransform.Direction.Backward);
                FourierTransform.FFT(pointsB, FourierTransform.Direction.Backward);

                for (int i = 0; i < h; i++)
                {
                    MorepointsR[i, j] = pointsR[i];
                    MorepointsG[i, j] = pointsG[i];
                    MorepointsB[i, j] = pointsB[i];
                }
            }

           // for (int i = 0; i < h; i++)
           //     for (int j = 0; j < w; j++)
           //     {
           //         MorepointsR[i, j] *= Math.Pow((-1), i + j);
           //         MorepointsG[i, j] *= Math.Pow((-1), i + j);
           //         MorepointsB[i, j] *= Math.Pow((-1), i + j);
           //     }


            double buf;
            double maxR = 0;
            double maxG = 0;
            double maxB = 0;
            #region Эта залупа кое-как работает
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    if (maxR < Math.Log(Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                  MorepointsR[i, j].Im * MorepointsR[i, j].Im)))
                    
                        maxR = Math.Log(Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                  MorepointsR[i, j].Im * MorepointsR[i, j].Im));
            
                    if (maxG < Math.Log(Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                  MorepointsG[i, j].Im * MorepointsG[i, j].Im)))
            
                        maxG = Math.Log(Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                  MorepointsG[i, j].Im * MorepointsG[i, j].Im));
            
                    if (maxB < Math.Log(Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                  MorepointsB[i, j].Im * MorepointsB[i, j].Im)))
            
                        maxB = Math.Log(Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                  MorepointsB[i, j].Im * MorepointsB[i, j].Im));
                }

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    buf = 255 * Math.Log(Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                   MorepointsR[i, j].Im * MorepointsR[i, j].Im)) / maxR;
                    lul[i * w * 3 + j * 3 + 2] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3 + 2] = 255;

                    buf = 255 * Math.Log(Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                   MorepointsG[i, j].Im * MorepointsG[i, j].Im)) / maxG;
                    lul[i * w * 3 + j * 3 + 1] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3 + 1] = 255;

                    buf = 255 * Math.Log(Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                   MorepointsB[i, j].Im * MorepointsB[i, j].Im)) / maxB;
            
                    lul[i * w * 3 + j * 3] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3] = 255;
                }
            #endregion

            #region Эта залупа поинтересней
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    if (maxR < Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                  MorepointsR[i, j].Im * MorepointsR[i, j].Im))

                        maxR = Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                  MorepointsR[i, j].Im * MorepointsR[i, j].Im);

                    if (maxG < Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                  MorepointsG[i, j].Im * MorepointsG[i, j].Im))

                        maxG = Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                  MorepointsG[i, j].Im * MorepointsG[i, j].Im);

                    if (maxB < Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                  MorepointsB[i, j].Im * MorepointsB[i, j].Im))

                        maxB = Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                  MorepointsB[i, j].Im * MorepointsB[i, j].Im);
                }

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    buf = 25000 * Math.Sqrt(MorepointsR[i, j].Re * MorepointsR[i, j].Re +
                                                   MorepointsR[i, j].Im * MorepointsR[i, j].Im) / maxR;
                    lul[i * w * 3 + j * 3 + 2] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3 + 2] = 255;

                    buf = 25000 * Math.Sqrt(MorepointsG[i, j].Re * MorepointsG[i, j].Re +
                                                   MorepointsG[i, j].Im * MorepointsG[i, j].Im) / maxG;
                    lul[i * w * 3 + j * 3 + 1] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3 + 1] = 255;
                    
                    buf = 25000 * Math.Sqrt(MorepointsB[i, j].Re * MorepointsB[i, j].Re +
                                                   MorepointsB[i, j].Im * MorepointsB[i, j].Im) / maxB;

                    lul[i * w * 3 + j * 3] = (byte)buf;
                    if (buf > 255)
                        lul[i * w * 3 + j * 3] = 255;
                }
            #endregion

            mainImage = ananas.ImageFromByte(lul,256,512);
            SetPictureBox1And4FromMainImage();
            //FurieTrans furieTrans = new FurieTrans();

            //furieTrans.dpf(pointsR, pointsG, pointsB, pointsR.Count());
            int z = 0;
        }

        #endregion
        //private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBox2.SelectedIndex == 0 ||
        //       comboBox2.SelectedIndex == 1)
        //    {
        //        panel3.Size = new Size(128, 80);
        //        comboBox2.Size = new Size(100, 80);
        //
        //        comboBox2.Location = new Point(14, 6);
        //        button11.Location = new Point(25, 40);
        //
        //        label2.Visible = false;
        //        label3.Visible = false;
        //        textBox3.Visible = false;
        //        textBox4.Visible = false;
        //    }
        //    else
        //    {
        //        //Size 195; 154
        //        //Location comboBox 43; 10
        //        //Location button 59; 118
        //        panel3.Size = new Size(223, 161);
        //
        //        label2.Visible = true;
        //        label3.Visible = true;
        //        textBox3.Visible = true;
        //        textBox4.Visible = true;
        //
        //        label2.Location = new Point(10, 50);
        //        label3.Location = new Point(10, 82);
        //        textBox3.Location = new Point(120, 48);
        //        textBox4.Location = new Point(120, 78);
        //        comboBox2.Location = new Point(43, 10);
        //        button11.Location = new Point(12, 125);
        //        button12.Location = new Point(120, 125);
        //    }
        //
        //    switch(comboBox2.SelectedIndex)
        //    {
        //        case 2:
        //            {
        //                textBox3.Text = "-0,2";
        //                textBox4.Text = "20";
        //                break;
        //            }
        //        case 3:
        //            {
        //                textBox3.Text = "0,2";
        //                textBox4.Text = "20";
        //                break;
        //            }
        //        case 4:
        //            {
        //                label2.Visible = false;
        //                textBox3.Visible = false;
        //                textBox4.Text = "20";
        //                break;
        //            }
        //        case 5:
        //            {
        //                textBox3.Text = "0,15";
        //                textBox4.Text = "20";
        //                break;
        //            }
        //        default:
        //            {
        //                textBox3.Text = "";
        //                textBox4.Text = "";
        //                break;
        //            }
        //    }
        //}
        //
        //private void button12_Click(object sender, EventArgs e)
        //{
        //    dataGridView1.Rows.Clear();
        //    dataGridView1.Refresh();
        //    textBox2.Text = "";
        //    if (pictureBox6.Visible == true)
        //    {
        //        if (pictureBox6.Image != null)
        //            pictureBox6.Image.Dispose();
        //        pictureBox6.Image = new Bitmap(pictureBox6.Width, pictureBox6.Height);
        //    }
        //}
    }
}