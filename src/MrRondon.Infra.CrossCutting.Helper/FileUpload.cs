using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MrRondon.Infra.CrossCutting.Helper
{
    public class FileUpload
    {
        private const int AcceptableSize = 2;
        private static readonly string[] AcceptableExtensions = { ".png", ".jpeg", ".jpg", ".jfif" };

        public static byte[] GetBytes(HttpPostedFileBase file, string property)
        {
            IsValidImage(file, property);

            var compressedImage = CompressImage(file.InputStream, 60, file.FileName);
            return compressedImage;
        }

        public static bool IsValidImage(HttpPostedFileBase file, string property)
        {
            if (file == null) throw new Exception($"Nenhuma imagem para {property} foi informada");
            var extension = GetExtension(file);
            if (AcceptableExtensions.All(a => a != extension))
                throw new Exception($"Apenas esses tipos de imagens são aceitas: {string.Join(",", AcceptableExtensions).ToUpper()}.");

            var fileSize = ConvertBytesToMegabytes(file.ContentLength);
            if (fileSize > AcceptableSize)
                throw new Exception($"O tamanho máximo aceito é de {AcceptableSize}MB");

            return true;
        }

        public static string GetExtension(HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName);
        }

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static byte[] CompressImage(Stream stream, long quality, string filepath)
        {
            var param = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, quality) }
            };
            var image = Image.FromStream(stream);
            var codec = GetCodec(image.RawFormat);

            using (var ms = new MemoryStream())
            {
                image.Save(ms, codec, param);
                //image.Save($@"C:\Users\GT\Pictures\\new_{filepath}", codec, param);
                var result = ms.ToArray();
                ms.Dispose();

                return result;
            }
        }

        private static ImageCodecInfo GetCodec(ImageFormat formato)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            var codec = codecs.FirstOrDefault(c => c.FormatID == formato.Guid);
            if (codec == null) throw new NotSupportedException();
            return codec;
        }
    }
}