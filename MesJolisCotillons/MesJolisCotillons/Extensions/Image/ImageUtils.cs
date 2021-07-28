using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Extensions
{
    public static class ImageUtils
    {
        public static MemoryStream CompressImage(System.Drawing.Image img, long quality, ImageCodecInfo codec)
        {
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            var ms = new MemoryStream();
            img.Save(ms, codec, parameters);

            return ms;
        }

        public static MemoryStream CompressImage2(System.Drawing.Image img, long quality, ImageCodecInfo codec = null) 
        {
            ImageCodecInfo jpgEncoder = ImageCodecInfo.GetImageEncoders()[0];
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            var ms = new MemoryStream();
            img.Save(ms, jpgEncoder, myEncoderParameters);

            return ms;
        }

        public static MemoryStream CompressJpegImage(System.Drawing.Image image, long quality)
        {
            ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders()
                .Where(r => r.CodecName.ToUpperInvariant().Contains("JPEG"))
                .Select(r => r).FirstOrDefault();

            var encoder = System.Drawing.Imaging.Encoder.Quality;
            var parameters = new EncoderParameters(1);
            var parameter = new EncoderParameter(encoder, quality);
            parameters.Param[0] = parameter;

            var ms = new MemoryStream();
            image.Save(ms, codecInfo, parameters);
            return ms;
        }
    }
}