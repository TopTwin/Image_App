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
        private Bitmap working_image = null;
        private Bitmap additional_image = null;
        private Bitmap result_image = null;
        private List<Bitmap> images = null;
        private bool choice_R = false;
        private bool choice_G = false;
        private bool choice_B = false;
        public Form1()
        {
            InitializeComponent();
            working_image = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            result_image = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            additional_image = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            images = new List<Bitmap>();
            pictureBox1.Image = working_image;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            if(pictureBox2.Image != null && pictureBox3.Image != null)      //проверка на максимальное количество картинок
            {
                MessageBox.Show(
                    "Открыто максимальное количество картинок"
                    );
                return;
            }
            using OpenFileDialog openFileDialog  = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(pictureBox2.Image == null)       //если есть место для первой картинки, заполняем его
                {
                    working_image = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(working_image);                              //добавляем картинку в память
                    pictureBox1.Image = working_image;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    working_image = new Bitmap(working_image, new Size(pictureBox2.Width, pictureBox2.Height));
                    pictureBox2.Image = working_image;                      //отображаем ее в основном окне
                }
                else                               //если есть место для второй картинки, заполняем его
                {
                    working_image = new Bitmap(openFileDialog.FileName);    //считываем картинку
                    images.Add(working_image);                              //добавляем в память новую картинку
                    pictureBox1.Image = working_image;                      //отображаем ее в маленькой рамке справа
                    //изменяем размер картинки под маленькое окно
                    working_image = new Bitmap(working_image, new Size(pictureBox3.Width, pictureBox3.Height));
                    pictureBox3.Image = working_image;                      //отображаем ее в основном окне
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
            if(images.Count == 2)
            {
                var time = DateTime.Now;

                Size size_1 = images[0].Size;               //размеры картинок
                Size size_2 = images[1].Size;

                if(comboBox1.SelectedIndex < 4)
                {
                    if (size_1.Width * size_1.Height > size_2.Width * size_2.Height)    //ищем наибольшую картинку
                    {
                        working_image = (Bitmap)images[0].Clone();          //берем за основу большую картинку
                        additional_image = new Bitmap(images[1]);           //в доп переменную кладем вторую
                    }
                    else
                    {
                        working_image = (Bitmap)images[1].Clone();          //тоже самое что чуть выше
                        additional_image = new Bitmap(images[0]);
                    }
                    //увеличим меньшую картинку до размеров большой
                    additional_image = new Bitmap(additional_image, new Size(working_image.Width, working_image.Height));
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
                additional_image.Dispose();
                var time2 = DateTime.Now;
                Console.WriteLine(Math.Round((time2 - time).TotalMilliseconds) + "мс");
            }
        }
        public static T Clamp<T>(T val, T min, T max) where T: IComparable<T>
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
                if (working_image != null)
                    working_image.Dispose();
                working_image = (Bitmap)images[0].Clone();
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (images.Count >= 2)
            {
                if (working_image != null)
                    working_image.Dispose();
                pictureBox1.Image = images[1];
                working_image = (Bitmap)images[1].Clone();
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (result_image != null)
            {
                if (working_image != null)
                    working_image.Dispose();
                pictureBox1.Image = result_image;
                working_image = (Bitmap)result_image.Clone();
            }
        }
        private void ArithmeticMeanPix()
        {                
            
            for (int i = 0; i < working_image.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < working_image.Width; j++)
                {
                    var pix1 = working_image.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = additional_image.GetPixel(j, i);

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
                    working_image.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)working_image.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = working_image;
            pictureBox1.Refresh();
        }
        private void SummPix()
        {
            for (int i = 0; i < working_image.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < working_image.Width; j++)
                {
                    var pix1 = working_image.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = additional_image.GetPixel(j, i);

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
                    working_image.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)working_image.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = working_image;
            pictureBox1.Refresh();
        }
        private void CompositionPix()
        {
            double koef = 1;
            try
            {
                koef = Convert.ToDouble(textBox1.Text);
            }
            catch(Exception)
            {
                MessageBox.Show(
                    "Неверный формат",
                    "Ошибка"
                    );
                return;
            }
            for (int i = 0; i < working_image.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < working_image.Width; j++)
                {
                    var pix1 = working_image.GetPixel(j, i);        //получаем пиксили картинок

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
                    working_image.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)working_image.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = working_image;
            pictureBox1.Refresh();
        }
        private void MaxPix()
        {
            for (int i = 0; i < working_image.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < working_image.Width; j++)
                {
                    var pix1 = working_image.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = additional_image.GetPixel(j, i);

                    int R = 0;
                    int G = 0;
                    int B = 0;

                    if (choice_R)           //обрабатываем цвета в зависимости от выбранного цветового канала
                        if((pix1.R > pix2.R))
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
                    working_image.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)working_image.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = working_image;
            pictureBox1.Refresh();
        }
        private void MinPix()
        {
            for (int i = 0; i < working_image.Height; i++)      //цикл перебора пикселей
            {
                for (int j = 0; j < working_image.Width; j++)
                {
                    var pix1 = working_image.GetPixel(j, i);        //получаем пиксили картинок
                    var pix2 = additional_image.GetPixel(j, i);

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
                    working_image.SetPixel(j, i, pix1);
                }
            }
            if (result_image != null)
                result_image.Dispose();
            result_image = (Bitmap)working_image.Clone();
            pictureBox4.Image = result_image;
            pictureBox1.Image = working_image;
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
            if(radioButton1.Checked)
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
    }
}
