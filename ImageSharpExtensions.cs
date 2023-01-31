using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PixelSort;

public static class ImageProcessingExtensions
{
    public static IImageProcessingContext Sort( this IImageProcessingContext context, PixelSortMode mode )
    {
        ArgumentNullException.ThrowIfNull( context );
        return context.ApplyProcessor( new PixelSortProcessor(mode) );
    }
}

public static class Rgba32Extensions
{
    public static int ToHexValue( this Rgba32 pixel ) => ( pixel.A << 24 ) | ( pixel.R << 16 ) | ( pixel.G << 8 ) | ( pixel.B << 0 );
}

public sealed class Rgba32Comparer : Comparer<Rgba32>
{
    public static readonly Rgba32Comparer Hex = new( ( x, y ) => x.ToHexValue() - y.ToHexValue() );

    private readonly Func<Rgba32, Rgba32, int> compare;

    private Rgba32Comparer( Func<Rgba32, Rgba32, int> compare )
        => this.compare = compare;

    public override int Compare( Rgba32 x, Rgba32 y ) => compare( x, y );
}