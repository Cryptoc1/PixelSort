using ImageMagick;

namespace PixelSort;

public readonly ref struct PixelSortContext
{
    public readonly int Height { get; init; }
    public readonly IPixelCollection<byte> Pixels { get; init; }
    public readonly int Width { get; init; }

    public static PixelSortContext Create( MagickImage image )
        => new() { Height = image.Height, Pixels = image.GetPixels(), Width = image.Width };

    // NOTE: ref struct cannot implement interfaces (IDisposable), but can still implement the "disposable pattern" via compiler
    public void Dispose( ) => Pixels?.Dispose();
}