using System.Numerics;

namespace PixelSort;

public static class Vector2Math
{
    public static Vector2 Translate( this Vector2 origin, Vector2 axis, float distance )
        => origin + ( distance * axis );
}
