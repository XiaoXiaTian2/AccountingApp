using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingApp.Utility
{
    internal static class ImgCompressor
    {
        public static Image ResizeImage(Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(img, 0, 0, width, height);
            }

            return bitmap;
        }

        public static Image GetCompressedImage(Image img, long quality = 10L)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(img.RawFormat);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(myEncoder, quality);

            MemoryStream mst = new MemoryStream();
            img.Save(mst, jpgEncoder, encoderParams);
            Image nImg = Image.FromStream(mst);

            return nImg;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
