using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    internal static class GraphicsExtensions
    {
        #region Methods & Functions
        public static Icon GetApplicationIcon(this Process process)
        {
            return Icon.ExtractAssociatedIcon(process.MainModule.FileName);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Icon overlayIcon, int? minX = null,
                                         int? minY = null)
        {
            return graphics.AddImage(bounds, overlayIcon, ImageOverlayPosition.Center, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Image overlayImage, int? minX = null,
                                         int? minY = null)
        {
            return graphics.AddImage(bounds, overlayImage, ImageOverlayPosition.Center, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Icon overlayIcon,
                                         ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            return graphics.AddImage(bounds, overlayIcon.ToBitmap(), overlayPosition, minX, minY);
        }

        public static Rectangle AddImage(this Graphics graphics, Rectangle bounds, Image overlayImage,
                                         ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            Rectangle newBounds = bounds.GetImageBounds(overlayImage.Size, overlayPosition, minX, minY);
            graphics.DrawImage(overlayImage, newBounds);

            return newBounds;
        }

        public static Icon AddOverlay(this Icon icon, Image overlayIcon, ImageOverlayPosition overlayPosition,
                                      double downSizeModifier = 0.75)
        {
            return icon.AddOverlay(overlayIcon, overlayPosition, default(Point), downSizeModifier);
        }

        public static Icon AddOverlay(this Icon icon, Image overlayIcon, ImageOverlayPosition overlayPosition,
                                      Size offset = new Size(), double downSizeModifier = 0.75)
        {
            return icon.AddOverlay(overlayIcon, overlayPosition, new Point(offset), downSizeModifier);
        }

        public static Icon AddOverlay(this Icon icon, Image overlayIcon, ImageOverlayPosition overlayPosition,
                                      Point offset = new Point(), double downSizeModifier = 0.75)
        {
            Bitmap n = new Bitmap(overlayIcon, icon.Size.DownSize(downSizeModifier));
            Bitmap iconBitmap = icon.ToBitmap();
            using (Graphics g = Graphics.FromImage(iconBitmap))
            {
                Rectangle location = Rectangle.Round(g.VisibleClipBounds.GetImageBounds(n.Size, overlayPosition));
                location.Offset(offset);
                g.AddOverlay(n, location, overlayPosition);

                //g.Flush();
            }
            return Icon.FromHandle(iconBitmap.GetHicon());
        }

        public static Icon AddOverlay(this Icon icon, Icon overlayIcon, ImageOverlayPosition overlayPosition,
                                      double downSizeModifier = 0.75)
        {
            return icon.AddOverlay(overlayIcon, overlayPosition, default(Point), downSizeModifier);
        }

        public static Icon AddOverlay(this Icon icon, Icon overlayIcon, ImageOverlayPosition overlayPosition,
                                      Point offset = new Point(), double downSizeModifier = 0.75)
        {
            Bitmap n = new Bitmap(overlayIcon.ToBitmap(), icon.Size.DownSize(downSizeModifier));
            Bitmap iconBitmap = icon.ToBitmap();
            using (Graphics g = Graphics.FromImage(iconBitmap))
            {
                Rectangle location = Rectangle.Round(g.VisibleClipBounds.GetImageBounds(n.Size, overlayPosition));
                offset.X *= -1;
                offset.Y *= -1;
                location.Offset(offset);
                g.AddOverlay(n, location, overlayPosition);

                //g.Flush();
            }
            return Icon.FromHandle(iconBitmap.GetHicon());
        }

        private static Size DownSize(this Size size, double modifier)
        {
            double width = size.Width;
            double height = size.Height;
            if (modifier > 1)
                return new Size((int) (width / modifier), (int) (height / modifier));
            return new Size((int) (width * modifier), (int) (height * modifier));
        }

        public static void AddOverlay(this Graphics graphics, Image image, Rectangle bounds,
                                      ImageOverlayPosition overlayPosition)
        {
            Rectangle overlayBounds = bounds.GetImageBounds(image.Size, overlayPosition);
            graphics.DrawImage(image, overlayBounds);
        }

        public static void AddOverlay(this Graphics graphics, Icon icon, Rectangle bounds,
                                      ImageOverlayPosition overlayPosition)
        {
            graphics.AddOverlay(icon.ToBitmap(), bounds, overlayPosition);
        }

        public static Brush ToBrush(this Color color)
        {
            return new SolidBrush(color);
        }

        public static Rectangle GetImageBounds(this Rectangle currentBounds, int imageWidth, int imageHeight,
                                               ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            Rectangle returnValue;
            switch (overlayPosition)
            {
                case ImageOverlayPosition.BottomRight:
                    returnValue = new Rectangle((currentBounds.X + currentBounds.Width) - imageWidth,
                        (currentBounds.Y + currentBounds.Height) - imageHeight, imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.BottomLeft:
                    returnValue = new Rectangle(currentBounds.X, (currentBounds.Y + currentBounds.Height) - imageHeight,
                        imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.TopLeft:
                    returnValue = new Rectangle(currentBounds.X, currentBounds.Y, imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.TopRight:
                    returnValue = new Rectangle((currentBounds.X + currentBounds.Width) - imageWidth, currentBounds.Y,
                        imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.Center:
                default:
                    returnValue = new Rectangle(currentBounds.X + ((currentBounds.Width - imageWidth) / 2),
                        currentBounds.Y + ((currentBounds.Height - imageHeight) / 2), imageWidth, imageHeight);
                    break;
            }
            if (minX.HasValue
                && returnValue.X - currentBounds.X > minX.Value)
                returnValue = new Rectangle(minX.Value, returnValue.Y, returnValue.Width, returnValue.Height);
            if (minY.HasValue
                && returnValue.Y - currentBounds.Y > minY.Value)
                returnValue = new Rectangle(returnValue.X, minY.Value, returnValue.Width, returnValue.Height);

            return returnValue;
        }

        public static RectangleF GetImageBounds(this RectangleF currentBounds, int imageWidth, int imageHeight,
                                                ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            RectangleF returnValue;
            switch (overlayPosition)
            {
                case ImageOverlayPosition.BottomRight:
                    returnValue = new RectangleF((currentBounds.X + currentBounds.Width) - imageWidth,
                        (currentBounds.Y + currentBounds.Height) - imageHeight, imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.BottomLeft:
                    returnValue = new RectangleF(currentBounds.X, (currentBounds.Y + currentBounds.Height) - imageHeight,
                        imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.TopLeft:
                    returnValue = new RectangleF(currentBounds.X, currentBounds.Y, imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.TopRight:
                    returnValue = new RectangleF((currentBounds.X + currentBounds.Width) - imageWidth, currentBounds.Y,
                        imageWidth, imageHeight);
                    break;

                case ImageOverlayPosition.Center:
                default:
                    returnValue = new RectangleF(currentBounds.X + ((currentBounds.Width - imageWidth) / 2),
                        currentBounds.Y + ((currentBounds.Height - imageHeight) / 2), imageWidth, imageHeight);
                    break;
            }
            if (minX.HasValue
                && returnValue.X - currentBounds.X > minX.Value)
                returnValue = new RectangleF(minX.Value, returnValue.Y, returnValue.Width, returnValue.Height);
            if (minY.HasValue
                && returnValue.Y - currentBounds.Y > minY.Value)
                returnValue = new RectangleF(returnValue.X, minY.Value, returnValue.Width, returnValue.Height);

            return returnValue;
        }

        public static Rectangle GetImageBounds(this Rectangle currentBounds, Size imageSize,
                                               ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            return currentBounds.GetImageBounds(imageSize.Width, imageSize.Height, overlayPosition, minX, minY);
        }

        public static RectangleF GetImageBounds(this RectangleF currentBounds, Size imageSize,
                                                ImageOverlayPosition overlayPosition, int? minX = null, int? minY = null)
        {
            return currentBounds.GetImageBounds(imageSize.Width, imageSize.Height, overlayPosition, minX, minY);
        }
        #endregion
    }
}