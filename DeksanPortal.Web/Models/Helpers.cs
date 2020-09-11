using DeksanPortal.Data.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeksanPortal.Web.Models
{
    public static class Helpers
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var claims = principal.FindFirst("UserId");
            return claims?.Value;
        }
        public static string GetName(this ClaimsPrincipal principal)
        {
            var claims = principal.FindFirst("Name");
            return claims?.Value;
        }
        public static string GetClass(this ClaimsPrincipal principal)
        {
            var claims = principal.FindFirst("Class");
            return claims?.Value;
        }
        public static Education GetEducation(this ClaimsPrincipal principal)
        {
            var claims = principal.FindFirst("Education");
            return claims != null ? Enum.Parse<Education>(claims.Value) : default;
        }
        public static async Task<string> SaveImage(string ImageUrl, Stream imageStream, string rootPath)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                throw new Exception("There is no image.");
            }

            if (imageStream.Length > 5120000)
            {
                throw new Exception("Photo cannot be bigger than 5120000 bytes.");
            }

            string[] photos;

            try
            {
                photos = SetThumbnails(imageStream, rootPath);
            }
            catch (ArgumentException)
            {
                throw new Exception("Invalid file format.");
            }

            if (photos != null)
            {
                bool userHasThumbnail = ImageUrl != null;
                var oldPhoto = ImageUrl;
                var oldThumbnail = ImageUrl;
                if (userHasThumbnail)
                {
                    DeletePhotoIfExist(oldPhoto);
                    DeletePhotoIfExist(oldThumbnail);
                }
            }
            ImageUrl = photos[0];
            return ImageUrl;
        }
        public static string[] SetThumbnails(Stream imageStream, string rootPath)
        {
            var originalImage = Image.FromStream(imageStream);

            if (!ValidateImageFormat(originalImage, out string ext))
            {
                throw new ArgumentException();
            }

            SetCorrectOrientation(originalImage);

            var photoId = Guid.NewGuid().ToString();

            //var mobileThumbnail = $"Content/Photos/Users/Thumbnails/500/{photoId}.jpg";
            var webThumbnail = $"/assets/aimages/books/{photoId}.{ext}";
            //CreateAndSaveThumbnail(originalImage, 500, mobileThumbnail);
            CreateAndSaveThumbnail(originalImage, 100, rootPath + webThumbnail);
            originalImage.Dispose();

            return new[] { webThumbnail };
            //return new[] { webThumbnail, mobileThumbnail };
        }
        private static bool ValidateImageFormat(Image image, out string ext)
        {
            if (ImageFormat.Jpeg.Equals(image.RawFormat))
            {
                ext = "jpeg";
                return true;
            }
            if (ImageFormat.Png.Equals(image.RawFormat))
            {
                ext = "png";
                return true;
            }
            if (ImageFormat.Gif.Equals(image.RawFormat))
            {
                ext = "gif";
                return true;
            }
            if (ImageFormat.Bmp.Equals(image.RawFormat))
            {
                ext = "bmp";
                return true;
            }

            ext = default;
            return false;
        }
        private static void SetCorrectOrientation(Image image)
        {
            //property id = 274 describe EXIF orientation parameter
            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {
                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                // This EXIF data is now invalid and should be removed.
                image.RemovePropertyItem(274);
            }
        }
        private static void CreateAndSaveThumbnail(Image image, int size, string thumbnailPath)
        {
            var thumbnailSize = GetThumbnailSize(image, size);

            using var bitmap = ResizeImage(image, thumbnailSize.Width, thumbnailSize.Height);

            bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
        }
        private static Size GetThumbnailSize(Image original, int size = 500)
        {
            var originalWidth = original.Width;
            var originalHeight = original.Height;

            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)size / originalWidth;
            }
            else
            {
                factor = (double)size / originalHeight;
            }

            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var result = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }
        public static void DeletePhotoIfExist(string photoPath)
        {
            if (photoPath == null)
                throw new ArgumentNullException(nameof(photoPath));

            if (File.Exists(photoPath))
            {
                File.Delete(photoPath);
            }
        }
    }
}
