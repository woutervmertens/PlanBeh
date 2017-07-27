using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PlanBeh
{
    static class Helpers
    {
        public static ImageSource ToImageSource(byte[] data)
        {
            var imageData = new BitmapImage();
            imageData.BeginInit();
            imageData.StreamSource = new MemoryStream(data);
            imageData.EndInit();
            imageData.Freeze();

            return imageData;
        }
    }
}
