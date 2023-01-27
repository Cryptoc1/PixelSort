namespace PixelSort;

public abstract partial class PixelSortMode
{
    public abstract ScanResult ScanColumn( PixelSortContext context, int column, int offset );
    public abstract ScanResult ScanRow( PixelSortContext context, int row, int offset );
}

public readonly record struct ScanResult( int Start, int NextOffset )
{
    //public static readonly ScanResult Empty = new( 0, 0 );

    public readonly int Length = NextOffset - Start;
}