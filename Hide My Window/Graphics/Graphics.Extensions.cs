using System.Diagnostics;
using System.Drawing;

namespace theDiary.Tools.HideMyWindow
{
    internal static class GraphicsExtensions
    {
        public static Icon GetApplicationIcon(this Process process)
        {
            return Icon.ExtractAssociatedIcon(process.MainModule.FileName);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Icon icon, int? minX = null, int? minY = null)
        {
            return graphics.AddImage(bounds, icon, ImagePosition.Centered, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Image image, int? minX = null, int? minY = null)
        {
            return graphics.AddImage(bounds, image, ImagePosition.Centered, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Icon icon, ImagePosition position, int? minX = null, int? minY = null)
        {
            return graphics.AddImage(bounds, icon.ToBitmap(), position, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Image image, ImagePosition position, int? minX = null, int? minY = null)
        {
            Rectangle newBounds = bounds.GetImageBounds(image.Size, position, minX, minY);
            graphics.DrawImage(image, newBounds);

            return newBounds;
        }

        /*
        public static void AddOverlayIcon(this Graphics graphics, Image image, Rectangle bounds, ImagePosition position)
        {
            System.Drawing.Rectangle overlayBounds = bounds.GetImageBounds(image.Size, position);
            graphics.DrawImage(image, overlayBounds);
        }

        public static void AddOverlayIcon(this Graphics graphics, Icon icon, Rectangle bounds, ImagePosition position)
        {
            graphics.AddOverlayIcon(icon.ToBitmap(), bounds, position);
        }*/

        public static Brush ToBrush(this Color color)
        {
            return new SolidBrush(color);
        }

        public static Rectangle GetImageBounds(this Rectangle currentBounds, int imageWidth, int imageHeight, ImagePosition position, int? minX = null, int? minY = null)
        {
            Rectangle returnValue;
            switch (position)
            {
                case ImagePosition.BottomRight:
                    returnValue = new Rectangle((currentBounds.X + currentBounds.Width) - imageWidth, (currentBounds.Y + currentBounds.Height) - imageHeight, imageWidth, imageHeight);
                    break;

                case ImagePosition.BottomLeft:
                    returnValue = new Rectangle(currentBounds.X, (currentBounds.Y + currentBounds.Height) - imageHeight, imageWidth, imageHeight);
                    break;

                case ImagePosition.TopLeft:
                    returnValue = new Rectangle(currentBounds.X, currentBounds.Y, imageWidth, imageHeight);
                    break;

                case ImagePosition.TopRight:
                    returnValue = new Rectangle((currentBounds.X + currentBounds.Width) - imageWidth, currentBounds.Y, imageWidth, imageHeight);
                    break;

                case ImagePosition.Centered:
                default:
                    returnValue = new Rectangle(currentBounds.X + ((currentBounds.Width - imageWidth) / 2), currentBounds.Y + ((currentBounds.Height - imageHeight) / 2), imageWidth, imageHeight);
                    break;
            }
            if (minX.HasValue && returnValue.X - currentBounds.X > minX.Value)
                returnValue = new Rectangle(minX.Value, returnValue.Y, returnValue.Width, returnValue.Height);
            if (minY.HasValue && returnValue.Y - currentBounds.Y > minY.Value)
                returnValue = new Rectangle(returnValue.X, minY.Value, returnValue.Width, returnValue.Height);

            return returnValue;
        }

        public static Rectangle GetImageBounds(this Rectangle currentBounds, Size imageSize, ImagePosition position, int? minX = null, int? minY = null)
        {
            return currentBounds.GetImageBounds(imageSize.Width, imageSize.Height, position, minX, minY);
        }
    }
}