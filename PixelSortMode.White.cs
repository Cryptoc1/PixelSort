namespace PixelSort;

public partial class PixelSortMode
{
    public sealed class White : PixelSortMode
    {
        public const int THRESHOLD = -12345678;

        public override ScanResult ScanColumn( PixelSortContext context, int column, int offset )
        {
            if( offset + 1 >= context.Height )
            {
                return new( offset, offset );
            }

            int? from = null, to = null;
            for( int y = offset; y < context.Height; y++ )
            {
                if( !from.HasValue && context.Pixels.GetHexValue( column, y )! > THRESHOLD )
                {
                    from = y;
                }

                if( !to.HasValue && context.Pixels.GetHexValue( column, y )! < THRESHOLD )
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
                if( !from.HasValue && context.Pixels.GetHexValue( x, row )! > THRESHOLD )
                {
                    from = x;
                }

                if( !to.HasValue && context.Pixels.GetHexValue( x, row )! < THRESHOLD )
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
}