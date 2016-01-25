namespace theDiary.Tools.HideMyWindow
{
    using System.ComponentModel;

    public enum ImageOverlayPosition
    {
        [Description("The image will overlay on the Center of the source.")] Center,

        [Description("The image will overlay on the Botom Right of the source.")] BottomRight,

        [Description("The image will overlay on the Bottom Left of the source.")] BottomLeft,

        [Description("The image will overlay on the Top Right of the source.")] TopRight,

        [Description("The image will overlay on the Top Left of the source.")] TopLeft
    }
}