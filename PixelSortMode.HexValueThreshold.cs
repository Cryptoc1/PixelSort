using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public partial class PixelSortMode
{
    public sealed class HexValueThreshold : PixelSortMode
    {
        private readonly int threshold;

        public HexValueThreshold( int value ) => threshold = value;

        public override ScanResult Scan( ImageFrame<Rgba32> image, Vector2 axis, Vector2 position )
        {
            ArgumentNullException.ThrowIfNull( image );

            int length = ( int )Vector2.Dot( new Vector2( image.Width, image.Height ), axis );

            int offset = ( int )Vector2.Dot( position, axis );
            if( offset + 1 >= length )
            {
                return new( offset, offset );
            }

            int? from = null, to = null;
            while( ( offset = ( int )Vector2.Dot( position, axis ) ) < length )
            {
                int value = image[ ( int )position.X, ( int )position.Y ].ToHexValue();
                if( !from.HasValue && value > threshold )
                {
                    from = offset;
                }

                if( !to.HasValue && value < threshold )
                {
                    to = offset + 1;
                }

                if( from.HasValue && to.HasValue )
                {
                    break;
                }

                position += axis;
            }

            return new( from ?? -1, to );
        }
    }
}