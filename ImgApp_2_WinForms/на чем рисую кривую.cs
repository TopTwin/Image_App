
//ќчень просто - это наследник класса Panel
//добавить его очень просто в конструкторе формы:
//
//  Form1() 
//   { ...
//     InitializeComponent();  //ќб€зательно после этой ф-ции
//     ...
//     var canvas = new pan();
//     canvas.Size = new Size(500,500);
//     canvas.Location = new Point(0, 0);
//     ...
//   }


using System.Drawing;
using System.Windows.Forms;

public partial class pan : System.Windows.Forms.Panel
{
    public pan()
    {
        //настраиваем стель дл€ плавного рисовани€
        this.SetStyle(
            System.Windows.Forms.ControlStyles.UserPaint |
            System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
            System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
            true);

        //прикрепл€ем методы к событи€м
        //событие отрисовки
        Paint += p_event;

        //событи€ мыши
        //перехватываем клики, смотрим координаты, 
        //создаем массивы с точками, рисуем, 
        //интерполируем, итд.
        MouseDown += Pan_MouseDown;
        MouseUp += Pan_MouseUp;
        MouseMove += Pan_MouseMove;

        //включаем посто€нную перерисовку по таймеру
        //не совсем оптимальный вариант, все врем€ рисовать на виджите
        //но дл€ сделанного на коленке пойдет
        
        Timer y = new Timer();
        y.Interval = 30;
        y.Tick += (s, a) => { this.Refresh(); };

        VisibleChanged += (s, a) => { y.Start(); };

    }

    private void Pan_MouseMove(object sender, MouseEventArgs e)
    {
        //e->Location - координаты мыши в рамках окна
    }

    private void Pan_MouseUp(object sender, MouseEventArgs e)
    {

    }

    private void Pan_MouseDown(object sender, MouseEventArgs e)
    {

        if (e.Button == MouseButtons.Left)     {       }
        else if (e.Button == MouseButtons.Right)
        {  }

    }

    public void p_event(object sender, System.Windows.Forms.PaintEventArgs e)
    {
        //событие отрисовки вызываетс€, когда ќ— дает окну команду на перересовку.

        //“ут уже знакомый нам Graphics
        //все что на нем рисуетс€ - отобразитс€ на форме в процессе перерисовки
        e.Graphics.FillRectangle(Brushes.Red,0,0,Size.Width,Size.Height);
    }
}

//–истограмма рисуетс€ немного по другому.
//≈сть Bitmap который прикреплен к pictureBox
//гистограмма рисуетс€ именно на нем (на битмапе),
//дл€ того, чтобы pictureBox скушал изменени€ на битмапе, его надо перересовать
//   pictureBox.Refresh();

//дл€ ускорени€ обработки картинки проходил все байты в массиве байтов изоюражени€ через
//паралельный For
//https://docs.microsoft.com/ru-ru/dotnet/api/system.threading.tasks.parallel.for


    