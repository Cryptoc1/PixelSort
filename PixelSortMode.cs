using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public abstract partial class PixelSortMode
{
    public static readonly PixelSortMode White = new HexValueThreshold( -12345678 );

    public abstract ScanResult Scan( ImageFrame<Rgba32> image, Vector2 axis, Vector2 position );
}

public readonly record struct ScanResult( int Start, int? NextOffset )
{
    public readonly int Length = ( NextOffset ?? 0 ) - Start;
}