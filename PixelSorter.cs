using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelSort;

public static class PixelSorter
{
    public static void Sort( Image<Rgba32> image, PixelSortMode mode )
    {
        ArgumentNullException.ThrowIfNull( image );

        SortColumns( image, mode );
        SortRows( image, mode );
    }

    public static void SortColumns( Image<Rgba32> image, PixelSortMode mode )
    {
        ArgumentNullException.ThrowIfNull( image );

        for( int column = 0; column < image.Width - 1; column++ )
        {
            int offset = 0;
            while( offset < image.Height - 1 )
            {
                var result = mode.ScanColumn( image, column, offset );
                if( result.Start < 0 )
                {
                    break;
                }

                offset = result.NextOffset;
                if( result.Length > 0 )
                {
                    SortPixels( image, new( column, result.Start ), Vector2.UnitY, result.Length );
                }
            }
        }
    }

    private static void SortPixels( Image<Rgba32> image, Vector2 origin, Vector2 unit, int count )
    {
        int x, y;
        var pixels = new Rgba32[ count ];

        for( int i = 0; i < pixels.Length; i++ )
        {
            (x, y) = Coord( origin, unit, i );
            pixels[ i ] = image[ x, y ];
        }

        Array.Sort( pixels, Rgba32PixelComparer.HexValue );
        for( int i = 0; i < pixels.Length; i++ )
        {
            (x, y) = Coord( origin, unit, i );
            image[ x, y ] = pixels[ i ];
        }

        static (int x, int y) Coord( Vector2 origin, Vector2 unit, int scale )
        {
            var c = origin + ( unit * scale );
            return (( int )c.X, ( int )c.Y);
        }
    }

    public static void SortRows( Image<Rgba32> image, PixelSortMode mode )
    {
        ArgumentNullException.ThrowIfNull( image );

        for( int row = 0; row < image.Height - 1; row++ )
        {
            int offset = 0;
            while( offset < image.Width - 1 )
            {
                var result = mode.ScanRow( image, row, offset );
                if( result.Start < 0 )
                {
                    break;
                }

                offset = result.NextOffset;
                if( result.Length > 0 )
                {
                    SortPixels( image, new( result.Start, row ), Vector2.UnitX, result.Length );
                }
            }
        }
    }
}