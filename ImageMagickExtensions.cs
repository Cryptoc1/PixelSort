using ImageMagick;

namespace PixelSort;

public static class ImageMagickExtensions
{
    public static int ToHexValue( this IMagickColor<byte> color ) => ( 255 << 24 ) | ( color.R << 16 ) | ( color.G << 8 ) | ( color.B << 0 );

    public static IMagickColor<byte>? GetPixelColor( this IPixelCollection<byte> pixels, int x, int y ) => pixels.GetPixel( x, y ).ToColor();

    public static int? GetHexValue( this IPixelCollection<byte> pixels, int x, int y )
    {
        var color = GetPixelColor( pixels, x, y );
        return color?.ToHexValue();
    }
}
