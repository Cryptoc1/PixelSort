using ImageMagick;

namespace PixelSort;

public static class ImageMagickExtensions
{
    public static int ToHexValue( this IMagickColor<byte> color ) => ( color.A << 24 ) | ( color.R << 16 ) | ( color.G << 8 ) | ( color.B << 0 );

    public static IMagickColor<byte>? GetPixelColor( this IPixelCollection<byte> pixels, int x, int y ) => pixels.GetPixel( x, y ).ToColor();

    public static int? GetHexValue( this IPixelCollection<byte> pixels, int x, int y )
    {
        var color = GetPixelColor( pixels, x, y );
        return color?.ToHexValue();
    }
}

public sealed class ColorComparer : Comparer<IMagickColor<byte>>
{
    public static readonly ColorComparer HexValue = new( ByHexValue );

    private readonly Func<IMagickColor<byte>?, IMagickColor<byte>?, int> compare;

    private ColorComparer( Func<IMagickColor<byte>?, IMagickColor<byte>?, int> compare )
        => this.compare = compare;

    private static int ByHexValue( IMagickColor<byte>? x, IMagickColor<byte>? y )
    {
        int a = x?.ToHexValue() ?? 0;
        int b = y?.ToHexValue() ?? 0;

        return a - b;
    }

    public override int Compare( IMagickColor<byte>? x, IMagickColor<byte>? y ) => compare( x, y );
}

