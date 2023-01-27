using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public abstract partial class PixelSortMode
{
    public static readonly PixelSortMode White = new HexValueThreshold( -12345678 );

    public abstract ScanResult ScanColumn( Image<Rgba32> image, int column, int offset );
    public abstract ScanResult ScanRow( Image<Rgba32> image, int row, int offset );
}

public readonly record struct ScanResult( int Start, int NextOffset )
{
    public readonly int Length = NextOffset - Start;
}