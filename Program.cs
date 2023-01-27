using PixelSort;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using( var image = await Image.LoadAsync<Rgba32>( Paths.Input( "source.jpg" ) ) )
{
    Console.WriteLine( "loaded image, sorting..." );

    PixelSorter.Sort( image, PixelSortMode.White );

    string filepath = Paths.Output();
    await image.SaveAsync( filepath );

    Console.WriteLine( $"wrote image to '{filepath}'." );
}

internal static class Paths
{
    private static readonly string ProjectDirectory = Path.GetFullPath( @"..\..\..", AppDomain.CurrentDomain.BaseDirectory );

    public static string Input( string filename ) => Path.Combine( ProjectDirectory, filename );
    public static string Output( string ext = "jpg" ) => Path.Combine( ProjectDirectory!, "artifacts", $"{Path.GetFileNameWithoutExtension( Path.GetRandomFileName() )}.{ext}" );
}