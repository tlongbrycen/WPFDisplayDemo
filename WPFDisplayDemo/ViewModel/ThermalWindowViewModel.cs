using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
namespace WPFDisplayDemo.ViewModel
{
    class ThermalWindowViewModel
    {
        public Image thermalImage;
        public ImageSource imgSource;
        public const int IMAGE_WIDTH = 100;
        public const int IMAGE_HEIGHT = 100;

        public ThermalWindowViewModel()
        {
            thermalImage = new Image();
            //OnLoadFromDummy();
            imgSource = new BitmapImage(new Uri(@"C:\Users\tlong\source\repos\WPFDisplayDemo\WPFDisplayDemo\Assets\sapa.jpg"));
        }

        public byte[] GetDummyData()
        {
            var rand = new Random();
            byte[] t = new byte[IMAGE_WIDTH * IMAGE_HEIGHT];
            for(int i = 0; i < IMAGE_WIDTH * IMAGE_HEIGHT; i++ )
            {
                t[i] = (byte)rand.Next(255);
            }
            return t;
        }

        public Bitmap GetDummyBitmap(byte[] data)
        {
            Bitmap bmp = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, 
                System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public BitmapSource ArrayToBitmapSource(byte[] data, int w, int h, int ch)
        {
            System.Windows.Media.PixelFormat format = PixelFormats.Default;

            if (ch == 1) format = PixelFormats.Gray8; //grey scale image 0-255
            if (ch == 3) format = PixelFormats.Bgr24; //RGB
            if (ch == 4) format = PixelFormats.Bgr32; //RGB + alpha


            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, ch * w, 0);

            return wbm;
        }

        public BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        }

        public void OnLoadFromDummy()
        {
            byte[] data = GetDummyData();
            BitmapSource bitmapSource = ArrayToBitmapSource(data,
                IMAGE_WIDTH, IMAGE_HEIGHT, 1);
            BitmapImage bmpImg = BitmapSourceToBitmapImage(bitmapSource);
            thermalImage.Source = bmpImg;
        }
    }
}
