namespace PixelSort;

public partial class PixelSortMode
{
    public sealed class Black : PixelSortMode
    {
        public override ScanResult ScanColumn( PixelSortContext context, int column, int offset ) => throw new NotImplementedException();
        public override ScanResult ScanRow( PixelSortContext context, int row, int offset ) => throw new NotImplementedException();
    }
}
