using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors;
using static System.Net.Mime.MediaTypeNames;

namespace PixelSort;

public sealed class PixelSortProcessor : IImageProcessor
{
    private readonly PixelSortMode mode;

    public PixelSortProcessor( PixelSortMode mode )
        => this.mode = mode;

    public IImageProcessor<TPixel> CreatePixelSpecificProcessor<TPixel>( Configuration configuration, Image<TPixel> source, Rectangle sourceRectangle )
        where TPixel : unmanaged, IPixel<TPixel>
    {
        if( typeof( TPixel ) != typeof( Rgba32 ) )
        {
            throw new InvalidImageContentException( $"{nameof( PixelSortProcessor )} does not support pixel type '{typeof( TPixel ).Name}'." );
        }

        var processor = new PixelSorter(
            mode,
            configuration,

            // NOTE: force cast
            Unsafe.As<Image<Rgba32>>( source ),
            sourceRectangle
        );

        // NOTE: force cast
        return Unsafe.As<IImageProcessor<TPixel>>( processor );
    }

    private sealed class PixelSorter : ImageProcessor<Rgba32>
    {
        private readonly PixelSortMode mode;

        public PixelSorter( PixelSortMode mode, Configuration configuration, Image<Rgba32> source, Rectangle sourceRectangle )
            : base( configuration, source, sourceRectangle )
            => this.mode = mode;

        protected override void OnFrameApply( ImageFrame<Rgba32> source )
        {
            Sort( source, mode, Vector2.UnitX );
            Sort( source, mode, Vector2.UnitY );
        }

        private static void Sort( ImageFrame<Rgba32> image, PixelSortMode mode, Vector2 axis )
        {
            var inverseAxis = new Vector2( axis.Y, axis.X );
            var size = new Vector2( image.Width, image.Height );

            int length = ( int )Vector2.Dot( size, axis );
            int inverseLength = ( int )Vector2.Dot( size, inverseAxis );

            var position = Vector2.Zero;
            while( Vector2.Dot( position, axis ) < length )
            {
                while( Vector2.Dot( position, inverseAxis ) < inverseLength )
                {
                    var result = mode.Scan( image, inverseAxis, position );
                    if( result.Start < 0 )
                    {
                        break;
                    }

                    if( result.Length > 0 )
                    {
                        var origin = ( position * axis ) + ( result.Start * inverseAxis );
                        SortPixels( image, origin, inverseAxis, result.Length );
                    }

                    position = ( position * axis ) + ( ( result.NextOffset ?? inverseLength ) * inverseAxis );
                }

                position = ( position + axis ) * axis;
            }
        }

        private static void SortPixels( ImageFrame<Rgba32> image, Vector2 origin, Vector2 axis, int count )
        {
            int x, y;
            var pixels = new Rgba32[ count ];

            for( int offset = 0; offset < pixels.Length; offset++ )
            {
                (x, y) = Translate( origin, axis, offset );
                pixels[ offset ] = image[ x, y ];
            }

            Array.Sort( pixels, Rgba32Comparer.Hex );
            for( int offset = 0; offset < pixels.Length; offset++ )
            {
                (x, y) = Translate( origin, axis, offset );
                image[ x, y ] = pixels[ offset ];
            }

            static (int x, int y) Translate( Vector2 origin, Vector2 axis, int distance )
            {
                var c = origin.Translate( axis, distance );
                return (( int )c.X, ( int )c.Y);
            }
        }
    }
}
