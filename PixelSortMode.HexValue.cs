namespace PixelSort;

public sealed class HexValueMode : PixelSortMode
{
    private readonly int threshold;

    public HexValueMode( int value ) => threshold = value;

    public override ScanResult ScanColumn( PixelSortContext context, int column, int offset )
    {
        if( offset + 1 >= context.Height )
        {
            return new( offset, offset );
        }

        int? from = null, to = null;
        for( int y = offset; y < context.Height; y++ )
        {
            if( !from.HasValue && context.Pixels.GetHexValue( column, y )! > threshold )
            {
                from = y;
            }

            if( !to.HasValue && context.Pixels.GetHexValue( column, y )! < threshold )
            {
                to = y + 1;
            }

            if( from.HasValue && to.HasValue )
            {
                break;
            }
        }

        return new( from ?? -1, to ?? context.Height );
    }

    public override ScanResult ScanRow( PixelSortContext context, int row, int offset )
    {
        if( offset + 1 >= context.Height )
        {
            return new( offset, offset );
        }

        int? from = null, to = null;
        for( int x = offset; x < context.Width; x++ )
        {
            if( !from.HasValue && context.Pixels.GetHexValue( x, row )! > threshold )
            {
                from = x;
            }

            if( !to.HasValue && context.Pixels.GetHexValue( x, row )! < threshold )
            {
                to = x + 1;
            }

            if( from.HasValue && to.HasValue )
            {
                break;
            }
        }

        return new( from ?? -1, to ?? context.Height );
    }
}