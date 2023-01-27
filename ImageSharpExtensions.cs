using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public static class PixelExtensions
{
    public static int GetHexValue( this Image<Rgba32> image, int x, int y ) => ToHexValue( image[ x, y ] );

    public static int ToHexValue( this Rgba32 pixel ) => ( pixel.A << 24 ) | ( pixel.R << 16 ) | ( pixel.G << 8 ) | ( pixel.B << 0 );
}

public sealed class Rgba32PixelComparer : Comparer<Rgba32>
{
    public static readonly Rgba32PixelComparer HexValue = new( ( x, y ) => x.ToHexValue() - y.ToHexValue() );

    private readonly Func<Rgba32, Rgba32, int> compare;

    private Rgba32PixelComparer( Func<Rgba32, Rgba32, int> compare )
        => this.compare = compare;

    public override int Compare( Rgba32 x, Rgba32 y ) => compare( x, y );
}