using ImageMagick;
using PixelSort;

using( var image = new MagickImage( Paths.Input( "source.jpg" ) ) )
{
    Console.WriteLine( "loaded, starting sort." );

    PixelSorter.Sort( image, PixelSortMode.White );

    string filepath = Paths.Output( "jpg" );
    await image.WriteAsync( filepath );

    Console.WriteLine( $"wrote image to '{filepath}'." );
}

internal static class Paths
{
    private static readonly string ProjectDirectory = Path.GetFullPath( @"..\..\..", AppDomain.CurrentDomain.BaseDirectory );

    public static string Input( string filename ) => Path.Combine( ProjectDirectory, filename );
    public static string Output( string ext = "jpg" ) => Path.Combine( ProjectDirectory!, "artifacts", $"{Path.GetFileNameWithoutExtension( Path.GetRandomFileName() )}.{ext}" );
}