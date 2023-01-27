using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public partial class PixelSortMode
{
    public sealed class HexValueThreshold : PixelSortMode
    {
        private readonly int threshold;

        public HexValueThreshold( int value ) => threshold = value;

        public override ScanResult ScanColumn( Image<Rgba32> image, int column, int offset )
        {
            if( offset + 1 >= image.Height )
            {
                return new( offset, offset );
            }

            int? from = null, to = null;
            for( int y = offset; y < image.Height; y++ )
            {
                int value = image.GetHexValue( column, y );
                if( !from.HasValue && value > threshold )
                {
                    from = y;
                }

                if( !to.HasValue && value < threshold )
                {
                    to = y + 1;
                }

                if( from.HasValue && to.HasValue )
                {
                    break;
                }
            }

            return new( from ?? -1, to ?? image.Height );
        }

        public override ScanResult ScanRow( Image<Rgba32> image, int row, int offset )
        {
            if( offset + 1 >= image.Width )
            {
                return new( offset, offset );
            }

            int? from = null, to = null;
            for( int x = offset; x < image.Width; x++ )
            {
                int value = image.GetHexValue( x, row );
                if( !from.HasValue && value > threshold )
                {
                    from = x;
                }

                if( !to.HasValue && value < threshold )
                {
                    to = x + 1;
                }

                if( from.HasValue && to.HasValue )
                {
                    break;
                }
            }

            return new( from ?? -1, to ?? image.Width );
        }
    }
}