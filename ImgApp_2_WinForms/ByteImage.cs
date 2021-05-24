/*
Суть более быстрой работы в том, что мы будет трогать напрямую байты изображения, без медленного посредника в лице Bitmap
*/
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

class ananas
{
    //Допустим, пишем какуюто функцию обработки изображений.
    public static byte[] ByteFromImage(Bitmap input)
    {
        int width = input.Width;
        int height = input.Height;
        //создаем временное изображние с нужным нам форматом хранения.
        //так как обработка побайтовая, там важно расположение байтов в картинке.
        //а оно опеределено форматом хранения

        byte[] input_bytes = new byte[0]; //пустой массивчик байт

        //по этому создадим новый битмап с нужным нам 3х байтовым форматом.
        using (Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
        {
            //устанавливаем DPI такой же как у исходного
            _tmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);

            //рисуем исходное изображение на временном, "типо-копируем"
            using (var g = Graphics.FromImage(_tmp))
            {
                g.DrawImageUnscaled(input, 0, 0);
            }
            input_bytes = getImgBytes(_tmp); //получаем байты изображения, см. описание ф-ции ниже
        }
        return input_bytes;
    }
    public static Bitmap ImageFromByte(byte[] bytes, int width, int height)
    {
        if (width <= 0 || height <= 0)
            return null;
        else
        {
            Bitmap img_ret = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            writeImageBytes(img_ret, bytes);

            return img_ret;
        }
    }
    public static Bitmap Image8bbpFromByte(byte[] bytes, int width, int height)
    {
        if (width <= 0 || height <= 0)
            return null;
        else
        {
            Bitmap img_ret = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            writeImageBytes(img_ret, bytes);

            return img_ret;
        }
    }
    //public static Bitmap SomeFunction(Bitmap input)
    //{
    //    int width = input.Width;
    //    int height = input.Height;
    //    //создаем временное изображние с нужным нам форматом хранения.
    //    //так как обработка побайтовая, там важно расположение байтов в картинке.
    //    //а оно опеределено форматом хранения
    //
    //    byte[] input_bytes = new byte[0]; //пустой массивчик байт
    //
    //    //по этому создадим новый битмап с нужным нам 3х байтовым форматом.
    //    using (Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
    //    {
    //        //устанавливаем DPI такой же как у исходного
    //        _tmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);
    //
    //        //рисуем исходное изображение на временном, "типо-копируем"
    //        using (var g = Graphics.FromImage(_tmp))
    //        {
    //            g.DrawImageUnscaled(input, 0, 0);
    //        }
    //        input_bytes = getImgBytes(_tmp); //получаем байты изображения, см. описание ф-ции ниже
    //
    //    }
    //
    //    /*
    //        Вот на этом моменте у нам в массиве input_bytes лежит побайтовая копия исходной картинки.
    //        в формате BGR-BGR-BGR-BGR-BGR-BGR (обратите внимание, цвета хранятся наоборот)
    //
    //        Обработка картинки таким образом В РАЗЫ быстрее, чем через Bitmap                
    //
    //     */
    //
    //
    //    //Допустим, мы обработаки картинку и сложили результат сюда:
    //    byte[] bytes = new byte[width * height * 3];
    //
    //
    //
    //    //Теперь надо сложить новые байты в битмап, 
    //    //создаем выходное изображние (отбратите внимание, без using!!, иначе будет нечего возвращать)
    //    Bitmap img_ret = new Bitmap(width, height, PixelFormat.Format24bppRgb);
    //    img_ret.SetResolution(input.HorizontalResolution, input.VerticalResolution);
    //
    //    writeImageBytes(img_ret, bytes);
    //
    //    return img_ret;
    //}
    //по хорошему, написанную фунцкцию надо вызывать так
    /*
        using (var b = SomeFunction(...)   вот он, потерянный using.
        {
                ...
        }

     */

    static byte[] getImgBytes(Bitmap img)
    {
        byte[] bytes = new byte[img.Width * img.Height * 3];  //выделяем память под массив байтов
        var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),  //блокируем участок памати, занимаемый изображением
            ImageLockMode.ReadOnly,
            img.PixelFormat);
        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);  //копируем байты изображения в массив
        img.UnlockBits(data);   //разблокируем изображение
        return bytes; //возвращаем байты
    }

    static void writeImageBytes(Bitmap img, byte[] bytes)
    {
        var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),  //блокируем участок памати, занимаемый изображением
            ImageLockMode.WriteOnly,
            img.PixelFormat);
        Marshal.Copy(bytes, 0, data.Scan0, bytes.Length); //копируем байты массива в изображение

        img.UnlockBits(data);  //разблокируем изображение
    }
}