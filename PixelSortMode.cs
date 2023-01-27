namespace PixelSort;

public abstract partial class PixelSortMode
{
    public static readonly PixelSortMode White = new HexValueMode( -12345678 );

    public abstract ScanResult ScanColumn( PixelSortContext context, int column, int offset );
    public abstract ScanResult ScanRow( PixelSortContext context, int row, int offset );
}

public readonly record struct ScanResult( int Start, int NextOffset )
{
    public readonly int Length = NextOffset - Start;
}