using System.Numerics;
using ImageMagick;

namespace PixelSort;

public static class PixelSorter
{
    public static void Sort( MagickImage image, PixelSortMode mode )
    {
        ArgumentNullException.ThrowIfNull( image );

        using var context = PixelSortContext.Create( image );

        SortColumns( context, mode );
        SortRows( context, mode );
    }

    private static void SortColumns( PixelSortContext context, PixelSortMode mode )
    {
        for( int column = 0; column < context.Width - 1; column++ )
        {
            int offset = 0;
            while( offset < context.Height - 1 )
            {
                var result = mode.ScanColumn( context, column, offset );
                if( result.Start < 0 )
                {
                    break;
                }

                offset = result.NextOffset;
                if( result.Length > 0 )
                {
                    SortPixels( context, new( column, result.Start ), Vector2.UnitY, result.Length );
                }
            }
        }
    }

    private static void SortPixels( PixelSortContext context, Vector2 origin, Vector2 unit, int count )
    {
        int x, y;
        var colors = new IMagickColor<byte>[ count ];

        for( int i = 0; i < colors.Length; i++ )
        {
            (x, y) = Coord( origin, unit, i );
            colors[ i ] = context.Pixels.GetPixelColor( x, y )!;
        }

        Array.Sort( colors );
        for( int i = 0; i < colors.Length; i++ )
        {
            (x, y) = Coord( origin, unit, i );
            context.Pixels.SetPixel( x, y, colors[ i ].ToByteArray() );
        }

        static (int x, int y) Coord( Vector2 origin, Vector2 unit, int scale )
        {
            var c = origin + ( unit * scale );
            return (( int )c.X, ( int )c.Y);
        }
    }

    private static void SortRows( PixelSortContext context, PixelSortMode mode )
    {
        for( int row = 0; row < context.Height - 1; row++ )
        {
            int offset = 0;
            while( offset < context.Width - 1 )
            {
                var result = mode.ScanRow( context, row, offset );
                if( result.Start < 0 )
                {
                    break;
                }

                offset = result.NextOffset;
                if( result.Length > 0 )
                {
                    SortPixels( context, new( result.Start, row ), Vector2.UnitX, result.Length );
                }
            }
        }
    }
}