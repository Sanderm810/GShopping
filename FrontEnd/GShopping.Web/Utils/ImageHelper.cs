using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Tutor.Base.Core.Utils;

namespace GShopping.Web.Utils
{
    public class ImageHelper
    {
        private const int ThumbnailWidth = 438;
        private const int ThumbnailHeight = 250;

        public static byte[] CreateThumbnail(string base64mage)
        {
            if (!string.IsNullOrWhiteSpace(base64mage))
            {
                var base64Content = DataUriPattern.Base64Content(base64mage);
                return CreateThumbnail(base64Content);
            }
            throw new Exception("Base64 não informado!");
        }

        public static byte[] CreateThumbnail(Base64Content base64ContentImage)
        {
            return CreateThumbnail(base64ContentImage.Bytes);
        }

        public static byte[] CreateThumbnail(byte[] bytesImage)
        {
            if (bytesImage != null)
            {
                return ResizeImage(bytesImage, ThumbnailWidth, ThumbnailHeight);
            }
            throw new Exception("Base64 não atende o formato esperado!");
        }

        private static byte[] ResizeImage(byte[] originalImage, int width, int height)
        {
            if (originalImage != null && originalImage.Length > 0)
            {
                using (Image image = Image.Load(originalImage, out IImageFormat imageFormat))
                {
                    if (IsNeededResize(image, width, height))
                    {
                        image.Mutate(x => x.Resize(width, height));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            image.Save(stream, imageFormat);
                            stream.Position = 0;
                            return stream.ToArray();
                        }
                    }
                }
            }
            return originalImage;
        }

        private static bool IsNeededResize(Image image, int width, int height)
        {
            return image.Width > width && image.Height > height;
        }
    }
}
