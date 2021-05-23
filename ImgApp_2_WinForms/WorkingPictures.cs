using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgApp_2_WinForms
{
    class WorkingPictures
    {
        List<PictureBox> pictureBoxes;
        List<CheckBox> checkBoxes;         
        Panel panel;
        Size size;
        public WorkingPictures(Panel _panel, Size sizePictureBox)
        {
            pictureBoxes = new List<PictureBox>();
            checkBoxes = new List<CheckBox>();
            panel = _panel;
            size = sizePictureBox;
        }
        public List<PictureBox> GetCheckedPictures()
        {
            List<PictureBox> _pictureBoxes = new List<PictureBox>();
            for (int i = 0; i < checkBoxes.Count; i++)
                if (checkBoxes[i].Checked)
                {
                    _pictureBoxes.Add(pictureBoxes[i]);
                }
            return _pictureBoxes;
        }
        public List<PictureBox> GetPictureBoxes()
        {
            return pictureBoxes;
        }
        
        public List<int> GetNumbersCheckedBoxes()
        {
            List<int> ppp = new List<int>();
            for (int i = 0; i < checkBoxes.Count; i++)
                if (checkBoxes[i].Checked)
                    ppp.Add(i);
            return ppp;
        }
        public void AddNewPictureAndCheckBox(Image _image)
        {

            PictureBox pictureBox = new PictureBox();       //создаем новый пикчрбокс
            pictureBox.Size = size;                         //задаем размер
            pictureBox.Visible = false;                         //задаем размер
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Image = (Bitmap)_image.Clone();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxes.Add(pictureBox);                   //добавляем в список
            panel.Controls.Add(pictureBox);                 //закрепляем за панель

            CheckBox checkBox = new CheckBox();
            checkBox.Visible = false;                       //создаем новый чекбокс
            checkBoxes.Add(checkBox);                       //добавляем в список
            panel.Controls.Add(checkBox);                   //закрепляем за панель
        }
        public void SetElementsOnPanel()
        {
            int margin_top = 15; //px
            int marginLeftForPictureBox = 30; //px
            int marginLeftForCheckBox = 10; //px

            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                Point point = new Point();
                point.X = marginLeftForPictureBox;
                point.Y = i * (size.Height + margin_top) + margin_top;
                pictureBoxes[i].Location = point;
                pictureBoxes[i].Visible = true;

                point.X = marginLeftForCheckBox;
                point.Y = -10 + margin_top + (size.Height / 2) + i * size.Height;
                checkBoxes[i].Location = point;
                checkBoxes[i].Visible = true;
            }
        }
        public void DeleteCheckedImages()
        {
            List<PictureBox> del = GetCheckedPictures();
            for(int i =0; i < del.Count; i++)
            {
                int index = pictureBoxes.IndexOf(del[i]);
                del[i].Dispose();
                pictureBoxes.RemoveAt(index);
                checkBoxes[index].Dispose();
                checkBoxes.RemoveAt(index);
            }
        }
    }
}
